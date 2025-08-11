using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Routing")]
    [Tooltip("Mixer principal avec un paramètre exposé (ex: MusicVolume)")]
    public AudioMixer mixer;
    [Tooltip("Nom du paramètre exposé pour le volume musique (en dB)")]
    public string musicVolumeParam = "MusicVolume";

    [Header("Lecture")]
    public float defaultFade = 1.5f;
    public bool persistAcrossScenes = true;

    [Header("Musique par scène (optionnel)")]
    public List<SceneTrack> sceneTracks = new(); // associer scènes -> musiques
    // Connaître le clip en cours
    public AudioClip CurrentClip => _active != null ? _active.clip : null;


    // --- Runtime
    private AudioSource _a;
    private AudioSource _b;
    private AudioSource _active;
    private Coroutine _fadeCo;
    private Coroutine _playlistCo;
    private Coroutine _customLoopCo;

    [System.Serializable]
    public class SceneTrack
    {
        public string sceneName;
        public AudioClip clip;
        public bool loop = true;
        public float fade = 1.5f;
        public float replayOnLoop = 0f;
    }

    void Awake()
    {
        // Singleton basique
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        // Deux AudioSource pour crossfader sans coupure
        _a = gameObject.AddComponent<AudioSource>();
        _b = gameObject.AddComponent<AudioSource>();
        _a.playOnAwake = _b.playOnAwake = false;
        _a.loop = _b.loop = true;
        _active = _a;

        if (persistAcrossScenes) DontDestroyOnLoad(gameObject);

        // Option: écouter le changement de scène pour lancer la musique dédiée
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    void OnDestroy()
    {
        if (Instance == this) SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    private void OnActiveSceneChanged(Scene prev, Scene next)
    {
        var found = sceneTracks.Find(s => s.sceneName == next.name && s.clip != null);
        if (found != null)
        {
            CrossfadeTo(found.clip, found.fade <= 0 ? defaultFade : found.fade, found.loop);
        }
    }

    // ----- API PUBLIQUE -----

    /// <summary>Lecture immédiate avec loop start optionnel</summary>
    public void Play(AudioClip clip, bool loop = true, float fade = -1f, float replayOnLoopStart = -1f)
        => CrossfadeTo(clip, fade < 0 ? defaultFade : fade, loop, 0f, replayOnLoopStart);

    /// <summary>Crossfade fluide vers un nouveau clip, avec loop start optionnel.</summary>
    public void CrossfadeTo(AudioClip next, float fadeDuration, bool loop = true, float startTime = 0f, float replayOnLoopStart = -1f)
    {
        if (next == null) return;
        if (_fadeCo != null) StopCoroutine(_fadeCo);
        if (_customLoopCo != null) { StopCoroutine(_customLoopCo); _customLoopCo = null; } // <— NEW

        var from = _active;
        var to = (_active == _a) ? _b : _a;
        _active = to;

        to.clip = next;
        to.time = Mathf.Clamp(startTime, 0, next.length - 0.01f);
        to.loop = (replayOnLoopStart < 0f) && loop; // <— NEW: loop natif seulement si pas de loop perso
        to.volume = 0f;
        to.Play();

        // NEW: si loop perso demandé, on le maintient avec une coroutine
        if (replayOnLoopStart >= 0f)
        {
            float clamped = Mathf.Clamp(replayOnLoopStart, 0f, next.length - 0.01f);
            _customLoopCo = StartCoroutine(CoMaintainCustomLoop(to, clamped));
        }

        _fadeCo = StartCoroutine(CoCrossfade(from, to, Mathf.Max(0.01f, fadeDuration)));
    }

    /// <summary>Playlist séquentielle; shuffle optionnel; bouclage facultatif</summary>
    public void PlayPlaylist(IList<AudioClip> clips, bool shuffle = false, bool loop = true, float fade = -1f)
    {
        if (clips == null || clips.Count == 0) return;
        if (_playlistCo != null) StopCoroutine(_playlistCo);
        _playlistCo = StartCoroutine(CoPlaylist(clips, shuffle, loop, fade < 0 ? defaultFade : fade));
    }

    public void Stop(float fade = 0.5f)
    {
        if (_fadeCo != null) StopCoroutine(_fadeCo);
        if (_customLoopCo != null) { StopCoroutine(_customLoopCo); _customLoopCo = null; } // <— NEW
        StartCoroutine(CoStop(fade));
    }

    public void Pause() { _a.Pause(); _b.Pause(); }
    public void Resume() { _a.UnPause(); _b.UnPause(); }

    /// <summary>Volume musique en linéaire [0..1] -> dB sur le Mixer</summary>
    public void SetMusicVolume(float linear01)
    {
        if (mixer == null || string.IsNullOrEmpty(musicVolumeParam)) return;
        // Convertit linéaire -> décibels (0 => -80 dB ≈ silence)
        float dB = (linear01 <= 0.0001f) ? -80f : 20f * Mathf.Log10(Mathf.Clamp01(linear01));
        mixer.SetFloat(musicVolumeParam, dB);
    }

    // ----- Coroutines internes -----

    private IEnumerator CoCrossfade(AudioSource from, AudioSource to, float dur)
    {
        float t = 0f;
        float fromStart = (from.isPlaying) ? from.volume : 0f;
        to.volume = 0f;

        while (t < dur)
        {
            t += Time.unscaledDeltaTime; // indépendant du Time.timeScale
            float k = t / dur;
            if (from.isPlaying) from.volume = Mathf.Lerp(fromStart, 0f, k);
            to.volume = Mathf.Lerp(0f, 1f, k);
            yield return null;
        }

        if (from.isPlaying) { from.Stop(); from.volume = 1f; }
        to.volume = 1f;
        _fadeCo = null;
    }

    private IEnumerator CoStop(float fade)
    {
        float t = 0f;
        float vA = _a.volume, vB = _b.volume;
        while (t < fade)
        {
            t += Time.unscaledDeltaTime;
            float k = t / fade;
            _a.volume = Mathf.Lerp(vA, 0f, k);
            _b.volume = Mathf.Lerp(vB, 0f, k);
            yield return null;
        }
        _a.Stop(); _b.Stop();
        _a.volume = _b.volume = 1f;
        _fadeCo = null;
    }

    private IEnumerator CoPlaylist(IList<AudioClip> clips, bool shuffle, bool loop, float fade)
    {
        var list = new List<AudioClip>(clips);
        int index = 0;

        while (true)
        {
            if (shuffle) Shuffle(list);
            for (index = 0; index < list.Count; index++)
            {
                var clip = list[index];
                if (clip == null) continue;

                CrossfadeTo(clip, fade, loop && (index == list.Count - 1 && !shuffle)); // la boucle continue gère
                // Attendre la fin du clip si pas en loop
                if (_active.loop) yield break;

                // attendre la fin en temps réel (insensible au timeScale)
                float remaining = clip.length - _active.time;
                while (remaining > 0f && _active.isPlaying)
                {
                    remaining -= Time.unscaledDeltaTime;
                    yield return null;
                }
            }
            if (!loop) break;
        }
        _playlistCo = null;
    }
    // NEW: maintient un « loop start » personnalisé (seek au moment voulu à chaque fin)
    private IEnumerator CoMaintainCustomLoop(AudioSource src, float loopStart)
    {
        // petite marge pour déclencher le seek juste avant la fin
        const float epsilon = 0.02f;

        var clip = src.clip;
        if (clip == null) yield break;

        // on boucle tant que ce src reste le _active et que le même clip joue
        while (src == _active && src.clip == clip)
        {
            if (src.isPlaying)
            {
                float remaining = clip.length - src.time;
                if (remaining <= epsilon)
                {
                    // Seek « soft » : on remet la tête de lecture au loopStart sans Stop/Play pour éviter un clic
                    src.time = loopStart;
                }
            }
            // unscaledDeltaTime pour être insensible au timeScale
            yield return null;
        }

        // sécurité
        if (_customLoopCo != null && src != _active) _customLoopCo = null;
    }
    private void Shuffle(List<AudioClip> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = Random.Range(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}

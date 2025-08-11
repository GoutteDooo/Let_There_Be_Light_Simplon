using UnityEngine;
using System.Collections.Generic;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
    }

    public List<Sound> sounds = new List<Sound>();

    private AudioSource oneShotSource; // pour PlaySFX classique
    private Dictionary<string, AudioSource> loopSources = new Dictionary<string, AudioSource>();

    void Awake()
    {
        // Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        oneShotSource = gameObject.AddComponent<AudioSource>();
    }

    /// <summary>
    /// Joue un SFX une seule fois
    /// </summary>
    public void PlaySFX(string name)
    {
        Sound s = sounds.Find(sound => sound.name == name);
        if (s != null && s.clip != null)
        {
            oneShotSource.PlayOneShot(s.clip, s.volume);
        }
        else
        {
            Debug.LogWarning($"SFX '{name}' non trouvé !");
        }
    }

    /// <summary>
    /// Joue un SFX en boucle jusqu'à arrêt manuel
    /// </summary>
    public void PlayLoopSFX(string name)
    {
        if (loopSources.ContainsKey(name))
            return; // déjà en cours de lecture

        Sound s = sounds.Find(sound => sound.name == name);
        if (s != null && s.clip != null)
        {
            AudioSource src = gameObject.AddComponent<AudioSource>();
            src.clip = s.clip;
            src.volume = s.volume;
            src.loop = true;
            src.Play();
            loopSources[name] = src;
        }
        else
        {
            Debug.LogWarning($"SFX '{name}' non trouvé !");
        }
    }

    /// <summary>
    /// Stoppe un SFX en boucle par son nom
    /// </summary>
    public void StopSFX(string name)
    {
        if (loopSources.TryGetValue(name, out AudioSource src))
        {
            src.Stop();
            Destroy(src); // détruit la source dédiée
            loopSources.Remove(name);
        }
    }

    /// <summary>
    /// Stoppe tous les SFX joués en boucle
    /// </summary>
    public void StopAllLoopSFX()
    {
        foreach (var kvp in loopSources)
        {
            if (kvp.Value != null)
            {
                kvp.Value.Stop();
                Destroy(kvp.Value);
            }
        }
        loopSources.Clear();
    }
}

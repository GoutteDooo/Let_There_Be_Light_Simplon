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
    private AudioSource audioSource;

    void Awake()
    {
        // Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySFX(string name)
    {
        Sound s = sounds.Find(sound => sound.name == name);
        if (s != null && s.clip != null)
        {
            audioSource.PlayOneShot(s.clip, s.volume);
        }
        else
        {
            Debug.LogWarning($"SFX '{name}' non trouvé !");
        }
    }
}

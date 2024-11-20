using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance; // Singleton instance

    [System.Serializable]
    public class Sound
    {
        public string name;         // Identifier for the sound
        public AudioClip clip;      // Audio clip
        [Range(0f, 1f)] public float volume = 1f; // Volume control
        public bool loop;           // Loop option
    }

    public List<Sound> sounds;       // List of sound effects
    private Dictionary<string, AudioSource> soundDictionary;

    private void Awake()
    {
        // Ensure Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Initialize sound dictionary and add AudioSources
        soundDictionary = new Dictionary<string, AudioSource>();
        foreach (var sound in sounds)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.clip = sound.clip;
            source.volume = sound.volume;
            source.loop = sound.loop;

            soundDictionary.Add(sound.name, source);
        }
    }

    public void PlaySound(string name)
    {
        if (soundDictionary.ContainsKey(name))
        {
            soundDictionary[name].Play();
        }
        else
        {
            Debug.LogWarning($"Sound '{name}' not found in SoundManager.");
        }
    }

    public void StopSound(string name)
    {
        if (soundDictionary.ContainsKey(name))
        {
            soundDictionary[name].Stop();
        }
        else
        {
            Debug.LogWarning($"Sound '{name}' not found in SoundManager.");
        }
    }
}

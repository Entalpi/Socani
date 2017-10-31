using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound {
    public enum SoundType {
        Effect, Music
    }

    public SoundType type = SoundType.Effect;

    public bool enabled = true;
    public bool loop = false;

    public AudioClip clip;

    public string name;

    [Range(0f, 1f)]
    public float volume = 0.5f;

    [Range(0.1f, 3f)]
    public float pitch = 1.0f;

    [HideInInspector]
    public AudioSource source;
}


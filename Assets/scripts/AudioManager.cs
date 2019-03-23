using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    // Singleton instance 
    public static AudioManager instance;

    // Disable/enable all the sounds
    public bool on;

    // Disable/enable theme music
    public bool music = true;

    // Disable/enable sound effects
    public bool effect = true;

    public Sound[] sounds;

	// Use this for initialization
	void Awake () {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);

		foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
	}

    void Start () {
        if (on && music) {
            Play("theme-song");
        }
    }

    public Sound GetSound(string name) {
        return Array.Find(sounds, sound => sound.name == name);
    }

    public void Play(Sound s) {
        if (!on) {
            return;
        }

        if (s != null) {
            if (s.type == Sound.SoundType.Music && !music || s.type == Sound.SoundType.Effect && !effect) {
                return;
            }
            s.source.Play();
        }
    }

    public void Play(string name) {
        if (!on) {
            return;
        }
   
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Could not find sound with name: " + name);
            return;
        }
        if (s.type == Sound.SoundType.Music && !music || s.type == Sound.SoundType.Effect && !effect) {
            return;
        }
        s.source.Play();
    }
}

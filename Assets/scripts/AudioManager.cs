using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;

    // Disable/enable all the sounds
    public bool on;

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
        Play("theme-song");
    }

    public Sound GetSound(string name) {
        return Array.Find(sounds, sound => sound.name == name);
    }

    public void Play(Sound s) {
        if (s != null) {
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
        s.source.Play();
    }
}

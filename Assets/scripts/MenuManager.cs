using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

  public GameObject musicButton;
  public GameObject soundButton;

  public void Play() {
		StartCoroutine (GetComponent<Fading> ().LoadScene("scenes/levelselectionmenu"));
  } 

  public void ToggleMusic() {
    AudioManager.instance.music = !AudioManager.instance.music;
    // TODO: Change icon on button and so on
    if (AudioManager.instance.music) {

    } else {

    }
  }

  public void ToggleSoundEffects() {
    AudioManager.instance.effect = !AudioManager.instance.effect;
    // TODO: Change icon on button and so on
    if (AudioManager.instance.effect) {

    } else {

    }
  }
}

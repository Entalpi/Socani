using UnityEngine;

public class MenuManager : MonoBehaviour {

  public GameObject musicButton;
  public GameObject soundButton;

  public void Play() {
    if (PlayerPrefs.GetInt("is_first_run", 1) == 1) {
      PlayerPrefs.SetInt("is_first_run", 0);
      StartCoroutine(GetComponent<Fading>().LoadScene("scenes/playing"));
    } else {
      StartCoroutine(GetComponent<Fading>().LoadScene("scenes/levelselectionmenu"));
    }
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

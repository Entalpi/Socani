using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AfterLevelMenuManager : MonoBehaviour {

  public Text info;
	public GameObject levelSelectionButton;

  // Rewind heads related
  public Image rewindHead1;
  public Image rewindHead2;
  public Image rewindHead3;

  void Start() {
    info.text = "Moves: " + LevelManager.instance.currentLevel.numberOfMoves;
    StartCoroutine(RewindHeadCoinAnimation());
  }

  public void nextLevelButtonPressed() {
		LevelManager.instance.nextLevel();
		StartCoroutine(GetComponent<Fading>().LoadScene("scenes/playing"));
	}

	public void LevelSelectionButtonPressed() {
    StartCoroutine(GetComponent<Fading>().LoadScene("scenes/levelselectionmenu"));
  }

  public IEnumerator RewindHeadCoinAnimation() {
    yield return new WaitForSeconds(0.2f);
  }
}

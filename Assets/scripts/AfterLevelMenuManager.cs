using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AfterLevelMenuManager : MonoBehaviour {

  public Text info;
	public GameObject levelSelectionButton;

  public List<Image> rewindHeads;
  public Text coinsRewardedThisLevel;
  public Text coinsTotal;

  void Start() {
    info.text = "Moves: " + LevelManager.instance.currentLevel.numberOfMoves;

    if (LevelManager.instance.currentLevel.completed) {
      coinsRewardedThisLevel.text = "" + PlayerPrefs.GetInt("coins");
      return;
    }
    coinsTotal.text = "" + PlayerPrefs.GetInt("coins");
    // coinsRewardedThisLevel.text = coins picked up & num rewinds left;
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
    // TODO: Animate things
    for (int i = 0; i < LevelManager.instance.currentLevel.numRewindsLeft; i++) { 
      yield return new WaitForSeconds(0.2f);
    }
  }
}

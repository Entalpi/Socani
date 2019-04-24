using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AfterLevelMenuManager : MonoBehaviour {

	public GameObject nextLevelButton;
  public List<Image> rewindHeads;
  public Text coinsRewardedThisLevel;
  public Text coinsGrabbedThisLevel;
  public Text coinsTotal;
  public Text info;

  void Start() {
    info.text = "Moves: " + LevelManager.instance.currentLevel.numberOfMoves;
    coinsRewardedThisLevel.text = "";
    uint coinsRewarded = (uint)LevelManager.instance.currentLevel.numRewindsLeft + LevelManager.instance.currentLevel.numCoinsRewarded;
    coinsTotal.text = "" + (PlayerPrefs.GetInt("coins") - coinsRewarded);

    if (LevelManager.instance.nextLevel() == null) { nextLevelButton.SetActive(false); }

    if (LevelManager.instance.nextLevel() != null) {
      if (!LevelManager.instance.nextLevel().unlocked) {
        if (LevelManager.instance.nextLevel().unlockPrice > PlayerPrefs.GetInt("coins")) {
          nextLevelButton.GetComponent<Text>().text = "Watch ad"; // FIXME
        } else {
          nextLevelButton.GetComponent<Text>().text = "Unlock level";
        }
      }
    }
    StartCoroutine(RewindHeadCoinAnimation());
  }

  public void nextLevelButtonPressed() {
    AudioManager.instance.Play("click");

    uint levelIndex = LevelManager.instance.currentLevel.levelIndex + 1;
    if (levelIndex >= LevelManager.instance.levels.Length) { return; }

    if (!LevelManager.instance.nextLevel().unlocked) {
      if (LevelManager.instance.nextLevel().unlockPrice > PlayerPrefs.GetInt("coins")) {
        // TODO: Watch ad
      } else {
        // TODO: Spend gold, unlock next level
      }
    }

		LevelManager.instance.loadNextLevel();
		StartCoroutine(GetComponent<Fading>().LoadScene("Resources/scenes/playing"));
	}

	public void LevelSelectionButtonPressed() {
    AudioManager.instance.Play("click");
    StartCoroutine(GetComponent<Fading>().LoadScene("Resources/scenes/levelselectionmenu"));
  }

  public IEnumerator RewindHeadCoinAnimation() {
    const float dt = 0.2f; 
    coinsRewardedThisLevel.text = "+0";

    for (int i = 0; i < LevelManager.instance.currentLevel.numRewindsLeft; i++) {
      for (int j = 0; j < (int) (1.0f / dt) + 2; j++) {
        rewindHeads[i].transform.localScale = new Vector3(j * dt, j * dt, 1.0f);
        yield return new WaitForSeconds(dt / 2.0f);
      }
      coinsRewardedThisLevel.text = "+" + (i + 1);
    }

    for (int i = 0; i < LevelManager.instance.currentLevel.numCoinsRewarded; i++) {
      coinsGrabbedThisLevel.text = "+" + i;
      yield return new WaitForSeconds(dt / 2.0f);
    }

    uint coinsRewarded = LevelManager.instance.currentLevel.numRewindsLeft + LevelManager.instance.currentLevel.numCoinsRewarded;
    for (int i = 0; i < coinsRewarded; i++) {
      coinsTotal.text = "" + (PlayerPrefs.GetInt("coins") - coinsRewarded + i + 1);
      AudioManager.instance.Play("score");
      yield return new WaitForSeconds(dt / 2.0f);
    }
  }
}

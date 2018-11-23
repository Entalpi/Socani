using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AfterLevelMenuManager : MonoBehaviour {

  public Text info;
  public Text menuTitle;
	public GameObject levelSelectionButton;

  void Start() {
    info.text = "Moves: " + LevelManager.instance.currentLevel.numberOfMoves;
  }

  public void nextLevelButtonPressed() {
		LevelManager.instance.nextLevel();
		StartCoroutine(GetComponent<Fading>().LoadScene("scenes/playing"));
	}

	public void LevelSelectionButtonPressed() {
		SceneManager.LoadScene("scenes/levelselectionmenu");
	}
}

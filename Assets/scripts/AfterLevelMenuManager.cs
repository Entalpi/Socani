using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AfterLevelMenuManager : MonoBehaviour {

	public Text menuTitle;
	public GameObject retryButton;
	public GameObject levelSelectionButton;

	void Start() {
		// TODO: Change text
	}

	public void RetryButtonPressed() {
		// Restart the level with the same level in the LevelManager
		SceneManager.LoadScene ("player");
	}

	public void LevelSelectionButtonPressed() {
		SceneManager.LoadScene ("levelselectionmenu");
	}
}

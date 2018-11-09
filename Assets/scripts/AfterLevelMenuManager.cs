using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AfterLevelMenuManager : MonoBehaviour {

	public Text menuTitle;
	public GameObject nextLevelButton;
	public GameObject levelSelectionButton;

	public void nextLevelButtonPressed() {
		LevelManager.instance.nextLevel ();
		StartCoroutine(GetComponent<Fading>().LoadScene("player"));
	}

	public void LevelSelectionButtonPressed() {
		SceneManager.LoadScene ("levelselectionmenu");
	}
}

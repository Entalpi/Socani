using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour {

    public GameObject scrollView;
    public Button[] buttons;
    public Button buttonPrefab;

	// Use this for initialization
	void Start () {
        Level[] levels = LevelManager.instance.levels;
        buttons = new Button[levels.Length];
        foreach (Level level in levels) {

        }
	}
}

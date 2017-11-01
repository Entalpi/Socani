using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour {

    public GameObject scrollView;
    public GameObject buttonPrefab;

	// Use this for initialization
	void Start () {
        Level[] levels = LevelManager.instance.levels;
        for (int i = 0; i < levels.Length; i++) {
            GameObject obj = Instantiate(buttonPrefab);
            Button btn = obj.GetComponent<Button>();
            if (btn) {
                int idx = i; // Using only 'i' does not work (C# lambda uses the variable value at i AFTER the loop is done)
                btn.onClick.AddListener(() => onButtonClicked(levels[idx]));
                btn.transform.SetParent(scrollView.transform, false);
            }
        }
	}

    private void onButtonClicked(Level level) {
        Debug.Log("Button clicked with Level: " + level.levelIndex);
    }
}

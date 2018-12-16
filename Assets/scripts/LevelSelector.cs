using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour {
  // Level selection related
  public GameObject scrollView;
  public GameObject buttonPrefab;

  public Text numCoinsText;
  public Coin coinPrefab;

	// Use this for initialization
	void Start () {
    Level[] levels = LevelManager.instance.levels;
    for (int i = 0; i < levels.Length; i++) {
      GameObject obj = Instantiate(buttonPrefab);
      Button btn = obj.GetComponent<Button>();
      if (btn) {
        btn.GetComponentInChildren<Text>().text = string.Format("{0}", i + 1);
        int idx = i; // Using only 'i' does not work (C# lambda uses the variable value at i AFTER the loop is done)
        btn.onClick.AddListener(() => onButtonClicked(levels[idx]));
        btn.transform.SetParent(scrollView.transform, false);
      }
    }

    int numCoins = PlayerPrefs.GetInt("coins");
    if (numCoins > 0) {
      numCoinsText.text = "+" + numCoins;
      for (uint i = 0; i < numCoins; i++) {
        Coin clone = Instantiate(coinPrefab, numCoinsText.transform);
        StartCoroutine(AnimateCoin(clone));
      }
    }
  }

  public IEnumerator AnimateCoin(Coin coin) {
    float deltaTheta = Random.Range(0.0f, Mathf.PI / 32.0f);
    float radius = 100.0f * Tile.Size.x;
    float thetaX = Mathf.Cos(Random.Range(0.0f, 2.0f * Mathf.PI));
    float thetaY = Mathf.Sin(Random.Range(0.0f, 2.0f * Mathf.PI));
    coin.transform.localScale = new Vector3(100.0f, 100.0f); 
    while (true) {
      coin.transform.localPosition = new Vector3(radius * Mathf.Cos(thetaX), radius * Mathf.Sin(thetaY));
      thetaX += deltaTheta; thetaY += deltaTheta;
      yield return 0.2f;
    }
  }

  private Vector3 randomVector2UnitCircle(float r) {
    return new Vector3(r * Mathf.Cos(Random.Range(0.0f, 2.0f * Mathf.PI)), r * Mathf.Sin(Random.Range(0.0f, 2.0f * Mathf.PI)), 0.0f);
  }

  private void onButtonClicked(Level level) {
		LevelManager.instance.currentLevel = level;
		StartCoroutine(GetComponent<Fading>().LoadScene("scenes/playing"));
  }
}

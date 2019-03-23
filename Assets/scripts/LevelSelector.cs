using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour {
  // Level selection related
  public GameObject scrollView;
  public GameObject buttonPrefab;

  private GameObject[] buttons;

  public Text numCoinsText;
  public Coin coinPrefab;

	// Use this for initialization
	void Start () {
    Level[] levels = LevelManager.instance.levels;
    buttons = new GameObject[levels.Length];
    for (int i = 0; i < levels.Length; i++) {
      buttons[i] = Instantiate(buttonPrefab);
      Button btn = buttons[i].GetComponent<Button>();
      if (btn) {
        Text btnText = btn.GetComponentInChildren<Text>();
        btnText.text = string.Format("{0}", i + 1);
        
        // Style button
        if (!levels[i].unlocked) {
          btnText.text = string.Format("{0}G", levels[i].unlockPrice);
          btnText.color = SocaniColor.NegativeText;
        }

        if (levels[i].completed) {
          btnText.color = SocaniColor.PositiveText;
        }

        int idx = i; // Using only 'i' does not work (C# lambda uses the variable value at i AFTER the loop is done)
        btn.onClick.AddListener(() => onButtonClicked(levels[idx]));
        btn.transform.SetParent(scrollView.transform, false);
      }
    }

    StartCoroutine(AnimateCoinText());

    int numCoins = PlayerPrefs.GetInt("coins");
    if (numCoins > 0) {
      numCoinsText.text = "+" + numCoins;
      for (uint i = 0; i < numCoins; i++) {
        Coin clone = Instantiate(coinPrefab, numCoinsText.transform);
        StartCoroutine(AnimateCoin(clone));
      }
    }
  }

  public IEnumerator AnimateCoinText() {
    const float dt = 0.05f;
    float t = 0.0f;
    while (true) {
      t += dt;
      float v = Mathf.Min(Mathf.Abs(Mathf.Cos(t)) + 0.5f, 1.5f);
      numCoinsText.transform.localScale = new Vector3(v, v, v);
      yield return 0.75f;
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

  public void DisplayLevelTooExpensiveAnimation(Level level) {
    // TODO: Check the button or something
  }

  public void DisplayLevelUnlockedAnimation(Level level) {
    uint numCoins = (uint)PlayerPrefs.GetInt("coins");
    numCoinsText.text = string.Format("{0}", numCoins);

    Button btn = buttons[level.levelIndex].GetComponent<Button>();
    Text btnText = btn.GetComponentInChildren<Text>();
    btnText.text = string.Format("{0}", level.levelIndex + 1);
    btnText.color = SocaniColor.InformationText;
  }

  private void onButtonClicked(Level level) {
    uint numCoins = (uint) PlayerPrefs.GetInt("coins");

    if (!level.unlocked) {
      if (level.unlockPrice > numCoins) {
        DisplayLevelTooExpensiveAnimation(level);
        return;
      }

      PlayerPrefs.SetInt("coins", (int)(numCoins - level.unlockPrice));
      LevelManager.instance.levels[level.levelIndex].unlocked = true;

      DisplayLevelUnlockedAnimation(level);
    } else {
      LevelManager.instance.currentLevel = level;
      StartCoroutine(GetComponent<Fading>().LoadScene("scenes/playing"));
    }
  }
}

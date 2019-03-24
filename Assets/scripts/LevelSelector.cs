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
    for (uint i = 0; i < levels.Length; i++) {
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

        uint idx = i; // Using only 'i' does not work (C# lambda uses the variable value at i AFTER the loop is done)
        btn.onClick.AddListener(() => onButtonClicked(levels[idx], idx));
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
      float v = Mathf.Min(Mathf.Abs(Mathf.Cos(t)) + 0.75f, 1.25f);
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
      yield return null;
    }
  }

  private Vector3 randomVector2UnitCircle(float r) {
    return new Vector3(r * Mathf.Cos(Random.Range(0.0f, 2.0f * Mathf.PI)), r * Mathf.Sin(Random.Range(0.0f, 2.0f * Mathf.PI)), 0.0f);
  }

  IEnumerator DisplayLevelTooExpensiveAnimation(Level level, uint idx) {
    const uint dt = 10;
    const float shakeLng = 10.0f;
    Vector3 originalPosition = buttons[idx].transform.position;
    for (int i = 0; i < dt; i++) {
      Vector3 position = originalPosition + new Vector3(shakeLng * Mathf.Sin(2 * Mathf.PI * (i / dt)), 0.0f, 0.0f);
      buttons[idx].transform.position = position;
      yield return null;
    }
  }

  public void DisplayLevelUnlockedAnimation(Level level) {
    // TODO: Animate things
    uint numCoins = (uint)PlayerPrefs.GetInt("coins");
    numCoinsText.text = string.Format("{0}", numCoins);

    Button btn = buttons[level.levelIndex].GetComponent<Button>();
    Text btnText = btn.GetComponentInChildren<Text>();
    btnText.text = string.Format("{0}", level.levelIndex + 1);
    btnText.color = SocaniColor.InformationText;
  }

  private void onButtonClicked(Level level, uint idx) {
    uint numCoins = (uint) PlayerPrefs.GetInt("coins");

    if (!level.unlocked) {
      if (level.unlockPrice > numCoins) {
        StartCoroutine(DisplayLevelTooExpensiveAnimation(level, idx));
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

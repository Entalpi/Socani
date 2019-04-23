using UnityEngine;

public class CrateGoal : MonoBehaviour {

  public Sprite redSprite;
  public Sprite blueSprite;
  public Sprite greenSprite;
  public Sprite normalSprite;

  public CrateColor _Color;  // 'backing field' ...
  public CrateColor Color {
    set {
      switch (value) {
        case CrateColor.red:
          GetComponent<SpriteRenderer>().sprite = redSprite;
          break;
        case CrateColor.blue:
          GetComponent<SpriteRenderer>().sprite = blueSprite;
          break;
        case CrateColor.green:
          GetComponent<SpriteRenderer>().sprite = greenSprite;
          break;
        case CrateColor.normal:
          GetComponent<SpriteRenderer>().sprite = normalSprite;
          break;
      }
      _Color = value;
    }
    get { return _Color; }
  }
}

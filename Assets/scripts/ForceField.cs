using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour {

  // Color to direction mappings
  public static Color downColor = new Color(1.0f, 0.3176471f, 0.0f, 1.0f);
  public static Color upColor   = new Color(0.8431373f, 0.0f, 1.0f, 1.0f);
  public static Color leftColor;
  public static Color rightColor;

  public enum Direction { up, down, left, right }
  public Direction forceDirection;

  public Rigidbody2D rb2D;

  private void Start() {
    rb2D = gameObject.GetComponent<Rigidbody2D>();
  }

  private void OnCollisionEnter2D(Collision2D collision) {
    Debug.Log(string.Format("ForceField hit {0}.", collision.gameObject.name));
  }

  public void directionFromColor(Color color) {
    // WTF: Color.Equals(other) does not work here ...
    if (color == downColor) {
      forceDirection = Direction.down; 
    } else if (color == upColor) {
      forceDirection = Direction.up;
    } else if (color == leftColor) {
      forceDirection = Direction.left;
    } else if (color == rightColor) {
      forceDirection = Direction.right;
    } else {
      Debug.Log(string.Format("Failed to map color ({0}) to ForceField.Direction.", color));
    }
  }
}

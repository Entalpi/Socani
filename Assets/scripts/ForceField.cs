using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour {

  // Color to direction mappings
  public Color downColor = new Color(1.0f, 81 / 255, 0.0f);
  public Color upColor   = new Color(215 / 255, 0.0f, 1.0f);
  public Color leftColor;
  public Color rightColor;

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
    if (color == downColor) {
      forceDirection = Direction.down; 
    } else if (color == upColor) {
      forceDirection = Direction.up;
    } else if (color == leftColor) {
      forceDirection = Direction.left;
    } else if (color == rightColor) {
      forceDirection = Direction.right;
    } else {
      Debug.Log("Failed to map color to ForceField.Direction.");
    }
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour {

  // Color to direction mappings to the tiles parsed in tile layers via Level.cs and as defined in GameBoard.cs
  public static Color downColor = new Color(1.0f, 0.3176471f, 0.0f, 1.0f);
  public static Color upColor   = new Color(0.8431373f, 0.0f, 1.0f, 1.0f);
  public static Color leftColor;
  public static Color rightColor;

  public enum Direction { up, down, left, right }
  public Direction forceDirection;

  private void OnTriggerEnter2D(Collider2D collision) {
    return;
    if (collision.gameObject.GetComponent<Tile>()) {
      Vector3Int boardPos = collision.gameObject.GetComponent<Tile>().boardPosition;
      Debug.Log(boardPos);
      bool validMove = FindObjectOfType<GameBoard>().valid_move(collision.gameObject, boardPos, DeltaFromDirection(forceDirection));
      Debug.Log(validMove);
    }
    Debug.Log(string.Format("ForceField hit {0}.", collision.gameObject.name));
  }

  public static Vector3Int DeltaFromDirection(Direction direction) {
    switch (direction) {
      case Direction.up: return Vector3Int.up;
      case Direction.down: return Vector3Int.down;
      case Direction.left: return Vector3Int.left;
      case Direction.right: return Vector3Int.right;
      default: throw new System.Exception("This should not happen, ever.");
    }
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

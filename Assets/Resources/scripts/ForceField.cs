using UnityEngine;

public class ForceField : MonoBehaviour {
  // Player clickable to switch force direction
  public bool switchable = false; // 'backing field' ...
  public bool Switchable {
    set {
      gameObject.GetComponent<SpriteRenderer>().color = SocaniColor.ActionableText;
      ForceDirection = ForceDirection;
      switchable = value;
    }
    get { return switchable; }
  }

  public Sprite downSprite;
  public Sprite upSprite;
  public Sprite leftSprite;
  public Sprite rightSprite;

  public enum Direction { up, down, left, right }
  private Direction forceDirection; // 'backing field' ...
  public Direction ForceDirection {
    set {
      switch (value) {
        case Direction.down:
          GetComponent<SpriteRenderer>().sprite = downSprite;
          break;
        case Direction.up:
          GetComponent<SpriteRenderer>().sprite = upSprite;
          break;
        case Direction.left:
          GetComponent<SpriteRenderer>().sprite = leftSprite;
          break;
        case Direction.right:
          GetComponent<SpriteRenderer>().sprite = rightSprite;
          break;
      }
      forceDirection = value;
    }
    get { return forceDirection; }
  }

  private void OnMouseDown() {
    // Rotate through all forcefield directions
    if (Switchable) {
      switch (ForceDirection) {
        case Direction.up:
          ForceDirection = Direction.right;
          break;
        case Direction.right:
          ForceDirection = Direction.down;
          break;
        case Direction.down:
          ForceDirection = Direction.left;
          break;
        case Direction.left:
          ForceDirection = Direction.up;
          break;
      }
    }
  }

  private void OnTriggerStay2D(Collider2D collision) {
    if (collision.gameObject.GetComponent<Tile>()) {
      Vector3Int boardPos = collision.gameObject.GetComponent<Tile>().boardPosition;
      Debug.Log(boardPos);
      bool validMove = FindObjectOfType<GameBoard>().validMove(collision.gameObject, boardPos, DeltaFromDirection(forceDirection));
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
}

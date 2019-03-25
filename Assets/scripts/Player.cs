using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour, IEndDragHandler {
	// Private
	private const float MOVE_COOLDOWN = 0.25f;
	private float since_last_move = 0.0f;

	// Player board tile representation
	private Tile tile; 
	private GameBoard board;
  private Rigidbody2D rb2D;
  private Animator animator;

	void Start() {
    animator = GetComponent<Animator>();
    board = FindObjectOfType<GameBoard>();
		tile = GetComponent<Tile>();
    // Disable rotation on the RB
    rb2D = GetComponent<Rigidbody2D>();
    rb2D.freezeRotation = true;
    // Setup internal tile in the GameBoard
    tile.boardPosition = board.world_to_board_position(transform.position);
    Vector2Int position = new Vector2Int(tile.boardPosition.x, tile.boardPosition.y);
    board.board[position].Add(gameObject);
	}
 
	void Update() {
		since_last_move += Time.deltaTime;
    if (since_last_move <= MOVE_COOLDOWN) { return; }
		bool did_move = false;
		if (Input.GetKey("up")) {
      animator.Play("Walking Up");
      did_move = board.validMove(gameObject, tile.boardPosition, Vector3Int.up);
		}
    else if (Input.GetKey("down")) {
      animator.Play("Walking Down");
      did_move = board.validMove(gameObject, tile.boardPosition, Vector3Int.down);
    }
    else if (Input.GetKey("left")) {
      animator.Play("Walking Left");
      did_move = board.validMove(gameObject, tile.boardPosition, Vector3Int.left);
    }
    else if (Input.GetKey("right")) {
      animator.Play("Walking Right");
      did_move = board.validMove(gameObject, tile.boardPosition, Vector3Int.right);
    }
    if (did_move) {
			board.endMove();
			since_last_move = 0.0f;
      board.CheckGamestate();
    }
  }

  private void OnTriggerEnter2D(Collider2D collision) {
    Debug.Log(string.Format("Player hit {0}.", collision.gameObject.name));
  }

  private void OnCollisionEnter2D(Collision2D collision) {
    Debug.Log(string.Format("Player hit {0}.", collision.gameObject.name));
  }

  private enum Direction { Up, Down, Right, Left }

  public void OnEndDrag(PointerEventData eventData) {
    Vector3 dragVector = (eventData.position - eventData.pressPosition).normalized;
    bool did_move = false;
    switch (swipeDirection(dragVector)) {
      case Direction.Up:
        animator.Play("Walking Up");
        did_move = board.validMove(gameObject, tile.boardPosition, Vector3Int.up);
        break;
      case Direction.Down:
        animator.Play("Walking Down");
        did_move = board.validMove(gameObject, tile.boardPosition, Vector3Int.down);
        break;
      case Direction.Left:
        animator.Play("Walking Left");
        did_move = board.validMove(gameObject, tile.boardPosition, Vector3Int.left);
        break;
      case Direction.Right:
        animator.Play("Walking Right");
        did_move = board.validMove(gameObject, tile.boardPosition, Vector3Int.right);
        break;
    }
    if (did_move) {
      board.endMove();
      since_last_move = 0.0f;
      board.CheckGamestate();
    }
  }

  private Direction swipeDirection(Vector3 dragVector) {
    float positiveX = Mathf.Abs(dragVector.x);
    float positiveY = Mathf.Abs(dragVector.y);
    if (positiveX > positiveY) {
      if (dragVector.x > 0) { return Direction.Right; } else { return Direction.Left; }
    } else {
      if (dragVector.y > 0) { return Direction.Up; } else { return Direction.Down; }
    }
  }
}
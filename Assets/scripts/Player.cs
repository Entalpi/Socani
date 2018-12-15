using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	// Private
	private const float MOVE_COOLDOWN = 0.25f;
	private float since_last_move = 0.0f;

	// Player board tile representation
	private Tile tile; 
	private GameBoard board;
  private Rigidbody2D rb2D;
  private Animator animator;

  // Animation constants
  private const int WALK_STATE_LEFT = 1;
  private const int WALK_STATE_UP = 2;
  private const int WALK_STATE_RIGHT = 3;
  private const int WALK_STATE_DOWN = 4;

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
      did_move = board.valid_move(gameObject, tile.boardPosition, Vector3Int.up);
      animator.SetInteger("WalkingState", WALK_STATE_UP);
		} else if (Input.GetKey("down")) {
      did_move = board.valid_move(gameObject, tile.boardPosition, Vector3Int.down);
      animator.SetInteger("WalkingState", WALK_STATE_DOWN);
    }
    else if (Input.GetKey("left")) {
      did_move = board.valid_move(gameObject, tile.boardPosition, Vector3Int.left);
      animator.SetInteger("WalkingState", WALK_STATE_LEFT);
    }
    else if (Input.GetKey("right")) {
      did_move = board.valid_move(gameObject, tile.boardPosition, Vector3Int.right);
      animator.SetInteger("WalkingState", WALK_STATE_RIGHT);
    }
    if (did_move) {
			board.endMove();
			since_last_move = 0.0f;
      board.checkGamestate();
    }
  }

  private void OnCollisionEnter2D(Collision2D collision) {
    Debug.Log("We hit something!");
  }
}

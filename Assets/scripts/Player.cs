using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	// Private
	private const float move_cooldown = 0.25f;
	private float since_last_move = 0.0f;

	// Player board tile representation
	private Tile tile; 
	private GameBoard board;
  private Rigidbody2D rb2D;

	void Start() {
    transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

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
    if (since_last_move <= move_cooldown) { return; }
		bool did_move = false;
		if (Input.GetKey("up")) {
      did_move = board.valid_move(gameObject, tile.board_position, Vector3Int.up);
		} else if (Input.GetKey("down")) {
      did_move = board.valid_move(gameObject, tile.board_position, Vector3Int.down);
    }
    else if (Input.GetKey("left")) {
      did_move = board.valid_move(gameObject, tile.board_position, Vector3Int.left);
    }
    else if (Input.GetKey("right")) {
      did_move = board.valid_move(gameObject, tile.board_position, Vector3Int.right);
    }
    if (did_move) {
			board.endMove();
			since_last_move = 0.0f;
			if (board.checkGamestate()) {
				Debug.Log("Victory!");
			}
		}
  }

  private void OnCollisionEnter2D(Collision2D collision) {
    Debug.Log("We hit something!");
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	// Private
	private const float move_cooldown = 0.25f;
	private float since_last_move = 0.0f;

	// Movement directions
	private Vector3Int delta_up   = new Vector3Int(0, 1, 0);
	private Vector3Int delta_down = new Vector3Int(0, -1, 0);
	private Vector3Int delta_left = new Vector3Int(-1, 0, 0);
	private Vector3Int delta_right = new Vector3Int(1, 0, 0);

	// Player board tile
	private Tile tile; 
	private GameBoard board;

	void Start () {
		board = FindObjectOfType<GameBoard> ();
		tile = GetComponent<Tile> ();
	}
	
	void Update () {
		since_last_move += Time.deltaTime;

		if (since_last_move >= move_cooldown) {
			bool did_move = false;
			if (Input.GetKey ("up")) {
				if (board.valid_move (gameObject, tile.board_position, delta_up)) {
					did_move = true;
				}
			} else if (Input.GetKey ("down")) {
				if (board.valid_move (gameObject, tile.board_position, delta_down)) {
					did_move = true;
				}
			} else if (Input.GetKey ("left")) {
				if (board.valid_move (gameObject, tile.board_position, delta_left)) {
					did_move = true;
				}
			} else if (Input.GetKey ("right")) {
				if (board.valid_move (gameObject, tile.board_position, delta_right)) {
					did_move = true;
				}
			}
			if (did_move) {
				since_last_move = 0.0f;
			}
		}
    }
}

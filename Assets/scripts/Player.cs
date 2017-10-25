using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	// Private
	private const float move_cooldown = 0.10f;
	private float since_last_move = 0.0f;

	// Board position 
	private Vector3Int position; 

	private GameBoard board;

	void Start () {
		board = FindObjectOfType<GameBoard> ();
		position = board.transform_to_board_position (transform.position);
		GetComponent<Rigidbody2D> ().freezeRotation = true;
	}
	
	void FixedUpdate () {
		since_last_move += Time.deltaTime;
		transform.position = Vector3.Lerp (transform.position, board.board_to_transform_position(position), 10.0f * Time.deltaTime);
		
		if (since_last_move >= move_cooldown) {
			if (Input.GetKey ("down")) {
				Vector3Int new_pos = position;
				new_pos.y--;
				if (board.valid_move (new_pos)) {
					position = new_pos;
				}
			}
			if (Input.GetKey ("up")) {
				Vector3Int new_pos = position;
				new_pos.y++;
				if (board.valid_move (new_pos)) {
					position = new_pos;
				}
			}
			if (Input.GetKey ("right")) {
				Vector3Int new_pos = position;
				new_pos.x++;
				if (board.valid_move (new_pos)) {
					position = new_pos;
				}
			}
			if (Input.GetKey ("left")) {
				Vector3Int new_pos = position;
				new_pos.x--;
				if (board.valid_move (new_pos)) {
					position = new_pos;
				}
			}
			since_last_move = 0.0f;
		}
    }
}

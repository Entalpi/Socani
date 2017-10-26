using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour {
	public TileMapping[] mappings;

	public Level curr_lvl;
	public Dictionary<Vector2Int, List<GameObject>> board;

	// Private
	private Vector2 tile_size = new Vector2 (1.25f, 1.25f);
	private Player player;

	void Start () {
		board = curr_lvl.load (this);
		foreach (Vector2Int pos in board.Keys) {
			var tiles = board[pos];
			for(int i = 0; i < tiles.Count; i++) {
				GameObject tile = tiles [i];
				Vector3 position = board_to_transform_position (new Vector3Int(pos.x, pos.y, i));
				// Fill the board with objects created from the references
				board[pos][i] = Instantiate (tile, position, Quaternion.identity);
			}
		}
		player = FindObjectOfType<Player> ();
	}

	private void move_object(GameObject obj, Vector3Int from, Vector3Int to) {
		// Move the tiles in the board 
		Vector2Int board_key_from = new Vector2Int(from.x, from.y);
		Vector2Int board_key_to = new Vector2Int (to.x, to.y);
		board [board_key_from].RemoveAt (from.z);
		board [board_key_to].Add (obj);
		to.z = board [board_key_to].Count - 1; // Place objects on top of the tile
		Tile tile = obj.GetComponent<Tile> ();
		if (tile) {
			tile.board_position = to; // Update tile board position
			// Start the transform movement
			StartCoroutine (tile.smoothMovement (board_to_transform_position (to)));
		}
	}
		
	// Checks if the board position is a valid position
	public bool valid_move(GameObject obj, Vector3Int pos, Vector3Int delta) {
		if (pos.x < 0)  { return false; }
		if (pos.y < 0)  { return false; }
		Vector2Int delta_key = new Vector2Int(pos.x + delta.x, pos.y + delta.y);
		if (!board.ContainsKey (delta_key)) { 
			return false;
		} else {
            // Check for moveable tiles
            for (int z = 0; z < board[delta_key].Count; z++) {
                GameObject game_obj = board[delta_key][z];
                Tile tile = game_obj.GetComponent<Tile> ();
				if (tile) {
					if (tile.moveable) {
                        Vector3Int from = new Vector3Int(pos.x, pos.y, z) + delta;
                        bool moveable = valid_move (game_obj, from, delta);
						if (moveable) {
                            // Move the object at (pos.x, pos.y, z) to (pos + delta)
                            move_object(obj, from, from + delta);
						}
						return moveable;
					} else {
						return false;
					}
				}
			}
		}
        // Move the object at (pos) to the top of the stack at (pos + delta)
        Vector3Int from2 = new Vector3Int(pos.x, pos.y, board[delta_key].Count - 1);
		move_object(obj, from2, from2 + delta);
		return true;
	}

	// Takes a board position returns the world position
	public Vector3 board_to_transform_position(Vector3Int pos) {
		Vector3 origo = Camera.main.ViewportToWorldPoint (new Vector3 (0.08f, 0.1f, 10f));
		return origo + new Vector3 (pos.x * tile_size.x, pos.y * tile_size.y, -pos.z);
	}

	// Take a world position and return the board position
	public Vector3Int transform_to_board_position(Vector3 pos) {
		Vector3 origo = Camera.main.ViewportToWorldPoint (new Vector3 (0.08f, 0.1f, 10f));
		pos -= origo;
		pos.x /= tile_size.x;
		pos.y /= tile_size.y;
		return new Vector3Int((int) pos.x, (int) pos.y, (int) -pos.z);
	}

	// Checks if all the crate goals are fulfilled and returns the result
	public bool check_gamestate() {
		foreach (var key in board.Keys) {
			var stack = board [key];
			for (int z = 0; z < stack.Count; z++) {
				GameObject obj = stack [z];
				// Check if all the crate goals have a crate on top of them
				if (obj.GetComponent<CrateGoal> ()) {
					z++;
					if (z < stack.Count) {
						if (stack [z].GetComponent<Crate> ()) {
							continue;
						} 
					}
					return false;
				}
			}
		}
		return true;
	}
}

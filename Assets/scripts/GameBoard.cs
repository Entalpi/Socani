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
		
	// Checks if the board position is a valid position
	public bool valid_move(Vector3Int pos) {
		if (pos.x < 0)  { return false; }
		if (pos.y < 0)  { return false; }
		if (!board.ContainsKey (new Vector2Int(pos.x, pos.y))) { return false; } 
		return true;
	}

	// Takes a board position returns the world position
	public Vector3 board_to_transform_position(Vector3Int pos) {
		Vector3 origo = Camera.main.ViewportToWorldPoint (new Vector3 (0.08f, 0.1f, 10f));
		return origo + new Vector3 (pos.x * tile_size.x, pos.y * tile_size.y, -pos.z);
	}

	// Take a world position and return the board position
	public Vector3Int transform_to_board_position(Vector3 pos) {
		Debug.Log (pos);
		Vector3 origo = Camera.main.ViewportToWorldPoint (new Vector3 (0.08f, 0.1f, 10f));
		pos -= origo;
		pos.x /= tile_size.x;
		pos.y /= tile_size.y;
		Debug.Log (pos);
		return new Vector3Int((int) pos.x, (int) pos.y, (int) -pos.z);
	}
}

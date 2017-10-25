using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level : MonoBehaviour {
	public Texture2D[] tile_layers;

	public Dictionary<Vector2Int, List<GameObject>> load(GameBoard game_board) {
		Dictionary<Vector2Int, List<GameObject>> board = new Dictionary<Vector2Int, List<GameObject>> ();

		Debug.Log ("# tile layers: " + tile_layers.Length);

		for (int z = 0; z < tile_layers.Length; z++) {
			Texture2D tile_layer = tile_layers [z];

			Debug.Log (tile_layer);
			for (int x = 0; x < tile_layer.width; x++) {
				for (int y = 0; y < tile_layer.height; y++) {
					// Generate a new tile
					Color color = tile_layers [z].GetPixel (x, y);
					if (color.a == 0.0f) {
						continue;
					}
					Debug.Log (color);

					var position = new Vector2Int(x, y);
					if (!board.ContainsKey(position)) {
						board[position] = new List<GameObject>(2);
					}
					List<GameObject> tile_stack = board [position];

					for (int i = 0; i < game_board.mappings.Length; i++) {
						TileMapping tile_mapping = game_board.mappings[i];
						if (color.Equals (tile_mapping.color)) {
							board [position].Add(tile_mapping.prefab);
						}
					}
				}
			}
		}

		return board;
	}
}

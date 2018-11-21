using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level : MonoBehaviour {
	// Is the level completed
	public bool completed = false;
  // # of coins on the level 
  public int num_coins = 0;
  // User visible level number 
  public int levelIndex = 0;
  // Textures representing the level composition
	public Texture2D[] tile_layers;
  // Calculated at load time
  public Vector2Int dimensions;

	// Loads the tile layers in to a Dictionary with the tile position as key and a list of tiles as the value
	public Dictionary<Vector2Int, List<GameObject>> load(GameBoard game_board) {
		Dictionary<Vector2Int, List<GameObject>> board = new Dictionary<Vector2Int, List<GameObject>>();

    int maxWidth  = 0;
    int maxHeight = 0;

    for (int z = 0; z < tile_layers.Length; z++) {
			Texture2D tile_layer = tile_layers[z];

      if (tile_layer.width  > maxWidth)  { maxWidth = tile_layer.width; }
      if (tile_layer.height > maxHeight) { maxHeight = tile_layer.height; }

      for (int x = 0; x < tile_layer.width; x++) {
				for (int y = 0; y < tile_layer.height; y++) {
					// Generate a new tile
					Color color = tile_layers[z].GetPixel(x, y);
					if (color.a == 0.0f) { continue; }

					var position = new Vector2Int(x, y);
					if (!board.ContainsKey(position)) {
						board[position] = new List<GameObject>(2);
					}

					for (int i = 0; i < game_board.mappings.Length; i++) {
						TileMapping tile_mapping = game_board.mappings[i];
						if (color.Equals(tile_mapping.color)) {
              board[position].Add(tile_mapping.prefab);
              // Construct level variables
              if (tile_mapping.prefab.GetComponent<Coin>()) {
                  num_coins++;
              }
						}
					}
				}
			}
		}
    dimensions = new Vector2Int(maxWidth, maxHeight);
		return board;
	}
}

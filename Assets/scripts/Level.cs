﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Using the Level to store some data about the 
/// completetion of it as well. 
/// </summary>
[System.Serializable]
public class Level : MonoBehaviour {
  /** UPDATEABLE PROPERTIES */
  // Is the level completed
  public bool completed = false;
  // Number of moves used when completed
  public uint numberOfMoves = 0;
  // Number of rewinds left when completed
  public int numRewindsLeft = 0;
  // Is the level unlocked
  public bool unlocked = false;
  // Unlock price in coins
  public uint unlockPrice = 0;
  // Number of coins rewarded for the completion of the level
  public uint numCoinsRewarded = 0;

  /** FIXED PROPERTIES */
  // # of coins on the level 
  public int num_coins = 0;
  // User visible level number 
  public int levelIndex = 0;
  // Textures representing the level composition
	public Texture2D[] tile_layers;
  // Calculated at load time
  public Vector2Int dimensions;
  
  // Resets all progress on the Level
  public void reset() {
    completed = false;
    numberOfMoves = 0;
    unlocked = false;
    numCoinsRewarded = 0;
  }

  // Loads the tile layers in to a Dictionary with the tile position as key and a list of tiles as the value
  public Dictionary<Vector2Int, List<GameObject>> load(GameBoard game_board) {
		Dictionary<Vector2Int, List<GameObject>> board = new Dictionary<Vector2Int, List<GameObject>>();

    int maxWidth  = 0; // Dimensions of the loaded lvl in tiles
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

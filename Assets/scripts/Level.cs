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
  public int numCoins = 0;
  // User visible level number 
  public int levelIndex = 0;
  // Textures representing the level composition
	public Texture2D[] tileLayers;
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
  public Dictionary<Vector2Int, List<GameObject>> load(GameBoard gameboard, ref Dictionary<Vector3Int, Color> tileMappings) {
		Dictionary<Vector2Int, List<GameObject>> board = new Dictionary<Vector2Int, List<GameObject>>();

    int maxWidth  = 0; // Dimensions of the loaded lvl in tiles
    int maxHeight = 0;

    for (int z = 0; z < tileLayers.Length; z++) {
			Texture2D tileLayer = tileLayers[z];

      if (tileLayer.width  > maxWidth)  { maxWidth = tileLayer.width; }
      if (tileLayer.height > maxHeight) { maxHeight = tileLayer.height; }

      for (int x = 0; x < tileLayer.width; x++) {
				for (int y = 0; y < tileLayer.height; y++) {
					// Generate a new tile
					Color color = tileLayers[z].GetPixel(x, y);
					if (color.a == 0.0f) { continue; }

					var position = new Vector2Int(x, y);
					if (!board.ContainsKey(position)) {
						board[position] = new List<GameObject>(2);
					}

          bool foundMatchingColor = false;
					for (int i = 0; i < gameboard.mappings.Length; i++) {
						TileMapping tileMapping = gameboard.mappings[i];
						if (color.Equals(tileMapping.color)) {
              foundMatchingColor = true;

              board[position].Add(tileMapping.prefab);
              // Construct level variables
              if (tileMapping.prefab.GetComponent<Coin>()) {
                numCoins++;
              }

              tileMappings[new Vector3Int(x, y, board[position].Count - 1)] = tileMapping.color;
            }
          }

          if (!foundMatchingColor) { Debug.Log(string.Format("Did not find matching color for color: {0}", color)); }
				}
			}
		}
    dimensions = new Vector2Int(maxWidth, maxHeight);
		return board;
	}
}

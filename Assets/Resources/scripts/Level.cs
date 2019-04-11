using System.Collections.Generic;
using System.IO;
using System.Xml;
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
  public uint numRewindsLeft = 0;
  // Is the level unlocked
  public bool unlocked = false;
  // Number of coins rewarded for the completion of the level
  public uint numCoinsRewarded = 0;

  /** FIXED PROPERTIES */
  // Unlock price in coins
  public uint unlockPrice = 0;
  // # of coins on the level 
  public int numCoins = 0;
  // User visible level number 
  public int levelIndex = 0;
  // Textures representing the level composition
	public Texture2D[] tileLayers;
  // Calculated at load time
  public Vector2Int dimensions;
  // Tiled level in .tmx format
  public string tiledLevelTMXData;
  
  // Resets all progress on the Level
  public void Reset() {
    completed = false;
    numberOfMoves = 0;
    unlocked = false;
    numCoinsRewarded = 0;
  }

  static Dictionary<int, GameObject> loadTileset(string filepath) {
    XmlDocument xmlDoc = new XmlDocument();
    xmlDoc.Load("Assets/Resources/levels/levels" + "/" + filepath); // HACK: Tiled uses relative filepaths ..
    Dictionary<int, GameObject> tileset = new Dictionary<int, GameObject>();
    foreach (XmlNode childNode in xmlDoc.ChildNodes) {
      if (childNode.Name == "tileset") {
        foreach (XmlNode tile in childNode.ChildNodes) {
          if (tile.Name == "tile") {
            int id = int.Parse(tile.Attributes.GetNamedItem("id").Value);
            foreach (XmlNode property in tile.FirstChild.ChildNodes) { // Assumption
              if (property.Attributes.GetNamedItem("name")?.Value == "prefab_name") {
                string prefabName = property.Attributes.GetNamedItem("value").Value;
                Debug.Log("prefabs/" + prefabName);
                tileset[id] = Resources.Load<GameObject>("prefabs/" + prefabName);
                if (tileset[id] == null) {
                  Debug.LogError("Lets start question our life choices");
                }
                Debug.Log("Loaded " + "prefabs/" + prefabName + " with id =" + id);
              }
            }
          }
        }
      }
    }
    return tileset;
  }

  Dictionary<Vector2Int, List<GameObject>> loadTMXFile(string tmxFile) {
    XmlDocument xmlDoc = new XmlDocument();
    xmlDoc.Load(tmxFile);
    foreach (XmlNode childNode in xmlDoc.ChildNodes) {
      if (childNode.Name == "map") {
        int width = int.Parse(childNode.Attributes.GetNamedItem("width")?.Value);
        int height = int.Parse(childNode.Attributes.GetNamedItem("height")?.Value);
        Dictionary<int, GameObject> tileset;
        if (childNode.FirstChild.Name == "tileset") {
          string tileSetPath = childNode.FirstChild.Attributes.GetNamedItem("source").Value;
          tileset = loadTileset(tileSetPath);
        } else {
          Debug.LogError("Tileset missing from level .tmx");
          return null;
        }
        Dictionary<Vector2Int, List<GameObject>> board = new Dictionary<Vector2Int, List<GameObject>>();
        foreach (XmlNode layer in childNode.ChildNodes) {
          if (layer.Name == "layer") {
            string tileLayer = layer.FirstChild.InnerText; // CSV 
            string[] tiles = tileLayer.Split(',');
            for (int x = 0; x < width; x++) {
              for (int y = 0; y < height; y++) {
                // NOTE: .tmx, 0 = empty thus all indices are + 1 and need to be decremented
                int tileIdx = int.Parse(tiles[x * width + y]) - 1;
                if (tileIdx <= -1) { continue; }
                var position = new Vector2Int(x, y);
                if (!board.ContainsKey(position)) {
                  board[position] = new List<GameObject>(2);
                }
                board[position].Add(tileset[tileIdx]);
              }
            }
          }
        }
        return board; // XML parsing succeeded
      }
    }
    return null; // XML parsing failed
  }

  // Loads the tile layers in to a Dictionary with the tile position as key and a list of tiles as the value
  public Dictionary<Vector2Int, List<GameObject>> Load(GameBoard gameboard, ref Dictionary<Vector3Int, Color> tileMappings) {
		Dictionary<Vector2Int, List<GameObject>> board = new Dictionary<Vector2Int, List<GameObject>>();

    int maxWidth  = 0; // Dimensions of the loaded lvl in tiles
    int maxHeight = 0;

    if (tiledLevelTMXData.Length > 0) {
      Debug.Log("Loading TMX");
      return loadTMXFile(tiledLevelTMXData);
    }

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

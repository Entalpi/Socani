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
  public uint levelIndex = 0;
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

  static void parseTileProperties(GameObject obj, int id, Dictionary<int, Dictionary<string, string>> tileProperties) {
    if (obj.GetComponent<ForceField>()) {
      if (tileProperties[id].ContainsKey("direction")) {
        switch (tileProperties[id]["direction"]) {
          case "left":
            obj.GetComponent<ForceField>().ForceDirection = ForceField.Direction.left;
            break;
          case "right":
            obj.GetComponent<ForceField>().ForceDirection = ForceField.Direction.right;
            break;
          case "down":
            obj.GetComponent<ForceField>().ForceDirection = ForceField.Direction.down;
            break;
          case "up":
            obj.GetComponent<ForceField>().ForceDirection = ForceField.Direction.up;
            break;
        }
      }
    }
    if (obj.GetComponent<Crate>()) {
      if (tileProperties[id].ContainsKey("color")) {
        switch (tileProperties[id]["color"]) {
          case "red":
            obj.GetComponent<Crate>().Color = CrateColor.red;
            break;
          case "blue":
            obj.GetComponent<Crate>().Color = CrateColor.blue;
            break;
          case "green":
            obj.GetComponent<Crate>().Color = CrateColor.green;
            break;
          case "normal":
            obj.GetComponent<Crate>().Color = CrateColor.normal;
            break;
        }
      }
    }
    if (obj.GetComponent<CrateGoal>()) {
      if (tileProperties[id].ContainsKey("color")) {
        switch (tileProperties[id]["color"]) {
          case "red":
            obj.GetComponent<CrateGoal>().Color = CrateColor.red;
            break;
          case "blue":
            obj.GetComponent<CrateGoal>().Color = CrateColor.blue;
            break;
          case "green":
            obj.GetComponent<CrateGoal>().Color = CrateColor.green;
            break;
          case "normal":
            obj.GetComponent<CrateGoal>().Color = CrateColor.normal;
            break;
        }
      }
    }
  }

  // Loads the tileset referenced by the level.tmx file into a dictionary lookup
  static Dictionary<int, GameObject> loadTileset(string filepath, ref Dictionary<int, Dictionary<string, string>> tilesetProperties) {
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
                tileset[id] = Resources.Load<GameObject>("prefabs/" + prefabName);
                if (tileset[id] == null) {
                  Debug.LogError("prefabs/" + prefabName + " is null in tileset");
                }
                // Debug.Log("Loaded " + "prefabs/" + prefabName + " with id = " + id);

                // Adds the properties of the tile to a dictionary for lookup later on
                foreach (XmlNode prop in tile.FirstChild.ChildNodes) { // Assumption
                  if (!tilesetProperties.ContainsKey(id)) {
                    tilesetProperties[id] = new Dictionary<string, string>();
                  }
                  tilesetProperties[id][prop.Attributes.GetNamedItem("name")?.Value] = prop.Attributes.GetNamedItem("value")?.Value;
                }
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
        dimensions = new Vector2Int(width, height);
        Dictionary<int, GameObject> tileset;
        Dictionary<int, Dictionary<string, string>> tilesetProperties = new Dictionary<int, Dictionary<string, string>>();
        if (childNode.FirstChild.Name == "tileset") {
          string tileSetPath = childNode.FirstChild.Attributes.GetNamedItem("source").Value;
          tileset = loadTileset(tileSetPath, ref tilesetProperties);
        } else {
          Debug.LogError("Tileset missing from level .tmx");
          return null;
        }
        Dictionary<Vector2Int, List<GameObject>> board = new Dictionary<Vector2Int, List<GameObject>>();
        foreach (XmlNode layer in childNode.ChildNodes) {
          if (layer.Name == "layer") {
            string tileLayer = layer.FirstChild.InnerText; // CSV 
            string[] tiles = tileLayer.Split(',');
            for (int y = 0; y < height; y++) {
              for (int x = 0; x < width; x++) {
                // NOTE: .tmx, 0 = empty thus all indices are + 1 and need to be decremented
                int tileIdx = int.Parse(tiles[y * width + x]) - 1;
                if (tileIdx <= -1) { continue; }
                var position = new Vector2Int(x, (height - y)); // NOTE: Tiled origin is top left (GameBoard is bottom left)
                if (!board.ContainsKey(position)) {
                  board[position] = new List<GameObject>(2);
                }

                GameObject obj = Instantiate(tileset[tileIdx]);
                parseTileProperties(obj, tileIdx, tilesetProperties);
                board[position].Add(obj);

                // Construct level variables
                if (tileset[tileIdx].GetComponent<Coin>()) {
                  numCoins++;
                }
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
  public Dictionary<Vector2Int, List<GameObject>> Load() {
    if (tiledLevelTMXData.Length > 0) {
      return loadTMXFile(tiledLevelTMXData);
    } 
    Debug.LogError("Failed to load level ...");
    return null;
	}
}

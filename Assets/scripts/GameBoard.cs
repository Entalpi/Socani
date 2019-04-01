using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour {
  public BoardUI boardUI; // Handles some UI related matters

	public TileMapping[] mappings;

  public bool inputEnabled = true;

	public Level currentLevel;
	public Dictionary<Vector2Int, List<GameObject>> board;

  // Represents one tiles movement from one place to another
  public interface ICloneable<T> {
    T Clone();
  }
	struct BoardMove: ICloneable<BoardMove> {
		public GameObject movee;
		public Vector2Int from, to;
    public BoardMove Clone() {
      BoardMove clone = new BoardMove();
      clone.movee = movee; clone.to = to; clone.from = from;
      return clone;
    }
	}
	// History of all the moves thus far
	private Stack<List<BoardMove>> moveHistory = new Stack<List<BoardMove>>();
	// All the tiles moved in a single move (stored later in move history)
	private List<BoardMove> tilesMoved = new List<BoardMove>();

	void Start() {
		menuPanel.SetActive(false); // Hide menu per default
    LoadLevel();
    Vector3 position = new Vector3(-currentLevel.dimensions.x * Tile.Size.x / 2.0f, -currentLevel.dimensions.y * Tile.Size.y / 2.0f, 10f);
    position += new Vector3(Tile.Size.x / 2.0f, Tile.Size.y / 2.0f, 0.0f);
    transform.position = position;
  }

  public void LoadLevel() {
		currentLevel = LevelManager.instance.getLevel();
    currentLevel.reset();
		moveHistory.Clear();

    // Mapping between prefabs filled in the board and their color that matched during loading of the level
    Dictionary<Vector3Int, Color> tileMappings = new Dictionary<Vector3Int, Color>();
    board = currentLevel.load(this, ref tileMappings);  // Load the current level

    // Rescale tile size to fit the screen based on the Level dimensions and pixel scr size
    float scaleX = Mathf.Min(Screen.width  / (100.0f * Tile.OriginalSize.x * (currentLevel.dimensions.x + 0)), 1.0f);
    float scaleY = Mathf.Min(Screen.height / (100.0f * Tile.OriginalSize.y * (currentLevel.dimensions.y + 0)), 1.0f);
    Vector2 scale = new Vector2(Mathf.Min(scaleX, scaleY), Mathf.Min(scaleX, scaleY)); 
    Tile.Size = Vector2.Scale(Tile.OriginalSize, new Vector2(scale.x, scale.y));

    // Fill the board with objects created from the references
    foreach (Vector2Int pos in board.Keys) {
      var tiles = board[pos];
      for (int z = 0; z < tiles.Count; z++) {
        GameObject tilePrefab = tiles[z];
        Vector3Int boardPosition = new Vector3Int(pos.x, pos.y, z);
        Vector3 worldPosition = board_to_world_position(boardPosition);
        GameObject obj = Instantiate(tilePrefab, worldPosition, Quaternion.identity);
        obj.transform.localScale = new Vector3(scale.x, scale.y, 1.0f);

        if (obj.GetComponent<Tile>()) {
          obj.GetComponent<Tile>().boardPosition = boardPosition;
        }

        if (obj.GetComponent<ForceField>()) {
          var key = new Vector3Int(pos.x, pos.y, z);
          var value = new Color(0.0f, 0.0f, 0.0f);
          if (tileMappings.TryGetValue(key, out value)) {
            obj.GetComponent<ForceField>().directionFromColor(value);
          }
        }

        obj.transform.SetParent(transform);
        board[pos][z] = obj;
      }
    }
  }

	// Called to end the move and record all the tiles moves during this move
	public void endMove() {
		if (tilesMoved.Count > 0) {            
			moveHistory.Push(tilesMoved.ConvertAll(move => move.Clone())); // Deep copy
			tilesMoved.Clear();
      currentLevel.numberOfMoves++;
    }
  }

  // Tries to move the object located at 'from' to 'to'
  private bool moveObject(GameObject obj, Vector2Int from, Vector2Int to, bool recordMove = true) {
    // Search for the object in the tile stack
    for (int z = 0; z < board[from].Count; z++) {
      GameObject gameobject = board[from][z];
      if (obj.Equals(gameobject)) {
        board[from].RemoveAt(z);
        board[to].Add(obj);

		    // Place objects on top of the tile
        Tile tile = obj.GetComponent<Tile>();
        if (tile) {
          Vector3Int newPosition = new Vector3Int(to.x, to.y, board[to].Count - 1);
          tile.boardPosition = newPosition; // Update tile board position
          // Start the transform movement
          tile.transform.position = board_to_world_position(tile.boardPosition);
			    // StartCoroutine(tile.smoothMovement(board_to_world_position(newPosition)));
			    AudioManager.instance.Play("move");
        }
        if (recordMove) {
          // Add move to history
          BoardMove boardMove;
          boardMove.movee = obj;
          boardMove.from = from;
          boardMove.to = to;
          tilesMoved.Add(boardMove);
        }
        return true;
      }
    }
    return false;
	}
	
  // Checks if the board position is a valid position
  public bool validMove(GameObject obj, Vector3Int pos, Vector3Int delta) {
    if (obj == null) { return false; }
    if (pos.x < 0)  { return false; }
		if (pos.y < 0)  { return false; }
		Vector2Int delta_key = new Vector2Int(pos.x + delta.x, pos.y + delta.y);
		Vector2Int from = new Vector2Int(pos.x, pos.y);
		Vector2Int to   = new Vector2Int(pos.x + delta.x, pos.y + delta.y);
		if (!board.ContainsKey(delta_key)) { 
			return false;
		} else {
      // Check for moveable tiles
      for (int z = 0; z < board[delta_key].Count; z++) {
        GameObject gameobject = board[delta_key][z];
        Tile tile = gameobject.GetComponent<Tile>();
				if (tile) {
					if (tile.moveable) {
            bool moveable = validMove(gameobject, pos + delta, delta);
				    if (moveable) {
              // Move the object at (pos.x, pos.y, z) to (pos + delta)
					    return moveObject(obj, from, to);
				    }
					  return moveable;
					} else {
						return false;
					}
				}
			}
		}
    // Move the object at (pos) to the top of the stack at (pos + delta)
		return moveObject(obj, from, to);
	}

	// Takes a board position returns the world position
	public Vector3 board_to_world_position(Vector3Int pos) {
    Vector3 origin = transform.position;
    return origin + new Vector3(pos.x * Tile.Size.x, pos.y * Tile.Size.y, -pos.z);
	}

	// Take a world position and return the board position
	public Vector3Int world_to_board_position(Vector3 pos) {
    Vector3 origin = transform.position;
    pos -= origin;
		pos.x /= Tile.Size.x;
		pos.y /= Tile.Size.y;
		return new Vector3Int((int) pos.x, (int) pos.y, (int) -pos.z);
	}

	// Checks if all the crate goals are fulfilled and returns the result while loading the next level
	public bool CheckGamestate() {
		foreach (var key in board.Keys) {
			var stack = board[key];
			for (int z = 0; z < stack.Count; z++) {
        // Check if all the crate goals have the correct crate on top of them
        if (stack[z].GetComponent<CrateGoal>()) {
          bool nextTile = false;
          for (int i = 0; i < stack.Count; i++) {
            if (stack[i].GetComponent<Crate>()) {
              if (stack[z].GetComponent<CrateGoal>().crateColor.Equals(stack[i].GetComponent<Crate>().crateColor)) {
                nextTile = true;
              }
            }
          }
          if (!nextTile) { return false; }
          if (nextTile) { continue; }
        }
      }
		}
    currentLevel.numRewindsLeft = boardUI.numRewindsLeft;
    LevelManager.instance.levelCompleted(currentLevel);
		StartCoroutine(GetComponent<Fading>().LoadScene("scenes/afterlevelmenu"));
		return true;
	}

  // Tries to find the relevant obj in the stack at pos and removes it from the board
  public bool remove_from_board(GameObject obj, Vector3Int pos) {
    List<GameObject> stack = board[new Vector2Int(pos.x, pos.y)];
    for (int z = 0; z < stack.Count; z++) {
      if (obj.Equals(stack[z])) {
        stack.RemoveAt(z);
        return true;
      }
    }
    return false;
  }

	// Menu interactions
	public GameObject menuPanel;

  IEnumerator animateMenuDisplay(bool show) {
    const uint dt = 10;
    for (int i = 0; i <= dt; i++) {
      menuPanel.GetComponent<CanvasGroup>().alpha = show ? i * (1.0f / dt) : 1.0f - i * (1.0f / dt);
      yield return null;
    }
    if (!show) {
      menuPanel.SetActive(!menuPanel.activeSelf);
    }
  }

	public void pressedMenuButton() {
    inputEnabled = false;
    menuPanel.SetActive(!menuPanel.activeSelf);
    StartCoroutine(animateMenuDisplay(true));
	}

  // FIXME: Spawning clones, destroy?
	public void pressedRestartButton() {
    boardUI.numRewindsLeft = 3;
    boardUI.updateRewindHeads();

    foreach (Vector2Int tilePosition in board.Keys) {
			foreach(GameObject gameObject in board[tilePosition]) {
				Destroy(gameObject);
			}
		}
		LoadLevel();
		menuPanel.SetActive(false);
		Destroy(GameObject.FindGameObjectWithTag("Player"));
	}

	public void pressedLevelsButton() {
		StartCoroutine(GetComponent<Fading>().LoadScene("scenes/levelselectionmenu"));
	}

	public void pressedMenuCancelButton() {
    inputEnabled = true;
    StartCoroutine(animateMenuDisplay(false));
	}

	public void pressedRewindButton() {
		if (moveHistory.Count == 0) { return; }
    currentLevel.numberOfMoves--;
    const bool recordMove = false;
		List<BoardMove> boardMoves = moveHistory.Pop();
		for (int i = 0; i < boardMoves.Count; i++) {
			BoardMove boardMove = boardMoves[i];
			moveObject(boardMove.movee, boardMove.to, boardMove.from, recordMove);
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameBoard : MonoBehaviour {
	public TileMapping[] mappings;

	public Level currentLevel;
	public Dictionary<Vector2Int, List<GameObject>> board;

	// Private
	private Vector2 tileSize = new Vector2 (1.25f, 1.25f);

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

	void Start () {
		menuPanel.SetActive(false); // Hide menu per default
    LoadLevel();
    transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / currentLevel.dimensions.x,  Screen.height / currentLevel.dimensions.y, 10f));
  }

  public void LoadLevel() {
		currentLevel = LevelManager.instance.getLevel();
		moveHistory.Clear();

    // Load the current level
		board = currentLevel.load(this);
    foreach (Vector2Int pos in board.Keys) {
      var tiles = board[pos];
      for (int z = 0; z < tiles.Count; z++) {
        GameObject tile = tiles[z];
        Vector3 position = board_to_transform_position(new Vector3Int(pos.x, pos.y, z));
        // Fill the board with objects created from the references
        GameObject obj = Instantiate(tile, position, Quaternion.identity);
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
		}
	}

  // Tries to move the object located at 'from' to 'to'
  private bool moveObject(GameObject obj, Vector2Int from, Vector2Int to, bool recordMove = true) {
    // Search for the object in the tile stack
    for (int z = 0; z < board[from].Count; z++) {
      GameObject game_object = board[from][z];
      if (obj.Equals(game_object)) {
        board[from].RemoveAt(z);
        board[to].Add(obj);

		    // Place objects on top of the tile
		    Vector3Int newPosition = new Vector3Int(to.x, to.y, board [to].Count - 1);
        Tile tile = obj.GetComponent<Tile>();
        if (tile) {
			    tile.board_position = newPosition; // Update tile board position
          // Start the transform movement
			    StartCoroutine(tile.smoothMovement(board_to_transform_position(newPosition)));
			    AudioManager.instance.Play ("move");
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
  public bool valid_move(GameObject obj, Vector3Int pos, Vector3Int delta) {
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
        GameObject game_obj = board[delta_key][z];
        Tile tile = game_obj.GetComponent<Tile>();
				if (tile) {
					if (tile.moveable) {
            bool moveable = valid_move (game_obj, pos + delta, delta);
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
	public Vector3 board_to_transform_position(Vector3Int pos) {
    Vector3 origo = transform.position;
    return origo + new Vector3 (pos.x * tileSize.x, pos.y * tileSize.y, -pos.z);
	}

	// Take a world position and return the board position
	public Vector3Int transform_to_board_position(Vector3 pos) {
    Vector3 origo = transform.position;
    pos -= origo;
		pos.x /= tileSize.x;
		pos.y /= tileSize.y;
		return new Vector3Int((int) pos.x, (int) pos.y, (int) -pos.z);
	}

	// Checks if all the crate goals are fulfilled and returns the result
	public bool checkGamestate() {
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
		LevelManager.instance.levelCompleted (currentLevel);
		StartCoroutine(GetComponent<Fading>().LoadScene("scenes/afterlevelmenu"));
		return true;
	}

  // Tries to find the relevant obj in the stack at pos and removes it from the board
  public bool remove_from_board(GameObject obj, Vector3Int pos) {
    var stack = board[new Vector2Int(pos.x, pos.y)];
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

	public void pressedMenuButton() {
		menuPanel.SetActive (!menuPanel.activeSelf);
	}

	public void pressedRestartButton() {
		foreach(Vector2Int tilePosition in board.Keys) {
			foreach (GameObject gameObject in board[tilePosition]) {
				Destroy (gameObject);
			}
		}
		LoadLevel ();
		menuPanel.SetActive (false);
		Destroy (GameObject.FindGameObjectWithTag("Player"));
	}

	public void pressedLevelsButton() {
		StartCoroutine(GetComponent<Fading>().LoadScene("scenes/levelselectionmenu"));
	}

	public void pressedMenuCancelButton() {
		menuPanel.SetActive (!menuPanel.activeSelf);
	}

	public void pressedRewindButton() {
		if (moveHistory.Count == 0) {
			return;
		}
    const bool recordMove = false;
		List<BoardMove> boardMoves = moveHistory.Pop ();
		for (int i = 0; i < boardMoves.Count; i++) {
			BoardMove boardMove = boardMoves [i];
			moveObject (boardMove.movee, boardMove.to, boardMove.from, recordMove);
		}
	}
}
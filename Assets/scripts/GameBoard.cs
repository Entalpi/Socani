using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBoard : MonoBehaviour {
    // Time remaining on the level
    public Text timer_text;

	public TileMapping[] mappings;

	public Level curr_lvl;
	public Dictionary<Vector2Int, List<GameObject>> board;

	// Private
	private Vector2 tile_size = new Vector2 (1.25f, 1.25f);
    private bool countdown = false;
    // Plays the countdown sounds by increasing pitches
    private Coroutine countDownRoutine;

	void Start () {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 9, Screen.height / 5, 10f));
        LoadLevel();
	}

    public void LoadLevel() {
        curr_lvl = LevelManager.instance.getLevel();

        // Load the current level
        board = curr_lvl.load(this);
        timer_text.text = string.Format("{0:0.0}", curr_lvl.time);
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

    public void Update() {
        curr_lvl.time -= Time.deltaTime;
        curr_lvl.time = Mathf.Clamp(curr_lvl.time, 0f, Mathf.Infinity);
        timer_text.text = string.Format("{0:0.0}", curr_lvl.time);
        if (!countdown && curr_lvl.time <= 10) {
            countdown = true;
            countDownRoutine = StartCoroutine(CountDown(10));
        } 
        if (curr_lvl.time == 0) {
            Debug.Log("TIME IS UP!");
        }
    }

    private IEnumerator CountDown(int seconds) {
        for (int i = 0; i < seconds; i++) {
            Sound sound = AudioManager.instance.GetSound("time-beat");
            sound.source.pitch = 1.0f + i * 0.25f;
            AudioManager.instance.Play(sound);
            yield return new WaitForSeconds(1);
        }
        yield return null;
    }

    // Tries to move the object located at 'from' to 'to'
    private bool move_object(GameObject obj, Vector3Int from, Vector3Int to) {
		// Move the tiles in the board 
		Vector2Int board_key_from = new Vector2Int(from.x, from.y);
		Vector2Int board_key_to = new Vector2Int (to.x, to.y);

        // Search for the object in the tile stack
        for (int z = 0; z < board[board_key_from].Count; z++) {
            GameObject game_object = board[board_key_from][z];
            if (obj.Equals(game_object)) {
                board[board_key_from].RemoveAt(z);
                board[board_key_to].Add(obj);

                to.z = board[board_key_to].Count - 1; // Place objects on top of the tile
                Tile tile = obj.GetComponent<Tile>();
                if (tile) {
                    tile.board_position = to; // Update tile board position
                    // Start the transform movement
                    StartCoroutine(tile.smoothMovement(board_to_transform_position(to)));
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
		if (!board.ContainsKey (delta_key)) { 
			return false;
		} else {
            // Check for moveable tiles
            for (int z = 0; z < board[delta_key].Count; z++) {
                GameObject game_obj = board[delta_key][z];
                Tile tile = game_obj.GetComponent<Tile> ();
				if (tile) {
					if (tile.moveable) {
                        bool moveable = valid_move (game_obj, pos + delta, delta);
						if (moveable) {
                            // Move the object at (pos.x, pos.y, z) to (pos + delta)
                            return move_object(obj, pos, pos + delta);
						}
						return moveable;
					} else {
						return false;
					}
				}
			}
		}
        // Move the object at (pos) to the top of the stack at (pos + delta)
		return move_object(obj, pos, pos + delta);
	}

	// Takes a board position returns the world position
	public Vector3 board_to_transform_position(Vector3Int pos) {
        Vector3 origo = transform.position;
        return origo + new Vector3 (pos.x * tile_size.x, pos.y * tile_size.y, -pos.z);
	}

	// Take a world position and return the board position
	public Vector3Int transform_to_board_position(Vector3 pos) {
        Vector3 origo = transform.position;
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
        if (countDownRoutine != null) {
            StopCoroutine(countDownRoutine);
        }
        
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
}

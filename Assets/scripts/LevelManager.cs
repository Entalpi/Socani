using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelManager : MonoBehaviour {
    public static LevelManager instance;

    // All the levels in the game
    public Level[] levels;

    // The lvl that is going be to played by the GameBoard
    public Level currentLevel;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);

        // TODO: Load previous progress and set up the levels?
    }

    public Level getLevel() {
		if (currentLevel) {
			return currentLevel;
		} else {
			Debug.LogWarning ("Scene 'player' started without a current level");
			return null;
		}
    }
}

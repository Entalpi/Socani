using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
			return Instantiate(currentLevel);
		} else {
			Debug.LogWarning ("Scene 'player' started without a current level");
			return Instantiate(levels[0]);
		}
    }

	public bool nextLevel() {
		// Out of levels! End of game?
		if (levels.Length <= currentLevel.levelIndex + 1) {
			return false;
		}
		currentLevel = levels [currentLevel.levelIndex + 1];
		return true;
	}

	// Returns whether or not it found the level 
	public bool levelCompleted(Level level) {
		Level levelPrefab = Array.Find (levels, match => match.levelIndex == level.levelIndex);
		if (levelPrefab) {
			levelPrefab.completed = true;
			return true;
		}
		return false;
	}
}

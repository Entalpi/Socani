using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelManager : MonoBehaviour {
    public static LevelManager instance;

    // All the levels in the game
    public Level[] levels;

    // The index of the lvl that is going be to played by the GameBoard
    public int currentLevel;

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
        Level level = levels[currentLevel];
        if (level) {
            return level;
        } else {
            Debug.LogWarning("Current level index is invalid: " + currentLevel);
            return null;
        }
    }
}

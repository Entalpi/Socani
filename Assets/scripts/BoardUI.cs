using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Binds all the UI interaction with the GameBoard.cs
/// </summary>
public class BoardUI : MonoBehaviour {

  public GameBoard board;

  public Text levelNumber;
  public Text numberOfMoves;

  // Use this for initialization
  void Start() {
    levelNumber.text = "#" + (LevelManager.instance.currentLevel.levelIndex + 1);
  }

  public void setMovesCounter(int value = 1) {

  }
}
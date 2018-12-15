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

  // Rewind related
  public Image rewindHead1;
  public Image rewindHead2;
  public Image rewindHead3;
  public int numRewindsLeft = 3;

  // Use this for initialization
  void Start() {
    levelNumber.text = "#" + (LevelManager.instance.currentLevel.levelIndex + 1);
  }

  void Update() {
    numberOfMoves.text = "" + board.currentLevel.numberOfMoves;
  }

  public void pressedRewindButton() {
    if (numRewindsLeft == 0) { return; }
    switch (numRewindsLeft) {
      case 1:
        rewindHead1.gameObject.active = false;
        break;
      case 2:
        rewindHead2.gameObject.active = false;
        break;
      case 3:
        rewindHead3.gameObject.active = false;
        break;
    }
    numRewindsLeft--;
    board.pressedRewindButton();
  }
}
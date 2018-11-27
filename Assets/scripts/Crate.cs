using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour {
  private void OnCollisionEnter2D(Collision2D col) {
    GameBoard board = FindObjectOfType<GameBoard>();
    if (board) {
      // Play effects
      if (col.collider.tag == "Player") {
        AudioManager.instance.Play("coin-pickup");
      }
     }
  }
}

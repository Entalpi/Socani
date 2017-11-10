using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
    private void OnCollisionEnter2D(Collision2D col) {
        GameBoard board = FindObjectOfType<GameBoard>();
        if (board) {
            var position = board.transform_to_board_position(transform.position);
            if (board.remove_from_board(gameObject, position)) {
                // Play effects
                if (col.collider.tag == "Player") {
                    AudioManager.instance.Play("coin-pickup");
                }
                Destroy(gameObject);
            }
        }
    }
}

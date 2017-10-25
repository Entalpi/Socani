using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour {
	public Player player;
	void Start () {
		Instantiate (player, transform.position, Quaternion.identity);
	}
}

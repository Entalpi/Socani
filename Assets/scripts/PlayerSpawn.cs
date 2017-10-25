using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour {
	public Player player;
	void Start () {
		player.transform.position = transform.position;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour {
	public Player player;
	void Start () {
        Vector3 delta = new Vector3(0f, 0f, -1.0f);
		Instantiate (player, transform.position + delta, Quaternion.identity);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour {
	public Player player;
	void Start () {
    Vector3 delta = new Vector3(0f, 0f, -1.0f);
    Player obj = Instantiate(player, transform.position + delta, Quaternion.identity);
    obj.transform.SetParent(this.transform.parent);
  }
}

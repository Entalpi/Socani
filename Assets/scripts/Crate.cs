using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour {
	void Start () {
		GetComponent<Rigidbody2D> ().freezeRotation = true;
	}
}

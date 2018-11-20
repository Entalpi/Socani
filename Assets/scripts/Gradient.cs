using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gradient : MonoBehaviour {

  public Color startColor = Color.white;
  public Color endColor   = Color.black;

	void Start () {
    MeshFilter meshFilter = GetComponent<MeshFilter>();
    if (meshFilter) {
      Debug.Log(meshFilter.mesh.vertices.Length);
      Debug.Log(meshFilter.mesh.colors.Length);
      meshFilter.mesh.colors = new Color[] { startColor, startColor, endColor, endColor };
    }
	}
}

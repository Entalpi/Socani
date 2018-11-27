using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
	public Vector3Int boardPosition;
	public bool moveable = true;
	public float inverseMoveTime = 1.0f / 0.1f;

  const float pixelsPerUnit = 100.0f; // Number of pixels per Unity unit
  public static readonly Vector2 OriginalSize = new Vector2(128.0f / pixelsPerUnit, 128.0f / pixelsPerUnit);
  public static Vector2 Size = OriginalSize;

  public IEnumerator smoothMovement(Vector3 end) {
		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
		while (sqrRemainingDistance > float.Epsilon) {
			Vector3 newPosition = Vector3.MoveTowards(transform.position, end, inverseMoveTime * Time.deltaTime);
			transform.position = newPosition;
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			yield return null;
		}
	}
}

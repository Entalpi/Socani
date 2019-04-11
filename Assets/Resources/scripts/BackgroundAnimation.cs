using System.Collections.Generic;
using UnityEngine;

public class BackgroundAnimation : MonoBehaviour {

  public Texture2D tile; // Tile to spawn 
  public int numberTiles = 8; // Number of tiles to spawn 
  private int currentTiles = 0; // Number of currently spawned tiles

  private List<GameObject> spawnedTiles = new List<GameObject>();

  void Start() {
    float width = Mathf.Abs(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 100.0f, 0.0f, 0.0f)).x);
    for (int i = 0; i < numberTiles; i++) {
      Invoke("spawnTile", Random.Range(0.0f, width)); // FIXME: width? As time?
    }
  }

  void Update() {
    foreach (GameObject obj in spawnedTiles) {
      if (obj.transform.position.y < -10) {
        reset(obj);
      }
    }
  }

  void reset(GameObject obj) {
    Vector3 randomPosition = new Vector3(Random.Range(-4.0f, 4.0f), 5.0f, 100.0f);
    obj.transform.position = randomPosition;
    obj.GetComponent<Rigidbody2D>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
    float scale = Random.Range(0.75f, 1.0f);
    obj.transform.localScale = new Vector3(scale, scale, 1.0f);
  }

  void spawnTile() {
    if (currentTiles > numberTiles) { return; }
    currentTiles++;

    GameObject obj = new GameObject();
    obj.name = "Falling brick";

    SpriteRenderer spriteRenderer = obj.AddComponent<SpriteRenderer>() as SpriteRenderer;
    spriteRenderer.sprite = Sprite.Create(tile, new Rect(0, 0, tile.width, tile.height), new Vector2(0.0f, 0.0f));
    Rigidbody2D rb2d = obj.AddComponent<Rigidbody2D>() as Rigidbody2D;
    rb2d.freezeRotation = true;
    reset(obj);
    spawnedTiles.Add(obj);
  }
}

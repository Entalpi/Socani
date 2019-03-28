using System.Collections.Generic;
using UnityEngine;

struct Cloud {
  public GameObject obj;
  public float speed;
}

public class CloudAnimation : MonoBehaviour {

  public List<Texture2D> sprites;

  public int windDirection = -1;

  public int maxDistance = 100;

  public int cloudsToSpawn = 8;

  private List<Cloud> clouds = new List<Cloud>();

  private void Start() {
    if (Random.Range(-1.0f, 1.0f) < 0.0f) {
      windDirection = -1;
    } else {
      windDirection = 1;
    }
    
    for (int i = 0; i < cloudsToSpawn; i++) {
      Invoke("spawnCloud", Random.Range(0.5f, 10.0f));
    }
  }

  private void Update() {
    const float waveStrength = 0.02f;
    for (int i = 0; i < clouds.Count; i++) {
      float x = clouds[i].obj.transform.position.x + clouds[i].speed;
      float y = clouds[i].obj.transform.position.y + waveStrength * Mathf.Sin(Time.time);
      clouds[i].obj.transform.position = new Vector3(x, y, clouds[i].obj.transform.position.z);
      if (Mathf.Abs(clouds[i].obj.transform.position.x) < maxDistance) {
        reset(clouds[i]);
      }
    }
  }

  private void reset(Cloud cloud) {

  }

  private void spawnCloud() {
    Cloud cloud = new Cloud();
    cloud.speed = Random.Range(0.05f, 0.2f);

    GameObject obj = new GameObject();
    obj.name = "Cloud";
    float scale = Random.Range(0.75f, 1.0f);
    obj.transform.localScale = new Vector3(scale, scale, 1.0f);

    // obj.transform.position = new Vector3(...);
    Vector3 randomPosition = new Vector3(5.0f, Random.Range(-4.0f, 4.0f), 100.0f);
    obj.transform.position = randomPosition;

    Texture2D texture = sprites[(int)Random.Range(0.0f, sprites.Count - 1)];

    SpriteRenderer spriteRenderer = obj.AddComponent<SpriteRenderer>() as SpriteRenderer;
    spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.0f, 0.0f));

    cloud.obj = obj;
    clouds.Add(cloud);
  }
}

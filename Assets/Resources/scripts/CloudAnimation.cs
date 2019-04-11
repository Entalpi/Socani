using System.Collections.Generic;
using UnityEngine;

class Cloud {
  public GameObject obj;
  public float speed;
  public float time;
  public float waveStrength;
  public float dt;
}

public class CloudAnimation : MonoBehaviour {

  public List<Texture2D> sprites;

  public int windDirection = 1;

  public float maxDistance = 8.0f;

  public int cloudsToSpawn = 8;

  private List<Cloud> clouds = new List<Cloud>();

  private void Start() {
    if (Random.Range(-1.0f, 1.0f) < 0.0f) {
      windDirection = -1;
    } else {
      windDirection = 1;
    }

    windDirection = 1;


    for (int i = 0; i < cloudsToSpawn; i++) {
      Invoke("spawnCloud", Random.Range(0.5f, 10.0f));
    }
  }

  private void Update() {
    for (int i = 0; i < clouds.Count; i++) {
      clouds[i].time += clouds[i].dt;
      float x = clouds[i].obj.transform.position.x + clouds[i].speed * windDirection;
      float y = clouds[i].obj.transform.position.y + clouds[i].waveStrength * Mathf.Sin(clouds[i].time);
      clouds[i].obj.transform.position = new Vector3(x, y, clouds[i].obj.transform.position.z);
      if (Mathf.Abs(clouds[i].obj.transform.position.x) >= maxDistance) {
        reset(clouds[i]);
      }
    }
  }

  private void reset(Cloud cloud) {
    cloud.time = Random.Range(0.0f, 1000.0f);
    cloud.speed = Random.Range(0.020f, 0.002f);
    cloud.waveStrength = Random.Range(0.005f, 0.01f);
    cloud.dt = Random.Range(0.005f, 0.01f);

    float scale = Random.Range(0.25f, 0.70f);
    cloud.obj.transform.localScale = new Vector3(scale, scale, 1.0f);

    Vector3 randomPosition = new Vector3(maxDistance * -windDirection, 5.0f + Random.Range(-2.0f, 2.0f), 100.0f);
    cloud.obj.transform.position = randomPosition;

    Texture2D texture = sprites[(int)Random.Range(0.0f, sprites.Count - 1)];

    SpriteRenderer spriteRenderer = cloud.obj.GetComponent<SpriteRenderer>();
    spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.0f, 0.0f));
  }

  private void spawnCloud() {
    Cloud cloud = new Cloud();
    cloud.time = Random.Range(0.0f, 1000.0f);
    cloud.speed = Random.Range(0.020f, 0.002f);
    cloud.waveStrength = Random.Range(0.005f, 0.01f);
    cloud.dt = Random.Range(0.005f, 0.01f);

    GameObject obj = new GameObject();
    obj.name = "Cloud";
    float scale = Random.Range(0.25f, 0.70f);
    obj.transform.localScale = new Vector3(scale, scale, 1.0f);

    Vector3 randomPosition = new Vector3(maxDistance * -windDirection, 5.0f + Random.Range(-2.0f, 2.0f), 100.0f);
    obj.transform.position = randomPosition;

    Texture2D texture = sprites[(int)Random.Range(0.0f, sprites.Count - 1)];
    SpriteRenderer spriteRenderer = obj.AddComponent<SpriteRenderer>() as SpriteRenderer;
    spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.0f, 0.0f));

    cloud.obj = obj;
    clouds.Add(cloud);
  }
}

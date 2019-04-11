using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fading : MonoBehaviour {

	public Texture2D fadeOutTexture;
	public float fadeSpeed = 0.75f;

	private int drawDepth = -1000;
	private float alpha = 1.0f;
	private int fadeDir = -1;

	void Start() {
		SceneManager.sceneLoaded += sceneLoaded;
	}

	void OnGUI() {
		alpha += fadeDir * fadeSpeed * Time.deltaTime;
		alpha = Mathf.Clamp(alpha, 0.0f, 1.0f);
		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;
		GUI.DrawTexture(new Rect (0, 0, Screen.width, Screen.height), fadeOutTexture);
	}

	public IEnumerator LoadScene(string name) {
		float fadeTime = BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
		SceneManager.LoadScene(name);
	}

	private float BeginFade(int direction) {
		fadeDir = direction;
		return fadeSpeed;
	}

	void sceneLoaded(Scene scene, LoadSceneMode mode) {
		BeginFade(-1); // Fade in
	}
}

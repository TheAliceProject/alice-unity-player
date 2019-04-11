using UnityEngine;
using System.Collections;

public class GameOverControl : MonoBehaviour {
	
	public GameObject explosion;
	
	IEnumerator Start () {
		RenderSettings.fog = true;
		RenderSettings.fogColor = Camera.main.backgroundColor;
		RenderSettings.fogMode = FogMode.Linear;
		RenderSettings.fogEndDistance = 20.0f;
	
		while (true) {
			var gameOverText = FlyingText.GetObjects ("GAME<br>OVER").transform;
			var pos = gameOverText.position;
			pos.z = -6.5f;
			gameOverText.position = pos;
			var rigidbodies = gameOverText.GetComponentsInChildren<Rigidbody>();
			foreach (var rb in rigidbodies) {
				rb.useGravity = false;
			}
			
			for (float i = 0.0f; i < 1.0f; i += Time.deltaTime) {
				pos = gameOverText.position;
				pos.y = Mathf.Lerp (5.0f, -.05f, i);
				gameOverText.position = pos;
				yield return null;
			}
			StartCoroutine (CameraShake (Camera.main));
			
			yield return new WaitForSeconds(1.75f);
			
			Instantiate (explosion, new Vector3(0.0f, 1.0f, -6.3f), Quaternion.identity);
			foreach (var rb in rigidbodies) {
				rb.useGravity = true;
				rb.AddExplosionForce (220.0f, new Vector3(0, 1, -6.5f), 10.0f, 9.0f);
			}
			
			yield return new WaitForSeconds(5.0f);
			Destroy (gameOverText.gameObject);
			yield return new WaitForSeconds(1.0f);
		}
	}
	
	public float startingShakeDistance = .4f;
	public float decreasePercentage = .5f;
	public float shakeSpeed = 40.0f;
	public int numberOfShakes = 3;
	
	IEnumerator CameraShake (Camera cam) {
		var originalPosition = cam.transform.localPosition.y;
		var shakeCounter = numberOfShakes;
		var shakeDistance = startingShakeDistance;
		var timer = 0.0f;
	
		while (shakeCounter > 0) {
			var pos = cam.transform.localPosition;
			pos.y = originalPosition + Mathf.Sin (timer) * shakeDistance;
			cam.transform.localPosition = pos;
			timer += Time.deltaTime * shakeSpeed;
			if (timer > Mathf.PI * 2.0f) {
				timer = 0.0f;
				shakeDistance *= decreasePercentage;
				shakeCounter--;
			}
			yield return null;
		}
		var pos2 = cam.transform.localPosition;
		pos2.y = originalPosition;
		cam.transform.localPosition = pos2;
	}
}
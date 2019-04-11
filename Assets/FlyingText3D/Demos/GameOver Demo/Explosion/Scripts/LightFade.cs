// Fades light over time
using UnityEngine;

public class LightFade : MonoBehaviour {
	
	public float lightIntensity = 2.0f;
	public float fadeSpeed = 1.0f;
	
	void Update () {
		lightIntensity = Mathf.Max (lightIntensity - Time.deltaTime*fadeSpeed, 0.0f);
		GetComponent<Light>().intensity = lightIntensity;
	}
}
using UnityEngine;
using System.Collections;

public class MenuObject : MonoBehaviour {
	
	public GameObject highlight;
	public float rotateSpeed;
	public Transform menuObject;
	
	void OnMouseOver () {
		// Activate highlight and rotate the text object
		if (MenuControl.clicked) return;
		menuObject.Rotate (Vector3.right * Time.deltaTime * rotateSpeed);
#if UNITY_3_4 || UNITY_3_5
		highlight.active = true;
#else
		highlight.SetActive (true);
#endif
		var pos = highlight.transform.position;
		pos.y = transform.position.y;
		highlight.transform.position = pos;
	}
	
	void OnMouseExit () {
		// Deactivate highlight and reset rotation
		if (MenuControl.clicked) return;
#if UNITY_3_4 || UNITY_3_5
		highlight.active = true;
#else
		highlight.SetActive (false);
#endif
		menuObject.eulerAngles = Vector3.zero;
	}
	
	IEnumerator OnMouseDown () {
		if (MenuControl.clicked) yield break;
		MenuControl.clicked = true;
#if UNITY_3_4 || UNITY_3_5
		highlight.active = true;
#else
		highlight.SetActive (false);
#endif
		menuObject.eulerAngles = Vector3.zero;
		
		// Move object toward camera and change color
		var startPos = transform.position;
		var endPos = new Vector3(0, -2, -16);
		var endColor = Camera.main.backgroundColor;
		for (float i = 0.0f; i <= 1.0f; i += Time.deltaTime * 2.0f) {
			menuObject.transform.position = Vector3.Lerp (startPos, endPos, i);
			menuObject.GetComponent<Renderer>().material.color = Color.Lerp (Color.white, endColor, i);
			yield return null;
		}
		
		// Reset back to start after a bit
		yield return new WaitForSeconds (1.5f);
		menuObject.transform.position = startPos;
		menuObject.GetComponent<Renderer>().material.color = Color.white;
		MenuControl.clicked = false;
	}
}
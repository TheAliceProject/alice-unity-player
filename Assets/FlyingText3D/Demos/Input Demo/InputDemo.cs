using UnityEngine;
using System.Collections;

public class InputDemo : MonoBehaviour {

	private GameObject textObject;
	private string enteredText;
	private char cursorChar = '-';
	private bool acceptInput;
	
	void Start () {
		InitializeText();
	}
	
	void InitializeText () {
		FlyingText.addRigidbodies = false;
		enteredText = "";
		acceptInput = true;
		textObject = FlyingText.GetObject ("-", new Vector3(-7, 6, 0), Quaternion.identity);
		InvokeRepeating ("BlinkCursor", .5f, .5f);
	}
	
	void OnGUI () {
		if (!acceptInput) return;
		
		GUI.Label (new Rect(10, 10, 500, 30), "Type some text! Hit return when done.");
	}
	
	void Update () {
		if (!acceptInput) return;
		
		foreach (var c in Input.inputString) {
			if (c == '\b') {
				if (enteredText.Length > 0) {
					enteredText = enteredText.Substring (0, enteredText.Length - 1);
				}
			}
			else if (c == '\n' || c == '\r') {
				if (enteredText.Length > 0) {
					StartCoroutine (ExplodeText());
				}
			}
			else if (c == '<' || c == '>') {
				// do nothing
			}
			else {
				enteredText += c;
			}
			FlyingText.UpdateObject (textObject, enteredText + cursorChar);
		}
	}
	
	IEnumerator ExplodeText () {
		acceptInput = false;
		CancelInvoke ("BlinkCursor");
		Destroy (textObject);
		FlyingText.addRigidbodies = true;
		var letters = FlyingText.GetObjectsArray (enteredText, new Vector3(-7, 6, 0), Quaternion.identity);
		foreach (var letter in letters) {
			letter.GetComponent<Rigidbody>().useGravity = false;
			letter.GetComponent<Rigidbody>().AddTorque (new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * 10.0f);
			letter.GetComponent<Rigidbody>().AddExplosionForce (390.0f, new Vector3(0, 1, 11), 15.0f);
		}
		
		yield return new WaitForSeconds (5);
		foreach (var letter in letters) {
			Destroy (letter);
		}
		InitializeText();
	}
	
	void BlinkCursor () {
		if (cursorChar == '-') {
			cursorChar = ' ';
		}
		else {
			cursorChar = '-';
		}
		FlyingText.UpdateObject (textObject, enteredText + cursorChar);
	}
}
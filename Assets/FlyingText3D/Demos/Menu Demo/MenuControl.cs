using UnityEngine;

public class MenuControl : MonoBehaviour {
	
	public GameObject highlight;
	public float rotateSpeed = 150.0f;
	public string[] itemText = new string[]{"<color=#13b62a>Start", "<color=#235fb9>Options", "<color=#cb3125>Quit"};
	public Vector3[] itemPositions = new Vector3[]{new Vector3(0, 4, 0), new Vector3(0, 0, 0), new Vector3(0, -4, 0)};
	public static bool clicked = false;
	
	void Start () {
		for (int i = 0; i < itemText.Length; i++) {
			// Create 3D text object
			var menuObject = FlyingText.GetObject (itemText[i]);
			menuObject.transform.position = itemPositions[i];
			
			// Create collider (we don't use a collider for the 3D text object because we don't want the collider to rotate)
			var cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
			cube.transform.localScale = new Vector3(7, 1.8f, .1f);
			cube.transform.position = itemPositions[i];
			cube.GetComponent<Renderer>().enabled = false;
			
			// Add the menu object script to the collider and assign values to the variables
			var menuScript = cube.AddComponent<MenuObject>();
			menuScript.highlight = highlight;
			menuScript.rotateSpeed = rotateSpeed;
			menuScript.menuObject = menuObject.transform;
		}
	}
}
using UnityEngine;
using UnityEngine.UI;

public class UI_JsonParser : MonoBehaviour
{
	[SerializeField] private Text text;

	private JsonParser parser;

	private void Start ()
	{
		parser = new JsonParser ();
	}

	public void ToJsonExample ()
	{
		text.text = parser.ToJsonExample();
	}

	public void FromJson ()
	{
		text.text = parser.FromJson();
	}
}

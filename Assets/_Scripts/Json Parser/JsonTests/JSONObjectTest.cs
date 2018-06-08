using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONObjectTest : MonoBehaviour {

	[SerializeField] private string encodedString = "{\"implementationBrief\": [\"vmPrimitive\", [\"playAudio:\", [\"audioSource\"]]]}";

	private void Start()
	{
		JSONObject j = new JSONObject(encodedString);
		accessData(j);
	}

	private void accessData(JSONObject obj)
	{
		switch (obj.type)
		{
			case JSONObject.Type.OBJECT:
				for (int i = 0; i < obj.list.Count; i++)
				{
					string key = (string)obj.keys[i];
					JSONObject j = (JSONObject)obj.list[i];
					Debug.Log(key);
					accessData(j);
				}
				break;
			case JSONObject.Type.ARRAY:
				foreach (JSONObject j in obj.list)
				{
					accessData(j);
				}
				break;
			case JSONObject.Type.STRING:
				Debug.Log(obj.str);
				break;
			case JSONObject.Type.NUMBER:
				Debug.Log(obj.n);
				break;
			case JSONObject.Type.BOOL:
				Debug.Log(obj.b);
				break;
			case JSONObject.Type.NULL:
				Debug.Log("NULL");
				break;
		}
	}
}

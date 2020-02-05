//#define PERFTEST        //For testing performance of parse/stringify.  Turn on editor profiling to see how we're doing

using UnityEngine;
using UnityEditor;

public class JSONChecker : EditorWindow {
	string JSON = @"{
	""TestObject"": {
		""SomeText"": ""Blah"",
		""SomeObject"": {
			""SomeNumber"": 42,
			""SomeFloat"": 13.37,
			""SomeBool"": true,
			""SomeNull"": null
		},
		
		""SomeEmptyObject"": { },
		""SomeEmptyArray"": [ ],
		""EmbeddedObject"": ""{\""field\"":\""Value with \\\""escaped quotes\\\""\""}""
	}
}";	  //dat string literal...
	string URL = "";
	JSONObject j;
	[MenuItem("Window/JSONChecker")]
	static void Init() {
		GetWindow(typeof(JSONChecker));
	}
	void OnGUI() {
		JSON = EditorGUILayout.TextArea(JSON);
		GUI.enabled = !string.IsNullOrEmpty(JSON);
		if(GUILayout.Button("Check JSON")) {
#if PERFTEST
            Profiler.BeginSample("JSONParse");
			j = JSONObject.Create(JSON);
            Profiler.EndSample();
            Profiler.BeginSample("JSONStringify");
            j.ToString(true);
            Profiler.EndSample();
#else
			j = JSONObject.Create(JSON);
#endif
			Debug.Log(j.ToString(true));
		}
		EditorGUILayout.Separator();
		URL = EditorGUILayout.TextField("URL", URL);
		if (GUILayout.Button("Get JSON")) {
			Debug.Log(URL);
			#if UNITY_2018_3_OR_NEWER
			UnityEngine.Networking.UnityWebRequest test = UnityEngine.Networking.UnityWebRequest.Get(URL);
			#else
			WWW test = new WWW(URL);
			#endif
			while (!test.isDone) ;
			if (!string.IsNullOrEmpty(test.error)) {
				Debug.Log(test.error);
			} else {
				#if UNITY_2018_3_OR_NEWER
				Debug.Log(test.downloadHandler.text);
				j = new JSONObject(test.downloadHandler.text);
				#else
				Debug.Log(test.text);
				j = new JSONObject(test.text);
				#endif
				Debug.Log(j.ToString(true));
			}

		}
		if(j) {
			//Debug.Log(System.GC.GetTotalMemory(false) + "");
			if(j.type == JSONObject.Type.NULL)
				GUILayout.Label("JSON fail:\n" + j.ToString(true));
			else
				GUILayout.Label("JSON success:\n" + j.ToString(true));

		}
	}
}

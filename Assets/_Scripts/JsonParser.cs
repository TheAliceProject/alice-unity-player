using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JsonParser
{

	public string ToJsonExample ()
	{
        ExampleClass obj = new ExampleClass ();
		obj.level = 1;
		obj.timeElapsed = 0.5f;
		obj.playerName = "Name";
	
		string json = JsonUtility.ToJson(obj);

		string extensions = "txt";
		string path = Crosstales.FB.FileBrowser.SaveFile("Save File", "", "ToJsonExample", extensions);
		System.IO.File.WriteAllText(path, json);

		return "Saved Object to Json file: " + path;
	}

	public string FromJson ()
	{
		//Debug.Log("OpenSingleFile");
		/*
            var extensions = new[] {
                new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),
                new ExtensionFilter("Sound Files", "mp3", "wav" ),
                new ExtensionFilter("All Files", "*" ),
            };
            */
		string extensions = "";
		string path = Crosstales.FB.FileBrowser.OpenSingleFile("Open File", "", extensions);
		//Debug.Log("Selected file: " + path);

		string json = System.IO.File.ReadAllText(path);
		ExampleClass obj = JsonUtility.FromJson<ExampleClass>(json);

		return "Loaded in Json file, stored in object: " + obj.ToString();
	}
}

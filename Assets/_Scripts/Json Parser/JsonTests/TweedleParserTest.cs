using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweedleParserTest : MonoBehaviour {

	void Start () {
		string extensions = "";
		string file = Crosstales.FB.FileBrowser.OpenSingleFile("Open File", "", extensions);
		string jsonStr = System.IO.File.ReadAllText(file);
		int index = file.LastIndexOf("/");
		string path = file.Substring(0, index + 1);
		
		Alice.Tweedle.Unlinked.JsonUnlinkedParser parser = new Alice.Tweedle.Unlinked.JsonUnlinkedParser(path);
		parser.ParseFile(jsonStr);
	}
}

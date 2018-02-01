using UnityEngine;
using UnityEngine.UI;

public class JsonParser : MonoBehaviour
{
    [SerializeField] private Text text;

    #region JSON_FILE

    public void ToJsonFileUI()
    {
        ExampleClass obj = new ExampleClass();
        obj.level = 1;
        obj.timeElapsed = 0.5f;
        obj.playerName = "Name";

        string json = JsonUtility.ToJson(obj);

        string extensions = "txt";
        string path = Crosstales.FB.FileBrowser.SaveFile("Save File", "", "ToJsonExample", extensions);
        System.IO.File.WriteAllText(path, json);

        text.text = "Saved Object to Json file: " + path;
    }

    public void FromJsonFileUI()
    {
        string extensions = "";
        string path = Crosstales.FB.FileBrowser.OpenSingleFile("Open File", "", extensions);

        text.text = FromJsonFile(path);
    }

    #endregion

    #region JSON_ZIP

    public void ToJsonZipUI()
    {
        ExampleClass obj = new ExampleClass();
        obj.level = 1;
        obj.timeElapsed = 0.5f;
        obj.playerName = "Name";

        string json = JsonUtility.ToJson(obj);

        string fileName = "ToJsonExample";
        string extensions = "txt";
        string path = Crosstales.FB.FileBrowser.SaveFile("Save File", "", fileName, extensions);
        System.IO.File.WriteAllText(path, json);

        // SAVE TO FOLDER
        UniZip.Compress(@"c:\Users\jacobwil\Downloads\ToJsonExample");
        //UniZip.CompressDirectory(path, "", (file) => { text.text = "Compressing {0}..." + file; });
    }

    public void FromJsonZipUI()
    {
        string extensions = "";
        string path = Crosstales.FB.FileBrowser.OpenSingleFile("Open File", "", extensions);
        string dst = path.Substring(0, path.Length - 4);
        UniZip.DecompressFolder(path);
        //UniZip.DecompressToDirectory(path, "", (fileName) => { text.text = "Decompressing {0}..." + fileName; });

    }

    #endregion 

    public string FromJsonFile (string path)
	{

		string json = System.IO.File.ReadAllText(path);
		ExampleClass obj = JsonUtility.FromJson<ExampleClass>(json);

		return "Loaded in Json file, stored in object: " + obj.ObjToString();
	}
}

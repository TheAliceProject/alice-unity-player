using UnityEngine;
using UnityEngine.UI;
using System.IO;

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

    private static string json_ext = "json";
    private static string zip_ext = "zip";

    public void ToJsonZipUI()
    {
        ExampleClass obj = new ExampleClass();
        obj.level = 1;
        obj.timeElapsed = 0.5f;
        obj.playerName = "Name";

        string json = JsonUtility.ToJson(obj);

        string fileName = "ToJsonExample";
        string filePath = Crosstales.FB.FileBrowser.SaveFile("Save File", "", fileName, json_ext);
        string folderPath = filePath.Substring(0, filePath.Length - json_ext.Length - 1);

        folderPath = UniqueDirectoryName(folderPath);
        Directory.CreateDirectory(folderPath);
        File.WriteAllText(System.IO.Path.Combine(folderPath, fileName + "." + json_ext), json);

        string zipPath = folderPath + "." + zip_ext;
        System.IO.Compression.ZipFile.CreateFromDirectory(folderPath, zipPath);
    }

    public void FromJsonZipUI()
    {
        string zipPath = Crosstales.FB.FileBrowser.OpenSingleFile("Open File", "", zip_ext);
        string extractPath = zipPath.Substring(0, zipPath.Length - zip_ext.Length - 1);
        extractPath = UniqueDirectoryName(extractPath);
        System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, extractPath);
        DirectoryInfo dir = new DirectoryInfo(extractPath);

        text.text = "";

        FileInfo[] files = dir.GetFiles("*." + json_ext);
        for (int i = 0; i < files.Length; i++)
        {
			text.text += FromJsonFile(files[i].FullName);
        }
    }

    private string UniqueDirectoryName(string path)
    {
        if (!Directory.Exists(path))
        {
            return path;
        }

        int folder_num = 1;
        while (Directory.Exists(path + " (" + folder_num + ")"))
        {
            folder_num++;
        }
        return path + " (" + folder_num + ")";
    }

    #endregion 

    public string FromJsonFile (string path)
	{
		string json = System.IO.File.ReadAllText(path);
        ExampleClass obj = JsonUtility.FromJson<ExampleClass>(json);

        return "Loaded in Json file, stored in object: " + obj.ObjToString();
	}

	public static void SetValue(Object obj, string variable, object value)
	{
		try
		{
			obj.GetType().GetField(variable, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(obj, value);
		} catch (System.Exception e)
		{
			return;
		}
	}
}
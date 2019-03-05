using UnityEngine;
using UnityEngine.UI;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;

public class JsonParser : MonoBehaviour
{
	[SerializeField] private Text text = null;

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
		ZipDirectory(zipPath, folderPath);
	}

	public void FromJsonZipUI()
	{
		string zipPath = Crosstales.FB.FileBrowser.OpenSingleFile("Open File", "", zip_ext);
		string extractPath = zipPath.Substring(0, zipPath.Length - zip_ext.Length - 1);
		extractPath = UniqueDirectoryName(extractPath);
		ExtractZipFile(zipPath, extractPath);
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

	#region Sharp Zip wrapper
	// The functions here are slightly trimmed versions of the samples from
	// https://github.com/icsharpcode/SharpZipLib/wiki/Zip-Samples#unpack-a-zip-with-full-control-over-the-operation

	public void ZipDirectory(string outPathname, string folderName)
	{
		FileStream fsOut = File.Create(outPathname);
		ZipOutputStream zipStream = new ZipOutputStream(fsOut);

		// This setting will strip the leading part of the folder path in the entries, to
		// make the entries relative to the starting folder.
		// To include the full path for each entry up to the drive root, assign folderOffset = 0.
		int folderOffset = folderName.Length + (folderName.EndsWith("\\", System.StringComparison.Ordinal) ? 0 : 1);

		CompressFolder(folderName, zipStream, folderOffset);

		zipStream.IsStreamOwner = true; // Makes the Close also Close the underlying stream
		zipStream.Close();
	}

	private void CompressFolder(string path, ZipOutputStream zipStream, int folderOffset)
	{
		foreach (string filename in Directory.GetFiles(path))
		{
			FileInfo fi = new FileInfo(filename);

			string entryName = filename.Substring(folderOffset); // Makes the name in zip based on the folder
			entryName = ZipEntry.CleanName(entryName); // Removes drive from name and fixes slash direction
			ZipEntry newEntry = new ZipEntry(entryName);
			newEntry.DateTime = fi.LastWriteTime; // Note the zip format stores 2 second granularity

			// To permit the zip to be unpacked by built-in extractor in WinXP and Server2003, WinZip 8, Java, and other older code,
			// you need to do one of the following: Specify UseZip64.Off, or set the Size.
			// If the file may be bigger than 4GB, or you do not need WinXP built-in compatibility, you do not need either,
			// but the zip will be in Zip64 format which not all utilities can understand.
			//   zipStream.UseZip64 = UseZip64.Off;
			newEntry.Size = fi.Length;

			zipStream.PutNextEntry(newEntry);

			// Zip the file in buffered chunks
			// the "using" will close the stream even if an exception occurs
			byte[] buffer = new byte[4096];
			using (FileStream streamReader = File.OpenRead(filename))
			{
				StreamUtils.Copy(streamReader, zipStream, buffer);
			}
			zipStream.CloseEntry();
		}
		foreach (string folder in Directory.GetDirectories(path))
		{
			CompressFolder(folder, zipStream, folderOffset);
		}
	}

	public void ExtractZipFile(string archiveFilenameIn, string outFolder)
	{
		ZipFile zf = null;
		try
		{
			FileStream fs = File.OpenRead(archiveFilenameIn);
			zf = new ZipFile(fs);
			foreach (ZipEntry zipEntry in zf)
			{
				if (!zipEntry.IsFile)
				{
					continue;           // Ignore directories
				}
				byte[] buffer = new byte[4096];     // 4K is optimum
				Stream zipStream = zf.GetInputStream(zipEntry);

				string fullZipToPath = Path.Combine(outFolder, zipEntry.Name);
				string directoryName = Path.GetDirectoryName(fullZipToPath);
				if (directoryName.Length > 0)
					Directory.CreateDirectory(directoryName);

				// Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
				// of the file, but does not waste memory.
				// The "using" will close the stream even if an exception occurs.
				using (FileStream streamWriter = File.Create(fullZipToPath))
				{
					StreamUtils.Copy(zipStream, streamWriter, buffer);
				}
			}
		}
		finally
		{
			if (zf != null)
			{
				zf.IsStreamOwner = true; // Makes close also shut the underlying stream
				zf.Close(); // Ensure we release resources
			}
		}
	}

	#endregion

	public string FromJsonFile(string path)
	{
		string json = System.IO.File.ReadAllText(path);
		ExampleClass obj = JsonUtility.FromJson<ExampleClass>(json);

		return "Loaded in Json file, stored in object: " + obj.ObjToString();
	}
}
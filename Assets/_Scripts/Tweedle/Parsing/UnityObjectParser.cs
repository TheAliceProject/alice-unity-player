using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Alice.Tweedle.Parsed
{
	public class UnityObjectParser : MonoBehaviour {

		public bool uniqueDirectory = true;

		private string root;

		private static string json_ext = "json";
		private static string project_ext = "a3p";
		private static string root_file = "manifest";

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

		public void Select()
		{
			string zipPath = Crosstales.FB.FileBrowser.OpenSingleFile("Open File", "", project_ext);
			string extractPath = zipPath.Substring(0, zipPath.Length - project_ext.Length - 1);
			if (uniqueDirectory)
			{
				extractPath = UniqueDirectoryName(extractPath);
			}

			System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, extractPath);
			DirectoryInfo dir = new DirectoryInfo(extractPath);
			
			FileInfo[] files = dir.GetFiles("*." + json_ext);
			string manifestFile = root_file + "." + json_ext;
			for (int i = 0; i < files.Length; i++)
			{
				if (files[i].Name == manifestFile)
				{
					JsonParser parse = new JsonParser(files[i].DirectoryName);
					parse.ParseJson(System.IO.File.ReadAllText(files[i].FullName));
					break;
				}
			}
			dir.Delete(true);
		}
	}
}
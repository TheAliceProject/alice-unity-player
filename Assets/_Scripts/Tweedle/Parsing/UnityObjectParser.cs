using System.IO;
using UnityEngine;

using ICSharpCode.SharpZipLib.Zip;


namespace Alice.Tweedle.Parsed
{
	public class UnityObjectParser : MonoBehaviour
	{
		static string project_ext = "a3p";

		public void Select()
		{
			string zipPath = Crosstales.FB.FileBrowser.OpenSingleFile("Open File", "", project_ext);
			if (System.IO.File.Exists(zipPath) == false)
				return;
			using (FileStream fileStream = new FileStream(zipPath, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				using (ZipFile zipFile = new ZipFile(fileStream))
				{
					TweedleSystem sys = new TweedleSystem();
					JsonParser reader = new JsonParser(sys, Path.GetFileNameWithoutExtension(fileStream.Name), zipFile);
					reader.Parse();
					// TODO store the TweedleSystem
				}
			}
		}
	}
}
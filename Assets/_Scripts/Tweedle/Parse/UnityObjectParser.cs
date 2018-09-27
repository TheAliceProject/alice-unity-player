using System.IO;
using System.Collections;
using UnityEngine;

using ICSharpCode.SharpZipLib.Zip;
using Alice.Tweedle.VM;

namespace Alice.Tweedle.Parse
{
	public class UnityObjectParser : MonoBehaviour
	{
		const string project_ext = "a3p";

		VirtualMachine vm;

		public void Select()
		{
			string zipPath = Crosstales.FB.FileBrowser.OpenSingleFile("Open File", "", project_ext);
			if (System.IO.File.Exists(zipPath) == false)
				return;
			TweedleSystem sys = new TweedleSystem();
			using (FileStream fileStream = new FileStream(zipPath, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				using (ZipFile zipFile = new ZipFile(fileStream))
				{
					JsonParser reader = new JsonParser(sys, zipFile);
					reader.Parse();
					// TODO store the TweedleSystem
				}
			}
			
            sys.Link();
            sys.QueueProgramMain(vm);
			StartQueueProcessing();
		}

		// Use this for MonoBehaviour initialization
		void Start()
		{
			vm = new VirtualMachine();
		}

		private void StartQueueProcessing()
		{
			StartCoroutine(WaitAndStartProcessing());
		}

		IEnumerator WaitAndStartProcessing() {
			yield return null;
			yield return null;
			yield return vm.ProcessQueue();
		}
	}
}

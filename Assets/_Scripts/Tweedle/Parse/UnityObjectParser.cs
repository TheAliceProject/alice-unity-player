using System.IO;
using UnityEngine;

using ICSharpCode.SharpZipLib.Zip;
using Alice.Tweedle.VM;

namespace Alice.Tweedle.Parse
{
	public class UnityObjectParser : MonoBehaviour
	{
		static string project_ext = "a3p";

        private TweedleSystem m_System;
        private VirtualMachine m_VM;
        private Coroutine m_QueueProcessor;

        public void Select()
		{
			string zipPath = Crosstales.FB.FileBrowser.OpenSingleFile("Open File", "", project_ext);
			if (System.IO.File.Exists(zipPath) == false)
				return;

			m_System?.Unload();
			if (m_QueueProcessor != null)
			{
                StopCoroutine(m_QueueProcessor);
            }

			m_System = new TweedleSystem();
			using (FileStream fileStream = new FileStream(zipPath, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				using (ZipFile zipFile = new ZipFile(fileStream))
				{
					JsonParser reader = new JsonParser(m_System, zipFile);
					reader.Parse();
					// TODO store the TweedleSystem
				}
			}
			
            m_System.Link();
            m_System.QueueProgramMain(m_VM);
			StartQueueProcessing();
		}

		// Use this for MonoBehaviour initialization
		void Start()
		{
			m_VM = new VirtualMachine();
		}

		// MonoBehaviour Update is called once per frame
		void Update()
		{

		}

		private void StartQueueProcessing()
		{
			m_QueueProcessor = StartCoroutine(m_VM.ProcessQueue());
		}
	}
}

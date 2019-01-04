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

        public void Select(GameObject canvas)
        {
            string zipPath = Crosstales.FB.FileBrowser.OpenSingleFile("Open File", "", project_ext);
            if (System.IO.File.Exists(zipPath) == false)
                return;

            Camera.main.backgroundColor = Color.clear;
            canvas.SetActive(false);

            if (Player.Unity.SceneGraph.Exists) {
                Player.Unity.SceneGraph.Current.Clear();
            }

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
                }
            }
            
            m_System.Link();
            m_System.DumpTypes();
            m_System.QueueProgramMain(m_VM);

            StartQueueProcessing();
        }

        // Use this for MonoBehaviour initialization
        void Start()
        {
            m_VM = new VirtualMachine();
        }

        private void StartQueueProcessing()
        {
            m_QueueProcessor = StartCoroutine(m_VM.ProcessQueue());
        }
    }
}

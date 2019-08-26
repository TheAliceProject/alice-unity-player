using System.IO;
using UnityEngine;

using ICSharpCode.SharpZipLib.Zip;
using Alice.Tweedle.VM;
using Alice.Tweedle.File;

namespace Alice.Tweedle.Parse
{
    public class UnityObjectParser : MonoBehaviour
    {
        static string project_ext = "a3w";
        public bool dumpTypeOutlines = false;
        public Canvas uiCanvas;
        public WorldLoaderControl worldLoader;

        private TweedleSystem m_System;
        private VirtualMachine m_VM;
        private Coroutine m_QueueProcessor;

        public void Select(string fileName = "")
        {
            string zipPath = fileName;
            if(zipPath == "")
                zipPath = Crosstales.FB.FileBrowser.OpenSingleFile("Open File", "", project_ext);

            if (System.IO.File.Exists(zipPath) == false)
                return;

            worldLoader.AddWorldToRecents(zipPath);

            Camera.main.backgroundColor = Color.clear;
            uiCanvas.gameObject.SetActive(false);

            if (Player.Unity.SceneGraph.Exists) {
                Player.Unity.SceneGraph.Current.Clear();
            }

            m_System?.Unload();
            if (m_QueueProcessor != null)
            {
                StopCoroutine(m_QueueProcessor);
            }

            m_System = new TweedleSystem();
            JsonParser.ParseZipFile(m_System, zipPath);
            m_System.Link();

            if (dumpTypeOutlines)
            {
                m_System.DumpTypes();
            }

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

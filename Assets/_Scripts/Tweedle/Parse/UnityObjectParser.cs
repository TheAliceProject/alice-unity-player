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
        public ModalWindow modalWindowPrefab;

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

            if (Player.Unity.SceneGraph.Exists) {
                Player.Unity.SceneGraph.Current.Clear();
            }

            m_System?.Unload();
            if (m_QueueProcessor != null)
            {
                StopCoroutine(m_QueueProcessor);
            }

            m_System = new TweedleSystem();
            try{
                JsonParser.ParseZipFile(m_System, zipPath);
            }
            catch (TweedleParseException exception)
            {
                ModalWindow modalWindow = Instantiate(modalWindowPrefab, uiCanvas.transform);
                string message = "This world is not compatible with this player.\n<b>Player:</b>\n   " + exception.ExpectedVersion + "\n<b>World:</b>\n   " + exception.DiscoveredVersion;
                modalWindow.SetData("Info", message);
                return;
            }

            m_System.Link();
            Camera.main.backgroundColor = Color.clear;

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
            uiCanvas.gameObject.SetActive(false);
        }
    }
}

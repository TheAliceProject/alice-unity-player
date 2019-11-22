using System.IO;
using UnityEngine;
using Alice.Tweedle.VM;
using UnityEngine.UI;
using System.Collections;
using BeauRoutine;
using SFB;

namespace Alice.Tweedle.Parse
{
    public class UnityObjectParser : MonoBehaviour
    {
        static string project_ext = "a3w";
        public bool dumpTypeOutlines = false;
        public Canvas uiCanvas;
        public VRRig uiRig;
        public Button loadNewWorldButton;

        public WorldLoaderControl worldLoader;
        public ModalWindow modalWindowPrefab;

        private TweedleSystem m_System;
        private VirtualMachine m_VM;
        private Routine m_QueueProcessor;
        private string m_currentFilePath;
        void Awake()
        {
            DeleteTemporaryAudioFiles();
            loadNewWorldButton.onClick.AddListener(() =>
            {
                OpenWorld();
            });
        }

        void OnDestroy()
        {
            DeleteTemporaryAudioFiles();
        }

        public void OpenWorld(string fileName = "") {
            string zipPath = fileName;
            if (zipPath == "") {
                var path = StandaloneFileBrowser.OpenFilePanel("Open File", "", project_ext, false);
                if(path.Length > 0)
                {
                    zipPath = path[0];
                    zipPath = System.Uri.UnescapeDataString(zipPath);
                }
            }
            if (System.IO.File.Exists(zipPath) == false) {
                Debug.LogWarning("UnityObjectParser.Select Failed to open File " + zipPath);
                return;
            }
            worldLoader.AddWorldToRecents(zipPath);
            m_currentFilePath = zipPath;

            if (Player.Unity.SceneGraph.Exists) {
                Player.Unity.SceneGraph.Current.Clear();
            }

            m_System?.Unload();
            if (m_QueueProcessor != null)
            {
                m_QueueProcessor.Stop();
            }

            m_System = new TweedleSystem();
            try{
                JsonParser.ParseZipFile(m_System, zipPath);
            }
            catch (TweedleParseException exception)
            {
                ModalWindow modalWindow = Instantiate(modalWindowPrefab, uiCanvas.transform);
                string message = "This world is not compatible with this player.\n<b>Player:</b>\n   " + exception.ExpectedVersion + "\n<b>World:</b>\n   " + exception.DiscoveredVersion;
                modalWindow.SetData("Oops!", message);
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

        public void ReloadCurrentLevel()
        {
            Routine.Start(ReloadDelayed());
        }

        public IEnumerator ReloadDelayed()
        {
            yield return null; // Wait a frame
            OpenWorld(m_currentFilePath);
        }

        // Use this for MonoBehaviour initialization
        void Start()
        {
            m_VM = new VirtualMachine();
        }

        private void StartQueueProcessing()
        {
            m_QueueProcessor.Replace(this, m_VM.ProcessQueue());
            uiCanvas.gameObject.SetActive(false);
            WorldObjects.GetVRObjects().SetActive(false);
        }

        private void DeleteTemporaryAudioFiles()
        {
            DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath);
            FileInfo[] info = dir.GetFiles("*.mp3");
            foreach (FileInfo f in info){
                if(f.FullName.Contains("tempAudio")){
                    try{
                        System.IO.File.Delete(f.FullName);
                    }catch(IOException exception){
                        // We'll get them on the next startup.
                    }
                }
            }
        }
    }
}

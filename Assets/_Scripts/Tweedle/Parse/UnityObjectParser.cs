using System;
using System.IO;
using UnityEngine;
using Alice.Tweedle.VM;
using UnityEngine.UI;
using System.Collections;
using Alice.Player.Modules;
using BeauRoutine;
using SFB;

namespace Alice.Tweedle.Parse
{
    public class UnityObjectParser : MonoBehaviour
    {
        public enum MainMenuControl
        {
            Normal,
            Disabled
        }

        static string project_ext = "a3w";
        public bool dumpTypeOutlines = false;
        public Transform mainMenu;
        public Transform mainMenuVr;

        public WorldLoaderControl worldLoader;
        public VRLoadingControl vrLoadingScreen;
        public ModalWindow modalWindowPrefab;
        public ModalWindow modalWindowPrefabVR;
        public LoadMoreControl[] loadMoreControl;
        public MenuControl[] menuControls;
        public LoadingControl loadingScreen;
        public WorldControl desktopWorldControl;

        private TweedleSystem m_System;
        private VirtualMachine m_VM;
        private Routine m_QueueProcessor;
        private Routine m_LoadRoutine;
        private string m_currentFilePath;
        void Awake()
        {
            DeleteTemporaryAudioFiles();
        }

        void OnDestroy()
        {
            DeleteTemporaryAudioFiles();
        }

        public void OpenWorld(string fileName = "", MainMenuControl mainMenuCtrl = MainMenuControl.Normal) {
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

            m_currentFilePath = zipPath;

            if (Player.Unity.SceneGraph.Exists) {
                Player.Unity.SceneGraph.Current.Clear();
            }

            m_System?.Unload();
            if (m_QueueProcessor != null)
            {
                m_QueueProcessor.Stop();
            }
            RenderSettings.skybox = null;
            LoadWorld(zipPath, mainMenuCtrl);
        }

        private void LoadWorld(string path, MainMenuControl mainMenuCtrl)
        {
            m_LoadRoutine.Replace(this, DisplayLoadingAndLoadLevel(path, mainMenuCtrl));
        }

        private IEnumerator DisplayLoadingAndLoadLevel(string path, MainMenuControl mainMenuCtrl)
        {
            desktopWorldControl.SetNormalTimescale();
            yield return YieldLoadingScreens(true);
            worldLoader.AddWorldToRecents(path);
            m_System = new TweedleSystem();
            try
            {
                JsonParser.ParseZipFile(m_System, path);
            }
            catch (TweedleVersionException tve)
            {
                NotifyUserOfLoadError(
                    "Unable to open the world with this player",
                    "This player is compatible with Alice " + tve.PlayerCompatibleAliceVersion +
                    "\nThe world was created using Alice " + tve.SourceAliceVersion + 
                    "\n\nThe player has " + tve.ExpectedVersion + "\nThe world requires " + tve.DiscoveredVersion +
                    "\n\nTry updating the player.");
                yield break;
            }
            catch (TweedleParseException tre)
            {
                NotifyUserOfLoadError("Unable to read this world", tre.Message);
                yield break;
            }

            m_System.Link();
            Camera.main.backgroundColor = Color.clear;

            if (dumpTypeOutlines)
            {
                m_System.DumpTypes();
            }

            m_System.QueueProgramMain(m_VM);

            StartQueueProcessing();
            yield return YieldLoadingScreens(false);

            WorldControl.ShowWorldControlsBriefly();
            if(mainMenuCtrl == MainMenuControl.Disabled)
                WorldControl.DisableMainMenu();
            desktopWorldControl.ResumeUserTimescale();
        }

        private void NotifyUserOfLoadError(string title, string message)
        {
            var modalWindow = Instantiate(modalWindowPrefab, mainMenu);
            modalWindow.SetData(title, message);
            if (VRControl.IsLoadedInVR())
            {
                var modalWindowVr = Instantiate(modalWindowPrefabVR, mainMenuVr);
                modalWindowVr.SetData(title, message);
                // Make sure when one closes, to close the other as well
                modalWindowVr.LinkWindow(modalWindow);
                modalWindow.LinkWindow(modalWindowVr);
            }
            FadeLoadingScreens(false);
            WorldControl.ReturnToMainMenu();
        }

        private void FadeLoadingScreens(bool on)
        {
            loadingScreen.DisplayLoadingScreen(false);
            vrLoadingScreen.FadeLoader(false);
        }
        private IEnumerator YieldLoadingScreens(bool on)
        {
            yield return Routine.Combine(loadingScreen.DisplayLoadingScreenRoutine(on),
                        vrLoadingScreen.FadeLoaderRoutine(on));
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
            m_VM = new VirtualMachine {ErrorHandler = NotifyUserOfError};

            // This code will open a world directly in the unity app.
            // On windows, right click and Open With... the Alice Player executable
            string[] args = System.Environment.GetCommandLineArgs();

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].ToLower().Contains(".a3w"))
                {
                    loadingScreen.fader.alpha = 1f;
                    OpenWorld(args[1]);
                    return;
                }
            }

            // This code will automatically launch a world if there is a .a3w in the StreamingAssets subdirectory
            DirectoryInfo dir = new DirectoryInfo(Application.streamingAssetsPath);
            FileInfo[] files = dir.GetFiles("*.a3w");
            if (files.Length == 2) // Really 1, but ignore SceneGraphLibrary
            {
                for (int i = 0; i < files.Length; i++)
                {
                    if(!files[i].Name.Contains(WorldObjects.SCENE_GRAPH_LIBRARY_NAME + ".a3w"))
                    {
                        loadingScreen.fader.alpha = 1f;
                        OpenWorld(files[i].FullName, MainMenuControl.Disabled);
                    }
                }
            }
            else if(files.Length > 2)
            {
                // If bundled with multiple files, we will put them on the "Load More" screen as a hub for their worlds
                for(int i = 0; i < menuControls.Length; i++)
                {
                    menuControls[i].DeactivateMainMenu();
                }
                for(int i = 0; i < loadMoreControl.Length; i++)
                {
                    loadMoreControl[i].gameObject.SetActive(true);
                    loadMoreControl[i].SetAsStandalone();
                }
            }
        }

        private void NotifyUserOfError(TweedleRuntimeException tre)
        {
            var dialog = DialogModule.spawnErrorDialog("There was an error executing this world.\n" + tre.Message + "\n\n Should execution continue?");
            dialog.OnReturn(keepTrying =>
            {
                if (keepTrying)
                {
                    m_VM.Resume();
                }
                else
                {
                    WorldControl.ReturnToMainMenu();
                }
            });
        }

        private void StartQueueProcessing()
        {
            m_QueueProcessor.Replace(this, m_VM.ProcessQueue());
            mainMenu.gameObject.SetActive(false);
            WorldObjects.GetVRObjects().SetActive(false);
        }

        private void DeleteTemporaryAudioFiles()
        {
            DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath);
            FileInfo[] info = dir.GetFiles("*.mp3");
            foreach (FileInfo f in info){
                if(f.FullName.Contains("tempAudio")){
                    try {
                        System.IO.File.Delete(f.FullName);
                    } catch(IOException) {
                        // We'll get them on the next startup.
                    }
                }
            }
        }
    }
}

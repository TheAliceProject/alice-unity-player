using System.IO;
using UnityEngine;
using Alice.Tweedle.VM;
using System.Collections;
using Alice.Player.Modules;
using BeauRoutine;
using System;
using ICSharpCode.SharpZipLib.Zip;

namespace Alice.Tweedle.Parse
{
    public class UnityObjectParser : MonoBehaviour
    {
        public enum MainMenuControl
        {
            Normal,
            Disabled
        }

        public static string project_ext = "a3w";
        public static string project_suffix = "." + project_ext;
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

        private TweedleSystem m_System;
        private VirtualMachine m_VM;
        private Routine m_QueueProcessor;
        private string m_currentFilePath;
        void Awake()
        {
            DeleteTemporaryAudioFiles();
        }

        void OnDestroy()
        {
            DeleteTemporaryAudioFiles();
        }
         
        // arg: fileName should be the fullPath of the target file.
        public void OpenWorld(string fileName, MainMenuControl mainMenuCtrl = MainMenuControl.Normal) {
           
            m_currentFilePath = fileName;

            if(Player.Unity.SceneGraph.Exists) {
                Player.Unity.SceneGraph.Current.Clear();
            }

            m_System?.Unload();
            if(m_QueueProcessor != null) {
                m_QueueProcessor.Stop();
            }
            RenderSettings.skybox = null;
            LoadWorld(m_currentFilePath, mainMenuCtrl);
        }

        private void LoadWorld(string path, MainMenuControl mainMenuCtrl)
        {
            StartCoroutine(DisplayLoadingAndLoadLevel(path, mainMenuCtrl));
        }

        private IEnumerator DisplayLoadingAndLoadLevel(string path, MainMenuControl mainMenuCtrl)
        {
            WorldObjects.GetWorldExecutionState().SetNormalTimescale();
            yield return YieldLoadingScreens(true);
            worldLoader.AddWorldToRecents(path);
            m_System = new TweedleSystem();

            yield return JsonParser.Parse(m_System, path, HandleParseException);
            
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
            WorldObjects.GetWorldExecutionState().ResumeUserTimescale();
        }

        private void HandleParseException(Exception e) {
            if (e is ZipException) {
                ZipException ze = e as ZipException;
                NotifyUserOfLoadError("Unable to read this file", ze.Message);
            } else if (e is TweedleVersionException) {
                TweedleVersionException tve = e as TweedleVersionException;
                NotifyUserOfLoadError(
                    "Unable to open the world with this player",
                    "This player is compatible with Alice " + tve.PlayerCompatibleAliceVersion +
                    "\nThe world was created using Alice " + tve.SourceAliceVersion + 
                    "\n\nThe player has " + tve.ExpectedVersion + "\nThe world requires " + tve.DiscoveredVersion +
                    "\n\nTry updating the player.");
            } else if (e is TweedleParseException) {
                NotifyUserOfLoadError("Unable to read this world", e.Message);
            } else {
                NotifyUserOfLoadError("An unexpected error occurred", e.Message);
            }

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
            Coroutine loadingScreenRoutine = StartCoroutine(loadingScreen.DisplayLoadingScreenRoutine(on));
            Coroutine vrLoadingScreenRoutine = StartCoroutine(vrLoadingScreen.FadeLoaderRoutine(on));
            yield return loadingScreenRoutine;
            yield return vrLoadingScreenRoutine;
        }
        public void ReloadCurrentLevel()
        {
            StartCoroutine(ReloadDelayed());
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

            for(int i = 0; i < args.Length; i++) {
                if(args[i].ToLower().Contains(project_suffix)) {
                    loadingScreen.fader.alpha = 1f;
                    OpenWorld(args[1]);
                    return;
                }
            }

            /*
             * The StreamingAssets/Default folder should by default contain two world files: SceneGraphLigrary.a3w and DefaultBundledWorld.a3w
             * If there are some worlds bundled in StreamingAssets folder, the code bellow will try to open them(if there is a single one)
             * or to list them in a menu(multiple bundled), waiting for a choice.
             * If no worlds found, 
             * ON WEBGL: Always open DefaultBundledWorld.a3w since listing StreamingAssets files does not work
             * ON PC PLATFORM: remain on the main menu
             * ON ANDROID/IOS: try to open the DefaultBundledWorld.a3w, which is an indicator of putting bundled world into the StreamingAssets folder
             */

#if UNITY_WEBGL
            // On WebGL, we will try to open a default world
            OpenWorld(Path.Combine(Application.streamingAssetsPath, WorldObjects.DEFAULT_FOLDER_PATH, WorldObjects.DEFAULT_BUNDLED_WORLD_NAME + project_suffix), MainMenuControl.Disabled);
#else
            DirectoryInfo dir = new DirectoryInfo(Application.streamingAssetsPath);
            DirectoryInfo dirDefault = new DirectoryInfo(Path.Combine(Application.streamingAssetsPath, WorldObjects.DEFAULT_FOLDER_PATH));
            FileInfo[] files = dir.GetFiles("*" + project_suffix);
            FileInfo[] filesDefault = dirDefault.GetFiles("*" + project_suffix);

            if (files.Length == 1) { // Only one world is bundled, auto load that world
                loadingScreen.fader.alpha = 1f;
                OpenWorld(files[0].FullName, MainMenuControl.Disabled);
            }
            else if(files.Length > 1) { // Multiple worlds are bundled, we will put them on the "Load More" screen as a hub for their worlds
                for (int i = 0; i < menuControls.Length; i++) {
                    menuControls[i].DeactivateMainMenu();
                }
                for(int i = 0; i < loadMoreControl.Length; i++) {
                    loadMoreControl[i].gameObject.SetActive(true);
                    loadMoreControl[i].SetAsStandalone();
                }
            }

    #if UNITY_ANDROID || UNITY_IOS
            // On Mobile platforms, when no bundled world found, we will try to open a default world
            OpenWorld(Path.Combine(Application.streamingAssetsPath, WorldObjects.DEFAULT_FOLDER_PATH, WorldObjects.DEFAULT_BUNDLED_WORLD_NAME + project_suffix), MainMenuControl.Disabled);
    #endif  
#endif      
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

        public void PurgeVm() {
            m_VM.EmptyQueue();
        }
    }
}

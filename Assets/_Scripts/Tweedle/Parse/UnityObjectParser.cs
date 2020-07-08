using System;
using System.IO;
using UnityEngine;
using Alice.Tweedle.VM;
using UnityEngine.UI;
using System.Collections;
using Alice.Player.Modules;
using BeauRoutine;
using SFB;
using UnityEngine.Networking;

namespace Alice.Tweedle.Parse
{
    public class UnityObjectParser : MonoBehaviour
    {
        public enum MainMenuControl
        {
            Normal,
            Disabled
        }

        static string project_ext = ".a3w";
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

        public IEnumerator MakeLibraryZip()
        {
            string libraryPath = Application.streamingAssetsPath + WorldObjects.DEFAULT_FOLDER_PATH + WorldObjects.SCENE_GRAPH_LIBRARY_NAME + project_ext;
            byte[] result;
            UnityWebRequest www = new UnityWebRequest(libraryPath);
            DownloadHandlerBuffer dH = new DownloadHandlerBuffer();
            www.downloadHandler = dH;
            yield return www.SendWebRequest();
            while(!www.isDone);
            result = www.downloadHandler.data;
            Stream libraryStream = new MemoryStream(result);
            JsonParser.SetLibraryStream(libraryStream);
        }

        IEnumerator loadStreamingAsset(string fileName)
        {
            string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);
            byte[] result;
            yield return YieldLoadingScreens(true);
            UnityWebRequest www = new UnityWebRequest(filePath);
            DownloadHandlerBuffer dH = new DownloadHandlerBuffer();
            www.downloadHandler = dH;
            yield return www.SendWebRequest();
            while(!www.isDone);
            result = www.downloadHandler.data;
            Stream stream = new MemoryStream(result);
            yield return Routine.Start(MakeLibraryZip());

            Camera.main.backgroundColor = Color.clear;

            if (Player.Unity.SceneGraph.Exists)
            {
                Player.Unity.SceneGraph.Current.Clear();
            }

            m_System?.Unload();
            if (m_QueueProcessor != null)
            {
                m_QueueProcessor.Stop();
            }

            m_System = new TweedleSystem();
            JsonParser.ParseZipFile(m_System, stream);
            m_System.Link();

            if (dumpTypeOutlines)
            {
                m_System.DumpTypes();
            }

            m_System.QueueProgramMain(m_VM);

            StartQueueProcessing();
            yield return YieldLoadingScreens(false);
         }
         
         
        // arg: fileName should be the fullPath of the target file.
        public void OpenWorld(string fileName = "", MainMenuControl mainMenuCtrl = MainMenuControl.Normal) {

#if UNITY_ANDROID || UNITY_IOS || UNITY_WEBGL

            Routine.Start(loadStreamingAsset(fileName));
#else

            string zipPath = fileName;
            if(zipPath == "") {
                var path = StandaloneFileBrowser.OpenFilePanel("Open File", "", project_ext, false);
                if(path.Length > 0) {
                    zipPath = path[0];
                    zipPath = System.Uri.UnescapeDataString(zipPath);
                }
            }
            if(System.IO.File.Exists(zipPath) == false) {
                Debug.LogWarning("UnityObjectParser.Select Failed to open File " + zipPath);
                return;
            }

            m_currentFilePath = zipPath;

            if(Player.Unity.SceneGraph.Exists) {
                Player.Unity.SceneGraph.Current.Clear();
            }

            m_System?.Unload();
            if(m_QueueProcessor != null) {
                m_QueueProcessor.Stop();
            }
            RenderSettings.skybox = null;
            LoadWorld(zipPath, mainMenuCtrl);

#endif
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
                NotifyUserOfLoadError("This world is not compatible with this player.\n<b>Player:</b>\n   " +
                                  tve.ExpectedVersion + "\n<b>World:</b>\n   " + tve.DiscoveredVersion);
                yield break;
            }
            catch (TweedleParseException tre)
            {
                NotifyUserOfLoadError("There was a problem reading this world.\n\n" + tre.Message);
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

        private void NotifyUserOfLoadError(string message)
        {
            var modalWindow = Instantiate(modalWindowPrefab, mainMenu);
            modalWindow.SetData("Oops!", message);
            if (VRControl.IsLoadedInVR())
            {
                var modalWindowVr = Instantiate(modalWindowPrefabVR, mainMenuVr);
                modalWindowVr.SetData("Oops!", message);
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

            for(int i = 0; i < args.Length; i++) {
                if(args[i].ToLower().Contains(project_ext)) {
                    loadingScreen.fader.alpha = 1f;
                    OpenWorld(args[1]);
                    return;
                }
            }

            /*
             * The StreamingAssets folder should by default contain two world files: SceneGraphLigrary.a3w and DefaultBundledWorld.a3w
             * If there are more worlds bundled in this folder, the code bellow will try to open them(if there is a single one)
             * or to list them in a menu(multiple bundled), waiting for a choice.
             * If no other worlds found, 
             * ON PC PLATFORM: remain on the main menu
             * ON ANDROID/WebGL: try to open the DefaultBundledWorld.a3w, which is an indicator of putting bundled world into the StreamingAssets folder
             */
            DirectoryInfo dir = new DirectoryInfo(Application.streamingAssetsPath),
                dirDefault = new DirectoryInfo(Application.streamingAssetsPath + "/Default");
            FileInfo[] files = dir.GetFiles("*" + project_ext),
                filesDefault = dirDefault.GetFiles("*" + project_ext);


            bool sceneGraphLibFound = false, defaultWorldFound = false;
            //int bundledWorldNum;

            for(int i = 0; i < filesDefault.Length; ++i) {
                if(filesDefault[i].Name.Contains(WorldObjects.SCENE_GRAPH_LIBRARY_NAME + project_ext)) {
                    sceneGraphLibFound = true;
                }
                else if(filesDefault[i].Name.Contains(WorldObjects.DEFAULT_BUNDLED_WORLD_NAME + project_ext)) {
                    defaultWorldFound = true;
                }
            }

            if(files.Length == 1) { // Only one world is bundled, auto load that world
                loadingScreen.fader.alpha = 1f;
                OpenWorld(files[0].FullName, MainMenuControl.Disabled);
            }
            else if(files.Length > 1) { // Multiple worlds are bundled, we will put them on the "Load More" screen as a hub for their worlds
                for(int i = 0; i < menuControls.Length; i++) {
                    menuControls[i].DeactivateMainMenu();
                }
                for(int i = 0; i < loadMoreControl.Length; i++) {
                    loadMoreControl[i].gameObject.SetActive(true);
                    loadMoreControl[i].SetAsStandalone();
                }
            }
#if UNITY_ANDROID || UNITY_IOS || UNITY_WEBGL
            // On Mobile platforms and WebGL, when no bundled world found, we will try to open a default world
            else if(files.Length == 0 && defaultWorldFound){
                OpenWorld(Application.streamingAssetsPath + WorldObjects.DEFAULT_FOLDER_PATH + WorldObjects.DEFAULT_BUNDLED_WORLD_NAME + project_ext, MainMenuControl.Disabled);
            }
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
    }
}

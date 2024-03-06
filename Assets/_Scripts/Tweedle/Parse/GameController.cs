using System.IO;
using UnityEngine;
using Alice.Tweedle.VM;
using System.Collections;
using Alice.Player.Modules;
using BeauRoutine;
using System;
using System.Linq;
using ICSharpCode.SharpZipLib.Zip;

namespace Alice.Tweedle.Parse
{
    public class GameController : MonoBehaviour
    {
        public bool dumpTypeOutlines = false;
        public Canvas desktopCanvas;
        public Transform mainMenu;
        public Transform mainMenuVr;

        public WorldLoaderControl worldLoader;
        public LoadingControl loadingScreen;
        public VRLoadingControl vrLoadingScreen;
        public ModalWindow modalWindowPrefab;
        public ModalWindow modalWindowPrefabVR;
        public LoadMoreControl[] loadMoreControl;
        public MenuControl[] menuControls;

        private TweedleSystem m_System;
        private VirtualMachine m_VM;
        private Routine m_QueueProcessor;
        private string m_currentFilePath;
        private bool m_IsLoading;

        private readonly LibraryCache m_LibraryCache = new();

        public static bool IsStandAlone { get; private set; }

        public TweedleSystem GetTweedleSystem() {
            return m_System;
        }

        private void Awake() {
            DeleteTemporaryAudioFiles();
            PreloadLibrary();
        }

        private void PreloadLibrary() {
            StartCoroutine(JsonParser.CacheLibrary(m_LibraryCache));
        }

        void OnDestroy()
        {
            DeleteTemporaryAudioFiles();
        }

        // arg: fileName should be the fullPath of the target file.
        public void OpenWorld(string fileName) {
            if (m_IsLoading) {
                return;
            }
            m_IsLoading = true;
            m_currentFilePath = fileName;

            if(Player.Unity.SceneGraph.Exists) {
                Player.Unity.SceneGraph.Current.Clear();
            }

            m_System?.Unload();
            m_QueueProcessor.Stop();
            StartCoroutine(DisplayLoadingAndLoadLevel());
        }

        private IEnumerator DisplayLoadingAndLoadLevel()
        {
            WorldObjects.GetWorldExecutionState().SetNormalTimescale();
            VRControl.HideControls();
            yield return YieldLoadingScreens(true);

            m_System = new TweedleSystem();
            yield return JsonParser.Parse(m_System, m_currentFilePath, m_LibraryCache, HandleReadException);
            m_System.Link();

            try {
                worldLoader.AddWorldToRecents(m_currentFilePath);
            } catch(Exception e) {
                HandleReadException(e);
            }
            RenderSettings.skybox = null;
            m_IsLoading = false;
            m_VM.Resume();
            yield return StartWorld();
        }

        private IEnumerator StartWorld() {
            var mainCamera = Camera.main;
            if (mainCamera != null) {
                mainCamera.backgroundColor = Color.clear;
            }

            if (dumpTypeOutlines)
            {
                m_System.DumpTypes();
            }

            m_System.QueueProgramMain(m_VM);
            StartQueueProcessing();
            yield return YieldLoadingScreens(false);
            VRControl.ShowControls();
            WorldControl.ShowWorldControlsBriefly();
            WorldObjects.GetWorldExecutionState().ResumeUserTimescale();
        }

        private void HandleReadException(Exception e) {
            switch (e) {
                case ZipException ze:
                    NotifyUserOfLoadError("Unable to read this file", ze.Message);
                    break;
                case TweedleVersionException tve:
                    NotifyUserOfLoadError(
                        "Unable to open the world",
                        $"This player is compatible with Alice {tve.PlayerCompatibleAliceVersion}\nThe world" +
                        $" is from Alice {tve.SourceAliceVersion}\n\n" +
                        (tve.LibraryComparison < 0 ? "Try updating the player." : "Try updating Alice and exporting again."));
                    break;
                case TweedleParseException _:
                    NotifyUserOfLoadError("Unable to read this world", e.Message);
                    break;
                default:
                    NotifyUserOfLoadError("An unexpected error occurred", e.Message);
                    break;
            }
        }

        private void NotifyUserOfLoadError(string title, string message)
        {
            var modalWindow = Instantiate(modalWindowPrefab, mainMenu);
            modalWindow.SetData(title, message);
            if (VRControl.IsLoadedInVR())
            {
                VRControl.ShowControls();
                var modalWindowVr = Instantiate(modalWindowPrefabVR, mainMenuVr);
                modalWindowVr.SetData(title, message);
                // Make sure when one closes, to close the other as well
                modalWindowVr.LinkWindow(modalWindow);
                modalWindow.LinkWindow(modalWindowVr);
            }
            FadeLoadingScreens();
            m_IsLoading = false;
            WorldControl.RemainOnMainMenu();
        }

        private void FadeLoadingScreens()
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
        public void ReloadCurrentLevel() {
            m_QueueProcessor.Stop();
            // TODO Move prep tracking to be per scene, not on TweedleSystem, which is code to be run, while prep
            // tracks the execution of static code.
            m_System.ResetPrep();
            StartCoroutine(StartWorld());
        }

        // Use this for MonoBehaviour initialization
        void Start()
        {
            m_VM = new VirtualMachine {ErrorHandler = NotifyUserOfError};

            // This code will open a world directly in the unity app.
            // On windows, right click and Open With... the Alice Player executable
            // This supersedes any further loading options.
            string[] args = System.Environment.GetCommandLineArgs();

            for(int i = 0; i < args.Length; i++) {
                if(args[i].ToLower().Contains(WorldObjects.ProjectExt)) {
                    loadingScreen.fader.alpha = 1f;
                    OpenWorld(args[1]);
                    return;
                }
            }

            /*
             * On the desktop it always opens to the main menu.
             * 
             * For other platforms (WEBGL/ANDROID/IOS )it looks in the (OS specific) directory for user worlds and the
             * worlds bundled in the player.
             * The StreamingAssets/Default directory contains SceneGraphLibrary.a3w, the shared base code, along with
             * a collection of bundled worlds.
             *
             * If there is a single bundled world it will open that world.
             * Otherwise, it will list both bundled and user worlds in a menu, presenting the choice.
             */
#if UNITY_IOS || UNITY_ANDROID || UNITY_WEBGL
            StartCoroutine(LoadMoreControl.ReadAvailableWorlds((bundledWorlds => {
                IsStandAlone = bundledWorlds.Count > 1;
                if (bundledWorlds.Count() == 1) {
                    // Only one world is bundled, auto load that world
                    OpenWorldDirectly(bundledWorlds.First());
                } else {
                    // Multiple worlds are bundled, we will put them on the "Load More" screen as a hub for their worlds
                    foreach (var mc in menuControls) {
                        mc.DeactivateMainMenu();
                    }
                    foreach (var lmc in loadMoreControl) {
                        lmc.gameObject.SetActive(true);
                            lmc.SetAsStandalone();
                    }
                }
            })));
#endif
        }
        
#if UNITY_IOS || UNITY_ANDROID || UNITY_WEBGL
        private void OpenWorldDirectly(string fullName) {
            loadingScreen.fader.alpha = 1f;
            WorldObjects.GetWorldExecutionState().DisableMainMenu();
            OpenWorld(fullName);
        }
#endif

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
            WorldObjects.SetVRObjectsActive(false);
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
            m_VM.Suspend();
        }

        public void ResumeVm() {
            m_VM.Resume();
        }

        public static string GetDefaultWorldMessage() {
#if UNITY_WEBGL
            return  $"Replace the project file, hosted at: {WorldObjects.BundledWorldsDirectory}/{WorldObjects.DefaultBundledWorldName}";
#else
            return $"Put them in {WorldObjects.BundledWorldsDirectory}";
#endif
        }
    }
}

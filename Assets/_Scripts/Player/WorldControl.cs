using System.Collections;
using System.Collections.Generic;
using Alice.Player.Unity;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR;

public class WorldControl : MonoBehaviour
{
    public Button mainMenuButton;
    public Button restartButton;
    public Camera CameraPrefab;
    public UISlidedown uISlidedown;
    public Button speedUpButton;
    public Button slowDownButton;
    public Button pauseButton;
    public TextMeshProUGUI status;

    public Image playPauseImage;
    public Sprite playSprite;
    public Sprite pauseSprite;

    public Material skyboxMaterial;

    private static List<WorldControl> currentWorldControls = new List<WorldControl>();
    private static bool isDisabledForThisInstance = false;

    void Start() {
        if(!currentWorldControls.Contains(this))
            currentWorldControls.Add(this);

        mainMenuButton.onClick.AddListener(ShowMainMenu);
        restartButton.onClick.AddListener(RestartWorld);
        speedUpButton.onClick.AddListener(WorldObjects.GetWorldExecutionState().IncreaseSpeed);
        slowDownButton.onClick.AddListener(WorldObjects.GetWorldExecutionState().DecreaseSpeed);
        pauseButton.onClick.AddListener(WorldObjects.GetWorldExecutionState().TogglePausePlay);

        // Disable main menu button when restarting from a bundled world app
        if(isDisabledForThisInstance)
            mainMenuButton.gameObject.SetActive(false);
    }

    private void ShowMainMenu()
    {
        var destroyedScene = StopScene();

        WorldObjects.GetIntroCanvas().SetActive(true);
        if (XRSettings.enabled)
        {
            WorldObjects.SetVRObjectsActive(true);
        }

        if (destroyedScene) {
            var newCamera = Instantiate(CameraPrefab);
            newCamera.stereoTargetEye = StereoTargetEyeMask.None; // Set to main display, not VR
            newCamera.tag = "MainCamera";
        }
        WorldObjects.GetWorldExecutionState().Reset();
        RenderSettings.skybox = skyboxMaterial;
        uISlidedown.ForceSlide(false);
    }

    private static bool StopScene() {
        WorldObjects.GetParser().PurgeVm();
        var sceneGraph = GameObject.Find("SceneGraph");
        var destroyedScene = (sceneGraph != null);
        if (destroyedScene) {
            SceneGraph.Current.Scene.DropAllListeners();
        }
        Destroy(sceneGraph);
        return destroyedScene;
    }

    private void RestartWorld() {
        StopScene();
        Camera newCamera = Instantiate(CameraPrefab);
        newCamera.tag = "MainCamera";
        uISlidedown.ForceSlide(false);
        WorldObjects.GetParser().ReloadCurrentLevel();
    }

    void OnDestroy() {
        currentWorldControls?.Remove(this);
    }

    public static void ShowWorldControlsBriefly()
    {
        foreach (WorldControl wc in currentWorldControls)
        {
            wc.ShowWorldControlBriefly();
        }
    }

    private void ShowWorldControlBriefly(){
        uISlidedown.ShowBriefly();
    }

    public static void DisableMainMenu()
    {
        foreach (WorldControl wc in currentWorldControls){
            wc.mainMenuButton.gameObject.SetActive(false);
        }
        isDisabledForThisInstance = true;
    }

    public static void UpdateViews() {
        foreach(var wc in currentWorldControls) {
            wc.UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (Time.timeScale == 0.0f){ // if(paused) ?
            playPauseImage.sprite = playSprite;
            status.text = "Paused";
        } else {
            playPauseImage.sprite = pauseSprite;
            status.text = string.Format("{0:0.0}x", Time.timeScale);
        }
    }

    public static void Restart() {
        foreach (var wc in currentWorldControls) {
            wc.RestartWorld();
        }
    }

    public static void ReturnToMainMenu() {
        foreach (var wc in currentWorldControls) {
            wc.ShowMainMenu();
        }
    }
}

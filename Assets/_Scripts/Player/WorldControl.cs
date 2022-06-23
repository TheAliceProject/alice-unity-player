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

    void Start() {
        if (!currentWorldControls.Contains(this))
            currentWorldControls.Add(this);

        mainMenuButton.onClick.AddListener(ShowMainMenu);
        restartButton.onClick.AddListener(RestartWorld);
        speedUpButton.onClick.AddListener(WorldObjects.GetWorldExecutionState().IncreaseSpeed);
        slowDownButton.onClick.AddListener(WorldObjects.GetWorldExecutionState().DecreaseSpeed);
        pauseButton.onClick.AddListener(WorldObjects.GetWorldExecutionState().TogglePausePlay);
    }

    private void ShowMainMenu()
    {
        var destroyedScene = SceneGraph.Current.DestroyScene();

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

    private void RestartWorld() {
        SceneGraph.Current.ResetScene();
        uISlidedown.ForceSlide(false);
        WorldObjects.GetParser().ReloadCurrentLevel();
    }

    void OnDestroy() {
        currentWorldControls?.Remove(this);
    }

    public static void ShowWorldControlsBriefly() {
        GetActiveControl()?.ShowWorldControlBriefly();
    }

    private void ShowWorldControlBriefly() {
        mainMenuButton.gameObject.SetActive(WorldObjects.GetWorldExecutionState().IsMainMenuAllowed());
        uISlidedown.ShowBriefly();
    }

    public static void UpdateViews() {
        GetActiveControl()?.UpdateUI();
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
        GetActiveControl()?.RestartWorld();
    }

    public static void ReturnToMainMenu() {
        GetActiveControl()?.ShowMainMenu();
    }

    private static WorldControl GetActiveControl() {
        WorldControl active = null;
        List<WorldControl> inactive = new List<WorldControl>();
        foreach (var wc in currentWorldControls) {
            if (wc.isActiveAndEnabled) {
                active = wc;
            }
            else {
                inactive.Add(wc);
            }
        }
        foreach (var wc in inactive) {
            currentWorldControls.Remove(wc);
        }

        return active;
    }
}

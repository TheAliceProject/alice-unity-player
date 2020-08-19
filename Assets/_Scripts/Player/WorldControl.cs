using System.Collections;
using System.Collections.Generic;
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

    private const float minimiumTimeScale = 1f / 64f;
    private const float maximumTimeScale = 64f;
    private static float currentTimeScale = 1f;
    private static bool paused = false;
    private static List<WorldControl> currentWorldControls = new List<WorldControl>();
    private static bool isDisabledForThisInstance = false;

    void Start()
    {
        if(!currentWorldControls.Contains(this))
            currentWorldControls.Add(this);

        mainMenuButton.onClick.AddListener(ShowMainMenu);

        restartButton.onClick.AddListener(() =>
        {
            Destroy(GameObject.Find("SceneGraph"));
            Camera newCamera = Instantiate(CameraPrefab);
            newCamera.tag = "MainCamera";
            uISlidedown.ForceSlide(false);
            WorldObjects.GetParser().ReloadCurrentLevel();
        });

        speedUpButton.onClick.AddListener(() =>
        {
            if(!paused){
                if(currentTimeScale >= maximumTimeScale)
                    return;
                currentTimeScale *= 2f;
            }
            else{
                paused = false;
            }
            Time.timeScale = currentTimeScale;
            UpdateStatus();
        });

        slowDownButton.onClick.AddListener(() =>
        {
            if(currentTimeScale <= minimiumTimeScale)
                return;

            currentTimeScale /= 2f;
            Time.timeScale = currentTimeScale;
            UpdateStatus();
        });

        pauseButton.onClick.AddListener(() =>
        {
            if(paused){
                Time.timeScale = currentTimeScale;
            }
            else{
                Time.timeScale = 0f;
            }
            paused = !paused;
            UpdateStatus();
        });

        // Disable main menu button when restarting from a bundled world app
        if(isDisabledForThisInstance)
            mainMenuButton.gameObject.SetActive(false);
    }

    private void ShowMainMenu()
    {
        var sceneGraph = GameObject.Find("SceneGraph");
        var destroyedScene = (sceneGraph != null);
        Destroy(sceneGraph);

        WorldObjects.GetIntroCanvas().SetActive(true);
        if (XRSettings.enabled)
        {
            WorldObjects.GetVRObjects().SetActive(true);
        }

        if (destroyedScene) {
            var newCamera = Instantiate(CameraPrefab);
            newCamera.stereoTargetEye = StereoTargetEyeMask.None; // Set to main display, not VR
            newCamera.tag = "MainCamera";
        }
        Time.timeScale = currentTimeScale = 1f;
        paused = false;
        RenderSettings.skybox = skyboxMaterial;
        uISlidedown.ForceSlide(false);
    }

    void OnDestroy()
    {
        if(currentWorldControls != null)
            currentWorldControls.Remove(this);
    }

    public void SetNormalTimescale(){
        Time.timeScale = 1f;
    }
    public void ResumeUserTimescale(){
        Time.timeScale = paused ? 0f : currentTimeScale;
        UpdateStatus();
    }

    void UpdateStatus()
    {
        foreach(WorldControl wc in currentWorldControls){
            wc.UpdateUI();
        }
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

    void UpdateUI()
    {
        if (Time.timeScale == 0.0f){ // if(paused) ?
            playPauseImage.sprite = playSprite;
            status.text = "Paused";
        }
        else{
            playPauseImage.sprite = pauseSprite;
            status.text = string.Format("{0:0.0}x", Time.timeScale);
        }
    }

    public static void ReturnToMainMenu()
    {
        foreach (WorldControl wc in currentWorldControls){
            wc.ShowMainMenu();
        }
    }
}

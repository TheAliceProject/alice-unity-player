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

    private const float minimiumTimeScale = 1f / 64f;
    private const float maximumTimeScale = 64f;
    private static float currentTimeScale = 1f;
    private static bool paused = false;
    private static List<WorldControl> currentWorldControls = new List<WorldControl>();

    void Start()
    {
        if(!currentWorldControls.Contains(this))
            currentWorldControls.Add(this);

        mainMenuButton.onClick.AddListener(() =>
        {
            Destroy(GameObject.Find("SceneGraph"));
            WorldObjects.GetIntroCanvas().SetActive(true);
            if(XRSettings.enabled)
            {
                WorldObjects.GetVRObjects().SetActive(true);
            }
            Camera newCamera = Instantiate(CameraPrefab);
            newCamera.stereoTargetEye = StereoTargetEyeMask.None; // Set to main display, not VR
            newCamera.tag = "MainCamera";
            Time.timeScale = currentTimeScale = 1f;
            uISlidedown.ForceSlide(false);
        });

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
}

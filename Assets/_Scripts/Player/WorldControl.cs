using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Alice.Tweedle.Parse;

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

    private float currentTimeScale = 1f;
    private bool paused = false;

    void Start()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            Destroy(GameObject.Find("SceneGraph"));
            WorldObjects.GetIntroCanvas().SetActive(true);
            Camera newCamera = Instantiate(CameraPrefab);
            newCamera.tag = "MainCamera";
            Time.timeScale = currentTimeScale = 1f;
            uISlidedown.ForceSlide(true);
        });

        restartButton.onClick.AddListener(() =>
        {
            Destroy(GameObject.Find("SceneGraph"));
            Camera newCamera = Instantiate(CameraPrefab);
            newCamera.tag = "MainCamera";
            uISlidedown.ForceSlide(true);
            WorldObjects.GetParser().ReloadCurrentLevel();
        });

        speedUpButton.onClick.AddListener(() =>
        {
            currentTimeScale *= 2f;
            Time.timeScale = currentTimeScale;
            UpdateStatus();
        });

        slowDownButton.onClick.AddListener(() =>
        {
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

    void UpdateStatus()
    {
        status.text = string.Format("Speed: {0:0.0}x", Time.timeScale);
    }
}

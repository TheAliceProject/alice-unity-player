using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldControl : MonoBehaviour
{
    public Button reloadButton;
    public GameObject introCanvas;
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
        reloadButton.onClick.AddListener(() =>
        {
            Destroy(GameObject.Find("SceneGraph"));
            introCanvas.gameObject.SetActive(true);
            Camera newCamera = Instantiate(CameraPrefab);
            newCamera.tag = "MainCamera";
            Time.timeScale = currentTimeScale = 1f;
            uISlidedown.ForceSlide(true);
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

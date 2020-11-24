using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public Color blueColor;
    public Color grayColor;

    public Texture playTexture;
    public Texture pauseTexture;
    public Texture speedTexture;
    public Texture fastTexture;
    public Texture slowTexture;
    public Texture replayTexture;
    public Texture homeTexture;

    public GameObject playPauseButton;
    public GameObject speedButton;
    public GameObject replayButton;
    public GameObject leaveButton;
    public GameObject speedSubButton;

    public Material buttonMaterial;
    public Material iconMaterial;

    public Transform rightController;
    public Transform head;

    // -1 Idle
    // 0 play / pause
    // 1 speed
    // 2 replay
    // 3 exit
    // 4 slow down
    // 5 speed up
    private int activeButton = -1;
    // 0 play
    // 1 pause
    private bool isPlaying = true;
    private bool isDisplaying = false;
    private bool isSpeedSubButtonActive = false;
    private float speed = 1;

    private WorldControl wc;

    // Start is called before the first frame update
    void Start()
    {
        SetDisplay(true);
        StartCoroutine(DisableMenuInSeconds(3));
        wc = GameObject.FindObjectOfType<WorldControl>();
    }

    private void ResetButton()
    {
        // set button color to gray as idle
        playPauseButton.GetComponent<MeshRenderer>().material.color = grayColor;
        speedButton.GetComponent<MeshRenderer>().material.color = grayColor;
        replayButton.GetComponent<MeshRenderer>().material.color = grayColor;
        leaveButton.GetComponent<MeshRenderer>().material.color = grayColor;

        // set icon of the button to cooresponding texture and idle color
        if(isPlaying)
            playPauseButton.transform.GetChild(0).GetComponent<MeshRenderer>().material.mainTexture = pauseTexture;
        else
            playPauseButton.transform.GetChild(0).GetComponent<MeshRenderer>().material.mainTexture = playTexture;
        playPauseButton.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = blueColor;
        speedButton.transform.GetChild(0).GetComponent<MeshRenderer>().material.mainTexture = speedTexture;
        speedButton.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = blueColor;
        replayButton.transform.GetChild(0).GetComponent<MeshRenderer>().material.mainTexture = replayTexture;
        replayButton.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = blueColor;
        leaveButton.transform.GetChild(0).GetComponent<MeshRenderer>().material.mainTexture = homeTexture;
        leaveButton.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = blueColor;

        // set outline disabled
        playPauseButton.transform.GetChild(1).gameObject.SetActive(false);
        speedButton.transform.GetChild(1).gameObject.SetActive(false);
        replayButton.transform.GetChild(1).gameObject.SetActive(false);
        leaveButton.transform.GetChild(1).gameObject.SetActive(false);

        speedSubButton.SetActive(false);

        activeButton = -1;
        isSpeedSubButtonActive = false;
    }

    private void UpdateButton(GameObject button)
    {
        ResetButton();

        button.GetComponent<MeshRenderer>().material.color = blueColor;
        button.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = grayColor;
        button.transform.GetChild(1).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("PrimaryRight"))
        {
            SetDisplay(!isDisplaying);
        }

        if (!isDisplaying)
            return;
        transform.position = rightController.position;
        transform.LookAt(head);
        transform.RotateAround(transform.position, transform.right, 70);

        if (Input.GetAxis("RightThumbstickUpDown") > 0)
        {
            if (isSpeedSubButtonActive)
            {
                speed = Input.GetAxis("RightThumbstickLeftRight") + 1;
                speed = (int)(speed / 0.5f) * 0.5f + 0.5f;
                speed = Mathf.Clamp(speed, 0.5f, 2);
                wc.ChangeSpeed(speed);
                speedSubButton.transform.GetChild(0).GetComponent<TextMesh>().text = speed.ToString("F2") + "x";
            }
            else
            {
                if (Input.GetAxis("RightThumbstickLeftRight") < -0.5)
                {
                    UpdateButton(playPauseButton);
                    activeButton = 0;
                }
                else if (Input.GetAxis("RightThumbstickLeftRight") < 0)
                {
                    UpdateButton(speedButton);
                    activeButton = 1;
                }
                else if (Input.GetAxis("RightThumbstickLeftRight") > 0.5)
                {
                    UpdateButton(leaveButton);
                    activeButton = 3;
                }
                else if (Input.GetAxis("RightThumbstickLeftRight") > 0)
                {
                    UpdateButton(replayButton);
                    activeButton = 2;
                }
            }
        }
        else if(!isSpeedSubButtonActive)
        {
            ResetButton();
            activeButton = -1;
        }

        if (VRControl.IsRightTriggerUp())
        {
            switch (activeButton)
            {
                case 0:
                    if (isPlaying)
                    {
                        Debug.Log(1);
                        playPauseButton.transform.GetChild(0).GetComponent<MeshRenderer>().material.mainTexture = playTexture;
                    }
                    else
                    {
                        playPauseButton.transform.GetChild(0).GetComponent<MeshRenderer>().material.mainTexture = pauseTexture;
                    }
                    isPlaying = !isPlaying;
                    wc.PauseGame();
                    break;
                case 1:
                    isSpeedSubButtonActive = !isSpeedSubButtonActive;
                    speedButton.transform.GetChild(1).gameObject.SetActive(isSpeedSubButtonActive);
                    speedSubButton.SetActive(isSpeedSubButtonActive);
                    break;
                case 2:
                    wc.RestartWorld();
                    break;
                case 3:
                    wc.ShowMainMenu();
                    break;
                default: break;
            }

        }
    }

    private void SetDisplay(bool shouldDisplay)
    {
        ResetButton();
        isDisplaying = shouldDisplay;
        playPauseButton.SetActive(shouldDisplay);
        speedButton.SetActive(shouldDisplay);
        replayButton.SetActive(shouldDisplay);
        leaveButton.SetActive(shouldDisplay);
        speedSubButton.SetActive(shouldDisplay);
    }

    private IEnumerator DisableMenuInSeconds(float secondsToWait)
    {
        yield return new WaitForSeconds(secondsToWait);
        SetDisplay(false);
    }
}

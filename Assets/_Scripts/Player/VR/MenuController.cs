using System.Collections;
using Alice.Player.Unity;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public Color blueColor;
    public Color grayColor;

    public GameObject playIcon;
    public GameObject pauseIcon;
    public GameObject speedIcon;
    public GameObject replayIcon;
    public GameObject homeIcon;

    public GameObject playPauseButton;
    public GameObject speedButton;
    public GameObject replayButton;
    public GameObject leaveButton;
    public GameObject speedSubButton;

    public Material buttonMaterial;
    public Material iconMaterial;

    public Transform rightController;
    public Transform head;

    private enum Button{
        None,
        PlayPause,
        Speed,
        Reload,
        Exit
    }
    private Button _activeButton = Button.None;

    private bool _isDisplaying;
    private bool _isSpeedSubButtonActive;
    private float _lastSpeedChangeTime;
    private const float TIME_BETWEEN_CHANGES = 0.66F;
    private WorldExecutionState _executionState;

    // Start is called before the first frame update
    void Start() {
        _executionState = WorldObjects.GetWorldExecutionState();
        SetDisplay(true);
        StartCoroutine(DisableMenuInSeconds(3));
    }

    private void ResetButton() {
        // set button color to gray as idle
        playPauseButton.GetComponent<MeshRenderer>().material.color = grayColor;
        speedButton.GetComponent<MeshRenderer>().material.color = grayColor;
        replayButton.GetComponent<MeshRenderer>().material.color = grayColor;
        leaveButton.GetComponent<MeshRenderer>().material.color = grayColor;

        UpdatePlayPauseIcon();

        playIcon.GetComponent<MeshRenderer>().material.color = blueColor;
        pauseIcon.GetComponent<MeshRenderer>().material.color = blueColor;
        speedIcon.GetComponent<MeshRenderer>().material.color = blueColor;
        replayIcon.GetComponent<MeshRenderer>().material.color = blueColor;
        homeIcon.GetComponent<MeshRenderer>().material.color = blueColor;

        // set outline disabled
        playPauseButton.transform.GetChild(0).gameObject.SetActive(false);
        speedButton.transform.GetChild(0).gameObject.SetActive(false);
        replayButton.transform.GetChild(0).gameObject.SetActive(false);
        leaveButton.transform.GetChild(0).gameObject.SetActive(false);

        speedSubButton.SetActive(false);

        _activeButton = Button.None;
        _isSpeedSubButtonActive = false;
    }

    private void UpdatePlayPauseIcon() {
        var isPaused = _executionState.IsPaused();
        playIcon.SetActive(isPaused);
        pauseIcon.SetActive(!isPaused);
    }

    // Update is called once per frame
    void Update() {
        if (!SceneGraph.Exists)
            return;
        if (VRControl.IsMenuTriggerDown())
            SetDisplay(!_isDisplaying);
        if (!_isDisplaying)
            return;

        if (Input.GetAxis("LeftThumbstickUpDown") > 0) {
            if (_isSpeedSubButtonActive)
                UpdateSpeed();
            else
                SelectButton();
        } else if(!_isSpeedSubButtonActive) {
            // should do nothing here
            // ResetButton();
            // _activeButton = Button.None;
        }
        if (VRControl.IsLeftTriggerUp())
            ActivateButton();
    }

    private void UpdateSpeed() {
        var now = Time.unscaledTime;
        if (now - TIME_BETWEEN_CHANGES < _lastSpeedChangeTime) return;

        if (Input.GetAxis("LeftThumbstickLeftRight") > .25) {
            _lastSpeedChangeTime = now;
            _executionState.IncreaseSpeed();
        } else if (Input.GetAxis("LeftThumbstickLeftRight") < -.25) {
            _lastSpeedChangeTime = now;
            _executionState.DecreaseSpeed();
        }
        speedSubButton.transform.GetChild(0).GetComponent<TextMesh>().text =
            _executionState.GetSpeed().ToString("F2") + "x";
    }

    private void SelectButton() {
        if (Input.GetAxis("LeftThumbstickLeftRight") < -0.5) {
            UpdateButton(playPauseButton);
            _activeButton = Button.PlayPause;
        } else if (Input.GetAxis("LeftThumbstickLeftRight") < 0)  {
            UpdateButton(speedButton);
            _activeButton = Button.Speed;
        } else if (_executionState.IsMainMenuAllowed() && Input.GetAxis("LeftThumbstickLeftRight") > 0.5) {
            UpdateButton(leaveButton);
            _activeButton = Button.Exit;
        } else if (Input.GetAxis("LeftThumbstickLeftRight") > 0)  {
            UpdateButton(replayButton);
            _activeButton = Button.Reload;
        }
    }

    private void UpdateButton(GameObject button) {
        ResetButton();

        button.GetComponent<MeshRenderer>().material.color = blueColor;
        button.transform.GetChild(0).gameObject.SetActive(true);
        button.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = grayColor;
        if(button.transform.childCount > 2) {
            button.transform.GetChild(2).GetComponent<MeshRenderer>().material.color = grayColor;
        }
    }

    private void ActivateButton() {
        switch (_activeButton) {
            case Button.None:
                break;
            case Button.PlayPause:
                _executionState.TogglePausePlay();
                UpdatePlayPauseIcon();
                break;
            case Button.Speed:
                _isSpeedSubButtonActive = !_isSpeedSubButtonActive;
                speedButton.transform.GetChild(0).gameObject.SetActive(_isSpeedSubButtonActive);
                speedSubButton.SetActive(_isSpeedSubButtonActive);
                UpdatePlayPauseIcon();
                break;
            case Button.Reload:
                _activeButton = Button.None;
                _executionState.RestartWorld();
                break;
            case Button.Exit:
                _activeButton = Button.None;
                _executionState.ShowMainMenu();
                break;
        }
    }

    private void SetDisplay(bool shouldDisplay) {
        ResetButton();
        _isDisplaying = shouldDisplay;
        playPauseButton.SetActive(shouldDisplay);
        speedButton.SetActive(shouldDisplay);
        replayButton.SetActive(shouldDisplay);
        leaveButton.SetActive(shouldDisplay && _executionState.IsMainMenuAllowed());
        speedSubButton.SetActive(shouldDisplay);
        if (shouldDisplay)
        {
            UpdateButton(playPauseButton);
            _activeButton = Button.PlayPause;
        }
        _executionState.EnableVrEvents(!shouldDisplay);
    }

    private IEnumerator DisableMenuInSeconds(float secondsToWait) {
        yield return new WaitForSeconds(secondsToWait);
        SetDisplay(false);
    }
}

using UnityEngine;

public class WorldExecutionState {
    private const float MINIMUM_TIME_SCALE = 1f / 64f;
    private const float MAXIMUM_TIME_SCALE = 64f;
    private float _currentTimeScale = 1f;
    private bool _isPaused;
    private bool _sendVrEvents;

    public void Reset() {
        Time.timeScale = _currentTimeScale = 1f;
        _isPaused = false;
    }

    public void SetNormalTimescale() {
        Time.timeScale = 1f;
    }

    public void ResumeUserTimescale() {
        Time.timeScale = _isPaused ? 0f : _currentTimeScale;
        UpdateStatus();
    }

    public void DecreaseSpeed() {
        if (_currentTimeScale <= MINIMUM_TIME_SCALE)
            return;
        _currentTimeScale /= 2f;
        ChangeSpeed(_currentTimeScale);
    }

    public void IncreaseSpeed() {
        if (_isPaused) {
            _isPaused = false;
        } else {
            if (_currentTimeScale >= MAXIMUM_TIME_SCALE)
                return;
            _currentTimeScale *= 2f;
        }
        ChangeSpeed(_currentTimeScale);
    }

    private void ChangeSpeed(float timeScale) {
        _currentTimeScale = timeScale;
        Time.timeScale = _currentTimeScale;
        UpdateStatus();
    }

    public void TogglePausePlay() {
        _isPaused = !_isPaused;
        ResumeUserTimescale();
    }

    private void UpdateStatus() {
        WorldControl.UpdateViews();
    }

    public bool IsPaused() {
        return _isPaused;
    }

    public float GetSpeed() {
        return _currentTimeScale;
    }

    public bool IsVrSendingEvents() {
        return _sendVrEvents;
    }

    public void EnableVrEvents(bool enable)
    {
        _sendVrEvents = enable;
    }

    public void RestartWorld() {
        WorldControl.Restart();
    }

    public void ShowMainMenu() {
        WorldControl.ReturnToMainMenu();
    }
}
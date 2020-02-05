using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class SettingsControl : MonoBehaviour
{
    public SystemSettings settings;
    
    public TMP_Dropdown dropdown;
    public Button applyButton;
    public Toggle fullScreen;
    private bool dirty;

    void Start()
    {
        applyButton.onClick.AddListener(ApplySettings);

        UpdateFromSettings();
    }
    private void UpdateFromSettings()
    {
        fullScreen.isOn = settings.IsFullScreen();

        var resolutionList = settings.GetAvailableResolutions();
        var options = resolutionList.Select(resolution => new TMP_Dropdown.OptionData(resolution)).ToList();
        dropdown.ClearOptions();
        dropdown.AddOptions(options);

        for(int i = 0; i < dropdown.options.Count; i++) {
            if(dropdown.options[i].text == settings.GetScreenDescription()) {
                dropdown.value = i;
                break;
            }
        }
    }

    private void ApplySettings()
    {
        settings.SetDimensions(dropdown.options[dropdown.value].text);
        settings.SetFullScreen(fullScreen.isOn);
        settings.SaveAndApply();
    }
}

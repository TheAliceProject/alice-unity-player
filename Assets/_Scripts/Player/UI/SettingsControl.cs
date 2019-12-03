using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SettingsControl : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public Button applyButton;
    public Toggle fullScreen;
    private bool dirty;
    private string[] supportedResolutions = { "1024 x 768", 
                                            "1280 x 720", 
                                            "1366 x 768", 
                                            "1440 x 900", 
                                            "1920 x 1080", 
                                            "1920 x 1200", 
                                            "2560 x 1440"};
    // Start is called before the first frame update
    void Start()
    {
        applyButton.onClick.AddListener(() =>
        {
            ApplySettings();
        });

        Resolution[] resolutions = Screen.resolutions;
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

        List<int> supportedWidths = new List<int>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            supportedWidths.Add(resolutions[i].width);
        }

        for (int i = 0; i < supportedResolutions.Length; i++)
        {
            string[] resolutionWidthHeight = ParseResolution(supportedResolutions[i]);
            if(supportedWidths.Contains(int.Parse(resolutionWidthHeight[0])))   // See if width is in supported widths
            {
                TMP_Dropdown.OptionData newOption = new TMP_Dropdown.OptionData();
                newOption.text = resolutionWidthHeight[0] + " x " + resolutionWidthHeight[1];
                options.Add(newOption);
            }
        }

        dropdown.ClearOptions();
        dropdown.AddOptions(options);
    }

    private void ApplySettings()
    {
        string[] parsed = ParseResolution(dropdown.options[dropdown.value].text);
        Screen.SetResolution(int.Parse(parsed[0]), int.Parse(parsed[1]), fullScreen.isOn);
    }

    private string[] ParseResolution(string resolutionString)
    {
        char[] separators = { ' ', 'x' };
        string[] parsed = resolutionString.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
        if (parsed.Length != 2)
        {
            Debug.LogError("Something went wrong when getting resolutions");
            return null;
        }
        return parsed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class SettingsControl : MonoBehaviour
{
    private class Settings{
        public int width;
        public int height;
        public int fullScreen;
    }

    public TMP_Dropdown dropdown;
    public Button applyButton;
    public Toggle fullScreen;
    private Settings settings = new Settings();
    private bool dirty;
    private string[] supportedResolutions = { "1024 x 768", 
                                            "1280 x 720", 
                                            "1366 x 768", 
                                            "1440 x 900",
                                            "1536 x 864",
                                            "1680 x 1050",
                                            "1920 x 1080", 
                                            "1920 x 1200", 
                                            "2560 x 1440"};
    private const string settingsFile = "settings.txt";

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
        ReadSettings();
        this.enabled = false;
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetInt("ScreenWidth", settings.width);
        PlayerPrefs.SetInt("ScreenHeight", settings.height);
        PlayerPrefs.SetInt("Fullscreen", settings.fullScreen > 0 ? 1 : 0);
    }
    private void ReadSettings()
    {
        settings.fullScreen = PlayerPrefs.GetInt("Fullscreen", 0);
        settings.width = PlayerPrefs.GetInt("ScreenWidth", 1024);
        settings.height = PlayerPrefs.GetInt("ScreenHeight", 768);

        fullScreen.isOn = settings.fullScreen > 0;
        string width = settings.width.ToString();
        string height = settings.height.ToString();
        for(int i = 0; i < dropdown.options.Count; i++){
            if(dropdown.options[i].text == width + " x " + height){
                dropdown.value = i;
                break;
            }
        }
        Screen.SetResolution(settings.width, settings.height, fullScreen.isOn);
    }

    private void ApplySettings()
    {
        string[] parsed = ParseResolution(dropdown.options[dropdown.value].text);
        settings.width = int.Parse(parsed[0]);
        settings.height = int.Parse(parsed[1]);
        settings.fullScreen = fullScreen.isOn ? 1 : 0;

        Screen.SetResolution(settings.width, settings.height, fullScreen.isOn);
        SaveSettings();
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

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "SystemSettings", menuName = "Shared/SystemSettings")]
public class SystemSettings : ScriptableObject {
    
    public class Dimensions {
        internal int Width;
        internal int Height;

        internal Dimensions(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override String ToString()
        {
            return Width + " x " + Height;
        }

        public void UpdateFrom(string resolutionString)
        {
            char[] separators = {' ', 'x'};
            var parsed = resolutionString.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            if (parsed.Length != 2)
            {
                Debug.LogError("Something went wrong when getting resolutions");
                return;
            }
            Width = int.Parse(parsed[0]);
            Height = int.Parse(parsed[1]);
        }
    }

    private readonly Dimensions _dimensions = new Dimensions(0,0);
    private bool _fullScreen;

    public Dimensions[] supportedResolutions = { new Dimensions(1024 , 768),
        new Dimensions(1280, 720),
        new Dimensions(1366, 768),
        new Dimensions(1440, 900),
        new Dimensions(1536, 864),
        new Dimensions(1680, 1050),
        new Dimensions(1920, 1080),
        new Dimensions(1920, 1200),
        new Dimensions(2560, 1440)};

    public void SaveAndApply()
    {
        PlayerPrefs.SetInt("ScreenWidth", _dimensions.Width);
        PlayerPrefs.SetInt("ScreenHeight", _dimensions.Height);
        PlayerPrefs.SetInt("Fullscreen", _fullScreen ? 1 : 0);

        ApplyScreenState();
    }

    public string GetScreenDescription() {
        return _dimensions.Width + " x " + _dimensions.Height;
    }

    public void ReadFromPrefs()
    {
        _fullScreen = PlayerPrefs.GetInt("Fullscreen", 0) > 0;
        _dimensions.Width = PlayerPrefs.GetInt("ScreenWidth", 1024);
        _dimensions.Height = PlayerPrefs.GetInt("ScreenHeight", 768);

        ApplyScreenState();
    }

    private void ApplyScreenState()
    {
        // Set resolution and fullscreen
#if UNITY_STANDALONE
        Screen.SetResolution(_dimensions.Width, _dimensions.Height, IsFullScreen());
#endif
    }

    public bool IsFullScreen()
    {
        return _fullScreen;
    }

    public void SetFullScreen(bool isFullScreen)
    {
        _fullScreen = isFullScreen;
    }

    public List<string> GetAvailableResolutions()
    {
        var supportedWidths = Screen.resolutions.Select(t => t.width).ToList();
        return (from res in supportedResolutions
            where supportedWidths.Contains(res.Width)
            select res.ToString()).ToList();

    }

    public void SetDimensions(string resolutionString)
    {
        _dimensions.UpdateFrom(resolutionString);
    }

}
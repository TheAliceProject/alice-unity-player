using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Alice.Tweedle.Parse;
using TMPro;

public class WorldLoaderControl : MonoBehaviour
{
    public RecentWorldButton recentWorldButtonPrefab;
    public Transform contentBox;
    public UnityObjectParser parser;
    public TextMeshProUGUI versionString;
    public Toggle loadInVR;
    
    private List<GameObject> activeButtons = new List<GameObject>();
    private List<string> recentWorlds = new List<string>();
    private const string RecentWorldsFileName = "/recentWorlds.txt";

    void Start()
    {
        PopulateLevels();
        
        loadInVR.onValueChanged.AddListener((value) =>
        {
            VRControl.I.LoadWorldInVR = value;
            VRControl.I.SetVROutput(value);
        });

        versionString.text = string.Format("Player Ver {0} - Library Ver {1}", PlayerLibraryManifest.Instance.PlayerLibraryVersion, PlayerLibraryManifest.Instance.GetLibraryVersion());
    }

    public void AddWorldToRecents(string file)
    {
        ClearButtons();
        if(recentWorlds.Contains(file))
            recentWorlds.Remove(file);
        recentWorlds.Insert(0, file);
        SaveLevels();
        LoadButtons(recentWorlds);
    }

    void PopulateLevels()
    {
        if(!File.Exists(Application.persistentDataPath + RecentWorldsFileName))
            return;

        recentWorlds.Clear();
        var fs = File.OpenText(Application.persistentDataPath + RecentWorldsFileName);
        string line = "";
        while (line != null)
        {
            line = fs.ReadLine();
            if (line == null)
                break;
            else if (line.Trim() == "") // Really looking for \n or \r or some combination here. Should never happen in theory unless someone purposefully messes with this file
                continue;

            recentWorlds.Add(line);
        }
        fs.Close();
        LoadButtons(recentWorlds);
    }

    void SaveLevels()
    {
        var fs = File.CreateText(Application.persistentDataPath + RecentWorldsFileName);
        for (int i = 0; i < recentWorlds.Count; i++)
        {
            fs.WriteLine(recentWorlds[i]);
        }
        fs.Close();
    }

    void LoadButtons(List<string> worldFiles)
    {
        ClearButtons();
        for (int i = 0; i < worldFiles.Count && i <= 8; i++)
        {    
            if (File.Exists(worldFiles[i]))
            {
                RecentWorldButton worldButton = Instantiate(recentWorldButtonPrefab, contentBox);
                worldButton.SetText(worldFiles[i]);
                worldButton.button.onClick.AddListener(() =>
                {
                    parser.Select(worldButton.GetFilePath());
                });
                activeButtons.Add(worldButton.gameObject);
            }
        }
    }

    void ClearButtons()
    {
        for (int i = 0; i < activeButtons.Count; i++)
        {
            Destroy(activeButtons[i]);
        }
        activeButtons.Clear();
    }
}

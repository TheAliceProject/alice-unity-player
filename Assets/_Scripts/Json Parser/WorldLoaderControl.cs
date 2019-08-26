using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Alice.Tweedle.Parse;

public class WorldLoaderControl : MonoBehaviour
{
    public RecentWorldButton recentWorldButtonPrefab;
    public Transform contentBox;
    public UnityObjectParser parser;

    private List<GameObject> activeButtons = new List<GameObject>();
    private List<string> recentWorlds = new List<string>();
    private const string RecentWorldsFileName = "/recentWorlds.txt";

    void Start()
    {
        PopulateLevels();
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
            else if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
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
        for (int i = 0; i < worldFiles.Count; i++)
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

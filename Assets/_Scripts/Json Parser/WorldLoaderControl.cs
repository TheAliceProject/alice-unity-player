using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using BeauRoutine;

public class WorldLoaderControl : MonoBehaviour
{
    public RecentWorldButton[] recentButtons;
    public Toggle loadInVR;
    public bool useVRSizing = false; // Set in inspector

    private List<string> recentWorlds = new List<string>();
    private const string RecentWorldsFileName = "/recentWorlds.txt";

    void Start()
    {
        PopulateLevels();
        
        if(loadInVR != null)
            loadInVR.onValueChanged.AddListener(VRControl.Loaded);
    }

    void OnEnable()
    {
        PopulateLevels();
    }

    public void AddWorldToRecents(string file)
    {
        if(recentWorlds.Contains(file))
            recentWorlds.Remove(file);
        recentWorlds.Insert(0, file);
        SaveLevels();
        LoadButtons(recentWorlds);
    }

    void PopulateLevels()
    {
        if(!File.Exists(Application.persistentDataPath + RecentWorldsFileName)){
            for (int i = 0; i < recentButtons.Length; i++)
                recentButtons[i].gameObject.SetActive(false);
            return;
        }


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
        for (int i = 0; i < recentButtons.Length; i++)
        {
            if(i > worldFiles.Count-1)
            {
                recentButtons[i].gameObject.SetActive(false);
            }
            else if (File.Exists(worldFiles[i]))
            {
                if (useVRSizing)
                {
                    recentButtons[i].ScaleText(1.5f);
                    recentButtons[i].collider.enabled = true;
                }
                recentButtons[i].gameObject.SetActive(true);
                recentButtons[i].SetText(worldFiles[i]);
                int x = i;
                recentButtons[i].button.onClick.RemoveAllListeners();
                recentButtons[i].button.onClick.AddListener(() =>
                {
                    WorldObjects.GetParser().OpenWorld(recentButtons[x].GetFilePath());
                });
            }
        }
    }

    int GetNumRecents()
    {
        return useVRSizing ? 6 : 8;
    }

    float GetSpacing()
    {
        return useVRSizing ? 100f : 105f;
    }
}

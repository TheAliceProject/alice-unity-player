using System;
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

    private List<RecentWorldData> recentWorlds = new List<RecentWorldData>();
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
        RecentWorldData toRemove = null;
        for (int i = 0; i < recentWorlds.Count; i++)
        {
            if(recentWorlds[i].path == file){
                toRemove = recentWorlds[i];
            }
        }
        if (toRemove != null){
            recentWorlds.Remove(toRemove);
        }

        RecentWorldData newWorld = new RecentWorldData(file);
        recentWorlds.Insert(0, newWorld);
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

            string[] parsedFile = line.Split('|');
            RecentWorldData data = new RecentWorldData(parsedFile);
            recentWorlds.Add(data);
        }
        fs.Close();
        LoadButtons(recentWorlds);
    }

    void SaveLevels()
    {
        var fs = File.CreateText(Application.persistentDataPath + RecentWorldsFileName);
        for (int i = 0; i < recentWorlds.Count; i++)
        {
            fs.WriteLine(recentWorlds[i].path + "|" + recentWorlds[i].lastOpened);
        }
        fs.Close();
    }

    void LoadButtons(List<RecentWorldData> worldFiles)
    {
        List<string> worldFilesTrimmed = new List<string>();

        int totalFilesFound = 0;
        for (int i = 0; i < worldFiles.Count; i++)
        {
            // Fixes issue where two of the same file exist in our list, but one uses "\" and one uses "/"
            string uniqueFile = worldFiles[i].path.Replace("/", "").Replace("\\", "");
            if(worldFilesTrimmed.Contains(uniqueFile))
                continue;
            else
                worldFilesTrimmed.Add(uniqueFile);

            if (File.Exists(worldFiles[i].path))
            {
                if (useVRSizing)
                {
                    recentButtons[totalFilesFound].ScaleText(1.5f);
                    recentButtons[totalFilesFound].collider.enabled = true;
                }
                recentButtons[totalFilesFound].gameObject.SetActive(true);
                recentButtons[totalFilesFound].SetData(worldFiles[i]);
                int x = totalFilesFound;
                recentButtons[totalFilesFound].button.onClick.RemoveAllListeners();
                recentButtons[totalFilesFound].button.onClick.AddListener(() =>
                {
                    recentButtons[x].SetLastOpenedNow();
                    WorldObjects.GetParser().OpenWorld(recentButtons[x].GetFilePath());
                });
                totalFilesFound++;
                if(totalFilesFound == 2)
                    break;
            }
        }

        if(totalFilesFound == 0){
            recentButtons[0].gameObject.SetActive(false);
            recentButtons[1].gameObject.SetActive(false);
        }
        else if(totalFilesFound == 1){
            recentButtons[1].gameObject.SetActive(false);
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

public class RecentWorldData
{
    public string path;
    public string author;
    public long lastOpened;
    private static DateTime _epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);

    public RecentWorldData(){
        path = "";
        author = "unknown";
        lastOpened = -1;
    }

    public RecentWorldData(string path){
        this.path = path;
        author = "unknown";
        SetLastOpenedNow();
    }

    public RecentWorldData(string[] entries)
    {
        author = "unknown";
        // ADD MORE FILE DATA HERE IN THE FUTURE, follow the format...
        if (entries.Length == 2){ // Great! They have the most recent version of the file cache
            path = entries[0];
            lastOpened = int.Parse(entries[1]);
        }
        else if(entries.Length == 1){ // Okay, we probably only have the path
            path = entries[0];
            lastOpened = -1;
        }
        else{ // Something went wrong
            Debug.LogError("Something went wrong parsing the recents file)");
        }
    }

    public void SetLastOpenedNow()
    {
        lastOpened = (long)(System.DateTime.UtcNow - _epochStart).TotalSeconds; // The current time
    }

    public long SecondsSinceLastOpened()
    {
        long currentTime = (long)(System.DateTime.UtcNow - _epochStart).TotalSeconds; // The current time
        return currentTime - lastOpened;
    }
}

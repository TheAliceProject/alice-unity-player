using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using BeauRoutine;

public class LoadMoreControl : MonoBehaviour
{
    public TMP_Dropdown filter;
    public Transform buttonParent;
    
    public enum WorldListLocation{
        LocalRecent,
        LocalName,
        // LocalAuthor,
        // ToDo: Possible future sorting methods
        // Internet,
        // FeaturedInternet,
    }

    public RecentWorldButton recentWorldPrefab;
    public bool useVRSizing; // set in inspector

    private const string RecentWorldsFileName = "/recentWorlds.txt";
    private List<RecentWorldButton> recentWorlds = new List<RecentWorldButton>();
    private Routine m_routine;

    void Start()
    {
        DestroyCurrentButtons();
        LoadButtons(GetRecentWorlds());

        filter.onValueChanged.AddListener(delegate
        {
            LoadButtons(GetRecentWorlds());
        });
    }

    void OnEnable()
    {
        LoadButtons(GetRecentWorlds());
    }

    private List<RecentWorldData> GetRecentWorlds(){
        if (!File.Exists(Application.persistentDataPath + RecentWorldsFileName)){
            return null;
        }

        List<RecentWorldData> recentWorldsData = new List<RecentWorldData>();
        recentWorldsData.Clear();
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
            if (!File.Exists(parsedFile[0]))
                continue;
            recentWorldsData.Add(data);
        }
        fs.Close();
        return recentWorldsData;
    }

    void LoadButtons(List<RecentWorldData> worldFiles)
    {
        m_routine.Replace(this, loadButtonsRoutine(worldFiles));
    }

    private IEnumerator loadButtonsRoutine(List<RecentWorldData> worldFiles)
    {
        // Remove any worlds that might be active
        DestroyCurrentButtons();

        for (int i = 0; i < worldFiles.Count; i++)
        {
            RecentWorldButton newButton = Instantiate(recentWorldPrefab, buttonParent);
            newButton.SetData(worldFiles[i]);
            newButton.collider.enabled = useVRSizing;
            recentWorlds.Add(newButton);
            newButton.button.onClick.AddListener(() =>
            {
                newButton.SetLastOpenedNow();
                WorldObjects.GetParser().OpenWorld(newButton.GetFilePath());
            });
        }

        yield return null;

        // Set content to  lowest child height 0 + size of one cell
        float minYPos = 0f;
        for (int i = 0; i < buttonParent.childCount; i++)
        {
            RectTransform trans = buttonParent.GetChild(i) as RectTransform;
            if (trans.anchoredPosition.y < minYPos)
                minYPos = trans.anchoredPosition.y;
        }

        Debug.Log("Min y pos: " + minYPos);

       // (buttonParent as RectTransform).SetSizeDelta(minYPos, Axis.Y);
        
    }

    private void DestroyCurrentButtons()
    {
        for (int i = recentWorlds.Count - 1; i >= 0; i--){
            Destroy(recentWorlds[i].gameObject);
        }
        recentWorlds.Clear();
    }
}

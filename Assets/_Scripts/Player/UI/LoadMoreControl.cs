using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using BeauRoutine;
using Alice.Tweedle.Parse;
using UnityEngine.UI;

public class LoadMoreControl : MonoBehaviour
{
    public TMP_Dropdown filter;
    public Transform buttonParent;
    public GridLayoutGroup buttonLayout;
    public ScrollRect rect;

    public enum WorldListSort{
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
    
    private const float MinCellWidth = 350;
    private const float CellAspectRatio = 0.85f;
    private static bool useBundledWorlds = false;

    void Start()
    {
        DestroyCurrentButtons();
        LoadButtons(useBundledWorlds ? GetBundledWorlds() : GetRecentWorlds());

        filter.onValueChanged.AddListener(delegate
        {
            LoadButtons(GetRecentWorlds());
        });
    }

    void Update()
    {
        if(VRControl.IsLoadedInVR()){
            if (Input.GetAxis("RightThumbstickUpDown") > 0.75f)
                rect.verticalNormalizedPosition += 0.01f;
            else if (Input.GetAxis("RightThumbstickUpDown") < -0.75f)
                rect.verticalNormalizedPosition -= 0.01f;
        }
    }

    private void OnRectTransformDimensionsChange()
    {
        ResizeGrid();
    }

    public void SetAsStandalone()
    {
        useBundledWorlds = true;
        filter.gameObject.SetActive(false);
    }

    void ResizeGrid()
    {
        RectTransform parent = buttonLayout.GetComponentInParent<RectTransform>();
        var parentWidth = parent.rect.width;
        var margin = buttonLayout.spacing.x;
 
        var cellsPerRow = (float) Math.Floor((parentWidth + margin) / (MinCellWidth + margin));
        var baseWidth = (cellsPerRow * (MinCellWidth + margin)) - margin;
        var spareWidth = parentWidth - baseWidth;
        var cellWidth = MinCellWidth + spareWidth / cellsPerRow;

        buttonLayout.cellSize = new Vector2(cellWidth, cellWidth * CellAspectRatio);
    }

    private List<RecentWorldData> GetBundledWorlds(){
        List<RecentWorldData> recentWorldsData = new List<RecentWorldData>();
        recentWorldsData.Clear();

        var dir = new DirectoryInfo(UnityObjectParser.AutoLoadedWorldsDirectory);
        var files = dir.GetFiles(WorldObjects.ProjectPattern);
        foreach (var file in files) {
            if (!File.Exists(file.FullName) || file.FullName.Contains(WorldObjects.SceneGraphLibraryName))
                continue;
            recentWorldsData.Add(new RecentWorldData(file.FullName));
        }
        return SortWorlds(recentWorldsData);
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
        return SortWorlds(recentWorldsData);
    }

    private List<RecentWorldData> SortWorlds(List<RecentWorldData> listToSort) {
        if((WorldListSort) filter.value == WorldListSort.LocalName)
            listToSort.Sort((a, b) => String.Compare(GetSortName(a), GetSortName(b), StringComparison.Ordinal));
        // Else take default ordering, by recent use
        return listToSort;
    }

    private String GetSortName(RecentWorldData world)
    {
        return Path.GetFileNameWithoutExtension(world.path);
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
                var parser = WorldObjects.GetParser();
                if (parser == null) {
                    return;
                }
                newButton.SetLastOpenedNow();
                parser.OpenWorld(newButton.GetFilePath());
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

       // (buttonParent as RectTransform).SetSizeDelta(minYPos, Axis.Y);
        
       ResizeGrid();
    }

    private void DestroyCurrentButtons()
    {
        for (int i = recentWorlds.Count - 1; i >= 0; i--){
            Destroy(recentWorlds[i].gameObject);
        }
        recentWorlds.Clear();
    }
}

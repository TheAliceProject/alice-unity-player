using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System.Linq;
using BeauRoutine;
using Alice.Storage;
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
    
    private List<RecentWorldButton> recentWorlds = new List<RecentWorldButton>();
    private Routine m_routine;
    
    private const float MinCellWidth = 350;
    private const float CellAspectRatio = 0.85f;

    private void Start()    
    {
        DestroyCurrentButtons();
        ReadWorldData();

        filter.onValueChanged.AddListener(delegate
        {
            LoadButtons(GetRecentWorlds());
        });
    }

    private void ReadWorldData() {
        if (GameController.IsStandAlone) {
            StartCoroutine(ReadAvailableWorlds());
        }
        else {
            LoadButtons(GetRecentWorlds());
        }
    }

    private IEnumerator ReadAvailableWorlds() {
        yield return ReadAvailableWorlds(SetWorldList);
    }

    public delegate void ListHandler(List<string> files);

    public static IEnumerator ReadAvailableWorlds(ListHandler onSuccess) {
        var available = new List<string>();
        yield return AddBundledWorlds(available);
        available.AddRange(GetUserWorlds());
        onSuccess(available);
    }

    private void SetWorldList(List<string> worldNames) {
        var worldData = worldNames.Select(file => new RecentWorldData(file)).ToList();
        LoadButtons(SortWorlds(worldData));
    }

    private static IEnumerator AddBundledWorlds(ICollection<string> worlds) {
        yield return PopulateFullBundledFileNames(WorldObjects.BundledWorldsListFile, worlds);
    }

    private static IEnumerator PopulateFullBundledFileNames(string worldListFile, ICollection<string> worlds) {
        yield return StorageReader.Read(worldListFile, (stream => {
            using (stream) {
                TextReader reader = new StreamReader(stream);
                while (true) {
                    var line = reader.ReadLine();
                    if (line == null)
                        break;
                    if (line.Trim() == "")
                        continue;
                    worlds.Add(Path.Combine(WorldObjects.BundledWorldsDirectory, line));
                }
            }
        }), _ => {
            // ignore exceptions
        });
    }

    private List<RecentWorldData> GetRecentWorlds(){
        var lines = GetFileEntries(WorldObjects.RecentWorldsListFile);
        var recentWorldsData = lines
            .Select(line => new RecentWorldData(line.Split('|')))
            .Where(data => File.Exists(data.path))
            .ToList();
        return SortWorlds(recentWorldsData);
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

    private static IEnumerable<string> GetUserWorlds() {
        var userFiles = new DirectoryInfo(Application.persistentDataPath).GetFiles(WorldObjects.ProjectPattern);
        return (from file in userFiles where File.Exists(file.FullName) select file.FullName).ToList();
    }

    private static IEnumerable<string> GetFileEntries(string worldListFile) {
        var lines = new List<string>();
        if (!File.Exists(worldListFile)) {
            return lines;
        }
        var fs = File.OpenText(worldListFile);
        while (true)
        {
            var line = fs.ReadLine();
            if (line == null)
                break;
            if (line.Trim() == "") // Really looking for \n or \r or some combination here. Should never happen in theory unless someone purposefully messes with this file
                continue;
            lines.Add(line);
        }
        fs.Close();
        return lines;
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
                var parser = WorldObjects.GetGameController();
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

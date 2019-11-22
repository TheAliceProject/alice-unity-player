using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    public Button MainMenu;
    public Button RecentWorlds;
    public Button LoadMoreWorlds;
    public Button About;
    public Button Settings;
    public Button[] CloseButtons;
    public Button CloseLoadMore;

    public GameObject MainMenuPanel;
    public GameObject RecentWorldsPanel;
    public GameObject AboutPanel;
    public GameObject LoadMoreWorldsPanel;
    public GameObject SettingsPanel;
    
    public GameObject[] allPanels;

    void Start()
    {
        MainMenu.onClick.AddListener(() => { SetOnlyThisPanelActive(MainMenuPanel); });
        RecentWorlds.onClick.AddListener(() => { SetOnlyThisPanelActive(RecentWorldsPanel); });
        About.onClick.AddListener(() => { SetOnlyThisPanelActive(AboutPanel); });
        Settings.onClick.AddListener(() => { SetOnlyThisPanelActive(SettingsPanel); });
        LoadMoreWorlds.onClick.AddListener(() => { SetOnlyThisPanelActive(LoadMoreWorldsPanel); });

        CloseLoadMore.onClick.AddListener(() => { SetOnlyThisPanelActive(RecentWorldsPanel);  });
        for (int i = 0; i < CloseButtons.Length; i++){
            CloseButtons[i].onClick.AddListener(() => { SetOnlyThisPanelActive(MainMenuPanel); });
        }
    }

    private void SetOnlyThisPanelActive(GameObject thisPanel)
    {
        for (int i = 0; i < allPanels.Length; i++){
            allPanels[i].SetActive(false);
        }
        thisPanel.SetActive(true);
    }
}

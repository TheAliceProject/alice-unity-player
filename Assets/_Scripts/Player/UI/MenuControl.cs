using System.IO;
using SFB;
using UnityEngine;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    public Button MainMenu;
    
    public Sprite menuSprite;
    public Sprite closeSprite;
    public Sprite homeSprite;

    public Button LoadMoreWorlds;
    public Button About;
    public Button Settings;
    public Button OpenWebsite;
    public Button loadNewWorldButton;

    public GameObject MainMenuPanel;
    public GameObject RecentWorldsPanel;
    public GameObject AboutPanel;
    public GameObject LoadMoreWorldsPanel;
    public GameObject SettingsPanel;
    public GameObject CreditedPanel;
    public GameObject LogoWithMenuPanel;
    
    private bool _isMenuOpen = false;

    void Start()
    {
        MainMenu.onClick.AddListener(() =>
        {
            if (_isMenuOpen)
                ShowRecentWorlds();
            else
                ShowMenu();
        });

        // Menu items
        About.onClick.AddListener(() =>
        {
            SetTopPanel(AboutPanel);
            MainMenu.image.sprite = homeSprite;
        });
        Settings.onClick.AddListener(() => {
            SetTopPanel(SettingsPanel);
            MainMenu.image.sprite = homeSprite;
        });

        LinkOpenWorldButton();
        // Button from Recent Worlds
        LoadMoreWorlds.onClick.AddListener(() =>
        {
            MainMenu.image.sprite = homeSprite;
            _isMenuOpen = true;
            CreditedPanel.SetActive(false);
            LoadMoreWorldsPanel.SetActive(true);
        });

        if(OpenWebsite)
            OpenWebsite.onClick.AddListener(() => { Application.OpenURL("https://www.alice.org/"); });
    }

    public virtual void LinkOpenWorldButton()
    {
        loadNewWorldButton.onClick.AddListener(() =>
        {
            string zipPath = "";
            var path = StandaloneFileBrowser.OpenFilePanel("Open File", "", WorldObjects.ProjectExt, false);
            if(path.Length > 0) {
                zipPath = path[0];
                zipPath = System.Uri.UnescapeDataString(zipPath);
            }
            
            if(System.IO.File.Exists(zipPath) == false) {
                throw new FileNotFoundException("UnityObjectParser.Select Failed to open File " + zipPath);
            }

            WorldObjects.GetParser().OpenWorld(zipPath);
        });
    }

    protected void ShowMenu()
    {
        SetTopPanel(LogoWithMenuPanel);
        MainMenu.image.sprite = closeSprite;
        MainMenuPanel.SetActive(true);
        RecentWorldsPanel.SetActive(false);
        _isMenuOpen = true;
    }

    private void ShowRecentWorlds()
    {
        SetTopPanel(LogoWithMenuPanel);
        MainMenu.image.sprite = menuSprite;
        MainMenuPanel.SetActive(false);
        RecentWorldsPanel.SetActive(true);
        _isMenuOpen = false;
    }

    protected virtual void SetTopPanel(GameObject thisPanel)
    {
        // Disable this, which is not a top panel with credits
        LoadMoreWorldsPanel.SetActive(false);

        // Enable parent of credits and top panel
        CreditedPanel.SetActive(true);

        // Disable all children of the top panel
        LogoWithMenuPanel.SetActive(false);
        SettingsPanel.SetActive(false);
        AboutPanel.SetActive(false);

        // Enable the selected panel
        thisPanel.SetActive(true);
    }
    
    public void DeactivateMainMenu()
    {
        MainMenu.gameObject.SetActive(false);
        LoadMoreWorldsPanel.SetActive(true);
    }
}

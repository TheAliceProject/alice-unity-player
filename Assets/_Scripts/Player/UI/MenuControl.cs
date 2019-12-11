using UnityEngine;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    public Button MainMenu;
    
    public Sprite menuSprite;
    public Sprite homeSprite;

    public Button RecentWorlds;
    public Button LoadMoreWorlds;
    public Button About;
    public Button Settings;
    public Button OpenWebsite;

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
        RecentWorlds.onClick.AddListener(() => { ShowRecentWorlds(); });
        About.onClick.AddListener(() => { SetTopPanel(AboutPanel); });
        Settings.onClick.AddListener(() => { SetTopPanel(SettingsPanel); });
        
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

    private void ShowMenu()
    {
        SetTopPanel(LogoWithMenuPanel);
        MainMenu.image.sprite = homeSprite;
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

    private void SetTopPanel(GameObject thisPanel)
    {
        // Disable this, which does not show credits
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
}

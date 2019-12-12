using UnityEngine;
using UnityEngine.UI;

public class MenuControlVR : MenuControl
{
    public GameObject fileBrowserPanel;

    public override void LinkOpenWorldButton()
    {
        loadNewWorldButton.onClick.AddListener(() =>
        {
            ShowFileBrowser();
        });
    }

    protected override void SetTopPanel(GameObject thisPanel)
    {
        fileBrowserPanel.SetActive(false);
        base.SetTopPanel(thisPanel);
    }

    private void ShowFileBrowser()
    {
        SetTopPanel(LogoWithMenuPanel);
        RecentWorldsPanel.SetActive(false);
        fileBrowserPanel.SetActive(true);
    }
}

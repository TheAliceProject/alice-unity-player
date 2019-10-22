using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecentWorldButton : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI fullText;

    private string filePath;

    public void SetText(string text)
    {
        int lastSlash = -1;
        lastSlash = Mathf.Max(text.LastIndexOf("/"), text.LastIndexOf("\\"));
        nameText.text = text.Substring(lastSlash + 1);
        fullText.text = text;
        filePath = text;
    }

    public string GetFilePath()
    {
        return filePath;
    }
}

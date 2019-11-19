﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class RecentWorldButton : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI fullText;
    public Image background;

    private string filePath;

    public void SetText(string text)
    {
        nameText.text = Path.GetFileName(text);
        fullText.text = text;
        filePath = text;

        string thumbnailPath = Application.persistentDataPath + "/" + Path.GetFileNameWithoutExtension(Path.GetFileName(text)) + "_thumb.png";
        if (File.Exists(thumbnailPath))
        {
            byte[] data = File.ReadAllBytes(thumbnailPath);
            Texture2D tex = null;
            tex = new Texture2D(160, 90);
            tex.LoadImage(data); //..this will auto-resize the texture dimensions.
            background.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0), 72);
        }

    }

    public string GetFilePath()
    {
        return filePath;
    }
}

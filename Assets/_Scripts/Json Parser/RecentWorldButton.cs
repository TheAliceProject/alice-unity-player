using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class RecentWorldButton : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI lastOpenedText;
    public TextMeshProUGUI authorText; // Unused for now
    public Image background;
    public BoxCollider collider;
    
    private RecentWorldData fileData = new RecentWorldData();

    public void SetData(RecentWorldData worldData)
    {
        nameText.text = Path.GetFileNameWithoutExtension(worldData.path);
        fileData.path = worldData.path;

        string thumbnailPath = Application.persistentDataPath + "/" + Path.GetFileNameWithoutExtension(worldData.path) + "_thumb.png";
        if (File.Exists(thumbnailPath))
        {
            byte[] data = File.ReadAllBytes(thumbnailPath);
            Texture2D tex = null;
            tex = new Texture2D(640, 360);
            tex.LoadImage(data); //..this will auto-resize the texture dimensions.
            background.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0), 72);
        }

        fileData.lastOpened = worldData.lastOpened;
        SetLastOpenedText();
    }

    public void ScaleText(float scaleFactor)
    {
        nameText.fontSize *= scaleFactor;
    }

    public string GetFilePath(){
        return fileData.path;
    }

    public void SetLastOpenedNow(){
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        fileData.lastOpened = (long)(System.DateTime.UtcNow - epochStart).TotalSeconds; // The current time
    }

    public long GetLastOpened(){
        return fileData.lastOpened;
    }

    private void SetLastOpenedText(){
        if(lastOpenedText) // else ignore
        {
            System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
            long currentTime = (long)(System.DateTime.UtcNow - epochStart).TotalSeconds; // The current time
            long secondsAgo = currentTime - fileData.lastOpened;
            if(fileData.lastOpened < 0)
                lastOpenedText.text = "";
            else if(secondsAgo < 60)
                lastOpenedText.text = secondsAgo.ToString() + " seconds ago";
            else if(secondsAgo / 60 < 60)
                lastOpenedText.text = (secondsAgo / 60).ToString() + " minutes ago";
            else if(secondsAgo / 3600 < 24)
                lastOpenedText.text = (secondsAgo / 3600).ToString() + " hours ago";
            else
                lastOpenedText.text = (secondsAgo / 86400).ToString() + " days ago";
        }
    }
}

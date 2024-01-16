using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using Alice.Storage;
using Alice.Tweedle.File;

public class RecentWorldButton : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI lastOpenedText;
    public TextMeshProUGUI authorText; // Unused for now
    public Image background;
    public BoxCollider collider;
    
    private RecentWorldData fileData = new();
    private ZipFile zipFile;
    private Stream fileStream;


    private void ExtractThumbnail(string fileName)
    {
        var thumbnailPath = Application.persistentDataPath + "/" + Path.GetFileNameWithoutExtension(fileName) + "_thumb.png";
        if (File.Exists(thumbnailPath)) {
            return;
        }
        StartCoroutine(StorageReader.Read(fileName, stream => {
            using (stream) {
                zipFile = new ZipFile(stream);
                using (zipFile) {
                    // Save thumbnail
                    var data = zipFile.ReadDataEntry("thumbnail.png");
                    if (data == null) {
                        return;
                    }
                    File.WriteAllBytes(thumbnailPath, data);
                }
            }
        }, _ => {
            // Ignored
        }));
    }

    public void SetData(RecentWorldData worldData)
    {
        nameText.text = Path.GetFileNameWithoutExtension(worldData.path);
        fileData.path = worldData.path;

        string thumbnailPath = Application.persistentDataPath + "/" + Path.GetFileNameWithoutExtension(worldData.path) + "_thumb.png";
        ExtractThumbnail(fileData.path);

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
        fileData.SetLastOpenedNow();
    }

    public long GetLastOpened(){
        return fileData.lastOpened;
    }

    private void SetLastOpenedText(){
        if(lastOpenedText) // else ignore
        {
            long secondsAgo = fileData.SecondsSinceLastOpened();
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

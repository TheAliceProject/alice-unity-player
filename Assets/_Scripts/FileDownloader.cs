using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;

public class FileDownloader : MonoBehaviour
{
    public string listFilesUrl = "http://yourserver.com/file_manager/list_files.php";
    public string downloadBaseUrl = "http://yourserver.com/file_manager/uploads/";
    //public TMP_Text updateText;

    void Awake()
    {
        StartCoroutine(GetFileListAndDownload());
    }

    IEnumerator GetFileListAndDownload()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(listFilesUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error fetching file list: " + webRequest.error);
                //updateText.text = "Error fetching file list: " + webRequest.error;
                yield break;
            }

            string json = webRequest.downloadHandler.text;
            Debug.Log("JSON Response: " + json);

            Dictionary<string, string> fileDict = new Dictionary<string, string>();
            try
            {
                fileDict = ParseJsonToDictionary(json);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error parsing JSON response: " + e.Message);
                // updateText.text = "Error parsing JSON response: " + e.Message;
                yield break;
            }

            if (fileDict == null || fileDict.Count == 0)
            {
                Debug.LogError("No files found in the response.");
                // updateText.text = "No files found in the response.";
                yield break;
            }

            List<string> fileList = new List<string>(fileDict.Values);
            Debug.Log("Fetched " + fileList.Count + " files");

            foreach (string fileName in fileList)
            {
                string fileUrl = downloadBaseUrl + fileName;
                Debug.Log("Downloading file: " + fileUrl);
                string filePath = Path.Combine(Application.persistentDataPath, fileName);
                yield return StartCoroutine(DownloadFile(fileUrl, filePath));
            }

            Debug.Log("All files downloaded successfully!");
            //SceneManager.LoadScene(1);
        }
    }

    IEnumerator DownloadFile(string url, string path)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error downloading file: " + webRequest.error);
                //updateText.text = "Error downloading file: " + webRequest.error;
            }
            else
            {
                byte[] data = webRequest.downloadHandler.data;
                SaveFile(path, data);
            }
        }
    }

    void SaveFile(string path, byte[] data)
    {
        try
        {
            File.WriteAllBytes(path, data);
            Debug.Log("File saved to: " + path);
            //updateText.text = "File saved to: " + path;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save file: " + e.Message);
            // updateText.text = "Failed to save file: " + e.Message;
        }
    }

    Dictionary<string, string> ParseJsonToDictionary(string json)
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        json = json.TrimStart('{').TrimEnd('}');
        string[] keyValuePairs = json.Split(',');

        foreach (string kvp in keyValuePairs)
        {
            string[] keyValue = kvp.Split(':');
            string key = keyValue[0].Trim().Trim('"');
            string value = keyValue[1].Trim().Trim('"');
            dict[key] = value;
        }

        return dict;
    }
}
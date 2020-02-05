using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class LoadAssetBundle : MonoBehaviour {

    [SerializeField] private bool uriRequest = false;
    [SerializeField] private string assetBundleName = "alice";
    [SerializeField] private string assentBundleVariant = "0";
    [SerializeField] private string assetName = "alice";

    private IEnumerator Start()
    {
        string assetBundle = assetBundleName + "." + assentBundleVariant;
        AssetBundle bundle;
        if (uriRequest)
        {
            string uri = "file:///" + Application.streamingAssetsPath + "/AssetBundles/" + assetBundle;
            Debug.Log(uri);
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(uri, 0);
            yield return request.SendWebRequest();
            bundle = DownloadHandlerAssetBundle.GetContent(request);
        } else
        {
            bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath + "/AssetBundles/", assetBundle));
        }
        LoadBundle(bundle);
    }

    private void LoadBundle(AssetBundle bundle)
    {
        if (bundle == null)
        {
            Debug.LogWarning("Failed to load AssetBundle: " + assetBundleName);
            return;
        }
        GameObject obj = bundle.LoadAsset<GameObject>(assetName);
        if (obj == null)
        {
            Debug.LogWarning("No asset with that name: " + assetName + " in bundle.");
            return;
        }
        Instantiate(obj);
    }
}

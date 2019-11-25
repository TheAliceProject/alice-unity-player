using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Alice.Tweedle.Parse;

public class CreditsControl : MonoBehaviour
{
    public TextMeshProUGUI versionString;
    // Start is called before the first frame update
    void Start()
    {
        if (versionString)
            versionString.text = string.Format("Player Ver {0}\nLibrary Ver {1}", PlayerLibraryManifest.Instance.PlayerLibraryVersion, PlayerLibraryManifest.Instance.GetLibraryVersion());
    }
}

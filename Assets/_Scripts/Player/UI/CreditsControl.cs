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
            versionString.text = string.Format("Player {0}\nCompatible with Alice {1}", PlayerLibraryManifest.Instance.playerVersion, PlayerLibraryManifest.Instance.aliceVersion);
    }
}

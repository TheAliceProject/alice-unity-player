using UnityEngine;

public class LauncherControl : MonoBehaviour
{
    public SystemSettings settings;

    void Start()
    {
        settings.ReadFromPrefs();
    }
}

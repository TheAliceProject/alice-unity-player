using UnityEngine;

namespace Crosstales.UI
{
    /// <summary>Static Button Manager.</summary>
    public class StaticManager : MonoBehaviour
    {
        public string AssetstoreURL = "https://goo.gl/qwtXyb";

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void OpenCrosstales()
        {
            Application.OpenURL(Common.Util.BaseConstants.ASSET_AUTHOR_URL);
        }

        public void OpenAssetstore()
        {
            Application.OpenURL(AssetstoreURL);
        }
    }
}
// © 2017-2018 crosstales LLC (https://www.crosstales.com)
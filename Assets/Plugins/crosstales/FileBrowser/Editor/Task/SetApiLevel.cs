using UnityEditor;
using UnityEngine;

namespace Crosstales.FB.EditorExt
{
    /// <summary>Sets the required .NET API level.</summary>
    [InitializeOnLoad]
    public static class SetApiLevel
    {
        private const ApiCompatibilityLevel TARGET_COMPATIBILITY =
#if UNITY_2018
            ApiCompatibilityLevel.NET_4_6;
#else
            ApiCompatibilityLevel.NET_2_0;
#endif // UNITY_2018

        #region Constructor

        static SetApiLevel()
        {
#if UNITY_STANDALONE_WIN

#if UNITY_2017 || UNITY_2018
            if (PlayerSettings.GetApiCompatibilityLevel(BuildTargetGroup.Standalone) != TARGET_COMPATIBILITY) {
                PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Standalone, TARGET_COMPATIBILITY);
#else
            if (PlayerSettings.apiCompatibilityLevel != TARGET_COMPATIBILITY)
            {
                PlayerSettings.apiCompatibilityLevel = TARGET_COMPATIBILITY;
#endif
                Debug.LogWarning("File Browser: API level changed to ." + TARGET_COMPATIBILITY.ToString()) ;
            }

#endif
        }

        #endregion
    }
}
// © 2017-2018 crosstales LLC (https://www.crosstales.com)
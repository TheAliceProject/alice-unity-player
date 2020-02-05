using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Build;
#if UNITY_EDITOR_OSX
using UnityEditor.iOS.Xcode;
#endif
using TriLib;
using System;
using System.IO;
using System.Linq;
#if UNITY_2018_2_OR_NEWER
using UnityEditor.Build.Reporting;
#endif
[InitializeOnLoad]
#if UNITY_2018_1_OR_NEWER
public class TriLibCheckPlugins : IPreprocessBuildWithReport, IPostprocessBuildWithReport
#else
public class TriLibCheckPlugins : IPreprocessBuild, IPostprocessBuild
#endif
{
    public const string DebugSymbol = "TRILIB_OUTPUT_MESSAGES";
    public const string ZipSymbol = "TRILIB_USE_ZIP";
    public const string DebugEnabledMenuPath = "TriLib/Enable Debug";
    public const string ZipEnabledMenuPath = "TriLib/Enable Zip loading";
#if UNITY_EDITOR_OSX
	public const string XCodeProjectPath = "Libraries/TriLib/TriLib/Plugins/iOS";
#endif
    public static bool PluginsLoaded { get; private set; }

    public int callbackOrder
    {
        get { return 1000; }
    }

#if UNITY_EDITOR_OSX
	private static bool _iosFileSharingEnabled;
#endif

    static TriLibCheckPlugins()
    {
        try
        {
            CheckForOldVersions();
            AssimpInterop.ai_IsExtensionSupported(".3ds");
            PluginsLoaded = true;
        }
        catch (Exception exception)
        {
            if (exception is DllNotFoundException)
            {
                if (EditorUtility.DisplayDialog("TriLib plugins not found", "TriLib was unable to find the native plugins.\n\nIf you just imported the package, you will have to restart Unity editor.\n\nIf you click \"Ask to save changes and restart\", you will be prompted to save your changes (if there is any) then Unity editor will restart.\n\nOtherwise, you will have to save your changes and restart Unity editor manually.", "Ask to save changes and restart", "I will do it manually"))
                {
                    EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                    var projectPath = Directory.GetParent(Application.dataPath);
                    EditorApplication.OpenProject(projectPath.FullName);
                }
            }
        }
    }

    static void CheckForOldVersions()
    {
        var assimpInteropAssetGuids = AssetDatabase.FindAssets("AssimpInterop t:Script");
        if (assimpInteropAssetGuids.Length > 0)
        {
            try
            {
                var assimpInteropAssetPath = AssetDatabase.GUIDToAssetPath(assimpInteropAssetGuids[0]);
                if (assimpInteropAssetPath.EndsWith("AssimpInterop.cs"))
                {
                    var assimpInteropDirectory = FileUtils.GetFileDirectory(assimpInteropAssetPath);
                    var scriptsDirectory = Directory.GetParent(assimpInteropDirectory);
                    if (scriptsDirectory != null && scriptsDirectory.Parent != null)
                    {
                        var pluginsDirectories = scriptsDirectory.Parent.GetDirectories("Plugins", SearchOption.TopDirectoryOnly);
                        if (pluginsDirectories.Length > 0)
                        {
                            var hasDeprecatedFolder = false;
                            var pluginsDirectory = pluginsDirectories[0];
                            if (pluginsDirectory.GetDirectories("Windows").Length == 0 && pluginsDirectory.GetDirectories("OSX").Length == 0 && pluginsDirectory.GetDirectories("Linux").Length == 0)
                            {
                                hasDeprecatedFolder = true;
                            }
                            else
                            {
                                var webGLDirectory = pluginsDirectory.GetDirectories("WebGL");
                                if (webGLDirectory.Length > 0 && webGLDirectory[0].GetDirectories("Emscripten1.37.3").Length > 0 || webGLDirectory[0].GetDirectories("Emscripten1.37.33").Length > 0 || webGLDirectory[0].GetDirectories("Emscripten1.37.40").Length > 0 || webGLDirectory[0].GetDirectories("Emscripten1.38.11").Length > 0)
                                {
                                    hasDeprecatedFolder = true;
                                }
                            }
                            if (hasDeprecatedFolder)
                            {
                                EditorUtility.DisplayDialog("TriLib", "Looks like you have an old TriLib install mixed with the newest update.\nPlease make a clean TriLib install (remove all TriLib files and install TriLib again).", "Ok");
                            }
                        }
                    }
                }
            }
            catch
            {

            }
        }
    }

    [MenuItem(DebugEnabledMenuPath)]
    public static void DebugEnabled()
    {
        GenerateSymbolsAndUpdateMenu(DebugEnabledMenuPath, DebugSymbol, true);
    }

    [MenuItem(DebugEnabledMenuPath, true)]
    public static bool DebugEnabledValidate()
    {
        GenerateSymbolsAndUpdateMenu(DebugEnabledMenuPath, DebugSymbol, false);
        return true;
    }

    [MenuItem(ZipEnabledMenuPath)]
    public static void ZipEnabled()
    {
        GenerateSymbolsAndUpdateMenu(ZipEnabledMenuPath, ZipSymbol, true);
    }

    [MenuItem(ZipEnabledMenuPath, true)]
    public static bool ZipEnabledValidate()
    {
        GenerateSymbolsAndUpdateMenu(ZipEnabledMenuPath, ZipSymbol, false);
        return true;
    }

    private static void GenerateSymbolsAndUpdateMenu(string menuPath, string checkingDefineSymbol, bool generateSymbols, bool? forceDefinition = null)
    {
        var isDefined = false;
        var defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        var defineSymbolsArray = defineSymbols.Split(';');
        var newDefineSymbols = generateSymbols ? string.Empty : null;
        foreach (var defineSymbol in defineSymbolsArray)
        {
            var trimmedDefineSymbol = defineSymbol.Trim();
            if (trimmedDefineSymbol == checkingDefineSymbol)
            {
                isDefined = true;
                if (!generateSymbols)
                {
                    break;
                }
                continue;
            }
            if (generateSymbols)
            {
                newDefineSymbols += string.Format("{0};", trimmedDefineSymbol);
            }
        }
        if (generateSymbols)
        {
            if (forceDefinition.HasValue && forceDefinition.GetValueOrDefault() || !forceDefinition.HasValue && !isDefined)
            {
                newDefineSymbols += string.Format("{0};", checkingDefineSymbol);
            }
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, newDefineSymbols);
        }
        Menu.SetChecked(menuPath, generateSymbols ? !isDefined : isDefined);
    }

#if UNITY_2018_1_OR_NEWER
    public void OnPreprocessBuild(BuildReport report)
    {
        var buildTarget = report.summary.platform;
#else
    public void OnPreprocessBuild(BuildTarget target, string path)
    {
        var buildTarget = target;
#endif
        if (buildTarget == BuildTarget.iOS)
        {
            var pluginsLabel = UnityEditorInternal.InternalEditorUtility.inBatchMode || EditorUtility.DisplayDialog("TriLib", "Building to iOS Device (iPhone, iPad) or iOS Simulator?", "Device", "Simulator") ? "_TriLib_iOS_Device_" : "_TriLib_iOS_Simulator_";
#if UNITY_EDITOR_OSX
		    _iosFileSharingEnabled = UnityEditorInternal.InternalEditorUtility.inBatchMode || EditorUtility.DisplayDialog ("TriLib", "Enable iOS File Sharing?", "Yes", "No");
#endif
            var iOSImporters = PluginImporter.GetImporters(BuildTarget.iOS);
            foreach (var importer in iOSImporters)
            {
                if (importer.isNativePlugin)
                {
                    var asset = AssetDatabase.LoadMainAssetAtPath(importer.assetPath);
                    var labels = AssetDatabase.GetLabels(asset);
                    if (labels.Contains("_TriLib_iOS_"))
                    {
                        importer.SetIncludeInBuildDelegate(assetPath => labels.Contains(pluginsLabel));
                    }
                }
            }
            if (!UnityEditorInternal.InternalEditorUtility.inBatchMode)
            {
#if UNITY_EDITOR_OSX
                EditorUtility.DisplayDialog("TriLib", "Warning: Bitcode is not supported by TriLib and will be disabled.", "Ok");
#else
                EditorUtility.DisplayDialog("TriLib", "Warning:\nBitcode is not supported. You should disable it on your project settings.\nZLIB library should be included in your project frameworks.", "Ok");
#endif
            }
        }
#if !UNITY_2018_1_OR_NEWER
        else if (buildTarget == BuildTarget.Android)
        {
            var androidImporters = PluginImporter.GetImporters(BuildTarget.Android);
            foreach (var importer in androidImporters)
            {
                if (importer.isNativePlugin)
                {
                    var asset = AssetDatabase.LoadMainAssetAtPath(importer.assetPath);
                    var labels = AssetDatabase.GetLabels(asset);
                    if (labels.Contains("_TriLib_Android_") && labels.Contains("_TriLib_Android_ARM64_"))
                    {
                        importer.SetIncludeInBuildDelegate(assetPath => false);
                    }
                }
            }
        }
#endif
        var allImporters = PluginImporter.GetImporters(buildTarget);
        foreach (var importer in allImporters)
        {
            if (!importer.isNativePlugin)
            {
                var asset = AssetDatabase.LoadMainAssetAtPath(importer.assetPath);
                var labels = AssetDatabase.GetLabels(asset);
                if (labels.Contains("_TriLib_ZIP_"))
                {
#if TRILIB_USE_ZIP
                    importer.SetIncludeInBuildDelegate(assetPath => PlayerSettings.GetScriptingBackend(BuildPipeline.GetBuildTargetGroup(buildTarget)) != ScriptingImplementation.WinRTDotNET);
#else
                    importer.SetIncludeInBuildDelegate(assetPath => false);
#endif
                }
            }
        }
    }

#if UNITY_2018_1_OR_NEWER
    public void OnPostprocessBuild(BuildReport report)
    {
#if UNITY_EDITOR_OSX
        var buildTarget = report.summary.platform;
        var buildPath = report.summary.outputPath;
#endif
#else
    public void OnPostprocessBuild(BuildTarget target, string path)
    {
#if UNITY_EDITOR_OSX
        var buildTarget = target;
        var buildPath = path;
#endif
#endif
#if UNITY_EDITOR_OSX
		if (buildTarget == BuildTarget.iOS) {
			var pbxProject = new PBXProject ();
			var pbxProjectPath = PBXProject.GetPBXProjectPath (buildPath);
			pbxProject.ReadFromFile (pbxProjectPath);
			var targetGuid = pbxProject.TargetGuidByName (PBXProject.GetUnityTargetName ());
			pbxProject.AddFrameworkToProject (targetGuid, "libz.dylib", true);
			pbxProject.AddFrameworkToProject (targetGuid, "libz.tbd", true);
			pbxProject.SetBuildProperty (targetGuid, "ENABLE_BITCODE", "NO");
			pbxProject.WriteToFile (pbxProjectPath);
			if (_iosFileSharingEnabled) {
				var plistPath = buildPath + "/info.plist";
				var plist = new PlistDocument ();
				plist.ReadFromFile (plistPath);
				var dict = plist.root.AsDict ();
				dict.SetBoolean ("UIFileSharingEnabled", true);
				plist.WriteToFile (plistPath);
			}
		}
#endif
    }
}

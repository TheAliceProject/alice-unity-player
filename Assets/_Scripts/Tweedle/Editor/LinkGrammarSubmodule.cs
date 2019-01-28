using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Diagnostics;
using System.IO;


public static class LinkGrammarSubmodule {

    private static readonly string k_ScriptsDir = "Assets/_Scripts/Tweedle/Grammar";
    private static readonly string k_SubmoduleDir = "submodules/tweedle/Grammar/CSharp/Alice/Tweedle";

    [InitializeOnLoadMethod]
    public static void LinkSubmodule() {

        if (Directory.Exists(GetFullScriptsPath())) {
            return;
        }

        #if UNITY_EDITOR_WIN
        ExecuteCommand("cmd.exe", string.Format("/c mklink /j \"{0}\" \"{1}\"", GetWindowsPath(k_ScriptsDir), GetWindowsPath(k_SubmoduleDir)) );
        #elif UNITY_EDITOR_OSX
        ExecuteCommand("ln", string.format("-s \"{0}\" \"{1}\"", k_SubmoduleDir, k_ScriptDir);
        #endif
    }

    [MenuItem("Tools/Re-Link Grammar Submodule")]
    public static void ReLinkSubmodule() {
        var path = GetFullScriptsPath();
        if (Directory.Exists(path)) {
            #if UNITY_EDITOR_WIN
            ExecuteCommand("cmd.exe", string.Format("/c rd /s /q \"{0}\"", GetWindowsPath(k_ScriptsDir)) );
            #elif UNITY_EDITOR_OSX
            ExecuteCommand("rm", string.format("-r \"{0}\"", k_ScriptDir);
            #endif
        }

        LinkSubmodule();
    }

    private static string GetFullScriptsPath() {
        return Application.dataPath + "/../" + k_ScriptsDir;
    }

    private static string GetWindowsPath(string path) {
        return path.Replace("/", "\\");
    }

    private static int ExecuteCommand(string command, string arguments, bool log = true) {
        int exitCode;
        ProcessStartInfo processInfo;
        Process process;

        processInfo = new ProcessStartInfo(command, arguments);
        processInfo.WorkingDirectory = Application.dataPath + "/../";
        processInfo.CreateNoWindow = true;
        processInfo.UseShellExecute = false;

        string output = null, error = null;
        if (log) {
            // *** Redirect the output ***
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;
        }

        process = Process.Start(processInfo);

        if (log) {
            output = process.StandardOutput.ReadToEnd();
            error = process.StandardError.ReadToEnd();
        }
        process.WaitForExit();

        exitCode = process.ExitCode;

        if (log) {
            if (!string.IsNullOrEmpty(output)) {
                UnityEngine.Debug.Log(output);
            }

            if (!string.IsNullOrEmpty(error)) {
                UnityEngine.Debug.Log(error);
            }

            if (exitCode != 0) {
                UnityEngine.Debug.Log("ExitCode: " + exitCode.ToString());
            }
        }

        process.Close();

        return exitCode;
    }
}

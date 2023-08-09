using System;
using UnityEditor;
using UnityEditor.Build.Reporting;

public class BuildScript {

   static void PerformPlayerBuild() {
      string[] args = System.Environment.GetCommandLineArgs();

      BuildTarget target = BuildTarget.NoTarget;
      string platform = "";
      string extension = "";
      bool? isDevBuild = null;

      bool error = false;

      // Assume the first 9 args are for Unity.exe, so start checking with the 10th one
      for (int i = 9; i < args.Length; i++) {
         string arg = args[i];
         
         if (arg == "-dev") {
            if (isDevBuild != null) {
               Console.WriteLine($"Duplicate flag: -dev");
               error = true;
            } else {
               isDevBuild = true;
            }
         } else if (arg == "-platform") {
            if (i + 1 < args.Length) {
               platform = args[i + 1];

               if (Enum.TryParse<BuildTarget>(platform, out target)) {
                  switch (target) {
                     case BuildTarget.StandaloneWindows:
                     case BuildTarget.StandaloneWindows64:
                        extension = ".exe";
                        break;
                     case BuildTarget.StandaloneLinux64:
                        extension = ".x86_64";
                        break;
                     case BuildTarget.StandaloneOSX:
                        extension = ".app";
                        break;
                     case BuildTarget.WebGL:
                        extension = "";
                        break;
                     default:
                        Console.WriteLine($"{target} builds are not yet supported");
                        error = true;
                        break;
                  }
               } else {
                  Console.WriteLine($"Unsupported platform: {platform}");
                  error = true;
               }

               i++;
            } else {
               Console.WriteLine($"No platform specified for flag -platform");
               error = true;
            }
         } else {
            Console.WriteLine($"Unknown parameter: {arg}");
            error = true;
         }
      }

      string buildFolder = platform;

      if (isDevBuild == null) {
         isDevBuild = false;
      }

      string[] playerScenes = {
         "Assets/_Scenes/Tweedle Project.unity"
      };


      if (!error) {
         BuildTargetGroup targetGroup = BuildPipeline.GetBuildTargetGroup(target);
         EditorUserBuildSettings.SwitchActiveBuildTarget(targetGroup, target);
         EditorUserBuildSettings.standaloneBuildSubtarget = StandaloneBuildSubtarget.Player;

         BuildPlayerOptions buildOptions = new BuildPlayerOptions();
         buildOptions.scenes = playerScenes;
         buildOptions.locationPathName = "./Build/" + buildFolder + "/AlicePlayer" + extension;
         buildOptions.target = target;
         buildOptions.subtarget = (int) EditorUserBuildSettings.standaloneBuildSubtarget;
         buildOptions.options = isDevBuild.Value ? BuildOptions.Development : BuildOptions.None;

         BuildReport report = BuildPipeline.BuildPlayer(buildOptions);

         if (report.summary.result != BuildResult.Succeeded) {
            error = true;
         }
      }

      if (error) {
         Console.WriteLine($"Build failed");
      } else {
         Console.WriteLine($"Build completed successfully");
      }
   }

}

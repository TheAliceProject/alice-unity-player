# Alice Player
A stand alone program to run worlds exported from the Alice 3 IDE.

Available for desktop on Mac, Windows, and Linux, and for VR on Oculus and Vive.

---
Build using Unity 2021.3.27f1

## Assets

### _Scenes/Tweedle Project.unity
The root scene for the project

### _Scripts
The `Dynamic Import` folder has the code for dynamically importing models both through asset bundles as well as GLTFUtility.
To create asset bundles from the models the script can be run in Unity (Assets > Build AssetBundles).

The `Tweedle` folder contains code for reading and running Tweedle, the text representation of Alice code. It is in the Java/C# family.
- The `File` folder has class definitions for reading in manifests.
- The `Grammar` folder has the Antlr grammar.
- The `Player` folder supports the excution of Tweedle.
- The `Parse` folder has tests for testing parsing, code to parse an a3w project, code to select the zip folder, code to parse twe files, and the class to hold all the parsed information.
- The inner `Tweedle` folder holds the specific language elements.

### StreamingAssets/Default
- The Scene Graph Library - the common expression of the scene logic from the IDE expressed in Tweedle.
- `bundledWorlds.txt` is a list of world files in this directory that will be available for loading when the player is launched. To be found they must be explicitly listed.

### ThirdParty
The supporting libraries, including:
- Antlr - for the Antlr parsing
- BeauRoutine - for management of routines
- FlyingText3D - for say dialogs
- JSON - for reading Json files the non-Unity way

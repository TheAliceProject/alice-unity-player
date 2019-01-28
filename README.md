# alice-unity-player
Build using Unity Version 2017.3.0f3
# _Scenes
The Model Import folder holds the scenes regarding dynamically importing models. The scene labeled AssetBundle was created to test loading in asset bundles, Unity's way of compact storage of files, however at Alice's current state this feature will not be used. For future versions of Alice it may be useful to store the gallery as individual asset bundles, and package those with the a3p folder rather than the model because this ensures the model will import properly as well as saving space for the user and time on import (since TriLib won't be as fast as asset bundles). The scene labeled TriLib is for testing import of meshes/skinned-meshes through the TriLib library.
The Project Import folder holds the scenes for testing json files/zip-files import, parsing a json file/string, as well as parsing a a3p project into Tweedle.
# _Scripts
The Dynamic Import folder has the code for dynamically importing models both through asset bundles as well as TriLib. To properly use the asset bundle scene, the create asset bundles script must be run to generate the asset bundles of the models (Assets > CreateAssetBundles).
The Json Parser folder is for testing the import/export functionality of json parsing in Unity. It has scripts which connect directly to Unity GameObjects for testing, as well as a test script for parsing a tweedle file.
The Tweedle folder contains all code for Tweedle and is self contained. The File folder has class defintions for reading in manifests; the Grammar folder has the Antlr grammar; the Linker folder has a Linker script (no functionality); Tweedle folder with the Tweedle classes; and the Parsing folder has tests for testing parsing, code to parse an a3p project, code to select the zip folder, code to parse twe files, and the class to hold all the parsed information. The UnityObjectParser is the script to be placed in the scene for file selection.
# Models
Has the models used to test the dynamic model import.
# Plugins
Antlr - for the Antlr parsing.
crosstales - for the file explorer selection when selecting files.
JSON - for reading Json files the non-Unity way.
TriLib - for dynamic model import.
System.IO.Compression.FileSystem - for extracting zip files.
# Prefabs
Json File - the UI for selecting and reading Json files.
Json Zip - the UI for selecting and reading Json zip files.
Tweedle Project - the UI for selecting and reading a3p files into a TweedleSystem.

# IMPORTANT: Tweedle Submodule
This repo uses git submodules. A symlink is required for the generated tweedle lexer and parser code.

Windows:
`cd <repo/directory>`
`mklink /j Assets\_Scripts\Tweedle\Grammar submodules\tweedle\Grammar\CSharp\Alice\Tweedle`

Mac:
`cd <repo/directory>`
`ln -s $PWD/submodules/tweedle/Grammar/CSharp/Alice/Tweedle Assets/_Scripts/Tweedle/Grammar`

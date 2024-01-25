#!/bin/bash

# TODO read this from a single shared source
VERSION_TAG="beta.23"

ARCH=$(uname)

echo Detected architecture is "$ARCH"

if [[ $ARCH =~ ^MINGW ]]; then # uname returns this in Git Bash
	echo Detected Windows architecture
	OS=Windows
elif [[ $ARCH =~ ^Darwin ]]; then
	echo Detected Mac architecture
	OS=Mac
elif [[ $ARCH =~ ^Linux ]]; then
	echo Detected Linux architecture
	OS=Linux
else
	echo Unknown architecture: "$ARCH"
fi

echo Detected OS: "$OS"

BASE_DIR=$(dirname "$0")

# This extracts the Unity version from ProjectVersion.txt
UNITY_VERSION=$(sed -n "s/m_EditorVersion: //p" "$BASE_DIR/ProjectSettings/ProjectVersion.txt")

echo Unity version: "$UNITY_VERSION"

if [ "$OS" = "Windows" ]; then
	UNITY_BINARY="/c/Program Files/Unity/Hub/Editor/$UNITY_VERSION/Editor/Unity.exe"
elif [ "$OS" = "Mac" ]; then
	UNITY_BINARY="/Applications/Unity/Hub/Editor/$UNITY_VERSION/Unity.app/Contents/MacOS/Unity"
elif [ "$OS" = "Linux" ]; then
	UNITY_BINARY="/home/$(whoami)/Unity/Hub/Editor/$UNITY_VERSION/Editor/Unity"
else
	echo Could not detect the operating system
fi

echo Unity binary path: "$UNITY_BINARY"

function zip_clean() {
  FILE_NAME="../Alice.Player-$VERSION_TAG-$1.zip"
  echo Compressing "$FILE_NAME"
  cd "$BASE_DIR"/Build/"$1" || exit
  zip -r "$FILE_NAME" ./* -x "Alice Player_BurstDebugInformation_DoNotShip/*" -x "log.txt" -x "*.DS_Store" -x "_MACOSX"
  cd ../..
}

build_for_platform () {
  echo Cleaning up build directory for "$1"
  rm -Rf "$BASE_DIR"/Build/"$1"
  mkdir "$BASE_DIR"/Build/"$1"
	echo Alice Unity Player build for "$1" started...

	# May add -dev flag for development build
	"$UNITY_BINARY" -quit -batchmode -projectPath "$BASE_DIR" -executeMethod BuildScript.PerformPlayerBuild -logFile "$BASE_DIR"/Build/"$1"/log.txt -platform "$1"
  if [ "$1" = "StandaloneOSX" ]; then
    echo Notarizing Mac app and bundling into dmg
    ./notarizeMacBuild.sh
    mv "$BASE_DIR/Build/StandaloneOSX/Alice Player.dmg" "$BASE_DIR/Build/Alice Player-$VERSION_TAG.dmg"
  else
    zip_clean "$1"
  fi	  
	printf "Alice Unity Player build for %s finished successfully\n\n", "$1"
}

# Note that the platform must match one of the values for the UnityEditor.BuildTarget enum

#Currently supported platforms:
# StandaloneWindows64, StandaloneOSX, StandaloneLinux64, WebGL, Android
if [ "$1" ]; then
	build_for_platform "$1"
else
	echo Building all
	build_for_platform StandaloneWindows64
	build_for_platform StandaloneLinux64
	build_for_platform WebGL
	build_for_platform Android
	build_for_platform StandaloneOSX
fi

echo Build script completed

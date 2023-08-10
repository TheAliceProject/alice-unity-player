#!/bin/bash

ARCH=$(uname)

echo Detected architecture is $ARCH 

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
	echo Unknown architecture: $ARCH
fi

echo Detected OS: $OS

BASE_DIR=$(dirname $0)

echo Cleaning up build directories

rm -Rf $BASE_DIR/Build
mkdir $BASE_DIR/Build

echo Starting Unity build

# This extracts the Unity version from ProjectVersion.txt
UNITY_VERSION=$(sed -n "s/m_EditorVersion: //p" $BASE_DIR/ProjectSettings/ProjectVersion.txt)

echo Unity version: $UNITY_VERSION

# Note that the TARGET_PLATFORM value must match one of the values for the UnityEditor enum

if [ $OS = "Windows" ]; then
	UNITY_BINARY="/c/Program Files/Unity/Hub/Editor/$UNITY_VERSION/Editor/Unity.exe"
elif [ $OS = "Mac" ]; then
	UNITY_BINARY="/Applications/Unity/Hub/Editor/$UNITY_VERSION/Unity.app/Contents/MacOS/Unity"
elif [ $OS = "Linux" ]; then
	UNITY_BINARY="/home/$(whoami)/Unity/Hub/Editor/$UNITY_VERSION/Editor/Unity"
else
	echo Could not detect the operating system
fi

#TARGET_PLATFORM=StandaloneWindows64
#TARGET_PLATFORM=StandaloneOSX
#TARGET_PLATFORM=StandaloneLinux64
#TARGET_PLATFORM=WebGL
TARGET_PLATFORM=Android

echo Unity binary path: $UNITY_BINARY
printf "Target Platform: $TARGET_PLATFORM\n\n"

printf "Building Alice Unity Player...\n\n"

echo Alice Unity Player build for $TARGET_PLATFORM started
"$UNITY_BINARY" -quit -batchmode -projectPath $BASE_DIR -executeMethod BuildScript.PerformPlayerBuild -logFile $BASE_DIR/Build/$TARGET_PLATFORM/log.txt -dev -platform $TARGET_PLATFORM
printf "Alice Unity Player build for $TARGET_PLATFORM finished successfully\n\n"

echo Unity build process complete

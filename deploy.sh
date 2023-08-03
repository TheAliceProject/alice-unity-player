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
mkdir $BASE_DIR/Build/Client
mkdir $BASE_DIR/Build/Server

echo Starting Unity build

# This extracts the Unity version from ProjectVersion.txt
UNITY_VERSION=$(sed -n "s/m_EditorVersion: \([[:digit:]]\+.[[:digit:]]\+.[[:alnum:]]\+\)/\1/p" $BASE_DIR/ProjectSettings/ProjectVersion.txt)

echo Unity version: $UNITY_VERSION

# Note that the TARGET_PLATFORM value must match one of the values for the UnityEditor enum

if [ $OS = "Windows" ]; then
	UNITY_BINARY="/c/Program Files/Unity/Hub/Editor/$UNITY_VERSION/Editor/Unity.exe"
	TARGET_PLATFORM=StandaloneWindows64
elif [ $OS = "Mac" ]; then
	UNITY_BINARY="/Applications/Unity/Hub/Editor/$UNITY_VERSION/Unity.app/Contents/MacOS/Unity"
	TARGET_PLATFORM=StandaloneOSX
elif [ $OS = "Linux" ]; then
	UNITY_BINARY="/home/dportnoy/Unity/Hub/Editor/$UNITY_VERSION/Editor/Unity"
	TARGET_PLATFORM=StandaloneLinux64
else
	echo Could not detect the operating system
fi

echo Unity binary path: $UNITY_BINARY
printf "Target Platform: $TARGET_PLATFORM\n\n"

echo Building Alice Unity Player...
"$UNITY_BINARY" -quit -batchmode -projectPath $BASE_DIR -executeMethod BuildScript.PerformPlayerBuild -logFile $BASE_DIR/Build/log.txt -dev -platform $TARGET_PLATFORM
echo Alice Unity Player build finished successfully

echo Unity build process complete

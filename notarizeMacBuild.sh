#!/bin/bash

ARCH=$(uname)

APPLE_ID="xxxx@xxxx.xxx"
APPLICATION_PASSWORD="xxxx-xxxx-xxxx-xxxx"
APPLE_TEAM_ID="xxxxxxxxxx"
SIGNER="Developer ID Application: Carnegie Mellon University ($APPLE_TEAM_ID)"

if [[ $ARCH =~ ^Darwin ]]; then
	echo "==== Detected Mac architecture"
else
	echo "==== Exiting - Unable to notarize Mac builds on architecture: $ARCH"
	exit
fi

BASE_DIR=$(dirname "$0")
MAC_BUILD_DIR=$BASE_DIR/Build/StandaloneOSX
MAC_DMG_DIR=$MAC_BUILD_DIR/bundle

MAC_PLAYER="$MAC_BUILD_DIR/Alice Player.app"
MAC_BUNDLE="$MAC_BUILD_DIR/Alice Player.dmg"

if [ -d "$MAC_PLAYER" ]; then
	if [ ! -d "$MAC_DMG_DIR" ]; then
		mkdir "$MAC_DMG_DIR"
	fi

	echo "==== Code signing Alice Player.app"
	codesign --deep --force --verify --verbose --timestamp --options runtime --entitlements "Alice Player.entitlements" --sign "$SIGNER" "$MAC_PLAYER"

	# Move the player to the temporary bundling directory 
	mv "$MAC_PLAYER" "$MAC_DMG_DIR"

	echo "==== Wrapping player in Alice Player.dmg"
	hdiutil create -volname "Alice Player" -srcfolder "$MAC_DMG_DIR" -ov "$MAC_BUNDLE"

	# Return the player and remove the temporary directory
	mv "$MAC_DMG_DIR"/* "$MAC_BUILD_DIR"
	rm -rf "$MAC_DMG_DIR"
	
	echo "==== Notarizing Alice Player.dmg"
  xcrun notarytool submit "$MAC_BUNDLE" --wait --apple-id "$APPLE_ID" --password "$APPLICATION_PASSWORD" --team-id "$APPLE_TEAM_ID"
else
	echo "==== Exiting - No Mac build found"
	exit
fi

echo "==== Code signing Mac build complete"

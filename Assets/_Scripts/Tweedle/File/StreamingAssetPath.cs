using UnityEngine;
using System.Collections;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Alice.Tweedle.File
{
    [System.Serializable]
    public struct StreamingAssetPath : IEquatable<StreamingAssetPath>  {
        public string relativePath;
        public string fullPath { get { return Application.streamingAssetsPath + "/" + relativePath; } }

        public bool Equals(StreamingAssetPath other) {
            return relativePath == other.relativePath;
        }

        public static implicit operator string (StreamingAssetPath inPath)
        {
            return inPath.relativePath;
        }

        public static implicit operator StreamingAssetPath (string inPath)
        {
            return new StreamingAssetPath() { relativePath = inPath };
        }

    }


    #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(StreamingAssetPath))]
    public class StreamingAssetPathDrawer : PropertyDrawer {

        private const string k_AssetPrefix = "Assets/StreamingAssets/";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var pathProp = property.FindPropertyRelative("relativePath");

            var evnt = Event.current;

            if (position.Contains(evnt.mousePosition) && (evnt.type == EventType.DragUpdated || evnt.type == EventType.DragPerform)) {
                DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
            
                
                if (evnt.type == EventType.DragPerform) {

                    var paths = DragAndDrop.paths;

                    if (paths.Length == 1 && paths[0].StartsWith(k_AssetPrefix)) {
                        DragAndDrop.AcceptDrag();
                        pathProp.stringValue = paths[0].Substring(k_AssetPrefix.Length);
                        property.serializedObject.ApplyModifiedProperties();
                    }
                }
            }


            EditorGUI.PropertyField(position, pathProp, new GUIContent(property.displayName));   
            
        }
    }
    #endif
}
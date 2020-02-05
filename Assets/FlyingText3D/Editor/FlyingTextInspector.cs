// FlyingText3D 2.2
// ©2017 Starscene Software. All rights reserved. Redistribution without permission not allowed.

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using FlyingText3D;

[CustomEditor (typeof(FlyingText))]
public class FlyingTextInspector : Editor {
	
	static bool showFonts = true;
	static bool showCharacterSettings = true;
	static bool showTextSettings = true;
	static bool showGOsettings = true;
	static string text = "";
	static Vector3 position = Vector3.zero;
	static Font ttfFile;
	static bool initialized = false;
	FlyingText t;
	
	void OnEnable () {
		t = target as FlyingText;
	}
	
	public override void OnInspectorGUI () {
#if UNITY_3_5
		EditorGUIUtility.LookLikeControls (90);
#else
		EditorGUIUtility.labelWidth = 90;
#endif
		showFonts = EditorGUILayout.Foldout (showFonts, "Font settings");
		if (showFonts) {
			var hasChanged = false;
			GUILayout.BeginVertical ("Box");
			if (GUILayout.Button ("+ Add Font")) {
				if (t.m_fontData == null) {
					t.m_fontData = new List<FontData>();
				}
				t.m_fontData.Add (new FontData());
				EditorUtility.SetDirty (t);
			}
			if (t.m_fontData != null) {
				if (t.m_fontData.Count == 0) {
					EditorGUILayout.HelpBox ("You need to add at least one font in order for FlyingText3D to work. Note that TTF fonts must be renamed to end with \"bytes\". You can use the convert .ttf to .bytes utility below to do this.", MessageType.Warning);
				}
				for (int i = 0; i < t.m_fontData.Count; i++) {
					GUILayout.BeginVertical ("Box");
					EditorGUILayout.LabelField ("Name in font:", t.m_fontData[i].fontName);
					GUILayout.BeginHorizontal();
					GUILayout.BeginVertical();
					t.m_fontData[i].ttfFile = EditorGUILayout.ObjectField ("Font #" + i, t.m_fontData[i].ttfFile, typeof(TextAsset), false) as TextAsset;
					if (!hasChanged && GUI.changed) {
						if (t.m_fontData[i].ttfFile != null) {
							t.m_fontData[i].fontName = TTFFontInfo.GetFontName (t.m_fontData[i].ttfFile.bytes);
						}
						hasChanged = true;
					}
					GUILayout.EndVertical();
					if (GUILayout.Button ("X", GUILayout.Width(22), GUILayout.Height(16))) {
						t.m_fontData.RemoveAt (i);
						EditorUtility.SetDirty (target);
					}
					GUILayout.EndHorizontal();
					GUILayout.EndVertical();
				}
			}
			GUILayout.EndVertical();
		}
#if UNITY_3_5
		EditorGUIUtility.LookLikeControls (125);
#else
		EditorGUIUtility.labelWidth = 125;
#endif
		showCharacterSettings = EditorGUILayout.Foldout (showCharacterSettings, "Character settings");
		if (showCharacterSettings) {
			GUILayout.BeginVertical ("Box");
			GUILayout.BeginHorizontal();
			t.m_defaultFont = EditorGUILayout.IntField ("Default Font", t.m_defaultFont, GUILayout.Width(148));
			if (t.m_fontData != null && t.m_fontData.Count != 0) {
				t.m_defaultFont = Mathf.Clamp (t.m_defaultFont, 0, t.m_fontData.Count-1);
				GUILayout.Label ("(" + t.m_fontData[t.m_defaultFont].fontName + ")");
			}
			else {
				t.m_defaultFont = 0;
			}
			GUILayout.EndHorizontal();
			t.m_defaultMaterial = EditorGUILayout.ObjectField ("Default Material", t.m_defaultMaterial, typeof(Material), false) as Material;
			t.m_defaultEdgeMaterial = EditorGUILayout.ObjectField ("Default Edge Material", t.m_defaultEdgeMaterial, typeof(Material), false) as Material;
			t.m_useEdgeMaterial = EditorGUILayout.Toggle ("Use Edge Material", t.m_useEdgeMaterial);
			t.m_defaultColor = EditorGUILayout.ColorField ("Default Color", t.m_defaultColor);
			t.m_computeTangents = EditorGUILayout.Toggle ("Compute Tangents", t.m_computeTangents);
			t.m_texturePerLetter = EditorGUILayout.Toggle ("Texture Per Letter", t.m_texturePerLetter);
			t.m_includeBackface = EditorGUILayout.Toggle ("Include Backface", t.m_includeBackface);
			t.m_defaultResolution = EditorGUILayout.IntField ("Default Resolution", t.m_defaultResolution);
			if (t.m_defaultResolution < 1) {
				t.m_defaultResolution = 1;
			}
			t.m_defaultSize = EditorGUILayout.FloatField ("Default Size", t.m_defaultSize);
			if (t.m_defaultSize < .001f) {
				t.m_defaultSize = .001f;
			}
			t.m_defaultDepth = EditorGUILayout.FloatField ("Default Depth", t.m_defaultDepth);
			if (t.m_defaultDepth < 0.0f) {
				t.m_defaultDepth = 0.0f;
			}
			t.m_smoothingAngle = EditorGUILayout.Slider ("Smoothing Angle", t.m_smoothingAngle, 0.0f, 180.0f);
			GUILayout.EndVertical();
		}
		showTextSettings = EditorGUILayout.Foldout (showTextSettings, "Text settings");
		if (showTextSettings) {
			GUILayout.BeginVertical ("Box");
			t.m_defaultLetterSpacing = EditorGUILayout.FloatField ("Default Letter Spacing", t.m_defaultLetterSpacing);
			t.m_defaultLineSpacing = EditorGUILayout.FloatField ("Default Line Spacing", t.m_defaultLineSpacing);
			t.m_defaultLineWidth = EditorGUILayout.FloatField ("Default Line Width", t.m_defaultLineWidth);
			t.m_wordWrap = EditorGUILayout.Toggle ("Word Wrap", t.m_wordWrap);
			t.m_tabStop = EditorGUILayout.FloatField ("Tab Stop", t.m_tabStop);
			if (t.m_tabStop < 0.0f) {
				t.m_tabStop = 0.0f;
			}
			t.m_defaultJustification = (Justify)EditorGUILayout.EnumPopup ("Default Justification", t.m_defaultJustification);
			t.m_verticalLayout = EditorGUILayout.Toggle ("Vertical Layout", t.m_verticalLayout);
			GUILayout.EndVertical();
		}
		
		showGOsettings = EditorGUILayout.Foldout (showGOsettings, "GameObject settings");
		if (showGOsettings) {
			GUILayout.BeginVertical ("Box");
			t.m_anchor = (TextAnchor)EditorGUILayout.EnumPopup ("Text Anchor", t.m_anchor);
			t.m_zAnchor = (ZAnchor)EditorGUILayout.EnumPopup ("Z Anchor", t.m_zAnchor);
			t.m_colliderType = (ColliderType)EditorGUILayout.EnumPopup ("Collider Type", t.m_colliderType);
			t.m_addRigidbodies = EditorGUILayout.Toggle ("Add rigidbodies", t.m_addRigidbodies);
			t.m_physicsMaterial = EditorGUILayout.ObjectField ("Physics Material", t.m_physicsMaterial, typeof(PhysicMaterial), false) as PhysicMaterial;
			GUILayout.EndVertical();
		}
		
		if (GUI.changed) {
			EditorUtility.SetDirty (target);
			initialized = false;
		}
		
		GUILayout.Space (8);
		GUILayout.Box ("", GUILayout.ExpandWidth (true), GUILayout.Height (2));
		GUILayout.Space (3);
		
#if UNITY_3_5
		EditorGUIUtility.LookLikeControls (50);
#else
		EditorGUIUtility.labelWidth = 50;
#endif
		GUILayout.Label ("Create FlyingText3D object utility", EditorStyles.boldLabel);
		text = EditorGUILayout.TextField ("Text: ", text);
		position = EditorGUILayout.Vector3Field ("Location: ", position);
		if (text == "" || !(t.m_fontData != null && t.m_fontData.Count != 0)) {
			GUI.enabled = false;
		}
		if (GUILayout.Button ("Create")) {
			if (!initialized) {
				FlyingText.instance.Initialize();
				initialized = true;
			}
#if UNITY_3_5 || UNITY_4_0 || UNITY_4_1 || UNITY_4_2
			Undo.RegisterSceneUndo ("Create 3D Text Object");
			var textObject = FlyingText.GetObject (text);
#else
			var textObject = FlyingText.GetObject (text);
			Undo.RegisterCreatedObjectUndo (textObject, "Create 3D Text Object");
#endif
			textObject.transform.position = position;
			var mesh = textObject.GetComponent<MeshFilter>().sharedMesh;
			if (!System.IO.Directory.Exists (Application.dataPath + "/3DTextMeshes")) {
				AssetDatabase.CreateFolder ("Assets", "3DTextMeshes");
			}
			AssetDatabase.CreateAsset (mesh, AssetDatabase.GenerateUniqueAssetPath ("Assets/3DTextMeshes/" + mesh.name + ".asset"));
		}
		
		GUI.enabled = true;		
		GUILayout.Space (8);
		GUILayout.Box ("", GUILayout.ExpandWidth (true), GUILayout.Height (2));
		GUILayout.Space (3);
		
#if UNITY_3_5
		EditorGUIUtility.LookLikeControls (50);
#else
		EditorGUIUtility.labelWidth = 50;
#endif
		
		GUILayout.Label ("Convert .ttf to .bytes utility", EditorStyles.boldLabel);
		ttfFile = EditorGUILayout.ObjectField ("TTF File:", ttfFile, typeof(Font), false) as Font;
		if (ttfFile == null) {
			GUI.enabled = false;
		}
		if (GUILayout.Button ("Convert")) {
			if (!System.IO.Directory.Exists (Application.dataPath + "/ConvertedFonts")) {
				AssetDatabase.CreateFolder ("Assets", "ConvertedFonts");
			}
			var file = AssetDatabase.GetAssetPath (ttfFile);
			if (file.ToLower().EndsWith (".ttf")) {
				var bytes = System.IO.File.ReadAllBytes (file);
				var idx = file.LastIndexOf ("/");
				file = file.Substring (idx+1, file.Length-idx-1);
				var name = Application.dataPath + "/ConvertedFonts/" + file.Substring (0, file.Length-4) + ".bytes";
				System.IO.File.WriteAllBytes (name, bytes);
				AssetDatabase.Refresh();
			}
		}
	}
}
using System.IO;
using UnityEngine;
using UnityEditor;
using TriLib;

namespace TriLibEditor
{
    public static class TriLibAssetImporter
    {
        public static void Import(string assetPath)
        {
            var assimpLoaderOptions = AssetLoaderOptions.CreateInstance();
            var assetImporter = AssetImporter.GetAtPath(assetPath);
            var userData = assetImporter.userData;
            if (!string.IsNullOrEmpty(userData))
            {
                assimpLoaderOptions.Deserialize(userData);
            }
            var folderPath = Path.GetDirectoryName(assetPath);
            var filename = Path.GetFileName(assetPath);
            var filePath = folderPath + "/" + filename;
            var prefabPath = filePath + ".prefab";
            var assimpLoader = new AssetLoader();
            assimpLoader.OnMeshCreated += (meshIndex, mesh) => ReplaceOldAsset(mesh, prefabPath);
            assimpLoader.OnMaterialCreated += delegate(uint materialIndex, bool isOverriden, Material material)
            {
                if (!isOverriden)
                {
                    ReplaceOldAsset(material, prefabPath);
                }
            };
            assimpLoader.OnTextureLoaded +=
                (sourcePath, material, propertyName, texture) => ReplaceOldAsset(texture, prefabPath);

            assimpLoader.OnAnimationClipCreated +=
                (animationClipIndex, animationClip) => ReplaceOldAsset(animationClip, prefabPath);
            assimpLoader.OnObjectLoaded += delegate(GameObject loadedGameObject)
            {
                #if UNITY_2018_3_OR_NEWER
                var existingPrefab = PrefabUtility.SaveAsPrefabAsset(loadedGameObject, prefabPath);
                #else
                var existingPrefab = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject));
                if (existingPrefab == null)
                {
                    existingPrefab = PrefabUtility.CreatePrefab(prefabPath, loadedGameObject);
                }
                else
                {
                    existingPrefab = PrefabUtility.ReplacePrefab(loadedGameObject, existingPrefab,
                        ReplacePrefabOptions.ReplaceNameBased);
                }
                #endif
                
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                AssetDatabase.ImportAsset(prefabPath);
                Object.DestroyImmediate(loadedGameObject);
                var activeEditor = TriLibAssetEditor.Active;
                if (activeEditor != null && activeEditor.AssetPath == assetPath)
                {
                    activeEditor.OnPrefabCreated((GameObject)existingPrefab);
                }
            };
            assimpLoader.LoadFromFile(assetPath, assimpLoaderOptions);
        }

        private static void ReplaceOldAsset(Object asset, string prefabPath)
        {
            var subAssets = AssetDatabase.LoadAllAssetsAtPath(prefabPath);
            foreach (var subAsset in subAssets)
            {
                if (subAsset.name == asset.name && asset.GetType() == subAsset.GetType())
                {
                    Object.DestroyImmediate(subAsset, true);
                }
            }
            AssetDatabase.AddObjectToAsset(asset, prefabPath);
        }
    }
}
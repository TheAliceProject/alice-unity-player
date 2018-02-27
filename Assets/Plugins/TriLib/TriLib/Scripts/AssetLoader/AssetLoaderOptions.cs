using UnityEngine;
using System;
using System.Collections.Generic;

namespace TriLib
{   
    /// <summary>
    /// Represents a series of asset loading options.
    /// </summary>
    [Serializable]
    public class AssetLoaderOptions : ScriptableObject
    { 
        /// <summary>
        /// Returns a new AssetLoaderOptions instance.
        /// </summary>
        /// <returns>The instance.</returns>
        public static AssetLoaderOptions CreateInstance()
        {
            return CreateInstance<AssetLoaderOptions>();
        }

        /// <summary>                              
        /// Turn on this field to disable animations loading.
        /// </summary>                              
        public bool DontLoadAnimations;

        /// <summary>                              
        /// Turn on this field to disable lights loading.
        /// </summary>                              
        public bool DontLoadLights = true;

        /// <summary>                              
        /// Turn on this field to disable cameras loading.
        /// </summary>                              
        public bool DontLoadCameras = true;

        /// <summary>
        /// Turn on this field to automatically play the first loaded animation.
        /// @note Only for legacy animations.
        /// </summary>
        public bool AutoPlayAnimations;

        /// <summary>
        /// Use this field to change default animations wrap mode.
        /// </summary>
        public WrapMode AnimationWrapMode = WrapMode.Loop;    

        /// <summary>
        /// Turn on this field to use legacy <see cref="UnityEngine.Animation"/> component.
        /// </summary>
        public bool UseLegacyAnimations = true;

        /// <summary>
        /// If you don´t wish to use legacy animations, use this field to specify a <see cref=" UnityEngine.RuntimeAnimatorController"/>.
        /// </summary>
        public RuntimeAnimatorController AnimatorController;

        /// <summary>
        /// If you don´t wish to use legacy animations, use this field to specify a <see cref=" UnityEngine.Avatar"/>.
        /// </summary>
        public Avatar Avatar;

#if UNITY_2017_3_OR_NEWER
		/// <summary>
		/// Enable this field to use 32 bits mesh vertex index format.
		/// </summary>
		public bool Use32BitsIndexFormat = true;
#endif

        /// <summary>
        /// Turn on this field to disable materials loading.
        /// </summary>                             
        public bool DontLoadMaterials;

		/// <summary>
		/// Turn on this field to automatically scan and apply alpha textured materials.
		/// </summary>
		public bool ApplyAlphaMaterials = true;

		/// <summary>
		/// Turn on this field to use cutout materials instead of alpha-blended materials.
		/// </summary>
		public bool UseCutoutMaterials = true;

        /// <summary>
        /// Turn on this field to use the Unity default specular material.
        /// </summary>
        public bool UseStandardSpecularMaterial;

        /// <summary>
        /// Turn on this field to enable mesh collider generation 
        /// @note Only for non-skinned mesh renderers.
        /// </summary>
        public bool GenerateMeshColliders;

        /// <summary>
        /// Turn on this field to indicate that generated mesh collider will be convex.
        /// </summary>
        public bool ConvexMeshColliders;

        /// <summary>
        /// Use this field to override materials with your own.
        /// If this array is not empty, each mesh material will be replaced by the material with the same index from this array.
        /// </summary>
        public List<Material> MaterialsOverride = new List<Material>();
        /// <summary>
        /// Use this field to override object rotation angles.
        /// </summary>
        public Vector3 RotationAngles = new Vector3(0f, 180f,0f);

        /// <summary>
        /// Use this field to override object scale.
        /// </summary>
        public float Scale = 1f;

        /// <summary>
        /// Use this field to set-up advanced object loading options. <see cref="AssimpPostProcessSteps"/>
        /// </summary>
        public AssimpPostProcessSteps PostProcessSteps = AssimpPostProcessSteps.FlipWindingOrder | AssimpPostProcessSteps.MakeLeftHanded | AssimpProcessPreset.TargetRealtimeMaxQuality;

        /// <summary>
        /// Use this field to override the object textures searching path.
        /// </summary>
        public string TexturesPathOverride;

        /// <summary>
        /// Use this field to set texture compression level.
        /// </summary>
        public TextureCompression TextureCompression = TextureCompression.NormalQuality;

        /// <summary>
        /// Use this field to define asset loading advanced configs.
        /// </summary>
        public List<AssetAdvancedConfig> AdvancedConfigs = new List<AssetAdvancedConfig>
        {
            new AssetAdvancedConfig(AssetAdvancedPropertyMetadata.GetConfigKey(AssetAdvancedPropertyClassNames.SplitLargeMeshesVertexLimit), 65000),
            new AssetAdvancedConfig(AssetAdvancedPropertyMetadata.GetConfigKey(AssetAdvancedPropertyClassNames.FBXImportReadLights), false),
            new AssetAdvancedConfig(AssetAdvancedPropertyMetadata.GetConfigKey(AssetAdvancedPropertyClassNames.FBXImportReadCameras), false)
        };

        /// <summary>
        /// Deserialize the specified JSON representation into this class.
        /// </summary>
        /// <param name="json">Json.</param>
        public void Deserialize(string json)
        {
            //AdvancedPropertiesDeserialization();
            JsonUtility.FromJsonOverwrite(json, this);
        }

        /// <summary>
        /// Serializes this instance to a JSON representation.
        /// </summary>
        public string Serialize()
        {
            //AdvancedPropertiesSerialization();
            return JsonUtility.ToJson(this);
        }
    }  
}
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TriLib
{
    /// <summary>
    /// Represents a <see cref="AssetLoader"/> object loaded event handler.
    /// </summary>
    public delegate void ObjectLoadedHandle(GameObject loadedGameObject);

    /// <summary>
    /// Represents a <see cref="AssetLoader"/> mesh creation event handler.
    /// </summary>
    public delegate void MeshCreatedHandle(uint meshIndex, Mesh mesh);

    /// <summary>
    /// Represents a <see cref="AssetLoader"/> material created event handler.
    /// </summary>
    public delegate void MaterialCreatedHandle(uint materialIndex, bool isOverriden, Material material);

    /// <summary>
    /// Represents a <see cref="AssetLoader"/> animation created event handler.
    /// </summary>
    public delegate void AnimationClipCreatedHandle(uint animationClipIndex, AnimationClip animationClip);

    /// <summary>
    /// Represents an asset loader. The main class used to load assets.
    /// </summary>
    public class AssetLoader : IDisposable
    {
        /// <summary>
        /// <see cref="MaterialData"/> relationship list.
        /// </summary>
        private MaterialData[] _materialData;

        /// <summary>
        /// <see cref="MeshData"/> relationship list.
        /// </summary>
        private MeshData[] _meshData;

        /// <summary>
        /// <see cref="NodeData"/> relationship list.
        /// </summary>
        private Dictionary<string, NodeData> _nodeDataDictionary;

        /// <summary>
        /// Temporary identifier assigned to nodes.
        /// </summary>
        private int _nodeId;

        /// <summary>
        /// Base Diffuse <see cref="UnityEngine.Material"/> used to load materials.
        /// </summary>
        private Material _standardBaseMaterial;

        /// <summary>
        /// Base Specular <see cref="UnityEngine.Material"/> used to load materials.
        /// </summary>
        private Material _standardSpecularMaterial;

		/// <summary>
		/// Base Diffuse Alpha <see cref="UnityEngine.Material"/> used to load materials.
		/// </summary>
		private Material _standardBaseAlphaMaterial;

		/// <summary>
		/// Base Specular Alpha <see cref="UnityEngine.Material"/> used to load materials.
		/// </summary>
		private Material _standardSpecularAlphaMaterial;

		/// <summary>
		/// Base Diffuse Cutout <see cref="UnityEngine.Material"/> used to load materials.
		/// </summary>
		private Material _standardBaseCutoutMaterial;

		/// <summary>
		/// Base Specular Cutout <see cref="UnityEngine.Material"/> used to load materials.
		/// </summary>
		private Material _standardSpecularCutoutMaterial;

        /// <summary>
        /// <see cref="UnityEngine.Texture"/> to show when no texture is found.
        /// </summary>
        private Texture2D _notFoundTexture;

        /// <summary>
        /// Use this field to assign the event that occurs when a mesh is loaded.
        /// </summary>
        public event MeshCreatedHandle OnMeshCreated;

        /// <summary>
        /// Use this field to assign the event that occurs when a material is created.
        /// </summary>
        public event MaterialCreatedHandle OnMaterialCreated;

#pragma warning disable 612, 618
        /// <summary>
        /// Use this field to assign the event that occurs when a texture is loaded.
        /// </summary>
        public event TextureLoadHandle OnTextureLoaded;
#pragma warning restore 612, 618

        /// <summary>
        /// Use this field to assign the event that occurs when an animation is created.
        /// </summary>
        public event AnimationClipCreatedHandle OnAnimationClipCreated;

        /// <summary>
        /// Use this field to assign the event that occurs when the object is loaded.
        /// </summary>
        public event ObjectLoadedHandle OnObjectLoaded;

        /// <summary>
        /// Checks if the file extension is supported.
        /// </summary>
        /// <returns>True, if the extension is supported. Otherwise, False.</returns>
        public static bool IsExtensionSupported(string extension)
        {
            return AssimpInterop.ai_IsExtensionSupported(extension);
        }

        /// <summary>
        /// Returns a list of supported file extensions.
        /// </summary>
        /// <returns>Supported file extensions.</returns>
        public static string GetSupportedFileExtensions()
        {
            string supportedFileExtensions;
            AssimpInterop.ai_GetExtensionList(out supportedFileExtensions);
            return supportedFileExtensions;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            OnMeshCreated = null;
            OnMaterialCreated = null;
            OnAnimationClipCreated = null;
            OnTextureLoaded = null;
            OnObjectLoaded = null;
            if (_nodeDataDictionary != null)
            {
                foreach (var item in _nodeDataDictionary.Values)
                {
                    item.Dispose();
                }
                _nodeDataDictionary = null;
            }
            if (_meshData != null)
            {
                foreach (var item in _meshData)
                {
                    item.Dispose();
                }
                _meshData = null;
            }
            if (_materialData != null)
            {
                foreach (var item in _materialData)
                {
                    item.Dispose();
                }
                _materialData = null;
            }
        }

        /// <summary>
        /// Loads the base <see cref="UnityEngine.Material"/>, if needed.
        /// @warning To ensure your materials will be loaded, don´t remove the material files included in the package.
        /// </summary>
        private bool LoadStandardMaterials()
        {
            if (_standardBaseMaterial == null)
            {
                _standardBaseMaterial = Resources.Load("StandardMaterial") as Material;
            }
            if (_standardSpecularMaterial == null)
            {
                _standardSpecularMaterial = Resources.Load("StandardSpecularMaterial") as Material;
            }
			if (_standardBaseAlphaMaterial == null)
			{
				_standardBaseAlphaMaterial = Resources.Load("StandardBaseAlphaMaterial") as Material;
			}
			if (_standardSpecularAlphaMaterial == null) {
				_standardSpecularAlphaMaterial = Resources.Load ("StandardSpecularAlphaMaterial") as Material;
			}
			if (_standardBaseCutoutMaterial == null)
			{
				_standardBaseCutoutMaterial = Resources.Load("StandardBaseCutoutMaterial") as Material;
			}
			if (_standardSpecularCutoutMaterial == null) {
				_standardSpecularCutoutMaterial = Resources.Load ("StandardSpecularCutoutMaterial") as Material;
			}
			return _standardBaseMaterial != null && _standardSpecularMaterial != null && _standardBaseAlphaMaterial != null && _standardSpecularAlphaMaterial != null && _standardBaseCutoutMaterial != null && _standardSpecularCutoutMaterial != null;
        }

        /// <summary>
        /// Loads the <see cref="UnityEngine.Texture"/> to show unknown textures, if needed.
        /// @warning Don´t remove the __NotFound.asset_ included in the package.
        /// </summary>
        private bool LoadNotFoundTexture()
        {
            if (_notFoundTexture == null)
            {
                _notFoundTexture = Resources.Load("NotFound") as Texture2D;
            }
            return _notFoundTexture != null;
        }

        /// <summary>
        /// Applies transformation from loading options into loaded object.
        /// </summary>
        /// <param name="rootGameObject"><see cref="UnityEngine.GameObject" /> to transform.</param>
        /// <param name="options"><see cref="AssetLoaderOptions"/> for loading the transformations.</param>
        private void LoadContextOptions(GameObject rootGameObject, AssetLoaderOptions options)
        {
            rootGameObject.transform.rotation = Quaternion.Euler(options.RotationAngles);
            rootGameObject.transform.localScale = Vector3.one * options.Scale;
        }

        /// <summary>
        /// Loads a <see cref="UnityEngine.GameObject"/> from input byte array with defined options.
        /// @warning To ensure your materials will be loaded, don´t remove material files included in the package.
        /// </summary>
        /// <param name="fileBytes">File bytes used to load the <see cref="UnityEngine.GameObject"/>.</param>
        /// <param name="filename">Original filename.</param>
        /// <param name="options"><see cref="AssetLoaderOptions"/> used to load the object.</param>
        /// <param name="wrapperGameObject">Use this field to load the new <see cref="UnityEngine.GameObject"/> into this <see cref="UnityEngine.GameObject"/> structure.</param> 
        /// <returns>A new <see cref="UnityEngine.GameObject"/>.</returns>
        /// <example>
        /// @code
        /// protected void Awake() {
        ///     GameObject myGameObject;
        ///     try {
        ///         using (var assetLoader = new AssetLoader()) {
        ///             //In case you don't have a valid filename, set this to the file extension
        ///             //to help TriLib assigining a file loader to this file
        ///             //example value: ".fbx"
        /// 			var filename = "c:/models/mymodel.fbx";
        /// 			var fileData = File.ReadAllBytes(filename);
        ///             gameObject = assetLoader.LoadFromMemory(fleData, filename);
        ///         }
        ///     } catch (Exception e) {
        ///         Debug.LogFormat("Unable to load mymodel.fbx. The loader returned: {0}", e);
        ///     }
        /// }
        /// @endcode
        /// </example>
        public GameObject LoadFromMemory(byte[] fileBytes, string filename, AssetLoaderOptions options = null, GameObject wrapperGameObject = null)
        {
            if (options == null)
            {
                options = AssetLoaderOptions.CreateInstance();
            }
            IntPtr scene;
            try
            {
				var extension = File.Exists(filename) ? Path.GetExtension(filename) : filename;
				scene = ImportFileFromMemory(fileBytes, extension, options);
            }
            catch (Exception exception)
            {
                throw new Exception("Error parsing file.", exception);
            }
            if (scene == IntPtr.Zero)
            {
                var error = AssimpInterop.ai_GetErrorString();
                throw new Exception(string.Format("Error loading asset. Assimp returns: [{0}]", error));
            }
            GameObject newGameObject = null;
            try
            {
                newGameObject = LoadInternal(filename, scene, options, wrapperGameObject);
            }
            catch
            {
                if (newGameObject != null)
                {
                    Object.Destroy(newGameObject);
                }
                throw;
            }
            AssimpInterop.ai_ReleaseImport(scene);
            if (OnObjectLoaded != null)
            {
                OnObjectLoaded(newGameObject);
            }
            return newGameObject;
        }

        /// <summary>
        /// Loads a <see cref="UnityEngine.GameObject"/> from input filename with defined options.
        /// @warning To ensure your materials will be loaded, don´t remove the material files included in the package.
        /// </summary>
        /// <param name="filename">Filename used to load the <see cref="UnityEngine.GameObject"/>.</param>
        /// <param name="options"><see cref="AssetLoaderOptions"/> used to load the object.</param>
        /// <param name="wrapperGameObject">Use this field to load the new <see cref="UnityEngine.GameObject"/> into this <see cref="UnityEngine.GameObject"/> structure.</param> 
        /// <returns>A new <see cref="UnityEngine.GameObject"/>.</returns>
        /// <example>
        /// @code
        /// protected void Awake() {
        ///     GameObject myGameObject;
        ///     try {
        ///         using (var assetLoader = new AssetLoader()) {
        ///             gameObject = assetLoader.LoadFromFile("mymodel.fbx");
        ///         }
        ///     } catch (Exception e) {
        ///         Debug.LogFormat("Unable to load mymodel.fbx. The loader returned: {0}", e);
        ///     }
        /// }
        /// @endcode
        /// </example>
        public GameObject LoadFromFile(string filename, AssetLoaderOptions options = null, GameObject wrapperGameObject = null)
        {
            if (options == null)
            {
                options = AssetLoaderOptions.CreateInstance();
            }
            IntPtr scene;
            try
            {
                scene = ImportFile(filename, options);
            }
            catch (Exception exception)
            {
                throw new Exception(string.Format("Error parsing file: {0}", filename), exception);
            }
            if (scene == IntPtr.Zero)
            {
                var error = AssimpInterop.ai_GetErrorString();
                throw new Exception(string.Format("Error loading asset. Assimp returns: [{0}]", error));
            }
            GameObject newGameObject = null;
            try
            {
                newGameObject = LoadInternal(filename, scene, options, wrapperGameObject);
            }
            catch
            {
                if (newGameObject != null)
                {
                    Object.Destroy(newGameObject);
                }
                throw;
            }
            AssimpInterop.ai_ReleaseImport(scene);
            if (OnObjectLoaded != null)
            {
                OnObjectLoaded(newGameObject);
            }
            return newGameObject;
        }

        /// <summary>
        /// Builds a property store.
        /// </summary>
        /// <param name="options">Input options.</param>
        /// <returns></returns>
        private static IntPtr BuildPropertyStore(AssetLoaderOptions options)
        {
            var propertyStore = AssimpInterop.ai_CreatePropertyStore();
            foreach (var advancedConfig in options.AdvancedConfigs)
            {
                AssetAdvancedConfigType assetAdvancedConfigType;
                string className;
                string description;
                string group;
                bool hasDefaultValue;
                bool hasMinValue;
                bool hasMaxValue;
                object defaultValue;
                object minValue;
                object maxValue;
                AssetAdvancedPropertyMetadata.GetOptionMetadata(advancedConfig.Key, out assetAdvancedConfigType, out className, out description, out group, out defaultValue, out minValue, out maxValue, out hasDefaultValue, out hasMinValue, out hasMaxValue);
                switch (assetAdvancedConfigType)
                {
                    case AssetAdvancedConfigType.AiComponent:
                        AssimpInterop.ai_SetImportPropertyInteger(propertyStore, advancedConfig.Key, advancedConfig.IntValue << 1);
                        break;
                    case AssetAdvancedConfigType.AiPrimitiveType:
                        AssimpInterop.ai_SetImportPropertyInteger(propertyStore, advancedConfig.Key, advancedConfig.IntValue << 1);
                        break;
                    case AssetAdvancedConfigType.AiUVTransform:
                        AssimpInterop.ai_SetImportPropertyInteger(propertyStore, advancedConfig.Key, advancedConfig.IntValue << 1);
                        break;
                    case AssetAdvancedConfigType.Bool:
                        AssimpInterop.ai_SetImportPropertyInteger(propertyStore, advancedConfig.Key, advancedConfig.BoolValue ? 1 : 0);
                        break;
                    case AssetAdvancedConfigType.Integer:
                        AssimpInterop.ai_SetImportPropertyInteger(propertyStore, advancedConfig.Key, advancedConfig.IntValue);
                        break;
                    case AssetAdvancedConfigType.Float:
                        AssimpInterop.ai_SetImportPropertyFloat(propertyStore, advancedConfig.Key, advancedConfig.FloatValue);
                        break;
                    case AssetAdvancedConfigType.String:
                        AssimpInterop.ai_SetImportPropertyString(propertyStore, advancedConfig.Key, advancedConfig.StringValue);
                        break;
					case AssetAdvancedConfigType.AiMatrix:
						AssimpInterop.ai_SetImportPropertyMatrix(propertyStore, advancedConfig.Key, advancedConfig.TranslationValue, advancedConfig.RotationValue, advancedConfig.ScaleValue);
						break;
                }
            }
            return propertyStore;
        }

        /// <summary>
        /// Imports the file based on given options.
        /// </summary>
        /// <returns>The loaded scene.</returns>
        /// <param name="fileBytes">File bytes used to load the <see cref="UnityEngine.GameObject"/>.</param>
        /// <param name="fileHint">File format hint. Eg: ".fbx".</param>
        /// <param name="options"><see cref="AssetLoaderOptions"/> used to load the object.</param>
        private static IntPtr ImportFileFromMemory(byte[] fileBytes, string fileHint, AssetLoaderOptions options)
        {
            IntPtr scene;
            if (options != null && options.AdvancedConfigs != null)
            {
                var propertyStore = BuildPropertyStore(options);
                scene = AssimpInterop.ai_ImportFileFromMemoryWithProperties(fileBytes, (uint)options.PostProcessSteps, fileHint, propertyStore);
                AssimpInterop.ai_CreateReleasePropertyStore(propertyStore);
            }
            else
            {
                scene = AssimpInterop.ai_ImportFileFromMemory(fileBytes, options == null ? 0 : (uint)options.PostProcessSteps, fileHint);
            }
            return scene;
        }

        /// <summary>
        /// Imports the file based on given options.
        /// </summary>
        /// <returns>The loaded scene.</returns>
        /// <param name="filename">Filename used to load the <see cref="UnityEngine.GameObject"/>.</param>
        /// <param name="options"><see cref="AssetLoaderOptions"/> used to load the object.</param>
        private static IntPtr ImportFile(string filename, AssetLoaderOptions options)
        {
            IntPtr scene;
            if (options != null && options.AdvancedConfigs != null)
            {
                var propertyStore = BuildPropertyStore(options);
                scene = AssimpInterop.ai_ImportFileEx(filename, (uint)options.PostProcessSteps, IntPtr.Zero, propertyStore);
                AssimpInterop.ai_CreateReleasePropertyStore(propertyStore);
            }
            else
            {
              scene = AssimpInterop.ai_ImportFile(filename, options == null ? 0 : (uint)options.PostProcessSteps);
            }
            return scene;
        }

        /// <summary>
        /// Processes the importing and returns a <see cref="UnityEngine.GameObject"/>.
        /// </summary>
        /// <param name="filename">Filename used to load the <see cref="UnityEngine.GameObject"/>.</param>
        /// <param name="scene">Previously loaded scene pointer.</param>
        /// <param name="options"><see cref="AssetLoaderOptions"/> used to load the object.</param>
        /// <param name="wrapperGameObject">Use this field to load the new <see cref="UnityEngine.GameObject"/> into this <see cref="UnityEngine.GameObject"/> structure.</param> 
        /// <returns>A new <see cref="UnityEngine.GameObject"/>.</returns>
        private GameObject LoadInternal(string filename, IntPtr scene, AssetLoaderOptions options, GameObject wrapperGameObject = null)
        {
            _nodeDataDictionary = new Dictionary<string, NodeData>();
            _nodeId = 0;
            if (!LoadNotFoundTexture())
            {
#if UNITY_EDITOR
                BuildNotFoundTexture();
#else
                throw new Exception("Please add the \"NotFound\" asset from the source package at the project 'Resources' folder.");
#endif
            }
            if (!LoadStandardMaterials())
            {
#if UNITY_EDITOR
                BuildDefaultMaterials();
#else
                throw new Exception("Please add the \"StandardMaterial\" and \"StandardSpecularMaterial\" assets from the source package at the project 'Resources' folder.");
#endif
            }
            if (AssimpInterop.aiScene_HasMaterials(scene) && !options.DontLoadMaterials)
            {
                _materialData = new MaterialData[AssimpInterop.aiScene_GetNumMaterials(scene)];
                BuildMaterials(filename, scene, options);
            }
            if (AssimpInterop.aiScene_HasMeshes(scene))
            {
                _meshData = new MeshData[AssimpInterop.aiScene_GetNumMeshes(scene)];
                BuildMeshes(scene, options);
            }
            wrapperGameObject = BuildWrapperObject(scene, options, wrapperGameObject);
            if (AssimpInterop.aiScene_HasMeshes(scene))
            {
                BuildBones(scene);
            }
            if (AssimpInterop.aiScene_HasAnimation(scene) && !options.DontLoadAnimations)
            {
                BuildAnimations(wrapperGameObject, scene, options);
            }
            if (AssimpInterop.aiScene_HasCameras(scene) && !options.DontLoadCameras)
            {
                BuildCameras(wrapperGameObject, scene, options);
            }
            if (AssimpInterop.aiScene_HasLights(scene) && !options.DontLoadLights)
            {
                BuildLights(wrapperGameObject, scene, options);
            }
            _nodeDataDictionary = null;
            _meshData = null;
            _materialData = null;
            return wrapperGameObject;
        }

#if UNITY_EDITOR
        /// <summary>
        /// Builds the not found texture.
        /// </summary>
        private void BuildNotFoundTexture()
        {
            const string notFoundTextureData = "iVBORw0KGgoAAAANSUhEUgAAAAgAAAAICAIAAABLbSncAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAtSURBVBhXY/gPBvYeZ+AIIgKSQObD2Xgl4ABZBUICWRQIUHQgAyJ0oEj8/w8AyzKd+YE5HWsAAAAASUVORK5CYII=";
            _notFoundTexture = new Texture2D(2, 2);
            _notFoundTexture.LoadImage(Convert.FromBase64String(notFoundTextureData));
            AssetDatabase.CreateAsset(_notFoundTexture, "Assets/TriLib/TriLib/Resources/NotFound.asset");
        }

        /// <summary>
        /// Builds the default materials.
        /// </summary>
        private void BuildDefaultMaterials()
        {
			//Standard Diffuse & Specular
            _standardBaseMaterial = new Material(Shader.Find("Standard"));
            _standardBaseMaterial.EnableKeyword("_EMISSION");
            _standardBaseMaterial.EnableKeyword("_SPECGLOSSMAP");
            _standardBaseMaterial.EnableKeyword("_NORMALMAP");
            _standardBaseMaterial.SetTexture("_MainTex", _notFoundTexture);
            _standardBaseMaterial.SetTexture("_EmissionMap", _notFoundTexture);
			_standardBaseMaterial.SetTexture("_BumpMap", _notFoundTexture);
			_standardBaseMaterial.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
            AssetDatabase.CreateAsset(_standardBaseMaterial, "Assets/TriLib/TriLib/Resources/StandardMaterial.mat");

            _standardSpecularMaterial = new Material(Shader.Find("Standard (Specular setup)"));
            _standardSpecularMaterial.EnableKeyword("_EMISSION");
            _standardSpecularMaterial.EnableKeyword("_SPECGLOSSMAP");
            _standardSpecularMaterial.EnableKeyword("_NORMALMAP");
            _standardSpecularMaterial.SetTexture("_MainTex", _notFoundTexture);
            _standardSpecularMaterial.SetTexture("_EmissionMap", _notFoundTexture);
            _standardSpecularMaterial.SetTexture("_SpecGlossMap", _notFoundTexture);
			_standardSpecularMaterial.SetTexture("_BumpMap", _notFoundTexture);
			_standardSpecularMaterial.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
            AssetDatabase.CreateAsset(_standardSpecularMaterial, "Assets/TriLib/TriLib/Resources/StandardSpecularMaterial.mat");

			//Alpha Diffuse & Specular
			_standardBaseAlphaMaterial = new Material(Shader.Find("Standard"));
			_standardBaseAlphaMaterial.SetFloat("_Mode", 3f);
			_standardBaseAlphaMaterial.SetOverrideTag("RenderType", "Transparent");
			_standardBaseAlphaMaterial.SetInt("_SrcBlend", (int)BlendMode.One);
			_standardBaseAlphaMaterial.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
			_standardBaseAlphaMaterial.SetInt("_ZWrite", 0);
			_standardBaseAlphaMaterial.DisableKeyword("_ALPHATEST_ON");
			_standardBaseAlphaMaterial.DisableKeyword("_ALPHABLEND_ON");
			_standardBaseAlphaMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
			_standardBaseAlphaMaterial.renderQueue = (int)RenderQueue.Transparent;
			_standardBaseAlphaMaterial.EnableKeyword("_EMISSION");
			_standardBaseAlphaMaterial.EnableKeyword("_SPECGLOSSMAP");
			_standardBaseAlphaMaterial.EnableKeyword("_NORMALMAP");
			_standardBaseAlphaMaterial.SetTexture("_MainTex", _notFoundTexture);
			_standardBaseAlphaMaterial.SetTexture("_EmissionMap", _notFoundTexture);
			_standardBaseAlphaMaterial.SetTexture("_BumpMap", _notFoundTexture);
			_standardBaseAlphaMaterial.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
			AssetDatabase.CreateAsset(_standardBaseAlphaMaterial, "Assets/TriLib/TriLib/Resources/StandardBaseAlphaMaterial.mat");

			_standardSpecularAlphaMaterial = new Material(Shader.Find("Standard (Specular setup)"));
			_standardSpecularAlphaMaterial.SetFloat("_Mode", 3f);
			_standardSpecularAlphaMaterial.SetOverrideTag("RenderType", "Transparent");
			_standardSpecularAlphaMaterial.SetInt("_SrcBlend", (int)BlendMode.One);
			_standardSpecularAlphaMaterial.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
			_standardSpecularAlphaMaterial.SetInt("_ZWrite", 0);
			_standardSpecularAlphaMaterial.DisableKeyword("_ALPHATEST_ON");
			_standardSpecularAlphaMaterial.DisableKeyword("_ALPHABLEND_ON");
			_standardSpecularAlphaMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
			_standardSpecularAlphaMaterial.renderQueue = (int)RenderQueue.Transparent;
			_standardSpecularAlphaMaterial.EnableKeyword("_EMISSION");
			_standardSpecularAlphaMaterial.EnableKeyword("_SPECGLOSSMAP");
			_standardSpecularAlphaMaterial.EnableKeyword("_NORMALMAP");
			_standardSpecularAlphaMaterial.SetTexture("_MainTex", _notFoundTexture);
			_standardSpecularAlphaMaterial.SetTexture("_EmissionMap", _notFoundTexture);
			_standardSpecularAlphaMaterial.SetTexture("_SpecGlossMap", _notFoundTexture);
			_standardSpecularAlphaMaterial.SetTexture("_BumpMap", _notFoundTexture);
			_standardSpecularAlphaMaterial.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
			AssetDatabase.CreateAsset(_standardSpecularAlphaMaterial, "Assets/TriLib/TriLib/Resources/StandardSpecularAlphaMaterial.mat");

			//Cutout Diffuse & Specular
			_standardBaseCutoutMaterial = new Material(Shader.Find("Standard"));
			_standardBaseCutoutMaterial.SetFloat("_Mode", 1f);
			_standardBaseCutoutMaterial.SetOverrideTag("RenderType", "TransparentCutout");
			_standardBaseCutoutMaterial.SetInt("_SrcBlend", (int)BlendMode.One);
			_standardBaseCutoutMaterial.SetInt("_DstBlend", (int)BlendMode.Zero);
			_standardBaseCutoutMaterial.SetInt("_ZWrite", 1);
			_standardBaseCutoutMaterial.EnableKeyword("_ALPHATEST_ON");
			_standardBaseCutoutMaterial.DisableKeyword("_ALPHABLEND_ON");
			_standardBaseCutoutMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			_standardBaseCutoutMaterial.renderQueue = (int)RenderQueue.AlphaTest;
			_standardBaseCutoutMaterial.EnableKeyword("_EMISSION");
			_standardBaseCutoutMaterial.EnableKeyword("_SPECGLOSSMAP");
			_standardBaseCutoutMaterial.EnableKeyword("_NORMALMAP");
			_standardBaseCutoutMaterial.SetTexture("_MainTex", _notFoundTexture);
			_standardBaseCutoutMaterial.SetTexture("_EmissionMap", _notFoundTexture);
			_standardBaseCutoutMaterial.SetTexture("_BumpMap", _notFoundTexture);
			_standardBaseCutoutMaterial.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
			AssetDatabase.CreateAsset(_standardBaseCutoutMaterial, "Assets/TriLib/TriLib/Resources/StandardBaseCutoutMaterial.mat");

			_standardSpecularCutoutMaterial = new Material(Shader.Find("Standard (Specular setup)"));
			_standardSpecularCutoutMaterial.SetFloat("_Mode", 1f);
			_standardSpecularCutoutMaterial.SetOverrideTag("RenderType", "TransparentCutout");
			_standardSpecularCutoutMaterial.SetInt("_SrcBlend", (int)BlendMode.One);
			_standardSpecularCutoutMaterial.SetInt("_DstBlend", (int)BlendMode.Zero);
			_standardSpecularCutoutMaterial.SetInt("_ZWrite", 1);
			_standardSpecularCutoutMaterial.EnableKeyword("_ALPHATEST_ON");
			_standardSpecularCutoutMaterial.DisableKeyword("_ALPHABLEND_ON");
			_standardSpecularCutoutMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			_standardSpecularCutoutMaterial.renderQueue = (int)RenderQueue.AlphaTest;
			_standardSpecularCutoutMaterial.EnableKeyword("_EMISSION");
			_standardSpecularCutoutMaterial.EnableKeyword("_SPECGLOSSMAP");
			_standardSpecularCutoutMaterial.EnableKeyword("_NORMALMAP");
			_standardSpecularCutoutMaterial.SetTexture("_MainTex", _notFoundTexture);
			_standardSpecularCutoutMaterial.SetTexture("_EmissionMap", _notFoundTexture);
			_standardSpecularCutoutMaterial.SetTexture("_SpecGlossMap", _notFoundTexture);
			_standardSpecularCutoutMaterial.SetTexture("_BumpMap", _notFoundTexture);
			_standardSpecularCutoutMaterial.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
			AssetDatabase.CreateAsset(_standardSpecularCutoutMaterial, "Assets/TriLib/TriLib/Resources/StandardSpecularCutoutMaterial.mat");

            AssetDatabase.SaveAssets();
        }
#endif

        /// <summary>
        /// Builds the <see cref="UnityEngine.Mesh"/> list.
        /// </summary>
		/// <param name="scene">Previously loaded scene pointer.</param>
		/// <param name="options"><see cref="AssetLoaderOptions"/> used to load the object.</param>
		private void BuildMeshes(IntPtr scene, AssetLoaderOptions options)
        {
            var meshCount = AssimpInterop.aiScene_GetNumMeshes(scene);
            for (uint m = 0; m < meshCount; m++)
            {
                var mesh = AssimpInterop.aiScene_GetMesh(scene, m);
                var vertexCount = AssimpInterop.aiMesh_VertexCount(mesh);

                var unityVertices = new Vector3[vertexCount];
                Vector3[] unityNormals = null;
                var hasNormals = AssimpInterop.aiMesh_HasNormals(mesh);
                if (hasNormals)
                {
                    unityNormals = new Vector3[vertexCount];
                }
                Vector4[] unityTangents = null;
                Vector4[] unityBiTangents = null;
                var hasTangentsAndBitangents = AssimpInterop.aiMesh_HasTangentsAndBitangents(mesh);
                if (hasTangentsAndBitangents)
                {
                    unityTangents = new Vector4[vertexCount];
                    unityBiTangents = new Vector4[vertexCount];
                }
                Vector2[] unityUv0 = null;
                var hasTextureCoords0 = AssimpInterop.aiMesh_HasTextureCoords(mesh, 0);
                if (hasTextureCoords0)
                {
                    unityUv0 = new Vector2[vertexCount];
                }
                Vector2[] unityUv1 = null;
                var hasTextureCoords1 = AssimpInterop.aiMesh_HasTextureCoords(mesh, 1);
                if (hasTextureCoords1)
                {
                    unityUv1 = new Vector2[vertexCount];
                }
                Vector2[] unityUv2 = null;
                var hasTextureCoords2 = AssimpInterop.aiMesh_HasTextureCoords(mesh, 2);
                if (hasTextureCoords2)
                {
                    unityUv2 = new Vector2[vertexCount];
                }
                Vector2[] unityUv3 = null;
                var hasTextureCoords3 = AssimpInterop.aiMesh_HasTextureCoords(mesh, 3);
                if (hasTextureCoords3)
                {
                    unityUv3 = new Vector2[vertexCount];
                }
                Color[] unityColors = null;
                var hasVertexColors = AssimpInterop.aiMesh_HasVertexColors(mesh, 0);
                if (hasVertexColors)
                {
                    unityColors = new Color[vertexCount];
                }
                for (uint v = 0; v < vertexCount; v++)
                {
                    unityVertices[v] = AssimpInterop.aiMesh_GetVertex(mesh, v);
                    if (hasNormals)
                    {
                        unityNormals[v] = AssimpInterop.aiMesh_GetNormal(mesh, v);
                    }
                    if (hasTangentsAndBitangents)
                    {
                        unityTangents[v] = AssimpInterop.aiMesh_GetTangent(mesh, v);
                        unityBiTangents[v] = AssimpInterop.aiMesh_GetBitangent(mesh, v);
                    }
                    if (hasTextureCoords0)
                    {
                        unityUv0[v] = AssimpInterop.aiMesh_GetTextureCoord(mesh, 0, v);
                    }
                    if (hasTextureCoords1)
                    {
                        unityUv1[v] = AssimpInterop.aiMesh_GetTextureCoord(mesh, 1, v);
                    }
                    if (hasTextureCoords2)
                    {
                        unityUv2[v] = AssimpInterop.aiMesh_GetTextureCoord(mesh, 2, v);
                    }
                    if (hasTextureCoords3)
                    {
                        unityUv3[v] = AssimpInterop.aiMesh_GetTextureCoord(mesh, 3, v);
                    }
                    if (hasVertexColors)
                    {
                        unityColors[v] = AssimpInterop.aiMesh_GetVertexColor(mesh, 0, v);
                    }
                }
                var meshName = AssimpInterop.aiMesh_GetName(mesh);
                var unityMesh = new Mesh
                {
                    name = string.IsNullOrEmpty(meshName) ? "Mesh_" + StringUtils.GenerateUniqueName(m) : meshName,
                    vertices = unityVertices
                };
#if UNITY_2017_3_OR_NEWER
				if (options.Use32BitsIndexFormat) {
					unityMesh.indexFormat = IndexFormat.UInt32;
				}
#endif
				if (hasNormals)
                {
                    unityMesh.normals = unityNormals;
                }
                if (hasTangentsAndBitangents)
                {
                    unityMesh.tangents = unityTangents;
                }
                if (hasTextureCoords0)
                {
                    unityMesh.uv = unityUv0;
                }
                if (hasTextureCoords1)
                {
                    unityMesh.uv2 = unityUv1;
                }
                if (hasTextureCoords2)
                {
                    unityMesh.uv3 = unityUv2;
                }
                if (hasTextureCoords3)
                {
                    unityMesh.uv4 = unityUv3;
                }
                if (hasVertexColors)
                {
                    unityMesh.colors = unityColors;
                }
                if (AssimpInterop.aiMesh_HasFaces(mesh))
                {
                    var facesCount = AssimpInterop.aiMesh_GetNumFaces(mesh);
                    var unityIndices = new int[facesCount * 3];
                    for (uint f = 0; f < facesCount; f++)
                    {
                        var face = AssimpInterop.aiMesh_GetFace(mesh, f);
                        var indexCount = AssimpInterop.aiFace_GetNumIndices(face);
                        if (indexCount > 3)
                        {
                            throw new UnityException("More than three face indices is not supported. Please enable \"Triangulate\" in your \"AssetLoaderOptions\" \"PostProcessSteps\" field");
                        }
                        for (uint i = 0; i < indexCount; i++)
                        {
                            unityIndices[f * 3 + i] = (int)AssimpInterop.aiFace_GetIndex(face, i);
                        }
                    }
                    unityMesh.SetIndices(unityIndices, MeshTopology.Triangles, 0);
                }
                var meshData = new MeshData { UnityMesh = unityMesh };
                _meshData[m] = meshData;
            }
        }

        /// <summary>
        /// Builds the <see cref="UnityEngine.Light"/> list.
        /// </summary>
        /// <param name="wrapperGameObject">Wrapper <see cref="UnityEngine.GameObject"/></param>
        /// <param name="scene">Previously loaded scene pointer.</param>
        /// <param name="options"><see cref="AssetLoaderOptions"/> used to load the object.</param>
        private static void BuildLights(GameObject wrapperGameObject, IntPtr scene, AssetLoaderOptions options)
        {
           //TODO:
        }

        /// <summary>
        /// Builds the <see cref="UnityEngine.Camera"/> list.
        /// </summary>
        /// <param name="wrapperGameObject">Wrapper <see cref="UnityEngine.GameObject"/></param>
        /// <param name="scene">Previously loaded scene pointer.</param>
        /// <param name="options"><see cref="AssetLoaderOptions"/> used to load the object.</param>
        private static void BuildCameras(GameObject wrapperGameObject, IntPtr scene, AssetLoaderOptions options)
        {
            for (uint c = 0; c < AssimpInterop.aiScene_GetNumCameras(scene); c++)
            {
                var camera = AssimpInterop.aiScene_GetCamera(scene, c);
                var cameraName = AssimpInterop.aiCamera_GetName(camera);
                var cameraGameObject = wrapperGameObject.transform.FindDeepChild(cameraName);
                if (cameraGameObject == null)
                {
#if ASSIMP_OUTPUT_MESSAGES
                    Debug.LogFormat("Camera node '{0}' not found", cameraName);
#endif
                    continue;
                }
                var unityCamera = cameraGameObject.gameObject.AddComponent<Camera>();
                unityCamera.aspect = AssimpInterop.aiCamera_GetAspect(camera);
                unityCamera.nearClipPlane = AssimpInterop.aiCamera_GetClipPlaneNear(camera);
                unityCamera.farClipPlane = AssimpInterop.aiCamera_GetClipPlaneFar(camera);
                unityCamera.fieldOfView = AssimpInterop.aiCamera_GetHorizontalFOV(camera);
                unityCamera.transform.localPosition = AssimpInterop.aiCamera_GetPosition(camera);
                unityCamera.transform.LookAt(AssimpInterop.aiCamera_GetLookAt(camera), AssimpInterop.aiCamera_GetUp(camera));
            }
        }

        /// <summary>
        /// Builds the <see cref="UnityEngine.Material"/> list.
        /// </summary>
        /// <param name="filename">Asset filename, if present.</param>
        /// <param name="scene">Previously loaded scene pointer.</param>
        /// <param name="options"><see cref="AssetLoaderOptions"/> used to load the object.</param>
        private void BuildMaterials(string filename, IntPtr scene, AssetLoaderOptions options)
        {
            string fileDirectory = null;
            string filenameWithoutExtension = null;
            if (filename != null)
            {
                var fileInfo = new FileInfo(filename);
                Debug.Assert(fileInfo.Directory != null, "fileInfo.Directory != null");
				fileDirectory = fileInfo.Directory.FullName;
				filenameWithoutExtension = Path.GetFileNameWithoutExtension(filename);
            }
            var texturesPath = !string.IsNullOrEmpty(options.TexturesPathOverride)
                ? options.TexturesPathOverride
                : fileDirectory;
            var materialsOverride = options.MaterialsOverride ?? new List<Material>();
            for (uint m = 0; m < AssimpInterop.aiScene_GetNumMaterials(scene); m++)
            {
                var material = AssimpInterop.aiScene_GetMaterial(scene, m);
                Material unityMaterial;
                bool isOverriden;
                if (materialsOverride.Count > m)
                {
                    isOverriden = true;
                    unityMaterial = materialsOverride[(int)m];
                }
                else
                {
                    isOverriden = false;
                    string materialName;
                    if (AssimpInterop.aiMaterial_HasName(material))
                    {
                        if (!AssimpInterop.aiMaterial_GetName(material, out materialName))
                        {
#if ASSIMP_OUTPUT_MESSAGES
                            Debug.LogWarning("Error loading material name");
#endif
                            materialName = "Material_" + StringUtils.GenerateUniqueName(m);
                        }
                    }
                    else
                    {
                        materialName = "Material_" + StringUtils.GenerateUniqueName(m);
                    }   
					var hasDiffuseAlphaTexture = false;
					var alpha = 1f;
					if (AssimpInterop.aiMaterial_HasOpacity(material))
					{
						float tmpAlpha;
						if (AssimpInterop.aiMaterial_GetOpacity(material, out tmpAlpha))
						{
							alpha = tmpAlpha;
						}
					}
					Texture2D diffuseTexture = null;
					var textureDiffuseLoaded = false;
					var numDiffuse = AssimpInterop.aiMaterial_GetNumTextureDiffuse(material);
					if (numDiffuse > 0)
					{
						string path;
						uint textureMapping;
						uint uvIndex;
						float blendMode;
						uint op;
						uint mapMode;
						if (AssimpInterop.aiMaterial_GetTextureDiffuse(material, 0, out path, out textureMapping,
							out uvIndex,
							out blendMode, out op, out mapMode))
						{
							var wrapMode = mapMode == (uint)TextureWrapMode.Clamp
								? TextureWrapMode.Clamp
								: TextureWrapMode.Repeat;
							var textureName = StringUtils.GenerateUniqueName(path);
							var checkAlphaChannel = options.ApplyAlphaMaterials;
							diffuseTexture = Texture2DUtils.LoadTextureFromFile(scene, path, textureName, null, null, ref checkAlphaChannel, wrapMode, texturesPath, OnTextureLoaded, options.TextureCompression, filenameWithoutExtension);
							hasDiffuseAlphaTexture = options.ApplyAlphaMaterials && checkAlphaChannel;
							textureDiffuseLoaded = true;
						}
						#if ASSIMP_OUTPUT_MESSAGES     
						else
						{
						Debug.LogWarning("Error loading diffuse texture " + m);
						}
						#endif
					}
					var hasSpecular = AssimpInterop.aiMaterial_HasSpecular(material);
                    var numSpecular = AssimpInterop.aiMaterial_GetNumTextureSpecular(material);
					if (hasDiffuseAlphaTexture || alpha < 1f) {
						if (options.UseCutoutMaterials) {
							unityMaterial = new Material (options.UseStandardSpecularMaterial && (hasSpecular || numSpecular > 0) ? _standardSpecularCutoutMaterial : _standardBaseCutoutMaterial);
						} else {
							unityMaterial = new Material (options.UseStandardSpecularMaterial && (hasSpecular || numSpecular > 0) ? _standardSpecularAlphaMaterial : _standardBaseAlphaMaterial);
						}
					} else {
						unityMaterial = new Material (options.UseStandardSpecularMaterial && (hasSpecular || numSpecular > 0) ? _standardSpecularMaterial : _standardBaseMaterial);
					}
                    unityMaterial.name = materialName;  
					if (!textureDiffuseLoaded) {
						unityMaterial.SetTexture ("_MainTex", null);
					} else {
						unityMaterial.SetTexture ("_MainTex", diffuseTexture);
					}
                    var diffuseLoaded = false;
                    if (AssimpInterop.aiMaterial_HasDiffuse(material))
                    {
                        Color colorDiffuse;
                        if (AssimpInterop.aiMaterial_GetDiffuse(material, out colorDiffuse))
                        {
                            colorDiffuse.a = alpha;
                            unityMaterial.SetColor("_Color", colorDiffuse);
                            diffuseLoaded = true;
                        }
#if ASSIMP_OUTPUT_MESSAGES  
                        else
                        {
                            Debug.LogWarning("Error loading diffuse color");
                        }
#endif
                    }
                    if (!diffuseLoaded)
                    {
                        unityMaterial.SetColor("_Color", Color.white);
                    }
                    var emissiveLoaded = false;
                    var hasEmissive = AssimpInterop.aiMaterial_HasEmissive(material);
                    if (hasEmissive)
                    {
                        Color colorEmissive;
                        if (AssimpInterop.aiMaterial_GetEmissive(material, out colorEmissive))
                        {
                            unityMaterial.SetColor("_EmissionColor", colorEmissive);
                            emissiveLoaded = true;
                        }
#if ASSIMP_OUTPUT_MESSAGES
                        else
                        {
                            Debug.LogWarning("Error loading emissive color");
                        }
#endif
                    }
                    if (!emissiveLoaded)
                    {
                        unityMaterial.SetColor("_EmissionColor", Color.black);
                    }
                    var emissiveTextureLoaded = false;
                    var numEmissive = AssimpInterop.aiMaterial_GetNumTextureEmissive(material);
                    if (numEmissive > 0)
                    {
                        string path;
                        uint textureMapping;
                        uint uvIndex;
                        float blendMode;
                        uint op;
                        uint mapMode;
                        if (AssimpInterop.aiMaterial_GetTextureEmissive(material, 0, out path, out textureMapping,
                                out uvIndex,
                                out blendMode, out op, out mapMode))
                        {
                            var wrapMode = mapMode == (uint)TextureWrapMode.Clamp
                                ? TextureWrapMode.Clamp
                                : TextureWrapMode.Repeat;
                            var textureName = StringUtils.GenerateUniqueName(path);
							var checkAlphaChannel = false;
							Texture2DUtils.LoadTextureFromFile(scene, path, textureName, unityMaterial, "_EmissionMap", ref checkAlphaChannel, wrapMode, texturesPath, OnTextureLoaded, options.TextureCompression, filenameWithoutExtension);
                            emissiveTextureLoaded = true;
                        }
#if ASSIMP_OUTPUT_MESSAGES         
                        else
                        {
                            Debug.LogWarning("Error loading emissive texture");
                        }
#endif
                    }
                    if (!emissiveTextureLoaded)
                    {
                        unityMaterial.SetTexture("_EmissionMap", null);
                        if (!emissiveLoaded)
                        {
                            unityMaterial.DisableKeyword("_EMISSION");
                        }
                    }
                    var specularLoaded = false;
                    if (hasSpecular)
                    {
                        Color colorSpecular;
                        if (AssimpInterop.aiMaterial_GetSpecular(material, out colorSpecular))
                        {
                            colorSpecular.a = alpha;
                            unityMaterial.SetColor("_SpecColor", colorSpecular);
                            specularLoaded = true;
                        }
#if ASSIMP_OUTPUT_MESSAGES
                        else
                        {
                            Debug.LogWarning("Error loading specular color");
                        }
#endif
                    }
                    if (!specularLoaded)
                    {
                        unityMaterial.SetColor("_SpecColor", Color.black);
                    }
                    var specularTextureLoaded = false;
                    if (numSpecular > 0)
                    {
                        string path;
                        uint textureMapping;
                        uint uvIndex;
                        float blendMode;
                        uint op;
                        uint mapMode;
                        if (AssimpInterop.aiMaterial_GetTextureSpecular(material, 0, out path, out textureMapping,
                                out uvIndex,
                                out blendMode, out op, out mapMode))
                        {
                            var wrapMode = mapMode == (uint)TextureWrapMode.Clamp
                                ? TextureWrapMode.Clamp
                                : TextureWrapMode.Repeat;
							var textureName = StringUtils.GenerateUniqueName(path);
							var checkAlphaChannel = false;
							Texture2DUtils.LoadTextureFromFile(scene, path, textureName, unityMaterial, "_SpecGlossMap", ref checkAlphaChannel, wrapMode, texturesPath, OnTextureLoaded, options.TextureCompression, filenameWithoutExtension);
                            specularTextureLoaded = true;
                        }
#if ASSIMP_OUTPUT_MESSAGES
                        else
                        {
                            Debug.LogWarning("Error loading specular texture");
                        }
#endif
                    }
                    if (!specularTextureLoaded)
                    {
                        unityMaterial.SetTexture("_SpecGlossMap", null);
                        unityMaterial.DisableKeyword("_SPECGLOSSMAP");
                    }
                    var normalTextureLoaded = false;
                    var numNormals = AssimpInterop.aiMaterial_GetNumTextureNormals(material);
                    if (numNormals > 0)
                    {
                        string path;
                        uint textureMapping;
                        uint uvIndex;
                        float blendMode;
                        uint op;
                        uint mapMode;
                        if (AssimpInterop.aiMaterial_GetTextureNormals(material, 0, out path, out textureMapping,
                                out uvIndex,
                                out blendMode, out op, out mapMode))
                        {
                            var wrapMode = mapMode == (uint)TextureWrapMode.Clamp
                                ? TextureWrapMode.Clamp
                                : TextureWrapMode.Repeat;
                            var textureName = StringUtils.GenerateUniqueName(path);
							var checkAlphaChannel = false;
							Texture2DUtils.LoadTextureFromFile(scene, path, textureName, unityMaterial, "_BumpMap", ref checkAlphaChannel, wrapMode, texturesPath, OnTextureLoaded, options.TextureCompression, filenameWithoutExtension, true);
                            normalTextureLoaded = true;
                        }
#if ASSIMP_OUTPUT_MESSAGES
                        else
                        {
                            Debug.LogWarning("Error loading normals texture");
                        }
#endif
                    }
                    var heightTextureLoaded = false;
                    var numHeight = AssimpInterop.aiMaterial_GetNumTextureHeight(material);
                    if (numHeight > 0)
                    {
                        string path;
                        uint textureMapping;
                        uint uvIndex;
                        float blendMode;
                        uint op;
                        uint mapMode;
                        if (AssimpInterop.aiMaterial_GetTextureHeight(material, 0, out path, out textureMapping,
                                out uvIndex,
                                out blendMode, out op, out mapMode))
                        {
                            var wrapMode = mapMode == (uint)TextureWrapMode.Clamp
                                ? TextureWrapMode.Clamp
                                : TextureWrapMode.Repeat;
                            var textureName = StringUtils.GenerateUniqueName(path);
							var checkAlphaChannel = false;
							Texture2DUtils.LoadTextureFromFile(scene, path, textureName, unityMaterial, "_BumpMap", ref checkAlphaChannel, wrapMode, texturesPath, OnTextureLoaded, options.TextureCompression, filenameWithoutExtension);
                            heightTextureLoaded = true;
                        }
#if ASSIMP_OUTPUT_MESSAGES
                        else
                        {
                            Debug.LogWarning("Error loading normals texture");
                        }
#endif
                    }
                    if (!heightTextureLoaded && !normalTextureLoaded)
                    {
                        unityMaterial.SetTexture("_BumpMap", null);
                        unityMaterial.DisableKeyword("_NORMALMAP");
                    }
                    var bumpScaleLoaded = false;
                    if (AssimpInterop.aiMaterial_HasBumpScaling(material))
                    {
                        float bumpScaling;
                        if (AssimpInterop.aiMaterial_GetBumpScaling(material, out bumpScaling))
                        {
                            if (Mathf.Approximately(bumpScaling, 0f))
                            {
                                bumpScaling = 1f;
                            }
                            unityMaterial.SetFloat("_BumpScale", bumpScaling);
                            bumpScaleLoaded = true;
                        }
#if ASSIMP_OUTPUT_MESSAGES
                        else
                        {
                            Debug.LogWarning("Error loading bump scaling");
                        }
#endif
                    }
                    if (!bumpScaleLoaded)
                    {
                        unityMaterial.SetFloat("_BumpScale", 1f);
                    }
                    var shininessLoaded = false;
                    if (AssimpInterop.aiMaterial_HasShininess(material))
                    {
                        float shininess;
                        if (AssimpInterop.aiMaterial_GetShininess(material, out shininess))
                        {
                            unityMaterial.SetFloat("_Glossiness", shininess);
                            shininessLoaded = true;
                        }
#if ASSIMP_OUTPUT_MESSAGES
                        else
                        {
                            Debug.LogWarning("Error loading shininess");
                        }
#endif
                    }
                    if (!shininessLoaded)
                    {
                        unityMaterial.SetFloat("_Glossiness", 0.5f);
                    }
                    var shininessStrengthLoaded = false;
                    if (AssimpInterop.aiMaterial_HasShininessStrength(material))
                    {
                        float shininessStrength;
                        if (AssimpInterop.aiMaterial_GetShininessStrength(material, out shininessStrength))
                        {
                            unityMaterial.SetFloat("_GlossMapScale", shininessStrength);
                            shininessStrengthLoaded = true;
                        }
                        else
                        {
#if ASSIMP_OUTPUT_MESSAGES
                            Debug.LogWarning("Error loading shininess strength");
#endif
                            unityMaterial.SetFloat("_GlossMapScale", 1f);
                        }
                    }
                    if (!shininessStrengthLoaded)
                    {
                        unityMaterial.SetFloat("_GlossMapScale", 1f);
                    }
                }
                if (unityMaterial == null)
                {
                    continue;
                }
                var materialData = new MaterialData { UnityMaterial = unityMaterial };
                _materialData[m] = materialData;
                if (OnMaterialCreated != null)
                {
                    OnMaterialCreated(m, isOverriden, unityMaterial);
                }
            }
        }

        /// <summary>
        /// Builds the bones and binding poses.
        /// </summary>
        /// <param name="scene">Previously loaded scene pointer.</param>
        private void BuildBones(IntPtr scene)
        {
            var meshCount = AssimpInterop.aiScene_GetNumMeshes(scene);
            for (uint m = 0; m < meshCount; m++)
            {
                var meshData = _meshData[m];
                var mesh = AssimpInterop.aiScene_GetMesh(scene, m);
                var unityMesh = meshData.UnityMesh;
                if (AssimpInterop.aiMesh_HasBones(mesh))
                {
                    var vertexCount = AssimpInterop.aiMesh_VertexCount(mesh);
                    var boneCount = AssimpInterop.aiMesh_GetNumBones(mesh);
                    var unityBindPoses = new Matrix4x4[boneCount];
                    var unityBoneTransforms = new Transform[boneCount];
                    var unityBoneWeights = new BoneWeight[vertexCount];
                    var unityBonesInVertices = new int[vertexCount];
                    for (uint b = 0; b < boneCount; b++)
                    {
                        var bone = AssimpInterop.aiMesh_GetBone(mesh, b);
                        var boneName = AssimpInterop.aiBone_GetName(bone);
                        if (!_nodeDataDictionary.ContainsKey(boneName))
                        {
#if ASSIMP_OUTPUT_MESSAGES
                            Debug.LogWarningFormat("Bone {0} not found in hierarchy", boneName);
#endif
                            continue;
                        }
                        var nodeData = _nodeDataDictionary[boneName];
                        var boneGameObject = nodeData.GameObject;
                        var unityBoneTransform = boneGameObject.transform;
                        unityBoneTransforms[b] = unityBoneTransform;
                        var unityBindPose = AssimpInterop.aiBone_GetOffsetMatrix(bone);
                        unityBindPoses[b] = unityBindPose;
                        var vertexWeightCount = AssimpInterop.aiBone_GetNumWeights(bone);
                        for (uint w = 0; w < vertexWeightCount; w++)
                        {
                            var vertexWeight = AssimpInterop.aiBone_GetWeights(bone, w);
                            var weightValue = AssimpInterop.aiVertexWeight_GetWeight(vertexWeight);
                            var weightVertexId = AssimpInterop.aiVertexWeight_GetVertexId(vertexWeight);
                            BoneWeight unityBoneWeight;
                            var unityCurrentBonesInVertex = unityBonesInVertices[weightVertexId];
                            var wInt = (int)b;
                            if (unityCurrentBonesInVertex == 0)
                            {
                                unityBoneWeight = new BoneWeight
                                {
                                    boneIndex0 = wInt,
                                    weight0 = weightValue
                                };
                                unityBoneWeights[weightVertexId] = unityBoneWeight;
                            }
                            else if (unityCurrentBonesInVertex == 1)
                            {
                                unityBoneWeight = unityBoneWeights[weightVertexId];
                                unityBoneWeight.boneIndex1 = wInt;
                                unityBoneWeight.weight1 = weightValue;
                                unityBoneWeights[weightVertexId] = unityBoneWeight;
                            }
                            else if (unityCurrentBonesInVertex == 2)
                            {
                                unityBoneWeight = unityBoneWeights[weightVertexId];
                                unityBoneWeight.boneIndex2 = wInt;
                                unityBoneWeight.weight2 = weightValue;
                                unityBoneWeights[weightVertexId] = unityBoneWeight;
                            }
                            else if (unityCurrentBonesInVertex == 3)
                            {
                                unityBoneWeight = unityBoneWeights[weightVertexId];
                                unityBoneWeight.boneIndex3 = wInt;
                                unityBoneWeight.weight3 = weightValue;
                                unityBoneWeights[weightVertexId] = unityBoneWeight;
                            }
                            else
                            {
#if ASSIMP_OUTPUT_MESSAGES
                                Debug.LogWarningFormat("Vertex {0} has more than 4 bone weights. This is not supported", weightVertexId);
#endif
                                unityBoneWeight = unityBoneWeights[weightVertexId];
                                unityBoneWeight.boneIndex3 = wInt;
                                unityBoneWeight.weight3 = weightValue;
                                unityBoneWeights[weightVertexId] = unityBoneWeight;
                            }
                            unityBonesInVertices[weightVertexId]++;
                        }
                    }
                    var skinnedMeshRenderer = meshData.SkinnedMeshRenderer;
                    skinnedMeshRenderer.bones = unityBoneTransforms;
                    unityMesh.bindposes = unityBindPoses;
                    unityMesh.boneWeights = unityBoneWeights;
                }
                if (OnMeshCreated != null)
                {
                    OnMeshCreated(m, unityMesh);
                }
            }
        }

        /// <summary>
        /// Builds the <see cref="UnityEngine.AnimationClip"/> list.
        /// </summary>
        /// <param name="wrapperGameObject">Wrapper <see cref="UnityEngine.GameObject"/> to add the animation component.</param>
        /// <param name="scene">Previously loaded scene pointer.</param>
        /// <param name="options"><see cref="AssetLoaderOptions"/> used to load the object.</param>
        private void BuildAnimations(GameObject wrapperGameObject, IntPtr scene, AssetLoaderOptions options)
        {
            var animationCount = AssimpInterop.aiScene_GetNumAnimations(scene);
            var unityAnimationClips = new AnimationClip[animationCount];
            for (uint a = 0; a < animationCount; a++)
            {
                var sceneAnimation = AssimpInterop.aiScene_GetAnimation(scene, a);
                var ticksPerSecond = AssimpInterop.aiAnimation_GetTicksPerSecond(sceneAnimation);
                if (ticksPerSecond <= 0)
                {
                    ticksPerSecond = 60f;
                }
                var sceneAnimatioName = AssimpInterop.aiAnimation_GetName(sceneAnimation);
                var unityAnimationClip = new AnimationClip
                {
                    name =
                        string.IsNullOrEmpty(sceneAnimatioName)
                            ? "Animation_" + StringUtils.GenerateUniqueName(a)
                            : sceneAnimatioName,
#if UNITY_EDITOR
                    legacy = options.UseLegacyAnimations,
#else
                        legacy = true,
#endif
                    frameRate = ticksPerSecond,
                };
                var durationInTicks = AssimpInterop.aiAnimation_GetDuraction(sceneAnimation);
                var totalTime = durationInTicks / ticksPerSecond;
                var animationChannelCount = AssimpInterop.aiAnimation_GetNumChannels(sceneAnimation);
                for (uint n = 0; n < animationChannelCount; n++)
                {
                    var nodeAnimationChannel = AssimpInterop.aiAnimation_GetAnimationChannel(sceneAnimation, n);
                    var nodeName = AssimpInterop.aiNodeAnim_GetNodeName(nodeAnimationChannel);
                    if (string.IsNullOrEmpty(nodeName))
                    {
#if ASSIMP_OUTPUT_MESSAGES
                        Debug.LogWarningFormat("Cannot find node for animation channel {0} from animation clip {1}", n, a);
#endif
                        continue;
                    }
                    if (!_nodeDataDictionary.ContainsKey(nodeName))
                    {
#if ASSIMP_OUTPUT_MESSAGES
                        Debug.LogWarningFormat("Cannot find node {0}", nodeName);
#endif
                        continue;
                    }
                    var nodeData = _nodeDataDictionary[nodeName];
                    var numRotationKeys = AssimpInterop.aiNodeAnim_GetNumRotationKeys(nodeAnimationChannel);
                    if (numRotationKeys > 0)
                    {
                        var unityRotationCurveX = new AnimationCurve();
                        var unityRotationCurveY = new AnimationCurve();
                        var unityRotationCurveZ = new AnimationCurve();
                        var unityRotationCurveW = new AnimationCurve();
                        for (uint r = 0; r < numRotationKeys; r++)
                        {
                            var rotationKey = AssimpInterop.aiNodeAnim_GetRotationKey(nodeAnimationChannel, r);
                            var time = AssimpInterop.aiQuatKey_GetTime(rotationKey) / ticksPerSecond;
                            var unityQuaternion = AssimpInterop.aiQuatKey_GetValue(rotationKey);
                            unityRotationCurveX.AddKey(time, unityQuaternion.x);
                            unityRotationCurveY.AddKey(time, unityQuaternion.y);
                            unityRotationCurveZ.AddKey(time, unityQuaternion.z);
                            unityRotationCurveW.AddKey(time, unityQuaternion.w);
                        }
                        unityAnimationClip.SetCurve(nodeData.Path, typeof(Transform), "localRotation.x",
                            FixCurve(totalTime, unityRotationCurveX));
                        unityAnimationClip.SetCurve(nodeData.Path, typeof(Transform), "localRotation.y",
                            FixCurve(totalTime, unityRotationCurveY));
                        unityAnimationClip.SetCurve(nodeData.Path, typeof(Transform), "localRotation.z",
                            FixCurve(totalTime, unityRotationCurveZ));
                        unityAnimationClip.SetCurve(nodeData.Path, typeof(Transform), "localRotation.w",
                            FixCurve(totalTime, unityRotationCurveW));
                    }
                    var numPositionKeys = AssimpInterop.aiNodeAnim_GetNumPositionKeys(nodeAnimationChannel);
                    if (numPositionKeys > 0)
                    {
                        var unityPositionCurveX = new AnimationCurve();
                        var unityPositionCurveY = new AnimationCurve();
                        var unityPositionCurveZ = new AnimationCurve();
                        for (uint p = 0; p < numPositionKeys; p++)
                        {
                            var positionKey = AssimpInterop.aiNodeAnim_GetPositionKey(nodeAnimationChannel, p);
                            var time = AssimpInterop.aiVectorKey_GetTime(positionKey) / ticksPerSecond;
                            var unityVector3 = AssimpInterop.aiVectorKey_GetValue(positionKey);
                            unityPositionCurveX.AddKey(time, unityVector3.x);
                            unityPositionCurveY.AddKey(time, unityVector3.y);
                            unityPositionCurveZ.AddKey(time, unityVector3.z);
                        }
                        unityAnimationClip.SetCurve(nodeData.Path, typeof(Transform), "localPosition.x",
                            FixCurve(totalTime, unityPositionCurveX));
                        unityAnimationClip.SetCurve(nodeData.Path, typeof(Transform), "localPosition.y",
                            FixCurve(totalTime, unityPositionCurveY));
                        unityAnimationClip.SetCurve(nodeData.Path, typeof(Transform), "localPosition.z",
                            FixCurve(totalTime, unityPositionCurveZ));
                    }
                    var numScalingKeys = AssimpInterop.aiNodeAnim_GetNumScalingKeys(nodeAnimationChannel);
                    if (numScalingKeys > 0)
                    {
                        var unityScaleCurveX = new AnimationCurve();
                        var unityScaleCurveY = new AnimationCurve();
                        var unityScaleCurveZ = new AnimationCurve();
                        for (uint s = 0; s < numScalingKeys; s++)
                        {
                            var scaleKey = AssimpInterop.aiNodeAnim_GetScalingKey(nodeAnimationChannel, s);
                            var time = AssimpInterop.aiVectorKey_GetTime(scaleKey) / ticksPerSecond;
                            var unityVector3 = AssimpInterop.aiVectorKey_GetValue(scaleKey);
                            unityScaleCurveX.AddKey(time, unityVector3.x);
                            unityScaleCurveY.AddKey(time, unityVector3.y);
                            unityScaleCurveZ.AddKey(time, unityVector3.z);
                        }
                        unityAnimationClip.SetCurve(nodeData.Path, typeof(Transform), "localScale.x",
                            FixCurve(totalTime, unityScaleCurveX));
                        unityAnimationClip.SetCurve(nodeData.Path, typeof(Transform), "localScale.y",
                            FixCurve(totalTime, unityScaleCurveY));
                        unityAnimationClip.SetCurve(nodeData.Path, typeof(Transform), "localScale.z",
                            FixCurve(totalTime, unityScaleCurveZ));
                    }
                }
                unityAnimationClip.EnsureQuaternionContinuity();
                unityAnimationClip.wrapMode = options.AnimationWrapMode;
                unityAnimationClips[a] = unityAnimationClip;
                if (OnAnimationClipCreated != null)
                {
                    OnAnimationClipCreated(a, unityAnimationClip);
                }
            }
            if (options.UseLegacyAnimations)
            {
                var unityAnimation = wrapperGameObject.GetComponent<Animation>();
                if (unityAnimation == null)
                {
                    unityAnimation = wrapperGameObject.AddComponent<Animation>();
                }
                AnimationClip defaultClip = null;
                for (var c = 0; c < unityAnimationClips.Length; c++)
                {
                    var unityAnimationClip = unityAnimationClips[c];
                    unityAnimation.AddClip(unityAnimationClip, unityAnimationClip.name);
                    if (c == 0)
                    {
                        defaultClip = unityAnimationClip;
                    }
                }
                unityAnimation.clip = defaultClip;
                if (options.AutoPlayAnimations)
                {
                    unityAnimation.Play();
                }
            }
            else
            {
                var unityAnimator = wrapperGameObject.GetComponent<Animator>();
                if (unityAnimator == null)
                {
                    unityAnimator = wrapperGameObject.AddComponent<Animator>();
                }
                if (options.AnimatorController != null)
                {
                    unityAnimator.runtimeAnimatorController = options.AnimatorController;
                }
				if (options.Avatar != null) {
					unityAnimator.avatar = options.Avatar;
				} else {
					unityAnimator.avatar = AvatarBuilder.BuildGenericAvatar(wrapperGameObject, "");
				}
            }
        }

        /// <summary>
        /// Builds the wrapper <see cref="UnityEngine.GameObject"/>.
        /// </summary>
        /// <param name="scene">Previously loaded scene poitner.</param>
        /// <param name="options"><see cref="AssetLoaderOptions"/> used to load the object.</param>
        /// <param name="templateObject">Use this field to load the object into this template <see cref="UnityEngine.GameObject"/> structure.</param> 
        /// <returns>A new <see cref="UnityEngine.GameObject"/>.</returns>
        private GameObject BuildWrapperObject(IntPtr scene, AssetLoaderOptions options, GameObject templateObject = null)
        {
            var rootNode = AssimpInterop.aiScene_GetRootNode(scene);
            var rootNodeData = new NodeData();
            var rootNodeId = _nodeId++;
            rootNodeData.Node = rootNode;
            rootNodeData.Id = rootNodeId;
            var fixedRootName = FixName(AssimpInterop.aiNode_GetName(rootNode), rootNodeId);
            rootNodeData.Name = fixedRootName;
            rootNodeData.Path = fixedRootName;
            var wrapperGameObject = templateObject ?? new GameObject
            {
                name = string.Format("Wrapper_{0}", fixedRootName)
            };
            var rootGameObject = BuildObject(scene, rootNodeData, options);
            LoadContextOptions(rootGameObject, options);
            rootGameObject.transform.parent = wrapperGameObject.transform;
            return wrapperGameObject;
        }

        /// <summary>
        /// Builds a <see cref="UnityEngine.GameObject"/> in the hierarchy.
        /// </summary>
        /// <param name="scene">Previously loaded scene pointer.</param>
        /// <param name="nodeData"><see cref="NodeData"/> related to this object.</param>
        /// <param name="options"><see cref="AssetLoaderOptions"/> used to load the object.</param>
        /// <returns></returns>
        private GameObject BuildObject(IntPtr scene, NodeData nodeData, AssetLoaderOptions options)
        {
            var gameObject = new GameObject
            {
                name = nodeData.Name
            };
            var node = nodeData.Node;
            var meshCount = AssimpInterop.aiNode_GetNumMeshes(node);
            var sceneHasMeshes = AssimpInterop.aiScene_HasMeshes(scene);
            if (meshCount > 0 && sceneHasMeshes)
            {
                for (uint m = 0; m < meshCount; m++)
                {
                    var meshIndex = AssimpInterop.aiNode_GetMeshIndex(node, m);
                    var mesh = AssimpInterop.aiScene_GetMesh(scene, meshIndex);
                    var materialIndex = AssimpInterop.aiMesh_GetMatrialIndex(mesh);
                    Material unityMaterial = null;
                    if (_materialData != null)
                    {
                        var materialDataItem = _materialData[materialIndex];
                        if (materialDataItem != null)
                        {
                            unityMaterial = materialDataItem.UnityMaterial;
                        }
                    }
                    if (unityMaterial == null)
                    {
#if ASSIMP_OUTPUT_MESSAGES
                        Debug.LogWarning("No material for mesh");
#endif
                        unityMaterial = _standardBaseMaterial;
                    }
                    var meshData = _meshData[meshIndex];
                    var unityMesh = meshData.UnityMesh;
                    var subGameObject = new GameObject
                    {
                        name = string.Format("<{0}:Mesh:{1}>", gameObject.name, m)
                    };
                    subGameObject.transform.parent = gameObject.transform;
                    var meshFilter = subGameObject.AddComponent<MeshFilter>();
                    meshFilter.mesh = unityMesh;
                    if (AssimpInterop.aiMesh_HasBones(mesh))
                    {
                        var skinnedMeshRenderer = subGameObject.AddComponent<SkinnedMeshRenderer>();
                        skinnedMeshRenderer.sharedMesh = unityMesh;
                        skinnedMeshRenderer.quality = SkinQuality.Bone4;
                        skinnedMeshRenderer.sharedMaterial = unityMaterial;
                        meshData.SkinnedMeshRenderer = skinnedMeshRenderer;
                    }
                    else
                    {
                        var meshRenderer = subGameObject.AddComponent<MeshRenderer>();
                        meshRenderer.sharedMaterial = unityMaterial;
                        if (options.GenerateMeshColliders)
                        {
                            var meshCollider = subGameObject.AddComponent<MeshCollider>();
                            meshCollider.sharedMesh = unityMesh;
                            meshCollider.convex = options.ConvexMeshColliders;
                        }
                    }
                }
            }
            if (nodeData.ParentNodeData != null)
            {
                gameObject.transform.parent = nodeData.ParentNodeData.GameObject.transform;
            }
            gameObject.transform.LoadMatrix(AssimpInterop.aiNode_GetTransformation(node));
            nodeData.GameObject = gameObject;
            _nodeDataDictionary.Add(nodeData.Name, nodeData);
            var childrenCount = AssimpInterop.aiNode_GetNumChildren(node);
            if (childrenCount > 0)
            {
                for (uint c = 0; c < childrenCount; c++)
                {
                    var child = AssimpInterop.aiNode_GetChildren(node, c);
                    var childId = _nodeId++;
                    var childNodeData = new NodeData
                    {
                        ParentNodeData = nodeData,
                        Node = child,
                        Id = childId,
                        Name = FixName(AssimpInterop.aiNode_GetName(child), childId)
                    };
                    childNodeData.Path = string.Format("{0}/{1}", nodeData.Path, childNodeData.Name);
                    BuildObject(scene, childNodeData, options);
                }
            }
            return gameObject;
        }

        /// <summary>
        /// Generates a unique name, if the given name is empty.
        /// </summary>
        /// <param name="name">Name to check.</param>
        /// <param name="id">Id to generate the unique name.</param>
        /// <returns>Final name.</returns>
        private string FixName(string name, int id)
        {
			return string.IsNullOrEmpty(name) || _nodeDataDictionary.ContainsKey(name) ? StringUtils.GenerateUniqueName(id) : name;
        }

        /// <summary>
        /// Fixes animation curve length issues (curves containing only one key or with length too small).
        /// </summary>
        /// <param name="animationLength">Final animation length.</param>
        /// <param name="curve">Curve to fix.</param>
        /// <returns>Fixed curve.</returns>
        private static AnimationCurve FixCurve(float animationLength, AnimationCurve curve)
        {
            if (Mathf.Approximately(animationLength, 0f))
            {
                animationLength = 1f;
            }
            if (curve.keys.Length == 1)
            {
                curve.AddKey(new Keyframe(animationLength, curve.keys[0].value));
            }
            return curve;
        }
    }

    /// <summary>
    /// Represents an asset node x Unity <see cref="UnityEngine.GameObject"/> relationship.
    /// </summary>
    public class NodeData : IDisposable
    {
        /// <summary>
        /// Unity <see cref="UnityEngine.GameObject"/> associated with this object.
        /// </summary>
        public GameObject GameObject;

        /// <summary>
        /// Relationsip name.
        /// </summary>
        public string Name;

        /// <summary>
        /// Nde pointer associated with this object.
        /// </summary>
        public IntPtr Node;

        /// <summary>
        /// Parent <see cref="NodeData"/>.
        /// </summary>
        public NodeData ParentNodeData;

        /// <summary>
        /// Path to represent the associated <see cref="UnityEngine.GameObject"/> in hierarchy.
        /// </summary>
        public string Path;

        /// <summary>
        /// Identifier used to generate a unique name for this.
        /// </summary>
        public int Id;

        /// <inheritdoc />
        public void Dispose()
        {
            ParentNodeData = null;
            GameObject = null;
            Name = null;
            Path = null;
        }
    }

    /// <summary>
    /// Represents an asset mesh x Unity <see cref="UnityEngine.Mesh"/> relationship.
    /// </summary>
    public class MeshData : IDisposable
    {
        /// <summary>
        /// Unity <see cref="UnityEngine.SkinnedMeshRenderer"/> associated with this object.
        /// </summary>
        public SkinnedMeshRenderer SkinnedMeshRenderer;

        /// <summary>
        /// Unity <see cref="UnityEngine.Mesh"/> associated with this object.
        /// </summary>
        public Mesh UnityMesh;

        /// <inheritdoc />
        public void Dispose()
        {
            UnityMesh = null;
            SkinnedMeshRenderer = null;
        }
    }

    /// <summary>
    /// Represents an asset material x Unity <see cref="UnityEngine.Material"/> relationship.
    /// </summary>
    public class MaterialData : IDisposable
    {
        /// <summary>
        /// Unity <see cref="UnityEngine.Material"/> associated with this object.
        /// </summary>
        public Material UnityMaterial;

        /// <inheritdoc />
        public void Dispose()
        {
            UnityMaterial = null;
        }
    }
}
using UnityEngine;
using System.IO;
using System;
#if USE_DEVIL
using DevIL;
#endif

namespace TriLib
{
    /// <summary>
    /// Represents a texture compression parameter.
    /// </summary>
    public enum TextureCompression
    {
        /// <summary>
        /// No texture compression will be applied.
        /// </summary>
        None,

        /// <summary>
        /// Normal-quality texture compression will be applied.
        /// </summary>
        NormalQuality,

        /// <summary>
        /// High-quality texture compression will be applied.
        /// </summary>
        HighQuality
    }

    /// <summary>
    /// Represents a <see cref="UnityEngine.Texture2D"/> loading event handle.
    /// </summary>
	[Obsolete("The Material parameter is inconsistent after the alpha textures support update, so, it will be removed on future versions.")]
    public delegate void TextureLoadHandle(string sourcePath, Material material, string propertyName, Texture2D texture);

    /// <summary>
    /// Represents a class to load external textures.
    /// </summary>
    public static class Texture2DUtils
    {
		#pragma warning disable 612, 618
        /// <summary>
        /// Loads a <see cref="UnityEngine.Texture2D"/> from an external source.
        /// </summary>
        /// <param name="scene">Scene where the texture belongs.</param>
        /// <param name="path">Path to load the texture data.</param>
        /// <param name="name">Name of the <see cref="UnityEngine.Texture2D"/> to be created.</param>
        /// <param name="material"><see cref="UnityEngine.Material"/> to assign the <see cref="UnityEngine.Texture2D"/>.</param>
        /// <param name="propertyName"><see cref="UnityEngine.Material"/> property name to assign to the <see cref="UnityEngine.Texture2D"/>.</param>
		/// <param name="checkAlphaChannel">If True, checks every image pixel to determine if alpha channel is being used and sets this value.</param>
        /// <param name="textureWrapMode">Wrap mode of the <see cref="UnityEngine.Texture2D"/> to be created.</param>
        /// <param name="basePath">Base path to lookup for the <see cref="UnityEngine.Texture2D"/>.</param>
        /// <param name="onTextureLoaded">Event to trigger when the <see cref="UnityEngine.Texture2D"/> finishes loading.</param>
        /// <param name="textureCompression">Texture loading compression level.</param>
        /// <param name="textureFileNameWithoutExtension">Texture filename without the extension.</param>
        /// <param name="isNormalMap">Is the Texture a Normal Map?</param>
		/// <returns>The loaded <see cref="UnityEngine.Texture2D"/>.</returns> 
		public static Texture2D LoadTextureFromFile(IntPtr scene, string path, string name, Material material, string propertyName, ref bool checkAlphaChannel, TextureWrapMode textureWrapMode = TextureWrapMode.Repeat, string basePath = null, TextureLoadHandle onTextureLoaded = null, TextureCompression textureCompression = TextureCompression.None, string textureFileNameWithoutExtension = null, bool isNormalMap = false)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
            bool assimpUncompressed;
            string finalPath;
            byte[] data;
			var texture = AssimpInterop.aiScene_GetEmbeddedTexture (scene, path);
			if (texture != IntPtr.Zero)
            {
				assimpUncompressed = !AssimpInterop.aiMaterial_IsEmbeddedTextureCompressed(scene, texture);
				var dataLength = AssimpInterop.aiMaterial_GetEmbeddedTextureDataSize(scene, texture, !assimpUncompressed);
				data = AssimpInterop.aiMaterial_GetEmbeddedTextureData(scene, texture, dataLength);
				finalPath = Path.GetFileNameWithoutExtension(path);
            }
            else
            {
				string filename = null;
                finalPath = path;
				data = FileUtils.LoadFileData (finalPath);
				if (data.Length == 0 && basePath != null)
                {
                    finalPath = Path.Combine(basePath, path);
				}
				data = FileUtils.LoadFileData (finalPath);
				if (data.Length == 0) {
					filename = FileUtils.GetFilename(path);
					finalPath = filename;
				}
				data = FileUtils.LoadFileData (finalPath);
				if (data.Length == 0 && basePath != null && filename != null)
                {
                    finalPath = Path.Combine(basePath, filename);
				}
				data = FileUtils.LoadFileData (finalPath);
				if (data.Length == 0)
                {
#if ASSIMP_OUTPUT_MESSAGES
                    Debug.LogWarningFormat("Texture '{0}' not found", path);
#endif
                    return null;
                }
                assimpUncompressed = false;
            }
            bool loaded;
            Texture2D tempTexture2D;
            if (assimpUncompressed)
            {
                //TODO: additional DLL methods to load actual resolution
                var textureResolution = Mathf.FloorToInt(Mathf.Sqrt(data.Length / 4));
				tempTexture2D = new Texture2D(textureResolution, textureResolution, TextureFormat.ARGB32, true);
                tempTexture2D.LoadRawTextureData(data);
                tempTexture2D.Apply();
                loaded = true;
            }
            else
            {
#if USE_DEVIL && (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
                loaded = IlLoader.LoadTexture2DFromByteArray(data, data.Length, out tempTexture2D);  
#else
				tempTexture2D = new Texture2D(2, 2, TextureFormat.RGBA32, true);
                loaded = tempTexture2D.LoadImage(data);
#endif
            }
            tempTexture2D.name = name;
            tempTexture2D.wrapMode = textureWrapMode;
            if (loaded)
            {
                var colors = tempTexture2D.GetPixels32();
                var finalTexture2D = new Texture2D(tempTexture2D.width, tempTexture2D.height, TextureFormat.ARGB32, true);
                if (isNormalMap)
                {
                    for (var i = 0; i < colors.Length; i++)
                    {
                        var color = colors[i];
                        color.a = color.r;
                        color.r = 0;
                        color.b = 0;
                        colors[i] = color;
                    }
                    finalTexture2D.SetPixels32(colors);
                    finalTexture2D.Apply();
                }
                else
                {
                    finalTexture2D.SetPixels32(colors);
                    finalTexture2D.Apply();
                    if (textureCompression != TextureCompression.None)
                    {
                        tempTexture2D.Compress(textureCompression == TextureCompression.HighQuality);
                    }
                }
				if (checkAlphaChannel) {
					checkAlphaChannel = false;
					foreach (var color in colors) {
						if (color.a != 255) {
							checkAlphaChannel = true;
							break;
						}
					}
				}
				if (material != null) {
					material.SetTexture(propertyName, finalTexture2D);
				}
                if (onTextureLoaded != null)
                {
                    onTextureLoaded(finalPath, material, propertyName, finalTexture2D);
                }
				return finalTexture2D;
            }
            else
            {
#if ASSIMP_OUTPUT_MESSAGES
                Debug.LogErrorFormat("Unable to load texture '{0}'", path);
#endif
            }
			return null;
        }
	}
	#pragma warning restore 612, 618
}


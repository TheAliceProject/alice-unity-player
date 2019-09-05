using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace TriLib
{
    /// <summary>
    /// Material property types.
    /// </summary>
    public enum aiPropertyTypeInfo
    {
        aiPTI_Float = 0x1,
        aiPTI_Double = 0x2,
        aiPTI_String = 0x3,
        aiPTI_Integer = 0x4,
        aiPTI_Buffer = 0x5
    }

    /// <summary>
    /// Internally represents the material property interface.
    /// </summary>
    public interface IMaterialProperty
    {
        string Name { get; }
        Type Type { get; }
        uint Index { get; }
        uint Semantic { get; }
    }
    
    /// <summary>
    /// Internally represents a material property.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MaterialProperty<T> : IMaterialProperty
    {
        public string Name { get; private set; }
        public Type Type
        {
            get { return typeof(T); }
        }

        public uint Index { get; private set; }
        public uint Semantic { get; private set; }
        public T Data { get; private set; }

        public MaterialProperty(string name, T data, uint index, uint semantic)
        {
            Name = name;
            Data = data;
            Index = index;
            Semantic = semantic;
        }

        public override string ToString()
        {
            return string.Format("{0}({1})", Name, Type);
        }
    }

    /// <summary>
    /// Internally represents a Unity <see cref="UnityEngine.Material"/>.
    /// </summary>
    public class MaterialData
    {
        public string Name;

        public bool AlphaLoaded;
        public float Alpha;

        public bool DiffuseInfoLoaded;
        public string DiffusePath;
        public TextureWrapMode DiffuseWrapMode;
        public string DiffuseName;
        public float DiffuseBlendMode;
        public uint DiffuseOp;
        public EmbeddedTextureData DiffuseEmbeddedTextureData;

        public bool DiffuseColorLoaded;
        public Color DiffuseColor;

        public bool EmissionInfoLoaded;
        public string EmissionPath;
        public TextureWrapMode EmissionWrapMode;
        public string EmissionName;
        public float EmissionBlendMode;
        public uint EmissionOp;
        public EmbeddedTextureData EmissionEmbeddedTextureData;

        public bool EmissionColorLoaded;
        public Color EmissionColor;

        public bool SpecularInfoLoaded;
        public string SpecularPath;
        public TextureWrapMode SpecularWrapMode;
        public string SpecularName;
        public float SpecularBlendMode;
        public uint SpecularOp;
        public EmbeddedTextureData SpecularEmbeddedTextureData;

        public bool SpecularColorLoaded;
        public Color SpecularColor;

        public bool NormalInfoLoaded;
        public string NormalPath;
        public TextureWrapMode NormalWrapMode;
        public string NormalName;
        public float NormalBlendMode;
        public uint NormalOp;
        public EmbeddedTextureData NormalEmbeddedTextureData;

        public bool HeightInfoLoaded;
        public string HeightPath;
        public TextureWrapMode HeightWrapMode;
        public string HeightName;
        public float HeightBlendMode;
        public uint HeightOp;
        public EmbeddedTextureData HeightEmbeddedTextureData;

        public bool BumpScaleLoaded;
        public float BumpScale;

        public bool GlossinessLoaded;
        public float Glossiness;

        public bool GlossMapScaleLoaded;
        public float GlossMapScale;

        public bool OcclusionInfoLoaded;
        public string OcclusionPath;
        public TextureWrapMode OcclusionWrapMode;
        public string OcclusionName;
        public float OcclusionBlendMode;
        public uint OcclusionOp;
        public EmbeddedTextureData OcclusionEmbeddedTextureData;
        
        public bool MetallicInfoLoaded;
        public string MetallicPath;
        public TextureWrapMode MetallicWrapMode;
        public string MetallicName;
        public float MetallicBlendMode;
        public uint MetallicOp;
        public EmbeddedTextureData MetallicEmbeddedTextureData;

        public IMaterialProperty[] Properties;

        public Material Material;
    }
}
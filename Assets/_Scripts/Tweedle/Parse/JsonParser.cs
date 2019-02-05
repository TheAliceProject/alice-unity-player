using Alice.Tweedle.File;
using ICSharpCode.SharpZipLib.Zip;
using System.Collections.Generic;
using System.IO;

namespace Alice.Tweedle.Parse
{
    public class JsonParser
    {
        private TweedleSystem m_System;
        private TweedleParser m_Parser;
        private ZipFile m_ZipFile;

        public TweedleSystem StoredSystem
        {
            get { return m_System; }
        }

        public JsonParser(TweedleSystem inSystem, ZipFile inZipFile)
        {
            m_System = inSystem;
            m_ZipFile = inZipFile;
            m_Parser = new TweedleParser();
        }

        internal void Parse()
        {
            // TODO: Use manifest to determine player assembly version
            string playerAssembly = Player.PlayerAssemblies.CURRENT;
            m_System.AddStaticAssembly(Player.PlayerAssemblies.Assembly(playerAssembly));

            ParseJson(ReadEntry("manifest.json"), "");
        }

        public void ParseJson(string manifestJson, string pathBase = "")
        {
            Manifest asset = UnityEngine.JsonUtility.FromJson<Manifest>(manifestJson);
            JSONObject jsonObj = new JSONObject(manifestJson);

            ParseResourceDetails(
                asset.resources,
                jsonObj[MemberInfoGetter.GetMemberName(() => asset.resources)],
                pathBase
                );
            ProjectType t = asset.Identifier.Type;
            switch (t)
            {
                case ProjectType.Library:
                    LibraryManifest libAsset = new LibraryManifest(asset);
                    m_System.AddLibrary(libAsset);
                    break;
                case ProjectType.World:
                    ProgramDescription worldAsset = new ProgramDescription(asset);
                    m_System.AddProgram(worldAsset);
                    break;
                case ProjectType.Model:
                    ModelManifest modelAsset = UnityEngine.JsonUtility.FromJson<ModelManifest>(manifestJson);
                    m_System.AddModel(modelAsset);
                    break;
            }
        }

        string ReadEntry(string location)
        {
            ZipEntry entry = m_ZipFile.GetEntry(location);
            if (entry == null)
            {
                UnityEngine.Debug.Log("Did not find entry for: " + location);
            }
            return ReadEntry(entry);
        }

        string ReadEntry(ZipEntry entry)
        {
            Stream entryStream = m_ZipFile.GetInputStream(entry);
            return (new StreamReader(entryStream)).ReadToEnd();
        }

        byte[] ReadDataEntry(string location)
        {
            ZipEntry entry = m_ZipFile.GetEntry(location);
            if (entry == null)
            {
                var dirPath = Path.GetDirectoryName(location);
                var dirEntry =  m_ZipFile.GetEntry(dirPath);

                UnityEngine.Debug.Log("Did not find entry for: " + location + " dir entry " + dirPath);
                return null;
            }
            return ReadDataEntry(entry);
        }

        byte[] ReadDataEntry(ZipEntry entry)
        {
            Stream entryStream = m_ZipFile.GetInputStream(entry);
            return (new BinaryReader(entryStream)).ReadBytes((int)entry.Size);
        }

        private void ParseResourceDetails(
            List<ResourceReference> resources,
            JSONObject json,
            string pathBase
            )
        {
            if (json == null || json.type != JSONObject.Type.ARRAY)
            {
                return;
            }
            for (int i = 0; i < resources.Count; i++)
            {
                ResourceReference strictResource = ReadResource(resources[i], json.list[i].ToString(), pathBase);
                resources[i] = strictResource;
                m_System.AddResource(strictResource);
            }
        }

        private ResourceReference ReadResource(ResourceReference resourceRef, string refJson, string pathBase)
        {
            switch (resourceRef.ContentType)
            {
                case ContentType.Audio:
                    return UnityEngine.JsonUtility.FromJson<AudioReference>(refJson);
                case ContentType.Class:
                    for (int j = 0; j < resourceRef.files.Count; j++)
                    {
                        string tweedleCode = ReadEntry(pathBase + resourceRef.files[j]);
                        TType tweClass = (TType)m_Parser.ParseType(tweedleCode, m_System.GetRuntimeAssembly());
                        m_System.GetRuntimeAssembly().Add(tweClass);
                    }
                    return UnityEngine.JsonUtility.FromJson<ClassReference>(refJson);
                case ContentType.Enum:
                    for (int j = 0; j < resourceRef.files.Count; j++)
                    {
                        string tweedleCode = ReadEntry(pathBase + resourceRef.files[j]);
                        TEnumType tweedleEnum = (TEnumType)m_Parser.ParseType(tweedleCode, m_System.GetRuntimeAssembly());
                        m_System.GetRuntimeAssembly().Add(tweedleEnum);
                    }
                    return UnityEngine.JsonUtility.FromJson<EnumReference>(refJson);
                case ContentType.Image:
                    CacheSceneGraphTexture(resourceRef, pathBase);
                    return UnityEngine.JsonUtility.FromJson<ImageReference>(refJson);
                case ContentType.Model:
                    for (int j = 0; j < resourceRef.files.Count; j++)
                    {
                        string subJson = ReadEntry(resourceRef.files[j]);
                        string manifestDir = pathBase + (Path.GetDirectoryName(resourceRef.files[j]) + Path.DirectorySeparatorChar).Replace(Path.DirectorySeparatorChar, '/');
                        ParseJson(subJson, manifestDir);
                    }
                    return UnityEngine.JsonUtility.FromJson<ModelReference>(refJson);
                case ContentType.SkeletonMesh:
                    CacheSceneGraphModel(resourceRef, pathBase);
                    return UnityEngine.JsonUtility.FromJson<StructureReference>(refJson);
                case ContentType.Texture:
                    CacheSceneGraphTexture(resourceRef, pathBase);
                    return UnityEngine.JsonUtility.FromJson<TextureReference>(refJson);
            }
            return null;
        }

        private void CacheSceneGraphTexture(ResourceReference resourceRef, string pathBase) {
            if (UnityEngine.Application.isPlaying && resourceRef.files.Count > 0) {
                byte[] data = ReadDataEntry(pathBase + resourceRef.files[0]);
                var texture = new UnityEngine.Texture2D(0,0);
                if (UnityEngine.ImageConversion.LoadImage(texture, data, true)) {
                    Player.Unity.SceneGraph.Current.TextureCache.Add(resourceRef.id, texture);
                }
            }
        }

        private void CacheSceneGraphModel(ResourceReference resourceRef, string pathBase) {
            if (UnityEngine.Application.isPlaying && resourceRef.files.Count > 0) {
                byte[] data = ReadDataEntry(pathBase + resourceRef.files[0]);
                using (var assetLoader = new TriLib.AssetLoader())
                { //Initializes our Asset Loader.
                    UnityEngine.GameObject loadedModel = assetLoader.LoadFromMemory(data, resourceRef.files[0], Player.Unity.SceneGraph.Current?.InternalResources?.ModelLoaderOptions); //Loads our model.
                    Player.Unity.SceneGraph.Current.ModelCache.Add(resourceRef.id, loadedModel);
                }
            }
        }
    }
}

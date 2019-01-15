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
            ParseJson(ReadEntry("manifest.json"));
        }

        public void ParseJson(string manifestJson)
        {
            Manifest asset = UnityEngine.JsonUtility.FromJson<Manifest>(manifestJson);
            JSONObject jsonObj = new JSONObject(manifestJson);

            // TODO: Use manifest to determine player assembly version
            string playerAssembly = Player.PlayerAssemblies.CURRENT;
            m_System.AddStaticAssembly(Player.PlayerAssemblies.Assembly(playerAssembly));

            ParseResourceDetails(
                asset.resources,
                jsonObj[MemberInfoGetter.GetMemberName(() => asset.resources)]
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
                UnityEngine.Debug.Log("Did not find entry for: " + location);
            }
            return ReadDataEntry(entry);
        }

        byte[] ReadDataEntry(ZipEntry entry)
        {
            Stream entryStream = m_ZipFile.GetInputStream(entry);
            return (new BinaryReader(entryStream)).ReadBytes((int)entryStream.Length);
        }

        private void ParseResourceDetails(
            List<ResourceReference> resources,
            JSONObject json)
        {
            if (json == null || json.type != JSONObject.Type.ARRAY)
            {
                return;
            }
            for (int i = 0; i < resources.Count; i++)
            {
                ResourceReference strictResource = ReadResource(resources[i], json.list[i].ToString());
                resources[i] = strictResource;
                m_System.AddResource(strictResource);
            }
        }

        private ResourceReference ReadResource(ResourceReference resourceRef, string refJson)
        {
            switch (resourceRef.ContentType)
            {
                case ContentType.Audio:
                    return UnityEngine.JsonUtility.FromJson<AudioReference>(refJson);
                case ContentType.Class:
                    for (int j = 0; j < resourceRef.files.Count; j++)
                    {
                        string tweedleCode = ReadEntry(resourceRef.files[j]);
                        TType tweClass = (TType)m_Parser.ParseType(tweedleCode, m_System.GetRuntimeAssembly());
                        m_System.GetRuntimeAssembly().Add(tweClass);
                    }
                    return UnityEngine.JsonUtility.FromJson<ClassReference>(refJson);
                case ContentType.Enum:
                    for (int j = 0; j < resourceRef.files.Count; j++)
                    {
                        string tweedleCode = ReadEntry(resourceRef.files[j]);
                        TEnumType tweedleEnum = (TEnumType)m_Parser.ParseType(tweedleCode, m_System.GetRuntimeAssembly());
                        m_System.GetRuntimeAssembly().Add(tweedleEnum);
                    }
                    return UnityEngine.JsonUtility.FromJson<EnumReference>(refJson);
                case ContentType.Image:
                    return UnityEngine.JsonUtility.FromJson<ImageReference>(refJson);
                case ContentType.Model:
                    for (int j = 0; j < resourceRef.files.Count; j++)
                    {
                        string subJson = System.IO.File.ReadAllText(resourceRef.files[j]);
                        ParseJson(subJson);
                    }
                    return UnityEngine.JsonUtility.FromJson<ModelReference>(refJson);
                case ContentType.SkeletonMesh:
                    return UnityEngine.JsonUtility.FromJson<StructureReference>(refJson);
                case ContentType.Texture:
                    return UnityEngine.JsonUtility.FromJson<TextureReference>(refJson);
            }
            return null;
        }
    }
}

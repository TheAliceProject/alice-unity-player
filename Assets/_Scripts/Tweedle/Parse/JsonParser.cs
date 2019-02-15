using Alice.Tweedle.File;
using ICSharpCode.SharpZipLib.Zip;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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

            ParseJson(m_ZipFile.ReadEntry("manifest.json"));
        }

        public void ParseJson(string inManifestJson, string inWorkingDir = "")
        {
            Manifest asset = JsonUtility.FromJson<Manifest>(inManifestJson);
            JSONObject jsonObj = new JSONObject(inManifestJson);

            ParsePrerequisiteDetails(
                asset,
                jsonObj[MemberInfoGetter.GetMemberName(() => asset.prerequisites)],
                inWorkingDir
                );
            
            ProjectType t = asset.Identifier.Type;
            switch (t)
            {
                case ProjectType.Library:
                    LibraryManifest libAsset = new LibraryManifest(asset);
                    m_System.AddLibrary(libAsset);
                    asset = libAsset;
                    break;
                case ProjectType.World:
                    ProgramDescription worldAsset = new ProgramDescription(asset);
                    m_System.AddProgram(worldAsset);
                    asset = worldAsset;
                    break;
                case ProjectType.Model:
                    ModelManifest modelAsset = JsonUtility.FromJson<ModelManifest>(inManifestJson);
                    
                    m_System.AddModel(modelAsset);
                    asset = modelAsset;
                    break;
            }

            ParseResourceDetails(
                asset,
                jsonObj[MemberInfoGetter.GetMemberName(() => asset.resources)],
                inWorkingDir
                );
        }

        private void ParsePrerequisiteDetails(Manifest manifest, JSONObject json, string workingDir)
        {
            if (json == null || json.type != JSONObject.Type.ARRAY)
            {
                return;
            }
            for (int i = 0; i < manifest.prerequisites.Count; i++)
            {
                if  (!m_System.LoadedFiles.Contains(manifest.prerequisites[i].identifier)) {
                    string manifestPath = workingDir + manifest.prerequisites[i].manifest;
                    string manifestJson = m_ZipFile.ReadEntry(manifestPath);
                    string manifestDir = GetDirectoryEntryPath(manifestPath);
                    ParseJson(manifestJson, manifestDir);
                }
            }
        }

        private void ParseResourceDetails(Manifest manifest, JSONObject json, string workingDir)
        {
            if (json == null || json.type != JSONObject.Type.ARRAY)
            {
                return;
            }
            for (int i = 0; i < manifest.resources.Count; i++)
            {
                ResourceReference strictResource = ReadResource(manifest.resources[i], json.list[i].ToString(), manifest, workingDir);
                manifest.resources[i] = strictResource;
                m_System.AddResource(strictResource);
            }
        }

        private ResourceReference ReadResource(ResourceReference resourceRef, string refJson, Manifest manifest, string workingDir)
        {
            ResourceReference strictRef = null;

            switch (resourceRef.ContentType)
            {
                case ContentType.Audio:
                    strictRef =  UnityEngine.JsonUtility.FromJson<AudioReference>(refJson);
                    break;
                case ContentType.Class:
                    ParseTweedleTypeResource(resourceRef, workingDir);
                    strictRef =  JsonUtility.FromJson<ClassReference>(refJson);
                    break;
                case ContentType.Enum:
                    ParseTweedleTypeResource(resourceRef, workingDir);
                    strictRef =  JsonUtility.FromJson<EnumReference>(refJson);
                    break;
                case ContentType.Image:
                    if (manifest is ModelManifest) {
                        CacheToDisk(resourceRef, workingDir);
                        strictRef = resourceRef;
                    } else {
                        strictRef =  JsonUtility.FromJson<ImageReference>(refJson);
                    }
                    break;
                case ContentType.Model:
                    for (int j = 0; j < resourceRef.files.Count; j++)
                    {
                        string manifestPath = workingDir + resourceRef.files[j];
                        string manifestJson = m_ZipFile.ReadEntry(manifestPath);
                        string manifestDir = GetDirectoryEntryPath(manifestPath);
                        ParseJson(manifestJson, manifestDir);
                    }
                    strictRef =  JsonUtility.FromJson<ModelReference>(refJson);
                    break;
                case ContentType.SkeletonMesh:
                    strictRef =  JsonUtility.FromJson<StructureReference>(refJson);
                    break;
                case ContentType.Texture:
                    strictRef =  JsonUtility.FromJson<TextureReference>(refJson);
                    break;
            }

            // prepend working path to files
            for (int i = 0, fileCount = strictRef.files.Count; i < fileCount; ++i) {
                strictRef.files[i] = workingDir + strictRef.files[i];
            }

            return strictRef;
        }

        private void ParseTweedleTypeResource(ResourceReference resourceRef, string workingDir) {
            for (int j = 0; j < resourceRef.files.Count; j++)
            {
                string tweedleCode = m_ZipFile.ReadEntry(workingDir + resourceRef.files[j]);
                TType tweedleType = m_Parser.ParseType(tweedleCode, m_System.GetRuntimeAssembly());
                m_System.GetRuntimeAssembly().Add(tweedleType);
            }
        }

        private void CacheToDisk(ResourceReference resourceRef, string workingDir) {
            var cachePath = Application.temporaryCachePath + "/" + workingDir;
            if (!Directory.Exists(cachePath)) {
                Directory.CreateDirectory(cachePath);
            }

            var data = m_ZipFile.ReadDataEntry(workingDir + resourceRef.files[0]);
            System.IO.File.WriteAllBytes(cachePath + resourceRef.files[0], data);
        }

        private static string GetDirectoryEntryPath(string inFilePath) {
            return Path.GetDirectoryName(inFilePath).Replace(Path.DirectorySeparatorChar, '/') + "/";
        }
    }
}

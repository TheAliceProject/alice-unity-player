using Alice.Tweedle.File;
using Alice.Player.Unity;
using ICSharpCode.SharpZipLib.Zip;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Alice.Tweedle.Parse
{
    public class JsonParser
    {

        public static void ParseZipFile(TweedleSystem inSystem, string inZipPath) {
            using (FileStream stream = new FileStream(inZipPath, FileMode.Open, FileAccess.Read, FileShare.None)) {
                using (ZipFile zipFile = new ZipFile(stream))
                {
                    JsonParser reader = new JsonParser(inSystem, zipFile);
                    reader.Parse();
                }
            }
        }

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

        private void Parse()
        {
            // TODO: Use manifest to determine player assembly version
            string playerAssembly = Player.PlayerAssemblies.CURRENT;
            m_System.AddStaticAssembly(Player.PlayerAssemblies.Assembly(playerAssembly));

            ParseJson(m_ZipFile.ReadEntry("manifest.json"));
        }

        public Manifest ParseJson(string inManifestJson, string inWorkingDir = "")
        {
            Manifest asset = JsonUtility.FromJson<Manifest>(inManifestJson);
            JSONObject jsonObj = new JSONObject(inManifestJson);

            ParsePrerequisites(asset.prerequisites);

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

            return asset;
        }

        private void ParsePrerequisites(List<ProjectIdentifier> prerequisites)
        {
            for (int i = 0; i < prerequisites.Count; i++)
            {
                if (!m_System.LoadedFiles.Contains(prerequisites[i])) {

                    PlayerLibraryReference libRef;
                    if (PlayerLibraryManifest.Instance.TryGetLibrary(prerequisites[i], out libRef)) {
                        ParseZipFile(m_System, libRef.path.fullPath);
                    } else {
                        throw new TweedleParseException("Could not find prerequisite " + prerequisites[i].name);
                    }
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
            string zipPath = workingDir + resourceRef.file;

            switch (resourceRef.ContentType)
            {
                case ContentType.Audio:
                    strictRef = UnityEngine.JsonUtility.FromJson<AudioReference>(refJson);
                    break;
                case ContentType.Class:
                    ParseTweedleTypeResource(resourceRef, workingDir);
                    strictRef = JsonUtility.FromJson<ClassReference>(refJson);
                    break;
                case ContentType.Enum:
                    ParseTweedleTypeResource(resourceRef, workingDir);
                    strictRef = JsonUtility.FromJson<EnumReference>(refJson);
                    break;
                case ContentType.Image:
                    if (manifest is ModelManifest) {
                        CacheToDisk(resourceRef, workingDir);
                        resourceRef.name = manifest.description.name + "/" + resourceRef.name;
                        strictRef = resourceRef;
                    } else {
                        LoadTexture(resourceRef, workingDir);
                        strictRef = JsonUtility.FromJson<ImageReference>(refJson);
                    }
                    break;
                case ContentType.Model:
                    string manifestJson = m_ZipFile.ReadEntry(zipPath);
                    string manifestDir = GetDirectoryEntryPath(zipPath);
                    var modelManifest = (ModelManifest)ParseJson(manifestJson, manifestDir);
                    LoadModelStructures(modelManifest);
                    strictRef = JsonUtility.FromJson<ModelReference>(refJson);
                    break;
                case ContentType.SkeletonMesh:
                    strictRef = JsonUtility.FromJson<StructureReference>(refJson);
                    break;
                case ContentType.Texture:
                    strictRef = JsonUtility.FromJson<TextureReference>(refJson);
                    break;
            }

            // prepend working path to resource file
            strictRef.file = zipPath;

            return strictRef;
        }

        private void ParseTweedleTypeResource(ResourceReference resourceRef, string workingDir) {
            string tweedleCode = m_ZipFile.ReadEntry(workingDir + resourceRef.file);
            TType tweedleType = m_Parser.ParseType(tweedleCode, m_System.GetRuntimeAssembly());
            m_System.GetRuntimeAssembly().Add(tweedleType);
        }

        private void CacheToDisk(ResourceReference resourceRef, string workingDir) {
            var cachePath = Application.temporaryCachePath + "/" + workingDir;
            if (!Directory.Exists(cachePath)) {
                Directory.CreateDirectory(cachePath);
            }

            var data = m_ZipFile.ReadDataEntry(workingDir + resourceRef.file);
            System.IO.File.WriteAllBytes(cachePath + resourceRef.file, data);
        }

        private void LoadTexture(ResourceReference resourceRef, string workingDir) {
            if (Application.isPlaying) {
                byte[] data = m_ZipFile.ReadDataEntry(workingDir + resourceRef.file);
                var texture = new Texture2D(0, 0);
                if (ImageConversion.LoadImage(texture, data, true)) {
                    SceneGraph.Current.TextureCache.Add(resourceRef.name, texture);
                }
            }
        }

        private void LoadModelStructures(ModelManifest inManifest) {
            if (Application.isPlaying) {

                for (int i = 0; i < inManifest.models.Count; ++i) {
                    ResourceReference meshRef = null;

                    // find struct resource
                    for (int j = 0; j < inManifest.resources.Count; ++j) {
                        if (inManifest.resources[j].name == inManifest.models[i].structure) {
                            meshRef = inManifest.resources[j];
                            break;
                        }
                    }

                    if (meshRef != null) {
                        byte[] data = m_ZipFile.ReadDataEntry(meshRef.file);

                        using (var assetLoader = new TriLib.AssetLoader())
                        {
                            var options = SceneGraph.Current?.InternalResources?.ModelLoaderOptions;
                            var cachePath = Application.temporaryCachePath + "/" + meshRef.file;
                            options.TexturesPathOverride = System.IO.Path.GetDirectoryName(cachePath);

                            GameObject loadedModel = assetLoader.LoadFromMemory(data, meshRef.file, options);

                            var cacheID = inManifest.description.name + "/" + inManifest.models[i].name;
                            SceneGraph.Current.ModelCache.Add(cacheID, loadedModel);
                        }
                    }
                }
            }
        }

        private static string GetDirectoryEntryPath(string inFilePath) {
            return Path.GetDirectoryName(inFilePath).Replace(Path.DirectorySeparatorChar, '/') + "/";
        }
    }
}

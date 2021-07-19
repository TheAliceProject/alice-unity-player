using System;
using Alice.Tweedle.File;
using Alice.Player.Unity;
using ICSharpCode.SharpZipLib.Zip;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text;
using NLayer;
using Siccity.GLTFUtility;

namespace Alice.Tweedle.Parse
{
    public class JsonParser
    {
        public static Stream libraryStream;
        public static void SetLibraryStream(Stream stream)
        {
            libraryStream = stream;
        }

        public static void ParseZipFile(TweedleSystem inSystem, string inZipPath) {
            using (FileStream stream = new FileStream(inZipPath, FileMode.Open, FileAccess.Read, FileShare.None)) {
                using (ZipFile zipFile = new ZipFile(stream))
                {
                    JsonParser reader = new JsonParser(inSystem, zipFile);
                    if(!inZipPath.Contains(WorldObjects.SCENE_GRAPH_LIBRARY_NAME))
                        reader.CacheThumbnail(inZipPath);
                    reader.Parse();
                }
            }
        }

        private TweedleSystem m_System;
        private TweedleParser m_Parser;
        private ZipFile m_ZipFile;
        public static int audioFiles = 0;

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

        public void CacheThumbnail(string fileName)
        {
            // Save thumbnail
            byte[] data = m_ZipFile.ReadDataEntry("thumbnail.png");
            if(data == null)
                return;
            System.IO.File.WriteAllBytes(Application.persistentDataPath + "/" + Path.GetFileNameWithoutExtension(fileName) + "_thumb.png", data);
        }

        private void Parse()
        {
            // TODO: Use manifest to determine player assembly version
            string playerAssembly = Player.PlayerAssemblies.CURRENT;
            m_System.AddStaticAssembly(Player.PlayerAssemblies.Assembly(playerAssembly));

            ParseJson(m_ZipFile.ReadEntry("manifest.json"));
        }

        public static void ParseZipFile(TweedleSystem inSystem, Stream inZipStream)
        {
            using (ZipFile zipFile = new ZipFile(inZipStream))
            {
                JsonParser reader = new JsonParser(inSystem, zipFile);
                reader.Parse();
            }
        }

        public Manifest ParseJson(string inManifestJson, string inWorkingDir = "")
        {
            Manifest asset = JsonUtility.FromJson<Manifest>(inManifestJson);
            JSONObject jsonObj = new JSONObject(inManifestJson);

            ParsePrerequisites(asset);

            ProjectType t = asset.Identifier.Type;
            switch (t)
            {
                case ProjectType.Library:
                    LibraryManifest libAsset = new LibraryManifest(asset);
                    m_System.AddLibrary(libAsset);
                    asset = libAsset;
                    Debug.Log("Loaded Library " + asset.Identifier.name + " version " + asset.Identifier.version);
                    break;
                case ProjectType.World:
                    ProgramDescription worldAsset = new ProgramDescription(asset);
                    m_System.AddProgram(worldAsset);
                    asset = worldAsset;
                    Debug.Log("Loaded Project produced in Alice version " + asset.provenance.aliceVersion);
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

        private void ParsePrerequisites(Manifest manifest) {
            var prerequisites = manifest.prerequisites;
            foreach (var t in prerequisites) {
                if (!m_System.LoadedFiles.Contains(t)) {

                    PlayerLibraryReference libRef;
                    if (PlayerLibraryManifest.Instance.TryGetLibrary(t, out libRef)) {
#if UNITY_ANDROID || UNITY_IOS || UNITY_WEBGL
                        ParseZipFile(m_System, libraryStream);                     
#else
                        ParseZipFile(m_System, libRef.path.fullPath);
#endif
                    } else {
                        throw new TweedleVersionException("Could not find prerequisite " + t.name,
                            WorldObjects.SCENE_GRAPH_LIBRARY_NAME + " " + PlayerLibraryManifest.Instance.GetLibraryVersion(),
                            WorldObjects.SCENE_GRAPH_LIBRARY_NAME + " " + t.version,
                            PlayerLibraryManifest.Instance.aliceVersion,
                            manifest.provenance.aliceVersion);
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
                    LoadAudio(resourceRef, workingDir);
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
            try
            {
                var tweedleCode = m_ZipFile.ReadEntry(workingDir + resourceRef.file);
                var tweedleType = m_Parser.ParseType(tweedleCode, m_System.GetRuntimeAssembly());
                m_System.GetRuntimeAssembly().Add(tweedleType);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw new TweedleParseException("Unable to read " + resourceRef.file, e);
            }
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

        private void LoadAudio(ResourceReference resourceRef, string workingDir){
            // Save file as either wav or mp3 depending on type.
            // If mp3 and we're on desktop, must convert to wav first using NAudio
            // Then load audioclip with unitywebrequest

            if (Application.isPlaying)
            {
                byte[] data = m_ZipFile.ReadDataEntry(workingDir + resourceRef.file);
                
                AudioClip audioClip = null;
                string fileSuffix = resourceRef.file.Substring(resourceRef.file.Length - 4).ToLower();
                if(fileSuffix == ".wav"){
                    string waveTest = Encoding.ASCII.GetString(data, 8, 4);
                    if(waveTest != "WAVE")
                        Debug.LogError("Detected wav file but header incorrect.");
                    audioClip = WavUtility.ToAudioClip(data);
                }
                else if(fileSuffix == ".mp3"){
                    // Hopefully an mp3 file (maybe in the future check some bytes? Probably unnecessary though)

                    // This is a bit silly, but it seems like you must save the file as an mp3, then load it in.
                    // I have tried to convert the mp3 byte array to a wav byte array without much luck.
                    string tempFile = Application.persistentDataPath + "/tempAudio" + audioFiles++.ToString() + ".mp3";
                    System.IO.File.WriteAllBytes(tempFile, data);
                    audioClip = LoadMp3(tempFile);
                    // These temporary files will get deleted when the program is started and/or stopped
                }
                else{
                    Debug.LogError(fileSuffix + " files are not supported at this time.");
                }
                if(audioClip != null)
                    SceneGraph.Current.AudioCache.Add(resourceRef.name, audioClip);
            }
        }

        public static AudioClip LoadMp3(string filePath) {
            string filename = System.IO.Path.GetFileNameWithoutExtension(filePath);

            MpegFile mpegFile = new MpegFile(filePath);

            // assign samples into AudioClip
            AudioClip ac = AudioClip.Create(filename,
                                            (int)(mpegFile.Length / sizeof(float) / mpegFile.Channels),
                                            mpegFile.Channels,
                                            mpegFile.SampleRate,
                                            true,
                                            data => { int actualReadCount = mpegFile.ReadSamples(data, 0, data.Length); },
                                            position => { mpegFile = new MpegFile(filePath); });
            return ac;
        }

        private void LoadModelStructures(ModelManifest inManifest) {
            if (!Application.isPlaying) return;

            foreach (var model in inManifest.models) {
                var meshRef = inManifest.GetStructure(model.structure);
                if (meshRef == null) continue;

                var data = m_ZipFile.ReadDataEntry(meshRef.file);
                var options = SceneGraph.Current?.InternalResources?.ModelLoaderOptions;
                
                //TODO: Cache data
                //var cachePath = Application.temporaryCachePath + "/" + meshRef.file;
                
                var loadedModel = Importer.LoadFromBytes(data, options);

                var cacheId = inManifest.description.name + "/" + model.name;

                var meshBounds = meshRef.boundingBox.AsBounds();
                var bounds = meshBounds.min.Equals(Vector3.zero) && meshBounds.max.Equals(Vector3.zero) ?
                    inManifest.boundingBox.AsBounds() : meshBounds;
                SceneGraph.Current.ModelCache.Add(cacheId, loadedModel, bounds, inManifest.jointBounds);
            }
        }

        private static string GetDirectoryEntryPath(string inFilePath) {
            return Path.GetDirectoryName(inFilePath).Replace(Path.DirectorySeparatorChar, '/') + "/";
        }
    }
}

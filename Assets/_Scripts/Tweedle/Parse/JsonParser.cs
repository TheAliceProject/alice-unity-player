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
using System.Collections;

namespace Alice.Tweedle.Parse
{
    public class ModelManifestHolder
    {
        public ModelManifest data;
    }

    public class JsonParser
    {
        Manifest manifest;
        ResourceReference strictRef;

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

        public static IEnumerator ParseZipFile(TweedleSystem inSystem, Stream inZipStream)
        {
            using (ZipFile zipFile = new ZipFile(inZipStream))
            {
                JsonParser reader = new JsonParser(inSystem, zipFile);
                yield return reader.Parse();
            }
        }

        private IEnumerator Parse()
        {
            // TODO: Use manifest to determine player assembly version
            string playerAssembly = Player.PlayerAssemblies.CURRENT;
            m_System.AddStaticAssembly(Player.PlayerAssemblies.Assembly(playerAssembly));

            yield return ParseJson(m_ZipFile.ReadEntry("manifest.json"));
        }

        public IEnumerator ParseJson(string inManifestJson, ModelManifestHolder modelHolder = null, string inWorkingDir = "")
        {
            manifest = JsonUtility.FromJson<Manifest>(inManifestJson);
            JSONObject jsonObj = new JSONObject(inManifestJson);

            yield return ParsePrerequisites(manifest);

            ProjectType t = manifest.Identifier.Type;
            switch (t)
            {
                case ProjectType.Library:
                    LibraryManifest libAsset = new LibraryManifest(manifest);
                    m_System.AddLibrary(libAsset);
                    manifest = libAsset;
                    Debug.Log("Loaded Library " + manifest.Identifier.name + " version " + manifest.Identifier.version);
                    break;
                case ProjectType.World:
                    ProgramDescription worldAsset = new ProgramDescription(manifest);
                    m_System.AddProgram(worldAsset);
                    manifest = worldAsset;
                    Debug.Log("Loaded Project produced in Alice version " + manifest.provenance.aliceVersion);
                    break;
                case ProjectType.Model:
                    ModelManifest modelAsset = JsonUtility.FromJson<ModelManifest>(inManifestJson);
                    m_System.AddModel(modelAsset);
                    manifest = modelAsset;
                    modelHolder.data = modelAsset;    
                    break;
            }

            yield return ParseResourceDetails(
                manifest,
                jsonObj[MemberInfoGetter.GetMemberName(() => manifest.resources)],
                inWorkingDir
                );
        }

        private IEnumerator ParsePrerequisites(Manifest manifest) 
        {
            var prerequisites = manifest.prerequisites;
            foreach (var t in prerequisites)
            {
                if (!m_System.LoadedFiles.Contains(t))
                {
                    PlayerLibraryReference libRef;
                    if (PlayerLibraryManifest.Instance.TryGetLibrary(t, out libRef))
                    {
                        yield return ParseZipFile(m_System, libraryStream);
                    }
                    else
                    {
                        throw new TweedleVersionException("Could not find prerequisite " + t.name,
                            WorldObjects.SCENE_GRAPH_LIBRARY_NAME + " " + PlayerLibraryManifest.Instance.GetLibraryVersion(),
                            WorldObjects.SCENE_GRAPH_LIBRARY_NAME + " " + t.version,
                            PlayerLibraryManifest.Instance.aliceVersion,
                            manifest.provenance.aliceVersion);
                    }
                }
            }
        }

        private IEnumerator ParseResourceDetails(Manifest manifest, JSONObject json, string workingDir)
        {
            if (json == null || json.type != JSONObject.Type.ARRAY)
            {
                yield break;
            }

            for (int i = 0; i < manifest.resources.Count; i++)
            {
                yield return ReadResource(manifest.resources[i], json.list[i].ToString(), manifest, workingDir);
                manifest.resources[i] = strictRef;
                m_System.AddResource(strictRef);
            }
        }

        private IEnumerator ReadResource(ResourceReference resourceRef, string refJson, Manifest manifest, string workingDir)
        {
            Debug.LogFormat("Reading resource {0} {1}", resourceRef.file, resourceRef.name);

            string zipPath = workingDir + resourceRef.file;

            switch (resourceRef.ContentType)
            {
                case ContentType.Audio:
                    strictRef = JsonUtility.FromJson<AudioReference>(refJson);
                    yield return LoadAudio(resourceRef, workingDir);
                    break;
                case ContentType.Class:
                    yield return ParseTweedleTypeResource(resourceRef, workingDir);
                    strictRef = JsonUtility.FromJson<ClassReference>(refJson);
                    break;
                case ContentType.Enum:
                    yield return ParseTweedleTypeResource(resourceRef, workingDir);
                    strictRef = JsonUtility.FromJson<EnumReference>(refJson);
                    break;
                case ContentType.Image:
                    if (manifest is ModelManifest) {
                        yield return CacheToDisk(resourceRef, workingDir);
                        resourceRef.name = manifest.description.name + "/" + resourceRef.name;
                        strictRef = resourceRef;
                    } else {
                        yield return LoadTexture(resourceRef, workingDir);
                        strictRef = JsonUtility.FromJson<ImageReference>(refJson);
                    }
                    break;
                case ContentType.Model:
                    string manifestJson = m_ZipFile.ReadEntry(zipPath);
                    string manifestDir = GetDirectoryEntryPath(zipPath);

                    ModelManifestHolder modelHolder = new ModelManifestHolder();
                    yield return ParseJson(manifestJson, modelHolder, manifestDir);
                    ModelManifest modelManifest = modelHolder.data;
                    yield return LoadModelStructures(modelManifest);
                    
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
        }

        private IEnumerator ParseTweedleTypeResource(ResourceReference resourceRef, string workingDir) {
            yield return null;

            try
            {
                using TextReader tweedleStream = new StreamReader(m_ZipFile.OpenEntryStream(workingDir + resourceRef.file));
                var tweedleType = m_Parser.ParseType(tweedleStream, m_System.GetRuntimeAssembly(), resourceRef.file);
                m_System.GetRuntimeAssembly().Add(tweedleType);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw new TweedleParseException("Unable to read " + resourceRef.file, e);
            }
        }

        private IEnumerator CacheToDisk(ResourceReference resourceRef, string workingDir) {
            yield return null;
            var cachePath = Application.temporaryCachePath + "/" + workingDir;
            if (!Directory.Exists(cachePath)) {
                Directory.CreateDirectory(cachePath);
            }

            var data = m_ZipFile.ReadDataEntry(workingDir + resourceRef.file);
            System.IO.File.WriteAllBytes(cachePath + resourceRef.file, data);
        }

        private IEnumerator LoadTexture(ResourceReference resourceRef, string workingDir) {
            yield return null;
            if (Application.isPlaying) {
                byte[] data = m_ZipFile.ReadDataEntry(workingDir + resourceRef.file);
                var texture = new Texture2D(0, 0);
                if (ImageConversion.LoadImage(texture, data, true)) {
                    SceneGraph.Current.TextureCache.Add(resourceRef.name, texture);
                }
            }
        }

        private IEnumerator LoadAudio(ResourceReference resourceRef, string workingDir){
            // Save file as either wav or mp3 depending on type.
            // If mp3 and we're on desktop, must convert to wav first using NAudio
            // Then load audioclip with unitywebrequest

            yield return null;

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

        private IEnumerator LoadModelStructures(ModelManifest inManifest) {
            if (!Application.isPlaying) yield break;

            foreach (var model in inManifest.models) {
                yield return null;
                var meshRef = inManifest.GetStructure(model.structure);
                if (meshRef == null) continue;

                yield return null;
                var data = m_ZipFile.ReadDataEntry(meshRef.file);
                var options = SceneGraph.Current?.InternalResources?.ModelLoaderOptions;
                
                //TODO: Cache data
                //var cachePath = Application.temporaryCachePath + "/" + meshRef.file;
                GameObject loadedModel = null;
                
                Importer.ImportGLBAsync(data, options, (GameObject go, AnimationClip[] anims) => {
                    loadedModel = go;
                });

                while (loadedModel == null) {
                    yield return null;
                }

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

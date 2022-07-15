using System;
using Alice.Tweedle.File;
using Alice.Player.Unity;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using UnityEngine;
using System.Text;
using NLayer;
using Siccity.GLTFUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Vector3 = UnityEngine.Vector3;

namespace Alice.Tweedle.Parse
{

    public class JsonParser
    {
        private readonly TweedleSystem m_System;
        private readonly TweedleParser m_Parser;
        private ZipFile m_ZipFile;
        private Stream m_FileStream;
        private readonly string m_FileName;
        private readonly ExceptionHandler m_ExceptionHandler;
        private readonly Dictionary<ProjectIdentifier, TweedleSystem> m_LibraryCache;

        public delegate void ExceptionHandler(Exception e);

        public JsonParser(TweedleSystem inSystem) :
            this(inSystem, "", new Dictionary<ProjectIdentifier, TweedleSystem>(), null)
        {}

        private JsonParser(TweedleSystem inSystem, string fileName,
            Dictionary<ProjectIdentifier, TweedleSystem> libraryCache,
            ExceptionHandler exceptionHandler)
        {
            m_System = inSystem;
            m_FileName = fileName;
            m_LibraryCache = libraryCache;
            m_Parser = new TweedleParser();
            m_ExceptionHandler = exceptionHandler;
        }

        private void CacheThumbnail(string fileName)
        {
#if !UNITY_WEBGL
            // Save thumbnail
            byte[] data = m_ZipFile.ReadDataEntry("thumbnail.png");
            if(data == null)
                return;
            System.IO.File.WriteAllBytes(Application.persistentDataPath + "/" + Path.GetFileNameWithoutExtension(fileName) + "_thumb.png", data);
#endif
        }

        public static IEnumerator Parse(TweedleSystem inSystem, string fileName,
            Dictionary<ProjectIdentifier, TweedleSystem> libraryCache,
            ExceptionHandler exceptionHandler)
        {
            var reader = new JsonParser(inSystem, fileName, libraryCache, exceptionHandler);
            yield return reader.Parse();
        }

        private IEnumerator LoadFile(string fileName) {
            if (fileName.StartsWith("jar:") || fileName.StartsWith("http:")) {
                // Use UnityWebRequest when reading from a compressed file
                yield return LoadFileWR(fileName);
            } else {
                yield return LoadFileFS(fileName);
            }
        }

        private IEnumerator LoadFileFS(string fileName) {
            if (!System.IO.File.Exists(fileName)) {
                HandleException(new FileNotFoundException(fileName));
            }
            m_FileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
            yield return null;
        }

        private IEnumerator LoadFileWR(string fileName) {
            var www = new UnityWebRequest(fileName) {downloadHandler = new DownloadHandlerBuffer()};
            yield return www.SendWebRequest();
            m_FileStream = new MemoryStream(www.downloadHandler.data);
        }

        private IEnumerator Parse()
        {
            yield return LoadFile(m_FileName);

            using (m_FileStream) {
                try {
                    m_ZipFile = new ZipFile(m_FileStream);
                } catch(Exception e) {
                    HandleException(e);
                }

                using (m_ZipFile) {
                    if(!m_FileName.Contains(WorldObjects.SceneGraphLibraryName))
                        CacheThumbnail(m_FileName);

                    // TODO: Use manifest to determine player assembly version
                    string playerAssembly = Player.PlayerAssemblies.CURRENT;
                    m_System.AddStaticAssembly(Player.PlayerAssemblies.Assembly(playerAssembly));

                    var manifestJson = m_ZipFile.ReadEntry("manifest.json");

                    var rootManifest = ParseProjectManifest(manifestJson);
                    yield return ParsePrerequisites(rootManifest);
                    LoadProject(rootManifest);
                }
            }
        }

        public void LoadStandAloneProject(string manifestJson) {
            LoadProject(ParseProjectManifest(manifestJson));
        }

        #region Manifest Parsing

        private static Manifest ParseProjectManifest(string manifestJson, string workingDirectory = "") {
            var manifest = SpecializeProjectManifest(manifestJson);
            var jsonObj = new JSONObject(manifestJson);
            SpecializeResourceManifests(manifest, jsonObj[MemberInfoGetter.GetMemberName(() => manifest.resources)], workingDirectory);
            return manifest;
        }

        private static Manifest SpecializeProjectManifest(string manifestJson) {
            var manifest = JsonUtility.FromJson<Manifest>(manifestJson);
            return manifest.Identifier.Type switch {
                ProjectType.Library => new LibraryManifest(manifest),
                ProjectType.World => new ProgramDescription(manifest),
                ProjectType.Model => JsonUtility.FromJson<ModelManifest>(manifestJson),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static void SpecializeResourceManifests(Manifest projectManifest, JSONObject rawResources, string workingDir) {
            if (!(rawResources is { type: JSONObject.Type.ARRAY })) return;

            for (var i = 0; i < projectManifest.resources.Count; i++) {
                var genericRef = projectManifest.resources[i];
                var strictRef =
                    SpecializeResourceManifest(genericRef, rawResources.list[i].ToString(), projectManifest);
                strictRef.file = workingDir + genericRef.file;
                projectManifest.resources[i] = strictRef;
            }
        }

        private static ResourceReference SpecializeResourceManifest(ResourceReference resourceRef, string refJson, Manifest parent) {
            return resourceRef.ContentType switch {
                ContentType.Audio => JsonUtility.FromJson<AudioReference>(refJson),
                ContentType.Class => JsonUtility.FromJson<ClassReference>(refJson),
                ContentType.Enum => JsonUtility.FromJson<EnumReference>(refJson),
                ContentType.Image => ImageReference(resourceRef, refJson, parent),
                ContentType.Model => JsonUtility.FromJson<ModelReference>(refJson),
                ContentType.SkeletonMesh => JsonUtility.FromJson<StructureReference>(refJson),
                ContentType.Texture => JsonUtility.FromJson<TextureReference>(refJson),
                ContentType.NULL => resourceRef,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static ResourceReference ImageReference(ResourceReference resourceRef, string refJson, Manifest parent) {
            if (parent is ModelManifest) {
                resourceRef.name = parent.description.name + "/" + resourceRef.name;
                return resourceRef;
            }
            return JsonUtility.FromJson<ImageReference>(refJson);
        }

        #endregion Manifest Parsing

        private void LoadProject(Manifest manifest, string workingDirectory = "") {
            manifest.AddToSystem(m_System);
            foreach (var resource in manifest.resources) {
                resource.LoadContent(this, workingDirectory);
                m_System.AddResource(resource);
            }
        }

        public static IEnumerator CacheLibrary(Dictionary<ProjectIdentifier, TweedleSystem> libraryCache) {
            var lib = PlayerLibraryManifest.Instance.GetLibraryReference();
            if (lib == null) yield break;
            
            var system = new TweedleSystem();
            yield return Parse(system, lib.Value.path.fullPath, libraryCache, null);
            libraryCache.Add(lib.Value.identifier, system);
        }

        private IEnumerator ParsePrerequisites(Manifest manifest)
        {
            var prerequisites = manifest.prerequisites;
            foreach (var t in prerequisites) {
                if (m_LibraryCache.TryGetValue(t, out var cachedLibrary)) {
                    m_System.SetLibraryAssembly(cachedLibrary.GetRuntimeAssembly());
                } else {
                    if (m_System.LoadedFiles.Contains(t)) continue;
                    var libraryMatch = PlayerLibraryManifest.Instance.TryGetLibrary(t, out var libRef);
                    if (libraryMatch == 0) {
                        var library = new TweedleSystem();
                        // TODO skip immediate full parsing in favor of lazy reading
                        yield return Parse(library, libRef.path.fullPath, m_LibraryCache, m_ExceptionHandler);
                        m_LibraryCache.Add(t, library);
                        m_System.SetLibraryAssembly(library.GetRuntimeAssembly());
                    }
                    else {
                        HandleException(new TweedleVersionException("Could not find prerequisite " + t.name,
                            WorldObjects.SceneGraphLibraryName + " " + PlayerLibraryManifest.Instance.GetLibraryVersion(),
                            WorldObjects.SceneGraphLibraryName + " " + t.version,
                            PlayerLibraryManifest.Instance.aliceVersion,
                            manifest.provenance.aliceVersion,
                            libraryMatch));
                    }
                }
            }
        }

        private void HandleException(Exception e) {
            m_FileStream?.Close();
            m_ExceptionHandler(e);
            throw e;
        }

        public void ParseTweedleTypeResource(ResourceReference resourceRef, string workingDir) {
            try
            {
                using TextReader tweedleStream = new StreamReader(m_ZipFile.OpenEntryStream(workingDir + resourceRef.file));
                var tweedleType = m_Parser.ParseType(tweedleStream, m_System.GetRuntimeAssembly(), resourceRef.file);
                m_System.GetRuntimeAssembly().Add(tweedleType);
            }
            catch (Exception e)
            {
                HandleException(new TweedleParseException("Unable to read " + resourceRef.file, e));
            }
        }

        public void LoadTexture(ImageReference resourceRef, string workingDir) {
            if (!Application.isPlaying) return;

            byte[] data = m_ZipFile.ReadDataEntry(workingDir + resourceRef.file);
            var texture = new Texture2D(0, 0);
            if (ImageConversion.LoadImage(texture, data, true)) {
                m_System.CacheTexture(resourceRef.name, texture);
            }
        }

        internal void LoadAudio(AudioReference resourceRef, string workingDir){
            if (!Application.isPlaying) return;
            
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
                audioClip = LoadMp3(new MemoryStream(data), resourceRef.file);
            }
            else{
                Debug.LogError(fileSuffix + " files are not supported at this time.");
            }
            if(audioClip != null)
                SceneGraph.Current.AudioCache.Add(resourceRef.name, audioClip);
        }

        private static AudioClip LoadMp3(Stream stream, string filePath) {
            string filename = Path.GetFileNameWithoutExtension(filePath);

            MpegFile mpegFile = new MpegFile(stream);
            // assign samples into AudioClip
            AudioClip ac = AudioClip.Create(filename,
                                            (int)(mpegFile.Length / sizeof(float) / mpegFile.Channels),
                                            mpegFile.Channels,
                                            mpegFile.SampleRate,
                                            false,
                                            data => { int actualReadCount = mpegFile.ReadSamples(data, 0, data.Length); });
            return ac;
        }

        public void LoadModel(ModelReference modelReference, string workingDir) {
            var zipPath = workingDir + modelReference.file;
            var manifestJson = m_ZipFile.ReadEntry(zipPath);
            var manifestDir = GetDirectoryEntryPath(zipPath);

            var modelManifest = (ModelManifest) ParseProjectManifest(manifestJson, manifestDir);
            LoadProject(modelManifest, manifestDir);
            LoadModelStructures(modelManifest);
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
                GameObject loadedModel = Importer.LoadFromBytes(data, options);

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

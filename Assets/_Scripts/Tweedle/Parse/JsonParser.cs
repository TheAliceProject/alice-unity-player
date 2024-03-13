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
using Alice.Storage;
using Vector3 = UnityEngine.Vector3;

namespace Alice.Tweedle.Parse
{

    public class JsonParser
    {
        private readonly TweedleSystem m_System;
        private readonly TweedleParser m_Parser;
        private ZipFile m_ZipFile;
        private readonly string m_FileName;
        private readonly ExceptionHandler m_ExceptionHandler;
        private readonly LibraryCache m_LibraryCache;

        public delegate void ExceptionHandler(Exception e);

        public JsonParser(TweedleSystem inSystem) :
            this(inSystem, "", new LibraryCache(), null)
        {}

        private JsonParser(TweedleSystem inSystem, string fileName,
            LibraryCache libraryCache,
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
            LibraryCache libraryCache,
            ExceptionHandler exceptionHandler)
        {
            var reader = new JsonParser(inSystem, fileName, libraryCache, exceptionHandler);
            yield return reader.Parse();
        }
        
        private IEnumerator Parse() {
            yield return StorageReader.Read(m_FileName, stream => {
                try {
                    m_ZipFile = new ZipFile(stream);
                }
                catch (Exception e) {
                    HandleException(e);
                }
            }, HandleException);

            using (m_ZipFile) {
                if (!m_FileName.Contains(WorldObjects.SceneGraphLibraryName))
                    CacheThumbnail(m_FileName);

                // TODO: Use manifest to determine player assembly version
                string playerAssembly = Player.PlayerAssemblies.CURRENT;
                m_System.AddStaticAssembly(Player.PlayerAssemblies.Assembly(playerAssembly));

                var manifestJson = m_ZipFile.ReadEntry("manifest.json");

                var rootManifest = ParseProjectManifest(manifestJson);
                yield return ParsePrerequisites(rootManifest);
                yield return LoadProject(rootManifest);
            }
        }

        public IEnumerator LoadStandAloneProject(string manifestJson) {
            yield return LoadProject(ParseProjectManifest(manifestJson));
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

        private IEnumerator LoadProject(Manifest manifest, string workingDirectory = "") {
            manifest.AddToSystem(m_System);
            foreach (var resource in manifest.resources) {
                yield return resource.LoadContent(this, workingDirectory);
                m_System.AddResource(resource);
            }
        }

        public static IEnumerator CacheLibrary(LibraryCache libraryCache) {
            var lib = PlayerLibraryManifest.Instance.GetLibraryReference();
            if (lib == null) yield break;
            yield return libraryCache.Read(lib.Value.identifier, lib.Value.path.fullPath, null);
        }

        private IEnumerator ParsePrerequisites(Manifest manifest)
        {
            var prerequisites = manifest.prerequisites;
            foreach (var t in prerequisites) {
                if (m_System.LoadedFiles.Contains(t)) continue;
                
                if (m_LibraryCache.TryGetValue(t, out var cachedLibrary)) {
                    m_System.SetLibraryAssembly(cachedLibrary.GetRuntimeAssembly());
                } else {
                    var libraryMatch = PlayerLibraryManifest.Instance.TryGetLibrary(t, out var libRef);
                    if (libraryMatch == 0) {
                        yield return m_LibraryCache.Read(t, libRef.path.fullPath, m_ExceptionHandler);
                        m_System.SetLibraryAssembly(m_LibraryCache.GetRuntimeAssembly(t));
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
            m_ZipFile?.Close();
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

        internal IEnumerator LoadAudio(AudioReference resourceRef, string workingDir) {
            if (!Application.isPlaying) yield break;
            byte[] data = m_ZipFile.ReadDataEntry(workingDir + resourceRef.file);
            AudioClip audioClip = null;
            string fileSuffix = resourceRef.file.Substring(resourceRef.file.Length - 4).ToLower();
            yield return null;
            try {
                switch (fileSuffix) {
                    case ".wav": {
                        var waveTest = Encoding.ASCII.GetString(data, 8, 4);
                        if (waveTest != "WAVE")
                            Debug.LogError("Detected wav file but header incorrect.");
                        audioClip = WavUtility.ToAudioClip(data);
                        break;
                    }
                    case ".mp3":
                        audioClip = LoadMp3(new MemoryStream(data), resourceRef.file);
                        break;
                    default:
                        Debug.LogError(fileSuffix + " files are not supported at this time.");
                        break;
                }
            } catch (Exception e) {
                Debug.LogError($"Problem reading the audio file: {resourceRef.file}\nResuming without it.\n{e}");
            }

            if(audioClip != null)
                SceneGraph.Current.AudioCache.Add(resourceRef.name, audioClip);
            yield return null;
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

        public IEnumerator LoadModel(ModelReference modelReference, string workingDir) {
            var zipPath = workingDir + modelReference.file;
            var manifestJson = m_ZipFile.ReadEntry(zipPath);
            var manifestDir = GetDirectoryEntryPath(zipPath);

            var modelManifest = (ModelManifest) ParseProjectManifest(manifestJson, manifestDir);
            yield return LoadProject(modelManifest, manifestDir);
            yield return LoadModelStructures(modelManifest);
        }

        private IEnumerator LoadModelStructures(ModelManifest inManifest) {
            if (!Application.isPlaying) yield break;

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
                yield return null;
            }
        }

        private static string GetDirectoryEntryPath(string inFilePath) {
            return Path.GetDirectoryName(inFilePath).Replace(Path.DirectorySeparatorChar, '/') + "/";
        }
    }
}

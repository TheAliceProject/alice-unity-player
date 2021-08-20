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
using UnityEngine.Networking;

namespace Alice.Tweedle.Parse
{

    public class JsonParser
    {
        Manifest manifest;
        ResourceReference strictRef;
        private TweedleSystem m_System;
        private TweedleParser m_Parser;
        private ZipFile m_ZipFile;
        public static int audioFiles = 0;
        private Stream m_FileStream;
        private string m_FileName;
        private ExceptionHandler m_ExceptionHandler;

        public delegate void ExceptionHandler(Exception e);


        public class ModelManifestHolder
        {
            public ModelManifest data;
        }

        public TweedleSystem StoredSystem
        {
            get { return m_System; }
        }

        public JsonParser(TweedleSystem inSystem, string fileName, ExceptionHandler exceptionHandler)
        {
            m_System = inSystem;
            m_FileName = fileName;
            m_Parser = new TweedleParser();
            m_ExceptionHandler = exceptionHandler;
        }

        public void CacheThumbnail(string fileName)
        {
#if !UNITY_WEBGL
            // Save thumbnail
            byte[] data = m_ZipFile.ReadDataEntry("thumbnail.png");
            if(data == null)
                return;
            System.IO.File.WriteAllBytes(Application.persistentDataPath + "/" + Path.GetFileNameWithoutExtension(fileName) + "_thumb.png", data);
#endif
        }

        public static IEnumerator Parse(TweedleSystem inSystem, string path, ExceptionHandler exceptionHandler)
        {
            JsonParser reader = new JsonParser(inSystem, path, exceptionHandler);
            yield return reader.Parse();
        }

        private IEnumerator LoadFile(string fileName) 
        {
            #if UNITY_ANDROID || UNITY_IOS || UNITY_WEBGL
                yield return LoadFileWR(fileName);
            #else
                yield return LoadFileFS(fileName);
            #endif
        }

        private IEnumerator LoadFileFS(string fileName) 
        {
            if (!System.IO.File.Exists(fileName)) {
                var e = new FileNotFoundException(fileName);
                m_ExceptionHandler(e);
                throw e;
            }

            m_FileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
            yield return null;
        }

        private IEnumerator LoadFileWR(string fileName) 
        {

            UnityWebRequest www = new UnityWebRequest(fileName);
            DownloadHandlerBuffer dH = new DownloadHandlerBuffer();
            www.downloadHandler = dH;
            yield return www.SendWebRequest();
            m_FileStream = new MemoryStream(www.downloadHandler.data);
        }

        public IEnumerator Parse()
        {
            yield return LoadFile(m_FileName);

            using (m_FileStream) {
                m_ZipFile = new ZipFile(m_FileStream);

                using (m_ZipFile) {
                    if(!m_FileName.Contains(WorldObjects.SCENE_GRAPH_LIBRARY_NAME))
                        CacheThumbnail(m_FileName);
                    
                    // TODO: Use manifest to determine player assembly version
                    string playerAssembly = Player.PlayerAssemblies.CURRENT;
                    m_System.AddStaticAssembly(Player.PlayerAssemblies.Assembly(playerAssembly));

                    yield return ParseJson(m_ZipFile.ReadEntry("manifest.json"));
                }
            }
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
                        yield return JsonParser.Parse(m_System, libRef.path.fullPath, m_ExceptionHandler);
                    }
                    else
                    {
                        var e = new TweedleVersionException("Could not find prerequisite " + t.name,
                            WorldObjects.SCENE_GRAPH_LIBRARY_NAME + " " + PlayerLibraryManifest.Instance.GetLibraryVersion(),
                            WorldObjects.SCENE_GRAPH_LIBRARY_NAME + " " + t.version,
                            PlayerLibraryManifest.Instance.aliceVersion,
                            manifest.provenance.aliceVersion);

                        m_ExceptionHandler(e);
                        throw e;
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
            string zipPath = workingDir + resourceRef.file;

            switch (resourceRef.ContentType)
            {
                case ContentType.Audio:
                    strictRef = JsonUtility.FromJson<AudioReference>(refJson);
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

        private void ParseTweedleTypeResource(ResourceReference resourceRef, string workingDir) {
            try
            {
                using TextReader tweedleStream = new StreamReader(m_ZipFile.OpenEntryStream(workingDir + resourceRef.file));
                var tweedleType = m_Parser.ParseType(tweedleStream, m_System.GetRuntimeAssembly(), resourceRef.file);
                m_System.GetRuntimeAssembly().Add(tweedleType);
            }
            catch (Exception e)
            {
                var e2 = new TweedleParseException("Unable to read " + resourceRef.file, e);
                m_ExceptionHandler(e2);
                throw e2;
            }
        }

        private void CacheToDisk(ResourceReference resourceRef, string workingDir) {
#if !UNITY_WEBGL
            var cachePath = Application.temporaryCachePath + "/" + workingDir;
            if (!Directory.Exists(cachePath)) {
                Directory.CreateDirectory(cachePath);
            }

            var data = m_ZipFile.ReadDataEntry(workingDir + resourceRef.file);
            System.IO.File.WriteAllBytes(cachePath + resourceRef.file, data);
#endif
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
            byte[] data = m_ZipFile.ReadDataEntry(workingDir + resourceRef.file);

            if (Application.isPlaying)
            {
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
        }

        public static AudioClip LoadMp3(Stream stream, string filePath) {
            string filename = Path.GetFileNameWithoutExtension(filePath);

            MpegFile mpegFile = new MpegFile(stream);

            // assign samples into AudioClip
            AudioClip ac = AudioClip.Create(filename,
                                            (int)(mpegFile.Length / sizeof(float) / mpegFile.Channels),
                                            mpegFile.Channels,
                                            mpegFile.SampleRate,
                                            false,
                                            data => { int actualReadCount = mpegFile.ReadSamples(data, 0, data.Length); },
                                            position => { mpegFile.Position = position;  });
            return ac;
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

                // Allow unity loop to process in between model imports.
                yield return null;
            }
        }

        private static string GetDirectoryEntryPath(string inFilePath) {
            return Path.GetDirectoryName(inFilePath).Replace(Path.DirectorySeparatorChar, '/') + "/";
        }
    }
}

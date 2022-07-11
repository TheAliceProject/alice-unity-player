using System;
using System.Collections.Generic;
using Alice.Tweedle.File;
using Alice.Tweedle.Interop;
using Alice.Tweedle.VM;
using Alice.Player.Primitives;
using Alice.Player.Modules;
using UnityEngine;
using ICSharpCode.SharpZipLib.Zip;

namespace Alice.Tweedle.Parse
{
    public class TweedleSystem
    {
        public HashSet<ProjectIdentifier> LoadedFiles { get; private set; }

        public Dictionary<string, LibraryManifest> Libraries { get; private set; }
        public Dictionary<string, ProgramDescription> Programs { get; private set; }
        public Dictionary<string, ModelManifest> Models { get; private set; }

        public Dictionary<ResourceIdentifier, ResourceReference> Resources { get; private set; }

        private List<TAssembly> m_StaticAssemblies = new List<TAssembly>();

        private TAssembly m_LibraryAssembly;
        private TAssembly m_RuntimeAssembly;

        private List<TType> m_TypeList = new List<TType>();
        private Dictionary<string, TType> m_TypeMap = new Dictionary<string, TType>();

        public TweedleSystem()
        {
            LoadedFiles = new HashSet<ProjectIdentifier>();

            Libraries = new Dictionary<string, LibraryManifest>();
            Programs = new Dictionary<string, ProgramDescription>();
            Models = new Dictionary<string, ModelManifest>();

            Resources = new Dictionary<ResourceIdentifier, ResourceReference>();

            InitializePrimitives();
        }

        private void InitializePrimitives()
        {
            AddStaticAssembly(TBuiltInTypes.Assembly());
        }

        #region Adding Resources

        public void AddLibrary(LibraryManifest libAsset)
        {
            LoadedFiles.Add(libAsset.Identifier);
            Libraries.Add(libAsset.Identifier.name, libAsset);
        }

        public void AddProgram(ProgramDescription programAsset)
        {
            LoadedFiles.Add(programAsset.Identifier);
            Programs.Add(programAsset.Identifier.name, programAsset);
        }

        public void AddModel(ModelManifest modelAsset)
        {
            // Debug.LogFormat("Loading Model Asset {0} ", modelAsset.Identifier.name);
            LoadedFiles.Add(modelAsset.Identifier);
            Models.Add(modelAsset.Identifier.name, modelAsset);
        }

        public void AddResource(ResourceReference resourceAsset)
        {
            ResourceIdentifier identifier = new ResourceIdentifier(resourceAsset.file, resourceAsset.ContentType, resourceAsset.FormatType);
            if (Resources.ContainsKey(identifier)) {
                Debug.LogWarningFormat("Resources with identifier {0} already exists", identifier.ToString());
            } else {
                Resources.Add(identifier, resourceAsset);
            }
        }

        /// <summary>
        /// Set the library assembly to the system.
        /// TODO Library assembly is lazily read as needed and caches types and resources.
        /// It will not be unloaded when the system is unloaded.
        /// </summary>
        public void SetLibraryAssembly(TAssembly assembly) {
            m_LibraryAssembly = assembly;
            GetRuntimeAssembly().AddDependency(assembly);
        }

        /// <summary>
        /// Adds a static assembly to the system.
        /// Statically loaded assemblies will not be unloaded when the system is unloaded.
        /// </summary>
        public void AddStaticAssembly(TAssembly assembly)
        {
            if (!m_StaticAssemblies.Contains(assembly)) {
                m_StaticAssemblies.Add(assembly);
            }
        }

        #endregion // Adding Resources

        #region Steps

        /// <summary>
        /// Performs linking steps for all loaded assemblies.
        /// Will skip over assemblies that have already been linked.
        /// </summary>
        public void Link()
        {
            foreach(var assembly in m_StaticAssemblies)
                Link(assembly);
            Link(m_LibraryAssembly);
            Link(m_RuntimeAssembly);
        }

        private void Link(TAssembly inAssembly) {
            if (inAssembly == null) return;
            
            inAssembly.Link();

            foreach (var type in inAssembly.AllTypes()) {
                if (m_TypeList.Contains(type)) continue;
                m_TypeList.Add(type);
                m_TypeMap.Add(type.Name, type);
            }
        }

        public void ResetPrep() {
            foreach (var t in m_TypeList) {
                t.ResetPrep();
            }
        }

        /// <summary>
        /// Runs static initializers for all loaded types,
        /// and queues up any additional execution steps
        /// on the given VM.
        /// </summary>
        public void Prep(VirtualMachine inVM)
        {
            ITweedleExpression staticInitializer;
            for (int i = 0; i < m_TypeList.Count; ++i)
            {
                m_TypeList[i].Prep(out staticInitializer);
                if (staticInitializer != null)
                    inVM.Queue(staticInitializer);
            }
        }

        /// <summary>
        /// Unload run time assembly.
        /// </summary>
        public void Unload() {
            if (m_RuntimeAssembly == null) return;
            m_RuntimeAssembly.Unload();
            m_RuntimeAssembly = null;
        }

        #endregion // Steps

        /// <summary>
        /// Returns the runtime assembly for this system.
        /// This assembly will be unloaded when the system is unloaded.
        /// </summary>
        public TAssembly GetRuntimeAssembly()
        {
            if (m_RuntimeAssembly == null)
            {
                m_RuntimeAssembly = new TAssembly("RuntimeAssembly", m_StaticAssemblies, TAssemblyFlags.Runtime);
            }
            return m_RuntimeAssembly;
        }

        public TType TypeNamed(string name)
        {
            TType type;
            m_TypeMap.TryGetValue(name, out type);
            return type;
        }

        internal void DumpTypes()
        {
            foreach (var type in m_TypeList)
            {
                UnityEngine.Debug.Log(TType.DumpOutline(type));
            }
        }

        internal void CacheTexture(string name, Texture2D texture) {
            GetRuntimeAssembly().Textures.Add(name, texture);
        }

        public Texture2D TextureNamed(string name) {
            if (GetRuntimeAssembly().Textures.TryGetValue(name, out var texture)) {
                return texture;
            }
            if (m_LibraryAssembly != null && m_LibraryAssembly.Textures.TryGetValue(name, out texture)) {
                return texture;
            }
            return null;
        }

        internal void QueueProgramMain(VirtualMachine vm)
        {
            TType prog;
            vm.Initialize(this);
            if (m_TypeMap.TryGetValue("Program", out prog))
            {
                TValue progVal = TBuiltInTypes.TYPE_REF.Instantiate(prog);

                NamedArgument[] arguments = new NamedArgument[1];
                TArrayType arrayMemberType = m_RuntimeAssembly.GetArrayType(TBuiltInTypes.TEXT_STRING);
                ArrayInitializer args = new ArrayInitializer(arrayMemberType, Array.Empty<ITweedleExpression>());
                arguments[0] = new NamedArgument("args", args);

                vm.Queue(new MethodCallExpression(progVal, "main", arguments));
            }
        }
    }
}

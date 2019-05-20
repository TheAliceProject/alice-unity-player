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
        private List<TAssembly> m_DynamicAssemblies = new List<TAssembly>();

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
            Debug.LogWarningFormat("Adding Model Asset {0} ", modelAsset);
            Debug.LogWarningFormat("   With Identifier {0} ", modelAsset.Identifier);
            Debug.LogWarningFormat("             Named {0} ", modelAsset.Identifier.name);
            LoadedFiles.Add(modelAsset.Identifier);
            Models.Add(modelAsset.Identifier.name, modelAsset);
        }

        public void AddResource(ResourceReference resourceAsset)
        {
            ResourceIdentifier identifier = new ResourceIdentifier(resourceAsset.name, resourceAsset.ContentType, resourceAsset.FormatType);
            if (Resources.ContainsKey(identifier))
            {
                Debug.LogWarningFormat("Resources with identifier {0} already exists", identifier.ToString());
            }
            else
            {
                Resources.Add(identifier, resourceAsset);
            }
        }

        /// <summary>
        /// Adds a dynamic assembly to the system.
        /// Dynamically loaded assemblies will be unloaded when the system is unloaded.
        /// </summary>
        public void AddDynamicAssembly(TAssembly assembly)
        {
            if (!m_DynamicAssemblies.Contains(assembly)) {
                m_DynamicAssemblies.Add(assembly);
            }
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
            foreach(var assembly in m_DynamicAssemblies)
                Link(assembly);
        }

        private void Link(TAssembly inAssembly)
        {
            inAssembly.Link();

            var allTypes = inAssembly.AllTypes();
            for (int i = 0; i < allTypes.Count; ++i)
            {
                TType type = allTypes[i];
                m_TypeList.Add(type);
                m_TypeMap.Add(type.Name, type);
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
        /// Unloads all dynamically-loaded assemblies.
        /// </summary>
        public void Unload()
        {
            for (int i = m_DynamicAssemblies.Count - 1; i >= 0; --i)
            {
                m_DynamicAssemblies[i].Unload();
            }
            m_DynamicAssemblies.Clear();
            m_RuntimeAssembly = null;
        }

        #endregion // Steps

        /// <summary>
        /// Returns a runtime assembly for this system.
        /// This assembly will be unloaded when the system is unloaded.
        /// </summary>
        public TAssembly GetRuntimeAssembly()
        {
            if (m_RuntimeAssembly == null)
            {
                m_RuntimeAssembly = new TAssembly("RuntimeAssembly", m_StaticAssemblies.ToArray(), TAssemblyFlags.Runtime);
                m_DynamicAssemblies.Add(m_RuntimeAssembly);
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

        internal void QueueProgramMain(VirtualMachine vm)
        {
            TType prog;
            vm.Initialize(this);
            if (m_TypeMap.TryGetValue("Program", out prog))
            {
                TValue progVal = TBuiltInTypes.TYPE_REF.Instantiate(prog);
                NamedArgument[] arguments = NamedArgument.EMPTY_ARGS;
                vm.Queue(new MethodCallExpression(progVal, "main", arguments));
            }
        }
    }
}

using System;
using System.Collections.Generic;

namespace Alice.Tweedle
{
    /// <summary>
    /// Container for types.
    /// </summary>
    public class TAssembly
    {
        #region Types

        private enum Status
        {
            Unlinked,
            Linking,
            FinishedLinking,
            Unloaded
        }

        #endregion // Types

        public readonly string Name;
        public readonly TAssemblyFlags Flags;

        private Status m_Status = Status.Unlinked;

        private List<TType> m_TypeList;
        private Dictionary<string, TType> m_TypeMap;
        private TAssembly[] m_Dependencies;

        /// <summary>
        /// Creates a new assembly with the given name and dependencies.
        /// </summary>
        public TAssembly(string inName, TAssembly[] inDependencies, TAssemblyFlags inFlags)
        {
            Name = inName;
            Flags = inFlags;

            m_TypeList = new List<TType>();
            m_TypeMap = new Dictionary<string, TType>();
            m_Dependencies = inDependencies;
        }

        /// <summary>
        /// Adds a type to the assembly.
        /// </summary>
        public void Add(TType inType)
        {
            if (m_Status != Status.Unlinked)
                throw new Exception("Assembly " + Name + " is " + m_Status + " - no new types can be added");
            if (m_TypeMap.ContainsKey(inType.Name))
                throw new Exception("Type with name " + inType.Name + " already exists");
            if (inType.Assembly != null && inType.Assembly != this)
                throw new Exception("Type " + inType.Name + " cannot be added to assembly " + Name + " - should be added to " + inType.Assembly?.Name + " instead");

            m_TypeList.Add(inType);
            m_TypeMap.Add(inType.Name, inType);
        }

        /// <summary>
        /// Links type references in this assembly.
        /// </summary>
        public void Link()
        {
            if (m_Status != Status.Unlinked)
                return;

            m_Status = Status.Linking;
            {
                // Make sure to link dependencies first
                for (int i = 0; i < m_Dependencies.Length; ++i)
                {
                    m_Dependencies[i].Link();
                }

                TAssemblyLinkContext linkingContext = new TAssemblyLinkContext(this, m_Dependencies);

                for (int i = 0; i < m_TypeList.Count; ++i)
                {
                    m_TypeList[i].Link(linkingContext);
                }

                for (int i = 0; i < m_TypeList.Count; ++i)
                {
                    m_TypeList[i].PostLink(linkingContext);
                }

                m_TypeList.Sort();
            }
            m_Status = Status.FinishedLinking;
        }

        /// <summary>
        /// Performs any unloading steps for types in the assembly.
        /// Necessary to clean up the generics cache.
        /// Note: Once unloaded this assembly cannot be used again.
        /// </summary>
        public void Unload()
        {
            if (m_Status != Status.Unloaded)
            {
                m_Status = Status.Unloaded;
                TGenerics.Unload(this);
            }
        }

        /// <summary>
        /// Returns the type with the given name in the assembly.
        /// </summary>
        public TType TypeNamed(string inName)
        {
            TType type;
            m_TypeMap.TryGetValue(inName, out type);
            return type;
        }

        /// <summary>
        /// Returns a list of all existing types in the assembly.
        /// </summary>
        public List<TType> AllTypes()
        {
            return m_TypeList;
        }

        public override string ToString()
        {
            return string.Format("TAssembly:{0} ({1} types)", Name, m_TypeList.Count);
        }

        /// <summary>
        /// Dependency array representing no dependencies.
        /// </summary>
        static public readonly TAssembly[] NO_DEPENDENCIES = new TAssembly[0];
    }

    /// <summary>
    /// Attributes for TAssemblies.
    /// </summary>
    public enum TAssemblyFlags
    {
        None = 0,

        // The assembly cannot be unloaded.
        CannotUnload = 0x001,

        // This assembly is embedded in the system.
        Embedded = 0x002,

        // This is a runtime assembly.
        Runtime = 0x004
    }
}

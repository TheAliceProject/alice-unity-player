using System;
using System.Collections.Generic;
using Alice.Tweedle.Parse;

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

        private string m_Name;
        private Status m_Status = Status.Unlinked;

        private List<TType> m_TypeList;
        private Dictionary<string, TType> m_TypeMap;
        private TAssembly[] m_RequiredAssembliesAndThis;

        /// <summary>
        /// Creates a new assembly with the given name and dependencies.
        /// </summary>
        public TAssembly(string inName, TAssembly[] inDependencies)
        {
            m_Name = inName;
            m_TypeList = new List<TType>();
            m_TypeMap = new Dictionary<string, TType>();

            m_RequiredAssembliesAndThis = new TAssembly[inDependencies.Length + 1];
            for (int i = 0; i < inDependencies.Length; ++i)
                m_RequiredAssembliesAndThis[i] = inDependencies[i];
            m_RequiredAssembliesAndThis[inDependencies.Length] = this;
        }

        /// <summary>
        /// Adds a type to the assembly.
        /// </summary>
        public void Add(TType inType)
        {
            if (m_Status != Status.Unlinked)
                throw new TweedleLinkException("Assembly " + m_Name + " is " + m_Status + " - no new types can be added");
            if (m_TypeMap.ContainsKey(inType.Name))
                throw new TweedleLinkException("Type with name " + inType.Name + " already exists");

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
                for (int i = 0; i < m_RequiredAssembliesAndThis.Length - 1; ++i)
                {
                    m_RequiredAssembliesAndThis[i].Link();
                }

                for (int i = 0; i < m_TypeList.Count; ++i)
                {
                    m_TypeList[i].Link(m_RequiredAssembliesAndThis);
                }

                for (int i = 0; i < m_TypeList.Count; ++i)
                {
                    m_TypeList[i].PostLink(m_RequiredAssembliesAndThis);
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

                for (int i = m_TypeList.Count - 1; i >= 0; --i)
                {
                    TGenerics.Unload(m_TypeList[i]);
                }
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

        /// <summary>
        /// Retrieves the type with the given name, searching through
        /// all the given assemblies.
        /// </summary>
        static public TType TypeNamed(TAssembly[] inAssemblies, string inName)
        {
            for (int i = inAssemblies.Length - 1; i >= 0; --i)
            {
                TType type = inAssemblies[i].TypeNamed(inName);
                if (type != null)
                    return type;
            }

            return null;
        }

        /// <summary>
        /// Dependency array representing no dependencies.
        /// </summary>
        static public readonly TAssembly[] NO_DEPENDENCIES = new TAssembly[0];
    }
}

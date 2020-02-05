namespace Alice.Tweedle
{
    /// <summary>
    /// Assembly linking context.
    /// </summary>
    public class TAssemblyLinkContext
    {
        /// <summary>
        /// Assembly initializing the linking step.
        /// </summary>
        public readonly TAssembly OwningAssembly;
        
        // Array of dependent assemblies
        private readonly TAssembly[] m_Dependencies;

        public TAssemblyLinkContext(TAssembly inOwningAssembly, TAssembly[] inDependencies)
        {
            OwningAssembly = inOwningAssembly;
            m_Dependencies = inDependencies ?? TAssembly.NO_DEPENDENCIES;
        }

        /// <summary>
        /// Retrieves the type with the given name.
        /// </summary>
        public TType TypeNamed(string inName)
        {
            TType type = OwningAssembly.TypeNamed(inName);
            if (type != null)
                return type;

            for (int i = m_Dependencies.Length - 1; i >= 0; --i)
            {
                type = m_Dependencies[i].TypeNamed(inName);
                if (type != null)
                    return type;
            }

            return null;
        }
    }
}

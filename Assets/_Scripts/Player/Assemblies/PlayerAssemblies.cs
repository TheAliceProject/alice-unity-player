using System.Diagnostics;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using System.Collections.Generic;
using System;

namespace Alice.Player
{
    /// <summary>
    /// Provides definitions and assemblies for interop code between Tweedle and the player.
    /// </summary>
    static public partial class PlayerAssemblies
    {
        #region Definitions

        private delegate void AssemblyCreationDelegate(TAssembly inAssembly);

        static private TAssembly s_CurrentAssembly;
        static private string s_CurrentAssemblyName;

        static private readonly TAssembly[] DEFAULT_DEPENDENCIES = new TAssembly[] { TBuiltInTypes.Assembly() };
        static private Dictionary<string, AssemblyCreationDelegate> s_AssemblyInitializers;

        /// <summary>
        /// Defines an initializer function for the given player assembly version.
        /// </summary>
        static private string DefineVersion(string inVersion, AssemblyCreationDelegate inInitDelegate)
        {
            if (s_AssemblyInitializers == null)
                s_AssemblyInitializers = new Dictionary<string, AssemblyCreationDelegate>();

            if (s_AssemblyInitializers.ContainsKey(inVersion))
                throw new Exception("Assembly with version " + inVersion + " already defined");

            s_AssemblyInitializers.Add(inVersion, inInitDelegate);
            return inVersion;
        }

        #endregion // Definitions

        /// <summary>
        /// Most current version of the player assembly.
        /// </summary>
        static public readonly string CURRENT = VERSION_0_0_1; // NOTE: Update this when more recent versions of the player are created.

        /// <summary>
        /// Returns the assembly for the given version.
        /// If already loaded, this will return the cached version.
        /// Otherwise, this will unload any existing assembly and generate a new assembly.
        /// </summary>
        static public TAssembly Assembly(string inVersion)
        {
            if (inVersion == null)
                throw new ArgumentNullException("inVersion");

            if (s_CurrentAssemblyName == inVersion)
                return s_CurrentAssembly;

            AssemblyCreationDelegate initDelegate;
            if (s_AssemblyInitializers == null || !s_AssemblyInitializers.TryGetValue(inVersion, out initDelegate))
                throw new TweedleRuntimeException("Unable to find definition for player assembly with version " + inVersion);

            if (s_CurrentAssembly != null)
            {
                s_CurrentAssembly.Unload();
                s_CurrentAssembly = null;
                s_CurrentAssemblyName = null;
            }

            s_CurrentAssemblyName = inVersion;
            s_CurrentAssembly = new TAssembly(inVersion, DEFAULT_DEPENDENCIES, TAssemblyFlags.Embedded);
            initDelegate(s_CurrentAssembly);

            return s_CurrentAssembly;
        }

        /// <summary>
        /// Unloads the current assembly.
        /// </summary>
        static public void Unload()
        {
            if (s_CurrentAssembly != null)
            {
                s_CurrentAssembly.Unload();
                s_CurrentAssembly = null;
                s_CurrentAssemblyName = null;
            }
        }
    }
}
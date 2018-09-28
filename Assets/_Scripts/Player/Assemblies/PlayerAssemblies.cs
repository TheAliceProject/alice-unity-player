using System.Diagnostics;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using System.Collections.Generic;
using System;

namespace Alice.Player
{
    static public partial class PlayerAssemblies
    {
        #region Definitions

        private delegate void AssemblyCreationDelegate(TAssembly inAssembly);

        static private TAssembly s_CurrentAssembly;
        static private string s_CurrentAssemblyName;

        static private readonly TAssembly[] DEFAULT_DEPENDENCIES = new TAssembly[] { TStaticTypes.Assembly() };
        static private Dictionary<string, AssemblyCreationDelegate> s_AssemblyInitializers;

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

        static public readonly string CURRENT = VERSION_0_0_1;

        static public TAssembly Assembly(string inName)
        {
            if (inName == null)
                throw new ArgumentNullException("inName");

            if (s_CurrentAssemblyName == inName)
                return s_CurrentAssembly;

            AssemblyCreationDelegate initDelegate;
            if (s_AssemblyInitializers == null || !s_AssemblyInitializers.TryGetValue(inName, out initDelegate))
                throw new TweedleRuntimeException("Unable to locate player assembly with name " + inName);

            if (s_CurrentAssembly != null)
            {
                s_CurrentAssembly.Unload();
                s_CurrentAssembly = null;
                s_CurrentAssemblyName = null;
            }

            s_CurrentAssemblyName = inName;
            s_CurrentAssembly = new TAssembly(inName, DEFAULT_DEPENDENCIES);
            initDelegate(s_CurrentAssembly);

            return s_CurrentAssembly;
        }
    }
}
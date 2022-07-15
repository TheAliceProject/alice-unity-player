using Alice.Tweedle.Interop;
using Alice.Tweedle;

namespace Alice.Player.Modules
{
    [PInteropType("System")]
    public static class SystemModule
    {
        [PInteropMethod]
        public static string getClassName(TValue instance)
        {
            return instance.Type.Name;
        }

        [PInteropMethod]
        public static string getDefaultWorldMessage()
        {
#if UNITY_WEBGL
            return "Replace the project file, hosted at: " + Tweedle.Parse.GameController.AutoLoadedWorldsDirectory;
#else
            return "Put them in " + Tweedle.Parse.GameController.AutoLoadedWorldsDirectory;
#endif
        }
    }    
}
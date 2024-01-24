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
        public static string getDefaultWorldMessage() {
            return Tweedle.Parse.GameController.GetDefaultWorldMessage();
        }
    }    
}
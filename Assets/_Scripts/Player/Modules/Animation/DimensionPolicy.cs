using Alice.Tweedle.Interop;

namespace Alice.Player.Modules {
    [PInteropType("DimensionPolicy")]
    public enum DimensionPolicyEnum {
        PRESERVE_NOTHING = 0,
        PRESERVE_VOLUME = 1,
        PRESERVE_ASPECT_RATIO = 2,
    }
}
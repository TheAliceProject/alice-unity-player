using System.Diagnostics;
using Alice.Tweedle.Interop;

namespace Alice.Player.Modules
{
    [PInteropType("ShapeModel")]
    public enum ShapeModelEmum
    {
        BOX,
        CONE,
        CYLINDER,
        DISC,
        SPHERE,
        TORUS
    }
}
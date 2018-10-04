using System.Diagnostics;
using Alice.Tweedle.Interop;

namespace Alice.Player.Modules
{
    [PInteropType("AnimationStyle")]
    public enum AnimationStyleEnum
    {
        BEGIN_AND_END_ABRUPTLY = 0,
        BEGIN_GENTLY_AND_END_ABRUPTLY = 1,
        BEGIN_ABRUPTLY_AND_END_GENTLY = 2,
        BEGIN_AND_END_GENTLY = 3
    }
}
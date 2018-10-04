using System;

namespace Alice.Tweedle
{
    /// <summary>
    /// Modifiers for a type member.
    /// </summary>
    [Flags]
    public enum MemberFlags
    {
        // No special modifier
        None            = 0x000,

        // Field is readonly
        Readonly        = 0x001,

        // Member is per-instance
        Instance        = 0x002,

        // Member is static
        Static            = 0x004,

        // Field
        Field            = 0x008,
        
        // Method
        Method            = 0x010,
        
        // Constructor Method
        Constructor        = 0x020,

        // From proxy/platform type
        PInterop        = 0x040,

        // Is an asynchronous method
        Async            = 0x080
    }
}
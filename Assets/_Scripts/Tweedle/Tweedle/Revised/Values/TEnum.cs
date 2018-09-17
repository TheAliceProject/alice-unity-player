using System.Collections;
using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	/// <summary>
	/// Name, ID, and object.
	/// </summary>
	public sealed class TEnum
	{
        public readonly string Name;
        public readonly int Value;
        public readonly TObject Object;

        /// <summary>
		/// Creates an enum value.
		/// </summary>
        public TEnum(string inName, int inValue)
		{
            Name = inName;
            Value = inValue;
            Object = new TObject();
        }

        static public explicit operator TObject(TEnum inEnum)
        {
            return inEnum?.Object;
        }
    }
}
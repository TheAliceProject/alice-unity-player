using System.Diagnostics;
using Alice.Tweedle.Interop;

namespace Alice.Tweedle.Modules
{
    [PInteropType]
    public sealed class Portion
    {
        public readonly double value;

        [PInteropConstructor]
        public Portion(double number)
        {
            if (value < 0 || value > 1)
                throw new TweedleRuntimeException("Cannot instantiate Portion with value " + value + " - must be between 0 and 1");
            value = number;
        }

        [PInteropConstructor]
        public Portion(Portion clone)
        {
            value = clone.value;
        }

        static public implicit operator double(Portion inPortion)
        {
            return inPortion != null ? inPortion.value : double.NaN;
        }
    }
}
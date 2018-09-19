using System.Diagnostics;
using Alice.Tweedle.Interop;

namespace Alice.Tweedle.Primitives
{
    [PInteropType]
    public sealed class Portion
    {
        [PInteropField]
        public readonly double value;

        #region  Interop Interfaces
        [PInteropConstructor]
        public Portion(double number)
        {
            if (number < 0 || number > 1)
                throw new TweedleRuntimeException("Cannot instantiate Portion with value " + number + " - must be between 0 and 1");
            value = number;
        }

        [PInteropConstructor]
        public Portion(Portion clone)
        {
            value = clone.value;
        }

        [PInteropMethod]
        public bool equals(Portion portion) {
            return value == portion;
        }

        [PInteropMethod]
        public Portion plus(Portion portion) {
            return new Portion(System.Math.Min(value + portion, 1));
        }

        [PInteropMethod]
        public Portion minus(Portion portion) {
            return new Portion(System.Math.Max(value - portion, 0));
        }

        public static Portion lerp(Portion a, Portion b, Portion t) {
            return new Portion((b.value-a.value)*t.value + a.value);
        }
        #endregion



        static public implicit operator double(Portion inPortion)
        {
            return inPortion != null ? inPortion.value : double.NaN;
        }

       

        
    }
}
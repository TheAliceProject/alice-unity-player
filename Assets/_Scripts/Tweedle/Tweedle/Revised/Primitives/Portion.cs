using System.Diagnostics;
using Alice.Tweedle.Interop;

namespace Alice.Tweedle.Primitives
{
    [PInteropType]
    public sealed class Portion
    {
       
        public readonly double value;

        #region Interop Interface 
        [PInteropConstructor]
        public Portion(double portion)
        {
            if (portion < 0 || portion > 1)
                throw new TweedleRuntimeException("Cannot instantiate Portion with value " + portion + " - must be between 0 and 1");
            value = portion;
        }

        [PInteropConstructor]
        public Portion(Portion clone)
        {
            value = clone.value;
        }

        [PInteropMethod]
        public bool equals(Portion other) {
            return value == other;
        }

        [PInteropMethod]
        public Portion add(Portion other) {
            return new Portion(System.Math.Min(value + other, 1));
        }

        [PInteropMethod]
        public Portion subtract(Portion other) {
            return new Portion(System.Math.Max(value - other, 0));
        }

        [PInteropMethod]
        public Portion interpolatePortion(Portion end, double portion) {
            return new Portion((end.value-value)*portion + value);
        }
        #endregion // Interop Interfaces



        static public implicit operator double(Portion inPortion)
        {
            return inPortion != null ? inPortion.value : double.NaN;
        }

       
        public override string ToString() {
            return string.Format("Portion({0:0.####})", value);
        }
        
    }
}
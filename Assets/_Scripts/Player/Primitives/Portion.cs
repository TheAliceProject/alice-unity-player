using System.Diagnostics;
using Alice.Tweedle.Interop;
using Alice.Tweedle;

namespace Alice.Player.Primitives
{
    [PInteropType]
    public sealed class Portion
    {
       
        public readonly double Value;

        #region Interop Interface 
        
        [PInteropConstructor]
        public Portion(double portion)
        {
            if (portion < 0 || portion > 1)
                throw new TweedleRuntimeException("Cannot instantiate Portion with value " + portion + " - must be between 0 and 1");
            Value = portion;
        }

        [PInteropField]
        public double numberValue { get { return Value; } }

        [PInteropMethod]
        public bool equals(Portion other) {
            return Value == other;
        }

        [PInteropMethod]
        public Portion add(Portion other) {
            return new Portion(System.Math.Min(Value + other, 1));
        }

        [PInteropMethod]
        public Portion subtract(Portion other) {
            return new Portion(System.Math.Max(Value - other, 0));
        }

        [PInteropMethod]
        public Portion interpolatePortion(Portion end, Portion portion) {
            return new Portion((end.Value-Value)*portion.Value + Value);
        }
        #endregion // Interop Interfaces



        static public implicit operator double(Portion inPortion)
        {
            return inPortion != null ? inPortion.Value : double.NaN;
        }

       
        public override string ToString() {
            return string.Format("Portion({0:0.####})", Value);
        }
        
        public override bool Equals(object obj) {
            if (obj is Portion) {
                return equals((Portion)obj);
            }
            return false;
        }

        public override int GetHashCode() {
            return Value.GetHashCode();
        }
    }
}
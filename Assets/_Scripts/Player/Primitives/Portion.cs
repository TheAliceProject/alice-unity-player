using Alice.Tweedle.Interop;
using UnityEngine;

namespace Alice.Player.Primitives
{
    [PInteropType]
    public class Portion
    {
       
        public readonly double Value;

        #region Interop Interface 

        [PInteropField]
        public static readonly Portion NONE = new Portion(0);
        [PInteropField]
        public static readonly Portion WHOLE = new Portion(1);
        
        [PInteropConstructor]
        public Portion(double portion)
        {
            if (portion < 0) {
                Debug.Log("Portion must be between 0 and 1. Treating " + portion + " as zero "  );
                Value = 0;
            } else if (portion > 1) {
                Debug.Log("Portion must be between 0 and 1. Treating " + portion + " as one "  );
                Value = 1;
            } else {
                Value = portion;
            }
        }

        [PInteropField]
        public double numberValue { get { return Value; } }

        [PInteropMethod]
        public bool equals(Portion other) {
            return Equals(other);
        }

        [PInteropMethod]
        public double toDecimalNumber() {
            return Value;
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
            return inPortion.Value;
        }
        
        static public implicit operator float(Portion inPortion)
        {
            return (float)inPortion.Value;
        }

        public override string ToString() {
            return string.Format("Portion({0:0.####})", Value);
        }
        
        public override bool Equals(object obj) {
            if (obj is Portion) {
                return ((Portion)obj).Value == Value;
            }
            return false;
        }

        public override int GetHashCode() {
            return Value.GetHashCode();
        }
    }
}
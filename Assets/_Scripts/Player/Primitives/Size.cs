using System.Diagnostics;
using Alice.Tweedle.Interop;

namespace Alice.Player.Primitives
{
    [PInteropType]
    public sealed class Size
    {
        public readonly Vector3 Value;

        public Size(Vector3 inVector)
        {
            Value = inVector;
        }

        #region Interop Interfaces
        [PInteropField]
        public double width { get {return Value.X; } }
        [PInteropField]
        public double height { get {return Value.Y; } }
        [PInteropField]
        public double depth { get {return Value.Z; } }

       
        [PInteropConstructor]
        public Size(double width, double height, double depth)
        {
            Value = new Vector3(width, height, depth);
        }

        [PInteropMethod]
        public bool equals(Size other) {
            return Value == other.Value;
        }

        [PInteropMethod]
        public Size scaledBy(double factor) {
            return new Size(Value*factor);
        }

        [PInteropMethod]
        public Size interpolatePortion(Size end, double portion) {
            return new Size(Vector3.Lerp(Value, end.Value, portion));
        }
        #endregion

        static public implicit operator UnityEngine.Vector3(Size inPosition)
        {
            return inPosition != null ? new UnityEngine.Vector3((float)inPosition.Value.X, (float)inPosition.Value.Y, (float)inPosition.Value.Z) : new UnityEngine.Vector3(float.NaN, float.NaN, float.NaN);
        }

        public override string ToString() {
            return string.Format("Size({0:0.##},{1:0.##},{2:0.##})", Value.X, Value.Y, Value.Z);
        }

        public override bool Equals(object obj) {
            if (obj is Size) {
                return equals((Size)obj);
            }
            return false;
        }

        public override int GetHashCode() {
            return Value.GetHashCode();
        }
    
    }
}
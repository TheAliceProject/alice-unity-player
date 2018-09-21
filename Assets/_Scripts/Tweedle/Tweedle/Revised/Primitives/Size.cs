using System.Diagnostics;
using Alice.Tweedle.Interop;

namespace Alice.Tweedle.Primitives
{
    [PInteropType]
    public sealed class Size
    {
        public readonly Vector3 value;

        public Size(Vector3 inVector)
        {
            value = inVector;
        }

        #region Interop Interfaces
        [PInteropField]
        public double width { get {return value.X; } }
        [PInteropField]
        public double height { get {return value.Y; } }
        [PInteropField]
        public double depth { get {return value.Z; } }

       
        [PInteropConstructor]
        public Size(double width, double height, double depth)
        {
            value = new Vector3(width, height, depth);
        }

        [PInteropConstructor]
        public Size(Size clone)
        {
            value = clone.value;
        }

        [PInteropMethod]
        public bool equals(Size other) {
            return value == other.value;
        }

        [PInteropMethod]
        public Size scaledBy(double factor) {
            return new Size(value*factor);
        }

        [PInteropMethod]
        public Size interpolatePortion(Size end, double portion) {
            return new Size(Vector3.Lerp(value, end.value, portion));
        }
        #endregion

        static public implicit operator UnityEngine.Vector3(Size inPosition)
        {
            return inPosition != null ? new UnityEngine.Vector3((float)inPosition.value.X, (float)inPosition.value.Y, (float)inPosition.value.Z) : new UnityEngine.Vector3(float.NaN, float.NaN, float.NaN);
        }

        public override string ToString() {
            return string.Format("Size({0:0.##},{1:0.##},{2:0.##})", value.X, value.Y, value.Z);
        }

        public override bool Equals(object obj) {
            if (obj is Size) {
                return equals((Size)obj);
            }
            return false;
        }

        public override int GetHashCode() {
            return value.GetHashCode();
        }
    
    }
}
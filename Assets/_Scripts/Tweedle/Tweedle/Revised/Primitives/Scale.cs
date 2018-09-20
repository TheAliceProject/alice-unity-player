using System.Diagnostics;
using Alice.Tweedle.Interop;

namespace Alice.Tweedle.Primitives
{
    [PInteropType]
    public sealed class Scale
    {
        public readonly Vector3 value = new Vector3(1,1,1);

        public Scale(Vector3 inVector)
        {
            value = inVector;
        }

        #region Interop Interfaces
        public static readonly Scale ONE = new Scale(1,1,1);

        [PInteropField]
        public double x { get {return value.X; } }
        [PInteropField]
        public double y { get {return value.Y; } }
        [PInteropField]
        public double z { get {return value.Z; } }

       
        [PInteropConstructor]
        public Scale(double x, double y, double z)
        {
            value = new Vector3(x, y, z);
        }

        [PInteropConstructor]
        public Scale(Size clone)
        {
            value = clone.value;
        }

        [PInteropMethod]
        public bool equals(Scale other) {
            return value == other.value;
        }

        [PInteropMethod]
        public Scale scaledBy(double factor) {
            return new Scale(value*factor);
        }

        [PInteropMethod]
        public Scale interpolatePortion(Scale end, double portion) {
            return new Scale(Vector3.Lerp(value, end.value, portion));
        }
        #endregion

        static public implicit operator UnityEngine.Vector3(Scale inPosition)
        {
            return inPosition != null ? new UnityEngine.Vector3((float)inPosition.value.X, (float)inPosition.value.Y, (float)inPosition.value.Z) : new UnityEngine.Vector3(float.NaN, float.NaN, float.NaN);
        }

        public override string ToString() {
            return string.Format("Scale({0:0.##},{1:0.##},{2:0.##})", value.X, value.Y, value.Z);
        }

    }
}
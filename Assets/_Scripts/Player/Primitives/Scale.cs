using System.Diagnostics;
using Alice.Tweedle.Interop;

namespace Alice.Player.Primitives
{
    [PInteropType]
    public sealed class Scale
    {
        public readonly Vector3 Value = new Vector3(1,1,1);

        public Scale(Vector3 inVector)
        {
            Value = inVector;
        }

        #region Interop Interfaces
        public static readonly Scale ONE = new Scale(1,1,1);

        [PInteropField]
        public double x { get {return Value.X; } }
        [PInteropField]
        public double y { get {return Value.Y; } }
        [PInteropField]
        public double z { get {return Value.Z; } }

       
        [PInteropConstructor]
        public Scale(double x, double y, double z)
        {
            Value = new Vector3(x, y, z);
        }

        [PInteropMethod]
        public bool equals(Scale other) {
            return Value == other.Value;
        }

        [PInteropMethod]
        public Scale scaledBy(double factor) {
            return new Scale(Value*factor);
        }

        [PInteropMethod]
        public Scale interpolatePortion(Scale end, double portion) {
            return new Scale(Vector3.Lerp(Value, end.Value, portion));
        }
        #endregion

        static public implicit operator UnityEngine.Vector3(Scale inPosition)
        {
            return inPosition != null ? new UnityEngine.Vector3((float)inPosition.Value.X, (float)inPosition.Value.Y, (float)inPosition.Value.Z) : new UnityEngine.Vector3(float.NaN, float.NaN, float.NaN);
        }

        public override string ToString() {
            return string.Format("Scale({0:0.##},{1:0.##},{2:0.##})", Value.X, Value.Y, Value.Z);
        }

        public override bool Equals(object obj) {
            if (obj is Scale) {
                return equals((Scale)obj);
            }
            return false;
        }

        public override int GetHashCode() {
            return Value.GetHashCode();
        }

    }
}
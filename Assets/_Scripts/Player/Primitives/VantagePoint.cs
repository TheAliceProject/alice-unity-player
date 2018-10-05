using System.Diagnostics;
using Alice.Tweedle.Interop;

namespace Alice.Player.Primitives
{
    [PInteropType]
    public sealed class VantagePoint {

        public readonly Matrix4x4 Value = Matrix4x4.Identity;

        public VantagePoint(Matrix4x4 inMatrix) {
            Value = inMatrix;
        }

        #region Interop Interfaces
        [PInteropField]
        public Orientation orientation { get { return new Orientation(Quaternion.CreateFromRotationMatrix(Value)); } }
        [PInteropField]
        public Position translation { get { return new Position(Value.Translation); } }

        [PInteropConstructor]
        public VantagePoint() {}

        [PInteropConstructor]
        public VantagePoint(double m11, double m12, double m13, double m14,
                            double m21, double m22, double m23, double m24,
                            double m31, double m32, double m33, double m34,
                            double m41, double m42, double m43, double m44) 
        {
            Value = new Matrix4x4(m11, m12, m13, m14, m21, m22, m23, m24, m31, m32, m33, m34, m41, m42, m43, m44);
        }

        [PInteropConstructor]
        public VantagePoint(Orientation orientation, Position translation) {
            Value = Matrix4x4.CreateFromQuaternion(orientation.Value);
            Value.Translation = translation.Value;
        }

        [PInteropMethod]
        public bool equals(VantagePoint other) {
            return Value == other.Value;
        }

        [PInteropMethod]
        public VantagePoint multiply(VantagePoint other) {
            return new VantagePoint(Matrix4x4.Multiply(Value, other.Value));
        }

            [PInteropMethod]
            public VantagePoint inverse() {
            Matrix4x4 result;
            Matrix4x4.Invert(Value, out result);
            return new VantagePoint(result);
        }

        [PInteropMethod]
        public VantagePoint interpolatePortion(VantagePoint end, Portion portion) {
            return new VantagePoint(Matrix4x4.Lerp(Value, end.Value, portion.Value));
        }
        #endregion // interop interfaces

        public override string ToString() {
            return string.Format("VantagePoint(\n"+
            "\t{0:0.##},{1:0.##},{2:0.##},{3:0.##}\n" +
            "\t{4:0.##},{5:0.##},{6:0.##},{7:0.##}\n" +
            "\t{8:0.##},{9:0.##},{10:0.##},{11:0.##}\n" + 
            "\t{12:0.##},{13:0.##},{14:0.##},{15:0.##})",
            Value.M11, Value.M21, Value.M31, Value.M41,
            Value.M12, Value.M22, Value.M32, Value.M42,
            Value.M13, Value.M23, Value.M33, Value.M43,
            Value.M14, Value.M24, Value.M34, Value.M44
            );
        }

        public override bool Equals(object obj) {
            if (obj is VantagePoint) {
                return equals((VantagePoint)obj);
            }
            return false;
        }

        public override int GetHashCode() {
            return Value.GetHashCode();
        }
    }
    
}

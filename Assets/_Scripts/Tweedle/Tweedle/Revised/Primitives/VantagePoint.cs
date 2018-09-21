using System.Diagnostics;
using Alice.Tweedle.Interop;

namespace Alice.Tweedle.Primitives
{
	[PInteropType]
	public sealed class VantagePoint {

		public Matrix4x4 value = Matrix4x4.Identity;

		public VantagePoint(Matrix4x4 inMatrix) {
			value = inMatrix;
		}

		#region Interop Interfaces
		[PInteropField]
		public Orientation orientation { get { return new Orientation(Quaternion.CreateFromRotationMatrix(value)); } }
		[PInteropField]
		public Position translation { get { return new Position(value.Translation); } }

		[PInteropConstructor]
		public VantagePoint() {}

		[PInteropConstructor]
		public VantagePoint(double m11, double m12, double m13, double m14,
							double m21, double m22, double m23, double m24,
                         	double m31, double m32, double m33, double m34,
                         	double m41, double m42, double m43, double m44) 
		{
			value = new Matrix4x4(m11, m12, m13, m14, m21, m22, m23, m24, m31, m32, m33, m34, m41, m42, m43, m44);
		}

		[PInteropConstructor]
		public VantagePoint(Orientation orientation, Position translation) {
			value = Matrix4x4.CreateFromQuaternion(orientation.value);
			value.Translation = translation.value;
		}

		[PInteropConstructor]
		public VantagePoint(VantagePoint clone) 
		{
			value = clone.value;
		}

		[PInteropMethod]
		public bool equals(VantagePoint other) {
			return value == other.value;
		}

		[PInteropMethod]
		public VantagePoint multiply(VantagePoint other) {
			return new VantagePoint(Matrix4x4.Multiply(value, other.value));
		}

		[PInteropMethod]
		public VantagePoint inverse() {
			VantagePoint result = new VantagePoint();
			Matrix4x4.Invert(value, out result.value);
			return result;
		}
		#endregion // interop interfaces

		public override string ToString() {
			return string.Format("VantagePoint(\n"+
			"\t{0:0.##},{1:0.##},{2:0.##},{3:0.##}\n" +
			"\t{4:0.##},{5:0.##},{6:0.##},{7:0.##}\n" +
			"\t{8:0.##},{9:0.##},{10:0.##},{11:0.##}\n" + 
			"\t{12:0.##},{13:0.##},{14:0.##},{15:0.##})",
			value.M11, value.M21, value.M31, value.M41,
			value.M12, value.M22, value.M32, value.M42,
			value.M13, value.M23, value.M33, value.M43,
			value.M14, value.M24, value.M34, value.M44
			);
		}

		public override bool Equals(object obj) {
            if (obj is VantagePoint) {
                return equals((VantagePoint)obj);
            }
            return false;
        }
		
		public override int GetHashCode() {
            return value.GetHashCode();
        }
	}
	
}

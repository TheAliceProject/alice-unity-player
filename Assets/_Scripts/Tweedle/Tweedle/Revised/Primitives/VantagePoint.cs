using System.Diagnostics;
using Alice.Tweedle.Interop;

namespace Alice.Tweedle.Primatives
{
	[PInteropType]
	public class VantagePoint {

		public Matrix4x4 value = Matrix4x4.Identity;


		[PInteropConstructor]
		public VantagePoint() {}

		public VantagePoint(double m11, double m12, double m13, double m14,
							double m21, double m22, double m23, double m24,
                         	double m31, double m32, double m33, double m34,
                         	double m41, double m42, double m43, double m44) 
		{
			value = new Matrix4x4(m11, m12, m13, m14, m21, m22, m23, m24, m31, m32, m33, m34, m41, m42, m43, m44);
		}

		[PInteropConstructor]
		public VantagePoint(VantagePoint clone) 
		{
			value = clone.value;
		}

		[PInteropMethod]
		public VantagePoint multiply(VantagePoint vantagePoint) {
			VantagePoint result = new VantagePoint();
			result.value = Matrix4x4.Multiply(value, vantagePoint.value);
			return result;
		}

		[PInteropMethod]
		public VantagePoint inverse() {
			VantagePoint result = new VantagePoint();
			Matrix4x4.Invert(value, out result.value);
			return result;
		}
	}
	
}

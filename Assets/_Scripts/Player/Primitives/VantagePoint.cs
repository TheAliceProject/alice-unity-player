using System.Diagnostics;
using Alice.Tweedle.Interop;

namespace Alice.Player.Primitives
{
    [PInteropType]
    public struct VantagePoint {
        #region Cubic Polynomial Evaluation
        // NOTE: hermite cubic matrix is transposed from Alice because of microsoft's matrix is row-ordered
        static private readonly Matrix4x4 s_HermiteCubic = Matrix4x4.Transpose(new Matrix4x4(2, -2, 1, 1, -3, 3, -2, -1, 0, 0, 1, 0, 1, 0, 0, 0));

        private static double EvaluateCubic(Matrix4x4 m_h, double m_gx, double m_gy, double m_gz, double m_gw, double t) {
            double ttt = t * t * t;
            double tt = t * t;
            return ( ( ( ttt * m_h.M11 ) + ( tt * m_h.M12 ) + ( t * m_h.M13 ) + m_h.M14) * m_gx ) +
                    ( ( ( ttt * m_h.M21 ) + ( tt * m_h.M22 ) + ( t * m_h.M23 ) + m_h.M24 ) * m_gy ) +
                    ( ( ( ttt * m_h.M31 ) + ( tt * m_h.M32 ) + ( t * m_h.M33 ) + m_h.M34 ) * m_gz ) +
                    ( ( ( ttt * m_h.M41 ) + ( tt * m_h.M42) + ( t * m_h.M43) + m_h.M44 ) * m_gw );
        }

        private static double EvaluateCubicDerivative(Matrix4x4 m_h, double m_gx, double m_gy, double m_gz, double m_gw, double t ) {
            double tt3 = t * t * 3;
            double t2 = t * 2;
            return ( ( ( tt3 * m_h.M11 ) + ( t2 * m_h.M12 ) + m_h.M13 ) * m_gx ) +
                    ( ( ( tt3 * m_h.M21 ) + ( t2 * m_h.M22 ) + m_h.M23 ) * m_gy ) +
                    ( ( ( tt3 * m_h.M31 ) + ( t2 * m_h.M32 ) + m_h.M33 ) * m_gz ) +
                    ( ( ( tt3 * m_h.M41 ) + ( t2 * m_h.M42 ) + m_h.M43) * m_gw );
        }
        #endregion // Cubic Polynomial Evaluation

        #region Unity Conversions
        /// <summary>
        /// Convert from Unity's left-hand coord system to Alice's right-hand coord system
        /// </summary>
        public static VantagePoint FromUnity(UnityEngine.Vector3 pos, UnityEngine.Quaternion rot) {
            return new VantagePoint(new Primitives.Vector3(pos.x, pos.y, -pos.z), new Primitives.Quaternion(rot.x, rot.y, -rot.z, -rot.w));
        }

        public UnityEngine.Vector3 UnityPosition() {
            return new UnityEngine.Vector3((float)TranslationValue.X, (float)TranslationValue.Y, -(float)TranslationValue.Z);
        }

        public UnityEngine.Quaternion UnityRotation() {
            return new UnityEngine.Quaternion((float)RotationValue.X, (float)RotationValue.Y, -(float)RotationValue.Z, -(float)RotationValue.W);
        }
        #endregion // Unity Conversions
        
        public readonly Quaternion RotationValue;
        public readonly Vector3 TranslationValue;

        /// <summary>
        /// Returns a matrix with basis vectors stored in rows
        /// </summary>
        public Matrix4x4 GetMatrix() {
            var m = Matrix4x4.CreateFromQuaternion(RotationValue);
            m.Translation = TranslationValue;
            return m;
        }


        public VantagePoint(Matrix4x4 inMatrix) {
            RotationValue = Quaternion.CreateFromRotationMatrix(inMatrix);
            TranslationValue = inMatrix.Translation;
        }

        public VantagePoint(Vector3 inTranslation, Quaternion inRotation) {
            RotationValue = inRotation;
            TranslationValue = inTranslation;
        }

        #region Interop Interfaces
        [PInteropField]
        static public readonly VantagePoint IDENTITY = new VantagePoint(Vector3.Zero, Quaternion.Identity);

        [PInteropMethod]
        static public double evaluateHermiteCubic(double x, double y, double z, double w, Portion t) {
            return EvaluateCubic(s_HermiteCubic, x, y, z, w, t.Value);
        }

        [PInteropMethod]
        static public double evaluateHermiteCubicDeritive(double x, double y, double z, double w, Portion t) {
            return EvaluateCubicDerivative(s_HermiteCubic, x, y, z, w, t.Value);
        }

        [PInteropField]
        public Orientation orientation { get { return new Orientation(RotationValue); } }
        [PInteropField]
        public Position position { get { return new Position(TranslationValue); } }

        [PInteropConstructor]
        public VantagePoint(Position position, Orientation orientation) {
            TranslationValue = position.Value;
            RotationValue = orientation.Value;
        }

        [PInteropConstructor]
        public VantagePoint(Position position) {
            RotationValue = Quaternion.Identity;
            TranslationValue = position.Value;
        }

        [PInteropConstructor]
        public VantagePoint(Orientation orientation) {
            RotationValue = orientation.Value;
            TranslationValue = Vector3.Zero;
        }

        [PInteropMethod]
        public bool equals(VantagePoint other) {
            return TranslationValue == other.TranslationValue && RotationValue == other.RotationValue;
        }

        [PInteropMethod]
        public VantagePoint multiply(VantagePoint other) {
            
            //var result = Matrix4x4.Multiply(other.GetMatrix(), GetMatrix());
            //return new VantagePoint(result);
            var rot = Quaternion.Multiply(RotationValue, other.RotationValue);
            var pos = Vector3.Transform(other.TranslationValue, RotationValue) + TranslationValue;
            return new VantagePoint(pos, rot);
        }

        [PInteropMethod]
        public VantagePoint inverse() {
            //Matrix4x4 result;
            //Matrix4x4.Invert(GetMatrix(), out result);
            //return new VantagePoint(result);
            var rot = Quaternion.Inverse(RotationValue);
            var pos = Vector3.Transform(Vector3.Negate(TranslationValue), rot);
            return new VantagePoint(pos, rot);
        }

        [PInteropMethod]
        public VantagePoint interpolatePortion(VantagePoint end, Portion portion) {
            return new VantagePoint(Vector3.Lerp(TranslationValue, end.TranslationValue, portion.Value),
                                    Quaternion.Slerp(RotationValue, end.RotationValue, portion.Value));
        }
        #endregion // interop interfaces

        public override string ToString() {
            return string.Format("VantagePoint({0},{1})", TranslationValue.ToString(), RotationValue.ToString());
        }

        public override bool Equals(object obj) {
            if (obj is VantagePoint) {
                return equals((VantagePoint)obj);
            }
            return false;
        }

        public override int GetHashCode() {
            return TranslationValue.GetHashCode() + RotationValue.GetHashCode();
        }
    }
    
}

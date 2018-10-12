using System.Diagnostics;
using Alice.Tweedle.Interop;

namespace Alice.Player.Primitives
{
    [PInteropType]
    public sealed class VantagePoint {
        
        public readonly Quaternion RotationValue;
        public readonly Vector3 TranslationValue;

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
        public static readonly VantagePoint IDENTITY = new VantagePoint(Vector3.Zero, Quaternion.Identity);

        [PInteropField]
        public Orientation orientation { get { return new Orientation(RotationValue); } }
        [PInteropField]
        public Position position { get { return new Position(TranslationValue); } }

        [PInteropConstructor]
        public VantagePoint() {}

        [PInteropConstructor]
        public VantagePoint(Position position, Orientation orientation) {
            TranslationValue = position.Value;
            RotationValue = orientation.Value;
        }

        [PInteropConstructor]
        public VantagePoint(Orientation orientation) {
            RotationValue = orientation.Value;
        }

        [PInteropMethod]
        public bool equals(VantagePoint other) {
            return TranslationValue == other.TranslationValue && RotationValue == other.RotationValue;
        }

        [PInteropMethod]
        public VantagePoint multiply(VantagePoint other) {
            /* 
            var m1 = Matrix4x4.CreateFromQuaternion(RotationValue);
            m1.Translation = TranslationValue;

            var m2 = Matrix4x4.CreateFromQuaternion(other.RotationValue);
            m2.Translation = other.TranslationValue;

            var result = Matrix4x4.Multiply(m1, m2);
            return new VantagePoint(result.Translation, Quaternion.CreateFromRotationMatrix(result));
            */
            return new VantagePoint(Vector3.Transform(other.TranslationValue, RotationValue) + TranslationValue ,
                                    Quaternion.Multiply(other.RotationValue, RotationValue));
        }

        [PInteropMethod]
        public VantagePoint inverse() {
            return new VantagePoint(Vector3.Negate(TranslationValue), Quaternion.Inverse(RotationValue));
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

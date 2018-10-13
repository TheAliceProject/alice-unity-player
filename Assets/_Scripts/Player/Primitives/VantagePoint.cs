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

using Alice.Tweedle.Interop;
using Alice.Player.Primitives;
using Alice.Tweedle;

namespace Alice.Player.Modules {
    [PInteropType]
    public class SizeProperty : PropertyBase<Size> {

        #region Interop Interfaces
        [PInteropConstructor]
        public SizeProperty(Size value) : base(value) {}

        [PInteropMethod]
        public AsyncReturn animateWidth(double width, DimensionPolicyEnum dimensionPolicy, double duration, AnimationStyleEnum animationStyle) {
            return AnimateAxis(width, 0, dimensionPolicy, duration, animationStyle);
        }

        [PInteropMethod]
        public AsyncReturn animateHeight(double height, DimensionPolicyEnum dimensionPolicy, double duration, AnimationStyleEnum animationStyle) {
            return AnimateAxis(height, 1, dimensionPolicy, duration, animationStyle);
        }

        [PInteropMethod]
        public AsyncReturn animateDepth(double depth, DimensionPolicyEnum dimensionPolicy, double duration, AnimationStyleEnum animationStyle) {
            return AnimateAxis(depth, 2, dimensionPolicy, duration, animationStyle);
        }
        #endregion //Interop Interfaces



        public override Size Interpolate(Size a, Size b, double t) {
            return a.interpolatePortion(b, t);
        }

        private AsyncReturn AnimateAxis(double inValue, int inAxis, DimensionPolicyEnum inPolicy, double inDuration, AnimationStyleEnum inStyle) {
            double factor = inValue / m_Value.Value[inAxis];

            Vector3 endValue = m_Value.Value;
            endValue[inAxis] = inValue;

            switch (inPolicy) {
                case DimensionPolicyEnum.PRESERVE_ASPECT_RATIO:
                    endValue[(inAxis + 1)%3] *= factor;
                    endValue[(inAxis + 2)%3] *= factor;
                    break;
                case DimensionPolicyEnum.PRESERVE_VOLUME:
                    double squash = 1.0 / System.Math.Sqrt( factor );
                    endValue[(inAxis + 1)%3] *= squash;
                    endValue[(inAxis + 2)%3] *= squash;
                    break;
            }
            
            return animateValue(new Size(endValue), inDuration, inStyle);
        }

        
    }
}
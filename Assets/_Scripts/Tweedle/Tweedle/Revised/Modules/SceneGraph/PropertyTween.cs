using Alice.Tweedle.Interop;
using Alice.Tweedle.Modules;

namespace Alice.Unity {
    

    public sealed class PropertyTween<T> : IPropertyTween {
        public delegate void FinishedDelegate();

        public readonly PropertyBase<T> Property;
        public readonly T StartValue;
        public readonly T EndValue;
        public readonly AnimationStyleEnum Style;
        public readonly double Duration;
        public double Time { get; private set; }
        
        private FinishedDelegate m_OnFinished;

        public PropertyTween(PropertyBase<T> inProp, T inStart, T inEnd, double inDuration, AnimationStyleEnum inStyle, FinishedDelegate inEndedDelegate = null) {
            Property = inProp;
            StartValue = inStart;
            EndValue = inEnd;
            Duration = inDuration;
            Style = inStyle;
            Time = 0;
            m_OnFinished = inEndedDelegate;
        }

        public void Step(double dt) {
            Property.setValue(Property.Interpolate(StartValue, EndValue, CalculatePortion(Time, Duration, Style)));
            UnityEngine.Debug.Log(Property.getValue().ToString());
            Time += dt;
        }

        public bool IsDone() {
            return Time >= Duration;
        }

        public void Finish() {
            Property.setValue(EndValue);
            UnityEngine.Debug.Log(Property.getValue().ToString());
            if (m_OnFinished != null) {
                m_OnFinished.Invoke();
            }
        }

        // Ported from TraditionalStyle java enum  
        private static double Gently( double x, double A, double B ) {
            double y, a3, b3, c3, m, b2;
            if( x < A ) {
                y = ( ( B - 1 ) / ( A * ( ( ( ( B * B ) - ( A * B ) ) + A ) - 1 ) ) ) * x * x;
            } else if( x > B ) {
                a3 = 1 / ( ( ( ( B * B ) - ( A * B ) ) + A ) - 1 );
                b3 = -2 * a3;
                c3 = 1 + a3;
                y = ( a3 * x * x ) + ( b3 * x ) + c3;
            } else {
                m = ( 2 * ( B - 1 ) ) / ( ( ( ( B * B ) - ( A * B ) ) + A ) - 1 );
                b2 = ( -m * A ) / 2;
                y = ( m * x ) + b2;
            }
            return y;
        }

        private static double CalculatePortion( double timeElapsed, double timeTotal, AnimationStyleEnum style) {
            if( timeTotal != 0 ) {
                double portion = System.Math.Min(timeElapsed/timeTotal, 1);
                bool isSlowInDesired = (style & AnimationStyleEnum.BEGIN_GENTLY_AND_END_ABRUPTLY) == AnimationStyleEnum.BEGIN_GENTLY_AND_END_ABRUPTLY;
                bool isSlowOutDesired = (style & AnimationStyleEnum.BEGIN_ABRUPTLY_AND_END_GENTLY) == AnimationStyleEnum.BEGIN_ABRUPTLY_AND_END_GENTLY;

                if( isSlowInDesired ) {
                    if(isSlowOutDesired ) {
                        return Gently( portion, 0.3, 0.8 );
                    } else {
                        return Gently( portion, 0.99, 0.999 );
                    }
                } else {
                    if( isSlowOutDesired ) {
                        return Gently( portion, 0.001, 0.01 );
                    } else {
                        return portion;
                    }
                }
            }
            else {
                return 1;
            }
        }
    }
}
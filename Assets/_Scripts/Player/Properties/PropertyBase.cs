using Alice.Tweedle.Interop;
using Alice.Player.Unity;
using Alice.Tweedle;

namespace Alice.Player.Modules {

    public abstract class PropertyBase<T> {
        public delegate void ValueChangedDelegate(PropertyBase<T> inProperty);

        protected TValue m_Owner;
        protected T m_Value;

        public bool IsAnimating { get; protected set; }

        public event ValueChangedDelegate OnValueChanged;
        
        public PropertyBase(TValue inOwner, T inValue) {
            m_Owner = inOwner;
            m_Value = inValue;
        }

        #region Interop Interfaces
        [PInteropMethod]
        public T getValue() {
            return m_Value;
        }

        [PInteropMethod]
        public void setValue(T value) {
            if (!m_Value.Equals(value)) {
                m_Value = value;
                if (OnValueChanged != null) {
                    OnValueChanged.Invoke(this);
                }
            }
        }

        // TODO: return blocking mechanism to VM
        [PInteropMethod]
        public void animateValue(T endValue, double duration, AnimationStyleEnum animationStyle) {
            if (!IsAnimating) {
                if (duration <= 0) {
                    setValue(endValue);
                    FinishAnimation();
                    return;
                }
                
                IsAnimating = true;
                var tween = new PropertyTween<T>(this, m_Value, endValue, duration, animationStyle, FinishAnimation);
                UnitySceneGraph.Current.QueueTween(tween);
            }
        }
        #endregion // Interop Interfaces

        // TODO: unblock current executition step
        protected void FinishAnimation() {
            IsAnimating = false;
        }

        public abstract T Interpolate(T a, T b, double t);
    }
}
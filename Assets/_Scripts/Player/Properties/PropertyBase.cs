using Alice.Tweedle.Interop;
using Alice.Player.Unity;
using Alice.Tweedle;

namespace Alice.Player.Modules {
    

    public abstract class PropertyBase<T>  {
        
        public delegate void ValueChangedDelegate(PropertyBase<T> inProperty);
      

    
        public bool IsAnimating { get {return m_AnimationCount > 0;} }

        public event ValueChangedDelegate OnValueChanged;

        protected T m_Value;
        private int m_AnimationCount = 0; 

        public PropertyBase(T inValue) {
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
        public AsyncReturn animateValue(T endValue, double duration, AnimationStyleEnum animationStyle) {

            if (duration <= 0) {
                setValue(endValue);
                return null;
            }
            
            m_AnimationCount++;
            
            var asyncReturn = new AsyncReturn();
            var tween = new PropertyTween<T>(this, m_Value, endValue, duration, animationStyle, asyncReturn);
            UnitySceneGraph.Current.QueueTween(tween);
            return asyncReturn;
        }
        #endregion // Interop Interfaces

        internal void FinishAnimation() {
            m_AnimationCount--;
        }

        public abstract T Interpolate(T a, T b, double t);
    }
}
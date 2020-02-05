using System;

namespace Alice.Tweedle.Interop
{
    public interface IAsyncReturn
    {
        bool HasReturned { get; }
        void Return(object inResult);
        void OnReturn(Action<object> inCallback);
    }

    public sealed class AsyncReturn : IAsyncReturn
    {
        private Action<object> m_Callback;
        private bool m_Returned;

        public bool HasReturned
        {
            get { return m_Returned; }
        }

        public void Return()
        {
            if (m_Returned)
            {
                UnityEngine.Debug.LogError("AsyncResult has already returned.");
                return;
            }

            m_Returned = true;
            m_Callback?.Invoke(null);
        }

        void IAsyncReturn.Return(object inObject)
        {
            Return();
        }

        public void OnReturn(Action inCallback)
        {
            if (m_Returned)
                inCallback();
            else
                m_Callback += (o) => inCallback();
        }

        void IAsyncReturn.OnReturn(Action<object> inCallback)
        {
            if (m_Returned)
                inCallback(null);
            else
                m_Callback += inCallback;
        }
    }

    public sealed class AsyncReturn<T> : IAsyncReturn
    {
        private Action<T> m_Callback;
        private bool m_Returned;
        private T m_Result;

        public bool HasReturned
        {
            get { return m_Returned; }
        }

        void IAsyncReturn.Return(object inObject)
        {
            Return((T)inObject);
        }

        public void Return(T inValue)
        {
            if (m_Returned)
            {
                throw new TweedleRuntimeException("AsyncResult has already returned.");
            }

            m_Returned = true;
            m_Result = inValue;
            m_Callback?.Invoke(inValue);
        }

        public void OnReturn(Action<T> inCallback)
        {
            if (m_Returned)
                inCallback(m_Result);
            else
                m_Callback += inCallback;
        }

        void IAsyncReturn.OnReturn(Action<object> inCallback)
        {
            if (m_Returned)
                inCallback(m_Result);
            else
                m_Callback += (val) => inCallback(val);
        }
    }
}
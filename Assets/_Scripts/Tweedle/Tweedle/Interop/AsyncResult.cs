using System;
using System.Collections.Generic;
using System.Reflection;

namespace Alice.Tweedle.Interop
{
    public interface IAsyncReturn
    {
        void Return(object inResult);
        void OnReturn(Action<object> inCallback);
    }

    public sealed class AsyncReturn : IAsyncReturn
    {
        private Action<object> m_Callback;
        private bool m_Returned;

        public void Return()
        {
            if (m_Returned)
            {
                throw new TweedleRuntimeException("AsyncResult has already returned.");
            }

            m_Returned = true;
            m_Callback?.Invoke(null);
        }

        void IAsyncReturn.Return(object inObject)
        {
            Return();
        }

        public void OnReturn(Action<object> inCallback)
        {
            if (m_Returned)
                inCallback(null);
            else
                m_Callback += inCallback;
        }
    }

    public sealed class AsyncReturn<T> : IAsyncReturn
    {
        private Action<object> m_Callback;
        private bool m_Returned;
        private T m_Result;

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

        public void OnReturn(Action<object> inCallback)
        {
            if (m_Returned)
                inCallback(m_Result);
            else
                m_Callback += inCallback;
        }
    }
}
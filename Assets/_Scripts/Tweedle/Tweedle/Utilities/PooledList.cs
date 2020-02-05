using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Alice.Utils
{
    public sealed class PooledList<T> : List<T>, IDisposable
    {
        private bool m_InUse = false;

        private void OnAlloc()
        {
            Debug.Assert(!m_InUse);

            m_InUse = true;
        }

        void IDisposable.Dispose()
        {
            Debug.Assert(m_InUse);

            Clear();
            s_Pool.Push(this);
            m_InUse = false;
        }

        #region Pool

        static private Stack<PooledList<T>> s_Pool = new Stack<PooledList<T>>(64);

        static public PooledList<T> Alloc()
        {
            PooledList<T> list = null;
            if (s_Pool.Count > 0)
                list = s_Pool.Pop();
            else
                list = new PooledList<T>();
            list.OnAlloc();

            return list;
        }

        static public PooledList<T> Alloc(IEnumerable<T> inCopy)
        {
            PooledList<T> list = Alloc();
            list.AddRange(inCopy);
            return list;
        }

        #endregion // Pool
    }
}
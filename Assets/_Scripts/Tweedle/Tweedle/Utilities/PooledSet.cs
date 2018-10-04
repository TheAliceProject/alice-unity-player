using System;
using System.Collections.Generic;

namespace Alice.Utils
{
    /// <summary>
    /// Pooled version of a Set.
    /// </summary>
    public class PooledSet<T> : HashSet<T>, IDisposable
    {
        private void Reset()
        {
            Clear();
        }

        /// <summary>
        /// Resets and recycles the PooledList to the pool.
        /// </summary>
        public void Dispose()
        {
            Reset();
            s_ObjectPool.Push(this);
        }

        #region Pool

        // Maximum number to hold in pool at a time.
        private const int POOL_SIZE = 8;

        // Object pool to hold available PooledSet.
        static private Stack<PooledSet<T>> s_ObjectPool = new Stack<PooledSet<T>>(POOL_SIZE);

        /// <summary>
        /// Retrieves a PooledList for use.
        /// </summary>
        static public PooledSet<T> Alloc()
        {
            if (s_ObjectPool.Count == 0) {
                return new PooledSet<T>();
            } 

            return s_ObjectPool.Pop();
        }

        /// <summary>
        /// Retrieves a PooledSet for use, copying the contents
        /// of the given IEnumerable.
        /// </summary>
        static public PooledSet<T> Alloc(IEnumerable<T> inToCopy)
        {
            PooledSet<T> set = (s_ObjectPool.Count == 0) ? new PooledSet<T>() : s_ObjectPool.Pop();
            foreach (var obj in inToCopy)
                set.Add(obj);
            return set;
        }

        #endregion
    }
}

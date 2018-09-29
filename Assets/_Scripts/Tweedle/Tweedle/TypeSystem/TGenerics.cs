using System;
using System.Collections.Generic;
using Alice.Utils;

namespace Alice.Tweedle
{
    // Handles generic TType specializations
    static public class TGenerics
    {
        #region Arrays

        // Arrays
        static private Dictionary<string, TArrayType> s_ArraySpecializationMap = new Dictionary<string, TArrayType>();

        /// <summary>
        /// Returns the array type for the given element type.
        /// </summary>
        static public TArrayType GetArrayType(TTypeRef inElementType, TAssembly inForAssembly)
        {
            TArrayType arrayType;
            if (!s_ArraySpecializationMap.TryGetValue(inElementType.Name, out arrayType))
            {
                arrayType = new TArrayType(inElementType);
                s_ArraySpecializationMap.Add(inElementType.Name, arrayType);
                inForAssembly?.Add(arrayType);
                // UnityEngine.Debug.Log("[TGenerics] Generated array type " + arrayType);
            }

            return arrayType;
        }

        static private void UnloadArray(TType inElementType)
        {
            TArrayType arrayType;
            if (s_ArraySpecializationMap.TryGetValue(inElementType.Name, out arrayType))
            {
                // UnityEngine.Debug.Log("[TGenerics] Unloaded array type " + arrayType);
                s_ArraySpecializationMap.Remove(inElementType.Name);
                Unload(arrayType);
            }
        }

        #endregion // Arrays

        static public void Unload(TType inType)
        {
            UnloadArray(inType);
        }

        static public void Reset()
        {
            s_ArraySpecializationMap.Clear();
        }
    }
}

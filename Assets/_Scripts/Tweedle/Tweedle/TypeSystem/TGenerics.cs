using System;
using System.Collections.Generic;
using Alice.Utils;

namespace Alice.Tweedle
{
    // Handles generic TType specializations
    static public class TGenerics
    {
        // List of all generated types
        static private List<TType> s_GeneratedTypes = new List<TType>(512);

        static private void UnloadType(TType inType, bool inbCheck)
        {
            if (inbCheck && !s_GeneratedTypes.Remove(inType))
            {
                return;
            }

            TArrayType arrType = inType as TArrayType;
            if (arrType != null)
            {
                UnloadArray(arrType.ElementType);
                return;
            }

            TLambdaType lambdaType = inType as TLambdaType;
            if (lambdaType != null)
            {
                UnloadLambda(lambdaType.Signature);
            }
        }

        #region Arrays

        // Map
        static private Dictionary<string, TArrayType> s_ArraySpecializationMap = new Dictionary<string, TArrayType>();

        /// <summary>
        /// Returns the array type for the given element type.
        /// </summary>
        static public TArrayType GetArrayType(TTypeRef inElementType, TAssembly inForAssembly)
        {
            TArrayType arrayType;
            if (!s_ArraySpecializationMap.TryGetValue(inElementType.Name, out arrayType))
            {
                arrayType = new TArrayType(inForAssembly, inElementType);
                s_ArraySpecializationMap.Add(inElementType.Name, arrayType);
                s_GeneratedTypes.Add(arrayType);
                inForAssembly?.Add(arrayType);
            }

            return arrayType;
        }

        static private void UnloadArray(TType inElementType)
        {
            s_ArraySpecializationMap.Remove(inElementType.Name);
        }

        #endregion // Arrays

        #region Lambdas

        // Map
        static private Dictionary<string, TLambdaType> s_LambdaSpecializationMap = new Dictionary<string, TLambdaType>();

        /// <summary>
        /// Returns the lambda type for the given signature.
        /// </summary>
        static public TLambdaType GetLambdaType(TLambdaSignature inSignature, TAssembly inForAssembly)
        {
            TLambdaType lambdaType;
            if (!s_LambdaSpecializationMap.TryGetValue(inSignature.Name, out lambdaType))
            {
                lambdaType = new TLambdaType(inForAssembly, inSignature);
                s_LambdaSpecializationMap.Add(inSignature.Name, lambdaType);
                s_GeneratedTypes.Add(lambdaType);
                inForAssembly?.Add(lambdaType);
            }

            return lambdaType;
        }

        static private void UnloadLambda(TLambdaSignature inSignature)
        {
            s_LambdaSpecializationMap.Remove(inSignature.Name);
        }

        #endregion // Lambdas

        /// <summary>
        /// Unloads all generic types from the given assembly.
        /// </summary>
        static public void Unload(TAssembly inAssembly)
        {
            for (int i = s_GeneratedTypes.Count - 1; i >= 0; --i)
            {
                TType generatedType = s_GeneratedTypes[i];
                if (generatedType.Assembly == inAssembly)
                {
                    s_GeneratedTypes.RemoveAt(i);
                    UnloadType(generatedType, false);
                }
            }
        }

        /// <summary>
        /// Unloads the given generic type.
        /// </summary>
        static public void Unload(TType inType)
        {
            UnloadType(inType, true);
        }

        /// <summary>
        /// Unloads all generic types from all unloadable assemblies.
        /// </summary>
        static public void Reset()
        {
            for (int i = s_GeneratedTypes.Count - 1; i >= 0; --i)
            {
                TType generatedType = s_GeneratedTypes[i];
                if (generatedType.Assembly == null || (generatedType.Assembly.Flags & TAssemblyFlags.CannotUnload) == 0)
                {
                    s_GeneratedTypes.RemoveAt(i);
                    UnloadType(generatedType, false);
                }
            }
        }
    }
}

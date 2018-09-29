using System;
using System.Collections.Generic;
using System.Reflection;
using Alice.Tweedle.Parse;

namespace Alice.Tweedle.Interop
{
    static public class TInterop
    {
        static private IntPtr GetTypePtr<T>() { return typeof(T).TypeHandle.Value; }

        // Basic conversions
        static private readonly IntPtr TYPEPTR_TVALUE = GetTypePtr<TValue>();

        static private readonly IntPtr TYPEPTR_VOID = typeof(void).TypeHandle.Value;
        static private readonly IntPtr TYPEPTR_INT = GetTypePtr<int>();
        static private readonly IntPtr TYPEPTR_FLOAT = GetTypePtr<float>();
        static private readonly IntPtr TYPEPTR_DOUBLE = GetTypePtr<double>();
        static private readonly IntPtr TYPEPTR_STRING = GetTypePtr<string>();
        static private readonly IntPtr TYPEPTR_BOOLEAN = GetTypePtr<bool>();

        // Async types
        static private readonly Type TYPE_IASYNCRETURN = typeof(IAsyncReturn);

        #region To TValue

        static public TValue ToTValue(object inObject, TweedleSystem inLibrary)
        {
            if (inObject == null)
            {
                return TValue.NULL;
            }

            Type type = inObject.GetType();
            IntPtr typePtr = type.TypeHandle.Value;
            TValue retVal;

            if (!TryConvertConstant(typePtr, inObject, out retVal))
            {
                TType ttype = inLibrary.TypeNamed(InteropTypeName(type));
                if (ttype != null)
                {
                    retVal = TValue.FromObject(ttype, inObject);
                }
                else
                {
                    throw new TweedleRuntimeException("Unable to convert from type " + type.Name + " to a TType");
                }
            }

            return retVal;
        }

        static public TValue ToTValue(object inObject)
        {
            if (inObject == null)
                return TValue.NULL;

            Type type = inObject.GetType();
            IntPtr typePtr = type.TypeHandle.Value;
            TValue retVal;

            if (!TryConvertConstant(typePtr, inObject, out retVal))
            {
                throw new TweedleRuntimeException("Unable to convert from type " + type.Name + " to a TType");
            }

            return retVal;
        }

        static private bool TryConvertConstant(IntPtr inTypeHandle, object inObject, out TValue outValue)
        {
            if (inTypeHandle == TYPEPTR_INT)
            {
                outValue = ToTValue((int)inObject);
                return true;
            }
            else if (inTypeHandle == TYPEPTR_FLOAT)
            {
                outValue = ToTValue((double)(float)inObject);
                return true;
            }
            else if (inTypeHandle == TYPEPTR_DOUBLE)
            {
                outValue = ToTValue((double)inObject);
                return true;
            }
            else if (inTypeHandle == TYPEPTR_STRING)
            {
                outValue = ToTValue((string)inObject);
                return true;
            }
            else if (inTypeHandle == TYPEPTR_BOOLEAN)
            {
                outValue = ToTValue((bool)inObject);
                return true;
            }
            else if (inTypeHandle == TYPEPTR_TVALUE)
            {
                outValue = (TValue)inObject;
                return true;
            }
            else
            {
                outValue = TValue.UNDEFINED;
                return false;
            }
        }

        static public TValue ToTValue(int inValue)
        {
            return TValue.FromInt(inValue);
        }

        static public TValue ToTValue(double inValue)
        {
            return TValue.FromNumber(inValue);
        }

        static public TValue ToTValue(bool inbValue)
        {
            return TValue.FromBoolean(inbValue);
        }

        static public TValue ToTValue(string inValue)
        {
            return TValue.FromString(inValue);
        }

        #endregion // To TValue

        #region To PObject

        static public object ToPObject(TValue inValue)
        {
            return inValue.ToPObject();
        }

        static public object ToPObject(TValue inValue, Type inType)
        {
            IntPtr typePtr = inType.TypeHandle.Value;
            if (typePtr == TYPEPTR_TVALUE)
            {
                return (object)inValue;
            }

            object obj = inValue.ToPObject();
            if (obj != null && obj.GetType().TypeHandle.Value != typePtr)
            {
                obj = Convert.ChangeType(obj, inType);
            }
            return obj;
        }

        #endregion // To PObject
    
        #region Types

        // static public TTypeRef TTypeFor(object inObject)
        // {
        //     if (inObject == null)
        //         return TBuiltInTypes.NULL;
        //     if (inObject is TValue)
        //         return ((TValue)inObject).Type;

        //     return TTypeFor(inObject.GetType());
        // }

        static public TTypeRef TTypeFor(Type inType, TAssembly inAssembly)
        {
            if (inType == null)
                return null;

            if (inType.IsArray)
            {
                Type elementType = inType.GetElementType();
                TTypeRef elementTypeRef = TTypeFor(elementType, inAssembly);
                return TGenerics.GetArrayType(elementTypeRef, inAssembly);
            }

            IntPtr typePtr = inType.TypeHandle.Value;
            if (typePtr == TYPEPTR_VOID)
            {
                return TBuiltInTypes.VOID;
            }
            if (typePtr == TYPEPTR_INT)
            {
                return TBuiltInTypes.WHOLE_NUMBER;
            }
            else if (typePtr == TYPEPTR_DOUBLE || typePtr == TYPEPTR_FLOAT)
            {
                return TBuiltInTypes.DECIMAL_NUMBER;
            }
            else if (typePtr == TYPEPTR_STRING)
            {
                return TBuiltInTypes.TEXT_STRING;
            }
            else if (typePtr == TYPEPTR_BOOLEAN)
            {
                return TBuiltInTypes.BOOLEAN;
            }
            else if (typePtr == TYPEPTR_TVALUE)
            {
                return TBuiltInTypes.ANY;
            }
            else
            {
                string typeName = InteropTypeName(inType);
                if (!string.IsNullOrEmpty(typeName))
                {
                    TType existingType = inAssembly?.TypeNamed(typeName);
                    if (existingType != null)
                        return existingType;
                    return new TTypeRef(typeName);
                }

                return null;
            }
        }

        static public string InteropTypeName(Type inType)
        {
            return PInteropTypeAttribute.GetTweedleName(inType);
        }

        #endregion // Types
    
        #region Types

        static public TType GenerateType(TAssembly inAssembly, Type inType)
        {
            if (inType.IsClass)
                return new TPObjectType(inAssembly, inType);
            else if (inType.IsEnum)
                return new TPEnumType(inAssembly, inType);
            else
                throw new Exception("Unable to convert type " + inType.Name + " to a tweedle type");
        }

        static public TField[] GenerateFields(TAssembly inAssembly, Type inType)
        {
            List<TField> tFields = new List<TField>();
            
            var pFields = inType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            foreach(var pField in pFields)
            {
                // UnityEngine.Debug.Log("Parsing field " + pField.Name);
                if (PInteropFieldAttribute.IsDefined(pField))
                    tFields.Add(new PField(inAssembly, pField));
            }

            var pProps = inType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            foreach(var pProp in pProps)
            {
                if (PInteropFieldAttribute.IsDefined(pProp))
                    tFields.Add(new PProperty(inAssembly, pProp));
            }

            return tFields.ToArray();
        }

        static public TMethod[] GenerateMethods(TAssembly inAssembly, Type inType)
        {
            List<TMethod> tMethods = new List<TMethod>();

            var pMethods = inType.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            foreach(var pMethod in pMethods)
            {
                if (PInteropMethodAttribute.IsDefined(pMethod))
                {
                    PMethodBase tMethod;
                    if (TYPE_IASYNCRETURN.IsAssignableFrom(pMethod.ReturnType))
                        tMethod = new PAsyncMethod(inAssembly, pMethod);
                    else
                        tMethod = new PMethod(inAssembly, pMethod);
                    tMethods.Add(tMethod);
                }
            }

            return tMethods.ToArray();
        }

        static public TMethod[] GenerateConstructors(TAssembly inAssembly, Type inType)
        {
            List<TMethod> tConstructors = new List<TMethod>();

            var pConstructors = inType.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach(var pConstructor in pConstructors)
            {
                if (PInteropConstructorAttribute.IsDefined(pConstructor))
                    tConstructors.Add(new PConstructor(inAssembly, pConstructor));
            }

            return tConstructors.ToArray();
        }

        #endregion // Members
    }
}
using System;
using Alice.Tweedle.Parse;

namespace Alice.Tweedle.Interop
{
    static public class TConvert
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
                TType ttype = inLibrary.TypeNamed(TTypeNameForType(type));
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

        static public TTypeRef TTypeFor(object inObject)
        {
            if (inObject == null)
                return TStaticTypes.NULL;
            if (inObject is TValue)
                return ((TValue)inObject).Type;

            return TTypeFor(inObject.GetType());
        }

        static public TTypeRef TTypeFor(Type inType)
        {
            if (inType == null)
                return null;

            IntPtr typePtr = inType.TypeHandle.Value;
            if (typePtr == TYPEPTR_VOID)
            {
                return TStaticTypes.VOID;
            }
            if (typePtr == TYPEPTR_INT)
            {
                return TStaticTypes.WHOLE_NUMBER;
            }
            else if (typePtr == TYPEPTR_DOUBLE || typePtr == TYPEPTR_FLOAT)
            {
                return TStaticTypes.DECIMAL_NUMBER;
            }
            else if (typePtr == TYPEPTR_STRING)
            {
                return TStaticTypes.TEXT_STRING;
            }
            else if (typePtr == TYPEPTR_BOOLEAN)
            {
                return TStaticTypes.BOOLEAN;
            }
            else if (typePtr == TYPEPTR_TVALUE)
            {
                return TStaticTypes.ANY;
            }
            else
            {
                return new TTypeRef(TTypeNameForType(inType));
            }
        }

        static public string TTypeNameForType(Type inType)
        {
            return PInteropTypeAttribute.GetTweedleName(inType);
        }

        #endregion // Types
    }
}
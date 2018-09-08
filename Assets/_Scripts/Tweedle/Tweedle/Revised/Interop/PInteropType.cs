using System;
using System.Collections.Generic;
using System.Reflection;

namespace Alice.Tweedle.Interop
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PInteropTypeAttribute : Attribute
    {
        public readonly string TweedleName;

        public PInteropTypeAttribute(string inTweedleName = null)
        {
            TweedleName = inTweedleName;
        }

        static private readonly Dictionary<IntPtr, string> s_TypeNames = new Dictionary<IntPtr, string>();

        static public bool IsDefined(Type inType)
        {
            return Attribute.IsDefined(inType, typeof(PInteropTypeAttribute));
        }

        static public string GetTweedleName(Type inType)
        {
            IntPtr typePtr = inType.TypeHandle.Value;
            string typeName;
            if (!s_TypeNames.TryGetValue(typePtr, out typeName))
            {
                PInteropTypeAttribute attr = (PInteropTypeAttribute)inType.GetCustomAttribute(typeof(PInteropTypeAttribute));
                if (attr != null)
                {
                    typeName = attr.TweedleName ?? inType.Name;
                    if (inType.IsAbstract && inType.IsSealed)
                    {
                        typeName = "$" + typeName;
                    }
                }

                s_TypeNames.Add(typePtr, typeName);
            }
            return typeName;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class PInteropFieldAttribute : Attribute
    {
        static public bool IsDefined(FieldInfo inFieldInfo)
        {
            return inFieldInfo.GetCustomAttribute(typeof(PInteropFieldAttribute)) != null;
        }

        static public bool IsDefined(PropertyInfo inProperty)
        {
            return inProperty.GetCustomAttribute(typeof(PInteropFieldAttribute)) != null;
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class PInteropMethodAttribute : Attribute
    {
        static public bool IsDefined(MethodInfo inMethodInfo)
        {
            return inMethodInfo.GetCustomAttribute(typeof(PInteropMethodAttribute)) != null;
        }
    }

    [AttributeUsage(AttributeTargets.Constructor)]
    public class PInteropConstructorAttribute : Attribute
    {
        static public bool IsDefined(ConstructorInfo inConstructorInfo)
        {
            return inConstructorInfo.GetCustomAttribute(typeof(PInteropConstructorAttribute)) != null;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace Alice.Tweedle
{
    public abstract partial class TType : IComparable<TType>, ILinkable
    {
        #region Helper Methods

        /// <summary>
        /// Finds the member with the given name and flags.
        /// </summary>
        static protected T FindMember<T>(T[] inMembers, string inName, MemberFlags inFlags) where T : class, ITypeMember
        {
            for (int i = 0; i < inMembers.Length; ++i)
            {
                T member = inMembers[i];
                if (member.Name.Equals(inName, StringComparison.Ordinal))
                {
                    if ((member.Flags & inFlags) != inFlags)
                        return null;

                    return member;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds a method with the given name, arguments, and flags.
        /// </summary>
        static protected T FindMethodWithArgs<T>(T[] inMembers, string inName, NamedArgument[] inArguments, MemberFlags inFlags) where T : TMethod
        {
            for (int i = 0; i < inMembers.Length; ++i)
            {
                T method = inMembers[i];
                if (method.Name.Equals(inName, StringComparison.Ordinal)
                    && (method.Flags & inFlags) == inFlags
                    && method.ExpectsArgs(inArguments))
                    return method;
            }

            return null;
        }

        /// <summary>
        /// Resolves all members.
        /// </summary>
        static protected void LinkMembers<T>(T[] inMembers, TAssemblyLinkContext inContext, TType inType) where T : ITypeMember
        {
            for (int i = 0; i < inMembers.Length; ++i)
            {
                inMembers[i].Link(inContext, inType);
            }
        }

        /// <summary>
        /// Returns if a source type is assignable to the target type.
        /// </summary>
        static protected bool IsAssignableFrom(TType inTarget, TType inSource)
        {
            TType type = inSource;
            while(type != null)
            {
                if (type == inSource)
                    return true;
                type = inSource.SuperType.Get();
            }

            return inSource == TBuiltInTypes.ANY;
        }

        /// <summary>
        /// Returns if a value's type is within the given type hierarchy.
        /// </summary>
        static protected bool InstanceOf(ref TValue inValue, TType inType, bool inbAllowTypeRef = true)
        {
            TType type = inValue.Type;
            if (type == inType)
                return true;
            if (inbAllowTypeRef && type == TBuiltInTypes.TYPE_REF)
                type = inValue.TypeRef().Get();
            return IsAssignableFrom(inType, type);
        }

        /// <summary>
        /// Traverses the inheritance hierarchy of a type.
        /// </summary>
        static protected void TraverseInheritance(TType inType, List<TType> ioList)
        {
            ioList.Clear();
            TType type = inType;
            while(type != null)
            {
                ioList.Add(type);
                type = type.SuperType.Get();
            }
        }

        /// <summary>
        /// Calculates the depth of inheritance of a type.
        /// </summary>
        static protected int CalculateInheritanceDepth(TType inType)
        {
            int depth = 0;
            TType type = (TType)inType.SuperType;
            while(type != null)
            {
                ++depth;
                type = (TType)type.SuperType;
            }
            return depth;
        }

        #endregion // Helper Methods

        #region Debugging

        static public string DumpOutline(TType inType)
        {
            StringBuilder builder = new StringBuilder(1024);
            builder.Append("type ").Append(inType.Name);
            if (inType.SuperType != null)
            {
                builder.Append(" extends ").Append(inType.SuperType);
            }
            builder.Append(" (ID=" + inType.ID + ")");
            builder.Append(" {")
                .Append("\n");

            // Write out constructors
            builder.Append("\n\t//Fields");
            foreach(var field in inType.Fields(null))
            {
                builder.Append("\n\t");
                DumpField(builder, field);
            }

            // Write out constructors
            builder.Append("\n\n\t//Constructors");
            foreach(var constructor in inType.Constructors(null))
            {
                builder.Append("\n\t");
                DumpMethod(builder, constructor);
            }

            // Write out constructors
            builder.Append("\n\n\t//Methods");
            foreach(var method in inType.Methods(null))
            {
                builder.Append("\n\t");
                DumpMethod(builder, method);
            }

            builder.Append("\n}");

            return builder.ToString();
        }

        static private void DumpMethod(StringBuilder inBuilder, TMethod inMethod)
        {
            if (inMethod.IsStatic())
                inBuilder.Append("static ");
            inBuilder.Append(inMethod.ReturnType).Append(" ").Append(inMethod.Name).Append('(');
            int paramCount = 0;
            foreach(var requiredParam in inMethod.RequiredParams)
            {
                if (paramCount > 0)
                    inBuilder.Append(", ");
                inBuilder.Append(requiredParam.ToTweedle());
                ++paramCount;
            }
            foreach(var optionalParam in inMethod.OptionalParams)
            {
                if (paramCount > 0)
                    inBuilder.Append(", ");
                inBuilder.Append(optionalParam.ToTweedle());
                ++paramCount;
            }
            inBuilder.Append(") [").Append(inMethod.Flags).Append("];");
        }

        static private void DumpField(StringBuilder inBuilder, TField inField)
        {
            if (inField.IsStatic())
                inBuilder.Append("static ");
            inBuilder.Append(inField.ToTweedle());
            inBuilder.Append(" [").Append(inField.Flags).Append("];");
        }

        #endregion // Debugging
    }
}

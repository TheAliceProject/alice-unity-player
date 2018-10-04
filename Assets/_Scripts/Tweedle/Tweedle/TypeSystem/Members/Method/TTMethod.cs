using System.Collections.Generic;
using Alice.Tweedle.Parse;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// Invokable tweedle method.
    /// </summary>
    public class TTMethod : TMethod
    {
        public readonly BlockStatement Body;

        public TTMethod(string inName, MemberFlags inFlags, TTypeRef inResultType, TParameter[] inRequiredParams, TParameter[] inOptionalParams, TweedleStatement[] inStatements)
        {
            Body = new BlockStatement(inStatements);

            if ((inFlags & MemberFlags.Static) == 0)
                inFlags |= MemberFlags.Instance;

            SetupMember(inName, inResultType, inFlags);
            SetupSignature(inResultType, inRequiredParams, inOptionalParams);
        }

        protected override void AddBodyStep(InvocationScope inScope, StepSequence ioMain)
        {
            if (Body.Statements.Length > 0)
            {
                ioMain.AddStep(Body.AsSequentialStep(inScope));
            }
        }

        /// <summary>
        /// Generates a default, empty constructor for a TType.
        /// </summary>
        static public TTMethod DefaultConstructor(TTypeRef inResultType)
        {
            return new TTMethod(TMethod.ConstructorName, MemberFlags.Constructor, inResultType, TParameter.EMPTY_PARAMS, TParameter.EMPTY_PARAMS, TweedleStatement.EMPTY_STATEMENTS);
        }

        /// <summary>
        /// Generates a default, empty constructor for a TType, calling the super type.
        /// </summary>
        static public TTMethod DefaultConstructorWithSuper(TTypeRef inResultType)
        {
            return new TTMethod(TMethod.ConstructorName, MemberFlags.Constructor, inResultType, TParameter.EMPTY_PARAMS, TParameter.EMPTY_PARAMS, DEFAULT_SUPER_STATEMENT);
        }

        // Default super instantiation
        static private readonly TweedleStatement[] DEFAULT_SUPER_STATEMENT = new TweedleStatement[]
        {
            new ExpressionStatement(new SuperInstantiation(NamedArgument.EMPTY_ARGS))
        };
    }
}
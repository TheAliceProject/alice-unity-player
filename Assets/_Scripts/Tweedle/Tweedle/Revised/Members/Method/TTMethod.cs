using System.Collections.Generic;
using Alice.Tweedle.Parse;
using Alice.VM;

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
            ioMain.AddStep(Body.AsSequentialStep(inScope));
        }
    }
}
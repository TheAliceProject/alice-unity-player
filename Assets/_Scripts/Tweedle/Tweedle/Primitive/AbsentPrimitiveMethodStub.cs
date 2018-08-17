using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	internal class AbsentPrimitiveMethodStub : TweedleMethod
	{
		private static readonly List<TweedleRequiredParameter> NoRequiredParams = new List<TweedleRequiredParameter>();
		private static readonly List<TweedleOptionalParameter> NoOptionalParams = new List<TweedleOptionalParameter>();
		private static readonly List<TweedleStatement> EmptyBody = new List<TweedleStatement>();

		public AbsentPrimitiveMethodStub(string methodName)
			: base(TweedleVoidType.VOID, methodName, NoRequiredParams, NoOptionalParams, EmptyBody)
		{
		}

		protected internal override ExecutionStep AsStep(string callStack, InvocationScope scope, Dictionary<string, TweedleExpression> arguments)
		{
			UnityEngine.Debug.LogError("Attempt to invoke missing primitive method " + Name);
			return new ValueStep(callStack, scope, TweedleNull.NULL);
		}

		public override bool IsStatic()
		{
			return true;
		}
	}
}
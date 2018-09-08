using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	// internal class AbsentPrimitiveMethodStub : TMethod
	// {
	// 	private static readonly List<TParameter> NoRequiredParams = new List<TParameter>();
	// 	private static readonly List<TweedleOptionalParameter> NoOptionalParams = new List<TweedleOptionalParameter>();
	// 	private static readonly TweedleStatement[] EmptyBody = new TweedleStatement[0];

	// 	public AbsentPrimitiveMethodStub(string methodName)
	// 		: base(TweedleVoidType.VOID, methodName, NoRequiredParams, NoOptionalParams, EmptyBody)
	// 	{
	// 	}

	// 	protected override ExecutionStep AsStep(string callStack, InvocationScope scope, Dictionary<string, TweedleExpression> arguments)
	// 	{
	// 		UnityEngine.Debug.LogError("Attempt to invoke missing primitive method " + Name);
	// 		return new ValueStep(callStack, scope, TweedleNull.NULL);
	// 	}

	// 	public override bool IsStatic()
	// 	{
	// 		return true;
	// 	}
	// }
}
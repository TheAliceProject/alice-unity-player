using System;
using System.Collections.Generic;
using System.Linq;
using Alice.VM;

namespace Alice.Tweedle
{
	// public class LambdaExpression : TweedleExpression
	// {
	// 	List<TParameter> parameters;
	// 	BlockStatement body;

	// 	public List<TParameter> Parameters
	// 	{
	// 		get { return parameters; }
	// 	}

	// 	internal TweedleLambdaType LambdaType()
	// 	{
	// 		return (TweedleLambdaType)Type;
	// 	}

	// 	public BlockStatement Body
	// 	{
	// 		get { return body; }
	// 	}

	// 	// TODO Extract a better return type than void from the statements
	// 	public LambdaExpression(List<TParameter> parameters, TweedleStatement[] statements)
	// 		: base(new TweedleLambdaType(parameters.Select(param => param.Type).ToList(), TweedleVoidType.VOID))
	// 	{
	// 		this.parameters = parameters;
	// 		body = new BlockStatement(statements);
	// 	}

	// 	public override ExecutionStep AsStep(ExecutionScope scope)
	// 	{
	// 		return new ValueStep(ToTweedle(), scope, new TLambda(this));
	// 	}
	// }
}
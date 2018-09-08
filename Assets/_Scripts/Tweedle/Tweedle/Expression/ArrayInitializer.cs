using System.Collections.Generic;
using System.Linq;
using Alice.VM;

namespace Alice.Tweedle
{
	// public class ArrayInitializer : TweedleExpression
	// {
	// 	List<TweedleExpression> elements;
	// 	TweedleExpression initializeSize;

	// 	public ArrayInitializer(TweedleArrayType arrayType, List<TweedleExpression> elements)
	// 		: base(arrayType)
	// 	{
	// 		this.elements = elements;
	// 	}

	// 	public ArrayInitializer(TType elementType, List<TweedleExpression> elements)
	// 		: base(new TweedleArrayType(elementType))
	// 	{
	// 		this.elements = elements;
	// 	}

	// 	public ArrayInitializer(List<TweedleExpression> elements)
	// 		: base(new TweedleArrayType(CommonType(elements)))
	// 	{
	// 		this.elements = elements;
	// 	}

	// 	public ArrayInitializer(TweedleArrayType arrayType, TweedleExpression initializeSize)
	// 		: base(arrayType)
	// 	{
	// 		this.initializeSize = initializeSize;
	// 	}

	// 	private static TType CommonType(List<TweedleExpression> elements)
	// 	{
	// 		return null;
	// 	}

	// 	public override ExecutionStep AsStep(ExecutionScope scope)
	// 	{
	// 		StepSequence main = new StepSequence("new Array", scope);
	// 		List<ExecutionStep> steps = elements.Select(elem => elem?.AsStep(scope)).ToList();

	// 		ExecutionStep sizeStep;
	// 		if (initializeSize != null)
	// 		{
	// 			sizeStep = initializeSize.AsStep(scope);
	// 		}
	// 		else
	// 		{
	// 			sizeStep = new ValueStep("new Array size", scope, TStaticTypes.WHOLE_NUMBER.Instantiate(steps.Count()));
	// 		}
	// 		main.AddStep(sizeStep);
	// 		foreach (var step in steps)
	// 		{
	// 			main.AddStep(step);
	// 		}
	// 		// TODO Use size to construct array
	// 		main.AddStep(new ValueGenerationStep(
	// 			"CreateArray", scope,
	// 			() => new TArray((TweedleArrayType)this.Type, steps.Select(el => el.Result).ToList())));
	// 		return main;
	// 	}
	// }
}
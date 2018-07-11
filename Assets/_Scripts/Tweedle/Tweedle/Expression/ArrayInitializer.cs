using System.Collections.Generic;
using System.Linq;
using Alice.VM;

namespace Alice.Tweedle
{
	public class ArrayInitializer : TweedleExpression
	{
		List<TweedleExpression> elements;
		TweedleExpression initializeSize;

		public ArrayInitializer(TweedleArrayType arrayType, List<TweedleExpression> elements)
			: base(arrayType)
		{
			this.elements = elements;
		}

		public ArrayInitializer(TweedleType elementType, List<TweedleExpression> elements)
			: base(new TweedleArrayType(elementType))
		{
			this.elements = elements;
		}

		public ArrayInitializer(List<TweedleExpression> elements)
			: base(new TweedleArrayType(CommonType(elements)))
		{
			this.elements = elements;
		}

		public ArrayInitializer(TweedleArrayType arrayType, TweedleExpression initializeSize)
			: base(arrayType)
		{
			this.initializeSize = initializeSize;
		}

		private static TweedleType CommonType(List<TweedleExpression> elements)
		{
			return null;
		}

		internal override ExecutionStep AsStep(TweedleFrame frame)
		{
			StepSequence main = new StepSequence("new Array", frame);
			List<ExecutionStep> steps = elements.Select(elem => elem?.AsStep(frame)).ToList();

			ExecutionStep sizeStep;
			if (initializeSize != null)
			{
				sizeStep = initializeSize.AsStep(frame);
			}
			else
			{
				sizeStep = new ValueStep("new Array size", frame, TweedleTypes.WHOLE_NUMBER.Instantiate(steps.Count()));
			}
			main.AddStep(sizeStep);
			foreach (var step in steps)
			{
				main.AddStep(step);
			}
			// TODO Use size to construct array
			main.AddStep(new ContextNotifyingEvaluationStep(
				"CreateArray", frame,
				() => new TweedleArray((TweedleArrayType)this.Type, steps.Select(el => el.Result).ToList())));
			return main;
		}
	}
}
using System;
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

		internal override EvaluationStep AsStep(TweedleFrame frame)
		{
			List<EvaluationStep> steps = elements.Select(elem => elem?.AsStep(frame)).ToList();
			if (initializeSize != null)
			{
				EvaluationStep sizeStep = initializeSize.AsStep(frame);
				return new InitArrayStep((TweedleArrayType)this.Type, steps, sizeStep);
			}
			return new InitArrayStep((TweedleArrayType)this.Type, steps);

		}
	}

	internal class InitArrayStep : EvaluationStep
	{
		List<EvaluationStep> elements;
		TweedleArrayType type;
		private EvaluationStep sizeStep;

		public InitArrayStep(TweedleArrayType type, List<EvaluationStep> elements)
			: base(new List<ExecutionStep>(elements))
		{
			this.type = type;
			this.elements = elements;
		}

		public InitArrayStep(TweedleArrayType type, List<EvaluationStep> elements, EvaluationStep sizeStep) : this(type, elements)
		{
			// TODO
			this.sizeStep = sizeStep;
			AddBlockingStep(sizeStep);
		}

		internal override bool Execute()
		{
			result = new TweedleArray(type, elements.Select(el => el.Result).ToList());
			return MarkCompleted();
		}
	}
}
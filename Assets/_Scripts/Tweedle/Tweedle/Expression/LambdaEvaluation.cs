﻿using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class LambdaEvaluation : TweedleExpression
	{
        private TweedleExpression target;
        private List<TweedleExpression> arguments;

        public List<TweedleExpression> Arguments
        {
            get { return arguments; }
        }

        public LambdaEvaluation(TweedleExpression target)
            : base()
        {
            this.target = target;
            arguments = new List<TweedleExpression>();
        }

        public LambdaEvaluation(TweedleExpression target, List<TweedleExpression> arguments)
            : base()
        {
            this.target = target;
            this.arguments = arguments;
        }

		public override TweedleValue Evaluate(TweedleFrame frame)
        {
            // TODO evaluate target to a lambda expression and evaluate it.
			throw new System.NotImplementedException();
		}
	}
}
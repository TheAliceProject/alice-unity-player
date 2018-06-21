using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	abstract class InvocationFrame : TweedleFrame
	{
		TweedleFrame callingFrame;
		internal TweedleMethod Method { get; set; }
		protected Dictionary<TweedleValueHolderDeclaration, EvaluationStep> argumentSteps;
		public TweedleValue Result { get; internal set; }

		public InvocationFrame(TweedleFrame frame)
			: base(frame.vm)
		{
			callingFrame = frame;
		}

		internal void CreateArgumentSteps(Dictionary<string, TweedleExpression> arguments)
		{
			argumentSteps = new Dictionary<TweedleValueHolderDeclaration, EvaluationStep>();
			foreach (TweedleRequiredParameter req in Method.RequiredParameters)
			{
				if (arguments.ContainsKey(req.Name))
				{
					EvaluationStep argStep = arguments[req.Name].AsStep(callingFrame);
					argumentSteps.Add(req, argStep);
				}
				else
				{
					throw new TweedleLinkException("Invalid method call on " + Method.Name + ". Missing value for required parameter " + req.Name);
				}
				foreach (TweedleOptionalParameter opt in Method.OptionalParameters)
				{
					EvaluationStep argStep;
					if (arguments.ContainsKey(opt.Name))
					{
						argStep = arguments[req.Name].AsStep(callingFrame);
					}
					else
					{
						argStep = opt.Initializer.AsStep(callingFrame);
					}
					argumentSteps.Add(opt, argStep);
				}
			}
		}

		internal virtual EvaluationStep InvokeStep()
		{
			return new StartStep(
				StoreArgsStep(),
				() => new ContextEvaluationStep(Method.Body.ToSequentialStep(this), () => Result));
		}

		ExecutionStep StoreArgsStep()
		{
			ExecutionStep storeArgs = new ActionStep(() =>
			{
				foreach (KeyValuePair<TweedleValueHolderDeclaration, EvaluationStep> pair in argumentSteps)
				{
					SetLocalValue(pair.Key, pair.Value.Result);
				}
			});
			foreach (ExecutionStep step in argumentSteps.Values)
			{
				storeArgs.AddBlockingStep(step);
			}
			return storeArgs;
		}
	}
}

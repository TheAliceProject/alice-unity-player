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
			: base("Invocation", frame.vm)
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
						argStep = arguments[opt.Name].AsStep(callingFrame);
					}
					else
					{
						argStep = opt.Initializer.AsStep(callingFrame);
					}
					argumentSteps.Add(opt, argStep);
				}
			}
		}


		internal override string StackWith(string stackTop)
		{
			return callingFrame.StackWith(stackTop + "\n" + callStackEntry);
		}

		internal virtual void QueueInvocationStep(string callStack, NotifyingStep parent, Dictionary<string, TweedleExpression> arguments)
		{
			vm.AddStep(InvocationStep(callStack, parent, arguments));
		}

		internal virtual NotifyingEvaluationStep InvocationStep(string callStack, NotifyingStep parent, Dictionary<string, TweedleExpression> arguments)
		{
			SequentialStepsEvaluation main = new SequentialStepsEvaluation(callStack, parent);
			AddSteps(main, arguments);
			main.AddStep(Method.Body.AsSequentialStep(null, this));
			main.AddEvaluationStep(ResultStep());
			return main;
		}

		private NotifyingEvaluationStep ResultStep()
		{
			return new SingleInputNotificationStep("call", this, null, arg => Result);
		}

		protected virtual void AddSteps(SequentialStepsEvaluation main, Dictionary<string, TweedleExpression> arguments)
		{
			AddArgumentSteps(main, arguments);
		}

		private void AddArgumentSteps(SequentialStepsEvaluation main, Dictionary<string, TweedleExpression> arguments)
		{
			foreach (TweedleRequiredParameter req in Method.RequiredParameters)
			{
				if (arguments.ContainsKey(req.Name))
				{
					main.AddStep(ArgumentStep(req, arguments[req.Name]));
				}
				else
				{
					throw new TweedleLinkException("Invalid method call on " + Method.Name + ". Missing value for required parameter " + req.Name);
				}
			}
			foreach (TweedleOptionalParameter opt in Method.OptionalParameters)
			{
				if (arguments.ContainsKey(opt.Name))
				{
					main.AddStep(ArgumentStep(opt, arguments[opt.Name]));
				}
				else
				{
					main.AddStep(ArgumentStep(opt, opt.Initializer));
				}
			}
		}

		NotifyingStep ArgumentStep(TweedleValueHolderDeclaration argDec, TweedleExpression expression)
		{
			NotifyingStep storeStep =
				new SingleInputNotificationStep("Arg", callingFrame, null, arg =>
				{
					return SetLocalValue(argDec, arg);
				});
			return expression.AsStep(storeStep, callingFrame);
		}

		internal virtual EvaluationStep InvokeStep(string callStack)
		{
			return new StartStep(
				StoreArgsStep(),
				() => new ContextEvaluationStep(callStack, Method.Body.ToSequentialStep(this), () => Result));
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

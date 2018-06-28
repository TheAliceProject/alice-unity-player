using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	abstract class InvocationFrame : TweedleFrame
	{
		TweedleFrame callingFrame;
		internal TweedleMethod Method { get; set; }
		public TweedleValue Result { get; internal set; }

		public InvocationFrame(TweedleFrame frame)
			: base("Invocation", frame.vm)
		{
			callingFrame = frame;
		}

		internal override string StackWith(string stackTop)
		{
			return callingFrame.StackWith(stackTop + "\n" + callStackEntry);
		}

		internal virtual void QueueInvocationStep(string callStack, NotifyingStep parent, Dictionary<string, TweedleExpression> arguments)
		{
			//UnityEngine.Debug.Log("Queueing method invocation " + callStack);
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
					//UnityEngine.Debug.Log("Storing argument " + argDec.Name);
					return SetLocalValue(argDec, arg);
				});
			return expression.AsStep(storeStep, callingFrame);
		}
	}
}

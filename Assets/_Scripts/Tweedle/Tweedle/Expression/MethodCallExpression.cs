using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Alice.VM;

namespace Alice.Tweedle
{
	public class MethodCallExpression : MemberAccessExpression
	{
		internal Dictionary<string, TweedleExpression> arguments;

		public string MethodName { get; }

		public MethodCallExpression(TweedleExpression target, string methodName, Dictionary<string, TweedleExpression> arguments)
			: base(target)
		{
			Contract.Requires(arguments != null);
			MethodName = methodName;
			this.arguments = arguments;
		}

		public TweedleExpression GetArg(string argName)
		{
			return arguments[argName];
		}

		internal override EvaluationStep AsStep(TweedleFrame frame)
		{
			return new MethodStep(base.AsStep(frame), frame, this);
		}
	}

	internal class MethodStep : EvaluationStep
	{
		EvaluationStep target;
		TweedleMethod method;
		EvaluationStep finishStep;
		Dictionary<TweedleValueHolderDeclaration, EvaluationStep> argumentSteps;
		TweedleFrame frame;
		MethodCallExpression callExpression;

		public MethodStep(EvaluationStep target, TweedleFrame frame, MethodCallExpression callExpression)
			: base(new List<ExecutionStep> { target })
		{
			this.target = target;
			this.frame = frame;
			this.callExpression = callExpression;
		}

		internal override bool Execute()
		{
			BlockingSteps.Clear();
			if (argumentSteps == null)
			{
				method = target.Result.MethodNamed(frame, callExpression.MethodName);
				if (method == null)//|| !method.ExpectsArgs(callExpression.arguments))
				{
					throw new TweedleRuntimeException("No method matching " + target.Result + "." + callExpression.MethodName + "()");
				}
				//method.CheckForRequiredArgs(callExpression.arguments);
				//method.FillInOptionalArgs(callExpression.arguments);
				AddArgumentSteps(callExpression.arguments);
				return false;
			}
			else
			{
				if (finishStep == null)
				{
					// Wrap in return logic?
					MethodFrame methodFrame = MethodCallFrame();
					finishStep = methodFrame.FinishStep();
					// TODO decide if both of these are needed?
					AddBlockingStep(finishStep);
					AddBlockingStep(method.Body.ToSequentialStep(methodFrame));
					return false;
				}
				else
				{
					result = finishStep.Result;
					return MarkCompleted();
				}
			}
		}

		private MethodFrame MethodCallFrame()
		{
			var callFrame = frame.MethodCallFrame(target.Result, null);
			foreach (KeyValuePair<TweedleValueHolderDeclaration, EvaluationStep> pair in argumentSteps)
			{
				callFrame.SetLocalValue(pair.Key, pair.Value.Result);
			}
			return callFrame;
		}

		private void AddArgumentSteps(Dictionary<string, TweedleExpression> arguments)
		{
			argumentSteps = new Dictionary<TweedleValueHolderDeclaration, EvaluationStep>();
			foreach (TweedleRequiredParameter req in method.RequiredParameters)
			{
				if (arguments.ContainsKey(req.Name))
				{
					EvaluationStep argStep = arguments[req.Name].AsStep(frame);
					argumentSteps.Add(req, argStep);
					AddBlockingStep(argStep);
				}
				else
				{
					throw new TweedleLinkException("Invalid method call on " + method.Name + ". Missing value for required parameter " + req.Name);
				}
				foreach (TweedleOptionalParameter opt in method.OptionalParameters)
				{
					EvaluationStep argStep;
					if (arguments.ContainsKey(opt.Name))
					{
						argStep = arguments[req.Name].AsStep(frame);
					}
					else
					{
						argStep = opt.Initializer.AsStep(frame);
					}
					argumentSteps.Add(opt, argStep);
					AddBlockingStep(argStep);
				}
			}
		}
	}
}
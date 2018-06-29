﻿using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public class TweedleMethod
	{
		public List<string> Modifiers { get; }
		public string Name { get; }
		public TweedleType Type { get; }
		public List<TweedleRequiredParameter> RequiredParameters { get; }
		public List<TweedleOptionalParameter> OptionalParameters { get; }
		public BlockStatement Body { get; }

		public TweedleMethod(TweedleType resultType, string name, List<TweedleRequiredParameter> required, List<TweedleOptionalParameter> optional, List<TweedleStatement> statements)
			: this(new List<string>(), resultType, name, required, optional, statements)
		{
		}

		public TweedleMethod(List<string> modifiers, TweedleType resultType, string name, List<TweedleRequiredParameter> required, List<TweedleOptionalParameter> optional, List<TweedleStatement> statements)
		{
			Modifiers = modifiers;
			Type = resultType;
			Name = name;
			RequiredParameters = required;
			OptionalParameters = optional;
			Body = new BlockStatement(statements);
		}

		// TODO use args for method identification not just call validation
		public bool ExpectsArgs(Dictionary<string, TweedleExpression> arguments)
		{
			foreach (TweedleRequiredParameter param in RequiredParameters)
			{
				if (!arguments.ContainsKey(param.Name))
				{
					return false;
				}
			}
			return true;
		}

		public virtual bool IsStatic()
		{
			return Modifiers.Contains("static");
		}

		protected internal virtual NotifyingEvaluationStep AsStep(string callStack, InvocationFrame frame, Dictionary<string, TweedleExpression> arguments, NotifyingStep next)
		{
			SequentialStepsEvaluation main = new SequentialStepsEvaluation(callStack, next);
			AddPrepSteps(frame, main, arguments);
			main.AddStep(Body.AsSequentialStep(null, frame));
			main.AddEvaluationStep(ResultStep(frame));
			return main;
		}

		private NotifyingEvaluationStep ResultStep(InvocationFrame frame)
		{
			return new SingleInputNotificationStep("call", frame, arg => frame.Result, null);
		}

		protected internal virtual void AddPrepSteps(InvocationFrame frame, SequentialStepsEvaluation main, Dictionary<string, TweedleExpression> arguments)
		{
			AddArgumentSteps(frame, main, arguments);
		}

		private void AddArgumentSteps(InvocationFrame frame, SequentialStepsEvaluation main, Dictionary<string, TweedleExpression> arguments)
		{
			foreach (TweedleRequiredParameter req in RequiredParameters)
			{
				if (arguments.ContainsKey(req.Name))
				{
					main.AddStep(ArgumentStep(frame, req, arguments[req.Name]));
				}
				else
				{
					throw new TweedleLinkException("Invalid method call on " + Name + ". Missing value for required parameter " + req.Name);
				}
			}
			foreach (TweedleOptionalParameter opt in OptionalParameters)
			{
				if (arguments.ContainsKey(opt.Name))
				{
					main.AddStep(ArgumentStep(frame, opt, arguments[opt.Name]));
				}
				else
				{
					main.AddStep(ArgumentStep(frame, opt, opt.Initializer));
				}
			}
		}

		NotifyingStep ArgumentStep(InvocationFrame frame, TweedleValueHolderDeclaration argDec, TweedleExpression expression)
		{
			NotifyingStep storeStep =
				new SingleInputNotificationStep(
					"Arg",
					frame.callingFrame,
					arg => frame.SetLocalValue(argDec, arg),
					null);
			return expression.AsStep(storeStep, frame.callingFrame);
		}
	}
}
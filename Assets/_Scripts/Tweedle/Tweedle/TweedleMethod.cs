using System.Collections.Generic;
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

		protected internal virtual ExecutionStep AsStep(string callStackEntry, InvocationScope scope, Dictionary<string, TweedleExpression> arguments)
		{
			StepSequence main = new StepSequence(callStackEntry, scope.callingScope);
			AddInvocationSteps(scope, main, arguments);
			return main;
		}

		internal void AddInvocationSteps(InvocationScope scope, StepSequence steps, Dictionary<string, TweedleExpression> arguments)
		{
			AddPrepSteps(scope, steps, arguments);
			steps.AddStep(Body.AsSequentialStep(scope));
			steps.AddStep(ResultStep(scope));
		}

		private ExecutionStep ResultStep(InvocationScope scope)
		{
			return new ValueComputationStep("call", scope, arg => scope.Result);
		}

		protected internal virtual void AddPrepSteps(InvocationScope scope, StepSequence main, Dictionary<string, TweedleExpression> arguments)
		{
			AddArgumentSteps(scope, main, arguments);
		}

		private void AddArgumentSteps(InvocationScope scope, StepSequence main, Dictionary<string, TweedleExpression> arguments)
		{
			foreach (TweedleRequiredParameter req in RequiredParameters)
			{
				if (arguments.ContainsKey(req.Name))
				{
					main.AddStep(ArgumentStep(scope, req, arguments[req.Name]));
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
					main.AddStep(ArgumentStep(scope, opt, arguments[opt.Name]));
				}
				else
				{
					main.AddStep(ArgumentStep(scope, opt, opt.Initializer));
				}
			}
		}

		ExecutionStep ArgumentStep(InvocationScope scope, TweedleValueHolderDeclaration argDec, TweedleExpression expression)
		{
			var argStep = expression.AsStep(scope.callingScope);
			var storeStep = new ValueComputationStep(
					"Arg",
					scope.callingScope,
					arg => scope.SetLocalValue(argDec, arg));
			argStep.OnCompletionNotify(storeStep);
			return argStep;
		}
	}
}
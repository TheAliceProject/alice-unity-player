using System;
using Alice.Tweedle;
using Alice.Tweedle.Parsed;

namespace Alice.VM
{
	public class VirtualMachine
	{
		private TweedleFrame staticFrame;
		public TweedleSystem Library { get; }

		public VirtualMachine(TweedleSystem tweedleSystem)
		{
			Library = tweedleSystem;
			Initialize();
		}

		private void Initialize()
		{
			staticFrame = new TweedleFrame(this);
			InstantiateEnums();
			// TODO Evaluate static variables
			// make enums hard refs?
		}

		private void InstantiateEnums()
		{
			// TODO add enums to the staticFrame
			// throw new NotImplementedException();
		}

		public void Execute(TweedleExpression exp)
		{
			Execute(new ExpressionStatement(exp), val => { });
		}

		public void Execute(TweedleStatement statement, Action<TweedleValue> next)
		{
			statement.Execute(staticFrame.ExecutionFrame(next));
		}
	}

}

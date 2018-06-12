using Alice.VM;
using NUnit.Framework;

namespace Alice.Tweedle.Parsed
{
	[TestFixture]
	public class TweedleStatementRunTest
	{
		void ExecuteStatement(string src, TweedleFrame frame)
		{
			//VirtualMachine vm = new VirtualMachine(new TweedleSystem());
			TweedleStatement stmt = new TweedleParser().ParseStatement(src);
			UnityEngine.Debug.Log("Parsed to " + stmt);
			stmt.Execute(frame, () => { });
			//vm.Execute(stmt, val => );
		}

		[Test]
		public void EmptyBlockShouldExecute()
		{
			TweedleFrame frame = new TweedleFrame(new VirtualMachine(new TweedleSystem()));
			ExecuteStatement("doInOrder {}", frame);
		}

		[Test]
		public void LocalDeclarationShouldUpdateTheFrame()
		{
			TweedleFrame frame = new TweedleFrame(new VirtualMachine(new TweedleSystem()));
			ExecuteStatement("WholeNumber x <- 3;", frame);

			Assert.NotNull(frame.GetValue("x"));
		}

		[Test]
		public void LocalDeclarationShouldSetTheValueOnTheFrame()
		{
			TweedleFrame frame = new TweedleFrame(new VirtualMachine(new TweedleSystem()));
			ExecuteStatement("WholeNumber x <- 3;", frame);

			Assert.AreEqual(3, ((TweedlePrimitiveValue<int>)frame.GetValue("x")).Value, "Should be 3");
		}
	}
}

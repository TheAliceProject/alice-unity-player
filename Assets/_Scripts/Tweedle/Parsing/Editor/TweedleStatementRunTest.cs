using Alice.VM;
using NUnit.Framework;

namespace Alice.Tweedle.Parsed
{
	[TestFixture]
	public class TweedleStatementRunTest
	{
		void ExecuteStatement(string src, TweedleFrame frame)
		{
			TweedleStatement stmt = new TweedleParser().ParseStatement(src);
			new VirtualMachine(null).ExecuteToFinish(stmt, frame);
		}

		[Test]
		public void EmptyDoInOrderShouldExecute()
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

		[Test]
		public void DoInOrderSingletonShouldUpdateParentValues()
		{
			TweedleFrame frame = new TweedleFrame(new VirtualMachine(new TweedleSystem()));
			ExecuteStatement("WholeNumber x <- 3;", frame);

			ExecuteStatement("doInOrder { x <- 4; }", frame);

			Assert.AreEqual(4, ((TweedlePrimitiveValue<int>)frame.GetValue("x")).Value, "Should be 4");
		}

		[Test]
		public void DoInOrderSequenceShouldUpdateParentValues()
		{
			TweedleFrame frame = new TweedleFrame(new VirtualMachine(new TweedleSystem()));
			ExecuteStatement("WholeNumber x <- 3;", frame);

			ExecuteStatement("doInOrder { x <- 4; x <- 34; x <- 12; }", frame);

			Assert.AreEqual(12, ((TweedlePrimitiveValue<int>)frame.GetValue("x")).Value, "Should be 12");
		}

		[Test]
		public void DoInOrderSequenceShouldReadPreviousValues()
		{
			TweedleFrame frame = new TweedleFrame(new VirtualMachine(new TweedleSystem()));
			ExecuteStatement("WholeNumber x <- 3;", frame);

			ExecuteStatement("doInOrder { x <- x * 4; WholeNumber y <- 2; x <- x + y; }", frame);

			Assert.AreEqual(14, ((TweedlePrimitiveValue<int>)frame.GetValue("x")).Value, "Should be 14");
		}

		[Test]
		public void DoTogetherSingletonShouldUpdateParentValues()
		{
			TweedleFrame frame = new TweedleFrame(new VirtualMachine(new TweedleSystem()));
			ExecuteStatement("WholeNumber x <- 3;", frame);

			ExecuteStatement("doTogether { x <- 4; }", frame);

			Assert.AreEqual(4, ((TweedlePrimitiveValue<int>)frame.GetValue("x")).Value, "Should be 4");
		}

		[Test]
		public void DoTogetherSequenceShouldUpdateParentValuesInSomeOrder()
		{
			TweedleFrame frame = new TweedleFrame(new VirtualMachine(new TweedleSystem()));
			ExecuteStatement("WholeNumber x <- 3;", frame);

			ExecuteStatement("doTogether { x <- 4; x <- 34; x <- 12; }", frame);

			Assert.AreNotEqual(3, ((TweedlePrimitiveValue<int>)frame.GetValue("x")).Value, "Should no longer be 3");
		}

		[Test]
		public void ConditionalTrueShouldEvaluateThen()
		{
			TweedleFrame frame = new TweedleFrame(new VirtualMachine(new TweedleSystem()));
			ExecuteStatement("WholeNumber x <- 2;", frame);
			ExecuteStatement("if( true ) { x <- 5; } else { x <- 17; }", frame);

			Assert.AreEqual(5, ((TweedlePrimitiveValue<int>)frame.GetValue("x")).Value, "Should be 5");
		}

		[Test]
		public void ConditionalFalseShouldEvaluateElse()
		{
			TweedleFrame frame = new TweedleFrame(new VirtualMachine(new TweedleSystem()));
			ExecuteStatement("WholeNumber x <- 2;", frame);
			ExecuteStatement("if( false ) { x <- 5; } else { x <- 17; }", frame);

			Assert.AreEqual(17, ((TweedlePrimitiveValue<int>)frame.GetValue("x")).Value, "Should be 17");
		}

		[Test]
		public void CountLoopShouldEvaluateNTimes()
		{
			TweedleFrame frame = new TweedleFrame(new VirtualMachine(new TweedleSystem()));
			ExecuteStatement("WholeNumber x <- 0;", frame);
			ExecuteStatement("countUpTo( c < 3 ) { x <- x + c; }", frame);

			Assert.AreEqual(3, ((TweedlePrimitiveValue<int>)frame.GetValue("x")).Value, "Should be 6");
		}

		[Test]
		public void CountLoopInnerDeclarationsShouldNotLeak()
		{
			TweedleFrame frame = new TweedleFrame(new VirtualMachine(new TweedleSystem()));
			ExecuteStatement("countUpTo( c < 3 ) { WholeNumber x <- c; }", frame);
			Assert.Throws<TweedleRuntimeException>(() => frame.GetValue("x"));
		}

		[Test]
		public void NestedCountLoopsShouldEvaluateNxNTimes()
		{
			TweedleFrame frame = new TweedleFrame(new VirtualMachine(new TweedleSystem()));
			ExecuteStatement("WholeNumber x <- 0;", frame);
			ExecuteStatement("countUpTo( a < 3 ) { countUpTo( b < 3 ) { x <- x + 1; } }", frame);

			Assert.AreEqual(9, ((TweedlePrimitiveValue<int>)frame.GetValue("x")).Value);
		}

		[Test]
		public void NestedCountLoopsShouldEvaluateNxNxNTimes()
		{
			TweedleFrame frame = new TweedleFrame(new VirtualMachine(new TweedleSystem()));
			ExecuteStatement("WholeNumber x <- 0;", frame);
			ExecuteStatement("countUpTo( a < 10 ) { countUpTo( b < 10 ) { countUpTo( c < 10 ) { x <- x + 1; } } }", frame);

			Assert.AreEqual(1000, ((TweedlePrimitiveValue<int>)frame.GetValue("x")).Value);
		}

		[Test]
		public void ForEachLoopShouldEvaluateNTimes()
		{
			TweedleFrame frame = new TweedleFrame(new VirtualMachine(new TweedleSystem()));
			ExecuteStatement("WholeNumber x <- 0;", frame);
			ExecuteStatement("forEach( WholeNumber c in new WholeNumber[] {5,2,3} ) { x <- x + c; }", frame);

			Assert.AreEqual(10, ((TweedlePrimitiveValue<int>)frame.GetValue("x")).Value, "Should be 10");
		}

		[Test]
		public void ForEachLoopInnerDeclarationsShouldNotLeak()
		{
			TweedleFrame frame = new TweedleFrame(new VirtualMachine(new TweedleSystem()));
			ExecuteStatement("forEach( WholeNumber c in new WholeNumber[] {5,2,3} ) { WholeNumber x <- c; }", frame);
			Assert.Throws<TweedleRuntimeException>(() => frame.GetValue("x"));
		}

		[Test]
		public void AWhileLoopShouldReevaluateEachLoop()
		{
			TweedleFrame frame = new TweedleFrame(new VirtualMachine(new TweedleSystem()));
			ExecuteStatement("WholeNumber x <- 0;", frame);

			ExecuteStatement("while(x < 4) { x <- x+1; }", frame);

			Assert.AreEqual(4, ((TweedlePrimitiveValue<int>)frame.GetValue("x")).Value);
		}

		[Test]
		public void WhileLoopInnerDeclarationsShouldNotLeak()
		{
			TweedleFrame frame = new TweedleFrame(new VirtualMachine(new TweedleSystem()));
			ExecuteStatement("WholeNumber z <- 0;", frame);
			ExecuteStatement("while(z < 2) { z <- z+1; WholeNumber x <- z; }", frame);
			Assert.Throws<TweedleRuntimeException>(() => frame.GetValue("x"));
		}

		[Test]
		public void EachTogetherShouldChangeParentValues()
		{
			TweedleFrame frame = new TweedleFrame(new VirtualMachine(new TweedleSystem()));
			ExecuteStatement("WholeNumber x <- 0;", frame);
			ExecuteStatement("eachTogether(WholeNumber c in new WholeNumber[] {5,2,3} ) { x <- x + c; }", frame);
			Assert.AreNotEqual(0, ((TweedlePrimitiveValue<int>)frame.GetValue("x")).Value);
		}

		[Test]
		public void EachTogetherInnerDeclarationsShouldNotLeak()
		{
			TweedleFrame frame = new TweedleFrame(new VirtualMachine(new TweedleSystem()));
			ExecuteStatement("eachTogether(WholeNumber c in new WholeNumber[] {5,2,3} ) { WholeNumber x <- c; }", frame);
			Assert.Throws<TweedleRuntimeException>(() => frame.GetValue("x"));
		}
	}
}

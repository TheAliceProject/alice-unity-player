using Alice.VM;
using NUnit.Framework;

namespace Alice.Tweedle.Parsed
{
	[TestFixture]
	public class TweedleFrameTest
	{
		VirtualMachine vm;
		TweedleFrame frame;

		public void Init()
		{
			vm = new VirtualMachine(null);
			frame = new TweedleFrame("Test", vm);
		}

		[Test]
		public void FrameShouldHoldLocalValue()
		{
			Init();
			TweedleLocalVariable xDec = new TweedleLocalVariable(TweedleTypes.WHOLE_NUMBER, "x");
			frame.SetLocalValue(xDec, TweedleTypes.WHOLE_NUMBER.Instantiate(12));

			TweedlePrimitiveValue<int> newVal = (TweedlePrimitiveValue<int>)frame.GetValue("x");
			Assert.AreEqual(12, newVal.Value, "The VM should have returned 12.");
		}

		[Test]
		public void FrameShouldHoldLocalValueWithInitializer()
		{
			Init();
			TweedleLocalVariable xVar = new TweedleLocalVariable(TweedleTypes.WHOLE_NUMBER, "x", TweedleTypes.WHOLE_NUMBER.Instantiate(12));
			LocalVariableDeclaration xDec = new LocalVariableDeclaration(false, xVar);
			vm.ExecuteToFinish(xDec, frame);

			TweedlePrimitiveValue<int> newVal = (TweedlePrimitiveValue<int>)frame.GetValue("x");
			Assert.AreEqual(12, newVal.Value, "The VM should have returned 12.");
		}

		[Test]
		public void FrameShouldRejectSettingOfUnitializedLocalValue()
		{
			Init();
			Assert.Throws<TweedleRuntimeException>(
				() => frame.SetValue("x", TweedleTypes.TEXT_STRING.Instantiate("twelve")));
		}

		[Test]
		public void FrameShouldRejectLocalValueOfWrongType()
		{
			Init();
			TweedleLocalVariable xDec = new TweedleLocalVariable(TweedleTypes.WHOLE_NUMBER, "x");

			Assert.Throws<TweedleRuntimeException>(
				() => frame.SetLocalValue(xDec, TweedleTypes.TEXT_STRING.Instantiate("twelve")));
		}

		[Test]
		public void FrameShouldRejectNullForPrimitiveValue()
		{
			Init();
			TweedleLocalVariable xDec = new TweedleLocalVariable(TweedleTypes.WHOLE_NUMBER, "x");

			Assert.Throws<TweedleRuntimeException>(
				() => frame.SetLocalValue(xDec, TweedleNull.NULL));
		}

		[Test]
		public void FrameShouldRejectNullForObjectValue()
		{
			Init();
			TweedleClass cls = new TweedleClass("Dummy",
												new System.Collections.Generic.List<TweedleField>(),
												new System.Collections.Generic.List<TweedleMethod>(),
												new System.Collections.Generic.List<TweedleConstructor>());
			TweedleLocalVariable objDec = new TweedleLocalVariable(cls, "o");
			Assert.Throws<TweedleRuntimeException>(
				() => frame.SetLocalValue(objDec, TweedleNull.NULL));
		}

		[Test]
		public void FrameShouldReadParentValue()
		{
			Init();
			TweedleLocalVariable xDec = new TweedleLocalVariable(TweedleTypes.WHOLE_NUMBER, "x");
			frame.SetLocalValue(xDec, TweedleTypes.WHOLE_NUMBER.Instantiate(12));
			TweedleFrame child = frame.ChildFrame();

			TweedlePrimitiveValue<int> newVal = (TweedlePrimitiveValue<int>)child.GetValue("x");
			Assert.AreEqual(12, newVal.Value, "The VM should have returned 12.");
		}

		[Test]
		public void FrameShouldWriteValueInitializedOnParent()
		{
			Init();
			TweedleLocalVariable xDec = new TweedleLocalVariable(TweedleTypes.WHOLE_NUMBER, "x");
			frame.SetLocalValue(xDec, TweedleTypes.WHOLE_NUMBER.Instantiate(12));
			TweedleFrame child = frame.ChildFrame();
			child.SetValue("x", TweedleTypes.WHOLE_NUMBER.Instantiate(77));

			TweedlePrimitiveValue<int> newVal = (TweedlePrimitiveValue<int>)child.GetValue("x");
			Assert.AreEqual(77, newVal.Value, "The VM should have returned 77.");
		}

		[Test]
		public void FrameShouldWriteToParentValue()
		{
			Init();
			TweedleLocalVariable xDec = new TweedleLocalVariable(TweedleTypes.WHOLE_NUMBER, "x");
			frame.SetLocalValue(xDec, TweedleTypes.WHOLE_NUMBER.Instantiate(12));
			TweedleFrame child = frame.ChildFrame();
			child.SetValue("x", TweedleTypes.WHOLE_NUMBER.Instantiate(77));

			TweedlePrimitiveValue<int> newVal = (TweedlePrimitiveValue<int>)frame.GetValue("x");
			Assert.AreEqual(77, newVal.Value, "The VM should have returned 77.");
		}

		[Test]
		public void FrameShouldAllowBlockingOfParentValue()
		{
			Init();
			TweedleLocalVariable xDec = new TweedleLocalVariable(TweedleTypes.WHOLE_NUMBER, "x");
			frame.SetLocalValue(xDec, TweedleTypes.WHOLE_NUMBER.Instantiate(12));
			TweedleFrame child = frame.ChildFrame();
			child.SetLocalValue(xDec, TweedleTypes.WHOLE_NUMBER.Instantiate(77));

			TweedlePrimitiveValue<int> newVal = (TweedlePrimitiveValue<int>)child.GetValue("x");
			Assert.AreEqual(77, newVal.Value, "The VM should have returned 77.");
		}

		[Test]
		public void FrameShouldChangeParentValue()
		{
			Init();
			TweedleLocalVariable xDec = new TweedleLocalVariable(TweedleTypes.WHOLE_NUMBER, "x");
			frame.SetLocalValue(xDec, TweedleTypes.WHOLE_NUMBER.Instantiate(12));
			TweedleFrame child = frame.ChildFrame();
			child.SetLocalValue(xDec, TweedleTypes.WHOLE_NUMBER.Instantiate(77));

			TweedlePrimitiveValue<int> newVal = (TweedlePrimitiveValue<int>)frame.GetValue("x");
			Assert.AreEqual(12, newVal.Value, "The VM should have returned 12.");
		}
	}
}

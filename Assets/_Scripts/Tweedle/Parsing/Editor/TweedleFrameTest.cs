using NUnit.Framework;

namespace Alice.Tweedle.Parsed
{
	[TestFixture]
	public class TweedleFrameTest
	{
		[Test]
		public void FrameShouldHoldLocalValue()
		{
			TweedleFrame frame = new TweedleFrame(null);
			TweedleLocalVariable xDec = new TweedleLocalVariable(TweedleTypes.WHOLE_NUMBER, "x");
			frame.SetLocalValue(xDec, TweedleTypes.WHOLE_NUMBER.Instantiate(12));

			TweedlePrimitiveValue<int> newVal = (TweedlePrimitiveValue<int>)frame.GetValue("x");
			Assert.AreEqual(12, newVal.Value, "The VM should have returned 12.");
		}

		[Test]
		public void FrameShouldHoldLocalValueWithInitializer()
		{
			TweedleFrame frame = new TweedleFrame(null);
			TweedleLocalVariable xDec = new TweedleLocalVariable(TweedleTypes.WHOLE_NUMBER, "x", TweedleTypes.WHOLE_NUMBER.Instantiate(12));
			frame.SetLocalValue(xDec);

			TweedlePrimitiveValue<int> newVal = (TweedlePrimitiveValue<int>)frame.GetValue("x");
			Assert.AreEqual(12, newVal.Value, "The VM should have returned 12.");
		}

		[Test]
		public void FrameShouldRejectSettingOfUnitializedLocalValue()
		{
			TweedleFrame frame = new TweedleFrame(null);

			Assert.Throws<TweedleRuntimeException>(
				() => frame.SetValue("x", TweedleTypes.TEXT_STRING.Instantiate("twelve"), value => { }));
		}

		[Test]
		public void FrameShouldRejectLocalValueOfWrongType()
		{
			TweedleFrame frame = new TweedleFrame(null);
			TweedleLocalVariable xDec = new TweedleLocalVariable(TweedleTypes.WHOLE_NUMBER, "x");

			Assert.Throws<TweedleRuntimeException>(
				() => frame.SetLocalValue(xDec, TweedleTypes.TEXT_STRING.Instantiate("twelve")));
		}

		[Test]
		public void FrameShouldRejectNullForPrimitiveValue()
		{
			TweedleFrame frame = new TweedleFrame(null);
			TweedleLocalVariable xDec = new TweedleLocalVariable(TweedleTypes.WHOLE_NUMBER, "x");

			Assert.Throws<TweedleRuntimeException>(
				() => frame.SetLocalValue(xDec, TweedleNull.NULL));
		}

		[Test]
		public void FrameShouldRejectNullForObjectValue()
		{
			TweedleFrame frame = new TweedleFrame(null);
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
			TweedleFrame parent = new TweedleFrame(null);
			TweedleLocalVariable xDec = new TweedleLocalVariable(TweedleTypes.WHOLE_NUMBER, "x");
			parent.SetLocalValue(xDec, TweedleTypes.WHOLE_NUMBER.Instantiate(12));
			TweedleFrame frame = parent.ChildFrame();

			TweedlePrimitiveValue<int> newVal = (TweedlePrimitiveValue<int>)frame.GetValue("x");
			Assert.AreEqual(12, newVal.Value, "The VM should have returned 12.");
		}

		[Test]
		public void FrameShouldWriteValueInitializedOnParent()
		{
			TweedleFrame parent = new TweedleFrame(null);
			TweedleLocalVariable xDec = new TweedleLocalVariable(TweedleTypes.WHOLE_NUMBER, "x");
			parent.SetLocalValue(xDec, TweedleTypes.WHOLE_NUMBER.Instantiate(12));
			TweedleFrame frame = parent.ChildFrame();
			frame.SetValue("x", TweedleTypes.WHOLE_NUMBER.Instantiate(77), value => { });

			TweedlePrimitiveValue<int> newVal = (TweedlePrimitiveValue<int>)frame.GetValue("x");
			Assert.AreEqual(77, newVal.Value, "The VM should have returned 77.");
		}

		[Test]
		public void FrameShouldWriteToParentValue()
		{
			TweedleFrame parent = new TweedleFrame(null);
			TweedleLocalVariable xDec = new TweedleLocalVariable(TweedleTypes.WHOLE_NUMBER, "x");
			parent.SetLocalValue(xDec, TweedleTypes.WHOLE_NUMBER.Instantiate(12));
			TweedleFrame frame = parent.ChildFrame();
			frame.SetValue("x", TweedleTypes.WHOLE_NUMBER.Instantiate(77), value => { });

			TweedlePrimitiveValue<int> newVal = (TweedlePrimitiveValue<int>)parent.GetValue("x");
			Assert.AreEqual(77, newVal.Value, "The VM should have returned 77.");
		}

		[Test]
		public void FrameShouldAllowBlockingOfParentValue()
		{
			TweedleFrame parent = new TweedleFrame(null);
			TweedleLocalVariable xDec = new TweedleLocalVariable(TweedleTypes.WHOLE_NUMBER, "x");
			parent.SetLocalValue(xDec, TweedleTypes.WHOLE_NUMBER.Instantiate(12));
			TweedleFrame frame = parent.ChildFrame();
			frame.SetLocalValue(xDec, TweedleTypes.WHOLE_NUMBER.Instantiate(77));

			TweedlePrimitiveValue<int> newVal = (TweedlePrimitiveValue<int>)frame.GetValue("x");
			Assert.AreEqual(77, newVal.Value, "The VM should have returned 77.");
		}

		[Test]
		public void FrameShouldChangeParentValue()
		{
			TweedleFrame parent = new TweedleFrame(null);
			TweedleLocalVariable xDec = new TweedleLocalVariable(TweedleTypes.WHOLE_NUMBER, "x");
			parent.SetLocalValue(xDec, TweedleTypes.WHOLE_NUMBER.Instantiate(12));
			TweedleFrame frame = parent.ChildFrame();
			frame.SetLocalValue(xDec, TweedleTypes.WHOLE_NUMBER.Instantiate(77));

			TweedlePrimitiveValue<int> newVal = (TweedlePrimitiveValue<int>)parent.GetValue("x");
			Assert.AreEqual(12, newVal.Value, "The VM should have returned 12.");
		}
	}
}

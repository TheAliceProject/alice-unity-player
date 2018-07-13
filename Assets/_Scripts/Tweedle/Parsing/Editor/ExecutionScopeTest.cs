using Alice.VM;
using NUnit.Framework;

namespace Alice.Tweedle.Parsed
{
	[TestFixture]
	public class ExecutionScopeTest
	{
		VirtualMachine vm;
		ExecutionScope scope;

		public void Init()
		{
			vm = new VirtualMachine();
			scope = new ExecutionScope("Test", vm);
		}

		[Test]
		public void ScopeShouldHoldLocalValue()
		{
			Init();
			TweedleLocalVariable xDec = new TweedleLocalVariable(TweedleTypes.WHOLE_NUMBER, "x");
			scope.SetLocalValue(xDec, TweedleTypes.WHOLE_NUMBER.Instantiate(12));

			TweedlePrimitiveValue<int> newVal = (TweedlePrimitiveValue<int>)scope.GetValue("x");
			Assert.AreEqual(12, newVal.Value, "The VM should have returned 12.");
		}

		[Test]
		public void ScopeShouldHoldLocalValueWithInitializer()
		{
			Init();
			TweedleLocalVariable xVar = new TweedleLocalVariable(TweedleTypes.WHOLE_NUMBER, "x", TweedleTypes.WHOLE_NUMBER.Instantiate(12));
			LocalVariableDeclaration xDec = new LocalVariableDeclaration(false, xVar);
			vm.ExecuteToFinish(xDec, scope);

			TweedlePrimitiveValue<int> newVal = (TweedlePrimitiveValue<int>)scope.GetValue("x");
			Assert.AreEqual(12, newVal.Value, "The VM should have returned 12.");
		}

		[Test]
		public void ScopeShouldRejectSettingOfUnitializedLocalValue()
		{
			Init();
			Assert.Throws<TweedleRuntimeException>(
				() => scope.SetValue("x", TweedleTypes.TEXT_STRING.Instantiate("twelve")));
		}

		[Test]
		public void ScopeShouldRejectLocalValueOfWrongType()
		{
			Init();
			TweedleLocalVariable xDec = new TweedleLocalVariable(TweedleTypes.WHOLE_NUMBER, "x");

			Assert.Throws<TweedleRuntimeException>(
				() => scope.SetLocalValue(xDec, TweedleTypes.TEXT_STRING.Instantiate("twelve")));
		}

		[Test]
		public void ScopeShouldRejectNullForPrimitiveValue()
		{
			Init();
			TweedleLocalVariable xDec = new TweedleLocalVariable(TweedleTypes.WHOLE_NUMBER, "x");

			Assert.Throws<TweedleRuntimeException>(
				() => scope.SetLocalValue(xDec, TweedleNull.NULL));
		}

		[Test]
		public void ScopeShouldRejectNullForObjectValue()
		{
			Init();
			TweedleClass cls = new TweedleClass("Dummy",
												new System.Collections.Generic.List<TweedleField>(),
												new System.Collections.Generic.List<TweedleMethod>(),
												new System.Collections.Generic.List<TweedleConstructor>());
			TweedleLocalVariable objDec = new TweedleLocalVariable(cls, "o");
			Assert.Throws<TweedleRuntimeException>(
				() => scope.SetLocalValue(objDec, TweedleNull.NULL));
		}

		[Test]
		public void ScopeShouldReadParentValue()
		{
			Init();
			TweedleLocalVariable xDec = new TweedleLocalVariable(TweedleTypes.WHOLE_NUMBER, "x");
			scope.SetLocalValue(xDec, TweedleTypes.WHOLE_NUMBER.Instantiate(12));
			ExecutionScope child = scope.ChildScope();

			TweedlePrimitiveValue<int> newVal = (TweedlePrimitiveValue<int>)child.GetValue("x");
			Assert.AreEqual(12, newVal.Value, "The VM should have returned 12.");
		}

		[Test]
		public void ScopeShouldWriteValueInitializedOnParent()
		{
			Init();
			TweedleLocalVariable xDec = new TweedleLocalVariable(TweedleTypes.WHOLE_NUMBER, "x");
			scope.SetLocalValue(xDec, TweedleTypes.WHOLE_NUMBER.Instantiate(12));
			ExecutionScope child = scope.ChildScope();
			child.SetValue("x", TweedleTypes.WHOLE_NUMBER.Instantiate(77));

			TweedlePrimitiveValue<int> newVal = (TweedlePrimitiveValue<int>)child.GetValue("x");
			Assert.AreEqual(77, newVal.Value, "The VM should have returned 77.");
		}

		[Test]
		public void ScopeShouldWriteToParentValue()
		{
			Init();
			TweedleLocalVariable xDec = new TweedleLocalVariable(TweedleTypes.WHOLE_NUMBER, "x");
			scope.SetLocalValue(xDec, TweedleTypes.WHOLE_NUMBER.Instantiate(12));
			ExecutionScope child = scope.ChildScope();
			child.SetValue("x", TweedleTypes.WHOLE_NUMBER.Instantiate(77));

			TweedlePrimitiveValue<int> newVal = (TweedlePrimitiveValue<int>)scope.GetValue("x");
			Assert.AreEqual(77, newVal.Value, "The VM should have returned 77.");
		}

		[Test]
		public void ScopeShouldAllowBlockingOfParentValue()
		{
			Init();
			TweedleLocalVariable xDec = new TweedleLocalVariable(TweedleTypes.WHOLE_NUMBER, "x");
			scope.SetLocalValue(xDec, TweedleTypes.WHOLE_NUMBER.Instantiate(12));
			ExecutionScope child = scope.ChildScope();
			child.SetLocalValue(xDec, TweedleTypes.WHOLE_NUMBER.Instantiate(77));

			TweedlePrimitiveValue<int> newVal = (TweedlePrimitiveValue<int>)child.GetValue("x");
			Assert.AreEqual(77, newVal.Value, "The VM should have returned 77.");
		}

		[Test]
		public void ScopeShouldChangeParentValue()
		{
			Init();
			TweedleLocalVariable xDec = new TweedleLocalVariable(TweedleTypes.WHOLE_NUMBER, "x");
			scope.SetLocalValue(xDec, TweedleTypes.WHOLE_NUMBER.Instantiate(12));
			ExecutionScope child = scope.ChildScope();
			child.SetLocalValue(xDec, TweedleTypes.WHOLE_NUMBER.Instantiate(77));

			TweedlePrimitiveValue<int> newVal = (TweedlePrimitiveValue<int>)scope.GetValue("x");
			Assert.AreEqual(12, newVal.Value, "The VM should have returned 12.");
		}
	}
}

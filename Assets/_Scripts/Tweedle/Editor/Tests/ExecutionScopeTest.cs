﻿using Alice.Tweedle.VM;
using NUnit.Framework;

namespace Alice.Tweedle.Parse
{
	[TestFixture]
	public class ExecutionScopeTest
	{
		TestVirtualMachine vm;
		ExecutionScope scope;

		public void Init()
		{
			vm = new TestVirtualMachine();
			scope = new ExecutionScope("Test", vm);
		}

		[Test]
		public void ScopeShouldHoldLocalValue()
		{
			Init();
			TLocalVariable xDec = new TLocalVariable(TStaticTypes.WHOLE_NUMBER, "x");
			scope.SetLocalValue(xDec, TStaticTypes.WHOLE_NUMBER.Instantiate(12));

			TValue newVal = (TValue)scope.GetValue("x");
			Assert.AreEqual(12, newVal.ToInt(), "The VM should have returned 12.");
		}

		[Test]
		public void ScopeShouldHoldLocalValueWithInitializer()
		{
			Init();
			TLocalVariable xVar = new TLocalVariable(TStaticTypes.WHOLE_NUMBER, "x", TStaticTypes.WHOLE_NUMBER.Instantiate(12));
			LocalVariableDeclaration xDec = new LocalVariableDeclaration(false, xVar);
			vm.ExecuteToFinish(xDec, scope);

			TValue newVal = (TValue)scope.GetValue("x");
			Assert.AreEqual(12, newVal.ToInt(), "The VM should have returned 12.");
		}

		[Test]
		public void ScopeShouldRejectSettingOfUnitializedLocalValue()
		{
			Init();
			Assert.Throws<TweedleRuntimeException>(
				() => scope.SetValue("x", TStaticTypes.TEXT_STRING.Instantiate("twelve")));
		}

		[Test]
		public void ScopeShouldRejectLocalValueOfWrongType()
		{
			Init();
			TLocalVariable xDec = new TLocalVariable(TStaticTypes.WHOLE_NUMBER, "x");

			Assert.Throws<TweedleRuntimeException>(
				() => scope.SetLocalValue(xDec, TStaticTypes.TEXT_STRING.Instantiate("twelve")));
		}

		[Test]
		public void ScopeShouldRejectNullForPrimitiveValue()
		{
			Init();
			TLocalVariable xDec = new TLocalVariable(TStaticTypes.WHOLE_NUMBER, "x");

			Assert.Throws<TweedleRuntimeException>(
				() => scope.SetLocalValue(xDec, TValue.NULL));
		}

		[Test]
		public void ScopeShouldRejectNullForObjectValue()
		{
			Init();
			TClassType cls = new TClassType("Dummy",
												TField.EMPTY_ARRAY,
												TMethod.EMPTY_ARRAY,
												TMethod.EMPTY_ARRAY);
			TLocalVariable objDec = new TLocalVariable(cls, "o");
			Assert.Throws<TweedleRuntimeException>(
				() => scope.SetLocalValue(objDec, TValue.NULL));
		}

		[Test]
		public void ScopeShouldReadParentValue()
		{
			Init();
			TLocalVariable xDec = new TLocalVariable(TStaticTypes.WHOLE_NUMBER, "x");
			scope.SetLocalValue(xDec, TStaticTypes.WHOLE_NUMBER.Instantiate(12));
			ExecutionScope child = scope.ChildScope();

			TValue newVal = (TValue)child.GetValue("x");
			Assert.AreEqual(12, newVal.ToInt(), "The VM should have returned 12.");
		}

		[Test]
		public void ScopeShouldWriteValueInitializedOnParent()
		{
			Init();
			TLocalVariable xDec = new TLocalVariable(TStaticTypes.WHOLE_NUMBER, "x");
			scope.SetLocalValue(xDec, TStaticTypes.WHOLE_NUMBER.Instantiate(12));
			ExecutionScope child = scope.ChildScope();
			child.SetValue("x", TStaticTypes.WHOLE_NUMBER.Instantiate(77));

			TValue newVal = (TValue)child.GetValue("x");
			Assert.AreEqual(77, newVal.ToInt(), "The VM should have returned 77.");
		}

		[Test]
		public void ScopeShouldWriteToParentValue()
		{
			Init();
			TLocalVariable xDec = new TLocalVariable(TStaticTypes.WHOLE_NUMBER, "x");
			scope.SetLocalValue(xDec, TStaticTypes.WHOLE_NUMBER.Instantiate(12));
			ExecutionScope child = scope.ChildScope();
			child.SetValue("x", TStaticTypes.WHOLE_NUMBER.Instantiate(77));

			TValue newVal = (TValue)scope.GetValue("x");
			Assert.AreEqual(77, newVal.ToInt(), "The VM should have returned 77.");
		}

		[Test]
		public void ScopeShouldAllowBlockingOfParentValue()
		{
			Init();
			TLocalVariable xDec = new TLocalVariable(TStaticTypes.WHOLE_NUMBER, "x");
			scope.SetLocalValue(xDec, TStaticTypes.WHOLE_NUMBER.Instantiate(12));
			ExecutionScope child = scope.ChildScope();
			child.SetLocalValue(xDec, TStaticTypes.WHOLE_NUMBER.Instantiate(77));

			TValue newVal = (TValue)child.GetValue("x");
			Assert.AreEqual(77, newVal.ToInt(), "The VM should have returned 77.");
		}

		[Test]
		public void ScopeShouldChangeParentValue()
		{
			Init();
			TLocalVariable xDec = new TLocalVariable(TStaticTypes.WHOLE_NUMBER, "x");
			scope.SetLocalValue(xDec, TStaticTypes.WHOLE_NUMBER.Instantiate(12));
			ExecutionScope child = scope.ChildScope();
			child.SetLocalValue(xDec, TStaticTypes.WHOLE_NUMBER.Instantiate(77));

			TValue newVal = (TValue)scope.GetValue("x");
			Assert.AreEqual(12, newVal.ToInt(), "The VM should have returned 12.");
		}
	}
}

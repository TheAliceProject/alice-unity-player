using NUnit.Framework;

namespace Alice.Tweedle.Parse
{
	[TestFixture]
	public class TweedleExpressionParseTest
	{
		private ITweedleExpression ParseExpression(string src)
		{
			return new TweedleParser().ParseExpression(src);
		}

		[Test]
		public void SomethingShouldBeCreatedForParenthesizedNumber()
		{
			ITweedleExpression tested = ParseExpression("(3)");
			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void ANumberShouldBeCreatedForParenthesizedNumber()
		{
			ITweedleExpression tested = ParseExpression("(3)");
			Assert.IsInstanceOf<TWholeNumberType>(tested.Type.Get(), "The parser should have returned a TweedlePrimitiveValue.");
		}

		[Test]
		public void ANumberShouldBeCreatedForMultiplyParenthesizedNumber()
		{
			ITweedleExpression tested = ParseExpression("(((3)))");
			Assert.IsInstanceOf<TWholeNumberType>(tested.Type.Get(), "The parser should have returned aTweedlePrimitiveValue.");
		}

		[Test]
		public void SomethingShouldBeCreatedForTrue()
		{
			ITweedleExpression tested = ParseExpression("true");

			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void BoolPrimitiveShouldBeCreatedForTrue()
		{
			ITweedleExpression tested = ParseExpression("true");
			Assert.IsInstanceOf<TBooleanType>(tested.Type.Get(), "The parser should have returned a TweedlePrimitiveValue.");
		}

		[Test]
		public void TruePrimitiveShouldBeCreatedForTrue()
		{
			TValue tested = (TValue)ParseExpression("true");
			Assert.IsTrue(tested.ToBoolean(), "The parser should have returned true.");
		}

		[Test]
		public void SomethingShouldBeCreatedForNotTrue()
		{
			ITweedleExpression tested = ParseExpression("!true");

			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void LogicalNotExpressionShouldBeCreatedForNotTrue()
		{
			ITweedleExpression tested = ParseExpression("!true");
			Assert.IsInstanceOf<LogicalNotExpression>(tested, "The parser should have returned a LogicalNotExpression.");
		}

		[Test]
		public void FalsePrimitiveShouldBeCreatedForNotTrue()
		{
			LogicalNotExpression tested = (LogicalNotExpression)ParseExpression("!true");
			Assert.IsFalse(tested.EvaluateNow().ToBoolean(),
						   "The parser should have returned false.");
		}

		[Test]
		public void SomethingShouldBeCreatedForFalse()
		{
			ITweedleExpression tested = ParseExpression("false");

			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void BoolPrimitiveShouldBeCreatedForFalse()
		{
			ITweedleExpression tested = ParseExpression("false");
			Assert.IsInstanceOf<TBooleanType>(tested.Type.Get(), "The parser should have returned a TweedlePrimitiveValue.");
		}

		[Test]
		public void FalsePrimitiveShouldBeCreatedForFalse()
		{
			TValue tested = (TValue)ParseExpression("false");
			Assert.IsFalse(tested.ToBoolean(), "The parser should have returned false.");
		}

		[Test]
		public void SomethingShouldBeCreatedForNotFalse()
		{
			ITweedleExpression tested = ParseExpression("!false");

			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void LogicalNotExpressionShouldBeCreatedForNotFalse()
		{
			ITweedleExpression tested = ParseExpression("!false");
			Assert.IsInstanceOf<LogicalNotExpression>(tested, "The parser should have returned a LogicalNotExpression.");
		}

		[Test]
		public void TruePrimitiveShouldBeCreatedForNotFalse()
		{
			LogicalNotExpression tested = (LogicalNotExpression)ParseExpression("!false");
			Assert.IsTrue(tested.EvaluateNow().ToBoolean(),
						  "The parser should have returned true.");
		}

		[Test]
		public void SomethingShouldBeCreatedForThis()
		{
			ITweedleExpression tested = ParseExpression("this");

			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void AThisExpressionShouldBeCreated()
		{
			ITweedleExpression tested = ParseExpression("this");

			Assert.IsInstanceOf<ThisExpression>(tested, "The parser should have returned a ThisExpression.");
		}

		[Test]
		public void SomethingShouldBeCreatedForConcatenation()
		{
			ITweedleExpression tested = ParseExpression("\"hello\" .. \" there\"");

			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void AConcatenationExpressionShouldBeCreated()
		{
			ITweedleExpression tested = ParseExpression("\"hello\" .. \" there\"");

			Assert.IsInstanceOf<StringConcatenationExpression>(tested, "The parser should have returned an StringConcatenationExpression.");
		}

		[Test]
		public void CreatedConcatenationExpressionShouldEvaluate()
		{
			StringConcatenationExpression tested = (StringConcatenationExpression)ParseExpression("\"hello\" .. \" there\"");

			Assert.NotNull(tested.EvaluateLiteral(), "The StringConcatenationExpression should evaluate to something.");
		}

		[Test]
		public void CreatedConcatenationExpressionShouldEvaluateToAPrimitiveValue()
		{
			StringConcatenationExpression tested = (StringConcatenationExpression)ParseExpression("\"hello\" .. \" there\"");

			Assert.IsInstanceOf<TTextStringType>(tested.EvaluateLiteral().Type, "The StringConcatenationExpression should evaluate to a tweedle value.");
		}

		[Test]
		public void CreatedConcatenationExpressionShouldContainCorrectResult()
		{
			StringConcatenationExpression tested = (StringConcatenationExpression)ParseExpression("\"hello\" .. \" there\"");

			Assert.AreEqual("hello there",
							tested.EvaluateLiteral().ToTextString(),
							"The StringConcatenationExpression should evaluate correctly.");
		}

		[Test]
		public void ConcatenationShouldKnowItsType()
		{
			ITweedleExpression tested = ParseExpression("\"hello\" .. \" there\"");
			Assert.AreEqual(TStaticTypes.TEXT_STRING,
				tested.Type,
				"The type should be TextString");
		}

		[Test]
		public void SomethingShouldBeCreatedForWholeNumberNegation()
		{
			ITweedleExpression tested = ParseExpression("-4");

			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void SomethingShouldBeCreatedForDecimalNegation()
		{
			ITweedleExpression tested = ParseExpression("-3.74");

			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void IntegerValueShouldBeCreatedFromNegative()
		{
			ITweedleExpression tested = ParseExpression("-4");

			Assert.IsInstanceOf<TWholeNumberType>(tested.Type.Get(), "The parser should have returned a TweedlePrimitiveValue.");
		}

		[Test]
		public void DecimalValueShouldBeCreatedFromNegative()
		{
			ITweedleExpression tested = ParseExpression("-3.74");

			Assert.IsInstanceOf<TDecimalNumberType>(tested.Type.Get(), "The parser should have returned a TweedlePrimitiveValue.");
		}

		[Test]
		public void CreatedNegativeWholeNumberShouldEvaluateCorrectly()
		{
			TValue tested = (TValue)ParseExpression("-4");

			Assert.AreEqual(-4, tested.ToInt());
		}

		[Test]
		public void CreatedNegativeDecimalShouldEvaluateCorrectly()
		{
			TValue tested = (TValue)ParseExpression("-3.74");

			Assert.AreEqual(-3.74, tested.ToDouble());
		}

		[Test]
		public void SomethingShouldBeCreatedForAddition()
		{
			ITweedleExpression tested = ParseExpression("3 + 4");

			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void AnAdditionExpressionShouldBeCreated()
		{
			ITweedleExpression tested = ParseExpression("3 + 4");

			Assert.IsInstanceOf<AdditionExpression>(tested, "The parser should have returned an AdditionExpression.");
		}

		/*	[Test]
			public void additionExpressionShouldHaveLeft() {
				AdditionExpression tested = (AdditionExpression) ParseExpression( "3 + 4" );
				Assert.True("The AdditionExpression should have a left hand side.", tested. );
			}*/

		[Test]
		public void CreatedAdditionExpressionShouldEvaluate()
		{
			AdditionExpression tested = (AdditionExpression)ParseExpression("3 + 4");

			Assert.NotNull(tested.EvaluateLiteral().Type, "The AdditionExpression should evaluate to something.");
		}

		[Test]
		public void CreatedWholeNumberAdditionExpressionShouldEvaluateToAPrimitiveValue()
		{
			AdditionExpression tested = (AdditionExpression)ParseExpression("3 + 4");

			Assert.IsInstanceOf<TWholeNumberType>(tested.EvaluateLiteral().Type, "The AdditionExpression should evaluate to a primitive value.");
		}

		[Test]
		public void WholeNumberAdditionExpressionShouldContainCorrectResult()
		{
			AdditionExpression tested = (AdditionExpression)ParseExpression("3 + 4");

			Assert.AreEqual(7,
							tested.EvaluateLiteral().ToInt(),
				"The AdditionExpression should evaluate correctly.");
		}

		[Test]
		public void WholeNumberAdditionShouldNotKnowItsType()
		{
			ITweedleExpression tested = ParseExpression("3 + 4");
			Assert.AreEqual(TStaticTypes.NUMBER, tested.Type, "The type should be Number");
		}

		[Test]
		public void CompoundMathShouldEvaluate()
		{
			ITweedleExpression tested = ParseExpression("(1 + 1 + 1 + 1 - 1 * 2 + 2) / 2");
			Assert.AreEqual(2,
							tested.EvaluateNow().ToInt(),
				"The compound expression should evaluate correctly to an int.");
		}

		[Test]
		public void CompoundMathShouldSpreadType()
		{
			ITweedleExpression tested = ParseExpression("(1 + 1 + 1.0 + 1 - 1 * 2 + 2) / 2");
			Assert.AreEqual(2.0,
							tested.EvaluateNow().ToDouble(),
				"The compound expression should evaluate correctly to a double.");
		}

		[Test]
		public void DecimalAdditionExpressionShouldEvaluateToAPrimitiveValue()
		{
			AdditionExpression tested = (AdditionExpression)ParseExpression("2.1 + 4.9");

			Assert.IsInstanceOf<TDecimalNumberType>(tested.EvaluateLiteral().Type, "The AdditionExpression should evaluate to a tweedle value.");
		}

		[Test]
		public void DecimalNumberAdditionExpressionShouldContainCorrectResult()
		{
			AdditionExpression tested = (AdditionExpression)ParseExpression("2.1 + 4.9");

			Assert.AreEqual(7.0,
				tested.EvaluateLiteral().ToDouble(),
				"The AdditionExpression should evaluate correctly.");
		}

		[Test]
		public void DecimalNumberAdditionShouldNotKnowItsType()
		{
			ITweedleExpression tested = ParseExpression("3.4 + 4.1");
			Assert.AreEqual(TStaticTypes.NUMBER, tested.Type, "The type should be Number");
		}

		[Test]
		public void PlusFailsWithStringArgument()
		{
			Assert.Throws<System.Exception>(() => ParseExpression("3 + \"4.1\""));
		}

		[Test]
		public void ConcatWorksWithNumericArgument()
		{
			ITweedleExpression tested = ParseExpression("3 .. \"4.1\"");
			Assert.AreEqual("34.1",
							tested.EvaluateNow().ToTextString(),
							"The StringConcatenationExpression should evaluate correctly.");
		}

		[Test]
		public void ConcatWithNumericArgumentProducesString()
		{
			ITweedleExpression tested = ParseExpression("3 .. \"4.1\"");
			Assert.IsInstanceOf<StringConcatenationExpression>(tested, "The parser should have returned an StringConcatenationExpression.");
		}

		[Test]
		public void SomethingShouldBeCreatedForStringConcat()
		{
			ITweedleExpression tested = ParseExpression("\"Hello\" .. \" there\"");

			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void StringConcatShouldBeCreatedForStringConcat()
		{
			ITweedleExpression tested = ParseExpression("\"Hello\" .. \" there\"");

			Assert.IsInstanceOf<StringConcatenationExpression>(tested, "The parser should have returned an StringConcatenationExpression.");
		}

		[Test]
		public void SomethingShouldBeCreatedForAdditionOfVariables()
		{
			ITweedleExpression tested = ParseExpression("a + b");

			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void AnAdditionExpressionShouldBeCreatedForVariables()
		{
			ITweedleExpression tested = ParseExpression("a + b");

			Assert.IsInstanceOf<AdditionExpression>(tested, "The parser should have returned an AdditionExpression.");
		}

		[Test]
		public void SomethingShouldBeCreatedForIdentifier()
		{
			ITweedleExpression tested = ParseExpression("x");
			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void AnIdentifierExpressionShouldBeCreated()
		{
			ITweedleExpression tested = ParseExpression("x");
			Assert.IsInstanceOf<IdentifierReference>(tested, "The parser should have returned an IdentifierReference.");
		}

		[Test]
		public void AnIdentifierExpressionShouldBeCreatedNamedX()
		{
			IdentifierReference tested = (IdentifierReference)ParseExpression("x");
			Assert.AreEqual("x", tested.Name, "The IdentifierReference should be named 'x'.");
		}

		[Test]
		public void SomethingShouldBeCreatedForAssignment()
		{
			ITweedleExpression tested = ParseExpression("x <- 3");
			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void AnAssignmentExpressionShouldBeCreated()
		{
			ITweedleExpression tested = ParseExpression("x <- 3");
			Assert.IsInstanceOf<AssignmentExpression>(tested, "The parser should have returned an AssignmentExpression.");
		}

		[Test]
		public void SomethingShouldBeCreatedForFieldReadOnThis()
		{
			ITweedleExpression tested = ParseExpression("this.myField");
			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void SFieldAccessExpressionOnThisShouldBeCreated()
		{
			ITweedleExpression tested = ParseExpression("this.myField");
			Assert.IsInstanceOf<FieldAccess>(tested, "The parser should have returned a FieldAccesss.");
		}

		[Test]
		public void AFieldAccessOnThisShouldHaveMyField()
		{
			FieldAccess tested = (FieldAccess)ParseExpression("this.myField");
			Assert.AreEqual("myField", tested.FieldName, "The field name should have been \"myField\".");
		}

		[Test]
		public void AFieldAccessOnThisShouldTargetByThisExpression()
		{
			FieldAccess tested = (FieldAccess)ParseExpression("this.myField");
			Assert.IsInstanceOf<ThisExpression>(tested.Target, "The target should have been a ThisExpression.");
		}

		[Test]
		public void SomethingShouldBeCreatedForFieldRead()
		{
			ITweedleExpression tested = ParseExpression("x.myField");
			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void AFieldAccessExpressionShouldBeCreated()
		{
			ITweedleExpression tested = ParseExpression("x.myField");
			Assert.IsInstanceOf<FieldAccess>(tested, "The parser should have returned a FieldAccesss.");
		}

		[Test]
		public void AFieldAccessShouldHaveMyField()
		{
			FieldAccess tested = (FieldAccess)ParseExpression("x.myField");
			Assert.AreEqual("myField", tested.FieldName, "The field name should have been \"myField\".");
		}

		[Test]
		public void AFieldAccessShouldTargetByIdentifier()
		{
			FieldAccess tested = (FieldAccess)ParseExpression("x.myField");
			Assert.IsInstanceOf<IdentifierReference>(tested.Target, "The target should have been an IdentifierReference.");
		}

		[Test]
		public void AFieldAccessTargetShouldBeCreatedNamedX()
		{
			FieldAccess field = (FieldAccess)ParseExpression("x.myField");
			IdentifierReference tested = (IdentifierReference)field.Target;
			Assert.AreEqual("x", tested.Name, "The IdentifierReference should be named 'x'.");
		}

		[Test]
		public void SomethingShouldBeCreatedForMethodCall()
		{
			ITweedleExpression tested = ParseExpression("x.myMethod()");
			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void AMethodCallExpressionShouldBeCreated()
		{
			ITweedleExpression tested = ParseExpression("x.myMethod()");
			Assert.IsInstanceOf<MethodCallExpression>(tested, "The parser should have returned a MethodCallExpression.");
		}

		[Test]
		public void AMethodCallExpressionShouldHaveMyMethod()
		{
			MethodCallExpression tested = (MethodCallExpression)ParseExpression("x.myMethod()");
			Assert.AreEqual("myMethod", tested.MethodName, "The method name should have been \"myMethod\".");
		}

		[Test]
		public void AMethodCallShouldTargetByIdentifier()
		{
			MethodCallExpression tested = (MethodCallExpression)ParseExpression("x.myMethod()");
			Assert.IsInstanceOf<IdentifierReference>(tested.Target, "The target should have been an IdentifierReference.");
		}

		[Test]
		public void AMethodCallTargetShouldBeCreatedNamedX()
		{
			MethodCallExpression method = (MethodCallExpression)ParseExpression("x.myMethod()");
			IdentifierReference tested = (IdentifierReference)method.Target;
			Assert.AreEqual("x", tested.Name, "The IdentifierReference should be named 'x'.");
		}

		[Test]
		public void SomethingShouldBeCreatedForMethodCallOnThis()
		{
			ITweedleExpression tested = ParseExpression("this.myMethod()");
			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void AMethodCallExpressionOnThisShouldBeCreated()
		{
			ITweedleExpression tested = ParseExpression("this.myMethod()");
			Assert.IsInstanceOf<MethodCallExpression>(tested, "The parser should have returned a MethodCallExpression.");
		}

		[Test]
		public void AMethodCallExpressionOnThisShouldHaveMyMethod()
		{
			MethodCallExpression tested = (MethodCallExpression)ParseExpression("this.myMethod()");
			Assert.AreEqual("myMethod", tested.MethodName, "The method name should have been \"myMethod\".");
		}

		[Test]
		public void AMethodCallShouldTargetThisExpression()
		{
			MethodCallExpression tested = (MethodCallExpression)ParseExpression("this.myMethod()");
			Assert.IsInstanceOf<ThisExpression>(tested.Target, "The target should have been a ThisExpression.");
		}

		[Test]
		[Ignore("Arrays not implemented")]
		public void SomethingShouldBeCreatedForPrimitiveArray()
		{
			ITweedleExpression tested = ParseExpression("new WholeNumber[] {3, 4, 5}");
			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		[Ignore("Arrays not implemented")]
		public void APrimitiveArrayInitializerShouldBeCreated()
		{
			ITweedleExpression tested = ParseExpression("new WholeNumber[] {3, 4, 5}");
			// Assert.IsInstanceOf<ArrayInitializer>(tested, "The parser should have returned a TweedleArrayInitializer.");
		}

		[Test]
		[Ignore("Arrays not implemented")]
		public void SomethingShouldBeCreatedForArrayIndex()
		{
			ITweedleExpression tested = ParseExpression("stuff[x]");
			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		[Ignore("Arrays not implemented")]
		public void AnArrayIndexShouldBeCreated()
		{
			ITweedleExpression tested = ParseExpression("stuff[x]");
			// Assert.IsInstanceOf<ArrayIndexExpression>(tested, "The parser should have returned an ArrayIndexExpression.");
		}

		[Test]
		[Ignore("Arrays not implemented")]
		public void SomethingShouldBeCreatedForObjectArray()
		{
			ITweedleExpression tested = ParseExpression("new SModel[] {this.sphere, this.walrus}");
			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		[Ignore("Arrays not implemented")]
		public void AnObjectArrayInitializerShouldBeCreated()
		{
			ITweedleExpression tested = ParseExpression("new SModel[] {this.sphere, this.walrus}");
			// Assert.IsInstanceOf<ArrayInitializer>(tested, "The parser should have returned a TweedleArrayInitializer.");
		}
	}
}

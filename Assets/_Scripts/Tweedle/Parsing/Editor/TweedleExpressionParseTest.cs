using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace Alice.Tweedle.Parsed
{
	public class TweedleExpressionParseTest
	{
		private TweedleExpression ParseExpression(string src)
		{
			return new TweedleParser().ParseExpression(src);
		}

		[Test]
		public void SomethingShouldBeCreatedForParenthesizedNumber()
		{
			TweedleExpression tested = ParseExpression("(3)");
			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void ANumberShouldBeCreatedForParenthesizedNumber()
		{
			TweedleExpression tested = ParseExpression("(3)");
			Assert.IsInstanceOf<TweedlePrimitiveValue>(tested, "The parser should have returned a TweedlePrimitiveValue.");
		}

		[Test]
		public void ANumberShouldBeCreatedForMultiplyParenthesizedNumber()
		{
			TweedleExpression tested = ParseExpression("(((3)))");
			Assert.IsInstanceOf<TweedlePrimitiveValue>(tested, "The parser should have returned aTweedlePrimitiveValue.");
		}

		[Test]
		public void SomethingShouldBeCreatedForThis()
		{
			TweedleExpression tested = ParseExpression("this");

			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void AThisExpressionShouldBeCreated()
		{
			TweedleExpression tested = ParseExpression("this");

			Assert.IsInstanceOf<ThisExpression>(tested, "The parser should have returned a ThisExpression.");
		}

		[Test]
		public void SomethingShouldBeCreatedForConcatenation()
		{
			TweedleExpression tested = ParseExpression("\"hello\" .. \" there\"");

			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void AConcatenationExpressionShouldBeCreated()
		{
			TweedleExpression tested = ParseExpression("\"hello\" .. \" there\"");

			Assert.IsInstanceOf<StringConcatenationExpression>(tested, "The parser should have returned an StringConcatenationExpression.");
		}

		[Test]
		public void CreatedConcatenationExpressionShouldEvaluate()
		{
			StringConcatenationExpression tested = (StringConcatenationExpression)ParseExpression("\"hello\" .. \" there\"");

			Assert.NotNull(tested.Evaluate(null), "The StringConcatenationExpression should evaluate to something.");
		}

		[Test]
		public void CreatedConcatenationExpressionShouldEvaluateToAPrimitiveValue()
		{
			StringConcatenationExpression tested = (StringConcatenationExpression)ParseExpression("\"hello\" .. \" there\"");

			Assert.IsInstanceOf<TweedlePrimitiveValue>(tested.Evaluate(null), "The StringConcatenationExpression should evaluate to a tweedle value.");
		}

		[Test]
		public void CreatedConcatenationExpressionShouldContainCorrectResult()
		{
			StringConcatenationExpression tested = (StringConcatenationExpression)ParseExpression("\"hello\" .. \" there\"");

			Assert.AreEqual("hello there",
				((TweedlePrimitiveValue<string>)tested.Evaluate(null)).Value,
							"The StringConcatenationExpression should evaluate correctly.");
		}

		[Test]
		public void ConcatenationShouldKnowItsType()
		{
			TweedleExpression tested = ParseExpression("\"hello\" .. \" there\"");
			Assert.AreEqual(TweedleTypes.TEXT_STRING,
				tested.Type,
				"The type should be TextString");
		}

		[Test]
		public void SomethingShouldBeCreatedForAddition()
		{
			TweedleExpression tested = ParseExpression("3 + 4");

			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void AnAdditionExpressionShouldBeCreated()
		{
			TweedleExpression tested = ParseExpression("3 + 4");

			Assert.IsInstanceOf<AdditionWholeExpression>(tested, "The parser should have returned an AdditionExpression.");
		}

		/*	[Test]
			public void additionExpressionShouldHaveLeft() {
				AdditionExpression tested = (AdditionExpression) ParseExpression( "3 + 4" );
				Assert.True("The AdditionExpression should have a left hand side.", tested. );
			}*/

		[Test]
		public void CreatedAdditionExpressionShouldEvaluate()
		{
			AdditionWholeExpression tested = (AdditionWholeExpression)ParseExpression("3 + 4");

			Assert.NotNull(tested.Evaluate(null), "The AdditionExpression should evaluate to something.");
		}

		[Test]
		public void CreatedAdditionExpressionShouldEvaluateToAPrimitiveValue()
		{
			AdditionWholeExpression tested = (AdditionWholeExpression)ParseExpression("3 + 4");

			Assert.IsInstanceOf<TweedlePrimitiveValue>(tested.Evaluate(null), "The AdditionExpression should evaluate to a tweedle value.");
		}

		[Test]
		public void CreatedAdditionExpressionShouldContainCorrectResult()
		{
			AdditionWholeExpression tested = (AdditionWholeExpression)ParseExpression("3 + 4");

			Assert.AreEqual(7,
				((TweedlePrimitiveValue<int>)tested.Evaluate(null)).Value,
				"The AdditionExpression should evaluate correctly.");
		}

		[Test]
		public void WholeNumberAdditionShouldKnowItsType()
		{
			TweedleExpression tested = ParseExpression("3 + 4");
			Assert.AreEqual(TweedleTypes.WHOLE_NUMBER,
				tested.Type,
				"The type should be WholeNumber");
		}

		[Test]
		public void DecimalNumberMultiplicationShouldKnowItsType()
		{
			TweedleExpression tested = ParseExpression("3.4 + 4.1");
			Assert.AreEqual(TweedleTypes.DECIMAL_NUMBER, tested.Type, "The type should be DecimalNumber");
		}

		[Test]
		public void SomethingShouldBeCreatedForStringConcat()
		{
			TweedleExpression tested = ParseExpression("\"Hello\" .. \" there\"");

			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void StringConcatShouldBeCreatedForStringConcat()
		{
			TweedleExpression tested = ParseExpression("\"Hello\" .. \" there\"");

			Assert.IsInstanceOf<StringConcatenationExpression>(tested, "The parser should have returned an StringConcatenationExpression.");
		}

		[Test]
		public void SomethingShouldBeCreatedForIdentifier()
		{
			TweedleExpression tested = ParseExpression("x");
			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void AnIdentifierExpressionShouldBeCreated()
		{
			TweedleExpression tested = ParseExpression("x");
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
			TweedleExpression tested = ParseExpression("x <- 3");
			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void AnAssignmentExpressionShouldBeCreated()
		{
			TweedleExpression tested = ParseExpression("x <- 3");
			Assert.IsInstanceOf<AssignmentExpression>(tested, "The parser should have returned an AssignmentExpression.");
		}

		[Test]
		public void SomethingShouldBeCreatedForFieldReadOnThis()
		{
			TweedleExpression tested = ParseExpression("this.myField");
			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void SFieldAccessExpressionOnThisShouldBeCreated()
		{
			TweedleExpression tested = ParseExpression("this.myField");
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
			TweedleExpression tested = ParseExpression("x.myField");
			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void AFieldAccessExpressionShouldBeCreated()
		{
			TweedleExpression tested = ParseExpression("x.myField");
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
			TweedleExpression tested = ParseExpression("x.myMethod()");
			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void AMethodCallExpressionShouldBeCreated()
		{
			TweedleExpression tested = ParseExpression("x.myMethod()");
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
			TweedleExpression tested = ParseExpression("this.myMethod()");
			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void AMethodCallExpressionOnThisShouldBeCreated()
		{
			TweedleExpression tested = ParseExpression("this.myMethod()");
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
		public void SomethingShouldBeCreatedForPrimitiveArray()
		{
			TweedleExpression tested = ParseExpression("new WholeNumber[] {3, 4, 5}");
			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void APrimitiveArrayInitializerShouldBeCreated()
		{
			TweedleExpression tested = ParseExpression("new WholeNumber[] {3, 4, 5}");
			Assert.IsInstanceOf<ArrayInitializer>(tested, "The parser should have returned a TweedleArrayInitializer.");
		}

		[Test]
		public void SomethingShouldBeCreatedForObjectArray()
		{
			TweedleExpression tested = ParseExpression("new SModel[] {this.sphere, this.walrus}");
			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void AnObjectArrayInitializerShouldBeCreated()
		{
			TweedleExpression tested = ParseExpression("new SModel[] {this.sphere, this.walrus}");
			Assert.IsInstanceOf<ArrayInitializer>(tested, "The parser should have returned a TweedleArrayInitializer.");
		}
	}
}

using NUnit.Framework;

namespace Alice.Tweedle.Parse
{
	[TestFixture]
	public class TweedleLiteralParseTest
	{
		TweedleExpression ParseExpression(string src)
		{
			return new TweedleParser().ParseExpression(src);
		}

		[Test]
		public void SomethingShouldBeCreatedForNullLiteral()
		{
			TweedleExpression tested = ParseExpression("null");

			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void ATweedleNullShouldBeReturnedForNullLiteral()
		{
			TweedleExpression tested = ParseExpression("null");

			Assert.IsInstanceOf<TweedleNull>(tested, "The parser should have returned a TweedleNull.");
		}

		[Test]
		public void TheNullLiteralShouldParseToTheNullSingleton()
		{
			TweedleExpression tested = ParseExpression("null");

			Assert.AreEqual(TweedleNull.NULL, tested, "The literal should be the single null object.");
		}

		[Test]
		public void SomethingShouldBeCreatedForBooleanLiteral()
		{
			TweedleExpression tested = ParseExpression("true");

			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void APrimitiveValueShouldBeCreatedForBooleanLiteral()
		{
			TweedleExpression tested = ParseExpression("true");

			Assert.IsInstanceOf<TweedlePrimitiveValue<bool>>(tested, "The parser should have returned a TweedlePrimitiveValue.");
		}

		[Test]
		public void TheValueInTheLiteralCreatedForBooleanLiteralTrueShouldMatch()
		{
			TweedlePrimitiveValue<bool> tested = (TweedlePrimitiveValue<bool>)ParseExpression("true");

			Assert.AreEqual(true, tested.Value, "The literal should hold the value true.");
		}

		[Test]
		public void TheValueInTheLiteralCreatedForBooleanLiteralFalseShouldMatch()
		{
			TweedlePrimitiveValue<bool> tested = (TweedlePrimitiveValue<bool>)ParseExpression("false");

			Assert.AreEqual(false, tested.Value, "The literal should hold the value false.");
		}

		[Test]
		public void SomethingShouldBeCreatedForStringLiteral()
		{
			TweedleExpression tested = ParseExpression("\"hello\"");

			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void APrimitiveValueShouldBeCreatedForStringLiteral()
		{
			TweedleExpression tested = ParseExpression("\"hello\"");

			Assert.IsInstanceOf<TweedlePrimitiveValue<string>>(tested, "The parser should have returned a TweedlePrimitiveValue.");
		}

		[Test]
		public void TheValueInTheLiteralCreatedForStringLiteralShouldMatch()
		{
			TweedlePrimitiveValue<string> tested = (TweedlePrimitiveValue<string>)ParseExpression("\"hello\"");

			Assert.AreEqual("hello", tested.Value, "The literal should hold the value.");
		}

		[Test]
		public void StringLiteralShouldAllowEscapes()
		{
			TweedlePrimitiveValue<string> tested = (TweedlePrimitiveValue<string>)ParseExpression("\"hello \\\"someone\\\"\"");

			Assert.AreEqual("hello \\\"someone\\\"", tested.Value, "The literal should hold the value.");
		}

		[Test]
		public void SomethingShouldBeCreatedForDecimalLiteral()
		{
			TweedleExpression tested = ParseExpression("4.731");

			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void APrimitiveValueShouldBeCreatedForDecimalLiteral()
		{
			TweedleExpression tested = ParseExpression("4.731");

			Assert.IsInstanceOf<TweedlePrimitiveValue<double>>(tested, "The parser should have returned a TweedlePrimitiveValue.");
		}

		[Test]
		public void aPrimitiveValueShouldBeInTheLiteralCreatedForDecimalLiteral()
		{
			TweedlePrimitiveValue<double> tested = (TweedlePrimitiveValue<double>)ParseExpression("4.731");

			Assert.IsInstanceOf<TweedlePrimitiveValue<double>>(tested.EvaluateNow(), "The parser should have returned a TweedlePrimitiveValue.");
		}

		[Test]
		public void TheValueInTheLiteralCreatedForDecimalLiteralShouldMatch()
		{
			TweedlePrimitiveValue<double> tested = (TweedlePrimitiveValue<double>)ParseExpression("4.731");

			Assert.AreEqual(4.731, tested.Value, "The literal should hold the value.");
		}

		[Test]
		public void SomethingShouldBeCreatedForNumberLiteral()
		{
			TweedleExpression tested = ParseExpression("4");

			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void APrimitiveValueShouldBeCreatedForNumberLiteral()
		{
			TweedleExpression tested = ParseExpression("4");

			Assert.IsInstanceOf<TweedlePrimitiveValue<int>>(tested, "The parser should have returned a TweedlePrimitiveValue.");
		}

		[Test]
		public void APrimitiveValueShouldBeInTheLiteralCreatedForNumberLiteral()
		{
			TweedlePrimitiveValue<int> tested = (TweedlePrimitiveValue<int>)ParseExpression("4");

			Assert.IsInstanceOf<TweedlePrimitiveValue<int>>(tested.EvaluateNow(), "The parser should have returned a TweedlePrimitiveValue.");
		}

		[Test]
		public void TheValueInTheLiteralCreatedForNumberLiteralShouldMatch()
		{
			TweedlePrimitiveValue<int> tested = (TweedlePrimitiveValue<int>)ParseExpression("4");
			TweedlePrimitiveValue<int> tweedleValue = (TweedlePrimitiveValue<int>)tested.EvaluateNow();

			Assert.AreEqual(4, tweedleValue.Value, "The literal should hold the value.");
		}

		[Test]
		public void SomethingShouldBeCreatedForNegativeNumberLiteral()
		{
			TweedleExpression tested = ParseExpression("-4");

			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void APrimitiveValueShouldBeCreatedForNegativeNumberLiteral()
		{
			TweedleExpression tested = ParseExpression("-4");

			Assert.IsInstanceOf<TweedlePrimitiveValue<int>>(tested, "The parser should have returned a TweedlePrimitiveValue.");
		}
	}
}

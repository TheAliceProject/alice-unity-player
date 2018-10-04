using NUnit.Framework;

namespace Alice.Tweedle.Parse
{
    [TestFixture]
    public class TweedleLiteralParseTest
    {
        ITweedleExpression ParseExpression(string src)
        {
            return new TweedleParser().ParseExpression(src);
        }

        [Test]
        public void SomethingShouldBeCreatedForNullLiteral()
        {
            ITweedleExpression tested = ParseExpression("null");

            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void ATweedleNullShouldBeReturnedForNullLiteral()
        {
            ITweedleExpression tested = ParseExpression("null");

            Assert.IsInstanceOf<TNullType>(tested.Type.Get(), "The parser should have returned a TweedleNull.");
        }

        [Test]
        public void TheNullLiteralShouldParseToTheNullSingleton()
        {
            ITweedleExpression tested = ParseExpression("null");

            Assert.AreEqual(TValue.NULL, tested, "The literal should be the single null object.");
        }

        [Test]
        public void SomethingShouldBeCreatedForBooleanLiteral()
        {
            ITweedleExpression tested = ParseExpression("true");

            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void APrimitiveValueShouldBeCreatedForBooleanLiteral()
        {
            ITweedleExpression tested = ParseExpression("true");

            Assert.IsInstanceOf<TBooleanType>(tested.Type.Get(), "The parser should have returned a TweedlePrimitiveValue.");
        }

        [Test]
        public void TheValueInTheLiteralCreatedForBooleanLiteralTrueShouldMatch()
        {
            TValue tested = (TValue)ParseExpression("true");

            Assert.AreEqual(true, tested.ToBoolean(), "The literal should hold the value true.");
        }

        [Test]
        public void TheValueInTheLiteralCreatedForBooleanLiteralFalseShouldMatch()
        {
            TValue tested = (TValue)ParseExpression("false");

            Assert.AreEqual(false, tested.ToBoolean(), "The literal should hold the value false.");
        }

        [Test]
        public void SomethingShouldBeCreatedForStringLiteral()
        {
            ITweedleExpression tested = ParseExpression("\"hello\"");

            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void APrimitiveValueShouldBeCreatedForStringLiteral()
        {
            ITweedleExpression tested = ParseExpression("\"hello\"");

            Assert.IsInstanceOf<TTextStringType>(tested.Type.Get(), "The parser should have returned a TweedlePrimitiveValue.");
        }

        [Test]
        public void TheValueInTheLiteralCreatedForStringLiteralShouldMatch()
        {
            TValue tested = (TValue)ParseExpression("\"hello\"");

            Assert.AreEqual("hello", tested.ToTextString(), "The literal should hold the value.");
        }

        [Test]
        public void StringLiteralShouldAllowEscapes()
        {
            TValue tested = (TValue)ParseExpression("\"hello \\\"someone\\\"\"");

            Assert.AreEqual("hello \\\"someone\\\"", tested.ToTextString(), "The literal should hold the value.");
        }

        [Test]
        public void SomethingShouldBeCreatedForDecimalLiteral()
        {
            ITweedleExpression tested = ParseExpression("4.731");

            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void APrimitiveValueShouldBeCreatedForDecimalLiteral()
        {
            ITweedleExpression tested = ParseExpression("4.731");

            Assert.IsInstanceOf<TDecimalNumberType>(tested.Type.Get(), "The parser should have returned a TweedlePrimitiveValue.");
        }

        [Test]
        public void aPrimitiveValueShouldBeInTheLiteralCreatedForDecimalLiteral()
        {
            TValue tested = (TValue)ParseExpression("4.731");

            Assert.IsInstanceOf<TDecimalNumberType>(tested.EvaluateNow().Type, "The parser should have returned a TweedlePrimitiveValue.");
        }

        [Test]
        public void TheValueInTheLiteralCreatedForDecimalLiteralShouldMatch()
        {
            TValue tested = (TValue)ParseExpression("4.731");

            Assert.AreEqual(4.731, tested.ToDouble(), "The literal should hold the value.");
        }

        [Test]
        public void SomethingShouldBeCreatedForNumberLiteral()
        {
            ITweedleExpression tested = ParseExpression("4");

            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void APrimitiveValueShouldBeCreatedForNumberLiteral()
        {
            ITweedleExpression tested = ParseExpression("4");

            Assert.IsInstanceOf<TWholeNumberType>(tested.Type.Get(), "The parser should have returned a TweedlePrimitiveValue.");
        }

        [Test]
        public void APrimitiveValueShouldBeInTheLiteralCreatedForNumberLiteral()
        {
            TValue tested = (TValue)ParseExpression("4");

            Assert.IsInstanceOf<TWholeNumberType>(tested.EvaluateNow().Type, "The parser should have returned a TweedlePrimitiveValue.");
        }

        [Test]
        public void TheValueInTheLiteralCreatedForNumberLiteralShouldMatch()
        {
            TValue tested = (TValue)ParseExpression("4");
            TValue tweedleValue = (TValue)tested.EvaluateNow();

            Assert.AreEqual(4, tweedleValue.ToInt(), "The literal should hold the value.");
        }

        [Test]
        public void SomethingShouldBeCreatedForNegativeNumberLiteral()
        {
            ITweedleExpression tested = ParseExpression("-4");

            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void APrimitiveValueShouldBeCreatedForNegativeNumberLiteral()
        {
            ITweedleExpression tested = ParseExpression("-4");

            Assert.IsInstanceOf<TWholeNumberType>(tested.Type.Get(), "The parser should have returned a TweedlePrimitiveValue.");
        }
    }
}

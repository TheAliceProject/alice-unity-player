using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace Alice.Tweedle.Unlinked
{
	public class TweedleParseTest
	{
		private TweedleType ParseString(string src)
		{
			TweedleUnlinkedParser t = new TweedleUnlinkedParser();
			return t.Parse(src);
		}

		[Test]
		public void SomethingShouldBeCreatedForARootClass()
		{
			TweedleClass tested = (TweedleClass)ParseString("class SThing {}");
			Assert.NotNull(tested, "The parser should have returned something.");
		}


		[Test]
		public void ARootClassShouldBeCreated()
		{
			TweedleClass tested = (TweedleClass)ParseString("class SThing {}");

			Assert.IsInstanceOf<TweedleClass>(tested, "The parser should have returned a UnlinkedClass.");
		}

		[Test]
		public void ClassShouldKnowItsName()
		{
			TweedleClass tested = (TweedleClass)ParseString("class SThing {}");

			Assert.AreEqual("SThing", tested.Name, "The name should be 'SThing'");
		}

		[Test]
		public void SubclassOfBooleanPrimitiveShouldFail()
		{
			Assert.Throws<System.NullReferenceException>(delegate ()
			{
				ParseString("class SScene extends Boolean {}");
			});
		}

		[Test]
		public void SubclassOfDecimalPrimitiveShouldFail()
		{
			Assert.Throws<System.NullReferenceException>(delegate ()
			{
				ParseString("class SScene extends DecimalNumber {}");
			});
		}

		[Test]
		public void SubclassOfWholePrimitiveShouldFail()
		{
			Assert.Throws<System.NullReferenceException>(delegate ()
			{
				ParseString("class SScene extends WholeNumber {}");
			});
		}

		[Test]
		public void SubclassOfNumberPrimitiveShouldFail()
		{
			Assert.Throws<System.NullReferenceException>(delegate ()
			{
				ParseString("class SScene extends Number {}");
			});
		}

		[Test]
		public void SubclassOfStringPrimitiveShouldFail()
		{
			Assert.Throws<System.NullReferenceException>(delegate ()
			{
				ParseString("class SScene extends String {}");
			});
		}

		//[Test]
		//public void EnumNamedSameAsBooleanPrimitiveShouldFail()
		//{
		//	Assert.That(() => ParseString("enum Boolean {TRUE, FALSE}"),
		//		Throws.TypeOf<Antlr4.Runtime.Misc.ParseCanceledException>());
		//}

		//[Test]
		//public void ClassNamedSameAsBooleanPrimitiveShouldFail()
		//{
		//	Assert.That(() => ParseString("class Boolean {}"),
		//		Throws.TypeOf<Antlr4.Runtime.Misc.ParseCanceledException>());
		//}

		[Test]
		public void SomethingShouldBeCreatedForASubclass()
		{
			TweedleClass tested = (TweedleClass)ParseString("class SScene extends SThing {}");

			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void ASubclassShouldBeCreated()
		{
			TweedleClass tested = (TweedleClass)ParseString("class SScene extends SThing {}");

			Assert.IsInstanceOf<TweedleClass>(tested, "The parser should have returned a UnlinkedClass.");
		}

		[Test]
		public void ClassNameShouldBeReturnedOnSubclass()
		{
			TweedleClass tested = (TweedleClass)ParseString("class SScene extends SThing {}");

			Assert.AreEqual("SScene",
							tested.Name,
							"The class name should have been SScene.");
		}

		[Test]
		public void SuperclassNameShouldBeReturnedOnSubclass()
		{
			TweedleClass sScene = (TweedleClass)ParseString("class SScene extends SThing {}");

			Assert.AreEqual("SThing",
							sScene.Super.Name,
							"The class SScene should have a superclass SThing.");
		}

		[Test]
		public void SomethingShouldBeCreatedForAnEnum()
		{
			TweedleEnum tested = (TweedleEnum)ParseString("enum Direction {UP, DOWN}");

			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void AnEnumShouldBeCreated()
		{
			TweedleEnum tested = (TweedleEnum)ParseString("enum Direction {UP, DOWN}");

			Assert.IsInstanceOf<TweedleEnum>(tested, "The parser should have returned a UnlinkedEnum.");
		}

		[Test]
		public void NameShouldBeReturnedOnEnum()
		{
			TweedleEnum tested = (TweedleEnum)ParseString("enum Direction {UP, DOWN}");

			Assert.AreEqual("Direction",
							tested.Name,
							"The enum name should have been Direction.");
		}

		[Test]
		public void EnumShouldIncludeTwoValues()
		{
			TweedleEnum directionEnum = (TweedleEnum)ParseString("enum Direction {UP, DOWN}");

			Assert.AreEqual(2,
							directionEnum.Values.Count,
							"The enum Direction should have two values.");
		}

		[Test]
		public void EnumShouldIncludeUpValue()
		{
			TweedleEnum directionEnum = (TweedleEnum)ParseString("enum Direction {UP, DOWN}");

			Assert.True(directionEnum.Values.Contains("UP"),
						"The enum Direction should include UP.");
		}

		[Test]
		public void EnumShouldIncludeDownValue()
		{
			TweedleEnum directionEnum = (TweedleEnum)ParseString("enum Direction {UP, DOWN}");

			Assert.True(directionEnum.Values.Contains("DOWN"),
						"The enum Direction should include DOWN.");
		}

		[Test]
		public void EnumShouldNotIncludeLeftValue()
		{
			TweedleEnum directionEnum = (TweedleEnum)ParseString("enum Direction {UP, DOWN}");

			Assert.False(directionEnum.Values.Contains("LEFT"),
						 "The enum Direction should not include LEFT.");
		}

	}
}
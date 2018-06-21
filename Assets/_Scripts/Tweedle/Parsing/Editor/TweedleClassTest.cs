using Alice.VM;
using NUnit.Framework;

namespace Alice.Tweedle.Parsed
{
    [TestFixture]
    public class TweedleClassTest
    {
		static TweedleClass ParseClass(string src)
        {
            return (TweedleClass)new TweedleParser().ParseType(src);
		}

		static string classToHave =
			"class ClassToHave {\n"
			+ "  ClassToHave()\n{ x <- 5; }\n"
			+ "  ClassToHave(WholeNumber start)\n{ x <- start; }\n"
			+ "  static WholeNumber getNumber() {"
			+ "    return 21;\n"
            + "  }\n"
			+ "  WholeNumber x;\n"
            + "  WholeNumber threeMore() {\n"
            + "    return 3 + x;\n"
            + "  }\n"
            + "}";

        static string parentClass =
            "class Parent {\n"
			+ "  Parent()\n{\n"
			+ "    myConstructedThing <- new ClassToHave(start: 27);\n"
			+ "    myReplacedThing <- new ClassToHave(start: -5);\n"
			+ "}\n"
			+ "  ClassToHave myThing <- new ClassToHave();"
			+ "  ClassToHave mySpecialThing <- new ClassToHave(start: 100 - 3);"
			+ "  ClassToHave myConstructedThing;"
			+ "  ClassToHave myReplacedThing <- new ClassToHave(start: 100);"
            + "static void main(TextString[] args) {"
            + "            Program story<-new Program();"
            + "            story.initializeInFrame(args: args);"
			+ "            story.setActiveScene(scene: story.getMyScene());\n }"
            + "ClassToHave getMyThing() {\n"
			+ "  return this.myThing;\n }\n"
            + "ClassToHave getMySpecialThing() {\n"
            + "  return mySpecialThing;\n }\n}";

		static string childClass =
			"class Child extends Parent {\n"
			+ "  Child()\n{ super();"
			+ "    myReplacedThing.x <- 1000;}\n"
            + "\n}";

		TweedleSystem NewSystem()
		{
			TweedleSystem system = new TweedleSystem();
			system.AddClass(ParseClass(classToHave));
			system.AddClass(ParseClass(parentClass));
			system.AddClass(ParseClass(childClass));
			return system;
		}

		VirtualMachine vm;
		TweedleFrame frame;

		public void Init()
		{
			vm = new VirtualMachine(NewSystem());
            frame = new TweedleFrame(vm);
		}

        void ExecuteStatement(string src)
        {
			Init();
            TweedleStatement stmt = new TweedleParser().ParseStatement(src);
            vm.ExecuteToFinish(stmt, frame);
        }

        [Test]
        public void AClassShouldBeCreatedForClassToHave()
        {
			TweedleClass tested = ParseClass(classToHave);
            Assert.NotNull(tested, "The parser should have returned something.");
		}

        [Test]
        public void AClassShouldBeCreatedForParentClass()
        {
            TweedleClass tested = ParseClass(parentClass);
            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void AClassShouldBeCreatedForChildClass()
        {
			TweedleClass tested = ParseClass(childClass);
            Assert.NotNull(tested, "The parser should have returned something.");
		}

        [Test]
        public void StaticMethodShouldReturnValue()
        {
            ExecuteStatement("WholeNumber val <- ClassToHave.getNumber();");
            var tested = frame.GetValue("val");

			Assert.IsInstanceOf<TweedlePrimitiveValue<int>>(tested);
		}

        [Test]
        public void StaticMethodShouldReturnCorrectValue()
        {
            ExecuteStatement("WholeNumber val <- ClassToHave.getNumber();");
			TweedlePrimitiveValue<int> tested = (TweedlePrimitiveValue<int>)frame.GetValue("val");

			Assert.AreEqual(21, tested.Value);
        }

        [Test]
        public void AnObjectShouldBeCreatedByConstruction()
        {
            ExecuteStatement("ClassToHave obj <- new ClassToHave();");
            var tested = frame.GetValue("obj");

            Assert.IsInstanceOf<TweedleObject>(tested);
        }
    }
}
using Alice.VM;
using NUnit.Framework;

namespace Alice.Tweedle.Parse
{
	[TestFixture]
	public class TClassTypeTest
	{
		static TweedleParser parser = new TweedleParser();

		static TClassType ParseClass(string src)
		{
			return (TClassType)parser.ParseType(src);
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
			+ "  WholeNumber thisless() {\n"
			+ "    return threeMore();\n"
			+ "  }\n"
			+ "  WholeNumber doParallelArraySteps() {\n"
			+ "    WholeNumber x <- 0;"
			// + "    eachTogether(WholeNumber c in new WholeNumber[] {5,2,3} ) { x <- x + c; }"
			+ "    return x;\n"
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
			+ "static void main(TextString args) {" // + "static void main(TextString[] args) {"
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
			+ "ClassToHave getMyThing() {\n"
			+ "  ClassToHave par <- super.getMyThing();\n"
			+ "  par.x <- par.x + 10;\n"
			+ "  return par;\n }\n"
			+ "\n}";

		TweedleSystem NewSystem()
		{
			TweedleSystem system = new TweedleSystem();
			system.AddType(ParseClass(classToHave));
			system.AddType(ParseClass(parentClass));
			system.AddType(ParseClass(childClass));
            system.Link();
            return system;
		}

		TestVirtualMachine vm;
		ExecutionScope scope;

		public void Init()
		{
			vm = new TestVirtualMachine(NewSystem());
			scope = new ExecutionScope("Test", vm);
		}

		void ExecuteStatement(string src)
		{
			vm.ExecuteToFinish(parser.ParseStatement(src), scope);
		}

		[Test]
		public void AClassShouldBeCreatedForClassToHave()
		{
			TClassType tested = ParseClass(classToHave);
			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void AClassShouldBeCreatedForParentClass()
		{
			TClassType tested = ParseClass(parentClass);
			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void AClassShouldBeCreatedForChildClass()
		{
			TClassType tested = ParseClass(childClass);
			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void StaticMethodShouldReturnValue()
		{
			Init();
			ExecuteStatement("WholeNumber val <- ClassToHave.getNumber();");
			var tested = scope.GetValue("val");

			Assert.IsInstanceOf<TWholeNumberType>(tested.Type);
		}

		[Test]
		public void StaticMethodShouldReturnCorrectValue()
		{
			Init();
			ExecuteStatement("WholeNumber val <- ClassToHave.getNumber();");
			TValue tested = scope.GetValue("val");

			Assert.AreEqual(21, tested.ToInt());
		}

		[Test]
		public void AnObjectShouldBeCreatedByConstruction()
		{
			Init();
			ExecuteStatement("ClassToHave obj <- new ClassToHave();");
			var tested = scope.GetValue("obj");

			Assert.IsInstanceOf<TObject>(tested.Object());
		}

		[Test]
		public void CreatedObjectShouldHaveFieldSet()
		{
			Init();
			ExecuteStatement("ClassToHave obj <- new ClassToHave();");
			TObject tested = scope.GetValue("obj").Object();

			Assert.IsInstanceOf<TWholeNumberType>(tested.Get("x").Type);
		}

		[Test]
		public void CreatedObjectShouldHaveDefaultValueOnField()
		{
			Init();
			ExecuteStatement("ClassToHave obj <- new ClassToHave();");
			TObject tested = scope.GetValue("obj").Object();

			Assert.AreEqual(5, tested.Get("x").ToInt());
		}

		[Test]
		public void AnObjectShouldBeCreatedByConstructionWithArg()
		{
			Init();
			ExecuteStatement("ClassToHave obj <- new ClassToHave(start: 100);");
			var tested = scope.GetValue("obj");

			Assert.IsInstanceOf<TObject>(tested.Object());
		}

		[Test]
		public void ObjectConstructedWithArgShouldHaveFieldSet()
		{
			Init();
			ExecuteStatement("ClassToHave obj <- new ClassToHave(start: 100);");
			TObject tested = scope.GetValue("obj").Object();

			Assert.IsInstanceOf<TWholeNumberType>(tested.Get("x").Type);
		}

		[Test]
		public void ObjectConstructedWithArgShouldHaveSetValueOnField()
		{
			Init();
			ExecuteStatement("ClassToHave obj <- new ClassToHave(start: 100);");
			TObject tested = scope.GetValue("obj").Object();

			Assert.AreEqual(100, tested.Get("x").ToInt());
		}

		[Test]
		public void MethodOnCreatedObjectShouldProduceResult()
		{
			Init();
			ExecuteStatement("ClassToHave obj <- new ClassToHave();");
			ExecuteStatement("WholeNumber val  <- obj.threeMore();");
			TValue tested = scope.GetValue("val");

			Assert.IsInstanceOf<TWholeNumberType>(tested.Type);
		}

		[Test]
		public void CreatedObjectShouldReadFieldInMethod()
		{
			Init();
			ExecuteStatement("ClassToHave obj <- new ClassToHave();");
			ExecuteStatement("WholeNumber val  <- obj.threeMore();");
			TValue tested = scope.GetValue("val");

			Assert.AreEqual(8, tested.ToInt());
		}

		[Test]
		public void MethodWithImplicitThisShouldProduceResult()
		{
			Init();
			ExecuteStatement("ClassToHave obj <- new ClassToHave();");
			ExecuteStatement("WholeNumber val  <- obj.thisless();");
			TValue tested = scope.GetValue("val");

			Assert.IsInstanceOf<TWholeNumberType>(tested.Type);
		}

		[Test]
		public void MethodWithImplicitThisShouldProduceCorrectResult()
		{
			Init();
			ExecuteStatement("ClassToHave obj <- new ClassToHave();");
			ExecuteStatement("WholeNumber val  <- obj.thisless();");
			TValue tested = scope.GetValue("val");

			Assert.AreEqual(8, tested.ToInt());
		}

		[Test]
		public void CreatedParentObjectShouldHaveDefaultObjectFieldSet()
		{
			Init();
			ExecuteStatement("Parent parent <- new Parent();");
			TObject tested = scope.GetValue("parent").Object();

			Assert.IsInstanceOf<TObject>(tested.Get("myThing").Object());
		}

		[Test]
		public void CreatedParentObjectShouldHaveDefaultObjectFieldSetWithCorrectValue()
		{
			Init();
			ExecuteStatement("Parent parent <- new Parent();");
			TObject tested = scope.GetValue("parent").Object();
			TObject fieldObject = tested.Get("myThing").Object();

			Assert.AreEqual(5, fieldObject.Get("x").ToInt());
		}

		[Test]
		public void CreatedParentObjectShouldHaveConstructorObjectFieldSet()
		{
			Init();
			ExecuteStatement("Parent parent <- new Parent();");
			TObject tested = scope.GetValue("parent").Object();

			Assert.IsInstanceOf<TObject>(tested.Get("myConstructedThing").Object());
		}

		[Test]
		public void CreatedParentObjectShouldHaveConstructorObjectFieldSetWithCorrectValue()
		{
			Init();
			ExecuteStatement("Parent parent <- new Parent();");
			TObject tested = scope.GetValue("parent").Object();
			TObject fieldObject = tested.Get("myConstructedThing").Object();

			Assert.AreEqual(27, fieldObject.Get("x").ToInt());
		}

		[Test]
		public void CreatedParentObjectShouldHaveConstructorObjectFieldOverrideSet()
		{
			Init();
			ExecuteStatement("Parent parent <- new Parent();");
			TObject tested = scope.GetValue("parent").Object();

			Assert.IsInstanceOf<TObject>(tested.Get("myReplacedThing").Object());
		}

		[Test]
		public void CreatedParentObjectShouldHaveConstructorObjectFieldOverrideSetWithCorrectValue()
		{
			Init();
			ExecuteStatement("Parent parent <- new Parent();");
			TObject tested = scope.GetValue("parent").Object();
			TObject fieldObject = tested.Get("myReplacedThing").Object();

			Assert.AreEqual(-5, fieldObject.Get("x").ToInt());
		}

		[Test]
		public void CreatedParentObjectShouldHaveComputedFieldSet()
		{
			Init();
			ExecuteStatement("Parent parent <- new Parent();");
			TObject tested = scope.GetValue("parent").Object();

			Assert.IsInstanceOf<TObject>(tested.Get("mySpecialThing").Object());
		}

		[Test]
		public void CreatedParentObjectShouldHaveComputedFieldSetWithCorrectValue()
		{
			Init();
			ExecuteStatement("Parent parent <- new Parent();");
			TObject tested = scope.GetValue("parent").Object();
			TObject fieldObject = tested.Get("mySpecialThing").Object();

			Assert.AreEqual(97, fieldObject.Get("x").ToInt());
		}

		[Test]
		public void CreatedChildObjectShouldHaveDefaultObjectFieldSet()
		{
			Init();
			ExecuteStatement("Child child <- new Child();");
			TObject tested = scope.GetValue("child").Object();

			Assert.IsInstanceOf<TObject>(tested.Get("myThing").Object());
		}

		[Test]
		public void CreatedChildObjectShouldHaveDefaultObjectFieldSetWithCorrectValue()
		{
			Init();
			ExecuteStatement("Child child <- new Child();");
			TObject tested = scope.GetValue("child").Object();
			TObject fieldObject = tested.Get("myThing").Object();

			Assert.AreEqual(5, fieldObject.Get("x").ToInt());
		}

		[Test]
		public void CreatedChildObjectShouldHaveConstructorObjectFieldSet()
		{
			Init();
			ExecuteStatement("Child child <- new Child();");
			TObject tested = scope.GetValue("child").Object();

			Assert.IsInstanceOf<TObject>(tested.Get("myConstructedThing").Object());
		}

		[Test]
		public void CreatedChildObjectShouldHaveConstructorObjectFieldSetWithCorrectValue()
		{
			Init();
			ExecuteStatement("Child child <- new Child();");
			TObject tested = scope.GetValue("child").Object();
			TObject fieldObject = tested.Get("myConstructedThing").Object();

			Assert.AreEqual(27, fieldObject.Get("x").ToInt());
		}

		[Test]
		public void CreatedChildObjectShouldHaveConstructorObjectFieldOverrideSet()
		{
			Init();
			ExecuteStatement("Child child <- new Child();");
			TObject tested = scope.GetValue("child").Object();

			Assert.IsInstanceOf<TObject>(tested.Get("myReplacedThing").Object());
		}

		[Test]
		public void CreatedChildObjectShouldHaveConstructorObjectFieldOverrideSetWithCorrectValue()
		{
			Init();
			ExecuteStatement("Child child <- new Child();");
			TObject tested = scope.GetValue("child").Object();
			TObject fieldObject = tested.Get("myReplacedThing").Object();

			Assert.AreEqual(1000, fieldObject.Get("x").ToInt());
		}

		[Test]
		public void CreatedChildObjectShouldHaveComputedFieldSet()
		{
			Init();
			ExecuteStatement("Child child <- new Child();");
			TObject tested = scope.GetValue("child").Object();

			Assert.IsInstanceOf<TObject>(tested.Get("mySpecialThing").Object());
		}

		[Test]
		public void CreatedChildObjectShouldHaveComputedFieldSetWithCorrectValue()
		{
			Init();
			ExecuteStatement("Child child <- new Child();");
			TObject tested = scope.GetValue("child").Object();
			TObject fieldObject = tested.Get("mySpecialThing").Object();

			Assert.AreEqual(97, fieldObject.Get("x").ToInt());
		}

		[Test]
		public void ChildShouldCallSuperInMethod()
		{
			Init();
			ExecuteStatement("Child child <- new Child();");
			ExecuteStatement("ClassToHave obj <- child.getMyThing();");
			TObject tested = scope.GetValue("obj").Object();

			Assert.AreEqual(15, tested.Get("x").ToInt());
		}

		[Test]
		public void EachInArrayInMethodShouldProduceNumericResult()
		{
			Init();
			ExecuteStatement("ClassToHave obj <- new ClassToHave();");
			ExecuteStatement("WholeNumber val  <- obj.doParallelArraySteps();");
			TValue tested = scope.GetValue("val");

			Assert.IsInstanceOf<TWholeNumberType>(tested.Type);
		}
	}
}

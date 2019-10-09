using System.Text.RegularExpressions;
using Alice.Tweedle.VM;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Alice.Tweedle.Parse
{
    [TestFixture]
    public class TweedleClassTest
    {
        static TweedleParser parser = new TweedleParser();

        static TClassType ParseClass(string src, TAssembly assembly = null)
        {
            return (TClassType)parser.ParseType(src, assembly);
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
            + "  WholeNumber optionalOrDefaultMore(WholeNumber n <- 3) {\n"
            + "    return n + x;\n"
            + "  }\n"
            + "  WholeNumber optionalOrDefaultMoreVariable(WholeNumber n <- x) {\n"
            + "    return n + x;\n"
            + "  }\n"
            + "  WholeNumber optionalOrDefaultEarlierVariable(WholeNumber a, WholeNumber n <- a) {\n"
            + "    return n + a;\n"
            + "  }\n"
            + "  WholeNumber requiredMore(WholeNumber n) {\n"
            + "    return n + x;\n"
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
            + "  WholeNumber myInnerValue <-  100;"
            + "static void main(TextString[] args) {"
            + "            Program story<-new Program();"
            + "            story.initializeInFrame(args: args);"
            + "            story.setActiveScene(scene: story.getMyScene());\n }"
            + "ClassToHave getMyThing() {\n"
            + "  return this.myThing;\n }\n"
            + "void doMyThing(WholeNumber value) {\n"
            + "  myInnerValue <- 4 + value;\n }\n"
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
            + "void doMyThing(WholeNumber value) {\n"
            + "  super.doMyThing(value: value + 1;\n }\n"
            + "\n}";

        static string grandchildClass =
            "class Grandchild extends Child {\n"
            + "  Grandchild()\n{ super();}\n"
            + "\n}";

        static string sibClass =
            "class Sibling extends Parent {\n"
            + "  Sibling()\n{ super();"
            + "}\n}";

        TweedleSystem NewSystem()
        {
            TweedleSystem system = new TweedleSystem();
            TAssembly assembly = system.GetRuntimeAssembly();
            assembly.Add(ParseClass(classToHave, assembly));
            assembly.Add(ParseClass(parentClass, assembly));
            assembly.Add(ParseClass(childClass, assembly));
            assembly.Add(ParseClass(grandchildClass, assembly));
            assembly.Add(ParseClass(sibClass, assembly));
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
        public void AClassShouldBeCreatedForGrandchildClass()
        {
            TClassType tested = ParseClass(grandchildClass);
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
        public void ChildShouldCallSuperWithArgumentInMethod()
        {
            Init();
            ExecuteStatement("Child child <- new Child();");
            ExecuteStatement("child.doMyThing(value: 2);");
            TObject tested = scope.GetValue("child").Object();

            Assert.AreEqual(7, tested.Get("myInnerValue").ToInt());
        }

        [Test]
        public void GrandchildShouldCallSuperWithArgumentInMethod()
        {
            Init();
            ExecuteStatement("Grandchild g <- new Grandchild();");
            ExecuteStatement("g.doMyThing(value: 3);");
            TObject tested = scope.GetValue("g").Object();

            Assert.AreEqual(8, tested.Get("myInnerValue").ToInt());
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

        [Test]
        public void MethodWithRequiredArgumentShouldExecuteAndReturn()
        {
            Init();
            ExecuteStatement("ClassToHave obj <- new ClassToHave();");
            ExecuteStatement("WholeNumber val  <- obj.requiredMore(n: 3);");
            TValue tested = scope.GetValue("val");

            Assert.AreEqual(8, tested.ToInt());
        }

        [Test]
        public void MethodWithRequiredArgumentShouldRequireArgument()
        {
            Init();
            ExecuteStatement("ClassToHave obj <- new ClassToHave();");

            LogAssert.Expect(UnityEngine.LogType.Error, new Regex("No method matching ClassToHave.requiredMore"));
            Assert.Throws<TweedleRuntimeException>(()=> {
                ExecuteStatement("WholeNumber val  <- obj.requiredMore();");
            });
        }

        [Test]
        public void MethodShouldNotAllowUnexpectedArgumentNames()
        {
            Init();
            ExecuteStatement("ClassToHave obj <- new ClassToHave();");

            LogAssert.Expect(UnityEngine.LogType.Error, new Regex("No method matching ClassToHave.requiredMore"));
            Assert.Throws<TweedleRuntimeException>(()=> {
                ExecuteStatement("WholeNumber val  <- obj.requiredMore(t: 5);");
            });
        }

        [Test]
        public void MethodWithUnnamedOptionalArgumentShouldUseDefaultValue()
        {
            Init();
            ExecuteStatement("ClassToHave obj <- new ClassToHave();");
            ExecuteStatement("WholeNumber val  <- obj.optionalOrDefaultMore();");
            TValue tested = scope.GetValue("val");

            Assert.AreEqual(8, tested.ToInt());
        }

        [Test]
        public void MethodWithNamedOptionalArgumentShouldUseNamedValue()
        {
            Init();
            ExecuteStatement("ClassToHave obj <- new ClassToHave();");
            ExecuteStatement("WholeNumber val  <- obj.optionalOrDefaultMore(n: 5);");
            TValue tested = scope.GetValue("val");

            Assert.AreEqual(10, tested.ToInt());
        }

        [Test]
        public void MethodWithUnnamedOptionalArgumentShouldUseDefaultValueFromInvocationScope()
        {
            Init();
            ExecuteStatement("ClassToHave obj <- new ClassToHave(start: 4);");
            // add local variable with same name as the default argument value member name to the calling scope
            ExecuteStatement("WholeNumber x <- 2;");
            ExecuteStatement("WholeNumber val  <- obj.optionalOrDefaultMoreVariable();");
            TValue tested = scope.GetValue("val");

            Assert.AreEqual(8, tested.ToInt());
        }

        [Test]
        public void MethodWithNamedOptionalArgumentShouldUseNamedValueFromCallingScope()
        {
            Init();
            ExecuteStatement("ClassToHave obj <- new ClassToHave(start: 4);");
            ExecuteStatement("WholeNumber x <- 2;");
            ExecuteStatement("WholeNumber val  <- obj.optionalOrDefaultMoreVariable(n: x);");
            TValue tested = scope.GetValue("val");

            Assert.AreEqual(6, tested.ToInt());
        }

        [Test]
        public void MethodWithNamedOptionalArgumentShouldUsePassedArgmuent() {
            Init();
            ExecuteStatement("ClassToHave obj <- new ClassToHave(start: 4);");
            ExecuteStatement("WholeNumber val  <- obj.optionalOrDefaultEarlierVariable(a: 9, n: 8);");
            TValue tested = scope.GetValue("val");

            Assert.AreEqual(17, tested.ToInt());
        }

        [Test]
        public void MethodWithUnnamedOptionalArgumentShouldUseDefaultValueFromPreviousArgmuent() {
            Init();
            ExecuteStatement("ClassToHave obj <- new ClassToHave(start: 4);");
            ExecuteStatement("WholeNumber val  <- obj.optionalOrDefaultEarlierVariable(a: 7);");
            TValue tested = scope.GetValue("val");

            Assert.AreEqual(14, tested.ToInt());
        }

        [Test]
        public void ChildClassShouldImplicityCastUpToParentClass()
        {
            Init();
            ExecuteStatement("Child child <- new Child();");
            ExecuteStatement("Parent childAsParent <- child;");
            var tested = scope.GetValue("childAsParent");

            Assert.IsInstanceOf<TObject>(tested.Object());
        }

        [Test]
        public void ChildVariableShouldNotAcceptParentInstance()
        {
            Init();
            LogAssert.Expect(UnityEngine.LogType.Error, new Regex("Unable to treat value Parent of type Parent as type Child"));
            Assert.Throws<TweedleRuntimeException>(() => {
                ExecuteStatement("Child childOnly <- new Parent();");
            });
        }

        [Test]
        [Ignore("Does not throw exception. Need to add check at link")]
        public void ChildClassShouldNotImplicityCastDownFromParentClass()
        {
            Init();
            ExecuteStatement("Parent childAsParent <- new Child();");

            Assert.Throws<TweedleLinkException>(() => {
                ExecuteStatement("Child childAsChild <- childAsParent;");
            });
        }

        [Test]
        public void ChildClassShouldNotImplicityCastToSiblingClass()
        {
            Init();
            ExecuteStatement("Child child <- new Child();");
            LogAssert.Expect(UnityEngine.LogType.Error, new Regex("Unable to treat value Child of type Child as type Sibling"));
            Assert.Throws<TweedleRuntimeException>(()=> {
                ExecuteStatement("Sibling childAsSibling <- child;");
            });
        }

        [Test]
        public void ChildClassShouldExplicityCastDownFromParentClassIfChildObjectIsOfChildClass()
        {
            Init();
            ExecuteStatement("Parent childAsParent <- new Child();");
            ExecuteStatement("Child childAsChild <- childAsParent as Child;");
            var tested = scope.GetValue("childAsChild");
            Assert.IsInstanceOf<TObject>(tested.Object());
        }

        [Test]
        public void ChildClassShouldNotExplicityCastToSiblingClass()
        {
            Init();
            ExecuteStatement("Child child <- new Child();");
            LogAssert.Expect(UnityEngine.LogType.Error, new Regex("Cannot cast type Child to type Sibling"));
            Assert.Throws<TweedleRuntimeException>(()=> {
                ExecuteStatement("Sibling childAsSibling <- child as Sibling;");
            });
        }
    }
}

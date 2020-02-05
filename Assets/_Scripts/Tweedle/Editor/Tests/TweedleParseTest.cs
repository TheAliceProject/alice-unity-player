using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System;

namespace Alice.Tweedle.Parse
{
    [TestFixture]
    public class TweedleParseTest
    {
        TType ParseType(string src)
        {
            TType type = new TweedleParser().ParseType(src);
            return type;
        }

        static string commonProgram =
            "class Program extends SProgram {\n"
            + "Program()\n {  super(); }\n"
            + "        Scene myScene<- new Scene();"
            + "static void main(TextString[] args) {"
            + "            Program story<-new Program();"
            + "            story.initializeInFrame(args: args);"
            + "            story.setActiveScene(scene: story.getMyScene());\n }"
            + "Scene getMyScene() {\n"
            + "  return this.myScene;\n }\n}";

        ///
        /// CLASS
        ///
        [Test]
        public void SomethingShouldBeCreatedForARootClass()
        {
            TClassType tested = (TClassType)ParseType("class SThing {}");
            Assert.NotNull(tested, "The parser should have returned something.");
        }


        [Test]
        public void ARootClassShouldBeCreated()
        {
            TClassType tested = (TClassType)ParseType("class SThing {}");

            Assert.IsInstanceOf<TClassType>(tested, "The parser should have returned a TClassType.");
        }

        [Test]
        public void ClassShouldKnowItsName()
        {
            TClassType tested = (TClassType)ParseType("class SThing {}");

            Assert.AreEqual("SThing", tested.Name, "The name should be 'SThing'");
        }

        [Test]
        public void SubclassOfBooleanPrimitiveShouldFail()
        {
            Assert.Throws<System.NullReferenceException>(delegate ()
            {
                ParseType("class SScene extends Boolean {}");
            });
        }

        [Test]
        public void SubclassOfDecimalPrimitiveShouldFail()
        {
            Assert.Throws<System.NullReferenceException>(delegate ()
            {
                ParseType("class SScene extends DecimalNumber {}");
            });
        }

        [Test]
        public void SubclassOfWholePrimitiveShouldFail()
        {
            Assert.Throws<System.NullReferenceException>(delegate ()
            {
                ParseType("class SScene extends WholeNumber {}");
            });
        }

        [Test]
        public void SubclassOfNumberPrimitiveShouldFail()
        {
            Assert.Throws<System.NullReferenceException>(delegate ()
            {
                ParseType("class SScene extends Number {}");
            });
        }

        [Test]
        public void SubclassOfTextStringPrimitiveShouldFail()
        {
            Assert.Throws<System.NullReferenceException>(delegate ()
            {
                ParseType("class SScene extends TextString {}");
            });
        }

        [Test]
        public void SubclassOfStringShouldNotFail()
        {
            TType tested = ParseType("class SScene extends String {}");

            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void ClassNamedSameAsBooleanPrimitiveShouldCreateSomething()
        {
            TType tested = ParseType("class Boolean {}");

            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void SomethingShouldBeCreatedForASubclass()
        {
            TClassType tested = (TClassType)ParseType("class SScene extends SThing {}");

            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void ASubclassShouldBeCreated()
        {
            TClassType tested = (TClassType)ParseType("class SScene extends SThing {}");

            Assert.IsInstanceOf<TClassType>(tested, "The parser should have returned a TClassType.");
        }

        [Test]
        public void ClassNameShouldBeReturnedOnSubclass()
        {
            TClassType tested = (TClassType)ParseType("class SScene extends SThing {}");

            Assert.AreEqual("SScene",
                            tested.Name,
                            "The class name should have been SScene.");
        }

        [Test]
        public void SuperclassNameShouldBeReturnedOnSubclass()
        {
            TClassType tested = (TClassType)ParseType("class SScene extends SThing {}");

            Assert.AreEqual("SThing",
                            tested.SuperType.Name,
                            "The class SScene should have a superclass SThing.");
        }

        ///
        /// CLASS PROPERTIES
        ///

        [Test]
        public void PropertyShouldHaveTypeOnClassProperty()
        {
            TClassType tested = (TClassType)ParseType("class SScene extends SThing { int test; }");
            //sScene.Fields(null).Add(new TField(new TweedleTypeReference("int"), "test"));

            Assert.AreEqual("int",
                            tested.Fields(null)[0].Type.Name,
                            "The class SScene should have a property with type int.");
        }

        [Test]
        public void PropertyShouldHaveNameOnClassProperty()
        {
            TClassType tested = (TClassType)ParseType("class SScene extends SThing { int test; }");
            //sScene.Fields(null).Add(new TField(new TweedleTypeReference("int"), "test"));

            Assert.AreEqual("test",
                            tested.Fields(null)[0].Name,
                            "The class SScene should have a property named test.");
        }

        [Test]
        public void PropertyModifierShouldBeReturnedOnClassProperty()
        {
            TClassType tested = (TClassType)ParseType("class SScene extends SThing { static int test; }");

            Assert.AreEqual(MemberFlags.Static,
                            tested.Fields(null)[0].Flags & MemberFlags.Static,
                            "The class SScene should have a property with static modifier.");
        }

        [Test]
        public void SomethingShouldBeCreatedForAProgram()
        {
            TClassType tested = (TClassType)ParseType(commonProgram);

            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void TwoMethodShouldBeCreatedForAProgram()
        {
            TClassType tested = (TClassType)ParseType(commonProgram);

            Assert.AreEqual(tested.Methods(null).Length, 2, "There should be 2 methods.");
        }

        [Test]
        public void GetMySceneMethodShouldBeCreatedForAProgram()
        {
            TClassType tested = (TClassType)ParseType(commonProgram);

            Assert.NotNull(tested.Method(null, "getMyScene", new string[0]), "There should be a method.");
        }

        [Test]
        public void AMainMethodShouldBeCreatedForAProgram()
        {
            TClassType tested = (TClassType)ParseType(commonProgram);

            Assert.NotNull(tested.Method(null, "main", new string[] { "args" }), "There should be a method.");
        }

        [Test]
        public void NoConstructorAsMethodShouldBeCreatedForAProgram()
        {
            TClassType tested = (TClassType)ParseType(commonProgram);
            Assert.Throws<NullReferenceException>(delegate ()
            {
                tested.Method(null, "constructor", new string[0]);
            });
        }

        [Test]
        public void StaticModifierShouldNotBeOnGetMySceneMethod()
        {
            TMethod tested = ((TClassType)ParseType(commonProgram)).Method(null, "getMyScene", new string[0]);

            Assert.IsFalse(tested.IsStatic(), "Method getMyScene should not be static.");
        }

        [Test]
        public void StaticModifierShouldBeOnMainMethod()
        {
            TMethod tested = ((TClassType)ParseType(commonProgram)).Method(null, "main", new string[] { "args" });

            Assert.IsTrue(tested.IsStatic(), "Method main should be static.");
        }

        ///
        /// CLASS CONSTRUCTOR
        ///

        [Test]
        public void SomethingShouldBeCreatedForClassWithConstructor()
        {
            string scene = "class Scene extends SScene models Scene {\n"
                        + "  Scene() {\n"
                        + "    super();\n"
                        + "  }\n"
                        + "}";
            TType tested = ParseType(scene);

            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void ClassWithMethodShouldHaveMethod()
        {
            string scene = "class Scene extends SScene {\n"
                            + "  WholeNumber sumThing() {\n"
                            + "    return 3 + 4;\n"
                            + "  }\n"
                            + "}";
            TClassType tested = (TClassType)ParseType(scene);

            Assert.IsNotEmpty(tested.Methods(null), "The class should have a method.");
        }

        [Test]
        public void ClassMethodShouldHaveReturnType()
        {
            string scene = "class Scene extends SScene {\n"
                            + "  WholeNumber sumThing() {\n"
                            + "    return 3 + 4;\n"
                            + "  }\n"
                            + "}";
            TClassType tested = (TClassType)ParseType(scene);
            TTMethod sumThing = (TTMethod)tested.Methods(null)[0];

            Assert.AreEqual(TBuiltInTypes.WHOLE_NUMBER, sumThing.Type, "The method should return a WholeNumber.");
        }

        [Test]
        public void ClassMethodShouldBeNamed()
        {
            string scene = "class Scene extends SScene {\n"
                            + "  WholeNumber sumThing() {\n"
                            + "    return 3 + 4;\n"
                            + "  }\n"
                            + "}";
            TClassType tested = (TClassType)ParseType(scene);
            TTMethod sumThing = (TTMethod)tested.Methods(null)[0];

            Assert.AreEqual("sumThing", sumThing.Name, "The method should be named.");
        }

        [Test]
        public void ClassMethodShouldHaveNoRequiredParams()
        {
            string scene = "class Scene extends SScene {\n"
                            + "  WholeNumber sumThing() {\n"
                            + "    return 3 + 4;\n"
                            + "  }\n"
                            + "}";
            TClassType tested = (TClassType)ParseType(scene);
            TTMethod sumThing = (TTMethod)tested.Methods(null)[0];

            Assert.IsEmpty(sumThing.RequiredParams, "The method should have no params.");
        }

        [Test]
        public void ClassMethodShouldHaveNoOptionalParams()
        {
            string scene = "class Scene extends SScene {\n"
                            + "  WholeNumber sumThing() {\n"
                            + "    return 3 + 4;\n"
                            + "  }\n"
                            + "}";
            TClassType tested = (TClassType)ParseType(scene);
            TTMethod sumThing = (TTMethod)tested.Methods(null)[0];

            Assert.IsEmpty(sumThing.OptionalParams, "The method should have no params.");
        }

        [Test]
        public void ClassMethodShouldHaveAStatement()
        {
            string scene = "class Scene extends SScene {\n"
                            + "  WholeNumber sumThing() {\n"
                            + "    return 3 + 4;\n"
                            + "  }\n"
                            + "}";
            TClassType tested = (TClassType)ParseType(scene);
            TTMethod sumThing = (TTMethod)tested.Methods(null)[0];

            Assert.AreEqual(1, sumThing.Body.Statements.Length, "The method should have one statement.");
        }

        [Test]
        public void ClassMethodWithEmptyReturnShouldHaveANonNullStatement()
        {
            string scene = "class Scene extends SScene {\n"
                            + "  WholeNumber sumThing() {\n"
                            + "    return;\n"
                            + "  }\n"
                            + "}";
            TClassType tested = (TClassType)ParseType(scene);
            TTMethod sumThing = (TTMethod)tested.Methods(null)[0];
            TweedleStatement stmt = sumThing.Body.Statements[0];

            Assert.NotNull(stmt, "The method statement should not be null.");
        }

        [Test]
        public void ClassMethodWithEmptyReturnShouldHaveReturnStatement()
        {
            string scene = "class Scene extends SScene {\n"
                            + "  WholeNumber sumThing() {\n"
                            + "    return;\n"
                            + "  }\n"
                            + "}";
            TClassType tested = (TClassType)ParseType(scene);
            TTMethod sumThing = (TTMethod)tested.Methods(null)[0];
            TweedleStatement stmt = sumThing.Body.Statements[0];

            Assert.IsInstanceOf<ReturnStatement>(stmt, "The method statement should be a return.");
        }

        [Test]
        public void ClassMethodWithEmptyReturnStatementShouldHaveAnExpression()
        {
            string scene = "class Scene extends SScene {\n"
                            + "  WholeNumber sumThing() {\n"
                            + "    return;\n"
                            + "  }\n"
                            + "}";
            TClassType tested = (TClassType)ParseType(scene);
            TTMethod sumThing = (TTMethod)tested.Methods(null)[0];
            ReturnStatement stmt = (ReturnStatement)sumThing.Body.Statements[0];

            Assert.NotNull(stmt.Expression, "The return statement should hold an expression.");
        }

        [Test]
        public void ClassMethodWithEmptyReturnReturnStatementShouldHaveTweedleNull()
        {
            string scene = "class Scene extends SScene {\n"
                            + "  WholeNumber sumThing() {\n"
                            + "    return;\n"
                            + "  }\n"
                            + "}";
            TClassType tested = (TClassType)ParseType(scene);
            TTMethod sumThing = (TTMethod)tested.Methods(null)[0];
            ReturnStatement stmt = (ReturnStatement)sumThing.Body.Statements[0];

            Assert.AreEqual(TValue.UNDEFINED, stmt.Expression, "The return statement should hold UNDEFINED.");
        }

        [Test]
        public void ClassMethodShouldHaveANonNullStatement()
        {
            string scene = "class Scene extends SScene {\n"
                            + "  WholeNumber sumThing() {\n"
                            + "    return 3 + 4;\n"
                            + "  }\n"
                            + "}";
            TClassType tested = (TClassType)ParseType(scene);
            TTMethod sumThing = (TTMethod)tested.Methods(null)[0];
            TweedleStatement stmt = sumThing.Body.Statements[0];

            Assert.NotNull(stmt, "The method statement should not be null.");
        }

        [Test]
        public void ClassMethodShouldHaveReturnStatement()
        {
            string scene = "class Scene extends SScene {\n"
                            + "  WholeNumber sumThing() {\n"
                            + "    return 3 + 4;\n"
                            + "  }\n"
                            + "}";
            TClassType tested = (TClassType)ParseType(scene);
            TTMethod sumThing = (TTMethod)tested.Methods(null)[0];
            TweedleStatement stmt = sumThing.Body.Statements[0];

            Assert.IsInstanceOf<ReturnStatement>(stmt, "The method statement should be a return.");
        }

        [Test]
        public void ClassMethodReturnStatementShouldHaveAnExpression()
        {
            string scene = "class Scene extends SScene {\n"
                            + "  WholeNumber sumThing() {\n"
                            + "    return 3 + 4;\n"
                            + "  }\n"
                            + "}";
            TClassType tested = (TClassType)ParseType(scene);
            TTMethod sumThing = (TTMethod)tested.Methods(null)[0];
            ReturnStatement stmt = (ReturnStatement)sumThing.Body.Statements[0];

            Assert.NotNull(stmt.Expression, "The method statement should hold an expression.");
        }

        [Test]
        public void ClassMethodReturnStatementShouldHaveAnAdditionExpression()
        {
            string scene = "class Scene extends SScene {\n"
                            + "  WholeNumber sumThing() {\n"
                            + "    return 3 + 4;\n"
                            + "  }\n"
                            + "}";
            TClassType tested = (TClassType)ParseType(scene);
            TTMethod sumThing = (TTMethod)tested.Methods(null)[0];
            ReturnStatement stmt = (ReturnStatement)sumThing.Body.Statements[0];

            Assert.IsInstanceOf<AdditionExpression>(stmt.Expression, "The method statement should hold an addition expression.");
        }

        [Test]
        public void SomethingShouldBeCreatedForClassWithListener()
        {
            string scene = "class Scene extends SScene {\n"
                        + "  Scene() {\n"
                        + "    super();\n"
                        + "  }\n"
                        + "  void initializeEventListeners() {\n"
                        + "    this.addSceneActivationListener(listener: (SceneActivationEvent event) -> {\n"
                        + "      this.myFirstMethod();\n"
                        + "    });\n"
                        + "  }\n"
                        + "}";
            TType tested = ParseType(scene);

            Assert.NotNull(tested, "The parser should have returned something.");
        }

        ///
        /// ENUM
        ///

        [Test]
        public void EnumNamedSameAsBooleanPrimitiveShouldCreateSomething()
        {
            TType tested = ParseType("enum Boolean {TRUE, FALSE}");

            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void SomethingShouldBeCreatedForAnEnum()
        {
            TEnumType tested = (TEnumType)ParseType("enum Direction {UP, DOWN}");

            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void AnEnumShouldBeCreated()
        {
            TEnumType tested = (TEnumType)ParseType("enum Direction {UP, DOWN}");

            Assert.IsInstanceOf<TEnumType>(tested, "The parser should have returned a TEnumType.");
        }

        [Test]
        public void NameShouldBeReturnedOnEnum()
        {
            TEnumType tested = (TEnumType)ParseType("enum Direction {UP, DOWN}");

            Assert.AreEqual("Direction",
                            tested.Name,
                            "The enum name should have been Direction.");
        }

        [Test]
        public void EnumShouldIncludeTwoValues()
        {
            TEnumType tested = (TEnumType)ParseType("enum Direction {UP, DOWN}");

            Assert.AreEqual(2,
                            tested.ValueInitializers().Length,
                            "The enum Direction should have two values.");
        }

        [Test]
        public void EnumShouldIncludeUpValue()
        {
            TEnumType tested = (TEnumType)ParseType("enum Direction {UP, DOWN}");

            Assert.NotNull(tested.GetValueInitializer("UP"),
                        "The enum Direction should include UP.");
        }

        [Test]
        public void EnumShouldIncludeDownValue()
        {
            TEnumType tested = (TEnumType)ParseType("enum Direction {UP, DOWN}");

            Assert.NotNull(tested.GetValueInitializer("DOWN"),
                        "The enum Direction should include DOWN.");
        }

        [Test]
        public void EnumShouldNotIncludeLeftValue()
        {
            TEnumType tested = (TEnumType)ParseType("enum Direction {UP, DOWN}");

            Assert.Null(tested.GetValueInitializer("LEFT"),
                         "The enum Direction should not include LEFT.");
        }

        ///
        /// Scene
        ///

        [Test]
        [Ignore("Missing features")]
        public void SomethingShouldBeCreatedForGeneratedScene()
        {
            string generatedScene = "class Scene extends SScene models Scene {\n" + "  Scene() {\n" + "    super();\n" + "  }\n"
                        + "\n" + "  void initializeEventListeners() {\n"
                        + "    this.addSceneActivationListener(listener: (SceneActivationEvent event)-> {\n"
                        + "      this.myFirstMethod();\n" + "    });\n" + "  }\n" + "\n" + "  void myFirstMethod() {\n"
                        + "    this.sphere.jump();\n" + "    this.sphere.jump();\n" + "    doTogether {\n"
                        + "      this.walrus.moveToward(target: this.sphere,amount: 2.0);\n"
                        + "      this.walrus.moveToward(target: this.cylinder,amount: 2.0);\n" + "    }\n" + "    doTogether {\n"
                        + "      this.sphere.setPaint(paint: Color.GREEN);\n" + "      this.sphere.setPaint(paint: Color.RED);\n"
                        + "    }\n" + "    this.walrus.say(text: \"hello \\\"Ralph\\\" How are you? \\\\\\\"/\\\" today?\");\n"
                        + "    doTogether {\n" + "      this.walrus.turn(direction: TurnDirection.LEFT,amount: 1.0);\n" + "    }\n"
                        + "*<  this.walrus.roll(direction: RollDirection.RIGHT,amount: 1.0); >*\n"
                        + "    this.walrus.turn(direction: TurnDirection.LEFT,amount: 1.0);\n" + "    doInOrder {\n"
                        + "      doInOrder {\n" + "      }\n" + "      // So much to say\n"
                        + "      // And I can use multiple lines\n"
                        + "      // Nicer if the other side updated as I typed, but what can you do?\n" + "*<    doInOrder {\n"
                        + "*<      this.walrus.turnToFace(target: this.cylinder,details: TurnToFace.duration(unknown: 2.0)); >*\n"
                        + "        this.walrus.turnToFace(target: this.sphere,details: TurnToFace.duration(unknown: 2.0));\n"
                        + "      } >*\n" + "    }\n" + "    doInOrder {\n" + "      doTogether {\n"
                        + "        forEach(SModel x in new SModel[]{this.sphere, this.walrus}) {\n" + "          doTogether {\n"
                        + "          }\n" + "        }\n" + "      }\n" + "      SModel[] muddles <- new SModel[]{};\n"
                        + "      doInOrder {\n" + "        doTogether {\n"
                        + "          this.walrus.turnToFace(target: this.cylinder,details: TurnToFace.duration(unknown: 2.0));\n"
                        + "          this.walrus.turnToFace(target: this.sphere,details: TurnToFace.duration(unknown: 2.0));\n"
                        + "        }\n" + "      }\n" + "    }\n" + "*<  countUpTo( indexA < 2 ) {\n" + "    } >*\n"
                        + "    countUpTo( indexB < 2 ) {\n" + "    }\n" + "*<  while (false) {\n" + "    } >*\n"
                        + "    while (false) {\n" + "    }\n"
                        + "*<  forEach(SModel x in new SModel[]{this.sphere, this.walrus}) {\n" + "      doTogether {\n"
                        + "      }\n" + "    } >*\n" + "    forEach(SModel x in new SModel[]{this.sphere, this.walrus}) {\n"
                        + "      doTogether {\n" + "      }\n" + "    }\n" + "*<  if(true) {\n" + "    } else {\n" + "    } >*\n"
                        + "    if(true) {\n" + "    } else {\n" + "    }\n" + "*<  doTogether {\n" + "    } >*\n"
                        + "    doTogether {\n" + "    }\n"
                        + "*<  eachTogether(TextString msg in new TextString[]{\"hello\", \"hello\"}) {\n"
                        + "      this.walrus.say(text: msg);\n" + "    } >*\n"
                        + "    eachTogether(TextString msg in new TextString[]{\"hello\", \"hello\"}) {\n"
                        + "      this.walrus.say(text: msg);\n" + "    }\n" + "*<  WholeNumber a <- 2; >*\n" + "    WholeNumber a <- 2;\n"
                        + "*<  a <- 2; >*\n" + "    a <- 2;\n" + "  }\n" + "\n" + "  void doInfix() {\n"
                        + "    WholeNumber v <- 1+2+(2-1)*3;\n" + "    if((true||false)&&false) {\n" + "    } else {\n" + "    }\n"
                        + "    if(false&&false||0.5<=1.0) {\n" + "    } else {\n" + "    }\n"
                        + "    if((false||false)&&(true||true)) {\n" + "    } else {\n" + "    }\n"
                        + "    if(false&&false||true&&true) {\n" + "    } else {\n" + "    }\n" + "  }\n"
                        + "  SGround ground <- new SGround();\n" + "  SCamera camera <- new SCamera();\n"
                        + "  Walrus walrus <- new Walrus();\n" + "  Sphere sphere <- new Sphere();\n"
                        + "  Cylinder cylinder <- new Cylinder();\n" + "\n" + "  void performCustomSetup() {\n"
                        + "    // Make adjustments to the starting scene, in a way not available in the Scene editor\n" + "  }\n"
                        + "\n" + "  void performGeneratedSetUp() {\n" + "    // DO NOT EDIT\n"
                        + "    // This code is automatically generated.  Any work you perform in this method will be overwritten.\n"
                        + "    // DO NOT EDIT\n"
                        + "    this.setAtmosphereColor(color: new Color(red: 0.588,green: 0.886,blue: 0.988));\n"
                        + "    this.setFromAboveLightColor(color: Color.WHITE);\n"
                        + "    this.setFromBelowLightColor(color: Color.BLACK);\n" + "    this.setFogDensity(density: 0.0);\n"
                        + "    this.setName(name: \"myScene\");\n" + "    this.ground.setPaint(paint: SurfaceAppearance.GRASS);\n"
                        + "    this.ground.setOpacity(opacity: 1.0);\n" + "    this.ground.setName(name: \"ground\");\n"
                        + "    this.ground.setVehicle(vehicle: this);\n" + "    this.camera.setName(name: \"camera\");\n"
                        + "    this.camera.setVehicle(vehicle: this);\n"
                        + "    this.camera.setOrientationRelativeToVehicle(orientation: new Orientation(x: 0.0,y: 0.995185,z: 0.0980144,w: 6.12323E-17));\n"
                        + "    this.camera.setPositionRelativeToVehicle(position: new Position(right: 9.61E-16,up: 1.56,backward: -7.85));\n"
                        + "    this.walrus.setPaint(paint: Color.WHITE);\n" + "    this.walrus.setOpacity(opacity: 1.0);\n"
                        + "    this.walrus.setName(name: \"walrus\");\n" + "    this.walrus.setVehicle(vehicle: this);\n"
                        + "    this.walrus.setOrientationRelativeToVehicle(orientation: new Orientation(x: 0.0,y: 0.0,z: 0.0,w: 1.0));\n"
                        + "    this.walrus.setPositionRelativeToVehicle(position: new Position(right: 0.618,up: 0.0111,backward: -0.877));\n"
                        + "    this.sphere.setRadius(radius: 0.5);\n" + "    this.sphere.setPaint(paint: Color.WHITE);\n"
                        + "    this.sphere.setOpacity(opacity: 1.0);\n" + "    this.sphere.setName(name: \"sphere\");\n"
                        + "    this.sphere.setVehicle(vehicle: this);\n"
                        + "    this.sphere.setOrientationRelativeToVehicle(orientation: new Orientation(x: 0.0,y: 0.0,z: 0.0,w: 1.0));\n"
                        + "    this.sphere.setPositionRelativeToVehicle(position: new Position(right: -7.34,up: 0.5,backward: 18.9));\n"
                        + "    this.cylinder.setRadius(radius: 0.5);\n" + "    this.cylinder.setLength(length: 1.0);\n"
                        + "    this.cylinder.setPaint(paint: Color.WHITE);\n" + "    this.cylinder.setOpacity(opacity: 1.0);\n"
                        + "    this.cylinder.setName(name: \"cylinder\");\n" + "    this.cylinder.setVehicle(vehicle: this);\n"
                        + "    this.cylinder.setOrientationRelativeToVehicle(orientation: new Orientation(x: 0.0,y: 0.0,z: 0.0,w: 1.0));\n"
                        + "    this.cylinder.setPositionRelativeToVehicle(position: new Position(right: 7.63,up: 0.0,backward: 19.7));\n"
                        + "  }\n" + "\n" + "  void handleActiveChanged(Boolean isActive,WholeNumber activationCount) {\n"
                        + "    if(isActive) {\n" + "      if(activationCount==1) {\n" + "        this.performGeneratedSetUp();\n"
                        + "        this.performCustomSetup();\n" + "        this.initializeEventListeners();\n" + "      } else {\n"
                        + "        this.restoreStateAndEventListeners();\n" + "      }\n" + "    } else {\n"
                        + "      this.preserveStateAndEventListeners();\n" + "    }\n" + "  }\n" + "  SGround getGround() {\n"
                        + "    return this.ground;\n" + "  }\n" + "  SCamera getCamera() {\n" + "    return this.camera;\n"
                        + "  }\n" + "  Walrus getWalrus() {\n" + "    return this.walrus;\n" + "  }\n" + "  Sphere getSphere() {\n"
                        + "    return this.sphere;\n" + "  }\n" + "  Cylinder getCylinder() {\n" + "    return this.cylinder;\n"
                        + "  }\n" + "}";
            TType tested = ParseType(generatedScene);

            Assert.NotNull(tested, "The parser should have returned something.");
        }
    }
}

using Alice.Tweedle.VM;
using NUnit.Framework;

namespace Alice.Tweedle.Parse
{
    [TestFixture]
    public class TweedleStatementRunTest
    {
        private static IStackFrame testStackFrame = new StaticStackFrame("Test");

        void ExecuteStatement(string src, ExecutionScope scope)
        {
            TweedleStatement stmt = new TweedleParser().ParseStatement(src);
            TestVirtualMachine.ExecuteToFinish(stmt, scope);
        }

        [Test]
        public void EmptyDoInOrderShouldExecute()
        {
            ExecuteStatement("doInOrder {}", GetTestScope());
        }

        private static ExecutionScope GetTestScope()
        {
            return new ExecutionScope(testStackFrame, new TestVirtualMachine(new TweedleSystem()));
        }

        private static ExecutionScope GetTestScopeWithReturnTestClass() {

            const string attrClassSrc = @"class AttrClass {
  AttrClass()
{}
  WholeNumber attr1 <- 0;
  WholeNumber attr2 <- 0;
}";

            const string returnTestSrc = @"class ReturnTest {
  ReturnTest()
{}
  void together(AttrClass instance, WholeNumber count1, WholeNumber count2) {
    doTogether {
       return;
       instance.attr1 <- count1;
       countUpTo(c < count2) { instance.attr2 <- instance.attr2 + 1; }
    }
  }
  WholeNumber sequential(WholeNumber earlyValue, WholeNumber lateValue){
    WholeNumber x <- earlyValue;
    if (true) {return x;}
    x <- lateValue;
    return x;
  }
  void voidReturn(WholeNumber earlyValue, WholeNumber lateValue){
    WholeNumber x <- earlyValue;
    if (true) {return;}
    x <- lateValue;
  }
  WholeNumber doLittle() {
    WholeNumber m <- 0;
    WholeNumber n <- 0;
    WholeNumber p <- 0;
    doTogether {
      n <- 1;
      m <- 1;
      p <- 1;
    }
    return n+m+p;
  }
  WholeNumber doLots() {
    WholeNumber m <- 0;
    WholeNumber n <- 0;
    WholeNumber p <- 0;
    doTogether {
      n <- 4;
      doInOrder {
        countUpTo( indexA < 60 ) {  // could delay things a little
          m <- m + 1;
        }
        p <- 16;
      }
    }
    return n+m+p;
  }
  WholeNumber doThingsRepeatedly() {
    WholeNumber m <- 0;
    WholeNumber n <- 0;
    WholeNumber p <- 0;
    doTogether {
      n <- 4;
      m <- m + 1;
      p <- 16;
    }
    doTogether {
      n <- n + 2;
      m <- m + 3;
      p <- p + 1;
    }
    doTogether {
      n <- n + 4;
      m <- m + 6;
      p <- p + 3;
    }
    return n+m+p;
  }
  WholeNumber doMostNestedThings() {
    WholeNumber m <- 0;
    WholeNumber n <- 0;
    WholeNumber p <- 0;
    doTogether {
*<      this.say(text: ""Nothing done here""); >*
        n <- 4;
        m <- m + 1;
        p <- 16;
    }
    doTogether {
*<      this.say(text: ""Nothing done here""); >*
        n <- n + 2;
        m <- m + 3;
        p <- p + 1;
    }
    doTogether {
*<      this.say(text: ""Nothing done here""); >*
        n <- n + 4;
        m <- m + 6;
        p <- p + 3;
    }
    return n+m+p;
  }
  WholeNumber doDisabledThingsTogether() {
    WholeNumber n <- 0;
    eachTogether(WholeNumber c in new WholeNumber[] {5,2,3} ) {
*<      n <- n + 100; >*
    }
    n <- n + 1;
    return n;
  }
}";

            TweedleSystem system = new TweedleSystem();
            TClassType attrClass = (TClassType)new TweedleParser().ParseType(attrClassSrc);
            system.GetRuntimeAssembly().Add(attrClass);
            TClassType returnTestClass = (TClassType)new TweedleParser().ParseType(returnTestSrc);
            system.GetRuntimeAssembly().Add(returnTestClass);
            system.Link();

            return new ExecutionScope(testStackFrame, new TestVirtualMachine(system));
        }

        private static ExecutionScope GetTestScopeWithStaticsOnClass()
        {
            const string childClassSrc = @"class ChildClass {
  ChildClass(TextString inside){ 
    this.insider <- inside;
  }
  TextString insider;
  }";

            const string valueClassSrc = @"class ValueClass {
  ValueClass(TextString nom, WholeNumber num){ 
    name <- nom;
    number <- num;
  }
  TextString name;
  WholeNumber number;
  static TextString sharedName <- ""valuable"";
  static WholeNumber sharedNumber <- 22;
  static ChildClass sharedChild <- new ChildClass(inside: ""secret"");
  TextString getChildValue() {
    return ValueClass.sharedChild.insider;
  }
}";

            TweedleSystem system = new TweedleSystem();
            TClassType childClass = (TClassType)new TweedleParser().ParseType(childClassSrc);
            system.GetRuntimeAssembly().Add(childClass);
            TClassType valueClass = (TClassType)new TweedleParser().ParseType(valueClassSrc);
            system.GetRuntimeAssembly().Add(valueClass);
            system.Link();
            return new ExecutionScope(testStackFrame, new TestVirtualMachine(system));
        }

        [Test]
        public void LocalDeclarationShouldUpdateTheScope()
        {
            ExecutionScope scope = GetTestScope();
            ExecuteStatement("WholeNumber x <- 3;", scope);

            Assert.NotNull(scope.GetValue("x").Type);
        }

        [Test]
        public void LocalDeclarationShouldSetTheValueOnTheScope()
        {
            ExecutionScope scope = GetTestScope();
            ExecuteStatement("WholeNumber x <- 3;", scope);

            Assert.AreEqual(3, scope.GetValue("x").ToInt(), "Should be 3");
        }

        [Test]
        public void StatementsAfterAnEarlyReturnShouldNotExecute()
        {
            ExecutionScope scope = GetTestScopeWithReturnTestClass();
            ExecuteStatement("ReturnTest early <- new ReturnTest();", scope);
            ExecuteStatement("WholeNumber x <- early.sequential(earlyValue: 3, lateValue: 7);", scope);

            Assert.AreEqual(3, scope.GetValue("x").ToInt(), "Should be 3");
        }

        [Test]
        public void VoidReturnStatementsCanExecute()
        {
            ExecutionScope scope = GetTestScopeWithReturnTestClass();
            ExecuteStatement("ReturnTest test <- new ReturnTest();", scope);
            Assert.DoesNotThrow(()=>{
                ExecuteStatement("test.voidReturn(earlyValue: 3, lateValue: 7);", scope);
            });
        }

        [Test]
        public void DoInOrderSingletonShouldUpdateParentValues()
        {
            ExecutionScope scope = GetTestScope();
            ExecuteStatement("WholeNumber x <- 3;", scope);

            ExecuteStatement("doInOrder { x <- 4; }", scope);

            Assert.AreEqual(4, scope.GetValue("x").ToInt(), "Should be 4");
        }

        [Test]
        public void DoInOrderSequenceShouldUpdateParentValues()
        {
            ExecutionScope scope = GetTestScope();
            ExecuteStatement("WholeNumber x <- 3;", scope);

            ExecuteStatement("doInOrder { x <- 4; x <- 34; x <- 12; }", scope);

            Assert.AreEqual(12, scope.GetValue("x").ToInt(), "Should be 12");
        }

        [Test]
        public void DoInOrderSequenceShouldReadPreviousValues()
        {
            ExecutionScope scope = GetTestScope();
            ExecuteStatement("WholeNumber x <- 3;", scope);

            ExecuteStatement("doInOrder { x <- x * 4; WholeNumber y <- 2; x <- x + y; }", scope);

            Assert.AreEqual(14, scope.GetValue("x").ToInt(), "Should be 14");
        }

        [Test]
        public void DoTogetherSingletonShouldUpdateParentValues()
        {
            ExecutionScope scope = GetTestScope();
            ExecuteStatement("WholeNumber x <- 3;", scope);

            ExecuteStatement("doTogether { x <- 4; }", scope);

            Assert.AreEqual(4, scope.GetValue("x").ToInt(), "Should be 4");
        }

        [Test]
        public void DoTogetherSequenceShouldUpdateParentValuesInSomeOrder()
        {
            ExecutionScope scope = GetTestScope();
            ExecuteStatement("WholeNumber x <- 3;", scope);

            ExecuteStatement("doTogether { x <- 4; x <- 34; x <- 12; }", scope);

            Assert.AreNotEqual(3, scope.GetValue("x").ToInt(), "Should no longer be 3");
        }

        [Test]
        public void SimpleDoTogetherStatementsShouldAllExecute() {
            var scope = GetTestScopeWithReturnTestClass();
            ExecuteStatement("ReturnTest test <- new ReturnTest();", scope);
            ExecuteStatement("WholeNumber x <- test.doLittle();", scope);

            Assert.AreEqual(3, scope.GetValue("x").ToInt());
        }

        [Test]
        public void NestedDoTogetherStatementsShouldAllExecute() {
            var scope = GetTestScopeWithReturnTestClass();
            ExecuteStatement("ReturnTest test <- new ReturnTest();", scope);
            ExecuteStatement("WholeNumber x <- test.doLots();", scope);

            Assert.AreEqual(80, scope.GetValue("x").ToInt());
        }

        [Test]
        public void SequenceOfDoTogetherStatementsShouldAllExecute() {
            var scope = GetTestScopeWithReturnTestClass();
            ExecuteStatement("ReturnTest test <- new ReturnTest();", scope);
            ExecuteStatement("WholeNumber x <- test.doThingsRepeatedly();", scope);

            Assert.AreEqual(40, scope.GetValue("x").ToInt());
        }

        [Test]
        public void SequentialDoTogetherStatementsWithDisabledStatementsShouldAllExecute() {
            var scope = GetTestScopeWithReturnTestClass();
            ExecuteStatement("ReturnTest test <- new ReturnTest();", scope);
            ExecuteStatement("WholeNumber x <- test.doMostNestedThings();", scope);

            Assert.AreEqual(40, scope.GetValue("x").ToInt());
        }

        [Test]
        public void EachTogetherWithDisabledStatementShouldOnlyContinueOnce() {
            var scope = GetTestScopeWithReturnTestClass();
            ExecuteStatement("ReturnTest test <- new ReturnTest();", scope);
            ExecuteStatement("WholeNumber x <- test.doDisabledThingsTogether();", scope);

            Assert.AreEqual(1, scope.GetValue("x").ToInt());
        }

        [Test]
        [Ignore("APUVR-108 Fix early return in doTogether blocks")]
        public void DoTogetherSequenceShouldReturnAfterAllStatementsHaveExecuted()
        {
            ExecutionScope scope = GetTestScopeWithReturnTestClass();
            ExecuteStatement("ReturnTest early <- new ReturnTest();", scope);
            ExecuteStatement("AttrClass instance <- new AttrClass();", scope);
            ExecuteStatement("early.together(instance: instance, count1: 3, count2: 7);", scope);
            ExecuteStatement("WholeNumber attr1 <- instance.attr1;", scope);
            ExecuteStatement("WholeNumber attr2 <- instance.attr2;", scope);
            
            Assert.AreEqual(3, scope.GetValue("attr1").ToInt(), "Should be 3");
            Assert.AreEqual(7, scope.GetValue("attr2").ToInt(), "Should be 7");
        }

        [Test]
        public void ConditionalTrueShouldEvaluateThen()
        {
            ExecutionScope scope = GetTestScope();
            ExecuteStatement("WholeNumber x <- 2;", scope);
            ExecuteStatement("if( true ) { x <- 5; } else { x <- 17; }", scope);

            Assert.AreEqual(5, scope.GetValue("x").ToInt(), "Should be 5");
        }

        [Test]
        public void ConditionalFalseShouldEvaluateElse()
        {
            ExecutionScope scope = GetTestScope();
            ExecuteStatement("WholeNumber x <- 2;", scope);
            ExecuteStatement("if( false ) { x <- 5; } else { x <- 17; }", scope);

            Assert.AreEqual(17, scope.GetValue("x").ToInt(), "Should be 17");
        }

        [Test]
        public void ConditionalIfShouldShouldNotLeak()
        {
            ExecutionScope scope = GetTestScope();
            ExecuteStatement("WholeNumber z <- 0;", scope);
            ExecuteStatement("if(true) { z <- z+1; WholeNumber x <- z; }", scope);
            Assert.Throws<TweedleRuntimeException>(() => scope.GetValue("x"));
        }

        [Test]
        public void ConditionalElseShouldShouldNotLeak()
        {
            ExecutionScope scope = GetTestScope();
            ExecuteStatement("WholeNumber z <- 2;", scope);
            ExecuteStatement("if( false ) { z <- 5; } else { z <- 17; WholeNumber x <- z;}", scope);
            Assert.Throws<TweedleRuntimeException>(() => scope.GetValue("x"));
        }

        [Test]
        public void CountLoopShouldEvaluateNTimes()
        {
            ExecutionScope scope = GetTestScope();
            ExecuteStatement("WholeNumber x <- 0;", scope);
            ExecuteStatement("countUpTo( c < 3 ) { x <- x + c; }", scope);

            Assert.AreEqual(3, scope.GetValue("x").ToInt(), "Should be 6");
        }

        [Test]
        public void CountLoopInnerDeclarationsShouldNotLeak()
        {
            ExecutionScope scope = GetTestScope();
            ExecuteStatement("countUpTo( c < 3 ) { WholeNumber x <- c; }", scope);
            Assert.Throws<TweedleRuntimeException>(() => scope.GetValue("x"));
        }

        [Test]
        public void NestedCountLoopsShouldEvaluateNxNTimes()
        {
            ExecutionScope scope = GetTestScope();
            ExecuteStatement("WholeNumber x <- 0;", scope);
            ExecuteStatement("countUpTo( a < 3 ) { countUpTo( b < 3 ) { x <- x + 1; } }", scope);

            Assert.AreEqual(9, scope.GetValue("x").ToInt());
        }

        [Test]
        public void NestedCountLoopsShouldEvaluate64Times()
        {
            ExecutionScope scope = GetTestScope();
            ExecuteStatement("WholeNumber x <- 0;", scope);
            ExecuteStatement("countUpTo( a < 4 ) { countUpTo( b < 4 ) { countUpTo( c < 4 ) { x <- x + 1; } } }", scope);

            Assert.AreEqual(64, scope.GetValue("x").ToInt());
        }

        [Test]
        public void NestedCountLoopsShouldEvaluateNxNxNTimes()
        {
            ExecutionScope scope = GetTestScope();
            ExecuteStatement("WholeNumber x <- 0;", scope);
            ExecuteStatement("countUpTo( a < 10 ) { countUpTo( b < 10 ) { countUpTo( c < 10 ) { x <- x + 1; } } }", scope);

            Assert.AreEqual(1000, scope.GetValue("x").ToInt());
        }

        [Test]
        public void ForEachLoopShouldEvaluateNTimes()
        {
            ExecutionScope scope = GetTestScope();
            ExecuteStatement("WholeNumber x <- 0;", scope);
            ExecuteStatement("forEach( WholeNumber c in new WholeNumber[] {5,2,3} ) { x <- x + c; }", scope);

            Assert.AreEqual(10, scope.GetValue("x").ToInt(), "Should be 10");
        }

        [Test]
        public void ForEachLoopInnerDeclarationsShouldNotLeak()
        {
            ExecutionScope scope = GetTestScope();
            ExecuteStatement("forEach( WholeNumber c in new WholeNumber[] {5,2,3} ) { WholeNumber x <- c; }", scope);
            Assert.Throws<TweedleRuntimeException>(() => scope.GetValue("x"));
        }

        [Test]
        public void AWhileLoopShouldReevaluateEachLoop()
        {
            ExecutionScope scope = GetTestScope();
            ExecuteStatement("WholeNumber x <- 0;", scope);

            ExecuteStatement("while(x < 4) { x <- x+1; }", scope);

            Assert.AreEqual(4, scope.GetValue("x").ToInt());
        }

        [Test]
        public void WhileLoopInnerDeclarationsShouldNotLeak()
        {
            ExecutionScope scope = GetTestScope();
            ExecuteStatement("WholeNumber z <- 0;", scope);
            ExecuteStatement("while(z < 2) { z <- z+1; WholeNumber x <- z; }", scope);
            Assert.Throws<TweedleRuntimeException>(() => scope.GetValue("x"));
        }

        [Test]
        public void EachTogetherShouldChangeParentValues()
        {
            ExecutionScope scope = GetTestScope();
            ExecuteStatement("WholeNumber x <- 0;", scope);
            ExecuteStatement("eachTogether(WholeNumber c in new WholeNumber[] {5,2,3} ) { x <- x + c; }", scope);
            Assert.AreNotEqual(0, scope.GetValue("x").ToInt());
        }

        [Test]
        public void EachTogetherInnerDeclarationsShouldNotLeak()
        {
            ExecutionScope scope = GetTestScope();
            ExecuteStatement("eachTogether(WholeNumber c in new WholeNumber[] {5,2,3} ) { WholeNumber x <- c; }", scope);
            Assert.Throws<TweedleRuntimeException>(() => scope.GetValue("x"));
        }

        [Test]
        public void ShouldReadInstanceString()
        {
            ExecutionScope scope = GetTestScopeWithStaticsOnClass();
            ExecuteStatement("ValueClass val <- new ValueClass(nom: \"Fred\", num: 44);", scope);
            ExecuteStatement("TextString x <- val.name;", scope);

            Assert.AreEqual("\"Fred\"", scope.GetValue("x").ToString(), "Should be Fred");
        }

        [Test]
        public void ShouldReadInstanceNumber()
        {
            ExecutionScope scope = GetTestScopeWithStaticsOnClass();
            ExecuteStatement("ValueClass val <- new ValueClass(nom: \"Fred\", num: 44);", scope);
            ExecuteStatement("WholeNumber x <- val.number;", scope);

            Assert.AreEqual(44, scope.GetValue("x").ToInt(), "Should be 44");
        }

        [Test]
        public void ShouldReadStaticString()
        {
            ExecutionScope scope = GetTestScopeWithStaticsOnClass();
            ExecuteStatement("ValueClass val <- new ValueClass(nom: \"Fred\", num: 44);", scope);
            ExecuteStatement("TextString x <- ValueClass.sharedName;", scope);

            Assert.AreEqual("\"valuable\"", scope.GetValue("x").ToString(), "Should be valuable");
        }

        [Test]
        public void ShouldReadStaticNumber()
        {
            ExecutionScope scope = GetTestScopeWithStaticsOnClass();
            ExecuteStatement("ValueClass val <- new ValueClass(nom: \"Fred\", num: 44);", scope);
            ExecuteStatement("WholeNumber x <- ValueClass.sharedNumber;", scope);

            Assert.AreEqual(22, scope.GetValue("x").ToInt(), "Should be 22");
        }

        [Test]
        public void ShouldReadStaticChildFromExpression()
        {
            ExecutionScope scope = GetTestScopeWithStaticsOnClass();
            ExecuteStatement("TextString x <- ValueClass.sharedChild.insider;", scope);

            Assert.AreEqual("\"secret\"", scope.GetValue("x").ToString(), "Should be secret");
        }

        [Test]
        public void ShouldReadStaticChildInMethod()
        {
            ExecutionScope scope = GetTestScopeWithStaticsOnClass();
            ExecuteStatement("ValueClass val <- new ValueClass(nom: \"Fred\", num: 44);", scope);
            ExecuteStatement("TextString x <- val.getChildValue();", scope);

            Assert.AreEqual("\"secret\"", scope.GetValue("x").ToString(), "Should be secret");
        }

        private static ExecutionScope GetTestScopeWithDisabledStatements()
        {
            const string classSrc = @"class SomeClass {
  SomeClass(){}
  TextString getValueAfterComment() {
    // return ""why"";
    return ""why not"";
  }
  TextString getValueAfterDisabledStatement() {
    *< return ""why""; >*
    return ""why not"";
  }
}";

            TweedleSystem system = new TweedleSystem();
            TClassType someClass = (TClassType)new TweedleParser().ParseType(classSrc);
            system.GetRuntimeAssembly().Add(someClass);
            system.Link();
            return new ExecutionScope(testStackFrame, new TestVirtualMachine(system));
        }

        [Test]
        public void ShouldContinueAfterComment()
        {
            ExecutionScope scope = GetTestScopeWithDisabledStatements();
            ExecuteStatement("SomeClass val <- new SomeClass();", scope);
            ExecuteStatement("TextString x <- val.getValueAfterComment();", scope);

            Assert.AreEqual("\"why not\"", scope.GetValue("x").ToString(), "Should be \"why not\"");
        }

        [Test]
        public void ShouldContinueAfterDisabledStatement()
        {
            ExecutionScope scope = GetTestScopeWithDisabledStatements();
            ExecuteStatement("SomeClass val <- new SomeClass();", scope);
            ExecuteStatement("TextString x <- val.getValueAfterDisabledStatement();", scope);

            Assert.AreEqual("\"why not\"", scope.GetValue("x").ToString(), "Should be \"why not\"");
        }
    }
}

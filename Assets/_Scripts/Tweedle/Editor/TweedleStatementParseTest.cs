using NUnit.Framework;

namespace Alice.Tweedle.Parse
{
    [TestFixture]
    public class TweedleStatementParseTest
	{
        private TweedleStatement ParseStatement(string src)
        {
            return new TweedleParser().ParseStatement(src);
        }

        [Test]
        public void SomethingShouldBeCreatedForCountUpTo()
        {
            TweedleStatement tested = ParseStatement("countUpTo( indexB < 2 ) {}");
            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void ATweedleCountLoopShouldBeCreatedForCountUpTo()
        {
            TweedleStatement tested = ParseStatement("countUpTo( indexB < 2 ) {}");
            Assert.IsInstanceOf<CountLoop>(tested, "The parser should have returned a TweedleCountLoop.");
        }

        [Test]
        public void ATweedleCountLoopShouldHaveABody()
        {
            CountLoop tested = (CountLoop)ParseStatement("countUpTo( indexB < 2 ) {}");
			Assert.NotNull(tested.Body, "The TweedleCountLoop should have a list of statements.");
		}

        [Test]
        public void ATweedleCountLoopShouldHaveBlockOfStatements()
        {
            CountLoop tested = (CountLoop)ParseStatement("countUpTo( indexB < 2 ) {}");
			Assert.NotNull(tested.Body.Statements, "The TweedleCountLoop should have a list of statements.");
        }

        [Test]
        public void ATweedleCountLoopShouldHaveEmptyBlockOfStatements()
        {
            CountLoop tested = (CountLoop)ParseStatement("countUpTo( indexB < 2 ) {}");
            Assert.IsEmpty(tested.Body.Statements, "The TweedleCountLoop should have an empty list of statements.");
        }

        [Test]
        public void ATweedleCountLoopShouldHaveVariableDeclaration()
        {
            CountLoop tested = (CountLoop)ParseStatement("countUpTo( indexB < 2 ) {}");
            Assert.NotNull(tested.Variable, "The TweedleCountLoop should have a Variable declaration.");
        }

        [Test]
        public void ATweedleCountLoopShouldHaveWholeNumberVariable()
        {
            CountLoop tested = (CountLoop)ParseStatement("countUpTo( indexB < 2 ) {}");
            Assert.AreEqual(TweedleTypes.WHOLE_NUMBER, tested.Variable.Type, "The TweedleCountLoop should have a Variable of type WholeNumber.");
        }

        [Test]
        public void ATweedleCountLoopShouldHaveCorrectlyNamedVariable()
        {
            CountLoop tested = (CountLoop)ParseStatement("countUpTo( indexB < 2 ) {}");
            Assert.AreEqual("indexB", tested.Variable.Name, "The TweedleCountLoop should have a Variable named indexB.");
        }

        [Test]
        public void SomethingShouldBeCreatedForIfThen()
        {
            TweedleStatement tested = ParseStatement("if(true) { }");
            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void AConditionalShouldBeCreatedForIfThen()
        {
            TweedleStatement tested = ParseStatement("if(true) { }");
            Assert.IsInstanceOf<ConditionalStatement>(tested, "The parser should have returned a TweedleConditionalStatement.");
        }

        [Test]
        public void AConditionalCreatedForIfThenShouldHaveCondition()
        {
            ConditionalStatement tested = (ConditionalStatement)ParseStatement("if(true) { }");
            Assert.AreEqual(TweedleTypes.TRUE, tested.Condition, "The parser should have returned a TweedleConditionalStatement with a true condition.");
        }

        [Test]
        public void AConditionalCreatedForIfThenShouldHaveThenStatementList()
        {
            ConditionalStatement tested = (ConditionalStatement)ParseStatement("if(true) { }");
			Assert.IsEmpty(tested.ThenBody.Statements, "The parser should have returned a TweedleConditionalStatement with an empty then block.");
        }

        [Test]
        public void AConditionalCreatedForIfThenShouldHaveElseStatementList()
        {
            ConditionalStatement tested = (ConditionalStatement)ParseStatement("if(true) { }");
			Assert.IsEmpty(tested.ElseBody.Statements, "The parser should have returned a TweedleConditionalStatement with an empty else block.");
        }

        [Test]
        public void SomethingShouldBeCreatedForIfThenElse()
        {
            TweedleStatement tested = ParseStatement("if(true) { } else { }");
            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void AConditionalShouldBeCreatedForIfThenElse()
        {
            TweedleStatement tested = ParseStatement("if(true) { } else { }");
            Assert.IsInstanceOf<ConditionalStatement>(tested, "The parser should have returned a TweedleConditionalStatement.");
        }

        [Test]
        public void AConditionalCreatedForIfThenElseShouldHaveThenStatementList()
        {
            ConditionalStatement tested = (ConditionalStatement)ParseStatement("if(true) { } else { }");
			Assert.IsEmpty(tested.ThenBody.Statements, "The parser should have returned a TweedleConditionalStatement with an empty then block.");
        }

        [Test]
        public void AConditionalCreatedForIfThenElseShouldHaveElseStatementList()
        {
            ConditionalStatement tested = (ConditionalStatement)ParseStatement("if(true) { } else { }");
			Assert.IsEmpty(tested.ElseBody.Statements, "The parser should have returned a TweedleConditionalStatement with an empty else block.");
        }

        [Test]
        public void SomethingShouldBeCreatedForForEach()
        {
            TweedleStatement tested = ParseStatement("forEach(SModel x in new SModel[] {this.sphere, this.walrus} ) {}");
            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void AForEachLoopShouldBeCreatedForForEach()
        {
            TweedleStatement tested = ParseStatement("forEach(SModel x in new SModel[] {this.sphere, this.walrus} ) {}");
            Assert.IsInstanceOf<ForEachInArrayLoop>(tested, "The parser should have returned a ForEachLoop.");
        }

        [Test]
        public void SomethingShouldBeCreatedTweedleEachInArrayTogether()
        {
            TweedleStatement tested = ParseStatement("eachTogether(SModel x in new SModel[] {this.sphere, this.walrus} ) {}");
            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void ATweedleEachInArrayTogetherShouldBeCreatedTweedleEachInArrayTogether()
        {
            TweedleStatement tested = ParseStatement("eachTogether(SModel x in new SModel[] {this.sphere, this.walrus} ) {}");
            Assert.IsInstanceOf<EachInArrayTogether>(tested, "The parser should have returned a TweedleEachInArrayTogether.");
        }

        [Test]
        public void AnEachTogetherShouldHaveTypedVariable()
        {
            EachInArrayTogether tested = (EachInArrayTogether)ParseStatement("eachTogether(SModel x in new SModel[] {this.sphere, this.walrus} ) {}");
            Assert.AreEqual("SModel", tested.ItemVariable.Type.Name, "The EachTogether should have an SModel variable.");
        }

        [Test]
        public void AnEachTogetherValuesShouldHaveArrayType()
        {
            EachInArrayTogether tested = (EachInArrayTogether)ParseStatement("eachTogether(SModel x in new SModel[] {this.sphere, this.walrus} ) {}");
            Assert.IsInstanceOf<TweedleArrayType>(tested.Array.Type, "The EachTogether values should be typed as an array.");
        }

        [Test]
        public void AnEachTogetherValuesShouldHaveElementTypeSModel()
        {
            EachInArrayTogether tested = (EachInArrayTogether)ParseStatement("eachTogether(SModel x in new SModel[] {this.sphere, this.walrus} ) {}");
            TweedleArrayType valArrayType = (TweedleArrayType)tested.Array.Type;
            Assert.AreEqual(valArrayType.ValueType,
                            tested.ItemVariable.Type,
                            "The EachTogether individual values should be typed as SModel.");
        }

        [Test]
        public void AnEachTogetherShouldHaveBlockOfStatements()
        {
            EachInArrayTogether tested = (EachInArrayTogether)ParseStatement("eachTogether(SModel x in new SModel[] {this.sphere, this.walrus} ) {}");
            Assert.NotNull(tested.Body.Statements, "The EachTogether should have a list of statements.");
        }

        [Test]
        public void AnEachTogetherShouldHaveEmptyBlockOfStatements()
        {
            EachInArrayTogether tested = (EachInArrayTogether)ParseStatement("eachTogether(SModel x in new SModel[] {this.sphere, this.walrus} ) {}");
            Assert.IsEmpty(tested.Body.Statements, "The EachTogether should have an empty list of statements.");
        }

        [Test]
        public void SomethingShouldBeCreatedForWhile()
        {
            TweedleStatement tested = ParseStatement("while(true) { }");
            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void AWhileLoopShouldBeCreatedForWhile()
        {
            TweedleStatement tested = ParseStatement("while(true) { }");
            Assert.IsInstanceOf<WhileLoop>(tested, "The parser should have returned a TweedleWhileLoop.");
        }

        [Test]
        public void AWhileLoopShouldHaveRunCondition()
        {
            WhileLoop tested = (WhileLoop)ParseStatement("while(true) { }");
            Assert.NotNull(tested.RunCondition, "The TweedleWhileLoop should have a run condition.");
        }

        [Test]
        public void ThisWhileLoopRunConditionShouldBeABooleanTrue()
        {
            WhileLoop tested = (WhileLoop)ParseStatement("while(true) { }");
            Assert.AreEqual(TweedleTypes.TRUE, tested.RunCondition, "The TweedleWhileLoop run condition should be the value True.");
        }

        [Test]
        public void AWhileLoopShouldHaveBlockOfStatements()
        {
            WhileLoop tested = (WhileLoop)ParseStatement("while(true) { }");
            Assert.NotNull(tested.Body.Statements, "The TweedleWhileLoop should have a list of statements.");
        }

        [Test]
        public void AWhileLoopShouldHaveEmptyBlockOfStatements()
        {
            WhileLoop tested = (WhileLoop)ParseStatement("while(true) { }");
            Assert.IsEmpty(tested.Body.Statements, "The TweedleWhileLoop should have an empty list of statements.");
        }

        [Test]
        public void SomethingShouldBeCreatedForDoInOrder()
        {
            TweedleStatement tested = ParseStatement("doInOrder { }");
            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void ADoInOrderShouldBeCreated()
        {
            TweedleStatement tested = ParseStatement("doInOrder { }");
            Assert.IsInstanceOf<DoInOrder>(tested, "The parser should have returned a TweedleDoInOrder.");
        }

        [Test]
        public void ADoInOrderShouldHaveBlockOfStatements()
        {
            DoInOrder tested = (DoInOrder)ParseStatement("doInOrder { }");
            Assert.NotNull(tested.Body.Statements, "The TweedleDoInOrder should have a list of statements.");
        }

        [Test]
        public void ADoInOrderShouldHaveEmptyBlockOfStatements()
        {
            DoInOrder tested = (DoInOrder)ParseStatement("doInOrder { }");
			Assert.IsEmpty(tested.Body.Statements, "The TweedleDoInOrder should have an empty list of statements.");
        }

        [Test]
        public void ADoInOrderShouldHaveBlockOfTwoStatements()
        {
            DoInOrder tested = (DoInOrder)ParseStatement("doInOrder { doInOrder {} return; }");
			Assert.AreEqual(2, tested.Body.Statements.Count, "The TweedleDoInOrder should have a list of 2 statements.");
        }

        [Test]
        public void ATweedleDoInOrdersFirstStatementShouldBeDoInOrder()
        {
            DoInOrder tested = (DoInOrder)ParseStatement("doInOrder { doInOrder {} return; }");
			Assert.IsInstanceOf<DoInOrder>(tested.Body.Statements[0], "The block's first statement should be a TweedleDoInOrder.");
        }

        [Test]
        public void ADoInOrdersSecondStatementShouldBeReturn()
        {
            DoInOrder tested = (DoInOrder)ParseStatement("doInOrder { doInOrder {} return; }");
			Assert.IsInstanceOf<ReturnStatement>(tested.Body.Statements[1], "The block's first statement should be a Return.");
        }

        [Test]
        public void SomethingShouldBeCreatedForDoTogether()
        {
            TweedleStatement tested = ParseStatement("doTogether { }");
            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void ADoTogetherShouldBeCreated()
        {
            TweedleStatement tested = ParseStatement("doTogether { }");
            Assert.IsInstanceOf<DoTogether>(tested, "The parser should have returned a TweedleDoTogether.");
        }

        [Test]
        public void ADoTogetherShouldHaveBlockOfStatements()
        {
            DoTogether tested = (DoTogether)ParseStatement("doTogether { }");
			Assert.NotNull(tested.Body.Statements, "The TweedleDoTogether should have a list of statements.");
        }

        [Test]
        public void ADoTogetherShouldHaveEmptyBlockOfStatements()
        {
            DoTogether tested = (DoTogether)ParseStatement("doTogether { }");
			Assert.IsEmpty(tested.Body.Statements, "The TweedleDoTogether should have an empty list of statements.");
        }

        [Test]
        public void ADoTogetherShouldHaveBlockOfTwoStatements()
        {
            DoTogether tested = (DoTogether)ParseStatement("doTogether { doInOrder {} return; }");
			Assert.AreEqual(2, tested.Body.Statements.Count, "The TweedleDoTogether should have a list of 2 statements.");
        }

        [Test]
        public void ADoTogethersFirstStatementShouldBeDoInOrder()
        {
            DoTogether tested = (DoTogether)ParseStatement("doTogether { doInOrder {} return; }");
			Assert.IsInstanceOf<DoInOrder>(tested.Body.Statements[0], "The block's first statement should be a DoInorder.");
        }

        [Test]
        public void ADoTogethersSecondStatementShouldBeReturn()
        {
            DoTogether tested = (DoTogether)ParseStatement("doTogether { doInOrder {} return; }");
			Assert.IsInstanceOf<ReturnStatement>(tested.Body.Statements[1], "The block's first statement should be a Return.");
        }

        [Test]
        public void SomethingShouldBeCreatedForEmptyReturn()
        {
            TweedleStatement tested = ParseStatement("return;");
            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void AReturnShouldBeCreatedForEmptyReturn()
        {
            TweedleStatement tested = ParseStatement("return;");
            Assert.IsInstanceOf<ReturnStatement>(tested, "The parser should have returned a TweedleReturnStatement.");
        }

        [Test]
        public void AnEmptyReturnShouldBeCreatedForEmptyReturn()
        {
            ReturnStatement tested = (ReturnStatement)ParseStatement("return;");
            Assert.AreEqual(TweedleNull.NULL, tested.Expression, "The TweedleReturnStatement should hold TweedleNull.");
        }

        [Test]
        public void AnVoidTypeReturnShouldBeCreatedForEmptyReturn()
        {
            ReturnStatement tested = (ReturnStatement)ParseStatement("return;");
            Assert.AreEqual(TweedleVoidType.VOID, tested.Type, "The TweedleReturnStatement should be type void.");
        }

        [Test]
        public void SomethingShouldBeCreatedForReturnWithValue()
        {
            TweedleStatement tested = ParseStatement("return 4;");
            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void AReturnShouldBeCreatedForReturnWithValue()
        {
            TweedleStatement tested = ParseStatement("return 4;");
            Assert.IsInstanceOf<ReturnStatement>(tested, "The parser should have returned a TweedleReturnStatement.");
        }

        [Test]
        public void AReturnExpressionShouldBeCreatedForReturnWithValue()
        {
            ReturnStatement tested = (ReturnStatement)ParseStatement("return 4;");
            Assert.NotNull(tested.Expression, "The TweedleReturnStatement should hold no value.");
        }

        [Test]
        public void AWholeNumberTypeReturnShouldBeCreatedForReturnWithValue()
        {
            ReturnStatement tested = (ReturnStatement)ParseStatement("return 4;");
            Assert.AreEqual(TweedleTypes.WHOLE_NUMBER, tested.Type, "The TweedleReturnStatement should be type WholeNumber.");
        }

        [Test]
        public void SomethingShouldBeCreatedForAssignment()
        {
            TweedleStatement tested = ParseStatement("x <- 3;");
            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void AnExpressionStatementShouldBeCreated()
        {
            TweedleStatement tested = ParseStatement("x <- 3;");
            Assert.IsInstanceOf<ExpressionStatement>(tested, "The parser should have returned an ExpressionStatement.");
        }

        [Test]
        public void AnAssignmentExpressionShouldBeCreated()
        {
            ExpressionStatement tested = (ExpressionStatement)ParseStatement("x <- 3;");
            Assert.IsInstanceOf<AssignmentExpression>(tested.Expression, "The Statement should have contained an AssignmentExpression.");
        }

        [Test]
        public void SomethingShouldBeCreatedForVariableDeclaration()
        {
            TweedleStatement tested = ParseStatement("WholeNumber a <- 2;");
            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void AVariableDeclarationShouldBeCreatedForVariableDeclaration()
        {
            TweedleStatement tested = ParseStatement("WholeNumber a <- 2;");
            Assert.IsInstanceOf<LocalVariableDeclaration>(tested, "The parser should have returned a TweedleLocalVariableDeclaration.");
        }

        [Test]
        public void AVariableDeclarationShouldBeConstant()
        {
            LocalVariableDeclaration tested = (LocalVariableDeclaration)ParseStatement("constant WholeNumber a <- 2;");
            Assert.IsTrue(tested.IsConstant, "The TweedleLocalVariableDeclaration should be constant.");
        }

        [Test]
        public void AVariableDeclarationShouldNotBeConstant()
        {
            LocalVariableDeclaration tested = (LocalVariableDeclaration)ParseStatement("WholeNumber a <- 2;");
            Assert.IsFalse(tested.IsConstant, "The TweedleLocalVariableDeclaration should not be constant.");
        }

        [Test]
        public void AVariableDeclarationShouldHaveAnInitializer()
        {
            LocalVariableDeclaration tested = (LocalVariableDeclaration)ParseStatement("WholeNumber a <- 2;");
            Assert.NotNull(tested.Variable.Initializer, "The TweedleLocalVariableDeclaration should be constant.");
        }

        [Test]
        public void AVariableDeclarationShouldHaveAnInitializerValue()
        {
            TweedleExpression tested = ((LocalVariableDeclaration)ParseStatement("WholeNumber a <- 2;"))
                .Variable.Initializer;
            Assert.IsInstanceOf<TweedlePrimitiveValue<int>>(tested, "The TweedleLocalVariableDeclaration should hold a integer primitive value.");
        }

        [Test]
        public void AVariableDeclarationShouldHaveAnInitializerOfTwo()
        {
            TweedlePrimitiveValue<int> tested = (TweedlePrimitiveValue< int >)((LocalVariableDeclaration)ParseStatement("WholeNumber a <- 2;"))
                .Variable.Initializer;
            Assert.AreEqual(2, tested.Value, "The TweedleLocalVariableDeclaration should hold 2.");
        }

        [Test]
        public void NestedDoInOrdersOuterOneShouldBeEnabled()
        {
            DoInOrder tested = (DoInOrder)ParseStatement("doInOrder { doInOrder {} }");
            Assert.IsTrue(tested.IsEnabled, "The outer doInOrder should be enabled.");
        }

        [Test]
        public void NestedDoInOrdersInnerOneShouldBeEnabled()
        {
            DoInOrder tested = (DoInOrder)ParseStatement("doInOrder { doInOrder {} }");
			Assert.IsTrue(tested.Body.Statements[0].IsEnabled, "The inner doInOrder should be enabled.");
        }

        [Test]
        public void DisabledNestedDoInOrdersOuterOneShouldBeDisabled()
        {
            DoInOrder tested = (DoInOrder)ParseStatement("*< doInOrder { doInOrder {} } >*");
            Assert.IsFalse(tested.IsEnabled, "The outer doInOrder should be disabled.");
        }

        [Test]
        public void DisabledNestedDoInOrdersInnerOneShouldBeEnabled()
        {
            DoInOrder tested = (DoInOrder)ParseStatement("*< doInOrder { doInOrder {} } >*");
			Assert.IsTrue(tested.Body.Statements[0].IsEnabled, "The inner doInOrder should be enabled.");
        }

        [Test]
        public void DisabledInnerDoInOrdersOuterOneShouldBeEnabled()
        {
            DoInOrder tested = (DoInOrder)ParseStatement("doInOrder { *< doInOrder {} >* }");
            Assert.IsTrue(tested.IsEnabled, "The outer doInOrder should be enabled.");
        }

        [Test]
        public void DisabledInnerDoInOrdersInnerOneShouldBeDisabled()
        {
            DoInOrder tested = (DoInOrder)ParseStatement("doInOrder { *< doInOrder {} >* }");
			Assert.IsFalse(tested.Body.Statements[0].IsEnabled, "The inner doInOrder should be disabled.");
        }
    }
}

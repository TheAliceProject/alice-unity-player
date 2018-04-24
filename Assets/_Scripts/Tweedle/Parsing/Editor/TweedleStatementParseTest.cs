using NUnit.Framework;

namespace Alice.Tweedle.Unlinked
{
    public class TweedleStatementParseTest
    {
        private TweedleStatement ParseStatement(string src)
        {
            return new TweedleUnlinkedParser().ParseStatement(src);
        }

        [Test]
        public void somethingShouldBeCreatedForCountUpTo()
        {
            TweedleStatement tested = ParseStatement("countUpTo( indexB < 2 ) {}");
            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void aTweedleCountLoopShouldBeCreatedForCountUpTo()
        {
            TweedleStatement tested = ParseStatement("countUpTo( indexB < 2 ) {}");
            Assert.IsInstanceOf<TweedleCountLoop>(tested, "The parser should have returned a TweedleCountLoop.");
        }

        [Test]
        public void aTweedleCountLoopShouldHaveBlockOfStatements()
        {
            TweedleCountLoop tested = (TweedleCountLoop)ParseStatement("countUpTo( indexB < 2 ) {}");
            Assert.NotNull(tested.Statements, "The TweedleCountLoop should have a list of statements.");
        }

        [Test]
        public void aTweedleCountLoopShouldHaveEmptyBlockOfStatements()
        {
            TweedleCountLoop tested = (TweedleCountLoop)ParseStatement("countUpTo( indexB < 2 ) {}");
            Assert.IsEmpty(tested.Statements, "The TweedleCountLoop should have an empty list of statements.");
        }

        [Test]
        public void aTweedleCountLoopShouldHaveVariableDeclaration()
        {
            TweedleCountLoop tested = (TweedleCountLoop)ParseStatement("countUpTo( indexB < 2 ) {}");
            Assert.NotNull(tested.Variable, "The TweedleCountLoop should have a Variable declaration.");
        }

        [Test]
        public void aTweedleCountLoopShouldHaveWholeNumberVariable()
        {
            TweedleCountLoop tested = (TweedleCountLoop)ParseStatement("countUpTo( indexB < 2 ) {}");
            Assert.AreEqual(TweedleTypes.WHOLE_NUMBER, tested.Variable.Type, "The TweedleCountLoop should have a Variable of type WholeNumber.");
        }

        [Test]
        public void aTweedleCountLoopShouldHaveCorrectlyNamedVariable()
        {
            TweedleCountLoop tested = (TweedleCountLoop)ParseStatement("countUpTo( indexB < 2 ) {}");
            Assert.AreEqual("indexB", tested.Variable.Name, "The TweedleCountLoop should have a Variable named indexB.");
        }

        [Test]
        public void somethingShouldBeCreatedForIfThen()
        {
            TweedleStatement tested = ParseStatement("if(true) { }");
            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void aConditionalShouldBeCreatedForIfThen()
        {
            TweedleStatement tested = ParseStatement("if(true) { }");
            Assert.IsInstanceOf<TweedleConditionalStatement>(tested, "The parser should have returned a TweedleConditionalStatement.");
        }

        [Test]
        public void aConditionalCreatedForIfThenShouldHaveCondition()
        {
            TweedleConditionalStatement tested = (TweedleConditionalStatement)ParseStatement("if(true) { }");
            Assert.AreEqual(TweedleTypes.TRUE, tested.Condition, "The parser should have returned a TweedleConditionalStatement with a true condition.");
        }

        [Test]
        public void aConditionalCreatedForIfThenShouldHaveThenStatementList()
        {
            TweedleConditionalStatement tested = (TweedleConditionalStatement)ParseStatement("if(true) { }");
            Assert.IsEmpty(tested.ThenBody, "The parser should have returned a TweedleConditionalStatement with an empty then block.");
        }

        [Test]
        public void aConditionalCreatedForIfThenShouldHaveElseStatementList()
        {
            TweedleConditionalStatement tested = (TweedleConditionalStatement)ParseStatement("if(true) { }");
            Assert.IsEmpty(tested.ElseBody, "The parser should have returned a TweedleConditionalStatement with an empty else block.");
        }

        [Test]
        public void somethingShouldBeCreatedForIfThenElse()
        {
            TweedleStatement tested = ParseStatement("if(true) { } else { }");
            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void aConditionalShouldBeCreatedForIfThenElse()
        {
            TweedleStatement tested = ParseStatement("if(true) { } else { }");
            Assert.IsInstanceOf<TweedleConditionalStatement>(tested, "The parser should have returned a TweedleConditionalStatement.");
        }

        [Test]
        public void aConditionalCreatedForIfThenElseShouldHaveThenStatementList()
        {
            TweedleConditionalStatement tested = (TweedleConditionalStatement)ParseStatement("if(true) { } else { }");
            Assert.IsEmpty(tested.ThenBody, "The parser should have returned a TweedleConditionalStatement with an empty then block.");
        }

        [Test]
        public void aConditionalCreatedForIfThenElseShouldHaveElseStatementList()
        {
            TweedleConditionalStatement tested = (TweedleConditionalStatement)ParseStatement("if(true) { } else { }");
            Assert.IsEmpty(tested.ElseBody, "The parser should have returned a TweedleConditionalStatement with an empty else block.");
        }

        [Test]
        public void somethingShouldBeCreatedForForEach()
        {
            TweedleStatement tested = ParseStatement("forEach(SModel x in new SModel[] {this.sphere, this.walrus} ) {}");
            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void aForEachLoopShouldBeCreatedForForEach()
        {
            TweedleStatement tested = ParseStatement("forEach(SModel x in new SModel[] {this.sphere, this.walrus} ) {}");
            Assert.IsInstanceOf<TweedleForEachInArrayLoop>(tested, "The parser should have returned a ForEachLoop.");
        }

        [Test]
        public void somethingShouldBeCreatedTweedleEachInArrayTogether()
        {
            TweedleStatement tested = ParseStatement("eachTogether(SModel x in new SModel[] {this.sphere, this.walrus} ) {}");
            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void aTweedleEachInArrayTogetherShouldBeCreatedTweedleEachInArrayTogether()
        {
            TweedleStatement tested = ParseStatement("eachTogether(SModel x in new SModel[] {this.sphere, this.walrus} ) {}");
            Assert.IsInstanceOf<TweedleEachInArrayTogether>(tested, "The parser should have returned a TweedleEachInArrayTogether.");
        }

        [Test]
        public void anEachTogetherShouldHaveTypedVariable()
        {
            TweedleEachInArrayTogether tested = (TweedleEachInArrayTogether)ParseStatement("eachTogether(SModel x in new SModel[] {this.sphere, this.walrus} ) {}");
            Assert.AreEqual("SModel", tested.ItemVariable.Type.Name, "The EachTogether should have an SModel variable.");
        }

        [Test]
        public void anEachTogetherValuesShouldHaveArrayType()
        {
            TweedleEachInArrayTogether tested = (TweedleEachInArrayTogether)ParseStatement("eachTogether(SModel x in new SModel[] {this.sphere, this.walrus} ) {}");
            Assert.IsInstanceOf<TweedleArrayType>(tested.Array.Type, "The EachTogether values should be typed as an array.");
        }

        [Test]
        public void anEachTogetherValuesShouldHaveElementTypeSModel()
        {
            TweedleEachInArrayTogether tested = (TweedleEachInArrayTogether)ParseStatement("eachTogether(SModel x in new SModel[] {this.sphere, this.walrus} ) {}");
            TweedleArrayType valArrayType = (TweedleArrayType)tested.Array.Type;
            Assert.AreEqual(valArrayType.ValueType,
                            tested.ItemVariable.Type,
                            "The EachTogether individual values should be typed as SModel.");
        }

        [Test]
        public void anEachTogetherShouldHaveBlockOfStatements()
        {
            TweedleEachInArrayTogether tested = (TweedleEachInArrayTogether)ParseStatement("eachTogether(SModel x in new SModel[] {this.sphere, this.walrus} ) {}");
            Assert.NotNull(tested.Statements, "The EachTogether should have a list of statements.");
        }

        [Test]
        public void anEachTogetherShouldHaveEmptyBlockOfStatements()
        {
            TweedleEachInArrayTogether tested = (TweedleEachInArrayTogether)ParseStatement("eachTogether(SModel x in new SModel[] {this.sphere, this.walrus} ) {}");
            Assert.IsEmpty(tested.Statements, "The EachTogether should have an empty list of statements.");
        }

        [Test]
        public void somethingShouldBeCreatedForWhile()
        {
            TweedleStatement tested = ParseStatement("while(true) { }");
            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void aWhileLoopShouldBeCreatedForWhile()
        {
            TweedleStatement tested = ParseStatement("while(true) { }");
            Assert.IsInstanceOf<TweedleWhileLoop>(tested, "The parser should have returned a TweedleWhileLoop.");
        }

        [Test]
        public void aWhileLoopShouldHaveRunCondition()
        {
            TweedleWhileLoop tested = (TweedleWhileLoop)ParseStatement("while(true) { }");
            Assert.NotNull(tested.RunCondition, "The TweedleWhileLoop should have a run condition.");
        }

        [Test]
        public void thisWhileLoopRunConditionShouldBeABooleanTrue()
        {
            TweedleWhileLoop tested = (TweedleWhileLoop)ParseStatement("while(true) { }");
            Assert.AreEqual(TweedleTypes.TRUE, tested.RunCondition, "The TweedleWhileLoop run condition should be the value True.");
        }

        [Test]
        public void aWhileLoopShouldHaveBlockOfStatements()
        {
            TweedleWhileLoop tested = (TweedleWhileLoop)ParseStatement("while(true) { }");
            Assert.NotNull(tested.Statements, "The TweedleWhileLoop should have a list of statements.");
        }

        [Test]
        public void aWhileLoopShouldHaveEmptyBlockOfStatements()
        {
            TweedleWhileLoop tested = (TweedleWhileLoop)ParseStatement("while(true) { }");
            Assert.IsEmpty(tested.Statements, "The TweedleWhileLoop should have an empty list of statements.");
        }

        [Test]
        public void somethingShouldBeCreatedForDoInOrder()
        {
            TweedleStatement tested = ParseStatement("doInOrder { }");
            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void aDoInOrderShouldBeCreated()
        {
            TweedleStatement tested = ParseStatement("doInOrder { }");
            Assert.IsInstanceOf<TweedleDoInOrder>(tested, "The parser should have returned a TweedleDoInOrder.");
        }

        [Test]
        public void aDoInOrderShouldHaveBlockOfStatements()
        {
            TweedleDoInOrder tested = (TweedleDoInOrder)ParseStatement("doInOrder { }");
            Assert.NotNull(tested.Statements, "The TweedleDoInOrder should have a list of statements.");
        }

        [Test]
        public void aDoInOrderShouldHaveEmptyBlockOfStatements()
        {
            TweedleDoInOrder tested = (TweedleDoInOrder)ParseStatement("doInOrder { }");
            Assert.IsEmpty(tested.Statements, "The TweedleDoInOrder should have an empty list of statements.");
        }

        [Test]
        public void aDoInOrderShouldHaveBlockOfTwoStatements()
        {
            TweedleDoInOrder tested = (TweedleDoInOrder)ParseStatement("doInOrder { doInOrder {} return; }");
            Assert.AreEqual(2, tested.Statements.Count, "The TweedleDoInOrder should have a list of 2 statements.");
        }

        [Test]
        public void aTweedleDoInOrdersFirstStatementShouldBeDoInOrder()
        {
            TweedleDoInOrder tested = (TweedleDoInOrder)ParseStatement("doInOrder { doInOrder {} return; }");
            Assert.IsInstanceOf<TweedleDoInOrder>(tested.Statements[0], "The block's first statement should be a TweedleDoInOrder.");
        }

        [Test]
        public void aDoInOrdersSecondStatementShouldBeReturn()
        {
            TweedleDoInOrder tested = (TweedleDoInOrder)ParseStatement("doInOrder { doInOrder {} return; }");
            Assert.IsInstanceOf<TweedleReturnStatement>(tested.Statements[1], "The block's first statement should be a Return.");
        }

        [Test]
        public void somethingShouldBeCreatedForDoTogether()
        {
            TweedleStatement tested = ParseStatement("doTogether { }");
            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void aDoTogetherShouldBeCreated()
        {
            TweedleStatement tested = ParseStatement("doTogether { }");
            Assert.IsInstanceOf<TweedleDoTogether>(tested, "The parser should have returned a TweedleDoTogether.");
        }

        [Test]
        public void aDoTogetherShouldHaveBlockOfStatements()
        {
            TweedleDoTogether tested = (TweedleDoTogether)ParseStatement("doTogether { }");
            Assert.NotNull(tested.Statements, "The TweedleDoTogether should have a list of statements.");
        }

        [Test]
        public void aDoTogetherShouldHaveEmptyBlockOfStatements()
        {
            TweedleDoTogether tested = (TweedleDoTogether)ParseStatement("doTogether { }");
            Assert.IsEmpty(tested.Statements, "The TweedleDoTogether should have an empty list of statements.");
        }

        [Test]
        public void aDoTogetherShouldHaveBlockOfTwoStatements()
        {
            TweedleDoTogether tested = (TweedleDoTogether)ParseStatement("doTogether { doInOrder {} return; }");
            Assert.AreEqual(2, tested.Statements.Count, "The TweedleDoTogether should have a list of 2 statements.");
        }

        [Test]
        public void aDoTogethersFirstStatementShouldBeDoInOrder()
        {
            TweedleDoTogether tested = (TweedleDoTogether)ParseStatement("doTogether { doInOrder {} return; }");
            Assert.IsInstanceOf<TweedleDoInOrder>(tested.Statements[0], "The block's first statement should be a DoInorder.");
        }

        [Test]
        public void aDoTogethersSecondStatementShouldBeReturn()
        {
            TweedleDoTogether tested = (TweedleDoTogether)ParseStatement("doTogether { doInOrder {} return; }");
            Assert.IsInstanceOf<TweedleReturnStatement>(tested.Statements[1], "The block's first statement should be a Return.");
        }

        [Test]
        public void somethingShouldBeCreatedForEmptyReturn()
        {
            TweedleStatement tested = ParseStatement("return;");
            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void aReturnShouldBeCreatedForEmptyReturn()
        {
            TweedleStatement tested = ParseStatement("return;");
            Assert.IsInstanceOf<TweedleReturnStatement>(tested, "The parser should have returned a TweedleReturnStatement.");
        }

        [Test]
        public void anEmptyReturnShouldBeCreatedForEmptyReturn()
        {
            TweedleReturnStatement tested = (TweedleReturnStatement)ParseStatement("return;");
            Assert.AreEqual(TweedleNull.NULL, tested.Expression, "The TweedleReturnStatement should hold TweedleNull.");
        }

        [Test]
        public void anVoidTypeReturnShouldBeCreatedForEmptyReturn()
        {
            TweedleReturnStatement tested = (TweedleReturnStatement)ParseStatement("return;");
            Assert.AreEqual(TweedleVoidType.VOID, tested.Type, "The TweedleReturnStatement should be type void.");
        }

        [Test]
        public void somethingShouldBeCreatedForReturnWithValue()
        {
            TweedleStatement tested = ParseStatement("return 4;");
            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void aReturnShouldBeCreatedForReturnWithValue()
        {
            TweedleStatement tested = ParseStatement("return 4;");
            Assert.IsInstanceOf<TweedleReturnStatement>(tested, "The parser should have returned a TweedleReturnStatement.");
        }

        [Test]
        public void aReturnExpressionShouldBeCreatedForReturnWithValue()
        {
            TweedleReturnStatement tested = (TweedleReturnStatement)ParseStatement("return 4;");
            Assert.NotNull(tested.Expression, "The TweedleReturnStatement should hold no value.");
        }

        [Test]
        public void aWholeNumberTypeReturnShouldBeCreatedForReturnWithValue()
        {
            TweedleReturnStatement tested = (TweedleReturnStatement)ParseStatement("return 4;");
            Assert.AreEqual(TweedleTypes.WHOLE_NUMBER, tested.Type, "The TweedleReturnStatement should be type WholeNumber.");
        }

        [Test]
        public void somethingShouldBeCreatedForAssignment()
        {
            TweedleStatement tested = ParseStatement("x <- 3;");
            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void anExpressionStatementShouldBeCreated()
        {
            TweedleStatement tested = ParseStatement("x <- 3;");
            Assert.IsInstanceOf<TweedleExpressionStatement>(tested, "The parser should have returned an ExpressionStatement.");
        }

        [Test]
        public void anAssignmentExpressionShouldBeCreated()
        {
            TweedleExpressionStatement tested = (TweedleExpressionStatement)ParseStatement("x <- 3;");
            Assert.IsInstanceOf<AssignmentExpression>(tested.Expression, "The Statement should have contained an AssignmentExpression.");
        }

        [Test]
        public void somethingShouldBeCreatedForVariableDeclaration()
        {
            TweedleStatement tested = ParseStatement("WholeNumber a <- 2;");
            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void aVariableDeclarationShouldBeCreatedForVariableDeclaration()
        {
            TweedleStatement tested = ParseStatement("WholeNumber a <- 2;");
            Assert.IsInstanceOf<TweedleLocalVariableDeclaration>(tested, "The parser should have returned a TweedleLocalVariableDeclaration.");
        }

        [Test]
        public void aVariableDeclarationShouldBeConstant()
        {
            TweedleLocalVariableDeclaration tested = (TweedleLocalVariableDeclaration)ParseStatement("constant WholeNumber a <- 2;");
            Assert.IsTrue(tested.IsConstant, "The TweedleLocalVariableDeclaration should be constant.");
        }

        [Test]
        public void aVariableDeclarationShouldNotBeConstant()
        {
            TweedleLocalVariableDeclaration tested = (TweedleLocalVariableDeclaration)ParseStatement("WholeNumber a <- 2;");
            Assert.IsFalse(tested.IsConstant, "The TweedleLocalVariableDeclaration should not be constant.");
        }

        [Test]
        public void aVariableDeclarationShouldHaveAnInitializer()
        {
            TweedleLocalVariableDeclaration tested = (TweedleLocalVariableDeclaration)ParseStatement("WholeNumber a <- 2;");
            Assert.NotNull(tested.Variable.Initializer, "The TweedleLocalVariableDeclaration should be constant.");
        }

        [Test]
        public void aVariableDeclarationShouldHaveAnInitializerValue()
        {
            TweedleExpression tested = ((TweedleLocalVariableDeclaration)ParseStatement("WholeNumber a <- 2;"))
                .Variable.Initializer;
            Assert.IsInstanceOf<TweedlePrimitiveValue<int>>(tested, "The TweedleLocalVariableDeclaration should hold a integer primitive value.");
        }

        [Test]
        public void aVariableDeclarationShouldHaveAnInitializerOfTwo()
        {
            TweedlePrimitiveValue<int> tested = (TweedlePrimitiveValue< int >)((TweedleLocalVariableDeclaration)ParseStatement("WholeNumber a <- 2;"))
                .Variable.Initializer;
            Assert.AreEqual(2, tested.Value, "The TweedleLocalVariableDeclaration should hold 2.");
        }

        [Test]
        public void nestedDoInOrdersOuterOneShouldBeEnabled()
        {
            TweedleDoInOrder tested = (TweedleDoInOrder)ParseStatement("doInOrder { doInOrder {} }");
            Assert.IsTrue(tested.IsEnabled, "The outer doInOrder should be enabled.");
        }

        [Test]
        public void nestedDoInOrdersInnerOneShouldBeEnabled()
        {
            TweedleDoInOrder tested = (TweedleDoInOrder)ParseStatement("doInOrder { doInOrder {} }");
            Assert.IsTrue(tested.Statements[0].IsEnabled, "The inner doInOrder should be enabled.");
        }

        [Test]
        public void disabledNestedDoInOrdersOuterOneShouldBeDisabled()
        {
            TweedleDoInOrder tested = (TweedleDoInOrder)ParseStatement("*< doInOrder { doInOrder {} } >*");
            Assert.IsFalse(tested.IsEnabled, "The outer doInOrder should be disabled.");
        }

        [Test]
        public void disabledNestedDoInOrdersInnerOneShouldBeEnabled()
        {
            TweedleDoInOrder tested = (TweedleDoInOrder)ParseStatement("*< doInOrder { doInOrder {} } >*");
            Assert.IsTrue(tested.Statements[0].IsEnabled, "The inner doInOrder should be enabled.");
        }

        [Test]
        public void disabledInnerDoInOrdersOuterOneShouldBeEnabled()
        {
            TweedleDoInOrder tested = (TweedleDoInOrder)ParseStatement("doInOrder { *< doInOrder {} >* }");
            Assert.IsTrue(tested.IsEnabled, "The outer doInOrder should be enabled.");
        }

        [Test]
        public void disabledInnerDoInOrdersInnerOneShouldBeDisabled()
        {
            TweedleDoInOrder tested = (TweedleDoInOrder)ParseStatement("doInOrder { *< doInOrder {} >* }");
            Assert.IsFalse(tested.Statements[0].IsEnabled, "The inner doInOrder should be disabled.");
        }
    }
}

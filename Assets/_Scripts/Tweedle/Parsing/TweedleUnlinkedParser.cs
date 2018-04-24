﻿using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System.Collections.Generic;
using System.Linq;
using Alice.Tweedle;

namespace Alice.Tweedle.Unlinked
{
	public class TweedleUnlinkedParser
	{
		public TweedleParser ParseSource(string src)
		{
			AntlrInputStream antlerStream = new AntlrInputStream(src);
			TweedleLexer lexer = new TweedleLexer(antlerStream);
			CommonTokenStream tokenStream = new CommonTokenStream(lexer);
			//Alice.Tweedle.TweedleParser parser = new Alice.Tweedle.TweedleParser(tokenStream);
			return new TweedleParser(tokenStream);
		}

		public TweedleType ParseType(string src)
		{
			return new TypeVisitor().Visit(ParseSource(src).typeDeclaration());
        }

        public TweedleStatement ParseStatement(string src)
        {
            return new StatementVisitor().Visit(ParseSource(src).blockStatement());
        }

		public TweedleExpression ParseExpression(string src)
		{
			return new ExpressionVisitor().Visit(ParseSource(src).expression());
        }

		private class TypeVisitor : TweedleParserBaseVisitor<TweedleType>
		{
			public override TweedleType VisitClassDeclaration([NotNull] TweedleParser.ClassDeclarationContext context)
			{
				base.VisitClassDeclaration(context);
				string className = context.identifier().GetText();
				TweedleClass tweClass;
				if (context.EXTENDS() != null)
				{
					string superclass = context.typeType().classOrInterfaceType().GetText();
					tweClass = new TweedleClass(className, new TweedleTypeReference(superclass)); // TODO
				} else
				{
					tweClass = new TweedleClass(className);
				}

				ClassBodyDeclarationVisitor cbdVisitor = new ClassBodyDeclarationVisitor(tweClass);
				context.classBody().classBodyDeclaration().ToList().ForEach(cbd => cbd.Accept(cbdVisitor));
				return tweClass;
			}

			public override TweedleType VisitEnumDeclaration([NotNull] TweedleParser.EnumDeclarationContext context)
			{
				base.VisitEnumDeclaration(context);
				List<string> values =
					context.enumConstants().enumConstant().Select(enumConst => enumConst.identifier().GetText()).ToList();
				return new TweedleEnum(context.identifier().GetText(), values);
			}
		}

        private class ClassBodyDeclarationVisitor : TweedleParserBaseVisitor<object>
		{
            private readonly TweedleClass tweClass;

			public ClassBodyDeclarationVisitor(TweedleClass tweClass)
			{
				this.tweClass = tweClass;
			}

			public override object VisitClassBodyDeclaration([NotNull] TweedleParser.ClassBodyDeclarationContext context)
			{
                TweedleParser.MemberDeclarationContext memberDec = context.memberDeclaration();
                if (memberDec != null)
                {
                    List<string> modifiers =
                        context.classModifier().Select(classMod => (classMod.STATIC() != null) ? "static" : null).ToList();
                    memberDec.Accept(new MemberDeclarationVisitor(tweClass, modifiers));
                }
                return base.VisitClassBodyDeclaration(context);
			}
		}

		private class MemberDeclarationVisitor : TweedleParserBaseVisitor<object>
		{
			private TweedleClass tweClass;
			private List<string> modifiers;

			public MemberDeclarationVisitor(TweedleClass tweClass, List<string> modifiers)
			{
				this.tweClass = tweClass;
				this.modifiers = modifiers;
			}

            public override object VisitFieldDeclaration([NotNull] TweedleParser.FieldDeclarationContext context)
            {
                TweedleType type = GetTypeType(context.typeType());
                string name = context.variableDeclarator().variableDeclaratorId().IDENTIFIER().GetText();
                TweedleField property;
                if (context.variableDeclarator().variableInitializer() != null)
                {
                    ExpressionVisitor initVisitor = new ExpressionVisitor(type);
                    TweedleExpression init =
                                    context.variableDeclarator().variableInitializer().Accept(initVisitor);
                    property = new TweedleField(modifiers, type, name, init);
                }
                else
                {
                    property = new TweedleField(modifiers, type, name);
                }
                tweClass.properties.Add(property);
                return null;
            }

            public override object VisitMethodDeclaration([NotNull] TweedleParser.MethodDeclarationContext context)
			{
				TweedleMethod method = new TweedleMethod(
						modifiers,
						GetTypeOrVoid(context.typeTypeOrVoid()),
						context.IDENTIFIER().GetText(),
						RequiredParameters(context.formalParameters()),
						OptionalParameters(context.formalParameters()),
                        CollectBlockStatements(context.methodBody().block().blockStatement())
					);
				tweClass.methods.Add(method);
				return base.VisitMethodDeclaration(context);
			}

            public override object VisitConstructorDeclaration([NotNull] TweedleParser.ConstructorDeclarationContext context)
			{
				TweedleConstructor constructor = new TweedleConstructor(
					GetTypeReference(context.IDENTIFIER().GetText()),
					context.IDENTIFIER().GetText(),
					RequiredParameters(context.formalParameters()),
                    OptionalParameters(context.formalParameters()),
                    CollectBlockStatements(context.constructorBody.blockStatement())
				);
				tweClass.constructors.Add(constructor);
				return base.VisitConstructorDeclaration(context);
			}

			private List<TweedleRequiredParameter> RequiredParameters(TweedleParser.FormalParametersContext context)
			{
				if (context.formalParameterList() == null || context.formalParameterList().requiredParameter() == null)
				{
					return new List<TweedleRequiredParameter>();
				}

				return context.formalParameterList().requiredParameter().Select(field => {
					TweedleType type = GetTypeType(field.typeType());
					return new TweedleRequiredParameter(
						type,
						field.variableDeclaratorId().IDENTIFIER().GetText()
						);
				}).ToList();
			}

            private List<TweedleOptionalParameter> OptionalParameters(TweedleParser.FormalParametersContext context)
			{
				if (context.formalParameterList() == null || context.formalParameterList().optionalParameter() == null)
				{
                    return new List<TweedleOptionalParameter>();
				}

                return context.formalParameterList().optionalParameter().Select(field => {
					TweedleType type = GetTypeType(field.typeType());
					return new TweedleOptionalParameter(
						type,
						field.variableDeclaratorId().IDENTIFIER().GetText(),
						field.Accept(new ExpressionVisitor(type))
						);
				}).ToList();
			}
		}

        private class ExpressionVisitor : TweedleParserBaseVisitor<TweedleExpression>
        {
			private TweedleType expectedType;

			public ExpressionVisitor(TweedleType expectedType)
			{
				this.expectedType = expectedType;
            }

            public ExpressionVisitor()
            {
                this.expectedType = null;
            }

			public override TweedleExpression VisitExpression([NotNull] TweedleParser.ExpressionContext context)
            {
                TweedleExpression expression = ParseExpression(context);

                if (expectedType != null && expression != null && expression.Type != null
                    && !expectedType.AcceptsType(expression.Type))
                {
                    UnityEngine.Debug.Log(
                        "Had been expecting expression of type " + expectedType + ", but it is typed as " + expression.Type);
                }
                return expression;
			}

			public TweedleExpression ParseExpression([NotNull] TweedleParser.ExpressionContext context)
            {
                if (context.NEW() != null)
                {
                    return InstantiationExpression(context);
                }

                IToken prefix = context.prefix;
				IToken operation = context.bop;

				if (prefix != null)
				{
					switch (prefix.Text)
					{
						case "+":
							return TypedExpression(context, TweedleTypes.NUMBER)[0];
						case "-":

							return NegativeExpression(TypedExpression(context, TweedleTypes.NUMBER)[0]);
						case "!":
							return new LogicalNotExpression(TypedExpression(context, TweedleTypes.BOOLEAN)[0]);
						default:
							throw new System.Exception("Expression has prefix name but cannot be converted to prefix expression.");
					}
				}
                else if (operation != null)
                {
					List<TweedleExpression> expressions;
					switch (operation.Text) {
                        case "." :
                            return FieldOrMethodRef(context);
                        case "*":
							return NewBinaryNumericExpression<MultiplicationDecimalExpression, MultiplicationWholeExpression>(context, TweedleTypes.NUMBER);
                        case "/":
							return NewBinaryNumericExpression<DivisionDecimalExpression, DivisionWholeExpression>(context, TweedleTypes.NUMBER);
						case "%":
							expressions = TypedExpression(context, TweedleTypes.WHOLE_NUMBER);
							return new ModuloExpression(expressions[0], expressions[1]);
						case "+":
							return NewBinaryNumericExpression<AdditionDecimalExpression, AdditionWholeExpression>(context, TweedleTypes.NUMBER);
						case "-":
							return NewBinaryNumericExpression<SubtractionDecimalExpression, SubtractionWholeExpression>(context, TweedleTypes.NUMBER);
						case "<=":
							return NewBinaryNumericExpression<LessThanOrEqualDecimalExpression, LessThanOrEqualWholeExpression>(context, TweedleTypes.NUMBER);
						case ">=":
							return NewBinaryNumericExpression<GreaterThanOrEqualDecimalExpression, GreaterThanOrEqualWholeExpression>(context, TweedleTypes.NUMBER);
						case ">":
							return NewBinaryNumericExpression<GreaterThanDecimalExpression, GreaterThanWholeExpression>(context, TweedleTypes.NUMBER);
						case "<":
							return NewBinaryNumericExpression<LessThanDecimalExpression, LessThanWholeExpression>(context, TweedleTypes.NUMBER);
						case "==":
							expressions = TypedExpression(context, null);
							return new EqualToExpression(expressions[0], expressions[1]);
						case "!=":
							expressions = TypedExpression(context, null);
							return new NotEqualToExpression(expressions[0], expressions[1]);
						case "&&":
							expressions = TypedExpression(context, TweedleTypes.BOOLEAN);
							return new LogicalAndExpression(expressions[0], expressions[1]);
						case "||":
							expressions = TypedExpression(context, TweedleTypes.BOOLEAN);
                            return new LogicalOrExpression(expressions[0], expressions[1]);
                        case "<-":
                            expressions = TypedExpression(context, null);
                            return new AssignmentExpression(expressions[0], expressions[1]);
						default :
                            throw new System.Exception("Binary operation not found.");
                    }

                }
                return base.VisitChildren(context); ;
			}

            private TweedleExpression InstantiationExpression(TweedleParser.ExpressionContext context)
            {
                string typeName = context.creator().createdName().GetText();
                TweedleParser.ArrayCreatorRestContext arrayDetails = context.creator().arrayCreatorRest();
                if (arrayDetails != null)
                {
                    TweedleType prim = GetPrimitiveType(typeName);
                    TweedleType memberType = prim ?? GetTypeReference(typeName);
                    if (arrayDetails.arrayInitializer() != null)
                    {
                        return new TweedleArrayInitializer(
                                        new TweedleArrayType(memberType),
                            arrayDetails.arrayInitializer().expression().Select(a=>a.Accept(new ExpressionVisitor(memberType)))
                                                     .ToList());
                    }
                    // TODO return sized array
                }
                else
                {
                    //TODO Object instantiation
                }
                return null;
            }

            private TweedleExpression FieldOrMethodRef(TweedleParser.ExpressionContext context)
            {
                // Use untyped expression visitor for target
                TweedleExpression target = context.expression(0).Accept(new ExpressionVisitor());
                if (context.IDENTIFIER() != null)
                {
                    return new FieldAccess(target, context.IDENTIFIER().GetText());
                }
                if (context.methodCall() != null)
                {
                    // TODO read labeledExpressionList
                    return new MethodCallExpression(target, context.methodCall().IDENTIFIER().GetText());
                }
                throw new System.Exception("Unexpected details on context " + context);
            }

			public override TweedleExpression VisitPrimary([NotNull] TweedleParser.PrimaryContext context)
            {
                if (context.expression() != null)
                {
                    // Parenthesized child expression
                    return this.VisitExpression(context.expression());
                }
				if (context.THIS() != null)
				{
					return new ThisExpression(null);
                }
                if (context.IDENTIFIER() != null)
                {
                    return new IdentifierReference(context.IDENTIFIER().GetText());
                }

				return base.VisitPrimary(context);
			}

			public override TweedleExpression VisitLiteral([NotNull] TweedleParser.LiteralContext context)
			{
				if (context.DECIMAL_LITERAL() != null)
				{
					int value = System.Convert.ToInt32(context.DECIMAL_LITERAL().GetText());
					return TweedleTypes.WHOLE_NUMBER.Instantiate(value);
				}

				if (context.FLOAT_LITERAL() != null)
				{
					double value = System.Convert.ToDouble(context.FLOAT_LITERAL().GetText());
					return TweedleTypes.DECIMAL_NUMBER.Instantiate(value);
				}

				if (context.NULL_LITERAL() != null)
				{
					return TweedleNull.NULL;
				}

				if (context.BOOL_LITERAL() != null)
				{
					bool value = System.Convert.ToBoolean(context.BOOL_LITERAL().GetText());
					return TweedleTypes.BOOLEAN.Instantiate(value);
				}

				if (context.STRING_LITERAL() != null)
				{
					string value = context.STRING_LITERAL().GetText();
					return TweedleTypes.TEXT_STRING.Instantiate(value.Substring(1, value.Length - 2));
				}

				throw new System.Exception("Literal couldn't be found in grammar.");
			}

			private List<TweedleExpression> TypedExpression(TweedleParser.ExpressionContext context, TweedleType type)
			{
				ExpressionVisitor expVisitor = new ExpressionVisitor(type);
				return context.expression().Select(exp => exp.Accept(expVisitor)).ToList();
			}

			private TweedleExpression NegativeExpression(TweedleExpression exp)
			{
				NegativeExpression negativeExp;
				if (exp.Type == TweedleTypes.DECIMAL_NUMBER)
				{
					negativeExp = new NegativeDecimalExpression(exp);
				} else if (exp.Type == TweedleTypes.WHOLE_NUMBER)
				{
					negativeExp = new NegativeWholeExpression(exp);
				} else
				{
					throw new System.Exception("Expression cannot be converted into a negative expression.");
				}
				if (exp is TweedlePrimitiveValue)
				{
					return negativeExp.Evaluate(null);
				}
				return negativeExp;
			}

			private TweedleExpression NewBinaryNumericExpression<Decimal, Whole>(TweedleParser.ExpressionContext context, TweedleType type)
			{
				List<TweedleExpression> expressions = TypedExpression(context, type);
				if (expressions[0].Type == TweedleTypes.DECIMAL_NUMBER || expressions[1].Type == TweedleTypes.DECIMAL_NUMBER)
				{
					return (TweedleExpression)System.Activator.CreateInstance(typeof(Decimal), new object[] { expressions[0], expressions[1] });
				}
				else if (expressions[0].Type == TweedleTypes.WHOLE_NUMBER || expressions[1].Type == TweedleTypes.WHOLE_NUMBER)
				{
					return (TweedleExpression)System.Activator.CreateInstance(typeof(Whole), new object[] { expressions[0], expressions[1] });
				} else 
				{
					throw new System.Exception("Expression cannot be converted into a number expression.");
				}
			}
		}

		private class StatementVisitor : TweedleParserBaseVisitor<TweedleStatement>
		{
			public override TweedleStatement VisitLocalVariableDeclaration([NotNull] TweedleParser.LocalVariableDeclarationContext context)
			{
                TweedleType type = GetTypeType(context.typeType());
                string name = context.variableDeclarator().variableDeclaratorId().IDENTIFIER().GetText();
                TweedleLocalVariable decl;
                if (context.variableDeclarator().variableInitializer() != null)
                {
                    ExpressionVisitor initVisitor = new ExpressionVisitor(type);
                    TweedleExpression init =
                                    context.variableDeclarator().variableInitializer().Accept(initVisitor);
                    decl = new TweedleLocalVariable(type, name, init);
                }
                else
                {
                    decl = new TweedleLocalVariable(type, name);
                }
                return new TweedleLocalVariableDeclaration(context.CONSTANT() != null, decl);
			}

			public override TweedleStatement VisitBlockStatement([NotNull] TweedleParser.BlockStatementContext context)
            {
                if (context.NODE_DISABLE() != null)
                {
                    TweedleStatement stmt = context.blockStatement().Accept(this);
                    stmt.Disable();
                    return stmt;
                }
                if (context.localVariableDeclaration() != null)
                {
                    return context.localVariableDeclaration().Accept(this);
                }
				return base.VisitBlockStatement(context);
            }

            public override TweedleStatement VisitStatement(TweedleParser.StatementContext context)
            {
                if (context.COUNT_UP_TO() != null)
                {
                    return new TweedleCountLoop(context.IDENTIFIER().GetText(),
                                           context.expression().Accept(new ExpressionVisitor(TweedleTypes.WHOLE_NUMBER)),
                                           CollectBlockStatements(context.block(0).blockStatement()));
                }
                if (context.IF() != null)
                {
                    TweedleExpression condition = context.parExpression().expression().Accept(new ExpressionVisitor(TweedleTypes.BOOLEAN));
                    List<TweedleStatement> thenBlock = CollectBlockStatements(context.block(0).blockStatement());
                    List<TweedleStatement> elseBlock = context.ELSE() != null
                                                              ? CollectBlockStatements(context.block(1).blockStatement())
                                                              : new List<TweedleStatement>();
                    return new TweedleConditionalStatement(condition, thenBlock, elseBlock);
                }
                if (context.forControl() != null)
                {
                    TweedleType valueType = GetTypeType(context.forControl().typeType());
                    TweedleArrayType arrayType = new TweedleArrayType(valueType);
                    TweedleLocalVariable loopVar =
                                    new TweedleLocalVariable(valueType, context.forControl().variableDeclaratorId().GetText());
                    TweedleExpression loopValues = context.forControl().expression().Accept(new ExpressionVisitor(arrayType));
                    List<TweedleStatement> statements = CollectBlockStatements(context.block(0).blockStatement());
                    if (context.FOR_EACH() != null)
                    {
                        return new TweedleForEachInArrayLoop(loopVar, loopValues, statements);
                    }
                    if (context.EACH_TOGETHER() != null)
                    {
                        return new TweedleEachInArrayTogether(loopVar, loopValues, statements);
                    }
                    throw new System.Exception("Found a forControl in a statement where it was not expected: " + context);
                }
                if (context.WHILE() != null)
                {
                    return new TweedleWhileLoop(context.parExpression().expression().Accept(new ExpressionVisitor(TweedleTypes.BOOLEAN)),
                                                CollectBlockStatements(context.block(0).blockStatement()));
                }
                if (context.DO_IN_ORDER() != null)
                {
                    return new TweedleDoInOrder(CollectBlockStatements(context.block(0).blockStatement()));
                }
                if (context.DO_TOGETHER() != null)
                {
                    return new TweedleDoTogether(CollectBlockStatements(context.block(0).blockStatement()));
                }
                if (context.RETURN() != null)
                {
                    if (context.expression() != null)
                    {
                        return new TweedleReturnStatement(context.expression().Accept(new ExpressionVisitor()));
                    }
                    else
                    {
                        return new TweedleReturnStatement();
                    }
                }
                TweedleParser.ExpressionContext expContext = context.statementExpression;
                if (expContext != null)
                {
                    return new TweedleExpressionStatement(context.expression().Accept(new ExpressionVisitor()));
                }
                throw new System.Exception("Found a statement that was not expected: " + context);
            }
        }

        private static List<TweedleStatement> CollectBlockStatements(TweedleParser.BlockStatementContext[] contexts)
        {
            StatementVisitor statementVisitor = new StatementVisitor();
            return contexts.Select(stmt => stmt.Accept(statementVisitor)).ToList();
        }

		private static TweedleType GetTypeOrVoid(TweedleParser.TypeTypeOrVoidContext context)
		{
			if (context.VOID() != null)
			{
				return TweedleVoidType.VOID;
			}
			return GetTypeType(context.typeType());
		}

		private static TweedleType GetTypeType(TweedleParser.TypeTypeContext context)
        {
			TweedleType baseType;
			if (context.classOrInterfaceType() != null)
			{
				baseType = GetTypeReference(context.classOrInterfaceType().GetText());
			} else
			{
				baseType = GetPrimitiveType(context.primitiveType().GetText());
			}
            if (context.ChildCount > 1 && baseType != null)
			{
				return new TweedleArrayType(baseType);
			}
			return baseType;
		}

		private static TweedleTypeReference GetTypeReference(string type)
		{
			return new TweedleTypeReference(type);
		}

		private static TweedlePrimitiveType GetPrimitiveType(string type)
		{
			foreach (TweedlePrimitiveType prim in TweedleTypes.PRIMITIVE_TYPES)
			{
				if (prim.Name == type)
				{
					return prim;
				}
			}
			return null;
		}
	}
}
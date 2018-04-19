using Antlr4.Runtime;
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

		public TweedleExpression ParseExpression(string src)
		{
			return new ExpressionVisitor(null).Visit(ParseSource(src).expression());
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
				TweedleTypeReference type = new TweedleTypeReference(context.typeType().GetText());
				VariableListVisitor variableVisitor = new VariableListVisitor(modifiers, type);
				List<TweedleField> properties = context.variableDeclarators().Accept(variableVisitor);
				tweClass.properties.AddRange(properties);
				return base.VisitFieldDeclaration(context);
			}

            public override object VisitMethodDeclaration([NotNull] TweedleParser.MethodDeclarationContext context)
			{
				StatementVisitor statementVisitor = new StatementVisitor();
				TweedleMethod method = new TweedleMethod(
						modifiers,
						GetTypeOrVoid(context.typeTypeOrVoid()),
						context.IDENTIFIER().GetText(),
						RequiredParameters(context.formalParameters()),
						OptionalParameters(context.formalParameters()),
						context.methodBody().block().blockStatement().Select(statement =>
							statement.Accept(statementVisitor)
						).ToList()
					);
				tweClass.methods.Add(method);
				return base.VisitMethodDeclaration(context);
			}

            public override object VisitConstructorDeclaration([NotNull] TweedleParser.ConstructorDeclarationContext context)
			{
				StatementVisitor statementVisitor = new StatementVisitor();
				TweedleConstructor constructor = new TweedleConstructor(
					GetTypeReference(context.IDENTIFIER().GetText()),
					context.IDENTIFIER().GetText(),
					RequiredParameters(context.formalParameters()),
					OptionalParameters(context.formalParameters()),
					context.constructorBody.blockStatement().Select(statement =>
							statement.Accept(statementVisitor)
						).ToList()
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

		private class VariableListVisitor : TweedleParserBaseVisitor<List<TweedleField>>
		{
			private List<string> modifiers;
			private TweedleType type;

			public VariableListVisitor(List<string> modifiers, TweedleType type)
			{
				this.modifiers = modifiers;
				this.type = type;
			}

			public override List<TweedleField> VisitVariableDeclarators([NotNull] TweedleParser.VariableDeclaratorsContext context)
			{
                ExpressionVisitor expressionVisitor = new ExpressionVisitor(type);
				return context.variableDeclarator()
						.Select(field => new TweedleField(
								modifiers,
								type,
								field.variableDeclaratorId().IDENTIFIER().GetText(),
								expressionVisitor.Visit(field)
						)).ToList();
			}
        }

        private class ExpressionVisitor : TweedleParserBaseVisitor<TweedleExpression>
        {
			private TweedleType expectedType;

			public ExpressionVisitor(TweedleType expectedType)
			{
				this.expectedType = expectedType;
			}

			public override TweedleExpression VisitExpression([NotNull] TweedleParser.ExpressionContext context)
			{
				TweedleExpression expression = ParseExpression(context);
				if (expectedType == null || expression == null ||
					expectedType.AcceptsType(expression.Type))
				{
					return expression;
				}
				return null;
			}

			public TweedleExpression ParseExpression([NotNull] TweedleParser.ExpressionContext context)
			{
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
							// return new MethodInvokation();
                            break;
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
						default :
                            throw new System.Exception("Binary operation not found.");
                    }

                }
                return base.VisitChildren(context); ;
			}

			/*public override TweedleExpression VisitArrayInitializer([NotNull] TweedleParser.ArrayInitializerContext context)
			{
                 UnityEngine.Debug.Log("array init");
                return new TweedleArrayInitializer(context.variableInitializer().Select(a => a.Accept(this)).ToList());
 			}*/

			public override TweedleExpression VisitPrimary([NotNull] TweedleParser.PrimaryContext context)
			{
				if (context.THIS() != null)
				{
					return new ThisExpression(null);
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
					return TweedleTypes.TEXT_STRING.Instantiate(value.Substring(1, value.Length - 1));
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
				List<string> modifiers = context.variableModifier().Select(modifier => modifier.GetText()).ToList();
				TweedleTypeReference type = new TweedleTypeReference(context.typeType().GetText());
				VariableListVisitor variableVisitor = new VariableListVisitor(modifiers, type);
				List<TweedleField> variables = context.variableDeclarators().Accept(variableVisitor);
				return new TweedleLocalVariableDeclaration(variables);
			}

			public override TweedleStatement VisitBlockStatement([NotNull] TweedleParser.BlockStatementContext context)
			{
				UnityEngine.Debug.Log("block statement");
				return base.VisitBlockStatement(context);
			}
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
			int bracketCount = context.ChildCount;
			TweedleType baseType;
			if (context.classOrInterfaceType() != null)
			{
				baseType = GetTypeReference(context.classOrInterfaceType().GetText());
			} else
			{
				baseType = GetPrimitiveType(context.primitiveType().GetText());
			}
			while (bracketCount > 1 && baseType != null)
			{
				baseType = new TweedleArrayType(baseType);
				bracketCount -= 2;
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
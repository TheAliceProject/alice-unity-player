using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System.Collections.Generic;
using System.Linq;

namespace Alice.Tweedle.Parsed
{
	public class TweedleParser
	{
		public Tweedle.TweedleParser ParseSource(string src)
		{
			AntlrInputStream antlerStream = new AntlrInputStream(src);
			TweedleLexer lexer = new TweedleLexer(antlerStream);
			CommonTokenStream tokenStream = new CommonTokenStream(lexer);
			return new Tweedle.TweedleParser(tokenStream);
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

		private class ClassBody
		{
			public List<TweedleField> properties;
			public List<TweedleMethod> methods;
			public List<TweedleConstructor> constructors;

			public ClassBody()
			{
				this.properties = new List<TweedleField>();
				this.methods = new List<TweedleMethod>();
				this.constructors = new List<TweedleConstructor>();
			}
		}

		private class TypeVisitor : TweedleParserBaseVisitor<TweedleType>
		{
			private ClassBody body;
			private ClassBodyDeclarationVisitor cbdVisitor;

			public override TweedleType VisitTypeDeclaration([NotNull] Tweedle.TweedleParser.TypeDeclarationContext context)
			{
				body = new ClassBody();
				cbdVisitor = new ClassBodyDeclarationVisitor(body);
				return base.VisitChildren(context);
			}

			public override TweedleType VisitClassDeclaration([NotNull] Tweedle.TweedleParser.ClassDeclarationContext context)
			{
				context.classBody().classBodyDeclaration().ToList().ForEach(cbd => cbd.Accept(cbdVisitor));
				string className = context.identifier().GetText();
				if (context.EXTENDS() != null)
				{
					string superclass = context.typeType().classType().GetText();
					return new TweedleClass(className, superclass,
						body.properties, body.methods, body.constructors);
				}
				else
				{
					return new TweedleClass(className,
						body.properties, body.methods, body.constructors);
				}
			}

			public override TweedleType VisitEnumDeclaration([NotNull] Tweedle.TweedleParser.EnumDeclarationContext context)
			{
				if (context.enumBodyDeclarations() != null)
				{
					context.enumBodyDeclarations().classBodyDeclaration().ToList().ForEach(cbd => cbd.Accept(cbdVisitor));
				}
				TweedleEnum tweedleEnum = new TweedleEnum(context.identifier().GetText(), body.properties, body.methods, body.constructors);
				context.enumConstants().enumConstant().ToList().ForEach(enumConst =>
				{
					string name = enumConst.identifier().GetText();
					Dictionary<string, TweedleExpression> arguments = new Dictionary<string, TweedleExpression>();
					if (enumConst.arguments() != null)
					{
						arguments = TweedleParser.VisitLabeledArguments(enumConst.arguments().labeledExpressionList());
					}
					tweedleEnum.AddEnumValue(new TweedleEnumValue(tweedleEnum, name, arguments));
				});
				return tweedleEnum;
			}
		}

		private class ClassBodyDeclarationVisitor : TweedleParserBaseVisitor<object>
		{
			private readonly ClassBody body;

			public ClassBodyDeclarationVisitor(ClassBody body)
			{
				this.body = body;
			}

			public override object VisitClassBodyDeclaration([NotNull] Tweedle.TweedleParser.ClassBodyDeclarationContext context)
			{
				Tweedle.TweedleParser.MemberDeclarationContext memberDec = context.memberDeclaration();
				if (memberDec != null)
				{
					List<string> modifiers =
						context.classModifier().Select(classMod => (classMod.STATIC() != null) ? "static" : null).ToList();
					memberDec.Accept(new MemberDeclarationVisitor(body, modifiers));
				}
				return base.VisitClassBodyDeclaration(context);
			}
		}

		private class MemberDeclarationVisitor : TweedleParserBaseVisitor<object>
		{
			private ClassBody body;
			private List<string> modifiers;

			public MemberDeclarationVisitor(ClassBody body, List<string> modifiers)
			{
				this.body = body;
				this.modifiers = modifiers;
			}

			public override object VisitFieldDeclaration([NotNull] Tweedle.TweedleParser.FieldDeclarationContext context)
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
				body.properties.Add(property);
				return base.VisitFieldDeclaration(context);
			}

			public override object VisitMethodDeclaration([NotNull] Tweedle.TweedleParser.MethodDeclarationContext context)
			{
				TweedleMethod method = new TweedleMethod(
						modifiers,
						GetTypeOrVoid(context.typeTypeOrVoid()),
						context.IDENTIFIER().GetText(),
						RequiredParameters(context.formalParameters()),
						OptionalParameters(context.formalParameters()),
						CollectBlockStatements(context.methodBody().block().blockStatement())
					);
				body.methods.Add(method);
				return base.VisitMethodDeclaration(context);
			}

			public override object VisitConstructorDeclaration([NotNull] Tweedle.TweedleParser.ConstructorDeclarationContext context)
			{
				TweedleConstructor constructor = new TweedleConstructor(
					GetTypeReference(context.IDENTIFIER().GetText()),
					context.IDENTIFIER().GetText(),
					RequiredParameters(context.formalParameters()),
					OptionalParameters(context.formalParameters()),
					CollectBlockStatements(context.constructorBody.blockStatement())
				);
				body.constructors.Add(constructor);
				return base.VisitConstructorDeclaration(context);
			}

			private List<TweedleRequiredParameter> RequiredParameters(Tweedle.TweedleParser.FormalParametersContext context)
			{
				if (context.formalParameterList() == null || context.formalParameterList().requiredParameter() == null)
				{
					return new List<TweedleRequiredParameter>();
				}

				return context.formalParameterList().requiredParameter().Select(field =>
				{
					TweedleType type = GetTypeType(field.typeType());
					return new TweedleRequiredParameter(
						type,
						field.variableDeclaratorId().IDENTIFIER().GetText()
						);
				}).ToList();
			}

			private List<TweedleOptionalParameter> OptionalParameters(Tweedle.TweedleParser.FormalParametersContext context)
			{
				if (context.formalParameterList() == null || context.formalParameterList().optionalParameter() == null)
				{
					return new List<TweedleOptionalParameter>();
				}

				return context.formalParameterList().optionalParameter().Select(field =>
				{
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

			public override TweedleExpression VisitExpression([NotNull] Tweedle.TweedleParser.ExpressionContext context)
			{
				TweedleExpression expression = ParseExpression(context);

				if (expectedType != null && expression != null && expression.Type != null
					&& !expectedType.AcceptsType(expression.Type) && !expression.Type.AcceptsType(expectedType))
				{
					throw new System.Exception("Had been expecting expression of type " + expectedType.Name + ", but it is typed as " + expression.Type.Name);
				}
				return expression;
			}

			public TweedleExpression ParseExpression([NotNull] Tweedle.TweedleParser.ExpressionContext context)
			{
				IToken prefix = context.prefix;
				IToken operation = context.bop;
				IToken bracket = context.bracket;

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
					switch (operation.Text)
					{
						case ".":
							return FieldOrMethodRef(context);
						case "..":
							return NewBinaryExpression<StringConcatenationExpression>(context, null);
						case "*":
							return NewBinaryExpression<MultiplicationExpression>(context, TweedleTypes.NUMBER);
						case "/":
							return NewBinaryExpression<DivisionExpression>(context, TweedleTypes.NUMBER);
						case "%":
							expressions = TypedExpression(context, TweedleTypes.WHOLE_NUMBER);
							return new ModuloExpression(expressions[0], expressions[1]);
						case "+":
							return NewBinaryExpression<AdditionExpression>(context, TweedleTypes.NUMBER);
						case "-":
							return NewBinaryExpression<SubtractionExpression>(context, TweedleTypes.NUMBER);
						case "<=":
							return NewBinaryExpression<LessThanOrEqualExpression>(context, TweedleTypes.NUMBER);
						case ">=":
							return NewBinaryExpression<GreaterThanOrEqualExpression>(context, TweedleTypes.NUMBER);
						case ">":
							return NewBinaryExpression<GreaterThanExpression>(context, TweedleTypes.NUMBER);
						case "<":
							return NewBinaryExpression<LessThanExpression>(context, TweedleTypes.NUMBER);
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
						default:
							throw new System.Exception("Binary operation not found.");
					}
				}
				else if (bracket != null)
				{
					TweedleExpression array = context.expression(0).Accept(new ExpressionVisitor(new TweedleArrayType()));
					TweedleExpression index = context.expression(1).Accept(new ExpressionVisitor(TweedleTypes.WHOLE_NUMBER));
					return new ArrayIndexExpression(array, index);
				}
				else if (context.lambdaCall() != null)
				{
					TweedleExpression lambdaSourceExp = TypedExpression(context, null)[0];
					if (context.lambdaCall().unlabeledExpressionList() == null)
					{
						return new LambdaEvaluation(lambdaSourceExp);
					}
					else
					{
						List<TweedleExpression> elements =
										VisitUnlabeledArguments(context.lambdaCall().unlabeledExpressionList(), new ExpressionVisitor());
						return new LambdaEvaluation(lambdaSourceExp, elements);
					}
				}
				return base.VisitChildren(context);
			}

			public override TweedleExpression VisitPrimary([NotNull] Tweedle.TweedleParser.PrimaryContext context)
			{
				if (context.expression() != null)
				{
					// Parenthesized child expression
					return this.VisitExpression(context.expression());
				}
				if (context.THIS() != null)
				{
					return new ThisExpression();
				}
				if (context.IDENTIFIER() != null)
				{
					return new IdentifierReference(context.IDENTIFIER().GetText());
				}

				return base.VisitChildren(context);
			}

			public override TweedleExpression VisitLiteral([NotNull] Tweedle.TweedleParser.LiteralContext context)
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

			public override TweedleExpression VisitSuperSuffix([NotNull] Tweedle.TweedleParser.SuperSuffixContext context)
			{
				if (context.IDENTIFIER() != null)
				{
					if (context.arguments() != null)
					{
						return MethodCallExpression.Super(context.IDENTIFIER().GetText(), TweedleParser.VisitLabeledArguments(context.arguments().labeledExpressionList()));
					}
					else
					{
						return FieldAccess.Super(context.IDENTIFIER().GetText());
					}
				}
				else if (context.arguments() != null)
				{
					return new SuperInstantiation(TweedleParser.VisitLabeledArguments(context.arguments().labeledExpressionList()));
				}
				throw new System.Exception("Super suffix could not be constructed."); ;
			}

			public override TweedleExpression VisitLambdaExpression([NotNull] Tweedle.TweedleParser.LambdaExpressionContext context)
			{
				List<TweedleRequiredParameter> parameters = context.lambdaParameters().requiredParameter()
					.Select(field => new TweedleRequiredParameter(
						GetTypeType(field.typeType()),
						field.variableDeclaratorId().IDENTIFIER().GetText()
					)).ToList();
				List<TweedleStatement> statements = CollectBlockStatements(context.block().blockStatement());
				return new LambdaExpression(parameters, statements);
			}

			public override TweedleExpression VisitMethodCall([NotNull] Tweedle.TweedleParser.MethodCallContext context)
			{
				return new MethodCallExpression(context.IDENTIFIER().GetText(),
												TweedleParser.VisitLabeledArguments(context.labeledExpressionList()));
			}

			public override TweedleExpression VisitCreator([NotNull] Tweedle.TweedleParser.CreatorContext context)
			{
				string typeName = context.createdName().GetText();
				Tweedle.TweedleParser.ArrayCreatorRestContext arrayCreator = context.arrayCreatorRest();
				if (arrayCreator != null)
				{
					TweedleType memberType = GetPrimitiveType(typeName);
					memberType = memberType ?? GetTypeReference(typeName);
					TweedleArrayType arrayMemberType = new TweedleArrayType(memberType);
					if (arrayCreator.arrayInitializer() != null)
					{
						List<TweedleExpression> elements = new List<TweedleExpression>();
						if (arrayCreator.arrayInitializer().unlabeledExpressionList() != null)
						{
							elements = arrayCreator.arrayInitializer().unlabeledExpressionList().expression().Select(a => a.Accept(new ExpressionVisitor(memberType))).ToList();
						}
						return new ArrayInitializer(
										arrayMemberType,
										elements);
					}
					else
					{
						return new ArrayInitializer(
							arrayMemberType,
							arrayCreator.expression().Accept(new ExpressionVisitor())
							);
					}
				}
				else
				{
					TweedleTypeReference typeRef = GetTypeReference(typeName);
					Tweedle.TweedleParser.LabeledExpressionListContext argsContext = context.classCreatorRest().arguments().labeledExpressionList();
					Dictionary<string, TweedleExpression> arguments = TweedleParser.VisitLabeledArguments(argsContext);
					return new Instantiation(
							typeRef,
							arguments
						);
				}
			}

			private TweedleExpression FieldOrMethodRef(Tweedle.TweedleParser.ExpressionContext context)
			{
				// Use untyped expression visitor for target
				TweedleExpression target = context.expression(0).Accept(new ExpressionVisitor());
				if (context.IDENTIFIER() != null)
				{
					return new FieldAccess(target, context.IDENTIFIER().GetText());
				}
				if (context.methodCall() != null)
				{
					Tweedle.TweedleParser.LabeledExpressionListContext argsContext = context.methodCall().labeledExpressionList();
					Dictionary<string, TweedleExpression> arguments = new Dictionary<string, TweedleExpression>();
					if (argsContext != null)
					{
						arguments = TweedleParser.VisitLabeledArguments(argsContext);
					}
					return new MethodCallExpression(target,
						context.methodCall().IDENTIFIER().GetText(),
						arguments
						);
				}
				throw new System.Exception("Unexpected details on context " + context);
			}

			private List<TweedleExpression> TypedExpression(Tweedle.TweedleParser.ExpressionContext context, TweedleType type)
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
				}
				else if (exp.Type == TweedleTypes.WHOLE_NUMBER)
				{
					negativeExp = new NegativeWholeExpression(exp);
				}
				else
				{
					throw new System.Exception("Expression cannot be converted into a negative expression.");
				}
				if (exp.IsLiteral())
				{
					return negativeExp.EvaluateNow();
				}
				return negativeExp;
			}

			private TweedleExpression NewBinaryExpression<T>(Tweedle.TweedleParser.ExpressionContext context, TweedlePrimitiveType childType) where T : BinaryExpression
			{
				List<TweedleExpression> expressions = TypedExpression(context, childType);
				return (TweedleExpression)System.Activator.CreateInstance(typeof(T), new object[] { expressions[0], expressions[1] });
			}
		}

		private class StatementVisitor : TweedleParserBaseVisitor<TweedleStatement>
		{
			public override TweedleStatement VisitLocalVariableDeclaration([NotNull] Tweedle.TweedleParser.LocalVariableDeclarationContext context)
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
				return new LocalVariableDeclaration(context.CONSTANT() != null, decl);
			}

			public override TweedleStatement VisitBlockStatement([NotNull] Tweedle.TweedleParser.BlockStatementContext context)
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

			public override TweedleStatement VisitStatement(Tweedle.TweedleParser.StatementContext context)
			{
				if (context.COUNT_UP_TO() != null)
				{
					return new CountLoop(context.IDENTIFIER().GetText(),
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
					return new ConditionalStatement(condition, thenBlock, elseBlock);
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
						return new ForEachInArrayLoop(loopVar, loopValues, statements);
					}
					if (context.EACH_TOGETHER() != null)
					{
						return new EachInArrayTogether(loopVar, loopValues, statements);
					}
					throw new System.Exception("Found a forControl in a statement where it was not expected: " + context);
				}
				if (context.WHILE() != null)
				{
					return new WhileLoop(context.parExpression().expression().Accept(new ExpressionVisitor(TweedleTypes.BOOLEAN)),
												CollectBlockStatements(context.block(0).blockStatement()));
				}
				if (context.DO_IN_ORDER() != null)
				{
					return new DoInOrder(CollectBlockStatements(context.block(0).blockStatement()));
				}
				if (context.DO_TOGETHER() != null)
				{
					return new DoTogether(CollectBlockStatements(context.block(0).blockStatement()));
				}
				if (context.RETURN() != null)
				{
					if (context.expression() != null)
					{
						return new ReturnStatement(context.expression().Accept(new ExpressionVisitor()));
					}
					else
					{
						return new ReturnStatement();
					}
				}
				Tweedle.TweedleParser.ExpressionContext expContext = context.statementExpression;
				if (expContext != null)
				{
					return new ExpressionStatement(context.expression().Accept(new ExpressionVisitor()));
				}
				throw new System.Exception("Found a statement that was not expected: " + context);
			}
		}

		private static List<TweedleStatement> CollectBlockStatements(Tweedle.TweedleParser.BlockStatementContext[] contexts)
		{
			StatementVisitor statementVisitor = new StatementVisitor();
			return contexts.Select(stmt => stmt.Accept(statementVisitor)).ToList();
		}

		private static Dictionary<string, TweedleExpression> VisitLabeledArguments(Tweedle.TweedleParser.LabeledExpressionListContext context)
		{
			Dictionary<string, TweedleExpression> arguments = new Dictionary<string, TweedleExpression>();
			if (context != null)
			{
				context.labeledExpression().ToList().ForEach(arg =>
				{
					TweedleExpression argValue = arg.expression().Accept(new ExpressionVisitor());
					arguments.Add(arg.IDENTIFIER().GetText(), argValue);
				});
			}
			return arguments;
		}

		private static List<TweedleExpression> VisitUnlabeledArguments(Tweedle.TweedleParser.UnlabeledExpressionListContext listContext,
																	   ExpressionVisitor expressionVisitor)
		{
			return listContext == null
					? new List<TweedleExpression>()
					: listContext.expression().Select(exp => exp.Accept(expressionVisitor)).ToList();
		}

		private static TweedleType GetTypeOrVoid(Tweedle.TweedleParser.TypeTypeOrVoidContext context)
		{
			if (context.VOID() != null)
			{
				return TweedleVoidType.VOID;
			}
			return GetTypeType(context.typeType());
		}

		private static TweedleType GetTypeType(Tweedle.TweedleParser.TypeTypeContext context)
		{
			if (context.lambdaTypeSignature() != null)
			{
				return GetLambdaType(context.lambdaTypeSignature());
			}
			else
			{
				TweedleType baseType = null;
				if (context.classType() != null)
				{
					baseType = GetTypeReference(context.classType().GetText());
				}
				else if (context.primitiveType() != null)
				{
					baseType = GetPrimitiveType(context.primitiveType().GetText());
				}
				if (context.ChildCount > 1 && baseType != null)
				{
					return new TweedleArrayType(baseType);
				}
				return baseType;
			}
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

		private static TweedleLambdaType GetLambdaType([NotNull] Tweedle.TweedleParser.LambdaTypeSignatureContext context)
		{
			List<TweedleType> typeList = new List<TweedleType>();
			if (context.typeList().typeType() != null)
			{
				context.typeList().typeType().Select(type => GetTypeType(type)).ToList();
			}
			return new TweedleLambdaType(typeList, GetTypeOrVoid(context.typeTypeOrVoid()));
		}
	}
}
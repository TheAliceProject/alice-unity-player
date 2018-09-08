using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alice.Tweedle.Parse
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

		public TType ParseType(string src)
		{
			return new TypeVisitor().Visit(ParseSource(src).typeDeclaration());
		}

		public TweedleStatement ParseStatement(string src)
		{
			return new StatementVisitor().Visit(ParseSource(src).blockStatement());
		}

		public ITweedleExpression ParseExpression(string src)
		{
			return new ExpressionVisitor().Visit(ParseSource(src).expression());
		}

		private class ClassBody
		{
			public List<TField> properties;
			public List<TMethod> methods;
			public List<TMethod> constructors;

			public ClassBody()
			{
				this.properties = new List<TField>();
				this.methods = new List<TMethod>();
				this.constructors = new List<TMethod>();
			}
		}

		private class TypeVisitor : TweedleParserBaseVisitor<TType>
		{
			private ClassBody body;
			private ClassBodyDeclarationVisitor cbdVisitor;

			public override TType VisitTypeDeclaration([NotNull] Tweedle.TweedleParser.TypeDeclarationContext context)
			{
				body = new ClassBody();
				cbdVisitor = new ClassBodyDeclarationVisitor(body);
				return base.VisitChildren(context);
			}

			public override TType VisitClassDeclaration([NotNull] Tweedle.TweedleParser.ClassDeclarationContext context)
			{
				context.classBody().classBodyDeclaration().ToList().ForEach(cbd => cbd.Accept(cbdVisitor));
				string className = context.identifier().GetText();
                if (context.EXTENDS() != null)
                {
                	string superclass = context.typeType().classType().GetText();
                	return new TClassType(className, superclass,
                		body.properties.ToArray(), body.methods.ToArray(), body.constructors.ToArray());
                }
                else
                {
                	return new TClassType(className,
						body.properties.ToArray(), body.methods.ToArray(), body.constructors.ToArray());
                }
            }

			public override TType VisitEnumDeclaration([NotNull] Tweedle.TweedleParser.EnumDeclarationContext context)
			{
                // TODO(Alex): Implement
                // if (context.enumBodyDeclarations() != null)
                // {
                // 	context.enumBodyDeclarations().classBodyDeclaration().ToList().ForEach(cbd => cbd.Accept(cbdVisitor));
                // }
                // TweedleEnum tweedleEnum = new TweedleEnum(context.identifier().GetText(), body.properties, body.methods, body.constructors);
                // context.enumConstants().enumConstant().ToList().ForEach(enumConst =>
                // {
                // 	string name = enumConst.identifier().GetText();
                // 	NamedArgument[] arguments = new NamedArgument[0];
                // 	if (enumConst.arguments() != null)
                // 	{
                // 		arguments = TweedleParser.VisitLabeledArguments(enumConst.arguments().labeledExpressionList());
                // 	}
                // 	tweedleEnum.AddEnumValue(new TweedleEnumValue(tweedleEnum, name, arguments));
                // });
                // return tweedleEnum;
                return null;
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
					memberDec.Accept(new MemberDeclarationVisitor(body, GetModifiers(modifiers)));
				}
				return base.VisitClassBodyDeclaration(context);
			}
		}

		private class MemberDeclarationVisitor : TweedleParserBaseVisitor<ITypeMember>
		{
			private ClassBody body;
			private MemberFlags modifiers;

			public MemberDeclarationVisitor(ClassBody body, MemberFlags modifiers)
			{
				this.body = body;
				this.modifiers = modifiers;
			}

			public override ITypeMember VisitFieldDeclaration([NotNull] Tweedle.TweedleParser.FieldDeclarationContext context)
			{
				TTypeRef type = GetTypeType(context.typeType());
				string name = context.variableDeclarator().variableDeclaratorId().IDENTIFIER().GetText();
				TField property;
				if (context.variableDeclarator().variableInitializer() != null)
				{
					ExpressionVisitor initVisitor = new ExpressionVisitor(type);
					ITweedleExpression init =
									context.variableDeclarator().variableInitializer().Accept(initVisitor);

					if ((modifiers & MemberFlags.Static) == MemberFlags.Static)
                        property = new TStaticField(name, type, modifiers, init);
					else
                    	property = new TObjectField(name, type, modifiers, init);
				}
				else
				{
					if ((modifiers & MemberFlags.Static) == MemberFlags.Static)
                        property = new TStaticField(name, type, modifiers);
					else
                    	property = new TObjectField(name, type, modifiers);
				}
				body.properties.Add(property);
				return base.VisitFieldDeclaration(context);
			}

			public override ITypeMember VisitMethodDeclaration([NotNull] Tweedle.TweedleParser.MethodDeclarationContext context)
			{
				TTMethod method = new TTMethod(
						context.IDENTIFIER().GetText(), 
						modifiers,
						GetTypeOrVoid(context.typeTypeOrVoid()),
						RequiredParameters(context.formalParameters()),
						OptionalParameters(context.formalParameters()),
						CollectBlockStatements(context.methodBody().block().blockStatement())
					);
				body.methods.Add(method);
				return base.VisitMethodDeclaration(context);
			}

			public override ITypeMember VisitConstructorDeclaration([NotNull] Tweedle.TweedleParser.ConstructorDeclarationContext context)
			{
				TTMethod method = new TTMethod(
						TMethod.ConstructorName,
						modifiers | MemberFlags.Constructor,
						GetTypeRef(context.IDENTIFIER().GetText()),
						RequiredParameters(context.formalParameters()),
						OptionalParameters(context.formalParameters()),
						CollectBlockStatements(context.constructorBody.blockStatement())
					);
				body.constructors.Add(method);
				return base.VisitConstructorDeclaration(context);
			}

			private TParameter[] RequiredParameters(Tweedle.TweedleParser.FormalParametersContext context)
			{
				if (context.formalParameterList() == null || context.formalParameterList().requiredParameter() == null)
				{
					return TParameter.EMPTY_PARAMS;
				}

				return context.formalParameterList().requiredParameter().Select(field =>
				{
					TTypeRef type = GetTypeType(field.typeType());
					return TParameter.RequiredParameter(
						type,
						field.variableDeclaratorId().IDENTIFIER().GetText()
						);
				}).ToArray();
			}

			private TParameter[] OptionalParameters(Tweedle.TweedleParser.FormalParametersContext context)
			{
				if (context.formalParameterList() == null || context.formalParameterList().optionalParameter() == null)
				{
					return TParameter.EMPTY_PARAMS;
				}

				return context.formalParameterList().optionalParameter().Select(field =>
				{
					TTypeRef type = GetTypeType(field.typeType());
					return TParameter.OptionalParameter(
						type,
						field.variableDeclaratorId().IDENTIFIER().GetText(),
						field.Accept(new ExpressionVisitor(type))
						);
				}).ToArray();
			}
		}

		private class ExpressionVisitor : TweedleParserBaseVisitor<ITweedleExpression>
		{
			private TTypeRef expectedType;

			public ExpressionVisitor(TTypeRef expectedType)
			{
				this.expectedType = expectedType;
			}

			public ExpressionVisitor()
			{
				this.expectedType = null;
			}

			public override ITweedleExpression VisitExpression([NotNull] Tweedle.TweedleParser.ExpressionContext context)
			{
				ITweedleExpression expression = ParseExpression(context);

				if (expectedType != null && expression != null && expression.Type != null
					&& expectedType.IsResolved() && expression.Type.IsResolved()
					&& !expression.Type.Get().CanCast(expectedType))
				{
					throw new System.Exception("Had been expecting expression of type " + expectedType.Name + ", but it is typed as " + expression.Type.Name);
				}
				return expression;
			}

			public ITweedleExpression ParseExpression([NotNull] Tweedle.TweedleParser.ExpressionContext context)
			{
				IToken prefix = context.prefix;
				IToken operation = context.bop;
				IToken bracket = context.bracket;

				if (prefix != null)
				{
					switch (prefix.Text)
					{
						case "+":
							return TypedExpression(context, TStaticTypes.NUMBER)[0];
						case "-":

							return NegativeExpression(TypedExpression(context, TStaticTypes.NUMBER)[0]);
						case "!":
							return new LogicalNotExpression(TypedExpression(context, TStaticTypes.BOOLEAN)[0]);
						default:
							throw new System.Exception("Expression has prefix name but cannot be converted to prefix expression.");
					}
				}
				else if (operation != null)
				{
					ITweedleExpression[] expressions;
					switch (operation.Text)
					{
						case ".":
							return FieldOrMethodRef(context);
						case "..":
							return NewBinaryExpression<StringConcatenationExpression>(context, null);
						case "*":
							return NewBinaryExpression<MultiplicationExpression>(context, TStaticTypes.NUMBER);
						case "/":
							return NewBinaryExpression<DivisionExpression>(context, TStaticTypes.NUMBER);
						case "%":
							expressions = TypedExpression(context, TStaticTypes.WHOLE_NUMBER);
							return new ModuloExpression(expressions[0], expressions[1]);
						case "+":
							return NewBinaryExpression<AdditionExpression>(context, TStaticTypes.NUMBER);
						case "-":
							return NewBinaryExpression<SubtractionExpression>(context, TStaticTypes.NUMBER);
						case "<=":
							return NewBinaryExpression<LessThanOrEqualExpression>(context, TStaticTypes.NUMBER);
						case ">=":
							return NewBinaryExpression<GreaterThanOrEqualExpression>(context, TStaticTypes.NUMBER);
						case ">":
							return NewBinaryExpression<GreaterThanExpression>(context, TStaticTypes.NUMBER);
						case "<":
							return NewBinaryExpression<LessThanExpression>(context, TStaticTypes.NUMBER);
						case "==":
							expressions = TypedExpression(context, null);
							return new EqualToExpression(expressions[0], expressions[1]);
						case "!=":
							expressions = TypedExpression(context, null);
							return new NotEqualToExpression(expressions[0], expressions[1]);
						case "&&":
							expressions = TypedExpression(context, TStaticTypes.BOOLEAN);
							return new LogicalAndExpression(expressions[0], expressions[1]);
						case "||":
							expressions = TypedExpression(context, TStaticTypes.BOOLEAN);
							return new LogicalOrExpression(expressions[0], expressions[1]);
						case "<-":
							expressions = TypedExpression(context, null);
							return new AssignmentExpression(expressions[0], expressions[1]);
						default:
							throw new System.Exception("Binary operation not found.");
					}
				}
				// else if (bracket != null)
				// {
				// 	ITweedleExpression array = context.expression(0).Accept(new ExpressionVisitor(new TweedleArrayType()));
				// 	ITweedleExpression index = context.expression(1).Accept(new ExpressionVisitor(TStaticTypes.WHOLE_NUMBER));
				// 	return new ArrayIndexExpression(array, index);
				// }
				// else if (context.lambdaCall() != null)
				// {
				// 	ITweedleExpression lambdaSourceExp = TypedExpression(context, null)[0];
				// 	if (context.lambdaCall().unlabeledExpressionList() == null)
				// 	{
				// 		return new LambdaEvaluation(lambdaSourceExp);
				// 	}
				// 	else
				// 	{
				// 		List<ITweedleExpression> elements =
				// 						VisitUnlabeledArguments(context.lambdaCall().unlabeledExpressionList(), new ExpressionVisitor());
				// 		return new LambdaEvaluation(lambdaSourceExp, elements);
				// 	}
				// }
				return base.VisitChildren(context);
			}

			public override ITweedleExpression VisitPrimary([NotNull] Tweedle.TweedleParser.PrimaryContext context)
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

			public override ITweedleExpression VisitLiteral([NotNull] Tweedle.TweedleParser.LiteralContext context)
			{
				if (context.DECIMAL_LITERAL() != null)
				{
					int value = System.Convert.ToInt32(context.DECIMAL_LITERAL().GetText());
					return TStaticTypes.WHOLE_NUMBER.Instantiate(value);
				}

				if (context.FLOAT_LITERAL() != null)
				{
					double value = System.Convert.ToDouble(context.FLOAT_LITERAL().GetText());
					return TStaticTypes.DECIMAL_NUMBER.Instantiate(value);
				}

				if (context.NULL_LITERAL() != null)
				{
					return TValue.NULL;
				}

				if (context.BOOL_LITERAL() != null)
				{
					bool value = System.Convert.ToBoolean(context.BOOL_LITERAL().GetText());
					return TStaticTypes.BOOLEAN.Instantiate(value);
				}

				if (context.STRING_LITERAL() != null)
				{
					string value = context.STRING_LITERAL().GetText();
					return TStaticTypes.TEXT_STRING.Instantiate(value.Substring(1, value.Length - 2));
				}

				throw new System.Exception("Literal couldn't be found in grammar.");
			}

			public override ITweedleExpression VisitSuperSuffix([NotNull] Tweedle.TweedleParser.SuperSuffixContext context)
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
                return null;
            }

			public override ITweedleExpression VisitLambdaExpression([NotNull] Tweedle.TweedleParser.LambdaExpressionContext context)
			{
                // List<TParameter> parameters = context.lambdaParameters().requiredParameter()
                // 	.Select(field => new TParameter(
                // 		GetTypeType(field.typeType()),
                // 		field.variableDeclaratorId().IDENTIFIER().GetText()
                // 	)).ToList();
                // TweedleStatement[] statements = CollectBlockStatements(context.block().blockStatement());
                // return new LambdaExpression(parameters, statements);
                return null;
            }

			public override ITweedleExpression VisitMethodCall([NotNull] Tweedle.TweedleParser.MethodCallContext context)
			{
				return new MethodCallExpression(context.IDENTIFIER().GetText(),
												TweedleParser.VisitLabeledArguments(context.labeledExpressionList()));
			}

			public override ITweedleExpression VisitCreator([NotNull] Tweedle.TweedleParser.CreatorContext context)
			{
				string typeName = context.createdName().GetText();
				Tweedle.TweedleParser.ArrayCreatorRestContext arrayCreator = context.arrayCreatorRest();
                if (arrayCreator != null)
                {
                    // TODO(Alex): Enable
                    // TType memberType = GetPrimitiveType(typeName);
                    // memberType = memberType ?? GetTypeRef(typeName);
                    // TweedleArrayType arrayMemberType = new TweedleArrayType(memberType);
                    // if (arrayCreator.arrayInitializer() != null)
                    // {
                    // 	List<ITweedleExpression> elements = new List<ITweedleExpression>();
                    // 	if (arrayCreator.arrayInitializer().unlabeledExpressionList() != null)
                    // 	{
                    // 		elements = arrayCreator.arrayInitializer().unlabeledExpressionList().expression().Select(a => a.Accept(new ExpressionVisitor(memberType))).ToList();
                    // 	}
                    // 	return new ArrayInitializer(
                    // 					arrayMemberType,
                    // 					elements);
                    // }
                    // else
                    // {
                    // 	return new ArrayInitializer(
                    // 		arrayMemberType,
                    // 		arrayCreator.expression().Accept(new ExpressionVisitor())
                    // 		);
                    // }
                    return null;
                }
                else
                {
                	TTypeRef typeRef = GetTypeRef(typeName);
                	Tweedle.TweedleParser.LabeledExpressionListContext argsContext = context.classCreatorRest().arguments().labeledExpressionList();
                	NamedArgument[] arguments = TweedleParser.VisitLabeledArguments(argsContext);
                	return new Instantiation(
                			typeRef,
                			arguments
                		);
                }
                return null;
            }

			private ITweedleExpression FieldOrMethodRef(Tweedle.TweedleParser.ExpressionContext context)
			{
				// Use untyped expression visitor for target
				ITweedleExpression target = context.expression(0).Accept(new ExpressionVisitor());
				if (context.IDENTIFIER() != null)
				{
					return new FieldAccess(target, context.IDENTIFIER().GetText());
				}
				if (context.methodCall() != null)
				{
					Tweedle.TweedleParser.LabeledExpressionListContext argsContext = context.methodCall().labeledExpressionList();
					NamedArgument[] arguments = new NamedArgument[0];
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

			private ITweedleExpression[] TypedExpression(Tweedle.TweedleParser.ExpressionContext context, TType type)
			{
				ExpressionVisitor expVisitor = new ExpressionVisitor(type);
				return context.expression().Select(exp => exp.Accept(expVisitor)).ToArray();
			}

			private ITweedleExpression NegativeExpression(ITweedleExpression exp)
			{
				NegativeExpression negativeExp;
				if (exp.Type == TStaticTypes.DECIMAL_NUMBER)
				{
					negativeExp = new NegativeDecimalExpression(exp);
				}
				else if (exp.Type == TStaticTypes.WHOLE_NUMBER)
				{
					negativeExp = new NegativeWholeExpression(exp);
				}
				else
				{
					throw new System.Exception("Expression cannot be converted into a negative expression.");
				}
				if (exp is TValue)
				{
                    return negativeExp.EvaluateLiteral();
				}
				return negativeExp;
			}

			private ITweedleExpression NewBinaryExpression<T>(Tweedle.TweedleParser.ExpressionContext context, TType childType) where T : BinaryExpression
			{
				ITweedleExpression[] expressions = TypedExpression(context, childType);
                BinaryExpression expression = (BinaryExpression)System.Activator.CreateInstance(typeof(T), new object[] { expressions[0], expressions[1] });
                
				// TODO(Alex): Optimization - if expressions 0 and 1 are both literal, evaluate now and return result?
				// ex. 3 + 4 should just return a value of 7 instead of a addition expression
				// if (expressions[0] is TValue && expressions[1] is TValue)
                // {
				// 	return expression.EvaluateLiteral();
                // }
                return expression;
            }
		}

		private class StatementVisitor : TweedleParserBaseVisitor<TweedleStatement>
		{
			public override TweedleStatement VisitLocalVariableDeclaration([NotNull] Tweedle.TweedleParser.LocalVariableDeclarationContext context)
			{
				TTypeRef type = GetTypeType(context.typeType());
				string name = context.variableDeclarator().variableDeclaratorId().IDENTIFIER().GetText();
				TLocalVariable decl;
				if (context.variableDeclarator().variableInitializer() != null)
				{
					ExpressionVisitor initVisitor = new ExpressionVisitor(type);
					ITweedleExpression init =
									context.variableDeclarator().variableInitializer().Accept(initVisitor);
					decl = new TLocalVariable(type, name, init);
				}
				else
				{
					decl = new TLocalVariable(type, name);
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
										   context.expression().Accept(new ExpressionVisitor(TStaticTypes.WHOLE_NUMBER)),
										   CollectBlockStatements(context.block(0).blockStatement()));
				}
				if (context.IF() != null)
				{
					ITweedleExpression condition = context.parExpression().expression().Accept(new ExpressionVisitor(TStaticTypes.BOOLEAN));
					TweedleStatement[] thenBlock = CollectBlockStatements(context.block(0).blockStatement());
					TweedleStatement[] elseBlock = context.ELSE() != null
															  ? CollectBlockStatements(context.block(1).blockStatement())
															  : new TweedleStatement[0];
					return new ConditionalStatement(condition, thenBlock, elseBlock);
				}
				// if (context.forControl() != null)
				// {
				// 	TType valueType = GetTypeType(context.forControl().typeType());
				// 	TweedleArrayType arrayType = new TweedleArrayType(valueType);
				// 	TLocalVariable loopVar =
				// 					new TLocalVariable(valueType, context.forControl().variableDeclaratorId().GetText());
				// 	ITweedleExpression loopValues = context.forControl().expression().Accept(new ExpressionVisitor(arrayType));
				// 	TweedleStatement[] statements = CollectBlockStatements(context.block(0).blockStatement());
				// 	if (context.FOR_EACH() != null)
				// 	{
				// 		return new ForEachInArrayLoop(loopVar, loopValues, statements);
				// 	}
				// 	if (context.EACH_TOGETHER() != null)
				// 	{
				// 		return new EachInArrayTogether(loopVar, loopValues, statements);
				// 	}
				// 	throw new System.Exception("Found a forControl in a statement where it was not expected: " + context);
				// }
				if (context.WHILE() != null)
				{
					return new WhileLoop(context.parExpression().expression().Accept(new ExpressionVisitor(TStaticTypes.BOOLEAN)),
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

		private static TweedleStatement[] CollectBlockStatements(Tweedle.TweedleParser.BlockStatementContext[] contexts)
		{
			StatementVisitor statementVisitor = new StatementVisitor();
			return contexts.Select(stmt => stmt.Accept(statementVisitor)).ToArray();
		}

		private static NamedArgument[] VisitLabeledArguments(Tweedle.TweedleParser.LabeledExpressionListContext context)
		{
			NamedArgument[] arguments;
			if (context != null)
			{
                var args = context.labeledExpression().ToArray();
                arguments = new NamedArgument[args.Length];
                int i = 0;
                Array.ForEach(args, (arg =>
				{
					ITweedleExpression argValue = arg.expression().Accept(new ExpressionVisitor());
					arguments[i++] = new NamedArgument(arg.IDENTIFIER().GetText(), argValue);
				}));
			}
			else
			{
                arguments = NamedArgument.EMPTY_ARGS;
            }
			return arguments;
		}

		private static List<ITweedleExpression> VisitUnlabeledArguments(Tweedle.TweedleParser.UnlabeledExpressionListContext listContext,
																	   ExpressionVisitor expressionVisitor)
		{
			return listContext == null
					? new List<ITweedleExpression>()
					: listContext.expression().Select(exp => exp.Accept(expressionVisitor)).ToList();
		}

		private static TTypeRef GetTypeOrVoid(Tweedle.TweedleParser.TypeTypeOrVoidContext context)
		{
			if (context.VOID() != null)
			{
				return TStaticTypes.VOID;
			}
			return GetTypeType(context.typeType());
		}

		private static TTypeRef GetTypeType(Tweedle.TweedleParser.TypeTypeContext context)
		{
			if (context.lambdaTypeSignature() != null)
			{
                // return GetLambdaType(context.lambdaTypeSignature());
                return null;
            }
			else
			{
				TTypeRef baseType = null;
				if (context.classType() != null)
				{
					baseType = GetTypeRef(context.classType().GetText());
				}
				else if (context.primitiveType() != null)
				{
					baseType = GetPrimitiveType(context.primitiveType().GetText());
				}
				// if (context.ChildCount > 1 && baseType != null)
				// {
				// 	return new TweedleArrayType(baseType);
				// }
				return baseType;
			}
		}

		private static TTypeRef GetTypeRef(string type)
		{
			return new TTypeRef(type);
		}

		private static TType GetPrimitiveType(string type)
		{
			foreach (TType prim in TStaticTypes.ALL_PRIMITIVE_TYPES)
			{
				if (prim.Name == type)
				{
					return prim;
				}
			}
			return null;
		}

		// private static TweedleLambdaType GetLambdaType([NotNull] Tweedle.TweedleParser.LambdaTypeSignatureContext context)
		// {
		// 	Tweedle.TweedleParser.TypeTypeContext[] typeTypeContext = context.typeList().typeType();
		// 	List<TType> typeList = typeTypeContext == null ?
		// 		new List<TType>() :
		// 		typeTypeContext.Select(type => GetTypeType(type)).ToList();
		// 	return new TweedleLambdaType(typeList, GetTypeOrVoid(context.typeTypeOrVoid()));
		// }

		private static MemberFlags GetModifiers(List<string> inModifiers)
		{
            MemberFlags flags = MemberFlags.None;
            for (int i = 0; i < inModifiers.Count; ++i)
			{
                string modifierStr = inModifiers[i];
				switch(modifierStr)
				{
					case "readonly":
                        flags |= MemberFlags.Readonly;
                        break;

					case "static":
                        flags |= MemberFlags.Static;
                        break;
                }
            }

            return flags;
        }
	}
}

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
		public TweedleType Parse(string src)
		{
			AntlrInputStream antlerStream = new AntlrInputStream(src);
			Alice.Tweedle.TweedleLexer lexer = new Alice.Tweedle.TweedleLexer(antlerStream);
			CommonTokenStream tokenStream = new CommonTokenStream(lexer);
			Alice.Tweedle.TweedleParser parser = new Alice.Tweedle.TweedleParser(tokenStream);

			return parser.typeDeclaration().Accept(new TypeVisitor());
		}

		private class TypeVisitor : TweedleParserBaseVisitor<TweedleType>
		{
			public override TweedleType VisitClassDeclaration([NotNull] TweedleParser.ClassDeclarationContext context)
			{
				base.VisitClassDeclaration(context);
				string className = context.identifier().GetText();
				TweedleClass unlinkedClass;
				if (context.EXTENDS() != null)
				{
					string superclass = context.typeType().classOrInterfaceType().GetText();
					unlinkedClass = new TweedleClass(className, new TweedleTypeReference(superclass)); // TODO
				} else
				{
					unlinkedClass = new TweedleClass(className);
				}

				context.classBody().classBodyDeclaration().ToList().ForEach(cbd => cbd.EnterRule(new ClassBodyDeclarationListener(unlinkedClass)));
				return unlinkedClass;
			}

			public override TweedleType VisitEnumDeclaration([NotNull] TweedleParser.EnumDeclarationContext context)
			{
				base.VisitEnumDeclaration(context);
				List<string> values =
					context.enumConstants().enumConstant().Select(enumConst => enumConst.identifier().GetText()).ToList();
				return new TweedleEnum(context.identifier().GetText(), values);
			}
		}

		private class ClassBodyDeclarationListener : TweedleParserBaseListener
		{
			private TweedleClass unlinkedClass;
			private List<string> modifiers;

			public ClassBodyDeclarationListener(TweedleClass unlinkedClass)
			{
				this.unlinkedClass = unlinkedClass;
			}

			public override void VisitTerminal([NotNull] ITerminalNode node)
			{
				base.VisitTerminal(node);
				modifiers = new List<string>
				{
					node.GetText()
				};
			}

			public override void EnterBlock([NotNull] TweedleParser.BlockContext context)
			{
				base.EnterBlock(context);
			}

			public override void EnterMemberDeclaration([NotNull] TweedleParser.MemberDeclarationContext context)
			{
				base.EnterMemberDeclaration(context);
				context.EnterRule(new MemberDeclarationListener(unlinkedClass, modifiers));
			}

		}

		public class MemberDeclarationListener : TweedleParserBaseListener
		{
			private TweedleClass unlinkedClass;
			private List<string> modifiers;

			public MemberDeclarationListener(TweedleClass unlinkedClass, List<string> modifiers)
			{
				this.unlinkedClass = unlinkedClass;
				this.modifiers = modifiers;
			}

			public override void EnterFieldDeclaration([NotNull] TweedleParser.FieldDeclarationContext context)
			{
				base.EnterFieldDeclaration(context);
				TweedleTypeReference type = new TweedleTypeReference(context.typeType().GetText());
				VaraiableVisitor variableVisitor = new VaraiableVisitor(modifiers, type);
				List<TweedleField> properties = context.variableDeclarators().Accept(variableVisitor);
				unlinkedClass.properties.AddRange(properties);
			}

			public override void EnterMethodDeclaration([NotNull] TweedleParser.MethodDeclarationContext context)
			{
				base.EnterMethodDeclaration(context);
				StatementVisitor statementVisitor = new StatementVisitor();
				TweedleMethod method = new TweedleMethodReference(
						modifiers,
						new TweedleTypeReference(context.typeTypeOrVoid().GetText()),
						context.IDENTIFIER().GetText(),
						RequiredParameters(context.formalParameters()),
						OptionalParameters(context.formalParameters()),
						context.methodBody().block().blockStatement().Select(statement =>
							statement.Accept(statementVisitor)
						).ToList()
					);
				unlinkedClass.methods.Add(method);
			}

			public override void EnterConstructorDeclaration([NotNull] TweedleParser.ConstructorDeclarationContext context)
			{
				base.EnterConstructorDeclaration(context);
				StatementVisitor statementVisitor = new StatementVisitor();
				TweedleConstructor constructor = new TweedleConstructor(
					new TweedleTypeReference(context.IDENTIFIER().GetText()),
					context.IDENTIFIER().GetText(),
					RequiredParameters(context.formalParameters()),
					OptionalParameters(context.formalParameters()),
					context.constructorBody.blockStatement().Select(statement =>
							statement.Accept(statementVisitor)
						).ToList()
				);
				unlinkedClass.constructors.Add(constructor);
			}

			private List<TweedleField> RequiredParameters(TweedleParser.FormalParametersContext context)
			{
				return context.formalParameterList().requiredParameter().Select(field => new TweedleField(
							field.variableModifier().Select(modifier => modifier.GetText()).ToList(),
							new TweedleTypeReference(field.typeType().GetText()),
							field.variableDeclaratorId().IDENTIFIER().GetText()
						)).ToList();
			}

			private List<TweedleField> OptionalParameters(TweedleParser.FormalParametersContext context)
			{
				return context.formalParameterList().optionalParameter().Select(field => new TweedleField(
						field.variableModifier().Select(modifier => modifier.GetText()).ToList(),
						new TweedleTypeReference(field.typeType().GetText()),
						field.variableDeclaratorId().IDENTIFIER().GetText()
					)).ToList();
			}
		}

		private class VaraiableVisitor : TweedleParserBaseVisitor<List<TweedleField>>
		{
			private List<string> modifiers;
			private TweedleType type;

			public VaraiableVisitor(List<string> modifiers, TweedleType type)
			{
				this.modifiers = modifiers;
				this.type = type;
			}

			public override List<TweedleField> VisitVariableDeclarators([NotNull] TweedleParser.VariableDeclaratorsContext context)
			{
				StatementVisitor statementVisitor = new StatementVisitor();
				List<TweedleField> properties =
					context.variableDeclarator()
						.Select(field => new TweedleField(
								modifiers,
								type,
								field.variableDeclaratorId().IDENTIFIER().GetText(),
								statementVisitor.Visit(field.variableInitializer())
						)).ToList();
				return properties;
			}
		}

		private class StatementVisitor : TweedleParserBaseVisitor<TweedleStatement>
		{
			public override TweedleStatement VisitLocalVariableDeclaration([NotNull] TweedleParser.LocalVariableDeclarationContext context)
			{
				if (!string.IsNullOrEmpty(context.NODE_COMMENT().GetText()))
				{
					return new TweedleComment(); // TODO
				}
				List<string> modifiers = context.variableModifier().Select(modifier => modifier.GetText()).ToList();
				TweedleTypeReference type = new TweedleTypeReference(context.typeType().GetText());
				VaraiableVisitor variableVisitor = new VaraiableVisitor(modifiers, type);
				List<TweedleField> variables = context.variableDeclarators().Accept(variableVisitor);
				return new TweedleLocalVariableDeclaration(variables);
			}

			public override TweedleStatement VisitBlockStatement([NotNull] TweedleParser.BlockStatementContext context)
			{
				return base.VisitBlockStatement(context);
			}
		}
	}
}
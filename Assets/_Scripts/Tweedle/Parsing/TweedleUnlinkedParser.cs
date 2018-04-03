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
		public TweedleTypeReference Parse(string src)
		{
			AntlrInputStream antlerStream = new AntlrInputStream(src);
			Alice.Tweedle.TweedleLexer lexer = new Alice.Tweedle.TweedleLexer(antlerStream);
			CommonTokenStream tokenStream = new CommonTokenStream(lexer);
			Alice.Tweedle.TweedleParser parser = new Alice.Tweedle.TweedleParser(tokenStream);

			return parser.typeDeclaration().Accept(new TypeVisitor());
		}

		private class TypeVisitor : TweedleParserBaseVisitor<TweedleTypeReference>
		{
			public override TweedleTypeReference VisitClassDeclaration([NotNull] TweedleParser.ClassDeclarationContext context)
			{
				base.VisitClassDeclaration(context);
				string className = context.identifier().GetText();
				TweedleClass unlinkedClass;
				if (context.EXTENDS() != null)
				{
					string superclass = context.typeType().classOrInterfaceType().GetText();
					unlinkedClass = new TweedleClass(className, superclass);
				} else
				{
					unlinkedClass = new TweedleClass(className);
				}

				context.classBody().classBodyDeclaration().ToList().ForEach(cbd => cbd.EnterRule(new ClassBodyDeclarationListener(ref unlinkedClass)));
				return unlinkedClass;
			}

			public override TweedleTypeReference VisitEnumDeclaration([NotNull] TweedleParser.EnumDeclarationContext context)
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

			public ClassBodyDeclarationListener(ref TweedleClass unlinkedClass)
			{
				this.unlinkedClass = unlinkedClass;
			}

			public override void VisitTerminal([NotNull] ITerminalNode node)
			{
				base.VisitTerminal(node);
				modifiers = new List<string>();
				modifiers.Add(node.GetText());
			}

			public override void EnterBlock([NotNull] TweedleParser.BlockContext context)
			{
				base.EnterBlock(context);
			}

			public override void EnterClassModifier([NotNull] TweedleParser.ClassModifierContext context)
			{
				base.EnterClassModifier(context);
				modifiers = new List<string>();
				// Ignore visibility
				modifiers.Add(context.STATIC().GetText());
			}

			public override void EnterMemberDeclaration([NotNull] TweedleParser.MemberDeclarationContext context)
			{
				base.EnterMemberDeclaration(context);
				context.EnterRule(new MemberDeclarationListener(ref unlinkedClass, modifiers));
			}

		}

		public class MemberDeclarationListener : TweedleParserBaseListener
		{
			private TweedleClass unlinkedClass;
			private List<string> modifiers;

			public MemberDeclarationListener(ref TweedleClass unlinkedClass, List<string> modifiers)
			{
				this.unlinkedClass = unlinkedClass;
				this.modifiers = modifiers;
			}

			public override void EnterFieldDeclaration([NotNull] TweedleParser.FieldDeclarationContext context)
			{
				base.EnterFieldDeclaration(context);
				string type = context.typeType().GetText();
				StatementVisitor statementVisitor = new StatementVisitor();
				List<TweedleField<TweedleType>> fields = 
					context.variableDeclarators().variableDeclarator()
						.Select(field => new TweedleField(
								modifiers,
								type,
								field.variableDeclaratorId().GetText(), 
								statementVisitor.Visit(field.variableInitializer())
						)).ToList();
				unlinkedClass.fields.AddRange(fields);
			}

			public override void EnterMethodDeclaration([NotNull] TweedleParser.MethodDeclarationContext context)
			{
				base.EnterMethodDeclaration(context);
				StatementVisitor statementVisitor = new StatementVisitor();
				TweedleMethod method = new TweedleMethod(
						modifiers, 
						context.typeTypeOrVoid().GetText(),
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
				TweedleConstructor<TweedleType> constructor = new TweedleConstructor<TweedleType>(
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
							field.typeType().GetText(),
							field.variableDeclaratorId().GetText()
						)).ToList();
			}

			private List<TweedleField> OptionalParameters(TweedleParser.FormalParametersContext context)
			{
				return context.formalParameterList().optionalParameter().Select(field => new TweedleField(
						field.variableModifier().Select(modifier => modifier.GetText()).ToList(),
						field.typeType().GetText(),
						field.variableDeclaratorId().GetText()
					)).ToList();
			}
		}

		private class StatementVisitor : TweedleParserBaseVisitor<TweedleStatement>
		{
			public override TweedleStatement VisitStatement([NotNull] TweedleParser.StatementContext context)
			{
				// TODO
				string statementName = context.GetText();
				return new TweedleStatement(statementName);
			}

			public override TweedleStatement VisitLocalVariableDeclaration([NotNull] TweedleParser.LocalVariableDeclarationContext context)
			{
				// TODO SHOULD RETURN A FIELD NOT A STATEMENT
				return new TweedleStatement("Should be a field");
			}

			public override TweedleStatement VisitBlockStatement([NotNull] TweedleParser.BlockStatementContext context)
			{
				return base.VisitBlockStatement(context);
			}

			public override TweedleStatement VisitArrayInitializer([NotNull] TweedleParser.ArrayInitializerContext context)
			{
				return base.VisitArrayInitializer(context);
			}

			public override TweedleStatement VisitExpression([NotNull] TweedleParser.ExpressionContext context)
			{
				return base.VisitExpression(context);
			}
		}
	}
}
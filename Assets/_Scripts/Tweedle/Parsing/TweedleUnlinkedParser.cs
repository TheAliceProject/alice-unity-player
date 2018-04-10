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

				ClassBodyDeclarationVisitor cbdVisitor = new ClassBodyDeclarationVisitor(unlinkedClass);
				context.classBody().classBodyDeclaration().ToList().ForEach(cbd => cbd.Accept(cbdVisitor));
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

		private class ClassBodyDeclarationVisitor : TweedleParserBaseVisitor<Visitor>
		{
			private TweedleClass unlinkedClass;
			private List<string> modifiers;

			public ClassBodyDeclarationVisitor(TweedleClass unlinkedClass)
			{
				this.unlinkedClass = unlinkedClass;
			}

			public override Visitor VisitClassModifier([NotNull] TweedleParser.ClassModifierContext context)
			{
				modifiers = new List<string>
				{
					context.STATIC().ToString()
				};
				return base.VisitClassModifier(context);
			}


			public override Visitor VisitMemberDeclaration([NotNull] TweedleParser.MemberDeclarationContext context)
			{
				context.Accept(new MemberDeclarationVisitor(unlinkedClass, modifiers));
				return base.VisitMemberDeclaration(context);
			}
		}

		private class MemberDeclarationVisitor : TweedleParserBaseVisitor<Visitor>
		{
			private TweedleClass unlinkedClass;
			private List<string> modifiers;

			public MemberDeclarationVisitor(TweedleClass unlinkedClass, List<string> modifiers)
			{
				this.unlinkedClass = unlinkedClass;
				this.modifiers = modifiers;
			}

			public override Visitor VisitFieldDeclaration([NotNull] TweedleParser.FieldDeclarationContext context)
			{
				//UnityEngine.Debug.Log("Field");
				//UnityEngine.Debug.Log(context.typeType().GetText());
				TweedleTypeReference type = new TweedleTypeReference(context.typeType().GetText());
				VariableListVisitor variableVisitor = new VariableListVisitor(modifiers, type);
				List<TweedleField> properties = context.variableDeclarators().Accept(variableVisitor);
				unlinkedClass.properties.AddRange(properties);
				return base.VisitFieldDeclaration(context);
			}

			public override Visitor VisitMethodDeclaration([NotNull] TweedleParser.MethodDeclarationContext context)
			{
				//UnityEngine.Debug.Log("Method");
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
				return base.VisitMethodDeclaration(context);
			}

			public override Visitor VisitConstructorDeclaration([NotNull] TweedleParser.ConstructorDeclarationContext context)
			{
				//UnityEngine.Debug.Log("Constructor");
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
				return base.VisitConstructorDeclaration(context);
			}

			private List<TweedleField> RequiredParameters(TweedleParser.FormalParametersContext context)
			{
				if (context.formalParameterList() == null || context.formalParameterList().requiredParameter() == null)
				{
					return new List<TweedleField>();
				}
				return context.formalParameterList().requiredParameter().Select(field => new TweedleField(
							field.variableModifier().Select(modifier => modifier.GetText()).ToList(),
							new TweedleTypeReference(field.typeType().GetText()),
							field.variableDeclaratorId().IDENTIFIER().GetText()
						)).ToList();
			}

			private List<TweedleField> OptionalParameters(TweedleParser.FormalParametersContext context)
			{
				if (context.formalParameterList() == null || context.formalParameterList().optionalParameter() == null)
				{
					return new List<TweedleField>();
				}
				return context.formalParameterList().optionalParameter().Select(field => new TweedleField(
						field.variableModifier().Select(modifier => modifier.GetText()).ToList(),
						new TweedleTypeReference(field.typeType().GetText()),
						field.variableDeclaratorId().IDENTIFIER().GetText()
					)).ToList();
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
		private class Visitor
		{
			// return class for the visitor
		}
	}
}
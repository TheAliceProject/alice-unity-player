using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System.Collections.Generic;
using System.Linq;

namespace Alice.Tweedle.Unlinked
{
	public class TweedleUnlinkedParser
	{
		public UnlinkedType Parse(string src)
		{
			AntlrInputStream antlerStream = new AntlrInputStream(src);
			Alice.Tweedle.TweedleLexer lexer = new Alice.Tweedle.TweedleLexer(antlerStream);
			CommonTokenStream tokenStream = new CommonTokenStream(lexer);
			Alice.Tweedle.TweedleParser parser = new Alice.Tweedle.TweedleParser(tokenStream);
			parser.ErrorHandler = new ThrowOnParseErrorStrategy();

			TypeVisitor typeVisitor = new TypeVisitor();
			return typeVisitor.Visit(parser.typeDeclaration());
		}

		private class TypeVisitor : TweedleParserBaseVisitor<UnlinkedType>
		{
			public override UnlinkedType VisitClassDeclaration([NotNull] TweedleParser.ClassDeclarationContext context)
			{
				base.VisitClassDeclaration(context);
				string className = context.identifier().GetText();
				UnlinkedClass unlinkedClass;
				if (context.EXTENDS() != null)
				{
					string superclass = context.typeType().classOrInterfaceType().GetText();
					unlinkedClass = new UnlinkedClass(className, superclass);
				} else
				{
					unlinkedClass = new UnlinkedClass(className);
				}

				ClassBodyDeclarationListener cbdListener = new ClassBodyDeclarationListener(unlinkedClass);
				context.classBody().classBodyDeclaration().ToList().ForEach(cbd => cbd.EnterRule(cbdListener));

				return unlinkedClass;
			}

			public override UnlinkedType VisitEnumDeclaration([NotNull] TweedleParser.EnumDeclarationContext context)
			{
				base.VisitEnumDeclaration(context);
				List<string> values = new List<string>();
				context.enumConstants().enumConstant().ToList().ForEach(enumConst => values.Add(enumConst.identifier().GetText());
				return new UnlinkedEnum(context.identifier().GetText(), values);
			}
		}

		private class ClassBodyDeclarationListener : TweedleParserBaseListener
		{
			private UnlinkedClass unlinkedClass;

			public ClassBodyDeclarationListener(UnlinkedClass unlinkedClass)
			{
				this.unlinkedClass = unlinkedClass;
			}

			public override void EnterFieldDeclaration([NotNull] TweedleParser.FieldDeclarationContext context)
			{
				base.EnterFieldDeclaration(context);
				List<UnlinkedField> fields = new List<UnlinkedField>();
				context.variableDeclarators().variableDeclarator()
					.ToList()
					.ForEach(field => fields.Add(new UnlinkedField(
							field.variableDeclaratorId().GetText(), 
							field.variableInitializer().Accept(new StatementVisitor())
					)));
				unlinkedClass.fields = fields;
			}

			public override void EnterMethodDeclaration([NotNull] TweedleParser.MethodDeclarationContext context)
			{
				base.EnterMethodDeclaration(context);
				List<UnlinkedMethod> methods = new List<UnlinkedMethod>();
				/*
				context.methodBody()
					.ToList()
					.ForEach(field => fields.Add(new UnlinkedField(
							field.variableDeclaratorId().GetText(),
							field.variableInitializer().Accept(new StatementVisitor())
					)));
				*/
				unlinkedClass.methods = methods;
			}

			public override void EnterConstructorDeclaration([NotNull] TweedleParser.ConstructorDeclarationContext context)
			{
				base.EnterConstructorDeclaration(context);
				List<UnlinkedConstructor> constructors = new List<UnlinkedConstructor>();
				//context.constructorBody
				unlinkedClass.constructors = constructors;
			}
		}

		private class ClassBodyDeclarationVisitor : TweedleParserBaseVisitor<UnlinkedClassBodyDeclaration>
		{
			/*public override UnlinkedClassBodyDeclaration VisitMethodBody([NotNull] TweedleParser.MethodBodyContext context)
			{
				return base.VisitMethodBody(context);
			}*/

			/*public override TweedleMethod ([NotNull] TweedleParser.MethodDeclarationContext context)
			{
				string methodName = context.IDENTIFIER().GetText();
				InstructionVisitor instructionVisitor = new InstructionVisitor();
				List<TweedleStatement> instructions = null;//= context.instruction
				return new TweedleMethod(methodName, instructions);
			}*/
		}

		private class StatementVisitor : TweedleParserBaseVisitor<UnlinkedStatement>
		{
			public override UnlinkedStatement VisitStatement([NotNull] TweedleParser.StatementContext context)
			{
				string statementName = context.GetText();
				return new UnlinkedStatement(statementName);
			}
		}

		public class ThrowOnParseErrorStrategy : DefaultErrorStrategy
		{
			public override void Recover(Parser recognizer, RecognitionException e)
			{
				AddExceptionToAllContexts(recognizer, e);
				throw new ParseCanceledException(e);
			}

			public override IToken RecoverInline(Parser recognizer)
			{
				InputMismatchException e = new InputMismatchException(recognizer);
				AddExceptionToAllContexts(recognizer, e);
				throw new ParseCanceledException(e);
			}

			private void AddExceptionToAllContexts(Parser recognizer, RecognitionException e)
			{
				/* TODO
				for (ParserRuleContext context = recognizer.getContext(); context != null; context = context.getParent())
				{
					context.exception = e;
				}
				*/
			}
		}
	}
}
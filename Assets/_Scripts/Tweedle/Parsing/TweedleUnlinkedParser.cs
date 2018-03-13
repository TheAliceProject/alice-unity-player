using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System.Collections.Generic;

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
				if (context.EXTENDS() != null)
				{
					string superclass = context.typeType().classOrInterfaceType().GetText();
					return new UnlinkedClass(className, superclass);
				} else
				{
					return new UnlinkedClass(className);
				}
			}

			public override UnlinkedType VisitEnumDeclaration([NotNull] TweedleParser.EnumDeclarationContext context)
			{
				base.VisitEnumDeclaration(context);
				List<string> values = new List<string>();
				var enumConsts = context.enumConstants().enumConstant();
				foreach (var enumConst in enumConsts)
				{
					values.Add(enumConst.identifier().GetText());
				}
				return new UnlinkedEnum(context.identifier().GetText(), values);
			}
		}

		private class MethodVisitor : TweedleParserBaseVisitor<TweedleMethod>
		{
			public override TweedleMethod VisitMethodDeclaration([NotNull] TweedleParser.MethodDeclarationContext context)
			{
				string methodName = context.IDENTIFIER().GetText();
				InstructionVisitor instructionVisitor = new InstructionVisitor();
				List<TweedleStatement> instructions = null;//= context.instruction
				return new TweedleMethod(methodName, instructions);
			}
		}

		private class InstructionVisitor : TweedleParserBaseVisitor<TweedleStatement>
		{
			public override TweedleStatement VisitGenericMethodDeclaration([NotNull] TweedleParser.GenericMethodDeclarationContext context)
			{
				string instructionName = context.GetText();
				//return new TweedleStatement(instructionName);
				return new TweedleStatement();
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
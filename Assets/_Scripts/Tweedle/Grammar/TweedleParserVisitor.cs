//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.7.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from C:/Users/Jonathan/Documents/Repos/Filament/tweedle/Grammar\TweedleParser.g4 by ANTLR 4.7.2

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace Alice.Tweedle {
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="TweedleParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7.2")]
public interface ITweedleParserVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.typeDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTypeDeclaration([NotNull] TweedleParser.TypeDeclarationContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.classModifier"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitClassModifier([NotNull] TweedleParser.ClassModifierContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.visibility"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVisibility([NotNull] TweedleParser.VisibilityContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.visibilityLevel"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVisibilityLevel([NotNull] TweedleParser.VisibilityLevelContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.classDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitClassDeclaration([NotNull] TweedleParser.ClassDeclarationContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.identifier"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIdentifier([NotNull] TweedleParser.IdentifierContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.enumDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitEnumDeclaration([NotNull] TweedleParser.EnumDeclarationContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.enumConstants"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitEnumConstants([NotNull] TweedleParser.EnumConstantsContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.enumConstant"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitEnumConstant([NotNull] TweedleParser.EnumConstantContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.enumBodyDeclarations"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitEnumBodyDeclarations([NotNull] TweedleParser.EnumBodyDeclarationsContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.classBody"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitClassBody([NotNull] TweedleParser.ClassBodyContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.classBodyDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitClassBodyDeclaration([NotNull] TweedleParser.ClassBodyDeclarationContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.memberDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMemberDeclaration([NotNull] TweedleParser.MemberDeclarationContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.methodDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMethodDeclaration([NotNull] TweedleParser.MethodDeclarationContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.methodBody"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMethodBody([NotNull] TweedleParser.MethodBodyContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.typeTypeOrVoid"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTypeTypeOrVoid([NotNull] TweedleParser.TypeTypeOrVoidContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.constructorDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitConstructorDeclaration([NotNull] TweedleParser.ConstructorDeclarationContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.fieldDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFieldDeclaration([NotNull] TweedleParser.FieldDeclarationContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.variableDeclarator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVariableDeclarator([NotNull] TweedleParser.VariableDeclaratorContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.variableDeclaratorId"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVariableDeclaratorId([NotNull] TweedleParser.VariableDeclaratorIdContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.variableInitializer"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVariableInitializer([NotNull] TweedleParser.VariableInitializerContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.arrayInitializer"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArrayInitializer([NotNull] TweedleParser.ArrayInitializerContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.classType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitClassType([NotNull] TweedleParser.ClassTypeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.formalParameters"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFormalParameters([NotNull] TweedleParser.FormalParametersContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.formalParameterList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFormalParameterList([NotNull] TweedleParser.FormalParameterListContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.requiredParameter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRequiredParameter([NotNull] TweedleParser.RequiredParameterContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.optionalParameter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOptionalParameter([NotNull] TweedleParser.OptionalParameterContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.lastFormalParameter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLastFormalParameter([NotNull] TweedleParser.LastFormalParameterContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.literal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLiteral([NotNull] TweedleParser.LiteralContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBlock([NotNull] TweedleParser.BlockContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.blockStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBlockStatement([NotNull] TweedleParser.BlockStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.localVariableDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLocalVariableDeclaration([NotNull] TweedleParser.LocalVariableDeclarationContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatement([NotNull] TweedleParser.StatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.forControl"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitForControl([NotNull] TweedleParser.ForControlContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.parExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParExpression([NotNull] TweedleParser.ParExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.unlabeledExpressionList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitUnlabeledExpressionList([NotNull] TweedleParser.UnlabeledExpressionListContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.labeledExpressionList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLabeledExpressionList([NotNull] TweedleParser.LabeledExpressionListContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.labeledExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLabeledExpression([NotNull] TweedleParser.LabeledExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.methodCall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMethodCall([NotNull] TweedleParser.MethodCallContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.lambdaCall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLambdaCall([NotNull] TweedleParser.LambdaCallContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpression([NotNull] TweedleParser.ExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.lambdaExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLambdaExpression([NotNull] TweedleParser.LambdaExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.lambdaParameters"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLambdaParameters([NotNull] TweedleParser.LambdaParametersContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.lambdaTypeSignature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLambdaTypeSignature([NotNull] TweedleParser.LambdaTypeSignatureContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.typeList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTypeList([NotNull] TweedleParser.TypeListContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.primary"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPrimary([NotNull] TweedleParser.PrimaryContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.creator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCreator([NotNull] TweedleParser.CreatorContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.createdName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCreatedName([NotNull] TweedleParser.CreatedNameContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.arrayCreatorRest"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArrayCreatorRest([NotNull] TweedleParser.ArrayCreatorRestContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.classCreatorRest"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitClassCreatorRest([NotNull] TweedleParser.ClassCreatorRestContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.typeType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTypeType([NotNull] TweedleParser.TypeTypeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.primitiveType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPrimitiveType([NotNull] TweedleParser.PrimitiveTypeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.superSuffix"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSuperSuffix([NotNull] TweedleParser.SuperSuffixContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="TweedleParser.arguments"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArguments([NotNull] TweedleParser.ArgumentsContext context);
}
} // namespace Alice.Tweedle

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.7
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from /Users/daniel/dev/alice/alice/core/tweedle/src/main/antlr4/org/alice/tweedle/TweedleParser.g4 by ANTLR 4.7

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
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="TweedleParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7")]
[System.CLSCompliant(false)]
public interface ITweedleParserListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.typeDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTypeDeclaration([NotNull] TweedleParser.TypeDeclarationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.typeDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTypeDeclaration([NotNull] TweedleParser.TypeDeclarationContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.classModifier"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterClassModifier([NotNull] TweedleParser.ClassModifierContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.classModifier"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitClassModifier([NotNull] TweedleParser.ClassModifierContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.visibility"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVisibility([NotNull] TweedleParser.VisibilityContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.visibility"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVisibility([NotNull] TweedleParser.VisibilityContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.visibilityLevel"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVisibilityLevel([NotNull] TweedleParser.VisibilityLevelContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.visibilityLevel"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVisibilityLevel([NotNull] TweedleParser.VisibilityLevelContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.variableModifier"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVariableModifier([NotNull] TweedleParser.VariableModifierContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.variableModifier"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVariableModifier([NotNull] TweedleParser.VariableModifierContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.classDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterClassDeclaration([NotNull] TweedleParser.ClassDeclarationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.classDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitClassDeclaration([NotNull] TweedleParser.ClassDeclarationContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.identifier"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIdentifier([NotNull] TweedleParser.IdentifierContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.identifier"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIdentifier([NotNull] TweedleParser.IdentifierContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.enumDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterEnumDeclaration([NotNull] TweedleParser.EnumDeclarationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.enumDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitEnumDeclaration([NotNull] TweedleParser.EnumDeclarationContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.enumConstants"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterEnumConstants([NotNull] TweedleParser.EnumConstantsContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.enumConstants"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitEnumConstants([NotNull] TweedleParser.EnumConstantsContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.enumConstant"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterEnumConstant([NotNull] TweedleParser.EnumConstantContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.enumConstant"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitEnumConstant([NotNull] TweedleParser.EnumConstantContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.classBody"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterClassBody([NotNull] TweedleParser.ClassBodyContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.classBody"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitClassBody([NotNull] TweedleParser.ClassBodyContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.classBodyDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterClassBodyDeclaration([NotNull] TweedleParser.ClassBodyDeclarationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.classBodyDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitClassBodyDeclaration([NotNull] TweedleParser.ClassBodyDeclarationContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.memberDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMemberDeclaration([NotNull] TweedleParser.MemberDeclarationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.memberDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMemberDeclaration([NotNull] TweedleParser.MemberDeclarationContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.methodDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMethodDeclaration([NotNull] TweedleParser.MethodDeclarationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.methodDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMethodDeclaration([NotNull] TweedleParser.MethodDeclarationContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.methodBody"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMethodBody([NotNull] TweedleParser.MethodBodyContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.methodBody"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMethodBody([NotNull] TweedleParser.MethodBodyContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.typeTypeOrVoid"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTypeTypeOrVoid([NotNull] TweedleParser.TypeTypeOrVoidContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.typeTypeOrVoid"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTypeTypeOrVoid([NotNull] TweedleParser.TypeTypeOrVoidContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.constructorDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterConstructorDeclaration([NotNull] TweedleParser.ConstructorDeclarationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.constructorDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitConstructorDeclaration([NotNull] TweedleParser.ConstructorDeclarationContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.fieldDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFieldDeclaration([NotNull] TweedleParser.FieldDeclarationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.fieldDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFieldDeclaration([NotNull] TweedleParser.FieldDeclarationContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.variableDeclarator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVariableDeclarator([NotNull] TweedleParser.VariableDeclaratorContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.variableDeclarator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVariableDeclarator([NotNull] TweedleParser.VariableDeclaratorContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.variableDeclaratorId"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVariableDeclaratorId([NotNull] TweedleParser.VariableDeclaratorIdContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.variableDeclaratorId"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVariableDeclaratorId([NotNull] TweedleParser.VariableDeclaratorIdContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.variableInitializer"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVariableInitializer([NotNull] TweedleParser.VariableInitializerContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.variableInitializer"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVariableInitializer([NotNull] TweedleParser.VariableInitializerContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.arrayInitializer"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterArrayInitializer([NotNull] TweedleParser.ArrayInitializerContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.arrayInitializer"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitArrayInitializer([NotNull] TweedleParser.ArrayInitializerContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.classOrInterfaceType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterClassOrInterfaceType([NotNull] TweedleParser.ClassOrInterfaceTypeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.classOrInterfaceType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitClassOrInterfaceType([NotNull] TweedleParser.ClassOrInterfaceTypeContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.formalParameters"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFormalParameters([NotNull] TweedleParser.FormalParametersContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.formalParameters"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFormalParameters([NotNull] TweedleParser.FormalParametersContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.formalParameterList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFormalParameterList([NotNull] TweedleParser.FormalParameterListContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.formalParameterList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFormalParameterList([NotNull] TweedleParser.FormalParameterListContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.requiredParameter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterRequiredParameter([NotNull] TweedleParser.RequiredParameterContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.requiredParameter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitRequiredParameter([NotNull] TweedleParser.RequiredParameterContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.optionalParameter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterOptionalParameter([NotNull] TweedleParser.OptionalParameterContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.optionalParameter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitOptionalParameter([NotNull] TweedleParser.OptionalParameterContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.lastFormalParameter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLastFormalParameter([NotNull] TweedleParser.LastFormalParameterContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.lastFormalParameter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLastFormalParameter([NotNull] TweedleParser.LastFormalParameterContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.literal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLiteral([NotNull] TweedleParser.LiteralContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.literal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLiteral([NotNull] TweedleParser.LiteralContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBlock([NotNull] TweedleParser.BlockContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBlock([NotNull] TweedleParser.BlockContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.blockStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBlockStatement([NotNull] TweedleParser.BlockStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.blockStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBlockStatement([NotNull] TweedleParser.BlockStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.localVariableDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLocalVariableDeclaration([NotNull] TweedleParser.LocalVariableDeclarationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.localVariableDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLocalVariableDeclaration([NotNull] TweedleParser.LocalVariableDeclarationContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStatement([NotNull] TweedleParser.StatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStatement([NotNull] TweedleParser.StatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.forControl"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterForControl([NotNull] TweedleParser.ForControlContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.forControl"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitForControl([NotNull] TweedleParser.ForControlContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.parExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterParExpression([NotNull] TweedleParser.ParExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.parExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitParExpression([NotNull] TweedleParser.ParExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.labeledExpressionList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLabeledExpressionList([NotNull] TweedleParser.LabeledExpressionListContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.labeledExpressionList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLabeledExpressionList([NotNull] TweedleParser.LabeledExpressionListContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.labeledExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLabeledExpression([NotNull] TweedleParser.LabeledExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.labeledExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLabeledExpression([NotNull] TweedleParser.LabeledExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.methodCall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMethodCall([NotNull] TweedleParser.MethodCallContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.methodCall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMethodCall([NotNull] TweedleParser.MethodCallContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExpression([NotNull] TweedleParser.ExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExpression([NotNull] TweedleParser.ExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.lambdaExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLambdaExpression([NotNull] TweedleParser.LambdaExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.lambdaExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLambdaExpression([NotNull] TweedleParser.LambdaExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.lambdaParameters"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLambdaParameters([NotNull] TweedleParser.LambdaParametersContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.lambdaParameters"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLambdaParameters([NotNull] TweedleParser.LambdaParametersContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.lambdaBody"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLambdaBody([NotNull] TweedleParser.LambdaBodyContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.lambdaBody"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLambdaBody([NotNull] TweedleParser.LambdaBodyContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.primary"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterPrimary([NotNull] TweedleParser.PrimaryContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.primary"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitPrimary([NotNull] TweedleParser.PrimaryContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.creator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterCreator([NotNull] TweedleParser.CreatorContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.creator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitCreator([NotNull] TweedleParser.CreatorContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.createdName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterCreatedName([NotNull] TweedleParser.CreatedNameContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.createdName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitCreatedName([NotNull] TweedleParser.CreatedNameContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.arrayCreatorRest"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterArrayCreatorRest([NotNull] TweedleParser.ArrayCreatorRestContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.arrayCreatorRest"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitArrayCreatorRest([NotNull] TweedleParser.ArrayCreatorRestContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.classCreatorRest"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterClassCreatorRest([NotNull] TweedleParser.ClassCreatorRestContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.classCreatorRest"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitClassCreatorRest([NotNull] TweedleParser.ClassCreatorRestContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.typeType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTypeType([NotNull] TweedleParser.TypeTypeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.typeType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTypeType([NotNull] TweedleParser.TypeTypeContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.primitiveType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterPrimitiveType([NotNull] TweedleParser.PrimitiveTypeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.primitiveType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitPrimitiveType([NotNull] TweedleParser.PrimitiveTypeContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.superSuffix"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterSuperSuffix([NotNull] TweedleParser.SuperSuffixContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.superSuffix"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitSuperSuffix([NotNull] TweedleParser.SuperSuffixContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.arguments"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterArguments([NotNull] TweedleParser.ArgumentsContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.arguments"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitArguments([NotNull] TweedleParser.ArgumentsContext context);
}
} // namespace Alice.Tweedle

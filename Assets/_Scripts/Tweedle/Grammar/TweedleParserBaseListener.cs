//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.7
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from C:/Users/jkieffer.FILAMENT/Documents/Repos/alice-unity-player/Assets/_Scripts/Tweedle/Grammar\TweedleParser.g4 by ANTLR 4.7

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
using IErrorNode = Antlr4.Runtime.Tree.IErrorNode;
using ITerminalNode = Antlr4.Runtime.Tree.ITerminalNode;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="ITweedleParserListener"/>,
/// which can be extended to create a listener which only needs to handle a subset
/// of the available methods.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7")]
[System.CLSCompliant(false)]
public partial class TweedleParserBaseListener : ITweedleParserListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.typeDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterTypeDeclaration([NotNull] TweedleParser.TypeDeclarationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.typeDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitTypeDeclaration([NotNull] TweedleParser.TypeDeclarationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.classModifier"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterClassModifier([NotNull] TweedleParser.ClassModifierContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.classModifier"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitClassModifier([NotNull] TweedleParser.ClassModifierContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.visibility"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterVisibility([NotNull] TweedleParser.VisibilityContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.visibility"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitVisibility([NotNull] TweedleParser.VisibilityContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.visibilityLevel"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterVisibilityLevel([NotNull] TweedleParser.VisibilityLevelContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.visibilityLevel"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitVisibilityLevel([NotNull] TweedleParser.VisibilityLevelContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.classDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterClassDeclaration([NotNull] TweedleParser.ClassDeclarationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.classDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitClassDeclaration([NotNull] TweedleParser.ClassDeclarationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.identifier"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterIdentifier([NotNull] TweedleParser.IdentifierContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.identifier"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitIdentifier([NotNull] TweedleParser.IdentifierContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.enumDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterEnumDeclaration([NotNull] TweedleParser.EnumDeclarationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.enumDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitEnumDeclaration([NotNull] TweedleParser.EnumDeclarationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.enumConstants"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterEnumConstants([NotNull] TweedleParser.EnumConstantsContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.enumConstants"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitEnumConstants([NotNull] TweedleParser.EnumConstantsContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.enumConstant"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterEnumConstant([NotNull] TweedleParser.EnumConstantContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.enumConstant"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitEnumConstant([NotNull] TweedleParser.EnumConstantContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.enumBodyDeclarations"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterEnumBodyDeclarations([NotNull] TweedleParser.EnumBodyDeclarationsContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.enumBodyDeclarations"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitEnumBodyDeclarations([NotNull] TweedleParser.EnumBodyDeclarationsContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.classBody"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterClassBody([NotNull] TweedleParser.ClassBodyContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.classBody"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitClassBody([NotNull] TweedleParser.ClassBodyContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.classBodyDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterClassBodyDeclaration([NotNull] TweedleParser.ClassBodyDeclarationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.classBodyDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitClassBodyDeclaration([NotNull] TweedleParser.ClassBodyDeclarationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.memberDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterMemberDeclaration([NotNull] TweedleParser.MemberDeclarationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.memberDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitMemberDeclaration([NotNull] TweedleParser.MemberDeclarationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.methodDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterMethodDeclaration([NotNull] TweedleParser.MethodDeclarationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.methodDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitMethodDeclaration([NotNull] TweedleParser.MethodDeclarationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.methodBody"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterMethodBody([NotNull] TweedleParser.MethodBodyContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.methodBody"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitMethodBody([NotNull] TweedleParser.MethodBodyContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.typeTypeOrVoid"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterTypeTypeOrVoid([NotNull] TweedleParser.TypeTypeOrVoidContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.typeTypeOrVoid"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitTypeTypeOrVoid([NotNull] TweedleParser.TypeTypeOrVoidContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.constructorDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterConstructorDeclaration([NotNull] TweedleParser.ConstructorDeclarationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.constructorDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitConstructorDeclaration([NotNull] TweedleParser.ConstructorDeclarationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.fieldDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterFieldDeclaration([NotNull] TweedleParser.FieldDeclarationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.fieldDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitFieldDeclaration([NotNull] TweedleParser.FieldDeclarationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.variableDeclarator"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterVariableDeclarator([NotNull] TweedleParser.VariableDeclaratorContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.variableDeclarator"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitVariableDeclarator([NotNull] TweedleParser.VariableDeclaratorContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.variableDeclaratorId"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterVariableDeclaratorId([NotNull] TweedleParser.VariableDeclaratorIdContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.variableDeclaratorId"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitVariableDeclaratorId([NotNull] TweedleParser.VariableDeclaratorIdContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.variableInitializer"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterVariableInitializer([NotNull] TweedleParser.VariableInitializerContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.variableInitializer"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitVariableInitializer([NotNull] TweedleParser.VariableInitializerContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.arrayInitializer"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterArrayInitializer([NotNull] TweedleParser.ArrayInitializerContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.arrayInitializer"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitArrayInitializer([NotNull] TweedleParser.ArrayInitializerContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.classType"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterClassType([NotNull] TweedleParser.ClassTypeContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.classType"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitClassType([NotNull] TweedleParser.ClassTypeContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.formalParameters"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterFormalParameters([NotNull] TweedleParser.FormalParametersContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.formalParameters"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitFormalParameters([NotNull] TweedleParser.FormalParametersContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.formalParameterList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterFormalParameterList([NotNull] TweedleParser.FormalParameterListContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.formalParameterList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitFormalParameterList([NotNull] TweedleParser.FormalParameterListContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.requiredParameter"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterRequiredParameter([NotNull] TweedleParser.RequiredParameterContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.requiredParameter"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitRequiredParameter([NotNull] TweedleParser.RequiredParameterContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.optionalParameter"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterOptionalParameter([NotNull] TweedleParser.OptionalParameterContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.optionalParameter"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitOptionalParameter([NotNull] TweedleParser.OptionalParameterContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.lastFormalParameter"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLastFormalParameter([NotNull] TweedleParser.LastFormalParameterContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.lastFormalParameter"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLastFormalParameter([NotNull] TweedleParser.LastFormalParameterContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.literal"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLiteral([NotNull] TweedleParser.LiteralContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.literal"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLiteral([NotNull] TweedleParser.LiteralContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.block"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBlock([NotNull] TweedleParser.BlockContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.block"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBlock([NotNull] TweedleParser.BlockContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.blockStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBlockStatement([NotNull] TweedleParser.BlockStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.blockStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBlockStatement([NotNull] TweedleParser.BlockStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.localVariableDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLocalVariableDeclaration([NotNull] TweedleParser.LocalVariableDeclarationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.localVariableDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLocalVariableDeclaration([NotNull] TweedleParser.LocalVariableDeclarationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.statement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterStatement([NotNull] TweedleParser.StatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.statement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitStatement([NotNull] TweedleParser.StatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.forControl"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterForControl([NotNull] TweedleParser.ForControlContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.forControl"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitForControl([NotNull] TweedleParser.ForControlContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.parExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterParExpression([NotNull] TweedleParser.ParExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.parExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitParExpression([NotNull] TweedleParser.ParExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.unlabeledExpressionList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterUnlabeledExpressionList([NotNull] TweedleParser.UnlabeledExpressionListContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.unlabeledExpressionList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitUnlabeledExpressionList([NotNull] TweedleParser.UnlabeledExpressionListContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.labeledExpressionList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLabeledExpressionList([NotNull] TweedleParser.LabeledExpressionListContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.labeledExpressionList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLabeledExpressionList([NotNull] TweedleParser.LabeledExpressionListContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.labeledExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLabeledExpression([NotNull] TweedleParser.LabeledExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.labeledExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLabeledExpression([NotNull] TweedleParser.LabeledExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.methodCall"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterMethodCall([NotNull] TweedleParser.MethodCallContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.methodCall"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitMethodCall([NotNull] TweedleParser.MethodCallContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.lambdaCall"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLambdaCall([NotNull] TweedleParser.LambdaCallContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.lambdaCall"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLambdaCall([NotNull] TweedleParser.LambdaCallContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExpression([NotNull] TweedleParser.ExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExpression([NotNull] TweedleParser.ExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.lambdaExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLambdaExpression([NotNull] TweedleParser.LambdaExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.lambdaExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLambdaExpression([NotNull] TweedleParser.LambdaExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.lambdaParameters"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLambdaParameters([NotNull] TweedleParser.LambdaParametersContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.lambdaParameters"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLambdaParameters([NotNull] TweedleParser.LambdaParametersContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.lambdaTypeSignature"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLambdaTypeSignature([NotNull] TweedleParser.LambdaTypeSignatureContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.lambdaTypeSignature"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLambdaTypeSignature([NotNull] TweedleParser.LambdaTypeSignatureContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.typeList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterTypeList([NotNull] TweedleParser.TypeListContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.typeList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitTypeList([NotNull] TweedleParser.TypeListContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.primary"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPrimary([NotNull] TweedleParser.PrimaryContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.primary"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPrimary([NotNull] TweedleParser.PrimaryContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.creator"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterCreator([NotNull] TweedleParser.CreatorContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.creator"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitCreator([NotNull] TweedleParser.CreatorContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.createdName"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterCreatedName([NotNull] TweedleParser.CreatedNameContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.createdName"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitCreatedName([NotNull] TweedleParser.CreatedNameContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.arrayCreatorRest"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterArrayCreatorRest([NotNull] TweedleParser.ArrayCreatorRestContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.arrayCreatorRest"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitArrayCreatorRest([NotNull] TweedleParser.ArrayCreatorRestContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.classCreatorRest"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterClassCreatorRest([NotNull] TweedleParser.ClassCreatorRestContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.classCreatorRest"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitClassCreatorRest([NotNull] TweedleParser.ClassCreatorRestContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.typeType"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterTypeType([NotNull] TweedleParser.TypeTypeContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.typeType"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitTypeType([NotNull] TweedleParser.TypeTypeContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.primitiveType"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPrimitiveType([NotNull] TweedleParser.PrimitiveTypeContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.primitiveType"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPrimitiveType([NotNull] TweedleParser.PrimitiveTypeContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.superSuffix"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterSuperSuffix([NotNull] TweedleParser.SuperSuffixContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.superSuffix"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitSuperSuffix([NotNull] TweedleParser.SuperSuffixContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TweedleParser.arguments"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterArguments([NotNull] TweedleParser.ArgumentsContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TweedleParser.arguments"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitArguments([NotNull] TweedleParser.ArgumentsContext context) { }

	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void EnterEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void ExitEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitTerminal([NotNull] ITerminalNode node) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitErrorNode([NotNull] IErrorNode node) { }
}
} // namespace Alice.Tweedle

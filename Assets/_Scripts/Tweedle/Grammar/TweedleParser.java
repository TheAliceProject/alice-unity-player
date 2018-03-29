// Generated from Assets/_Scripts/Tweedle/Grammar/TweedleParser.g4 by ANTLR 4.7.1
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.misc.*;
import org.antlr.v4.runtime.tree.*;
import java.util.List;
import java.util.Iterator;
import java.util.ArrayList;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast"})
public class TweedleParser extends Parser {
	static { RuntimeMetaData.checkVersion("4.7.1", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		BOOLEAN=1, CLASS=2, COUNT_UP_TO=3, CONSTANT=4, DECIMAL_NUMBER=5, DO_IN_ORDER=6, 
		DO_TOGETHER=7, EACH=8, ELSE=9, ENUM=10, EXTENDS=11, FOR_EACH=12, EACH_TOGETHER=13, 
		HIDE=14, IF=15, IN=16, LOOP=17, MODELS=18, NEW=19, NUMBER=20, PRIMETIME=21, 
		RETURN=22, STATIC=23, STRING=24, SUPER=25, THIS=26, TUCKED_AWAY=27, VOID=28, 
		WHILE=29, WHOLE_NUMBER=30, DECIMAL_LITERAL=31, FLOAT_LITERAL=32, BOOL_LITERAL=33, 
		STRING_LITERAL=34, NULL_LITERAL=35, LPAREN=36, RPAREN=37, LBRACE=38, RBRACE=39, 
		LBRACK=40, RBRACK=41, SEMI=42, COMMA=43, DOT=44, ASSIGN=45, GT=46, LT=47, 
		BANG=48, TILDE=49, QUESTION=50, COLON=51, EQUAL=52, LE=53, GE=54, NOTEQUAL=55, 
		AND=56, OR=57, INC=58, DEC=59, ADD=60, SUB=61, MUL=62, DIV=63, BITAND=64, 
		BITOR=65, CARET=66, MOD=67, ADD_ASSIGN=68, SUB_ASSIGN=69, MUL_ASSIGN=70, 
		DIV_ASSIGN=71, AND_ASSIGN=72, OR_ASSIGN=73, XOR_ASSIGN=74, MOD_ASSIGN=75, 
		LSHIFT_ASSIGN=76, RSHIFT_ASSIGN=77, URSHIFT_ASSIGN=78, ARROW=79, COLONCOLON=80, 
		AT=81, ELLIPSIS=82, LARROW=83, WS=84, COMMENT=85, LINE_COMMENT=86, NODE_COMMENT=87, 
		IDENTIFIER=88;
	public static final int
		RULE_typeDeclaration = 0, RULE_classModifier = 1, RULE_visibility = 2, 
		RULE_visibilityLevel = 3, RULE_variableModifier = 4, RULE_classDeclaration = 5, 
		RULE_identifier = 6, RULE_typeParameters = 7, RULE_typeParameter = 8, 
		RULE_typeBound = 9, RULE_enumDeclaration = 10, RULE_enumConstants = 11, 
		RULE_enumConstant = 12, RULE_classBody = 13, RULE_classBodyDeclaration = 14, 
		RULE_memberDeclaration = 15, RULE_methodDeclaration = 16, RULE_methodBody = 17, 
		RULE_typeTypeOrVoid = 18, RULE_genericMethodDeclaration = 19, RULE_genericConstructorDeclaration = 20, 
		RULE_constructorDeclaration = 21, RULE_fieldDeclaration = 22, RULE_variableDeclarators = 23, 
		RULE_variableDeclarator = 24, RULE_variableDeclaratorId = 25, RULE_variableInitializer = 26, 
		RULE_arrayInitializer = 27, RULE_classOrInterfaceType = 28, RULE_typeArgument = 29, 
		RULE_formalParameters = 30, RULE_formalParameterList = 31, RULE_requiredParameter = 32, 
		RULE_optionalParameter = 33, RULE_lastFormalParameter = 34, RULE_literal = 35, 
		RULE_block = 36, RULE_blockStatement = 37, RULE_localVariableDeclaration = 38, 
		RULE_statement = 39, RULE_forControl = 40, RULE_parExpression = 41, RULE_labeledExpressionList = 42, 
		RULE_labeledExpression = 43, RULE_expressionList = 44, RULE_methodCall = 45, 
		RULE_expression = 46, RULE_lambdaExpression = 47, RULE_lambdaParameters = 48, 
		RULE_lambdaBody = 49, RULE_primary = 50, RULE_classType = 51, RULE_creator = 52, 
		RULE_createdName = 53, RULE_innerCreator = 54, RULE_arrayCreatorRest = 55, 
		RULE_classCreatorRest = 56, RULE_explicitGenericInvocation = 57, RULE_typeArgumentsOrDiamond = 58, 
		RULE_nonWildcardTypeArgumentsOrDiamond = 59, RULE_nonWildcardTypeArguments = 60, 
		RULE_typeList = 61, RULE_typeType = 62, RULE_primitiveType = 63, RULE_typeArguments = 64, 
		RULE_superSuffix = 65, RULE_explicitGenericInvocationSuffix = 66, RULE_arguments = 67;
	public static final String[] ruleNames = {
		"typeDeclaration", "classModifier", "visibility", "visibilityLevel", "variableModifier", 
		"classDeclaration", "identifier", "typeParameters", "typeParameter", "typeBound", 
		"enumDeclaration", "enumConstants", "enumConstant", "classBody", "classBodyDeclaration", 
		"memberDeclaration", "methodDeclaration", "methodBody", "typeTypeOrVoid", 
		"genericMethodDeclaration", "genericConstructorDeclaration", "constructorDeclaration", 
		"fieldDeclaration", "variableDeclarators", "variableDeclarator", "variableDeclaratorId", 
		"variableInitializer", "arrayInitializer", "classOrInterfaceType", "typeArgument", 
		"formalParameters", "formalParameterList", "requiredParameter", "optionalParameter", 
		"lastFormalParameter", "literal", "block", "blockStatement", "localVariableDeclaration", 
		"statement", "forControl", "parExpression", "labeledExpressionList", "labeledExpression", 
		"expressionList", "methodCall", "expression", "lambdaExpression", "lambdaParameters", 
		"lambdaBody", "primary", "classType", "creator", "createdName", "innerCreator", 
		"arrayCreatorRest", "classCreatorRest", "explicitGenericInvocation", "typeArgumentsOrDiamond", 
		"nonWildcardTypeArgumentsOrDiamond", "nonWildcardTypeArguments", "typeList", 
		"typeType", "primitiveType", "typeArguments", "superSuffix", "explicitGenericInvocationSuffix", 
		"arguments"
	};

	private static final String[] _LITERAL_NAMES = {
		null, "'Boolean'", "'class'", "'countUpTo'", "'constant'", "'DecimalNumber'", 
		"'doInOrder'", "'doTogether'", "'each'", "'else'", "'enum'", "'extends'", 
		"'forEach'", "'eachTogether'", "'hidden'", "'if'", "'in'", "'loop'", "'models'", 
		"'new'", "'Number'", "'primetime'", "'return'", "'static'", "'String'", 
		"'super'", "'this'", "'tuckedAway'", "'void'", "'while'", "'WholeNumber'", 
		null, null, null, null, "'null'", "'('", "')'", "'{'", "'}'", "'['", "']'", 
		"';'", "','", "'.'", "'='", "'>'", "'<'", "'!'", "'~'", "'?'", "':'", 
		"'=='", "'<='", "'>='", "'!='", "'&&'", "'||'", "'++'", "'--'", "'+'", 
		"'-'", "'*'", "'/'", "'&'", "'|'", "'^'", "'%'", "'+='", "'-='", "'*='", 
		"'/='", "'&='", "'|='", "'^='", "'%='", "'<<='", "'>>='", "'>>>='", "'->'", 
		"'::'", "'@'", "'...'", "'<-'", null, null, null, "'**'"
	};
	private static final String[] _SYMBOLIC_NAMES = {
		null, "BOOLEAN", "CLASS", "COUNT_UP_TO", "CONSTANT", "DECIMAL_NUMBER", 
		"DO_IN_ORDER", "DO_TOGETHER", "EACH", "ELSE", "ENUM", "EXTENDS", "FOR_EACH", 
		"EACH_TOGETHER", "HIDE", "IF", "IN", "LOOP", "MODELS", "NEW", "NUMBER", 
		"PRIMETIME", "RETURN", "STATIC", "STRING", "SUPER", "THIS", "TUCKED_AWAY", 
		"VOID", "WHILE", "WHOLE_NUMBER", "DECIMAL_LITERAL", "FLOAT_LITERAL", "BOOL_LITERAL", 
		"STRING_LITERAL", "NULL_LITERAL", "LPAREN", "RPAREN", "LBRACE", "RBRACE", 
		"LBRACK", "RBRACK", "SEMI", "COMMA", "DOT", "ASSIGN", "GT", "LT", "BANG", 
		"TILDE", "QUESTION", "COLON", "EQUAL", "LE", "GE", "NOTEQUAL", "AND", 
		"OR", "INC", "DEC", "ADD", "SUB", "MUL", "DIV", "BITAND", "BITOR", "CARET", 
		"MOD", "ADD_ASSIGN", "SUB_ASSIGN", "MUL_ASSIGN", "DIV_ASSIGN", "AND_ASSIGN", 
		"OR_ASSIGN", "XOR_ASSIGN", "MOD_ASSIGN", "LSHIFT_ASSIGN", "RSHIFT_ASSIGN", 
		"URSHIFT_ASSIGN", "ARROW", "COLONCOLON", "AT", "ELLIPSIS", "LARROW", "WS", 
		"COMMENT", "LINE_COMMENT", "NODE_COMMENT", "IDENTIFIER"
	};
	public static final Vocabulary VOCABULARY = new VocabularyImpl(_LITERAL_NAMES, _SYMBOLIC_NAMES);

	/**
	 * @deprecated Use {@link #VOCABULARY} instead.
	 */
	@Deprecated
	public static final String[] tokenNames;
	static {
		tokenNames = new String[_SYMBOLIC_NAMES.length];
		for (int i = 0; i < tokenNames.length; i++) {
			tokenNames[i] = VOCABULARY.getLiteralName(i);
			if (tokenNames[i] == null) {
				tokenNames[i] = VOCABULARY.getSymbolicName(i);
			}

			if (tokenNames[i] == null) {
				tokenNames[i] = "<INVALID>";
			}
		}
	}

	@Override
	@Deprecated
	public String[] getTokenNames() {
		return tokenNames;
	}

	@Override

	public Vocabulary getVocabulary() {
		return VOCABULARY;
	}

	@Override
	public String getGrammarFileName() { return "TweedleParser.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public ATN getATN() { return _ATN; }

	public TweedleParser(TokenStream input) {
		super(input);
		_interp = new ParserATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}
	public static class TypeDeclarationContext extends ParserRuleContext {
		public ClassDeclarationContext classDeclaration() {
			return getRuleContext(ClassDeclarationContext.class,0);
		}
		public EnumDeclarationContext enumDeclaration() {
			return getRuleContext(EnumDeclarationContext.class,0);
		}
		public List<ClassModifierContext> classModifier() {
			return getRuleContexts(ClassModifierContext.class);
		}
		public ClassModifierContext classModifier(int i) {
			return getRuleContext(ClassModifierContext.class,i);
		}
		public TypeDeclarationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_typeDeclaration; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterTypeDeclaration(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitTypeDeclaration(this);
		}
	}

	public final TypeDeclarationContext typeDeclaration() throws RecognitionException {
		TypeDeclarationContext _localctx = new TypeDeclarationContext(_ctx, getState());
		enterRule(_localctx, 0, RULE_typeDeclaration);
		int _la;
		try {
			setState(147);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case CLASS:
			case ENUM:
			case STATIC:
			case AT:
				enterOuterAlt(_localctx, 1);
				{
				setState(139);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==STATIC || _la==AT) {
					{
					{
					setState(136);
					classModifier();
					}
					}
					setState(141);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				setState(144);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case CLASS:
					{
					setState(142);
					classDeclaration();
					}
					break;
				case ENUM:
					{
					setState(143);
					enumDeclaration();
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				}
				break;
			case SEMI:
				enterOuterAlt(_localctx, 2);
				{
				setState(146);
				match(SEMI);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ClassModifierContext extends ParserRuleContext {
		public VisibilityContext visibility() {
			return getRuleContext(VisibilityContext.class,0);
		}
		public TerminalNode STATIC() { return getToken(TweedleParser.STATIC, 0); }
		public ClassModifierContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_classModifier; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterClassModifier(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitClassModifier(this);
		}
	}

	public final ClassModifierContext classModifier() throws RecognitionException {
		ClassModifierContext _localctx = new ClassModifierContext(_ctx, getState());
		enterRule(_localctx, 2, RULE_classModifier);
		try {
			setState(151);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case AT:
				enterOuterAlt(_localctx, 1);
				{
				setState(149);
				visibility();
				}
				break;
			case STATIC:
				enterOuterAlt(_localctx, 2);
				{
				setState(150);
				match(STATIC);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class VisibilityContext extends ParserRuleContext {
		public VisibilityLevelContext visibilityLevel() {
			return getRuleContext(VisibilityLevelContext.class,0);
		}
		public VisibilityContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_visibility; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterVisibility(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitVisibility(this);
		}
	}

	public final VisibilityContext visibility() throws RecognitionException {
		VisibilityContext _localctx = new VisibilityContext(_ctx, getState());
		enterRule(_localctx, 4, RULE_visibility);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(153);
			match(AT);
			setState(154);
			visibilityLevel();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class VisibilityLevelContext extends ParserRuleContext {
		public TerminalNode HIDE() { return getToken(TweedleParser.HIDE, 0); }
		public TerminalNode TUCKED_AWAY() { return getToken(TweedleParser.TUCKED_AWAY, 0); }
		public TerminalNode PRIMETIME() { return getToken(TweedleParser.PRIMETIME, 0); }
		public VisibilityLevelContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_visibilityLevel; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterVisibilityLevel(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitVisibilityLevel(this);
		}
	}

	public final VisibilityLevelContext visibilityLevel() throws RecognitionException {
		VisibilityLevelContext _localctx = new VisibilityLevelContext(_ctx, getState());
		enterRule(_localctx, 6, RULE_visibilityLevel);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(156);
			_la = _input.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << HIDE) | (1L << PRIMETIME) | (1L << TUCKED_AWAY))) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class VariableModifierContext extends ParserRuleContext {
		public TerminalNode CONSTANT() { return getToken(TweedleParser.CONSTANT, 0); }
		public VariableModifierContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_variableModifier; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterVariableModifier(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitVariableModifier(this);
		}
	}

	public final VariableModifierContext variableModifier() throws RecognitionException {
		VariableModifierContext _localctx = new VariableModifierContext(_ctx, getState());
		enterRule(_localctx, 8, RULE_variableModifier);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(158);
			match(CONSTANT);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ClassDeclarationContext extends ParserRuleContext {
		public TerminalNode CLASS() { return getToken(TweedleParser.CLASS, 0); }
		public IdentifierContext identifier() {
			return getRuleContext(IdentifierContext.class,0);
		}
		public ClassBodyContext classBody() {
			return getRuleContext(ClassBodyContext.class,0);
		}
		public TypeParametersContext typeParameters() {
			return getRuleContext(TypeParametersContext.class,0);
		}
		public TerminalNode EXTENDS() { return getToken(TweedleParser.EXTENDS, 0); }
		public TypeTypeContext typeType() {
			return getRuleContext(TypeTypeContext.class,0);
		}
		public TerminalNode MODELS() { return getToken(TweedleParser.MODELS, 0); }
		public TerminalNode IDENTIFIER() { return getToken(TweedleParser.IDENTIFIER, 0); }
		public ClassDeclarationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_classDeclaration; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterClassDeclaration(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitClassDeclaration(this);
		}
	}

	public final ClassDeclarationContext classDeclaration() throws RecognitionException {
		ClassDeclarationContext _localctx = new ClassDeclarationContext(_ctx, getState());
		enterRule(_localctx, 10, RULE_classDeclaration);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(160);
			match(CLASS);
			setState(161);
			identifier();
			setState(163);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==LT) {
				{
				setState(162);
				typeParameters();
				}
			}

			setState(167);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==EXTENDS) {
				{
				setState(165);
				match(EXTENDS);
				setState(166);
				typeType();
				}
			}

			setState(171);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==MODELS) {
				{
				setState(169);
				match(MODELS);
				setState(170);
				match(IDENTIFIER);
				}
			}

			setState(173);
			classBody();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class IdentifierContext extends ParserRuleContext {
		public TerminalNode IDENTIFIER() { return getToken(TweedleParser.IDENTIFIER, 0); }
		public IdentifierContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_identifier; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterIdentifier(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitIdentifier(this);
		}
	}

	public final IdentifierContext identifier() throws RecognitionException {
		IdentifierContext _localctx = new IdentifierContext(_ctx, getState());
		enterRule(_localctx, 12, RULE_identifier);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(175);
			match(IDENTIFIER);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class TypeParametersContext extends ParserRuleContext {
		public List<TypeParameterContext> typeParameter() {
			return getRuleContexts(TypeParameterContext.class);
		}
		public TypeParameterContext typeParameter(int i) {
			return getRuleContext(TypeParameterContext.class,i);
		}
		public TypeParametersContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_typeParameters; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterTypeParameters(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitTypeParameters(this);
		}
	}

	public final TypeParametersContext typeParameters() throws RecognitionException {
		TypeParametersContext _localctx = new TypeParametersContext(_ctx, getState());
		enterRule(_localctx, 14, RULE_typeParameters);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(177);
			match(LT);
			setState(178);
			typeParameter();
			setState(183);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(179);
				match(COMMA);
				setState(180);
				typeParameter();
				}
				}
				setState(185);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(186);
			match(GT);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class TypeParameterContext extends ParserRuleContext {
		public TerminalNode IDENTIFIER() { return getToken(TweedleParser.IDENTIFIER, 0); }
		public TerminalNode EXTENDS() { return getToken(TweedleParser.EXTENDS, 0); }
		public TypeBoundContext typeBound() {
			return getRuleContext(TypeBoundContext.class,0);
		}
		public TypeParameterContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_typeParameter; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterTypeParameter(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitTypeParameter(this);
		}
	}

	public final TypeParameterContext typeParameter() throws RecognitionException {
		TypeParameterContext _localctx = new TypeParameterContext(_ctx, getState());
		enterRule(_localctx, 16, RULE_typeParameter);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(188);
			match(IDENTIFIER);
			setState(191);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==EXTENDS) {
				{
				setState(189);
				match(EXTENDS);
				setState(190);
				typeBound();
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class TypeBoundContext extends ParserRuleContext {
		public List<TypeTypeContext> typeType() {
			return getRuleContexts(TypeTypeContext.class);
		}
		public TypeTypeContext typeType(int i) {
			return getRuleContext(TypeTypeContext.class,i);
		}
		public TypeBoundContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_typeBound; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterTypeBound(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitTypeBound(this);
		}
	}

	public final TypeBoundContext typeBound() throws RecognitionException {
		TypeBoundContext _localctx = new TypeBoundContext(_ctx, getState());
		enterRule(_localctx, 18, RULE_typeBound);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(193);
			typeType();
			setState(198);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==BITAND) {
				{
				{
				setState(194);
				match(BITAND);
				setState(195);
				typeType();
				}
				}
				setState(200);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class EnumDeclarationContext extends ParserRuleContext {
		public TerminalNode ENUM() { return getToken(TweedleParser.ENUM, 0); }
		public IdentifierContext identifier() {
			return getRuleContext(IdentifierContext.class,0);
		}
		public EnumConstantsContext enumConstants() {
			return getRuleContext(EnumConstantsContext.class,0);
		}
		public EnumDeclarationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_enumDeclaration; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterEnumDeclaration(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitEnumDeclaration(this);
		}
	}

	public final EnumDeclarationContext enumDeclaration() throws RecognitionException {
		EnumDeclarationContext _localctx = new EnumDeclarationContext(_ctx, getState());
		enterRule(_localctx, 20, RULE_enumDeclaration);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(201);
			match(ENUM);
			setState(202);
			identifier();
			setState(203);
			match(LBRACE);
			setState(205);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==IDENTIFIER) {
				{
				setState(204);
				enumConstants();
				}
			}

			setState(207);
			match(RBRACE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class EnumConstantsContext extends ParserRuleContext {
		public List<EnumConstantContext> enumConstant() {
			return getRuleContexts(EnumConstantContext.class);
		}
		public EnumConstantContext enumConstant(int i) {
			return getRuleContext(EnumConstantContext.class,i);
		}
		public EnumConstantsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_enumConstants; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterEnumConstants(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitEnumConstants(this);
		}
	}

	public final EnumConstantsContext enumConstants() throws RecognitionException {
		EnumConstantsContext _localctx = new EnumConstantsContext(_ctx, getState());
		enterRule(_localctx, 22, RULE_enumConstants);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(209);
			enumConstant();
			setState(214);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(210);
				match(COMMA);
				setState(211);
				enumConstant();
				}
				}
				setState(216);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class EnumConstantContext extends ParserRuleContext {
		public IdentifierContext identifier() {
			return getRuleContext(IdentifierContext.class,0);
		}
		public ArgumentsContext arguments() {
			return getRuleContext(ArgumentsContext.class,0);
		}
		public ClassBodyContext classBody() {
			return getRuleContext(ClassBodyContext.class,0);
		}
		public EnumConstantContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_enumConstant; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterEnumConstant(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitEnumConstant(this);
		}
	}

	public final EnumConstantContext enumConstant() throws RecognitionException {
		EnumConstantContext _localctx = new EnumConstantContext(_ctx, getState());
		enterRule(_localctx, 24, RULE_enumConstant);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(217);
			identifier();
			setState(219);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==LPAREN) {
				{
				setState(218);
				arguments();
				}
			}

			setState(222);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==LBRACE) {
				{
				setState(221);
				classBody();
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ClassBodyContext extends ParserRuleContext {
		public List<ClassBodyDeclarationContext> classBodyDeclaration() {
			return getRuleContexts(ClassBodyDeclarationContext.class);
		}
		public ClassBodyDeclarationContext classBodyDeclaration(int i) {
			return getRuleContext(ClassBodyDeclarationContext.class,i);
		}
		public ClassBodyContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_classBody; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterClassBody(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitClassBody(this);
		}
	}

	public final ClassBodyContext classBody() throws RecognitionException {
		ClassBodyContext _localctx = new ClassBodyContext(_ctx, getState());
		enterRule(_localctx, 26, RULE_classBody);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(224);
			match(LBRACE);
			setState(228);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << BOOLEAN) | (1L << CLASS) | (1L << DECIMAL_NUMBER) | (1L << ENUM) | (1L << NUMBER) | (1L << STATIC) | (1L << STRING) | (1L << VOID) | (1L << WHOLE_NUMBER) | (1L << LBRACE) | (1L << SEMI) | (1L << LT))) != 0) || _la==AT || _la==IDENTIFIER) {
				{
				{
				setState(225);
				classBodyDeclaration();
				}
				}
				setState(230);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(231);
			match(RBRACE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ClassBodyDeclarationContext extends ParserRuleContext {
		public BlockContext block() {
			return getRuleContext(BlockContext.class,0);
		}
		public TerminalNode STATIC() { return getToken(TweedleParser.STATIC, 0); }
		public MemberDeclarationContext memberDeclaration() {
			return getRuleContext(MemberDeclarationContext.class,0);
		}
		public List<ClassModifierContext> classModifier() {
			return getRuleContexts(ClassModifierContext.class);
		}
		public ClassModifierContext classModifier(int i) {
			return getRuleContext(ClassModifierContext.class,i);
		}
		public ClassBodyDeclarationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_classBodyDeclaration; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterClassBodyDeclaration(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitClassBodyDeclaration(this);
		}
	}

	public final ClassBodyDeclarationContext classBodyDeclaration() throws RecognitionException {
		ClassBodyDeclarationContext _localctx = new ClassBodyDeclarationContext(_ctx, getState());
		enterRule(_localctx, 28, RULE_classBodyDeclaration);
		int _la;
		try {
			setState(245);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,17,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(233);
				match(SEMI);
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(235);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==STATIC) {
					{
					setState(234);
					match(STATIC);
					}
				}

				setState(237);
				block();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(241);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==STATIC || _la==AT) {
					{
					{
					setState(238);
					classModifier();
					}
					}
					setState(243);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				setState(244);
				memberDeclaration();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class MemberDeclarationContext extends ParserRuleContext {
		public MethodDeclarationContext methodDeclaration() {
			return getRuleContext(MethodDeclarationContext.class,0);
		}
		public GenericMethodDeclarationContext genericMethodDeclaration() {
			return getRuleContext(GenericMethodDeclarationContext.class,0);
		}
		public FieldDeclarationContext fieldDeclaration() {
			return getRuleContext(FieldDeclarationContext.class,0);
		}
		public ConstructorDeclarationContext constructorDeclaration() {
			return getRuleContext(ConstructorDeclarationContext.class,0);
		}
		public GenericConstructorDeclarationContext genericConstructorDeclaration() {
			return getRuleContext(GenericConstructorDeclarationContext.class,0);
		}
		public ClassDeclarationContext classDeclaration() {
			return getRuleContext(ClassDeclarationContext.class,0);
		}
		public EnumDeclarationContext enumDeclaration() {
			return getRuleContext(EnumDeclarationContext.class,0);
		}
		public MemberDeclarationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_memberDeclaration; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterMemberDeclaration(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitMemberDeclaration(this);
		}
	}

	public final MemberDeclarationContext memberDeclaration() throws RecognitionException {
		MemberDeclarationContext _localctx = new MemberDeclarationContext(_ctx, getState());
		enterRule(_localctx, 30, RULE_memberDeclaration);
		try {
			setState(254);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,18,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(247);
				methodDeclaration();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(248);
				genericMethodDeclaration();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(249);
				fieldDeclaration();
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(250);
				constructorDeclaration();
				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(251);
				genericConstructorDeclaration();
				}
				break;
			case 6:
				enterOuterAlt(_localctx, 6);
				{
				setState(252);
				classDeclaration();
				}
				break;
			case 7:
				enterOuterAlt(_localctx, 7);
				{
				setState(253);
				enumDeclaration();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class MethodDeclarationContext extends ParserRuleContext {
		public TypeTypeOrVoidContext typeTypeOrVoid() {
			return getRuleContext(TypeTypeOrVoidContext.class,0);
		}
		public TerminalNode IDENTIFIER() { return getToken(TweedleParser.IDENTIFIER, 0); }
		public FormalParametersContext formalParameters() {
			return getRuleContext(FormalParametersContext.class,0);
		}
		public MethodBodyContext methodBody() {
			return getRuleContext(MethodBodyContext.class,0);
		}
		public MethodDeclarationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_methodDeclaration; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterMethodDeclaration(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitMethodDeclaration(this);
		}
	}

	public final MethodDeclarationContext methodDeclaration() throws RecognitionException {
		MethodDeclarationContext _localctx = new MethodDeclarationContext(_ctx, getState());
		enterRule(_localctx, 32, RULE_methodDeclaration);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(256);
			typeTypeOrVoid();
			setState(257);
			match(IDENTIFIER);
			setState(258);
			formalParameters();
			setState(263);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==LBRACK) {
				{
				{
				setState(259);
				match(LBRACK);
				setState(260);
				match(RBRACK);
				}
				}
				setState(265);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(266);
			methodBody();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class MethodBodyContext extends ParserRuleContext {
		public BlockContext block() {
			return getRuleContext(BlockContext.class,0);
		}
		public MethodBodyContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_methodBody; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterMethodBody(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitMethodBody(this);
		}
	}

	public final MethodBodyContext methodBody() throws RecognitionException {
		MethodBodyContext _localctx = new MethodBodyContext(_ctx, getState());
		enterRule(_localctx, 34, RULE_methodBody);
		try {
			setState(270);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case LBRACE:
				enterOuterAlt(_localctx, 1);
				{
				setState(268);
				block();
				}
				break;
			case SEMI:
				enterOuterAlt(_localctx, 2);
				{
				setState(269);
				match(SEMI);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class TypeTypeOrVoidContext extends ParserRuleContext {
		public TypeTypeContext typeType() {
			return getRuleContext(TypeTypeContext.class,0);
		}
		public TerminalNode VOID() { return getToken(TweedleParser.VOID, 0); }
		public TypeTypeOrVoidContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_typeTypeOrVoid; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterTypeTypeOrVoid(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitTypeTypeOrVoid(this);
		}
	}

	public final TypeTypeOrVoidContext typeTypeOrVoid() throws RecognitionException {
		TypeTypeOrVoidContext _localctx = new TypeTypeOrVoidContext(_ctx, getState());
		enterRule(_localctx, 36, RULE_typeTypeOrVoid);
		try {
			setState(274);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case BOOLEAN:
			case DECIMAL_NUMBER:
			case NUMBER:
			case STRING:
			case WHOLE_NUMBER:
			case IDENTIFIER:
				enterOuterAlt(_localctx, 1);
				{
				setState(272);
				typeType();
				}
				break;
			case VOID:
				enterOuterAlt(_localctx, 2);
				{
				setState(273);
				match(VOID);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class GenericMethodDeclarationContext extends ParserRuleContext {
		public TypeParametersContext typeParameters() {
			return getRuleContext(TypeParametersContext.class,0);
		}
		public MethodDeclarationContext methodDeclaration() {
			return getRuleContext(MethodDeclarationContext.class,0);
		}
		public GenericMethodDeclarationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_genericMethodDeclaration; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterGenericMethodDeclaration(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitGenericMethodDeclaration(this);
		}
	}

	public final GenericMethodDeclarationContext genericMethodDeclaration() throws RecognitionException {
		GenericMethodDeclarationContext _localctx = new GenericMethodDeclarationContext(_ctx, getState());
		enterRule(_localctx, 38, RULE_genericMethodDeclaration);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(276);
			typeParameters();
			setState(277);
			methodDeclaration();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class GenericConstructorDeclarationContext extends ParserRuleContext {
		public TypeParametersContext typeParameters() {
			return getRuleContext(TypeParametersContext.class,0);
		}
		public ConstructorDeclarationContext constructorDeclaration() {
			return getRuleContext(ConstructorDeclarationContext.class,0);
		}
		public GenericConstructorDeclarationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_genericConstructorDeclaration; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterGenericConstructorDeclaration(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitGenericConstructorDeclaration(this);
		}
	}

	public final GenericConstructorDeclarationContext genericConstructorDeclaration() throws RecognitionException {
		GenericConstructorDeclarationContext _localctx = new GenericConstructorDeclarationContext(_ctx, getState());
		enterRule(_localctx, 40, RULE_genericConstructorDeclaration);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(279);
			typeParameters();
			setState(280);
			constructorDeclaration();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ConstructorDeclarationContext extends ParserRuleContext {
		public BlockContext constructorBody;
		public TerminalNode IDENTIFIER() { return getToken(TweedleParser.IDENTIFIER, 0); }
		public FormalParametersContext formalParameters() {
			return getRuleContext(FormalParametersContext.class,0);
		}
		public BlockContext block() {
			return getRuleContext(BlockContext.class,0);
		}
		public ConstructorDeclarationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_constructorDeclaration; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterConstructorDeclaration(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitConstructorDeclaration(this);
		}
	}

	public final ConstructorDeclarationContext constructorDeclaration() throws RecognitionException {
		ConstructorDeclarationContext _localctx = new ConstructorDeclarationContext(_ctx, getState());
		enterRule(_localctx, 42, RULE_constructorDeclaration);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(282);
			match(IDENTIFIER);
			setState(283);
			formalParameters();
			setState(284);
			((ConstructorDeclarationContext)_localctx).constructorBody = block();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class FieldDeclarationContext extends ParserRuleContext {
		public TypeTypeContext typeType() {
			return getRuleContext(TypeTypeContext.class,0);
		}
		public VariableDeclaratorsContext variableDeclarators() {
			return getRuleContext(VariableDeclaratorsContext.class,0);
		}
		public FieldDeclarationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_fieldDeclaration; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterFieldDeclaration(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitFieldDeclaration(this);
		}
	}

	public final FieldDeclarationContext fieldDeclaration() throws RecognitionException {
		FieldDeclarationContext _localctx = new FieldDeclarationContext(_ctx, getState());
		enterRule(_localctx, 44, RULE_fieldDeclaration);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(286);
			typeType();
			setState(287);
			variableDeclarators();
			setState(288);
			match(SEMI);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class VariableDeclaratorsContext extends ParserRuleContext {
		public List<VariableDeclaratorContext> variableDeclarator() {
			return getRuleContexts(VariableDeclaratorContext.class);
		}
		public VariableDeclaratorContext variableDeclarator(int i) {
			return getRuleContext(VariableDeclaratorContext.class,i);
		}
		public VariableDeclaratorsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_variableDeclarators; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterVariableDeclarators(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitVariableDeclarators(this);
		}
	}

	public final VariableDeclaratorsContext variableDeclarators() throws RecognitionException {
		VariableDeclaratorsContext _localctx = new VariableDeclaratorsContext(_ctx, getState());
		enterRule(_localctx, 46, RULE_variableDeclarators);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(290);
			variableDeclarator();
			setState(295);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(291);
				match(COMMA);
				setState(292);
				variableDeclarator();
				}
				}
				setState(297);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class VariableDeclaratorContext extends ParserRuleContext {
		public VariableDeclaratorIdContext variableDeclaratorId() {
			return getRuleContext(VariableDeclaratorIdContext.class,0);
		}
		public TerminalNode LARROW() { return getToken(TweedleParser.LARROW, 0); }
		public VariableInitializerContext variableInitializer() {
			return getRuleContext(VariableInitializerContext.class,0);
		}
		public VariableDeclaratorContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_variableDeclarator; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterVariableDeclarator(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitVariableDeclarator(this);
		}
	}

	public final VariableDeclaratorContext variableDeclarator() throws RecognitionException {
		VariableDeclaratorContext _localctx = new VariableDeclaratorContext(_ctx, getState());
		enterRule(_localctx, 48, RULE_variableDeclarator);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(298);
			variableDeclaratorId();
			setState(301);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==LARROW) {
				{
				setState(299);
				match(LARROW);
				setState(300);
				variableInitializer();
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class VariableDeclaratorIdContext extends ParserRuleContext {
		public TerminalNode IDENTIFIER() { return getToken(TweedleParser.IDENTIFIER, 0); }
		public VariableDeclaratorIdContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_variableDeclaratorId; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterVariableDeclaratorId(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitVariableDeclaratorId(this);
		}
	}

	public final VariableDeclaratorIdContext variableDeclaratorId() throws RecognitionException {
		VariableDeclaratorIdContext _localctx = new VariableDeclaratorIdContext(_ctx, getState());
		enterRule(_localctx, 50, RULE_variableDeclaratorId);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(303);
			match(IDENTIFIER);
			setState(308);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==LBRACK) {
				{
				{
				setState(304);
				match(LBRACK);
				setState(305);
				match(RBRACK);
				}
				}
				setState(310);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class VariableInitializerContext extends ParserRuleContext {
		public ArrayInitializerContext arrayInitializer() {
			return getRuleContext(ArrayInitializerContext.class,0);
		}
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public VariableInitializerContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_variableInitializer; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterVariableInitializer(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitVariableInitializer(this);
		}
	}

	public final VariableInitializerContext variableInitializer() throws RecognitionException {
		VariableInitializerContext _localctx = new VariableInitializerContext(_ctx, getState());
		enterRule(_localctx, 52, RULE_variableInitializer);
		try {
			setState(313);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case LBRACE:
				enterOuterAlt(_localctx, 1);
				{
				setState(311);
				arrayInitializer();
				}
				break;
			case BOOLEAN:
			case DECIMAL_NUMBER:
			case NEW:
			case NUMBER:
			case STRING:
			case SUPER:
			case THIS:
			case VOID:
			case WHOLE_NUMBER:
			case DECIMAL_LITERAL:
			case FLOAT_LITERAL:
			case BOOL_LITERAL:
			case STRING_LITERAL:
			case NULL_LITERAL:
			case LPAREN:
			case LT:
			case BANG:
			case TILDE:
			case INC:
			case DEC:
			case ADD:
			case SUB:
			case IDENTIFIER:
				enterOuterAlt(_localctx, 2);
				{
				setState(312);
				expression(0);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ArrayInitializerContext extends ParserRuleContext {
		public List<VariableInitializerContext> variableInitializer() {
			return getRuleContexts(VariableInitializerContext.class);
		}
		public VariableInitializerContext variableInitializer(int i) {
			return getRuleContext(VariableInitializerContext.class,i);
		}
		public ArrayInitializerContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_arrayInitializer; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterArrayInitializer(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitArrayInitializer(this);
		}
	}

	public final ArrayInitializerContext arrayInitializer() throws RecognitionException {
		ArrayInitializerContext _localctx = new ArrayInitializerContext(_ctx, getState());
		enterRule(_localctx, 54, RULE_arrayInitializer);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(315);
			match(LBRACE);
			setState(327);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << BOOLEAN) | (1L << DECIMAL_NUMBER) | (1L << NEW) | (1L << NUMBER) | (1L << STRING) | (1L << SUPER) | (1L << THIS) | (1L << VOID) | (1L << WHOLE_NUMBER) | (1L << DECIMAL_LITERAL) | (1L << FLOAT_LITERAL) | (1L << BOOL_LITERAL) | (1L << STRING_LITERAL) | (1L << NULL_LITERAL) | (1L << LPAREN) | (1L << LBRACE) | (1L << LT) | (1L << BANG) | (1L << TILDE) | (1L << INC) | (1L << DEC) | (1L << ADD) | (1L << SUB))) != 0) || _la==IDENTIFIER) {
				{
				setState(316);
				variableInitializer();
				setState(321);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,26,_ctx);
				while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
					if ( _alt==1 ) {
						{
						{
						setState(317);
						match(COMMA);
						setState(318);
						variableInitializer();
						}
						} 
					}
					setState(323);
					_errHandler.sync(this);
					_alt = getInterpreter().adaptivePredict(_input,26,_ctx);
				}
				setState(325);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==COMMA) {
					{
					setState(324);
					match(COMMA);
					}
				}

				}
			}

			setState(329);
			match(RBRACE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ClassOrInterfaceTypeContext extends ParserRuleContext {
		public TerminalNode IDENTIFIER() { return getToken(TweedleParser.IDENTIFIER, 0); }
		public ClassOrInterfaceTypeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_classOrInterfaceType; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterClassOrInterfaceType(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitClassOrInterfaceType(this);
		}
	}

	public final ClassOrInterfaceTypeContext classOrInterfaceType() throws RecognitionException {
		ClassOrInterfaceTypeContext _localctx = new ClassOrInterfaceTypeContext(_ctx, getState());
		enterRule(_localctx, 56, RULE_classOrInterfaceType);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(331);
			match(IDENTIFIER);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class TypeArgumentContext extends ParserRuleContext {
		public TypeTypeContext typeType() {
			return getRuleContext(TypeTypeContext.class,0);
		}
		public TerminalNode EXTENDS() { return getToken(TweedleParser.EXTENDS, 0); }
		public TerminalNode SUPER() { return getToken(TweedleParser.SUPER, 0); }
		public TypeArgumentContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_typeArgument; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterTypeArgument(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitTypeArgument(this);
		}
	}

	public final TypeArgumentContext typeArgument() throws RecognitionException {
		TypeArgumentContext _localctx = new TypeArgumentContext(_ctx, getState());
		enterRule(_localctx, 58, RULE_typeArgument);
		int _la;
		try {
			setState(339);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case BOOLEAN:
			case DECIMAL_NUMBER:
			case NUMBER:
			case STRING:
			case WHOLE_NUMBER:
			case IDENTIFIER:
				enterOuterAlt(_localctx, 1);
				{
				setState(333);
				typeType();
				}
				break;
			case QUESTION:
				enterOuterAlt(_localctx, 2);
				{
				setState(334);
				match(QUESTION);
				setState(337);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==EXTENDS || _la==SUPER) {
					{
					setState(335);
					_la = _input.LA(1);
					if ( !(_la==EXTENDS || _la==SUPER) ) {
					_errHandler.recoverInline(this);
					}
					else {
						if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
						_errHandler.reportMatch(this);
						consume();
					}
					setState(336);
					typeType();
					}
				}

				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class FormalParametersContext extends ParserRuleContext {
		public FormalParameterListContext formalParameterList() {
			return getRuleContext(FormalParameterListContext.class,0);
		}
		public FormalParametersContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_formalParameters; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterFormalParameters(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitFormalParameters(this);
		}
	}

	public final FormalParametersContext formalParameters() throws RecognitionException {
		FormalParametersContext _localctx = new FormalParametersContext(_ctx, getState());
		enterRule(_localctx, 60, RULE_formalParameters);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(341);
			match(LPAREN);
			setState(343);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << BOOLEAN) | (1L << CONSTANT) | (1L << DECIMAL_NUMBER) | (1L << NUMBER) | (1L << STRING) | (1L << WHOLE_NUMBER))) != 0) || _la==IDENTIFIER) {
				{
				setState(342);
				formalParameterList();
				}
			}

			setState(345);
			match(RPAREN);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class FormalParameterListContext extends ParserRuleContext {
		public List<RequiredParameterContext> requiredParameter() {
			return getRuleContexts(RequiredParameterContext.class);
		}
		public RequiredParameterContext requiredParameter(int i) {
			return getRuleContext(RequiredParameterContext.class,i);
		}
		public List<OptionalParameterContext> optionalParameter() {
			return getRuleContexts(OptionalParameterContext.class);
		}
		public OptionalParameterContext optionalParameter(int i) {
			return getRuleContext(OptionalParameterContext.class,i);
		}
		public LastFormalParameterContext lastFormalParameter() {
			return getRuleContext(LastFormalParameterContext.class,0);
		}
		public FormalParameterListContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_formalParameterList; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterFormalParameterList(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitFormalParameterList(this);
		}
	}

	public final FormalParameterListContext formalParameterList() throws RecognitionException {
		FormalParameterListContext _localctx = new FormalParameterListContext(_ctx, getState());
		enterRule(_localctx, 62, RULE_formalParameterList);
		int _la;
		try {
			int _alt;
			setState(379);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,37,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(347);
				requiredParameter();
				setState(352);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,32,_ctx);
				while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
					if ( _alt==1 ) {
						{
						{
						setState(348);
						match(COMMA);
						setState(349);
						requiredParameter();
						}
						} 
					}
					setState(354);
					_errHandler.sync(this);
					_alt = getInterpreter().adaptivePredict(_input,32,_ctx);
				}
				setState(359);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,33,_ctx);
				while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
					if ( _alt==1 ) {
						{
						{
						setState(355);
						match(COMMA);
						setState(356);
						optionalParameter();
						}
						} 
					}
					setState(361);
					_errHandler.sync(this);
					_alt = getInterpreter().adaptivePredict(_input,33,_ctx);
				}
				setState(364);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==COMMA) {
					{
					setState(362);
					match(COMMA);
					setState(363);
					lastFormalParameter();
					}
				}

				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(366);
				optionalParameter();
				setState(371);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,35,_ctx);
				while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
					if ( _alt==1 ) {
						{
						{
						setState(367);
						match(COMMA);
						setState(368);
						optionalParameter();
						}
						} 
					}
					setState(373);
					_errHandler.sync(this);
					_alt = getInterpreter().adaptivePredict(_input,35,_ctx);
				}
				setState(376);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==COMMA) {
					{
					setState(374);
					match(COMMA);
					setState(375);
					lastFormalParameter();
					}
				}

				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(378);
				lastFormalParameter();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class RequiredParameterContext extends ParserRuleContext {
		public TypeTypeContext typeType() {
			return getRuleContext(TypeTypeContext.class,0);
		}
		public VariableDeclaratorIdContext variableDeclaratorId() {
			return getRuleContext(VariableDeclaratorIdContext.class,0);
		}
		public List<VariableModifierContext> variableModifier() {
			return getRuleContexts(VariableModifierContext.class);
		}
		public VariableModifierContext variableModifier(int i) {
			return getRuleContext(VariableModifierContext.class,i);
		}
		public RequiredParameterContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_requiredParameter; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterRequiredParameter(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitRequiredParameter(this);
		}
	}

	public final RequiredParameterContext requiredParameter() throws RecognitionException {
		RequiredParameterContext _localctx = new RequiredParameterContext(_ctx, getState());
		enterRule(_localctx, 64, RULE_requiredParameter);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(384);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==CONSTANT) {
				{
				{
				setState(381);
				variableModifier();
				}
				}
				setState(386);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(387);
			typeType();
			setState(388);
			variableDeclaratorId();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class OptionalParameterContext extends ParserRuleContext {
		public TypeTypeContext typeType() {
			return getRuleContext(TypeTypeContext.class,0);
		}
		public VariableDeclaratorIdContext variableDeclaratorId() {
			return getRuleContext(VariableDeclaratorIdContext.class,0);
		}
		public TerminalNode LARROW() { return getToken(TweedleParser.LARROW, 0); }
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public List<VariableModifierContext> variableModifier() {
			return getRuleContexts(VariableModifierContext.class);
		}
		public VariableModifierContext variableModifier(int i) {
			return getRuleContext(VariableModifierContext.class,i);
		}
		public OptionalParameterContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_optionalParameter; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterOptionalParameter(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitOptionalParameter(this);
		}
	}

	public final OptionalParameterContext optionalParameter() throws RecognitionException {
		OptionalParameterContext _localctx = new OptionalParameterContext(_ctx, getState());
		enterRule(_localctx, 66, RULE_optionalParameter);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(393);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==CONSTANT) {
				{
				{
				setState(390);
				variableModifier();
				}
				}
				setState(395);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(396);
			typeType();
			setState(397);
			variableDeclaratorId();
			setState(398);
			match(LARROW);
			setState(399);
			expression(0);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class LastFormalParameterContext extends ParserRuleContext {
		public TypeTypeContext typeType() {
			return getRuleContext(TypeTypeContext.class,0);
		}
		public VariableDeclaratorIdContext variableDeclaratorId() {
			return getRuleContext(VariableDeclaratorIdContext.class,0);
		}
		public List<VariableModifierContext> variableModifier() {
			return getRuleContexts(VariableModifierContext.class);
		}
		public VariableModifierContext variableModifier(int i) {
			return getRuleContext(VariableModifierContext.class,i);
		}
		public LastFormalParameterContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_lastFormalParameter; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterLastFormalParameter(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitLastFormalParameter(this);
		}
	}

	public final LastFormalParameterContext lastFormalParameter() throws RecognitionException {
		LastFormalParameterContext _localctx = new LastFormalParameterContext(_ctx, getState());
		enterRule(_localctx, 68, RULE_lastFormalParameter);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(404);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==CONSTANT) {
				{
				{
				setState(401);
				variableModifier();
				}
				}
				setState(406);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(407);
			typeType();
			setState(408);
			match(ELLIPSIS);
			setState(409);
			variableDeclaratorId();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class LiteralContext extends ParserRuleContext {
		public TerminalNode DECIMAL_LITERAL() { return getToken(TweedleParser.DECIMAL_LITERAL, 0); }
		public TerminalNode FLOAT_LITERAL() { return getToken(TweedleParser.FLOAT_LITERAL, 0); }
		public TerminalNode STRING_LITERAL() { return getToken(TweedleParser.STRING_LITERAL, 0); }
		public TerminalNode BOOL_LITERAL() { return getToken(TweedleParser.BOOL_LITERAL, 0); }
		public TerminalNode NULL_LITERAL() { return getToken(TweedleParser.NULL_LITERAL, 0); }
		public LiteralContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_literal; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterLiteral(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitLiteral(this);
		}
	}

	public final LiteralContext literal() throws RecognitionException {
		LiteralContext _localctx = new LiteralContext(_ctx, getState());
		enterRule(_localctx, 70, RULE_literal);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(411);
			_la = _input.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << DECIMAL_LITERAL) | (1L << FLOAT_LITERAL) | (1L << BOOL_LITERAL) | (1L << STRING_LITERAL) | (1L << NULL_LITERAL))) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class BlockContext extends ParserRuleContext {
		public List<BlockStatementContext> blockStatement() {
			return getRuleContexts(BlockStatementContext.class);
		}
		public BlockStatementContext blockStatement(int i) {
			return getRuleContext(BlockStatementContext.class,i);
		}
		public BlockContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_block; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterBlock(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitBlock(this);
		}
	}

	public final BlockContext block() throws RecognitionException {
		BlockContext _localctx = new BlockContext(_ctx, getState());
		enterRule(_localctx, 72, RULE_block);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(413);
			match(LBRACE);
			setState(417);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << BOOLEAN) | (1L << COUNT_UP_TO) | (1L << CONSTANT) | (1L << DECIMAL_NUMBER) | (1L << DO_IN_ORDER) | (1L << DO_TOGETHER) | (1L << FOR_EACH) | (1L << EACH_TOGETHER) | (1L << IF) | (1L << NEW) | (1L << NUMBER) | (1L << RETURN) | (1L << STRING) | (1L << SUPER) | (1L << THIS) | (1L << VOID) | (1L << WHILE) | (1L << WHOLE_NUMBER) | (1L << DECIMAL_LITERAL) | (1L << FLOAT_LITERAL) | (1L << BOOL_LITERAL) | (1L << STRING_LITERAL) | (1L << NULL_LITERAL) | (1L << LPAREN) | (1L << LBRACE) | (1L << LT) | (1L << BANG) | (1L << TILDE) | (1L << INC) | (1L << DEC) | (1L << ADD) | (1L << SUB))) != 0) || _la==NODE_COMMENT || _la==IDENTIFIER) {
				{
				{
				setState(414);
				blockStatement();
				}
				}
				setState(419);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(420);
			match(RBRACE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class BlockStatementContext extends ParserRuleContext {
		public LocalVariableDeclarationContext localVariableDeclaration() {
			return getRuleContext(LocalVariableDeclarationContext.class,0);
		}
		public StatementContext statement() {
			return getRuleContext(StatementContext.class,0);
		}
		public BlockStatementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_blockStatement; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterBlockStatement(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitBlockStatement(this);
		}
	}

	public final BlockStatementContext blockStatement() throws RecognitionException {
		BlockStatementContext _localctx = new BlockStatementContext(_ctx, getState());
		enterRule(_localctx, 74, RULE_blockStatement);
		try {
			setState(426);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,42,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(422);
				localVariableDeclaration();
				setState(423);
				match(SEMI);
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(425);
				statement();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class LocalVariableDeclarationContext extends ParserRuleContext {
		public TypeTypeContext typeType() {
			return getRuleContext(TypeTypeContext.class,0);
		}
		public VariableDeclaratorsContext variableDeclarators() {
			return getRuleContext(VariableDeclaratorsContext.class,0);
		}
		public TerminalNode NODE_COMMENT() { return getToken(TweedleParser.NODE_COMMENT, 0); }
		public List<VariableModifierContext> variableModifier() {
			return getRuleContexts(VariableModifierContext.class);
		}
		public VariableModifierContext variableModifier(int i) {
			return getRuleContext(VariableModifierContext.class,i);
		}
		public LocalVariableDeclarationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_localVariableDeclaration; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterLocalVariableDeclaration(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitLocalVariableDeclaration(this);
		}
	}

	public final LocalVariableDeclarationContext localVariableDeclaration() throws RecognitionException {
		LocalVariableDeclarationContext _localctx = new LocalVariableDeclarationContext(_ctx, getState());
		enterRule(_localctx, 76, RULE_localVariableDeclaration);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(429);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NODE_COMMENT) {
				{
				setState(428);
				match(NODE_COMMENT);
				}
			}

			setState(434);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==CONSTANT) {
				{
				{
				setState(431);
				variableModifier();
				}
				}
				setState(436);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(437);
			typeType();
			setState(438);
			variableDeclarators();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class StatementContext extends ParserRuleContext {
		public BlockContext blockLabel;
		public ExpressionContext statementExpression;
		public Token identifierLabel;
		public BlockContext block() {
			return getRuleContext(BlockContext.class,0);
		}
		public TerminalNode COUNT_UP_TO() { return getToken(TweedleParser.COUNT_UP_TO, 0); }
		public List<StatementContext> statement() {
			return getRuleContexts(StatementContext.class);
		}
		public StatementContext statement(int i) {
			return getRuleContext(StatementContext.class,i);
		}
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public TerminalNode IF() { return getToken(TweedleParser.IF, 0); }
		public ParExpressionContext parExpression() {
			return getRuleContext(ParExpressionContext.class,0);
		}
		public TerminalNode ELSE() { return getToken(TweedleParser.ELSE, 0); }
		public TerminalNode FOR_EACH() { return getToken(TweedleParser.FOR_EACH, 0); }
		public ForControlContext forControl() {
			return getRuleContext(ForControlContext.class,0);
		}
		public TerminalNode EACH_TOGETHER() { return getToken(TweedleParser.EACH_TOGETHER, 0); }
		public TerminalNode WHILE() { return getToken(TweedleParser.WHILE, 0); }
		public TerminalNode DO_IN_ORDER() { return getToken(TweedleParser.DO_IN_ORDER, 0); }
		public TerminalNode DO_TOGETHER() { return getToken(TweedleParser.DO_TOGETHER, 0); }
		public TerminalNode RETURN() { return getToken(TweedleParser.RETURN, 0); }
		public TerminalNode IDENTIFIER() { return getToken(TweedleParser.IDENTIFIER, 0); }
		public TerminalNode NODE_COMMENT() { return getToken(TweedleParser.NODE_COMMENT, 0); }
		public StatementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_statement; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterStatement(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitStatement(this);
		}
	}

	public final StatementContext statement() throws RecognitionException {
		StatementContext _localctx = new StatementContext(_ctx, getState());
		enterRule(_localctx, 78, RULE_statement);
		int _la;
		try {
			setState(485);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,47,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(440);
				((StatementContext)_localctx).blockLabel = block();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(441);
				match(COUNT_UP_TO);
				{
				setState(442);
				expression(0);
				}
				setState(443);
				statement();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(445);
				match(IF);
				setState(446);
				parExpression();
				setState(447);
				statement();
				setState(450);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,45,_ctx) ) {
				case 1:
					{
					setState(448);
					match(ELSE);
					setState(449);
					statement();
					}
					break;
				}
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(452);
				match(FOR_EACH);
				setState(453);
				match(LPAREN);
				setState(454);
				forControl();
				setState(455);
				match(RPAREN);
				setState(456);
				statement();
				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(458);
				match(EACH_TOGETHER);
				setState(459);
				match(LPAREN);
				setState(460);
				forControl();
				setState(461);
				match(RPAREN);
				setState(462);
				statement();
				}
				break;
			case 6:
				enterOuterAlt(_localctx, 6);
				{
				setState(464);
				match(WHILE);
				setState(465);
				parExpression();
				setState(466);
				statement();
				}
				break;
			case 7:
				enterOuterAlt(_localctx, 7);
				{
				setState(468);
				match(DO_IN_ORDER);
				setState(469);
				block();
				}
				break;
			case 8:
				enterOuterAlt(_localctx, 8);
				{
				setState(470);
				match(DO_TOGETHER);
				setState(471);
				block();
				}
				break;
			case 9:
				enterOuterAlt(_localctx, 9);
				{
				setState(472);
				match(RETURN);
				setState(474);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << BOOLEAN) | (1L << DECIMAL_NUMBER) | (1L << NEW) | (1L << NUMBER) | (1L << STRING) | (1L << SUPER) | (1L << THIS) | (1L << VOID) | (1L << WHOLE_NUMBER) | (1L << DECIMAL_LITERAL) | (1L << FLOAT_LITERAL) | (1L << BOOL_LITERAL) | (1L << STRING_LITERAL) | (1L << NULL_LITERAL) | (1L << LPAREN) | (1L << LT) | (1L << BANG) | (1L << TILDE) | (1L << INC) | (1L << DEC) | (1L << ADD) | (1L << SUB))) != 0) || _la==IDENTIFIER) {
					{
					setState(473);
					expression(0);
					}
				}

				setState(476);
				match(SEMI);
				}
				break;
			case 10:
				enterOuterAlt(_localctx, 10);
				{
				setState(477);
				((StatementContext)_localctx).statementExpression = expression(0);
				setState(478);
				match(SEMI);
				}
				break;
			case 11:
				enterOuterAlt(_localctx, 11);
				{
				setState(480);
				((StatementContext)_localctx).identifierLabel = match(IDENTIFIER);
				setState(481);
				match(COLON);
				setState(482);
				statement();
				}
				break;
			case 12:
				enterOuterAlt(_localctx, 12);
				{
				setState(483);
				match(NODE_COMMENT);
				setState(484);
				statement();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ForControlContext extends ParserRuleContext {
		public TypeTypeContext typeType() {
			return getRuleContext(TypeTypeContext.class,0);
		}
		public VariableDeclaratorIdContext variableDeclaratorId() {
			return getRuleContext(VariableDeclaratorIdContext.class,0);
		}
		public TerminalNode IN() { return getToken(TweedleParser.IN, 0); }
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public ForControlContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_forControl; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterForControl(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitForControl(this);
		}
	}

	public final ForControlContext forControl() throws RecognitionException {
		ForControlContext _localctx = new ForControlContext(_ctx, getState());
		enterRule(_localctx, 80, RULE_forControl);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(487);
			typeType();
			setState(488);
			variableDeclaratorId();
			setState(489);
			match(IN);
			setState(490);
			expression(0);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ParExpressionContext extends ParserRuleContext {
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public ParExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_parExpression; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterParExpression(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitParExpression(this);
		}
	}

	public final ParExpressionContext parExpression() throws RecognitionException {
		ParExpressionContext _localctx = new ParExpressionContext(_ctx, getState());
		enterRule(_localctx, 82, RULE_parExpression);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(492);
			match(LPAREN);
			setState(493);
			expression(0);
			setState(494);
			match(RPAREN);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class LabeledExpressionListContext extends ParserRuleContext {
		public List<LabeledExpressionContext> labeledExpression() {
			return getRuleContexts(LabeledExpressionContext.class);
		}
		public LabeledExpressionContext labeledExpression(int i) {
			return getRuleContext(LabeledExpressionContext.class,i);
		}
		public LabeledExpressionListContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_labeledExpressionList; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterLabeledExpressionList(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitLabeledExpressionList(this);
		}
	}

	public final LabeledExpressionListContext labeledExpressionList() throws RecognitionException {
		LabeledExpressionListContext _localctx = new LabeledExpressionListContext(_ctx, getState());
		enterRule(_localctx, 84, RULE_labeledExpressionList);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(496);
			labeledExpression();
			setState(501);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(497);
				match(COMMA);
				setState(498);
				labeledExpression();
				}
				}
				setState(503);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class LabeledExpressionContext extends ParserRuleContext {
		public Token expressionLabel;
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public TerminalNode IDENTIFIER() { return getToken(TweedleParser.IDENTIFIER, 0); }
		public LabeledExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_labeledExpression; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterLabeledExpression(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitLabeledExpression(this);
		}
	}

	public final LabeledExpressionContext labeledExpression() throws RecognitionException {
		LabeledExpressionContext _localctx = new LabeledExpressionContext(_ctx, getState());
		enterRule(_localctx, 86, RULE_labeledExpression);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(504);
			((LabeledExpressionContext)_localctx).expressionLabel = match(IDENTIFIER);
			setState(505);
			match(COLON);
			setState(506);
			expression(0);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ExpressionListContext extends ParserRuleContext {
		public List<ExpressionContext> expression() {
			return getRuleContexts(ExpressionContext.class);
		}
		public ExpressionContext expression(int i) {
			return getRuleContext(ExpressionContext.class,i);
		}
		public ExpressionListContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_expressionList; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterExpressionList(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitExpressionList(this);
		}
	}

	public final ExpressionListContext expressionList() throws RecognitionException {
		ExpressionListContext _localctx = new ExpressionListContext(_ctx, getState());
		enterRule(_localctx, 88, RULE_expressionList);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(508);
			expression(0);
			setState(513);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(509);
				match(COMMA);
				setState(510);
				expression(0);
				}
				}
				setState(515);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class MethodCallContext extends ParserRuleContext {
		public TerminalNode IDENTIFIER() { return getToken(TweedleParser.IDENTIFIER, 0); }
		public LabeledExpressionListContext labeledExpressionList() {
			return getRuleContext(LabeledExpressionListContext.class,0);
		}
		public MethodCallContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_methodCall; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterMethodCall(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitMethodCall(this);
		}
	}

	public final MethodCallContext methodCall() throws RecognitionException {
		MethodCallContext _localctx = new MethodCallContext(_ctx, getState());
		enterRule(_localctx, 90, RULE_methodCall);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(516);
			match(IDENTIFIER);
			setState(517);
			match(LPAREN);
			setState(519);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==IDENTIFIER) {
				{
				setState(518);
				labeledExpressionList();
				}
			}

			setState(521);
			match(RPAREN);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ExpressionContext extends ParserRuleContext {
		public Token prefix;
		public Token bop;
		public Token postfix;
		public PrimaryContext primary() {
			return getRuleContext(PrimaryContext.class,0);
		}
		public MethodCallContext methodCall() {
			return getRuleContext(MethodCallContext.class,0);
		}
		public TerminalNode NEW() { return getToken(TweedleParser.NEW, 0); }
		public CreatorContext creator() {
			return getRuleContext(CreatorContext.class,0);
		}
		public TypeTypeContext typeType() {
			return getRuleContext(TypeTypeContext.class,0);
		}
		public List<ExpressionContext> expression() {
			return getRuleContexts(ExpressionContext.class);
		}
		public ExpressionContext expression(int i) {
			return getRuleContext(ExpressionContext.class,i);
		}
		public LambdaExpressionContext lambdaExpression() {
			return getRuleContext(LambdaExpressionContext.class,0);
		}
		public TerminalNode IDENTIFIER() { return getToken(TweedleParser.IDENTIFIER, 0); }
		public TypeArgumentsContext typeArguments() {
			return getRuleContext(TypeArgumentsContext.class,0);
		}
		public ClassTypeContext classType() {
			return getRuleContext(ClassTypeContext.class,0);
		}
		public TerminalNode LARROW() { return getToken(TweedleParser.LARROW, 0); }
		public TerminalNode THIS() { return getToken(TweedleParser.THIS, 0); }
		public InnerCreatorContext innerCreator() {
			return getRuleContext(InnerCreatorContext.class,0);
		}
		public TerminalNode SUPER() { return getToken(TweedleParser.SUPER, 0); }
		public SuperSuffixContext superSuffix() {
			return getRuleContext(SuperSuffixContext.class,0);
		}
		public ExplicitGenericInvocationContext explicitGenericInvocation() {
			return getRuleContext(ExplicitGenericInvocationContext.class,0);
		}
		public NonWildcardTypeArgumentsContext nonWildcardTypeArguments() {
			return getRuleContext(NonWildcardTypeArgumentsContext.class,0);
		}
		public ExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_expression; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterExpression(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitExpression(this);
		}
	}

	public final ExpressionContext expression() throws RecognitionException {
		return expression(0);
	}

	private ExpressionContext expression(int _p) throws RecognitionException {
		ParserRuleContext _parentctx = _ctx;
		int _parentState = getState();
		ExpressionContext _localctx = new ExpressionContext(_ctx, _parentState);
		ExpressionContext _prevctx = _localctx;
		int _startState = 92;
		enterRecursionRule(_localctx, 92, RULE_expression, _p);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(554);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,54,_ctx) ) {
			case 1:
				{
				setState(524);
				primary();
				}
				break;
			case 2:
				{
				setState(525);
				methodCall();
				}
				break;
			case 3:
				{
				setState(526);
				match(NEW);
				setState(527);
				creator();
				}
				break;
			case 4:
				{
				setState(528);
				match(LPAREN);
				setState(529);
				typeType();
				setState(530);
				match(RPAREN);
				setState(531);
				expression(20);
				}
				break;
			case 5:
				{
				setState(533);
				((ExpressionContext)_localctx).prefix = _input.LT(1);
				_la = _input.LA(1);
				if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << INC) | (1L << DEC) | (1L << ADD) | (1L << SUB))) != 0)) ) {
					((ExpressionContext)_localctx).prefix = (Token)_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				setState(534);
				expression(18);
				}
				break;
			case 6:
				{
				setState(535);
				((ExpressionContext)_localctx).prefix = _input.LT(1);
				_la = _input.LA(1);
				if ( !(_la==BANG || _la==TILDE) ) {
					((ExpressionContext)_localctx).prefix = (Token)_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				setState(536);
				expression(17);
				}
				break;
			case 7:
				{
				setState(537);
				lambdaExpression();
				}
				break;
			case 8:
				{
				setState(538);
				typeType();
				setState(539);
				match(COLONCOLON);
				setState(545);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case LT:
				case IDENTIFIER:
					{
					setState(541);
					_errHandler.sync(this);
					_la = _input.LA(1);
					if (_la==LT) {
						{
						setState(540);
						typeArguments();
						}
					}

					setState(543);
					match(IDENTIFIER);
					}
					break;
				case NEW:
					{
					setState(544);
					match(NEW);
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				}
				break;
			case 9:
				{
				setState(547);
				classType();
				setState(548);
				match(COLONCOLON);
				setState(550);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==LT) {
					{
					setState(549);
					typeArguments();
					}
				}

				setState(552);
				match(NEW);
				}
				break;
			}
			_ctx.stop = _input.LT(-1);
			setState(633);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,60,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) triggerExitRuleEvent();
					_prevctx = _localctx;
					{
					setState(631);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,59,_ctx) ) {
					case 1:
						{
						_localctx = new ExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(556);
						if (!(precpred(_ctx, 16))) throw new FailedPredicateException(this, "precpred(_ctx, 16)");
						setState(557);
						((ExpressionContext)_localctx).bop = _input.LT(1);
						_la = _input.LA(1);
						if ( !(((((_la - 62)) & ~0x3f) == 0 && ((1L << (_la - 62)) & ((1L << (MUL - 62)) | (1L << (DIV - 62)) | (1L << (MOD - 62)))) != 0)) ) {
							((ExpressionContext)_localctx).bop = (Token)_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(558);
						expression(17);
						}
						break;
					case 2:
						{
						_localctx = new ExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(559);
						if (!(precpred(_ctx, 15))) throw new FailedPredicateException(this, "precpred(_ctx, 15)");
						setState(560);
						((ExpressionContext)_localctx).bop = _input.LT(1);
						_la = _input.LA(1);
						if ( !(_la==ADD || _la==SUB) ) {
							((ExpressionContext)_localctx).bop = (Token)_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(561);
						expression(16);
						}
						break;
					case 3:
						{
						_localctx = new ExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(562);
						if (!(precpred(_ctx, 14))) throw new FailedPredicateException(this, "precpred(_ctx, 14)");
						setState(570);
						_errHandler.sync(this);
						switch ( getInterpreter().adaptivePredict(_input,55,_ctx) ) {
						case 1:
							{
							setState(563);
							match(LT);
							setState(564);
							match(LT);
							}
							break;
						case 2:
							{
							setState(565);
							match(GT);
							setState(566);
							match(GT);
							setState(567);
							match(GT);
							}
							break;
						case 3:
							{
							setState(568);
							match(GT);
							setState(569);
							match(GT);
							}
							break;
						}
						setState(572);
						expression(15);
						}
						break;
					case 4:
						{
						_localctx = new ExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(573);
						if (!(precpred(_ctx, 13))) throw new FailedPredicateException(this, "precpred(_ctx, 13)");
						setState(574);
						((ExpressionContext)_localctx).bop = _input.LT(1);
						_la = _input.LA(1);
						if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << GT) | (1L << LT) | (1L << LE) | (1L << GE))) != 0)) ) {
							((ExpressionContext)_localctx).bop = (Token)_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(575);
						expression(14);
						}
						break;
					case 5:
						{
						_localctx = new ExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(576);
						if (!(precpred(_ctx, 12))) throw new FailedPredicateException(this, "precpred(_ctx, 12)");
						setState(577);
						((ExpressionContext)_localctx).bop = _input.LT(1);
						_la = _input.LA(1);
						if ( !(_la==EQUAL || _la==NOTEQUAL) ) {
							((ExpressionContext)_localctx).bop = (Token)_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(578);
						expression(13);
						}
						break;
					case 6:
						{
						_localctx = new ExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(579);
						if (!(precpred(_ctx, 11))) throw new FailedPredicateException(this, "precpred(_ctx, 11)");
						setState(580);
						((ExpressionContext)_localctx).bop = match(BITAND);
						setState(581);
						expression(12);
						}
						break;
					case 7:
						{
						_localctx = new ExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(582);
						if (!(precpred(_ctx, 10))) throw new FailedPredicateException(this, "precpred(_ctx, 10)");
						setState(583);
						((ExpressionContext)_localctx).bop = match(CARET);
						setState(584);
						expression(11);
						}
						break;
					case 8:
						{
						_localctx = new ExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(585);
						if (!(precpred(_ctx, 9))) throw new FailedPredicateException(this, "precpred(_ctx, 9)");
						setState(586);
						((ExpressionContext)_localctx).bop = match(BITOR);
						setState(587);
						expression(10);
						}
						break;
					case 9:
						{
						_localctx = new ExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(588);
						if (!(precpred(_ctx, 8))) throw new FailedPredicateException(this, "precpred(_ctx, 8)");
						setState(589);
						((ExpressionContext)_localctx).bop = match(AND);
						setState(590);
						expression(9);
						}
						break;
					case 10:
						{
						_localctx = new ExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(591);
						if (!(precpred(_ctx, 7))) throw new FailedPredicateException(this, "precpred(_ctx, 7)");
						setState(592);
						((ExpressionContext)_localctx).bop = match(OR);
						setState(593);
						expression(8);
						}
						break;
					case 11:
						{
						_localctx = new ExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(594);
						if (!(precpred(_ctx, 6))) throw new FailedPredicateException(this, "precpred(_ctx, 6)");
						setState(595);
						((ExpressionContext)_localctx).bop = match(QUESTION);
						setState(596);
						expression(0);
						setState(597);
						match(COLON);
						setState(598);
						expression(7);
						}
						break;
					case 12:
						{
						_localctx = new ExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(600);
						if (!(precpred(_ctx, 5))) throw new FailedPredicateException(this, "precpred(_ctx, 5)");
						setState(601);
						match(LARROW);
						setState(602);
						expression(5);
						}
						break;
					case 13:
						{
						_localctx = new ExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(603);
						if (!(precpred(_ctx, 24))) throw new FailedPredicateException(this, "precpred(_ctx, 24)");
						setState(604);
						((ExpressionContext)_localctx).bop = match(DOT);
						setState(616);
						_errHandler.sync(this);
						switch ( getInterpreter().adaptivePredict(_input,57,_ctx) ) {
						case 1:
							{
							setState(605);
							match(IDENTIFIER);
							}
							break;
						case 2:
							{
							setState(606);
							methodCall();
							}
							break;
						case 3:
							{
							setState(607);
							match(THIS);
							}
							break;
						case 4:
							{
							setState(608);
							match(NEW);
							setState(610);
							_errHandler.sync(this);
							_la = _input.LA(1);
							if (_la==LT) {
								{
								setState(609);
								nonWildcardTypeArguments();
								}
							}

							setState(612);
							innerCreator();
							}
							break;
						case 5:
							{
							setState(613);
							match(SUPER);
							setState(614);
							superSuffix();
							}
							break;
						case 6:
							{
							setState(615);
							explicitGenericInvocation();
							}
							break;
						}
						}
						break;
					case 14:
						{
						_localctx = new ExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(618);
						if (!(precpred(_ctx, 23))) throw new FailedPredicateException(this, "precpred(_ctx, 23)");
						setState(619);
						match(LBRACK);
						setState(620);
						expression(0);
						setState(621);
						match(RBRACK);
						}
						break;
					case 15:
						{
						_localctx = new ExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(623);
						if (!(precpred(_ctx, 19))) throw new FailedPredicateException(this, "precpred(_ctx, 19)");
						setState(624);
						((ExpressionContext)_localctx).postfix = _input.LT(1);
						_la = _input.LA(1);
						if ( !(_la==INC || _la==DEC) ) {
							((ExpressionContext)_localctx).postfix = (Token)_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						}
						break;
					case 16:
						{
						_localctx = new ExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(625);
						if (!(precpred(_ctx, 3))) throw new FailedPredicateException(this, "precpred(_ctx, 3)");
						setState(626);
						match(COLONCOLON);
						setState(628);
						_errHandler.sync(this);
						_la = _input.LA(1);
						if (_la==LT) {
							{
							setState(627);
							typeArguments();
							}
						}

						setState(630);
						match(IDENTIFIER);
						}
						break;
					}
					} 
				}
				setState(635);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,60,_ctx);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			unrollRecursionContexts(_parentctx);
		}
		return _localctx;
	}

	public static class LambdaExpressionContext extends ParserRuleContext {
		public LambdaParametersContext lambdaParameters() {
			return getRuleContext(LambdaParametersContext.class,0);
		}
		public LambdaBodyContext lambdaBody() {
			return getRuleContext(LambdaBodyContext.class,0);
		}
		public LambdaExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_lambdaExpression; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterLambdaExpression(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitLambdaExpression(this);
		}
	}

	public final LambdaExpressionContext lambdaExpression() throws RecognitionException {
		LambdaExpressionContext _localctx = new LambdaExpressionContext(_ctx, getState());
		enterRule(_localctx, 94, RULE_lambdaExpression);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(636);
			lambdaParameters();
			setState(637);
			match(ARROW);
			setState(638);
			lambdaBody();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class LambdaParametersContext extends ParserRuleContext {
		public List<TerminalNode> IDENTIFIER() { return getTokens(TweedleParser.IDENTIFIER); }
		public TerminalNode IDENTIFIER(int i) {
			return getToken(TweedleParser.IDENTIFIER, i);
		}
		public FormalParameterListContext formalParameterList() {
			return getRuleContext(FormalParameterListContext.class,0);
		}
		public LambdaParametersContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_lambdaParameters; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterLambdaParameters(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitLambdaParameters(this);
		}
	}

	public final LambdaParametersContext lambdaParameters() throws RecognitionException {
		LambdaParametersContext _localctx = new LambdaParametersContext(_ctx, getState());
		enterRule(_localctx, 96, RULE_lambdaParameters);
		int _la;
		try {
			setState(656);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,63,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(640);
				match(IDENTIFIER);
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(641);
				match(LPAREN);
				setState(643);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << BOOLEAN) | (1L << CONSTANT) | (1L << DECIMAL_NUMBER) | (1L << NUMBER) | (1L << STRING) | (1L << WHOLE_NUMBER))) != 0) || _la==IDENTIFIER) {
					{
					setState(642);
					formalParameterList();
					}
				}

				setState(645);
				match(RPAREN);
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(646);
				match(LPAREN);
				setState(647);
				match(IDENTIFIER);
				setState(652);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==COMMA) {
					{
					{
					setState(648);
					match(COMMA);
					setState(649);
					match(IDENTIFIER);
					}
					}
					setState(654);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				setState(655);
				match(RPAREN);
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class LambdaBodyContext extends ParserRuleContext {
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public BlockContext block() {
			return getRuleContext(BlockContext.class,0);
		}
		public LambdaBodyContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_lambdaBody; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterLambdaBody(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitLambdaBody(this);
		}
	}

	public final LambdaBodyContext lambdaBody() throws RecognitionException {
		LambdaBodyContext _localctx = new LambdaBodyContext(_ctx, getState());
		enterRule(_localctx, 98, RULE_lambdaBody);
		try {
			setState(660);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case BOOLEAN:
			case DECIMAL_NUMBER:
			case NEW:
			case NUMBER:
			case STRING:
			case SUPER:
			case THIS:
			case VOID:
			case WHOLE_NUMBER:
			case DECIMAL_LITERAL:
			case FLOAT_LITERAL:
			case BOOL_LITERAL:
			case STRING_LITERAL:
			case NULL_LITERAL:
			case LPAREN:
			case LT:
			case BANG:
			case TILDE:
			case INC:
			case DEC:
			case ADD:
			case SUB:
			case IDENTIFIER:
				enterOuterAlt(_localctx, 1);
				{
				setState(658);
				expression(0);
				}
				break;
			case LBRACE:
				enterOuterAlt(_localctx, 2);
				{
				setState(659);
				block();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class PrimaryContext extends ParserRuleContext {
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public TerminalNode THIS() { return getToken(TweedleParser.THIS, 0); }
		public TerminalNode SUPER() { return getToken(TweedleParser.SUPER, 0); }
		public LiteralContext literal() {
			return getRuleContext(LiteralContext.class,0);
		}
		public TerminalNode IDENTIFIER() { return getToken(TweedleParser.IDENTIFIER, 0); }
		public TypeTypeOrVoidContext typeTypeOrVoid() {
			return getRuleContext(TypeTypeOrVoidContext.class,0);
		}
		public TerminalNode CLASS() { return getToken(TweedleParser.CLASS, 0); }
		public NonWildcardTypeArgumentsContext nonWildcardTypeArguments() {
			return getRuleContext(NonWildcardTypeArgumentsContext.class,0);
		}
		public ExplicitGenericInvocationSuffixContext explicitGenericInvocationSuffix() {
			return getRuleContext(ExplicitGenericInvocationSuffixContext.class,0);
		}
		public ArgumentsContext arguments() {
			return getRuleContext(ArgumentsContext.class,0);
		}
		public PrimaryContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_primary; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterPrimary(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitPrimary(this);
		}
	}

	public final PrimaryContext primary() throws RecognitionException {
		PrimaryContext _localctx = new PrimaryContext(_ctx, getState());
		enterRule(_localctx, 100, RULE_primary);
		try {
			setState(680);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,66,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(662);
				match(LPAREN);
				setState(663);
				expression(0);
				setState(664);
				match(RPAREN);
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(666);
				match(THIS);
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(667);
				match(SUPER);
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(668);
				literal();
				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(669);
				match(IDENTIFIER);
				}
				break;
			case 6:
				enterOuterAlt(_localctx, 6);
				{
				setState(670);
				typeTypeOrVoid();
				setState(671);
				match(DOT);
				setState(672);
				match(CLASS);
				}
				break;
			case 7:
				enterOuterAlt(_localctx, 7);
				{
				setState(674);
				nonWildcardTypeArguments();
				setState(678);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case SUPER:
				case IDENTIFIER:
					{
					setState(675);
					explicitGenericInvocationSuffix();
					}
					break;
				case THIS:
					{
					setState(676);
					match(THIS);
					setState(677);
					arguments();
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ClassTypeContext extends ParserRuleContext {
		public TerminalNode IDENTIFIER() { return getToken(TweedleParser.IDENTIFIER, 0); }
		public ClassOrInterfaceTypeContext classOrInterfaceType() {
			return getRuleContext(ClassOrInterfaceTypeContext.class,0);
		}
		public TypeArgumentsContext typeArguments() {
			return getRuleContext(TypeArgumentsContext.class,0);
		}
		public ClassTypeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_classType; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterClassType(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitClassType(this);
		}
	}

	public final ClassTypeContext classType() throws RecognitionException {
		ClassTypeContext _localctx = new ClassTypeContext(_ctx, getState());
		enterRule(_localctx, 102, RULE_classType);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(685);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,67,_ctx) ) {
			case 1:
				{
				setState(682);
				classOrInterfaceType();
				setState(683);
				match(DOT);
				}
				break;
			}
			setState(687);
			match(IDENTIFIER);
			setState(689);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==LT) {
				{
				setState(688);
				typeArguments();
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class CreatorContext extends ParserRuleContext {
		public NonWildcardTypeArgumentsContext nonWildcardTypeArguments() {
			return getRuleContext(NonWildcardTypeArgumentsContext.class,0);
		}
		public CreatedNameContext createdName() {
			return getRuleContext(CreatedNameContext.class,0);
		}
		public ClassCreatorRestContext classCreatorRest() {
			return getRuleContext(ClassCreatorRestContext.class,0);
		}
		public ArrayCreatorRestContext arrayCreatorRest() {
			return getRuleContext(ArrayCreatorRestContext.class,0);
		}
		public CreatorContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_creator; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterCreator(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitCreator(this);
		}
	}

	public final CreatorContext creator() throws RecognitionException {
		CreatorContext _localctx = new CreatorContext(_ctx, getState());
		enterRule(_localctx, 104, RULE_creator);
		try {
			setState(700);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case LT:
				enterOuterAlt(_localctx, 1);
				{
				setState(691);
				nonWildcardTypeArguments();
				setState(692);
				createdName();
				setState(693);
				classCreatorRest();
				}
				break;
			case BOOLEAN:
			case DECIMAL_NUMBER:
			case NUMBER:
			case STRING:
			case WHOLE_NUMBER:
			case IDENTIFIER:
				enterOuterAlt(_localctx, 2);
				{
				setState(695);
				createdName();
				setState(698);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case LBRACK:
					{
					setState(696);
					arrayCreatorRest();
					}
					break;
				case LPAREN:
					{
					setState(697);
					classCreatorRest();
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class CreatedNameContext extends ParserRuleContext {
		public List<TerminalNode> IDENTIFIER() { return getTokens(TweedleParser.IDENTIFIER); }
		public TerminalNode IDENTIFIER(int i) {
			return getToken(TweedleParser.IDENTIFIER, i);
		}
		public List<TypeArgumentsOrDiamondContext> typeArgumentsOrDiamond() {
			return getRuleContexts(TypeArgumentsOrDiamondContext.class);
		}
		public TypeArgumentsOrDiamondContext typeArgumentsOrDiamond(int i) {
			return getRuleContext(TypeArgumentsOrDiamondContext.class,i);
		}
		public PrimitiveTypeContext primitiveType() {
			return getRuleContext(PrimitiveTypeContext.class,0);
		}
		public CreatedNameContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_createdName; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterCreatedName(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitCreatedName(this);
		}
	}

	public final CreatedNameContext createdName() throws RecognitionException {
		CreatedNameContext _localctx = new CreatedNameContext(_ctx, getState());
		enterRule(_localctx, 106, RULE_createdName);
		int _la;
		try {
			setState(717);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case IDENTIFIER:
				enterOuterAlt(_localctx, 1);
				{
				setState(702);
				match(IDENTIFIER);
				setState(704);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==LT) {
					{
					setState(703);
					typeArgumentsOrDiamond();
					}
				}

				setState(713);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==DOT) {
					{
					{
					setState(706);
					match(DOT);
					setState(707);
					match(IDENTIFIER);
					setState(709);
					_errHandler.sync(this);
					_la = _input.LA(1);
					if (_la==LT) {
						{
						setState(708);
						typeArgumentsOrDiamond();
						}
					}

					}
					}
					setState(715);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				}
				break;
			case BOOLEAN:
			case DECIMAL_NUMBER:
			case NUMBER:
			case STRING:
			case WHOLE_NUMBER:
				enterOuterAlt(_localctx, 2);
				{
				setState(716);
				primitiveType();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class InnerCreatorContext extends ParserRuleContext {
		public TerminalNode IDENTIFIER() { return getToken(TweedleParser.IDENTIFIER, 0); }
		public ClassCreatorRestContext classCreatorRest() {
			return getRuleContext(ClassCreatorRestContext.class,0);
		}
		public NonWildcardTypeArgumentsOrDiamondContext nonWildcardTypeArgumentsOrDiamond() {
			return getRuleContext(NonWildcardTypeArgumentsOrDiamondContext.class,0);
		}
		public InnerCreatorContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_innerCreator; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterInnerCreator(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitInnerCreator(this);
		}
	}

	public final InnerCreatorContext innerCreator() throws RecognitionException {
		InnerCreatorContext _localctx = new InnerCreatorContext(_ctx, getState());
		enterRule(_localctx, 108, RULE_innerCreator);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(719);
			match(IDENTIFIER);
			setState(721);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==LT) {
				{
				setState(720);
				nonWildcardTypeArgumentsOrDiamond();
				}
			}

			setState(723);
			classCreatorRest();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ArrayCreatorRestContext extends ParserRuleContext {
		public ArrayInitializerContext arrayInitializer() {
			return getRuleContext(ArrayInitializerContext.class,0);
		}
		public List<ExpressionContext> expression() {
			return getRuleContexts(ExpressionContext.class);
		}
		public ExpressionContext expression(int i) {
			return getRuleContext(ExpressionContext.class,i);
		}
		public ArrayCreatorRestContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_arrayCreatorRest; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterArrayCreatorRest(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitArrayCreatorRest(this);
		}
	}

	public final ArrayCreatorRestContext arrayCreatorRest() throws RecognitionException {
		ArrayCreatorRestContext _localctx = new ArrayCreatorRestContext(_ctx, getState());
		enterRule(_localctx, 110, RULE_arrayCreatorRest);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(725);
			match(LBRACK);
			setState(753);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case RBRACK:
				{
				setState(726);
				match(RBRACK);
				setState(731);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==LBRACK) {
					{
					{
					setState(727);
					match(LBRACK);
					setState(728);
					match(RBRACK);
					}
					}
					setState(733);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				setState(734);
				arrayInitializer();
				}
				break;
			case BOOLEAN:
			case DECIMAL_NUMBER:
			case NEW:
			case NUMBER:
			case STRING:
			case SUPER:
			case THIS:
			case VOID:
			case WHOLE_NUMBER:
			case DECIMAL_LITERAL:
			case FLOAT_LITERAL:
			case BOOL_LITERAL:
			case STRING_LITERAL:
			case NULL_LITERAL:
			case LPAREN:
			case LT:
			case BANG:
			case TILDE:
			case INC:
			case DEC:
			case ADD:
			case SUB:
			case IDENTIFIER:
				{
				setState(735);
				expression(0);
				setState(736);
				match(RBRACK);
				setState(743);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,77,_ctx);
				while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
					if ( _alt==1 ) {
						{
						{
						setState(737);
						match(LBRACK);
						setState(738);
						expression(0);
						setState(739);
						match(RBRACK);
						}
						} 
					}
					setState(745);
					_errHandler.sync(this);
					_alt = getInterpreter().adaptivePredict(_input,77,_ctx);
				}
				setState(750);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,78,_ctx);
				while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
					if ( _alt==1 ) {
						{
						{
						setState(746);
						match(LBRACK);
						setState(747);
						match(RBRACK);
						}
						} 
					}
					setState(752);
					_errHandler.sync(this);
					_alt = getInterpreter().adaptivePredict(_input,78,_ctx);
				}
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ClassCreatorRestContext extends ParserRuleContext {
		public ArgumentsContext arguments() {
			return getRuleContext(ArgumentsContext.class,0);
		}
		public ClassBodyContext classBody() {
			return getRuleContext(ClassBodyContext.class,0);
		}
		public ClassCreatorRestContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_classCreatorRest; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterClassCreatorRest(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitClassCreatorRest(this);
		}
	}

	public final ClassCreatorRestContext classCreatorRest() throws RecognitionException {
		ClassCreatorRestContext _localctx = new ClassCreatorRestContext(_ctx, getState());
		enterRule(_localctx, 112, RULE_classCreatorRest);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(755);
			arguments();
			setState(757);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,80,_ctx) ) {
			case 1:
				{
				setState(756);
				classBody();
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ExplicitGenericInvocationContext extends ParserRuleContext {
		public NonWildcardTypeArgumentsContext nonWildcardTypeArguments() {
			return getRuleContext(NonWildcardTypeArgumentsContext.class,0);
		}
		public ExplicitGenericInvocationSuffixContext explicitGenericInvocationSuffix() {
			return getRuleContext(ExplicitGenericInvocationSuffixContext.class,0);
		}
		public ExplicitGenericInvocationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_explicitGenericInvocation; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterExplicitGenericInvocation(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitExplicitGenericInvocation(this);
		}
	}

	public final ExplicitGenericInvocationContext explicitGenericInvocation() throws RecognitionException {
		ExplicitGenericInvocationContext _localctx = new ExplicitGenericInvocationContext(_ctx, getState());
		enterRule(_localctx, 114, RULE_explicitGenericInvocation);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(759);
			nonWildcardTypeArguments();
			setState(760);
			explicitGenericInvocationSuffix();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class TypeArgumentsOrDiamondContext extends ParserRuleContext {
		public TypeArgumentsContext typeArguments() {
			return getRuleContext(TypeArgumentsContext.class,0);
		}
		public TypeArgumentsOrDiamondContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_typeArgumentsOrDiamond; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterTypeArgumentsOrDiamond(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitTypeArgumentsOrDiamond(this);
		}
	}

	public final TypeArgumentsOrDiamondContext typeArgumentsOrDiamond() throws RecognitionException {
		TypeArgumentsOrDiamondContext _localctx = new TypeArgumentsOrDiamondContext(_ctx, getState());
		enterRule(_localctx, 116, RULE_typeArgumentsOrDiamond);
		try {
			setState(765);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,81,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(762);
				match(LT);
				setState(763);
				match(GT);
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(764);
				typeArguments();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class NonWildcardTypeArgumentsOrDiamondContext extends ParserRuleContext {
		public NonWildcardTypeArgumentsContext nonWildcardTypeArguments() {
			return getRuleContext(NonWildcardTypeArgumentsContext.class,0);
		}
		public NonWildcardTypeArgumentsOrDiamondContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_nonWildcardTypeArgumentsOrDiamond; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterNonWildcardTypeArgumentsOrDiamond(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitNonWildcardTypeArgumentsOrDiamond(this);
		}
	}

	public final NonWildcardTypeArgumentsOrDiamondContext nonWildcardTypeArgumentsOrDiamond() throws RecognitionException {
		NonWildcardTypeArgumentsOrDiamondContext _localctx = new NonWildcardTypeArgumentsOrDiamondContext(_ctx, getState());
		enterRule(_localctx, 118, RULE_nonWildcardTypeArgumentsOrDiamond);
		try {
			setState(770);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,82,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(767);
				match(LT);
				setState(768);
				match(GT);
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(769);
				nonWildcardTypeArguments();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class NonWildcardTypeArgumentsContext extends ParserRuleContext {
		public TypeListContext typeList() {
			return getRuleContext(TypeListContext.class,0);
		}
		public NonWildcardTypeArgumentsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_nonWildcardTypeArguments; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterNonWildcardTypeArguments(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitNonWildcardTypeArguments(this);
		}
	}

	public final NonWildcardTypeArgumentsContext nonWildcardTypeArguments() throws RecognitionException {
		NonWildcardTypeArgumentsContext _localctx = new NonWildcardTypeArgumentsContext(_ctx, getState());
		enterRule(_localctx, 120, RULE_nonWildcardTypeArguments);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(772);
			match(LT);
			setState(773);
			typeList();
			setState(774);
			match(GT);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class TypeListContext extends ParserRuleContext {
		public List<TypeTypeContext> typeType() {
			return getRuleContexts(TypeTypeContext.class);
		}
		public TypeTypeContext typeType(int i) {
			return getRuleContext(TypeTypeContext.class,i);
		}
		public TypeListContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_typeList; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterTypeList(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitTypeList(this);
		}
	}

	public final TypeListContext typeList() throws RecognitionException {
		TypeListContext _localctx = new TypeListContext(_ctx, getState());
		enterRule(_localctx, 122, RULE_typeList);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(776);
			typeType();
			setState(781);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(777);
				match(COMMA);
				setState(778);
				typeType();
				}
				}
				setState(783);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class TypeTypeContext extends ParserRuleContext {
		public ClassOrInterfaceTypeContext classOrInterfaceType() {
			return getRuleContext(ClassOrInterfaceTypeContext.class,0);
		}
		public PrimitiveTypeContext primitiveType() {
			return getRuleContext(PrimitiveTypeContext.class,0);
		}
		public TypeTypeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_typeType; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterTypeType(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitTypeType(this);
		}
	}

	public final TypeTypeContext typeType() throws RecognitionException {
		TypeTypeContext _localctx = new TypeTypeContext(_ctx, getState());
		enterRule(_localctx, 124, RULE_typeType);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(786);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case IDENTIFIER:
				{
				setState(784);
				classOrInterfaceType();
				}
				break;
			case BOOLEAN:
			case DECIMAL_NUMBER:
			case NUMBER:
			case STRING:
			case WHOLE_NUMBER:
				{
				setState(785);
				primitiveType();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			setState(792);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==LBRACK) {
				{
				{
				setState(788);
				match(LBRACK);
				setState(789);
				match(RBRACK);
				}
				}
				setState(794);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class PrimitiveTypeContext extends ParserRuleContext {
		public TerminalNode BOOLEAN() { return getToken(TweedleParser.BOOLEAN, 0); }
		public TerminalNode DECIMAL_NUMBER() { return getToken(TweedleParser.DECIMAL_NUMBER, 0); }
		public TerminalNode WHOLE_NUMBER() { return getToken(TweedleParser.WHOLE_NUMBER, 0); }
		public TerminalNode NUMBER() { return getToken(TweedleParser.NUMBER, 0); }
		public TerminalNode STRING() { return getToken(TweedleParser.STRING, 0); }
		public PrimitiveTypeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_primitiveType; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterPrimitiveType(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitPrimitiveType(this);
		}
	}

	public final PrimitiveTypeContext primitiveType() throws RecognitionException {
		PrimitiveTypeContext _localctx = new PrimitiveTypeContext(_ctx, getState());
		enterRule(_localctx, 126, RULE_primitiveType);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(795);
			_la = _input.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << BOOLEAN) | (1L << DECIMAL_NUMBER) | (1L << NUMBER) | (1L << STRING) | (1L << WHOLE_NUMBER))) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class TypeArgumentsContext extends ParserRuleContext {
		public List<TypeArgumentContext> typeArgument() {
			return getRuleContexts(TypeArgumentContext.class);
		}
		public TypeArgumentContext typeArgument(int i) {
			return getRuleContext(TypeArgumentContext.class,i);
		}
		public TypeArgumentsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_typeArguments; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterTypeArguments(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitTypeArguments(this);
		}
	}

	public final TypeArgumentsContext typeArguments() throws RecognitionException {
		TypeArgumentsContext _localctx = new TypeArgumentsContext(_ctx, getState());
		enterRule(_localctx, 128, RULE_typeArguments);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(797);
			match(LT);
			setState(798);
			typeArgument();
			setState(803);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(799);
				match(COMMA);
				setState(800);
				typeArgument();
				}
				}
				setState(805);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(806);
			match(GT);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class SuperSuffixContext extends ParserRuleContext {
		public ArgumentsContext arguments() {
			return getRuleContext(ArgumentsContext.class,0);
		}
		public TerminalNode IDENTIFIER() { return getToken(TweedleParser.IDENTIFIER, 0); }
		public SuperSuffixContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_superSuffix; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterSuperSuffix(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitSuperSuffix(this);
		}
	}

	public final SuperSuffixContext superSuffix() throws RecognitionException {
		SuperSuffixContext _localctx = new SuperSuffixContext(_ctx, getState());
		enterRule(_localctx, 130, RULE_superSuffix);
		try {
			setState(814);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case LPAREN:
				enterOuterAlt(_localctx, 1);
				{
				setState(808);
				arguments();
				}
				break;
			case DOT:
				enterOuterAlt(_localctx, 2);
				{
				setState(809);
				match(DOT);
				setState(810);
				match(IDENTIFIER);
				setState(812);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,87,_ctx) ) {
				case 1:
					{
					setState(811);
					arguments();
					}
					break;
				}
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ExplicitGenericInvocationSuffixContext extends ParserRuleContext {
		public TerminalNode SUPER() { return getToken(TweedleParser.SUPER, 0); }
		public SuperSuffixContext superSuffix() {
			return getRuleContext(SuperSuffixContext.class,0);
		}
		public TerminalNode IDENTIFIER() { return getToken(TweedleParser.IDENTIFIER, 0); }
		public ArgumentsContext arguments() {
			return getRuleContext(ArgumentsContext.class,0);
		}
		public ExplicitGenericInvocationSuffixContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_explicitGenericInvocationSuffix; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterExplicitGenericInvocationSuffix(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitExplicitGenericInvocationSuffix(this);
		}
	}

	public final ExplicitGenericInvocationSuffixContext explicitGenericInvocationSuffix() throws RecognitionException {
		ExplicitGenericInvocationSuffixContext _localctx = new ExplicitGenericInvocationSuffixContext(_ctx, getState());
		enterRule(_localctx, 132, RULE_explicitGenericInvocationSuffix);
		try {
			setState(820);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case SUPER:
				enterOuterAlt(_localctx, 1);
				{
				setState(816);
				match(SUPER);
				setState(817);
				superSuffix();
				}
				break;
			case IDENTIFIER:
				enterOuterAlt(_localctx, 2);
				{
				setState(818);
				match(IDENTIFIER);
				setState(819);
				arguments();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ArgumentsContext extends ParserRuleContext {
		public ExpressionListContext expressionList() {
			return getRuleContext(ExpressionListContext.class,0);
		}
		public ArgumentsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_arguments; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).enterArguments(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof TweedleParserListener ) ((TweedleParserListener)listener).exitArguments(this);
		}
	}

	public final ArgumentsContext arguments() throws RecognitionException {
		ArgumentsContext _localctx = new ArgumentsContext(_ctx, getState());
		enterRule(_localctx, 134, RULE_arguments);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(822);
			match(LPAREN);
			setState(824);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << BOOLEAN) | (1L << DECIMAL_NUMBER) | (1L << NEW) | (1L << NUMBER) | (1L << STRING) | (1L << SUPER) | (1L << THIS) | (1L << VOID) | (1L << WHOLE_NUMBER) | (1L << DECIMAL_LITERAL) | (1L << FLOAT_LITERAL) | (1L << BOOL_LITERAL) | (1L << STRING_LITERAL) | (1L << NULL_LITERAL) | (1L << LPAREN) | (1L << LT) | (1L << BANG) | (1L << TILDE) | (1L << INC) | (1L << DEC) | (1L << ADD) | (1L << SUB))) != 0) || _la==IDENTIFIER) {
				{
				setState(823);
				expressionList();
				}
			}

			setState(826);
			match(RPAREN);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public boolean sempred(RuleContext _localctx, int ruleIndex, int predIndex) {
		switch (ruleIndex) {
		case 46:
			return expression_sempred((ExpressionContext)_localctx, predIndex);
		}
		return true;
	}
	private boolean expression_sempred(ExpressionContext _localctx, int predIndex) {
		switch (predIndex) {
		case 0:
			return precpred(_ctx, 16);
		case 1:
			return precpred(_ctx, 15);
		case 2:
			return precpred(_ctx, 14);
		case 3:
			return precpred(_ctx, 13);
		case 4:
			return precpred(_ctx, 12);
		case 5:
			return precpred(_ctx, 11);
		case 6:
			return precpred(_ctx, 10);
		case 7:
			return precpred(_ctx, 9);
		case 8:
			return precpred(_ctx, 8);
		case 9:
			return precpred(_ctx, 7);
		case 10:
			return precpred(_ctx, 6);
		case 11:
			return precpred(_ctx, 5);
		case 12:
			return precpred(_ctx, 24);
		case 13:
			return precpred(_ctx, 23);
		case 14:
			return precpred(_ctx, 19);
		case 15:
			return precpred(_ctx, 3);
		}
		return true;
	}

	public static final String _serializedATN =
		"\3\u608b\ua72a\u8133\ub9ed\u417c\u3be7\u7786\u5964\3Z\u033f\4\2\t\2\4"+
		"\3\t\3\4\4\t\4\4\5\t\5\4\6\t\6\4\7\t\7\4\b\t\b\4\t\t\t\4\n\t\n\4\13\t"+
		"\13\4\f\t\f\4\r\t\r\4\16\t\16\4\17\t\17\4\20\t\20\4\21\t\21\4\22\t\22"+
		"\4\23\t\23\4\24\t\24\4\25\t\25\4\26\t\26\4\27\t\27\4\30\t\30\4\31\t\31"+
		"\4\32\t\32\4\33\t\33\4\34\t\34\4\35\t\35\4\36\t\36\4\37\t\37\4 \t \4!"+
		"\t!\4\"\t\"\4#\t#\4$\t$\4%\t%\4&\t&\4\'\t\'\4(\t(\4)\t)\4*\t*\4+\t+\4"+
		",\t,\4-\t-\4.\t.\4/\t/\4\60\t\60\4\61\t\61\4\62\t\62\4\63\t\63\4\64\t"+
		"\64\4\65\t\65\4\66\t\66\4\67\t\67\48\t8\49\t9\4:\t:\4;\t;\4<\t<\4=\t="+
		"\4>\t>\4?\t?\4@\t@\4A\tA\4B\tB\4C\tC\4D\tD\4E\tE\3\2\7\2\u008c\n\2\f\2"+
		"\16\2\u008f\13\2\3\2\3\2\5\2\u0093\n\2\3\2\5\2\u0096\n\2\3\3\3\3\5\3\u009a"+
		"\n\3\3\4\3\4\3\4\3\5\3\5\3\6\3\6\3\7\3\7\3\7\5\7\u00a6\n\7\3\7\3\7\5\7"+
		"\u00aa\n\7\3\7\3\7\5\7\u00ae\n\7\3\7\3\7\3\b\3\b\3\t\3\t\3\t\3\t\7\t\u00b8"+
		"\n\t\f\t\16\t\u00bb\13\t\3\t\3\t\3\n\3\n\3\n\5\n\u00c2\n\n\3\13\3\13\3"+
		"\13\7\13\u00c7\n\13\f\13\16\13\u00ca\13\13\3\f\3\f\3\f\3\f\5\f\u00d0\n"+
		"\f\3\f\3\f\3\r\3\r\3\r\7\r\u00d7\n\r\f\r\16\r\u00da\13\r\3\16\3\16\5\16"+
		"\u00de\n\16\3\16\5\16\u00e1\n\16\3\17\3\17\7\17\u00e5\n\17\f\17\16\17"+
		"\u00e8\13\17\3\17\3\17\3\20\3\20\5\20\u00ee\n\20\3\20\3\20\7\20\u00f2"+
		"\n\20\f\20\16\20\u00f5\13\20\3\20\5\20\u00f8\n\20\3\21\3\21\3\21\3\21"+
		"\3\21\3\21\3\21\5\21\u0101\n\21\3\22\3\22\3\22\3\22\3\22\7\22\u0108\n"+
		"\22\f\22\16\22\u010b\13\22\3\22\3\22\3\23\3\23\5\23\u0111\n\23\3\24\3"+
		"\24\5\24\u0115\n\24\3\25\3\25\3\25\3\26\3\26\3\26\3\27\3\27\3\27\3\27"+
		"\3\30\3\30\3\30\3\30\3\31\3\31\3\31\7\31\u0128\n\31\f\31\16\31\u012b\13"+
		"\31\3\32\3\32\3\32\5\32\u0130\n\32\3\33\3\33\3\33\7\33\u0135\n\33\f\33"+
		"\16\33\u0138\13\33\3\34\3\34\5\34\u013c\n\34\3\35\3\35\3\35\3\35\7\35"+
		"\u0142\n\35\f\35\16\35\u0145\13\35\3\35\5\35\u0148\n\35\5\35\u014a\n\35"+
		"\3\35\3\35\3\36\3\36\3\37\3\37\3\37\3\37\5\37\u0154\n\37\5\37\u0156\n"+
		"\37\3 \3 \5 \u015a\n \3 \3 \3!\3!\3!\7!\u0161\n!\f!\16!\u0164\13!\3!\3"+
		"!\7!\u0168\n!\f!\16!\u016b\13!\3!\3!\5!\u016f\n!\3!\3!\3!\7!\u0174\n!"+
		"\f!\16!\u0177\13!\3!\3!\5!\u017b\n!\3!\5!\u017e\n!\3\"\7\"\u0181\n\"\f"+
		"\"\16\"\u0184\13\"\3\"\3\"\3\"\3#\7#\u018a\n#\f#\16#\u018d\13#\3#\3#\3"+
		"#\3#\3#\3$\7$\u0195\n$\f$\16$\u0198\13$\3$\3$\3$\3$\3%\3%\3&\3&\7&\u01a2"+
		"\n&\f&\16&\u01a5\13&\3&\3&\3\'\3\'\3\'\3\'\5\'\u01ad\n\'\3(\5(\u01b0\n"+
		"(\3(\7(\u01b3\n(\f(\16(\u01b6\13(\3(\3(\3(\3)\3)\3)\3)\3)\3)\3)\3)\3)"+
		"\3)\5)\u01c5\n)\3)\3)\3)\3)\3)\3)\3)\3)\3)\3)\3)\3)\3)\3)\3)\3)\3)\3)"+
		"\3)\3)\3)\3)\5)\u01dd\n)\3)\3)\3)\3)\3)\3)\3)\3)\3)\5)\u01e8\n)\3*\3*"+
		"\3*\3*\3*\3+\3+\3+\3+\3,\3,\3,\7,\u01f6\n,\f,\16,\u01f9\13,\3-\3-\3-\3"+
		"-\3.\3.\3.\7.\u0202\n.\f.\16.\u0205\13.\3/\3/\3/\5/\u020a\n/\3/\3/\3\60"+
		"\3\60\3\60\3\60\3\60\3\60\3\60\3\60\3\60\3\60\3\60\3\60\3\60\3\60\3\60"+
		"\3\60\3\60\3\60\5\60\u0220\n\60\3\60\3\60\5\60\u0224\n\60\3\60\3\60\3"+
		"\60\5\60\u0229\n\60\3\60\3\60\5\60\u022d\n\60\3\60\3\60\3\60\3\60\3\60"+
		"\3\60\3\60\3\60\3\60\3\60\3\60\3\60\3\60\3\60\5\60\u023d\n\60\3\60\3\60"+
		"\3\60\3\60\3\60\3\60\3\60\3\60\3\60\3\60\3\60\3\60\3\60\3\60\3\60\3\60"+
		"\3\60\3\60\3\60\3\60\3\60\3\60\3\60\3\60\3\60\3\60\3\60\3\60\3\60\3\60"+
		"\3\60\3\60\3\60\3\60\3\60\3\60\3\60\3\60\5\60\u0265\n\60\3\60\3\60\3\60"+
		"\3\60\5\60\u026b\n\60\3\60\3\60\3\60\3\60\3\60\3\60\3\60\3\60\3\60\3\60"+
		"\5\60\u0277\n\60\3\60\7\60\u027a\n\60\f\60\16\60\u027d\13\60\3\61\3\61"+
		"\3\61\3\61\3\62\3\62\3\62\5\62\u0286\n\62\3\62\3\62\3\62\3\62\3\62\7\62"+
		"\u028d\n\62\f\62\16\62\u0290\13\62\3\62\5\62\u0293\n\62\3\63\3\63\5\63"+
		"\u0297\n\63\3\64\3\64\3\64\3\64\3\64\3\64\3\64\3\64\3\64\3\64\3\64\3\64"+
		"\3\64\3\64\3\64\3\64\5\64\u02a9\n\64\5\64\u02ab\n\64\3\65\3\65\3\65\5"+
		"\65\u02b0\n\65\3\65\3\65\5\65\u02b4\n\65\3\66\3\66\3\66\3\66\3\66\3\66"+
		"\3\66\5\66\u02bd\n\66\5\66\u02bf\n\66\3\67\3\67\5\67\u02c3\n\67\3\67\3"+
		"\67\3\67\5\67\u02c8\n\67\7\67\u02ca\n\67\f\67\16\67\u02cd\13\67\3\67\5"+
		"\67\u02d0\n\67\38\38\58\u02d4\n8\38\38\39\39\39\39\79\u02dc\n9\f9\169"+
		"\u02df\139\39\39\39\39\39\39\39\79\u02e8\n9\f9\169\u02eb\139\39\39\79"+
		"\u02ef\n9\f9\169\u02f2\139\59\u02f4\n9\3:\3:\5:\u02f8\n:\3;\3;\3;\3<\3"+
		"<\3<\5<\u0300\n<\3=\3=\3=\5=\u0305\n=\3>\3>\3>\3>\3?\3?\3?\7?\u030e\n"+
		"?\f?\16?\u0311\13?\3@\3@\5@\u0315\n@\3@\3@\7@\u0319\n@\f@\16@\u031c\13"+
		"@\3A\3A\3B\3B\3B\3B\7B\u0324\nB\fB\16B\u0327\13B\3B\3B\3C\3C\3C\3C\5C"+
		"\u032f\nC\5C\u0331\nC\3D\3D\3D\3D\5D\u0337\nD\3E\3E\5E\u033b\nE\3E\3E"+
		"\3E\2\3^F\2\4\6\b\n\f\16\20\22\24\26\30\32\34\36 \"$&(*,.\60\62\64\66"+
		"8:<>@BDFHJLNPRTVXZ\\^`bdfhjlnprtvxz|~\u0080\u0082\u0084\u0086\u0088\2"+
		"\r\5\2\20\20\27\27\35\35\4\2\r\r\33\33\3\2!%\3\2<?\3\2\62\63\4\2@AEE\3"+
		"\2>?\4\2\60\61\678\4\2\66\6699\3\2<=\7\2\3\3\7\7\26\26\32\32  \2\u0386"+
		"\2\u0095\3\2\2\2\4\u0099\3\2\2\2\6\u009b\3\2\2\2\b\u009e\3\2\2\2\n\u00a0"+
		"\3\2\2\2\f\u00a2\3\2\2\2\16\u00b1\3\2\2\2\20\u00b3\3\2\2\2\22\u00be\3"+
		"\2\2\2\24\u00c3\3\2\2\2\26\u00cb\3\2\2\2\30\u00d3\3\2\2\2\32\u00db\3\2"+
		"\2\2\34\u00e2\3\2\2\2\36\u00f7\3\2\2\2 \u0100\3\2\2\2\"\u0102\3\2\2\2"+
		"$\u0110\3\2\2\2&\u0114\3\2\2\2(\u0116\3\2\2\2*\u0119\3\2\2\2,\u011c\3"+
		"\2\2\2.\u0120\3\2\2\2\60\u0124\3\2\2\2\62\u012c\3\2\2\2\64\u0131\3\2\2"+
		"\2\66\u013b\3\2\2\28\u013d\3\2\2\2:\u014d\3\2\2\2<\u0155\3\2\2\2>\u0157"+
		"\3\2\2\2@\u017d\3\2\2\2B\u0182\3\2\2\2D\u018b\3\2\2\2F\u0196\3\2\2\2H"+
		"\u019d\3\2\2\2J\u019f\3\2\2\2L\u01ac\3\2\2\2N\u01af\3\2\2\2P\u01e7\3\2"+
		"\2\2R\u01e9\3\2\2\2T\u01ee\3\2\2\2V\u01f2\3\2\2\2X\u01fa\3\2\2\2Z\u01fe"+
		"\3\2\2\2\\\u0206\3\2\2\2^\u022c\3\2\2\2`\u027e\3\2\2\2b\u0292\3\2\2\2"+
		"d\u0296\3\2\2\2f\u02aa\3\2\2\2h\u02af\3\2\2\2j\u02be\3\2\2\2l\u02cf\3"+
		"\2\2\2n\u02d1\3\2\2\2p\u02d7\3\2\2\2r\u02f5\3\2\2\2t\u02f9\3\2\2\2v\u02ff"+
		"\3\2\2\2x\u0304\3\2\2\2z\u0306\3\2\2\2|\u030a\3\2\2\2~\u0314\3\2\2\2\u0080"+
		"\u031d\3\2\2\2\u0082\u031f\3\2\2\2\u0084\u0330\3\2\2\2\u0086\u0336\3\2"+
		"\2\2\u0088\u0338\3\2\2\2\u008a\u008c\5\4\3\2\u008b\u008a\3\2\2\2\u008c"+
		"\u008f\3\2\2\2\u008d\u008b\3\2\2\2\u008d\u008e\3\2\2\2\u008e\u0092\3\2"+
		"\2\2\u008f\u008d\3\2\2\2\u0090\u0093\5\f\7\2\u0091\u0093\5\26\f\2\u0092"+
		"\u0090\3\2\2\2\u0092\u0091\3\2\2\2\u0093\u0096\3\2\2\2\u0094\u0096\7,"+
		"\2\2\u0095\u008d\3\2\2\2\u0095\u0094\3\2\2\2\u0096\3\3\2\2\2\u0097\u009a"+
		"\5\6\4\2\u0098\u009a\7\31\2\2\u0099\u0097\3\2\2\2\u0099\u0098\3\2\2\2"+
		"\u009a\5\3\2\2\2\u009b\u009c\7S\2\2\u009c\u009d\5\b\5\2\u009d\7\3\2\2"+
		"\2\u009e\u009f\t\2\2\2\u009f\t\3\2\2\2\u00a0\u00a1\7\6\2\2\u00a1\13\3"+
		"\2\2\2\u00a2\u00a3\7\4\2\2\u00a3\u00a5\5\16\b\2\u00a4\u00a6\5\20\t\2\u00a5"+
		"\u00a4\3\2\2\2\u00a5\u00a6\3\2\2\2\u00a6\u00a9\3\2\2\2\u00a7\u00a8\7\r"+
		"\2\2\u00a8\u00aa\5~@\2\u00a9\u00a7\3\2\2\2\u00a9\u00aa\3\2\2\2\u00aa\u00ad"+
		"\3\2\2\2\u00ab\u00ac\7\24\2\2\u00ac\u00ae\7Z\2\2\u00ad\u00ab\3\2\2\2\u00ad"+
		"\u00ae\3\2\2\2\u00ae\u00af\3\2\2\2\u00af\u00b0\5\34\17\2\u00b0\r\3\2\2"+
		"\2\u00b1\u00b2\7Z\2\2\u00b2\17\3\2\2\2\u00b3\u00b4\7\61\2\2\u00b4\u00b9"+
		"\5\22\n\2\u00b5\u00b6\7-\2\2\u00b6\u00b8\5\22\n\2\u00b7\u00b5\3\2\2\2"+
		"\u00b8\u00bb\3\2\2\2\u00b9\u00b7\3\2\2\2\u00b9\u00ba\3\2\2\2\u00ba\u00bc"+
		"\3\2\2\2\u00bb\u00b9\3\2\2\2\u00bc\u00bd\7\60\2\2\u00bd\21\3\2\2\2\u00be"+
		"\u00c1\7Z\2\2\u00bf\u00c0\7\r\2\2\u00c0\u00c2\5\24\13\2\u00c1\u00bf\3"+
		"\2\2\2\u00c1\u00c2\3\2\2\2\u00c2\23\3\2\2\2\u00c3\u00c8\5~@\2\u00c4\u00c5"+
		"\7B\2\2\u00c5\u00c7\5~@\2\u00c6\u00c4\3\2\2\2\u00c7\u00ca\3\2\2\2\u00c8"+
		"\u00c6\3\2\2\2\u00c8\u00c9\3\2\2\2\u00c9\25\3\2\2\2\u00ca\u00c8\3\2\2"+
		"\2\u00cb\u00cc\7\f\2\2\u00cc\u00cd\5\16\b\2\u00cd\u00cf\7(\2\2\u00ce\u00d0"+
		"\5\30\r\2\u00cf\u00ce\3\2\2\2\u00cf\u00d0\3\2\2\2\u00d0\u00d1\3\2\2\2"+
		"\u00d1\u00d2\7)\2\2\u00d2\27\3\2\2\2\u00d3\u00d8\5\32\16\2\u00d4\u00d5"+
		"\7-\2\2\u00d5\u00d7\5\32\16\2\u00d6\u00d4\3\2\2\2\u00d7\u00da\3\2\2\2"+
		"\u00d8\u00d6\3\2\2\2\u00d8\u00d9\3\2\2\2\u00d9\31\3\2\2\2\u00da\u00d8"+
		"\3\2\2\2\u00db\u00dd\5\16\b\2\u00dc\u00de\5\u0088E\2\u00dd\u00dc\3\2\2"+
		"\2\u00dd\u00de\3\2\2\2\u00de\u00e0\3\2\2\2\u00df\u00e1\5\34\17\2\u00e0"+
		"\u00df\3\2\2\2\u00e0\u00e1\3\2\2\2\u00e1\33\3\2\2\2\u00e2\u00e6\7(\2\2"+
		"\u00e3\u00e5\5\36\20\2\u00e4\u00e3\3\2\2\2\u00e5\u00e8\3\2\2\2\u00e6\u00e4"+
		"\3\2\2\2\u00e6\u00e7\3\2\2\2\u00e7\u00e9\3\2\2\2\u00e8\u00e6\3\2\2\2\u00e9"+
		"\u00ea\7)\2\2\u00ea\35\3\2\2\2\u00eb\u00f8\7,\2\2\u00ec\u00ee\7\31\2\2"+
		"\u00ed\u00ec\3\2\2\2\u00ed\u00ee\3\2\2\2\u00ee\u00ef\3\2\2\2\u00ef\u00f8"+
		"\5J&\2\u00f0\u00f2\5\4\3\2\u00f1\u00f0\3\2\2\2\u00f2\u00f5\3\2\2\2\u00f3"+
		"\u00f1\3\2\2\2\u00f3\u00f4\3\2\2\2\u00f4\u00f6\3\2\2\2\u00f5\u00f3\3\2"+
		"\2\2\u00f6\u00f8\5 \21\2\u00f7\u00eb\3\2\2\2\u00f7\u00ed\3\2\2\2\u00f7"+
		"\u00f3\3\2\2\2\u00f8\37\3\2\2\2\u00f9\u0101\5\"\22\2\u00fa\u0101\5(\25"+
		"\2\u00fb\u0101\5.\30\2\u00fc\u0101\5,\27\2\u00fd\u0101\5*\26\2\u00fe\u0101"+
		"\5\f\7\2\u00ff\u0101\5\26\f\2\u0100\u00f9\3\2\2\2\u0100\u00fa\3\2\2\2"+
		"\u0100\u00fb\3\2\2\2\u0100\u00fc\3\2\2\2\u0100\u00fd\3\2\2\2\u0100\u00fe"+
		"\3\2\2\2\u0100\u00ff\3\2\2\2\u0101!\3\2\2\2\u0102\u0103\5&\24\2\u0103"+
		"\u0104\7Z\2\2\u0104\u0109\5> \2\u0105\u0106\7*\2\2\u0106\u0108\7+\2\2"+
		"\u0107\u0105\3\2\2\2\u0108\u010b\3\2\2\2\u0109\u0107\3\2\2\2\u0109\u010a"+
		"\3\2\2\2\u010a\u010c\3\2\2\2\u010b\u0109\3\2\2\2\u010c\u010d\5$\23\2\u010d"+
		"#\3\2\2\2\u010e\u0111\5J&\2\u010f\u0111\7,\2\2\u0110\u010e\3\2\2\2\u0110"+
		"\u010f\3\2\2\2\u0111%\3\2\2\2\u0112\u0115\5~@\2\u0113\u0115\7\36\2\2\u0114"+
		"\u0112\3\2\2\2\u0114\u0113\3\2\2\2\u0115\'\3\2\2\2\u0116\u0117\5\20\t"+
		"\2\u0117\u0118\5\"\22\2\u0118)\3\2\2\2\u0119\u011a\5\20\t\2\u011a\u011b"+
		"\5,\27\2\u011b+\3\2\2\2\u011c\u011d\7Z\2\2\u011d\u011e\5> \2\u011e\u011f"+
		"\5J&\2\u011f-\3\2\2\2\u0120\u0121\5~@\2\u0121\u0122\5\60\31\2\u0122\u0123"+
		"\7,\2\2\u0123/\3\2\2\2\u0124\u0129\5\62\32\2\u0125\u0126\7-\2\2\u0126"+
		"\u0128\5\62\32\2\u0127\u0125\3\2\2\2\u0128\u012b\3\2\2\2\u0129\u0127\3"+
		"\2\2\2\u0129\u012a\3\2\2\2\u012a\61\3\2\2\2\u012b\u0129\3\2\2\2\u012c"+
		"\u012f\5\64\33\2\u012d\u012e\7U\2\2\u012e\u0130\5\66\34\2\u012f\u012d"+
		"\3\2\2\2\u012f\u0130\3\2\2\2\u0130\63\3\2\2\2\u0131\u0136\7Z\2\2\u0132"+
		"\u0133\7*\2\2\u0133\u0135\7+\2\2\u0134\u0132\3\2\2\2\u0135\u0138\3\2\2"+
		"\2\u0136\u0134\3\2\2\2\u0136\u0137\3\2\2\2\u0137\65\3\2\2\2\u0138\u0136"+
		"\3\2\2\2\u0139\u013c\58\35\2\u013a\u013c\5^\60\2\u013b\u0139\3\2\2\2\u013b"+
		"\u013a\3\2\2\2\u013c\67\3\2\2\2\u013d\u0149\7(\2\2\u013e\u0143\5\66\34"+
		"\2\u013f\u0140\7-\2\2\u0140\u0142\5\66\34\2\u0141\u013f\3\2\2\2\u0142"+
		"\u0145\3\2\2\2\u0143\u0141\3\2\2\2\u0143\u0144\3\2\2\2\u0144\u0147\3\2"+
		"\2\2\u0145\u0143\3\2\2\2\u0146\u0148\7-\2\2\u0147\u0146\3\2\2\2\u0147"+
		"\u0148\3\2\2\2\u0148\u014a\3\2\2\2\u0149\u013e\3\2\2\2\u0149\u014a\3\2"+
		"\2\2\u014a\u014b\3\2\2\2\u014b\u014c\7)\2\2\u014c9\3\2\2\2\u014d\u014e"+
		"\7Z\2\2\u014e;\3\2\2\2\u014f\u0156\5~@\2\u0150\u0153\7\64\2\2\u0151\u0152"+
		"\t\3\2\2\u0152\u0154\5~@\2\u0153\u0151\3\2\2\2\u0153\u0154\3\2\2\2\u0154"+
		"\u0156\3\2\2\2\u0155\u014f\3\2\2\2\u0155\u0150\3\2\2\2\u0156=\3\2\2\2"+
		"\u0157\u0159\7&\2\2\u0158\u015a\5@!\2\u0159\u0158\3\2\2\2\u0159\u015a"+
		"\3\2\2\2\u015a\u015b\3\2\2\2\u015b\u015c\7\'\2\2\u015c?\3\2\2\2\u015d"+
		"\u0162\5B\"\2\u015e\u015f\7-\2\2\u015f\u0161\5B\"\2\u0160\u015e\3\2\2"+
		"\2\u0161\u0164\3\2\2\2\u0162\u0160\3\2\2\2\u0162\u0163\3\2\2\2\u0163\u0169"+
		"\3\2\2\2\u0164\u0162\3\2\2\2\u0165\u0166\7-\2\2\u0166\u0168\5D#\2\u0167"+
		"\u0165\3\2\2\2\u0168\u016b\3\2\2\2\u0169\u0167\3\2\2\2\u0169\u016a\3\2"+
		"\2\2\u016a\u016e\3\2\2\2\u016b\u0169\3\2\2\2\u016c\u016d\7-\2\2\u016d"+
		"\u016f\5F$\2\u016e\u016c\3\2\2\2\u016e\u016f\3\2\2\2\u016f\u017e\3\2\2"+
		"\2\u0170\u0175\5D#\2\u0171\u0172\7-\2\2\u0172\u0174\5D#\2\u0173\u0171"+
		"\3\2\2\2\u0174\u0177\3\2\2\2\u0175\u0173\3\2\2\2\u0175\u0176\3\2\2\2\u0176"+
		"\u017a\3\2\2\2\u0177\u0175\3\2\2\2\u0178\u0179\7-\2\2\u0179\u017b\5F$"+
		"\2\u017a\u0178\3\2\2\2\u017a\u017b\3\2\2\2\u017b\u017e\3\2\2\2\u017c\u017e"+
		"\5F$\2\u017d\u015d\3\2\2\2\u017d\u0170\3\2\2\2\u017d\u017c\3\2\2\2\u017e"+
		"A\3\2\2\2\u017f\u0181\5\n\6\2\u0180\u017f\3\2\2\2\u0181\u0184\3\2\2\2"+
		"\u0182\u0180\3\2\2\2\u0182\u0183\3\2\2\2\u0183\u0185\3\2\2\2\u0184\u0182"+
		"\3\2\2\2\u0185\u0186\5~@\2\u0186\u0187\5\64\33\2\u0187C\3\2\2\2\u0188"+
		"\u018a\5\n\6\2\u0189\u0188\3\2\2\2\u018a\u018d\3\2\2\2\u018b\u0189\3\2"+
		"\2\2\u018b\u018c\3\2\2\2\u018c\u018e\3\2\2\2\u018d\u018b\3\2\2\2\u018e"+
		"\u018f\5~@\2\u018f\u0190\5\64\33\2\u0190\u0191\7U\2\2\u0191\u0192\5^\60"+
		"\2\u0192E\3\2\2\2\u0193\u0195\5\n\6\2\u0194\u0193\3\2\2\2\u0195\u0198"+
		"\3\2\2\2\u0196\u0194\3\2\2\2\u0196\u0197\3\2\2\2\u0197\u0199\3\2\2\2\u0198"+
		"\u0196\3\2\2\2\u0199\u019a\5~@\2\u019a\u019b\7T\2\2\u019b\u019c\5\64\33"+
		"\2\u019cG\3\2\2\2\u019d\u019e\t\4\2\2\u019eI\3\2\2\2\u019f\u01a3\7(\2"+
		"\2\u01a0\u01a2\5L\'\2\u01a1\u01a0\3\2\2\2\u01a2\u01a5\3\2\2\2\u01a3\u01a1"+
		"\3\2\2\2\u01a3\u01a4\3\2\2\2\u01a4\u01a6\3\2\2\2\u01a5\u01a3\3\2\2\2\u01a6"+
		"\u01a7\7)\2\2\u01a7K\3\2\2\2\u01a8\u01a9\5N(\2\u01a9\u01aa\7,\2\2\u01aa"+
		"\u01ad\3\2\2\2\u01ab\u01ad\5P)\2\u01ac\u01a8\3\2\2\2\u01ac\u01ab\3\2\2"+
		"\2\u01adM\3\2\2\2\u01ae\u01b0\7Y\2\2\u01af\u01ae\3\2\2\2\u01af\u01b0\3"+
		"\2\2\2\u01b0\u01b4\3\2\2\2\u01b1\u01b3\5\n\6\2\u01b2\u01b1\3\2\2\2\u01b3"+
		"\u01b6\3\2\2\2\u01b4\u01b2\3\2\2\2\u01b4\u01b5\3\2\2\2\u01b5\u01b7\3\2"+
		"\2\2\u01b6\u01b4\3\2\2\2\u01b7\u01b8\5~@\2\u01b8\u01b9\5\60\31\2\u01b9"+
		"O\3\2\2\2\u01ba\u01e8\5J&\2\u01bb\u01bc\7\5\2\2\u01bc\u01bd\5^\60\2\u01bd"+
		"\u01be\5P)\2\u01be\u01e8\3\2\2\2\u01bf\u01c0\7\21\2\2\u01c0\u01c1\5T+"+
		"\2\u01c1\u01c4\5P)\2\u01c2\u01c3\7\13\2\2\u01c3\u01c5\5P)\2\u01c4\u01c2"+
		"\3\2\2\2\u01c4\u01c5\3\2\2\2\u01c5\u01e8\3\2\2\2\u01c6\u01c7\7\16\2\2"+
		"\u01c7\u01c8\7&\2\2\u01c8\u01c9\5R*\2\u01c9\u01ca\7\'\2\2\u01ca\u01cb"+
		"\5P)\2\u01cb\u01e8\3\2\2\2\u01cc\u01cd\7\17\2\2\u01cd\u01ce\7&\2\2\u01ce"+
		"\u01cf\5R*\2\u01cf\u01d0\7\'\2\2\u01d0\u01d1\5P)\2\u01d1\u01e8\3\2\2\2"+
		"\u01d2\u01d3\7\37\2\2\u01d3\u01d4\5T+\2\u01d4\u01d5\5P)\2\u01d5\u01e8"+
		"\3\2\2\2\u01d6\u01d7\7\b\2\2\u01d7\u01e8\5J&\2\u01d8\u01d9\7\t\2\2\u01d9"+
		"\u01e8\5J&\2\u01da\u01dc\7\30\2\2\u01db\u01dd\5^\60\2\u01dc\u01db\3\2"+
		"\2\2\u01dc\u01dd\3\2\2\2\u01dd\u01de\3\2\2\2\u01de\u01e8\7,\2\2\u01df"+
		"\u01e0\5^\60\2\u01e0\u01e1\7,\2\2\u01e1\u01e8\3\2\2\2\u01e2\u01e3\7Z\2"+
		"\2\u01e3\u01e4\7\65\2\2\u01e4\u01e8\5P)\2\u01e5\u01e6\7Y\2\2\u01e6\u01e8"+
		"\5P)\2\u01e7\u01ba\3\2\2\2\u01e7\u01bb\3\2\2\2\u01e7\u01bf\3\2\2\2\u01e7"+
		"\u01c6\3\2\2\2\u01e7\u01cc\3\2\2\2\u01e7\u01d2\3\2\2\2\u01e7\u01d6\3\2"+
		"\2\2\u01e7\u01d8\3\2\2\2\u01e7\u01da\3\2\2\2\u01e7\u01df\3\2\2\2\u01e7"+
		"\u01e2\3\2\2\2\u01e7\u01e5\3\2\2\2\u01e8Q\3\2\2\2\u01e9\u01ea\5~@\2\u01ea"+
		"\u01eb\5\64\33\2\u01eb\u01ec\7\22\2\2\u01ec\u01ed\5^\60\2\u01edS\3\2\2"+
		"\2\u01ee\u01ef\7&\2\2\u01ef\u01f0\5^\60\2\u01f0\u01f1\7\'\2\2\u01f1U\3"+
		"\2\2\2\u01f2\u01f7\5X-\2\u01f3\u01f4\7-\2\2\u01f4\u01f6\5X-\2\u01f5\u01f3"+
		"\3\2\2\2\u01f6\u01f9\3\2\2\2\u01f7\u01f5\3\2\2\2\u01f7\u01f8\3\2\2\2\u01f8"+
		"W\3\2\2\2\u01f9\u01f7\3\2\2\2\u01fa\u01fb\7Z\2\2\u01fb\u01fc\7\65\2\2"+
		"\u01fc\u01fd\5^\60\2\u01fdY\3\2\2\2\u01fe\u0203\5^\60\2\u01ff\u0200\7"+
		"-\2\2\u0200\u0202\5^\60\2\u0201\u01ff\3\2\2\2\u0202\u0205\3\2\2\2\u0203"+
		"\u0201\3\2\2\2\u0203\u0204\3\2\2\2\u0204[\3\2\2\2\u0205\u0203\3\2\2\2"+
		"\u0206\u0207\7Z\2\2\u0207\u0209\7&\2\2\u0208\u020a\5V,\2\u0209\u0208\3"+
		"\2\2\2\u0209\u020a\3\2\2\2\u020a\u020b\3\2\2\2\u020b\u020c\7\'\2\2\u020c"+
		"]\3\2\2\2\u020d\u020e\b\60\1\2\u020e\u022d\5f\64\2\u020f\u022d\5\\/\2"+
		"\u0210\u0211\7\25\2\2\u0211\u022d\5j\66\2\u0212\u0213\7&\2\2\u0213\u0214"+
		"\5~@\2\u0214\u0215\7\'\2\2\u0215\u0216\5^\60\26\u0216\u022d\3\2\2\2\u0217"+
		"\u0218\t\5\2\2\u0218\u022d\5^\60\24\u0219\u021a\t\6\2\2\u021a\u022d\5"+
		"^\60\23\u021b\u022d\5`\61\2\u021c\u021d\5~@\2\u021d\u0223\7R\2\2\u021e"+
		"\u0220\5\u0082B\2\u021f\u021e\3\2\2\2\u021f\u0220\3\2\2\2\u0220\u0221"+
		"\3\2\2\2\u0221\u0224\7Z\2\2\u0222\u0224\7\25\2\2\u0223\u021f\3\2\2\2\u0223"+
		"\u0222\3\2\2\2\u0224\u022d\3\2\2\2\u0225\u0226\5h\65\2\u0226\u0228\7R"+
		"\2\2\u0227\u0229\5\u0082B\2\u0228\u0227\3\2\2\2\u0228\u0229\3\2\2\2\u0229"+
		"\u022a\3\2\2\2\u022a\u022b\7\25\2\2\u022b\u022d\3\2\2\2\u022c\u020d\3"+
		"\2\2\2\u022c\u020f\3\2\2\2\u022c\u0210\3\2\2\2\u022c\u0212\3\2\2\2\u022c"+
		"\u0217\3\2\2\2\u022c\u0219\3\2\2\2\u022c\u021b\3\2\2\2\u022c\u021c\3\2"+
		"\2\2\u022c\u0225\3\2\2\2\u022d\u027b\3\2\2\2\u022e\u022f\f\22\2\2\u022f"+
		"\u0230\t\7\2\2\u0230\u027a\5^\60\23\u0231\u0232\f\21\2\2\u0232\u0233\t"+
		"\b\2\2\u0233\u027a\5^\60\22\u0234\u023c\f\20\2\2\u0235\u0236\7\61\2\2"+
		"\u0236\u023d\7\61\2\2\u0237\u0238\7\60\2\2\u0238\u0239\7\60\2\2\u0239"+
		"\u023d\7\60\2\2\u023a\u023b\7\60\2\2\u023b\u023d\7\60\2\2\u023c\u0235"+
		"\3\2\2\2\u023c\u0237\3\2\2\2\u023c\u023a\3\2\2\2\u023d\u023e\3\2\2\2\u023e"+
		"\u027a\5^\60\21\u023f\u0240\f\17\2\2\u0240\u0241\t\t\2\2\u0241\u027a\5"+
		"^\60\20\u0242\u0243\f\16\2\2\u0243\u0244\t\n\2\2\u0244\u027a\5^\60\17"+
		"\u0245\u0246\f\r\2\2\u0246\u0247\7B\2\2\u0247\u027a\5^\60\16\u0248\u0249"+
		"\f\f\2\2\u0249\u024a\7D\2\2\u024a\u027a\5^\60\r\u024b\u024c\f\13\2\2\u024c"+
		"\u024d\7C\2\2\u024d\u027a\5^\60\f\u024e\u024f\f\n\2\2\u024f\u0250\7:\2"+
		"\2\u0250\u027a\5^\60\13\u0251\u0252\f\t\2\2\u0252\u0253\7;\2\2\u0253\u027a"+
		"\5^\60\n\u0254\u0255\f\b\2\2\u0255\u0256\7\64\2\2\u0256\u0257\5^\60\2"+
		"\u0257\u0258\7\65\2\2\u0258\u0259\5^\60\t\u0259\u027a\3\2\2\2\u025a\u025b"+
		"\f\7\2\2\u025b\u025c\7U\2\2\u025c\u027a\5^\60\7\u025d\u025e\f\32\2\2\u025e"+
		"\u026a\7.\2\2\u025f\u026b\7Z\2\2\u0260\u026b\5\\/\2\u0261\u026b\7\34\2"+
		"\2\u0262\u0264\7\25\2\2\u0263\u0265\5z>\2\u0264\u0263\3\2\2\2\u0264\u0265"+
		"\3\2\2\2\u0265\u0266\3\2\2\2\u0266\u026b\5n8\2\u0267\u0268\7\33\2\2\u0268"+
		"\u026b\5\u0084C\2\u0269\u026b\5t;\2\u026a\u025f\3\2\2\2\u026a\u0260\3"+
		"\2\2\2\u026a\u0261\3\2\2\2\u026a\u0262\3\2\2\2\u026a\u0267\3\2\2\2\u026a"+
		"\u0269\3\2\2\2\u026b\u027a\3\2\2\2\u026c\u026d\f\31\2\2\u026d\u026e\7"+
		"*\2\2\u026e\u026f\5^\60\2\u026f\u0270\7+\2\2\u0270\u027a\3\2\2\2\u0271"+
		"\u0272\f\25\2\2\u0272\u027a\t\13\2\2\u0273\u0274\f\5\2\2\u0274\u0276\7"+
		"R\2\2\u0275\u0277\5\u0082B\2\u0276\u0275\3\2\2\2\u0276\u0277\3\2\2\2\u0277"+
		"\u0278\3\2\2\2\u0278\u027a\7Z\2\2\u0279\u022e\3\2\2\2\u0279\u0231\3\2"+
		"\2\2\u0279\u0234\3\2\2\2\u0279\u023f\3\2\2\2\u0279\u0242\3\2\2\2\u0279"+
		"\u0245\3\2\2\2\u0279\u0248\3\2\2\2\u0279\u024b\3\2\2\2\u0279\u024e\3\2"+
		"\2\2\u0279\u0251\3\2\2\2\u0279\u0254\3\2\2\2\u0279\u025a\3\2\2\2\u0279"+
		"\u025d\3\2\2\2\u0279\u026c\3\2\2\2\u0279\u0271\3\2\2\2\u0279\u0273\3\2"+
		"\2\2\u027a\u027d\3\2\2\2\u027b\u0279\3\2\2\2\u027b\u027c\3\2\2\2\u027c"+
		"_\3\2\2\2\u027d\u027b\3\2\2\2\u027e\u027f\5b\62\2\u027f\u0280\7Q\2\2\u0280"+
		"\u0281\5d\63\2\u0281a\3\2\2\2\u0282\u0293\7Z\2\2\u0283\u0285\7&\2\2\u0284"+
		"\u0286\5@!\2\u0285\u0284\3\2\2\2\u0285\u0286\3\2\2\2\u0286\u0287\3\2\2"+
		"\2\u0287\u0293\7\'\2\2\u0288\u0289\7&\2\2\u0289\u028e\7Z\2\2\u028a\u028b"+
		"\7-\2\2\u028b\u028d\7Z\2\2\u028c\u028a\3\2\2\2\u028d\u0290\3\2\2\2\u028e"+
		"\u028c\3\2\2\2\u028e\u028f\3\2\2\2\u028f\u0291\3\2\2\2\u0290\u028e\3\2"+
		"\2\2\u0291\u0293\7\'\2\2\u0292\u0282\3\2\2\2\u0292\u0283\3\2\2\2\u0292"+
		"\u0288\3\2\2\2\u0293c\3\2\2\2\u0294\u0297\5^\60\2\u0295\u0297\5J&\2\u0296"+
		"\u0294\3\2\2\2\u0296\u0295\3\2\2\2\u0297e\3\2\2\2\u0298\u0299\7&\2\2\u0299"+
		"\u029a\5^\60\2\u029a\u029b\7\'\2\2\u029b\u02ab\3\2\2\2\u029c\u02ab\7\34"+
		"\2\2\u029d\u02ab\7\33\2\2\u029e\u02ab\5H%\2\u029f\u02ab\7Z\2\2\u02a0\u02a1"+
		"\5&\24\2\u02a1\u02a2\7.\2\2\u02a2\u02a3\7\4\2\2\u02a3\u02ab\3\2\2\2\u02a4"+
		"\u02a8\5z>\2\u02a5\u02a9\5\u0086D\2\u02a6\u02a7\7\34\2\2\u02a7\u02a9\5"+
		"\u0088E\2\u02a8\u02a5\3\2\2\2\u02a8\u02a6\3\2\2\2\u02a9\u02ab\3\2\2\2"+
		"\u02aa\u0298\3\2\2\2\u02aa\u029c\3\2\2\2\u02aa\u029d\3\2\2\2\u02aa\u029e"+
		"\3\2\2\2\u02aa\u029f\3\2\2\2\u02aa\u02a0\3\2\2\2\u02aa\u02a4\3\2\2\2\u02ab"+
		"g\3\2\2\2\u02ac\u02ad\5:\36\2\u02ad\u02ae\7.\2\2\u02ae\u02b0\3\2\2\2\u02af"+
		"\u02ac\3\2\2\2\u02af\u02b0\3\2\2\2\u02b0\u02b1\3\2\2\2\u02b1\u02b3\7Z"+
		"\2\2\u02b2\u02b4\5\u0082B\2\u02b3\u02b2\3\2\2\2\u02b3\u02b4\3\2\2\2\u02b4"+
		"i\3\2\2\2\u02b5\u02b6\5z>\2\u02b6\u02b7\5l\67\2\u02b7\u02b8\5r:\2\u02b8"+
		"\u02bf\3\2\2\2\u02b9\u02bc\5l\67\2\u02ba\u02bd\5p9\2\u02bb\u02bd\5r:\2"+
		"\u02bc\u02ba\3\2\2\2\u02bc\u02bb\3\2\2\2\u02bd\u02bf\3\2\2\2\u02be\u02b5"+
		"\3\2\2\2\u02be\u02b9\3\2\2\2\u02bfk\3\2\2\2\u02c0\u02c2\7Z\2\2\u02c1\u02c3"+
		"\5v<\2\u02c2\u02c1\3\2\2\2\u02c2\u02c3\3\2\2\2\u02c3\u02cb\3\2\2\2\u02c4"+
		"\u02c5\7.\2\2\u02c5\u02c7\7Z\2\2\u02c6\u02c8\5v<\2\u02c7\u02c6\3\2\2\2"+
		"\u02c7\u02c8\3\2\2\2\u02c8\u02ca\3\2\2\2\u02c9\u02c4\3\2\2\2\u02ca\u02cd"+
		"\3\2\2\2\u02cb\u02c9\3\2\2\2\u02cb\u02cc\3\2\2\2\u02cc\u02d0\3\2\2\2\u02cd"+
		"\u02cb\3\2\2\2\u02ce\u02d0\5\u0080A\2\u02cf\u02c0\3\2\2\2\u02cf\u02ce"+
		"\3\2\2\2\u02d0m\3\2\2\2\u02d1\u02d3\7Z\2\2\u02d2\u02d4\5x=\2\u02d3\u02d2"+
		"\3\2\2\2\u02d3\u02d4\3\2\2\2\u02d4\u02d5\3\2\2\2\u02d5\u02d6\5r:\2\u02d6"+
		"o\3\2\2\2\u02d7\u02f3\7*\2\2\u02d8\u02dd\7+\2\2\u02d9\u02da\7*\2\2\u02da"+
		"\u02dc\7+\2\2\u02db\u02d9\3\2\2\2\u02dc\u02df\3\2\2\2\u02dd\u02db\3\2"+
		"\2\2\u02dd\u02de\3\2\2\2\u02de\u02e0\3\2\2\2\u02df\u02dd\3\2\2\2\u02e0"+
		"\u02f4\58\35\2\u02e1\u02e2\5^\60\2\u02e2\u02e9\7+\2\2\u02e3\u02e4\7*\2"+
		"\2\u02e4\u02e5\5^\60\2\u02e5\u02e6\7+\2\2\u02e6\u02e8\3\2\2\2\u02e7\u02e3"+
		"\3\2\2\2\u02e8\u02eb\3\2\2\2\u02e9\u02e7\3\2\2\2\u02e9\u02ea\3\2\2\2\u02ea"+
		"\u02f0\3\2\2\2\u02eb\u02e9\3\2\2\2\u02ec\u02ed\7*\2\2\u02ed\u02ef\7+\2"+
		"\2\u02ee\u02ec\3\2\2\2\u02ef\u02f2\3\2\2\2\u02f0\u02ee\3\2\2\2\u02f0\u02f1"+
		"\3\2\2\2\u02f1\u02f4\3\2\2\2\u02f2\u02f0\3\2\2\2\u02f3\u02d8\3\2\2\2\u02f3"+
		"\u02e1\3\2\2\2\u02f4q\3\2\2\2\u02f5\u02f7\5\u0088E\2\u02f6\u02f8\5\34"+
		"\17\2\u02f7\u02f6\3\2\2\2\u02f7\u02f8\3\2\2\2\u02f8s\3\2\2\2\u02f9\u02fa"+
		"\5z>\2\u02fa\u02fb\5\u0086D\2\u02fbu\3\2\2\2\u02fc\u02fd\7\61\2\2\u02fd"+
		"\u0300\7\60\2\2\u02fe\u0300\5\u0082B\2\u02ff\u02fc\3\2\2\2\u02ff\u02fe"+
		"\3\2\2\2\u0300w\3\2\2\2\u0301\u0302\7\61\2\2\u0302\u0305\7\60\2\2\u0303"+
		"\u0305\5z>\2\u0304\u0301\3\2\2\2\u0304\u0303\3\2\2\2\u0305y\3\2\2\2\u0306"+
		"\u0307\7\61\2\2\u0307\u0308\5|?\2\u0308\u0309\7\60\2\2\u0309{\3\2\2\2"+
		"\u030a\u030f\5~@\2\u030b\u030c\7-\2\2\u030c\u030e\5~@\2\u030d\u030b\3"+
		"\2\2\2\u030e\u0311\3\2\2\2\u030f\u030d\3\2\2\2\u030f\u0310\3\2\2\2\u0310"+
		"}\3\2\2\2\u0311\u030f\3\2\2\2\u0312\u0315\5:\36\2\u0313\u0315\5\u0080"+
		"A\2\u0314\u0312\3\2\2\2\u0314\u0313\3\2\2\2\u0315\u031a\3\2\2\2\u0316"+
		"\u0317\7*\2\2\u0317\u0319\7+\2\2\u0318\u0316\3\2\2\2\u0319\u031c\3\2\2"+
		"\2\u031a\u0318\3\2\2\2\u031a\u031b\3\2\2\2\u031b\177\3\2\2\2\u031c\u031a"+
		"\3\2\2\2\u031d\u031e\t\f\2\2\u031e\u0081\3\2\2\2\u031f\u0320\7\61\2\2"+
		"\u0320\u0325\5<\37\2\u0321\u0322\7-\2\2\u0322\u0324\5<\37\2\u0323\u0321"+
		"\3\2\2\2\u0324\u0327\3\2\2\2\u0325\u0323\3\2\2\2\u0325\u0326\3\2\2\2\u0326"+
		"\u0328\3\2\2\2\u0327\u0325\3\2\2\2\u0328\u0329\7\60\2\2\u0329\u0083\3"+
		"\2\2\2\u032a\u0331\5\u0088E\2\u032b\u032c\7.\2\2\u032c\u032e\7Z\2\2\u032d"+
		"\u032f\5\u0088E\2\u032e\u032d\3\2\2\2\u032e\u032f\3\2\2\2\u032f\u0331"+
		"\3\2\2\2\u0330\u032a\3\2\2\2\u0330\u032b\3\2\2\2\u0331\u0085\3\2\2\2\u0332"+
		"\u0333\7\33\2\2\u0333\u0337\5\u0084C\2\u0334\u0335\7Z\2\2\u0335\u0337"+
		"\5\u0088E\2\u0336\u0332\3\2\2\2\u0336\u0334\3\2\2\2\u0337\u0087\3\2\2"+
		"\2\u0338\u033a\7&\2\2\u0339\u033b\5Z.\2\u033a\u0339\3\2\2\2\u033a\u033b"+
		"\3\2\2\2\u033b\u033c\3\2\2\2\u033c\u033d\7\'\2\2\u033d\u0089\3\2\2\2]"+
		"\u008d\u0092\u0095\u0099\u00a5\u00a9\u00ad\u00b9\u00c1\u00c8\u00cf\u00d8"+
		"\u00dd\u00e0\u00e6\u00ed\u00f3\u00f7\u0100\u0109\u0110\u0114\u0129\u012f"+
		"\u0136\u013b\u0143\u0147\u0149\u0153\u0155\u0159\u0162\u0169\u016e\u0175"+
		"\u017a\u017d\u0182\u018b\u0196\u01a3\u01ac\u01af\u01b4\u01c4\u01dc\u01e7"+
		"\u01f7\u0203\u0209\u021f\u0223\u0228\u022c\u023c\u0264\u026a\u0276\u0279"+
		"\u027b\u0285\u028e\u0292\u0296\u02a8\u02aa\u02af\u02b3\u02bc\u02be\u02c2"+
		"\u02c7\u02cb\u02cf\u02d3\u02dd\u02e9\u02f0\u02f3\u02f7\u02ff\u0304\u030f"+
		"\u0314\u031a\u0325\u032e\u0330\u0336\u033a";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}
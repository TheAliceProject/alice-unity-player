// Generated from c:\Users\jkieffer.FILAMENT\Documents\Repos\alice-unity-player\Assets\_Scripts\Tweedle\Grammar\TweedleParser.g4 by ANTLR 4.7.1
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
		COMPLETELY_HIDDEN=14, IF=15, IN=16, LOOP=17, MODELS=18, NEW=19, NUMBER=20, 
		PRIME_TIME=21, RETURN=22, STATIC=23, SUPER=24, THIS=25, TEXT_STRING=26, 
		TUCKED_AWAY=27, VOID=28, WHILE=29, WHOLE_NUMBER=30, DECIMAL_LITERAL=31, 
		FLOAT_LITERAL=32, BOOL_LITERAL=33, STRING_LITERAL=34, NULL_LITERAL=35, 
		LPAREN=36, RPAREN=37, LBRACE=38, RBRACE=39, LBRACK=40, RBRACK=41, SEMI=42, 
		COMMA=43, DOT=44, ASSIGN=45, GT=46, LT=47, BANG=48, TILDE=49, QUESTION=50, 
		COLON=51, EQUAL=52, LE=53, GE=54, NOTEQUAL=55, AND=56, OR=57, INC=58, 
		DEC=59, ADD=60, SUB=61, MUL=62, DIV=63, BITAND=64, BITOR=65, CARET=66, 
		MOD=67, ADD_ASSIGN=68, SUB_ASSIGN=69, MUL_ASSIGN=70, DIV_ASSIGN=71, AND_ASSIGN=72, 
		OR_ASSIGN=73, XOR_ASSIGN=74, MOD_ASSIGN=75, LSHIFT_ASSIGN=76, RSHIFT_ASSIGN=77, 
		URSHIFT_ASSIGN=78, ARROW=79, COLONCOLON=80, AT=81, CONCAT=82, ELLIPSIS=83, 
		LARROW=84, WS=85, COMMENT=86, LINE_COMMENT=87, NODE_DISABLE=88, NODE_ENABLE=89, 
		IDENTIFIER=90;
	public static final int
		RULE_typeDeclaration = 0, RULE_classModifier = 1, RULE_visibility = 2, 
		RULE_visibilityLevel = 3, RULE_classDeclaration = 4, RULE_identifier = 5, 
		RULE_enumDeclaration = 6, RULE_enumConstants = 7, RULE_enumConstant = 8, 
		RULE_enumBodyDeclarations = 9, RULE_classBody = 10, RULE_classBodyDeclaration = 11, 
		RULE_memberDeclaration = 12, RULE_methodDeclaration = 13, RULE_methodBody = 14, 
		RULE_typeTypeOrVoid = 15, RULE_constructorDeclaration = 16, RULE_fieldDeclaration = 17, 
		RULE_variableDeclarator = 18, RULE_variableDeclaratorId = 19, RULE_variableInitializer = 20, 
		RULE_arrayInitializer = 21, RULE_classType = 22, RULE_formalParameters = 23, 
		RULE_formalParameterList = 24, RULE_requiredParameter = 25, RULE_optionalParameter = 26, 
		RULE_lastFormalParameter = 27, RULE_literal = 28, RULE_block = 29, RULE_blockStatement = 30, 
		RULE_localVariableDeclaration = 31, RULE_statement = 32, RULE_forControl = 33, 
		RULE_parExpression = 34, RULE_unlabeledExpressionList = 35, RULE_labeledExpressionList = 36, 
		RULE_labeledExpression = 37, RULE_methodCall = 38, RULE_lambdaCall = 39, 
		RULE_expression = 40, RULE_lambdaExpression = 41, RULE_lambdaParameters = 42, 
		RULE_lambdaTypeSignature = 43, RULE_typeList = 44, RULE_primary = 45, 
		RULE_creator = 46, RULE_createdName = 47, RULE_arrayCreatorRest = 48, 
		RULE_classCreatorRest = 49, RULE_typeType = 50, RULE_primitiveType = 51, 
		RULE_superSuffix = 52, RULE_arguments = 53;
	public static final String[] ruleNames = {
		"typeDeclaration", "classModifier", "visibility", "visibilityLevel", "classDeclaration", 
		"identifier", "enumDeclaration", "enumConstants", "enumConstant", "enumBodyDeclarations", 
		"classBody", "classBodyDeclaration", "memberDeclaration", "methodDeclaration", 
		"methodBody", "typeTypeOrVoid", "constructorDeclaration", "fieldDeclaration", 
		"variableDeclarator", "variableDeclaratorId", "variableInitializer", "arrayInitializer", 
		"classType", "formalParameters", "formalParameterList", "requiredParameter", 
		"optionalParameter", "lastFormalParameter", "literal", "block", "blockStatement", 
		"localVariableDeclaration", "statement", "forControl", "parExpression", 
		"unlabeledExpressionList", "labeledExpressionList", "labeledExpression", 
		"methodCall", "lambdaCall", "expression", "lambdaExpression", "lambdaParameters", 
		"lambdaTypeSignature", "typeList", "primary", "creator", "createdName", 
		"arrayCreatorRest", "classCreatorRest", "typeType", "primitiveType", "superSuffix", 
		"arguments"
	};

	private static final String[] _LITERAL_NAMES = {
		null, "'Boolean'", "'class'", "'countUpTo'", "'constant'", "'DecimalNumber'", 
		"'doInOrder'", "'doTogether'", "'each'", "'else'", "'enum'", "'extends'", 
		"'forEach'", "'eachTogether'", "'CompletelyHidden'", "'if'", "'in'", "'loop'", 
		"'models'", "'new'", "'Number'", "'PrimeTime'", "'return'", "'static'", 
		"'super'", "'this'", "'TextString'", "'TuckedAway'", "'void'", "'while'", 
		"'WholeNumber'", null, null, null, null, "'null'", "'('", "')'", "'{'", 
		"'}'", "'['", "']'", "';'", "','", "'.'", "'='", "'>'", "'<'", "'!'", 
		"'~'", "'?'", "':'", "'=='", "'<='", "'>='", "'!='", "'&&'", "'||'", "'++'", 
		"'--'", "'+'", "'-'", "'*'", "'/'", "'&'", "'|'", "'^'", "'%'", "'+='", 
		"'-='", "'*='", "'/='", "'&='", "'|='", "'^='", "'%='", "'<<='", "'>>='", 
		"'>>>='", "'->'", "'::'", "'@'", "'..'", "'...'", "'<-'", null, null, 
		null, "'*<'", "'>*'"
	};
	private static final String[] _SYMBOLIC_NAMES = {
		null, "BOOLEAN", "CLASS", "COUNT_UP_TO", "CONSTANT", "DECIMAL_NUMBER", 
		"DO_IN_ORDER", "DO_TOGETHER", "EACH", "ELSE", "ENUM", "EXTENDS", "FOR_EACH", 
		"EACH_TOGETHER", "COMPLETELY_HIDDEN", "IF", "IN", "LOOP", "MODELS", "NEW", 
		"NUMBER", "PRIME_TIME", "RETURN", "STATIC", "SUPER", "THIS", "TEXT_STRING", 
		"TUCKED_AWAY", "VOID", "WHILE", "WHOLE_NUMBER", "DECIMAL_LITERAL", "FLOAT_LITERAL", 
		"BOOL_LITERAL", "STRING_LITERAL", "NULL_LITERAL", "LPAREN", "RPAREN", 
		"LBRACE", "RBRACE", "LBRACK", "RBRACK", "SEMI", "COMMA", "DOT", "ASSIGN", 
		"GT", "LT", "BANG", "TILDE", "QUESTION", "COLON", "EQUAL", "LE", "GE", 
		"NOTEQUAL", "AND", "OR", "INC", "DEC", "ADD", "SUB", "MUL", "DIV", "BITAND", 
		"BITOR", "CARET", "MOD", "ADD_ASSIGN", "SUB_ASSIGN", "MUL_ASSIGN", "DIV_ASSIGN", 
		"AND_ASSIGN", "OR_ASSIGN", "XOR_ASSIGN", "MOD_ASSIGN", "LSHIFT_ASSIGN", 
		"RSHIFT_ASSIGN", "URSHIFT_ASSIGN", "ARROW", "COLONCOLON", "AT", "CONCAT", 
		"ELLIPSIS", "LARROW", "WS", "COMMENT", "LINE_COMMENT", "NODE_DISABLE", 
		"NODE_ENABLE", "IDENTIFIER"
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
		public VisibilityContext visibility() {
			return getRuleContext(VisibilityContext.class,0);
		}
		public TypeDeclarationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_typeDeclaration; }
	}

	public final TypeDeclarationContext typeDeclaration() throws RecognitionException {
		TypeDeclarationContext _localctx = new TypeDeclarationContext(_ctx, getState());
		enterRule(_localctx, 0, RULE_typeDeclaration);
		int _la;
		try {
			setState(116);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case CLASS:
			case ENUM:
			case AT:
				enterOuterAlt(_localctx, 1);
				{
				setState(109);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==AT) {
					{
					setState(108);
					visibility();
					}
				}

				setState(113);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case CLASS:
					{
					setState(111);
					classDeclaration();
					}
					break;
				case ENUM:
					{
					setState(112);
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
				setState(115);
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
	}

	public final ClassModifierContext classModifier() throws RecognitionException {
		ClassModifierContext _localctx = new ClassModifierContext(_ctx, getState());
		enterRule(_localctx, 2, RULE_classModifier);
		try {
			setState(120);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case AT:
				enterOuterAlt(_localctx, 1);
				{
				setState(118);
				visibility();
				}
				break;
			case STATIC:
				enterOuterAlt(_localctx, 2);
				{
				setState(119);
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
	}

	public final VisibilityContext visibility() throws RecognitionException {
		VisibilityContext _localctx = new VisibilityContext(_ctx, getState());
		enterRule(_localctx, 4, RULE_visibility);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(122);
			match(AT);
			setState(123);
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
		public TerminalNode COMPLETELY_HIDDEN() { return getToken(TweedleParser.COMPLETELY_HIDDEN, 0); }
		public TerminalNode TUCKED_AWAY() { return getToken(TweedleParser.TUCKED_AWAY, 0); }
		public TerminalNode PRIME_TIME() { return getToken(TweedleParser.PRIME_TIME, 0); }
		public VisibilityLevelContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_visibilityLevel; }
	}

	public final VisibilityLevelContext visibilityLevel() throws RecognitionException {
		VisibilityLevelContext _localctx = new VisibilityLevelContext(_ctx, getState());
		enterRule(_localctx, 6, RULE_visibilityLevel);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(125);
			_la = _input.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << COMPLETELY_HIDDEN) | (1L << PRIME_TIME) | (1L << TUCKED_AWAY))) != 0)) ) {
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

	public static class ClassDeclarationContext extends ParserRuleContext {
		public TerminalNode CLASS() { return getToken(TweedleParser.CLASS, 0); }
		public IdentifierContext identifier() {
			return getRuleContext(IdentifierContext.class,0);
		}
		public ClassBodyContext classBody() {
			return getRuleContext(ClassBodyContext.class,0);
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
	}

	public final ClassDeclarationContext classDeclaration() throws RecognitionException {
		ClassDeclarationContext _localctx = new ClassDeclarationContext(_ctx, getState());
		enterRule(_localctx, 8, RULE_classDeclaration);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(127);
			match(CLASS);
			setState(128);
			identifier();
			setState(131);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==EXTENDS) {
				{
				setState(129);
				match(EXTENDS);
				setState(130);
				typeType();
				}
			}

			setState(135);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==MODELS) {
				{
				setState(133);
				match(MODELS);
				setState(134);
				match(IDENTIFIER);
				}
			}

			setState(137);
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
	}

	public final IdentifierContext identifier() throws RecognitionException {
		IdentifierContext _localctx = new IdentifierContext(_ctx, getState());
		enterRule(_localctx, 10, RULE_identifier);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(139);
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

	public static class EnumDeclarationContext extends ParserRuleContext {
		public TerminalNode ENUM() { return getToken(TweedleParser.ENUM, 0); }
		public IdentifierContext identifier() {
			return getRuleContext(IdentifierContext.class,0);
		}
		public EnumConstantsContext enumConstants() {
			return getRuleContext(EnumConstantsContext.class,0);
		}
		public EnumBodyDeclarationsContext enumBodyDeclarations() {
			return getRuleContext(EnumBodyDeclarationsContext.class,0);
		}
		public EnumDeclarationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_enumDeclaration; }
	}

	public final EnumDeclarationContext enumDeclaration() throws RecognitionException {
		EnumDeclarationContext _localctx = new EnumDeclarationContext(_ctx, getState());
		enterRule(_localctx, 12, RULE_enumDeclaration);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(141);
			match(ENUM);
			setState(142);
			identifier();
			setState(143);
			match(LBRACE);
			setState(145);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==IDENTIFIER) {
				{
				setState(144);
				enumConstants();
				}
			}

			setState(148);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==COMMA) {
				{
				setState(147);
				match(COMMA);
				}
			}

			setState(151);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==SEMI) {
				{
				setState(150);
				enumBodyDeclarations();
				}
			}

			setState(153);
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
	}

	public final EnumConstantsContext enumConstants() throws RecognitionException {
		EnumConstantsContext _localctx = new EnumConstantsContext(_ctx, getState());
		enterRule(_localctx, 14, RULE_enumConstants);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(155);
			enumConstant();
			setState(160);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,9,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					{
					{
					setState(156);
					match(COMMA);
					setState(157);
					enumConstant();
					}
					} 
				}
				setState(162);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,9,_ctx);
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
		public EnumConstantContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_enumConstant; }
	}

	public final EnumConstantContext enumConstant() throws RecognitionException {
		EnumConstantContext _localctx = new EnumConstantContext(_ctx, getState());
		enterRule(_localctx, 16, RULE_enumConstant);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(163);
			identifier();
			setState(165);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==LPAREN) {
				{
				setState(164);
				arguments();
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

	public static class EnumBodyDeclarationsContext extends ParserRuleContext {
		public List<ClassBodyDeclarationContext> classBodyDeclaration() {
			return getRuleContexts(ClassBodyDeclarationContext.class);
		}
		public ClassBodyDeclarationContext classBodyDeclaration(int i) {
			return getRuleContext(ClassBodyDeclarationContext.class,i);
		}
		public EnumBodyDeclarationsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_enumBodyDeclarations; }
	}

	public final EnumBodyDeclarationsContext enumBodyDeclarations() throws RecognitionException {
		EnumBodyDeclarationsContext _localctx = new EnumBodyDeclarationsContext(_ctx, getState());
		enterRule(_localctx, 18, RULE_enumBodyDeclarations);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(167);
			match(SEMI);
			setState(171);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << BOOLEAN) | (1L << DECIMAL_NUMBER) | (1L << NUMBER) | (1L << STATIC) | (1L << TEXT_STRING) | (1L << VOID) | (1L << WHOLE_NUMBER) | (1L << SEMI) | (1L << LT))) != 0) || _la==AT || _la==IDENTIFIER) {
				{
				{
				setState(168);
				classBodyDeclaration();
				}
				}
				setState(173);
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
	}

	public final ClassBodyContext classBody() throws RecognitionException {
		ClassBodyContext _localctx = new ClassBodyContext(_ctx, getState());
		enterRule(_localctx, 20, RULE_classBody);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(174);
			match(LBRACE);
			setState(178);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << BOOLEAN) | (1L << DECIMAL_NUMBER) | (1L << NUMBER) | (1L << STATIC) | (1L << TEXT_STRING) | (1L << VOID) | (1L << WHOLE_NUMBER) | (1L << SEMI) | (1L << LT))) != 0) || _la==AT || _la==IDENTIFIER) {
				{
				{
				setState(175);
				classBodyDeclaration();
				}
				}
				setState(180);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(181);
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
	}

	public final ClassBodyDeclarationContext classBodyDeclaration() throws RecognitionException {
		ClassBodyDeclarationContext _localctx = new ClassBodyDeclarationContext(_ctx, getState());
		enterRule(_localctx, 22, RULE_classBodyDeclaration);
		int _la;
		try {
			setState(191);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case SEMI:
				enterOuterAlt(_localctx, 1);
				{
				setState(183);
				match(SEMI);
				}
				break;
			case BOOLEAN:
			case DECIMAL_NUMBER:
			case NUMBER:
			case STATIC:
			case TEXT_STRING:
			case VOID:
			case WHOLE_NUMBER:
			case LT:
			case AT:
			case IDENTIFIER:
				enterOuterAlt(_localctx, 2);
				{
				setState(187);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==STATIC || _la==AT) {
					{
					{
					setState(184);
					classModifier();
					}
					}
					setState(189);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				setState(190);
				memberDeclaration();
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

	public static class MemberDeclarationContext extends ParserRuleContext {
		public MethodDeclarationContext methodDeclaration() {
			return getRuleContext(MethodDeclarationContext.class,0);
		}
		public FieldDeclarationContext fieldDeclaration() {
			return getRuleContext(FieldDeclarationContext.class,0);
		}
		public ConstructorDeclarationContext constructorDeclaration() {
			return getRuleContext(ConstructorDeclarationContext.class,0);
		}
		public MemberDeclarationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_memberDeclaration; }
	}

	public final MemberDeclarationContext memberDeclaration() throws RecognitionException {
		MemberDeclarationContext _localctx = new MemberDeclarationContext(_ctx, getState());
		enterRule(_localctx, 24, RULE_memberDeclaration);
		try {
			setState(196);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,15,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(193);
				methodDeclaration();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(194);
				fieldDeclaration();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(195);
				constructorDeclaration();
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
	}

	public final MethodDeclarationContext methodDeclaration() throws RecognitionException {
		MethodDeclarationContext _localctx = new MethodDeclarationContext(_ctx, getState());
		enterRule(_localctx, 26, RULE_methodDeclaration);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(198);
			typeTypeOrVoid();
			setState(199);
			match(IDENTIFIER);
			setState(200);
			formalParameters();
			setState(205);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==LBRACK) {
				{
				{
				setState(201);
				match(LBRACK);
				setState(202);
				match(RBRACK);
				}
				}
				setState(207);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(208);
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
	}

	public final MethodBodyContext methodBody() throws RecognitionException {
		MethodBodyContext _localctx = new MethodBodyContext(_ctx, getState());
		enterRule(_localctx, 28, RULE_methodBody);
		try {
			setState(212);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case LBRACE:
				enterOuterAlt(_localctx, 1);
				{
				setState(210);
				block();
				}
				break;
			case SEMI:
				enterOuterAlt(_localctx, 2);
				{
				setState(211);
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
	}

	public final TypeTypeOrVoidContext typeTypeOrVoid() throws RecognitionException {
		TypeTypeOrVoidContext _localctx = new TypeTypeOrVoidContext(_ctx, getState());
		enterRule(_localctx, 30, RULE_typeTypeOrVoid);
		try {
			setState(216);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case BOOLEAN:
			case DECIMAL_NUMBER:
			case NUMBER:
			case TEXT_STRING:
			case WHOLE_NUMBER:
			case LT:
			case IDENTIFIER:
				enterOuterAlt(_localctx, 1);
				{
				setState(214);
				typeType();
				}
				break;
			case VOID:
				enterOuterAlt(_localctx, 2);
				{
				setState(215);
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
	}

	public final ConstructorDeclarationContext constructorDeclaration() throws RecognitionException {
		ConstructorDeclarationContext _localctx = new ConstructorDeclarationContext(_ctx, getState());
		enterRule(_localctx, 32, RULE_constructorDeclaration);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(218);
			match(IDENTIFIER);
			setState(219);
			formalParameters();
			setState(220);
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
		public VariableDeclaratorContext variableDeclarator() {
			return getRuleContext(VariableDeclaratorContext.class,0);
		}
		public FieldDeclarationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_fieldDeclaration; }
	}

	public final FieldDeclarationContext fieldDeclaration() throws RecognitionException {
		FieldDeclarationContext _localctx = new FieldDeclarationContext(_ctx, getState());
		enterRule(_localctx, 34, RULE_fieldDeclaration);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(222);
			typeType();
			setState(223);
			variableDeclarator();
			setState(224);
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
	}

	public final VariableDeclaratorContext variableDeclarator() throws RecognitionException {
		VariableDeclaratorContext _localctx = new VariableDeclaratorContext(_ctx, getState());
		enterRule(_localctx, 36, RULE_variableDeclarator);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(226);
			variableDeclaratorId();
			setState(229);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==LARROW) {
				{
				setState(227);
				match(LARROW);
				setState(228);
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
	}

	public final VariableDeclaratorIdContext variableDeclaratorId() throws RecognitionException {
		VariableDeclaratorIdContext _localctx = new VariableDeclaratorIdContext(_ctx, getState());
		enterRule(_localctx, 38, RULE_variableDeclaratorId);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(231);
			match(IDENTIFIER);
			setState(236);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==LBRACK) {
				{
				{
				setState(232);
				match(LBRACK);
				setState(233);
				match(RBRACK);
				}
				}
				setState(238);
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
	}

	public final VariableInitializerContext variableInitializer() throws RecognitionException {
		VariableInitializerContext _localctx = new VariableInitializerContext(_ctx, getState());
		enterRule(_localctx, 40, RULE_variableInitializer);
		try {
			setState(241);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case LBRACE:
				enterOuterAlt(_localctx, 1);
				{
				setState(239);
				arrayInitializer();
				}
				break;
			case BOOLEAN:
			case DECIMAL_NUMBER:
			case NEW:
			case NUMBER:
			case SUPER:
			case THIS:
			case TEXT_STRING:
			case WHOLE_NUMBER:
			case DECIMAL_LITERAL:
			case FLOAT_LITERAL:
			case BOOL_LITERAL:
			case STRING_LITERAL:
			case NULL_LITERAL:
			case LPAREN:
			case BANG:
			case ADD:
			case SUB:
			case IDENTIFIER:
				enterOuterAlt(_localctx, 2);
				{
				setState(240);
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
		public UnlabeledExpressionListContext unlabeledExpressionList() {
			return getRuleContext(UnlabeledExpressionListContext.class,0);
		}
		public ArrayInitializerContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_arrayInitializer; }
	}

	public final ArrayInitializerContext arrayInitializer() throws RecognitionException {
		ArrayInitializerContext _localctx = new ArrayInitializerContext(_ctx, getState());
		enterRule(_localctx, 42, RULE_arrayInitializer);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(243);
			match(LBRACE);
			setState(245);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << BOOLEAN) | (1L << DECIMAL_NUMBER) | (1L << NEW) | (1L << NUMBER) | (1L << SUPER) | (1L << THIS) | (1L << TEXT_STRING) | (1L << WHOLE_NUMBER) | (1L << DECIMAL_LITERAL) | (1L << FLOAT_LITERAL) | (1L << BOOL_LITERAL) | (1L << STRING_LITERAL) | (1L << NULL_LITERAL) | (1L << LPAREN) | (1L << BANG) | (1L << ADD) | (1L << SUB))) != 0) || _la==IDENTIFIER) {
				{
				setState(244);
				unlabeledExpressionList();
				}
			}

			setState(247);
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

	public static class ClassTypeContext extends ParserRuleContext {
		public TerminalNode IDENTIFIER() { return getToken(TweedleParser.IDENTIFIER, 0); }
		public ClassTypeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_classType; }
	}

	public final ClassTypeContext classType() throws RecognitionException {
		ClassTypeContext _localctx = new ClassTypeContext(_ctx, getState());
		enterRule(_localctx, 44, RULE_classType);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(249);
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

	public static class FormalParametersContext extends ParserRuleContext {
		public FormalParameterListContext formalParameterList() {
			return getRuleContext(FormalParameterListContext.class,0);
		}
		public FormalParametersContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_formalParameters; }
	}

	public final FormalParametersContext formalParameters() throws RecognitionException {
		FormalParametersContext _localctx = new FormalParametersContext(_ctx, getState());
		enterRule(_localctx, 46, RULE_formalParameters);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(251);
			match(LPAREN);
			setState(253);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << BOOLEAN) | (1L << DECIMAL_NUMBER) | (1L << NUMBER) | (1L << TEXT_STRING) | (1L << WHOLE_NUMBER) | (1L << LT))) != 0) || _la==IDENTIFIER) {
				{
				setState(252);
				formalParameterList();
				}
			}

			setState(255);
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
	}

	public final FormalParameterListContext formalParameterList() throws RecognitionException {
		FormalParameterListContext _localctx = new FormalParameterListContext(_ctx, getState());
		enterRule(_localctx, 48, RULE_formalParameterList);
		int _la;
		try {
			int _alt;
			setState(289);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,29,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(257);
				requiredParameter();
				setState(262);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,24,_ctx);
				while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
					if ( _alt==1 ) {
						{
						{
						setState(258);
						match(COMMA);
						setState(259);
						requiredParameter();
						}
						} 
					}
					setState(264);
					_errHandler.sync(this);
					_alt = getInterpreter().adaptivePredict(_input,24,_ctx);
				}
				setState(269);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,25,_ctx);
				while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
					if ( _alt==1 ) {
						{
						{
						setState(265);
						match(COMMA);
						setState(266);
						optionalParameter();
						}
						} 
					}
					setState(271);
					_errHandler.sync(this);
					_alt = getInterpreter().adaptivePredict(_input,25,_ctx);
				}
				setState(274);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==COMMA) {
					{
					setState(272);
					match(COMMA);
					setState(273);
					lastFormalParameter();
					}
				}

				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(276);
				optionalParameter();
				setState(281);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,27,_ctx);
				while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
					if ( _alt==1 ) {
						{
						{
						setState(277);
						match(COMMA);
						setState(278);
						optionalParameter();
						}
						} 
					}
					setState(283);
					_errHandler.sync(this);
					_alt = getInterpreter().adaptivePredict(_input,27,_ctx);
				}
				setState(286);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==COMMA) {
					{
					setState(284);
					match(COMMA);
					setState(285);
					lastFormalParameter();
					}
				}

				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(288);
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
		public RequiredParameterContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_requiredParameter; }
	}

	public final RequiredParameterContext requiredParameter() throws RecognitionException {
		RequiredParameterContext _localctx = new RequiredParameterContext(_ctx, getState());
		enterRule(_localctx, 50, RULE_requiredParameter);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(291);
			typeType();
			setState(292);
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
		public OptionalParameterContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_optionalParameter; }
	}

	public final OptionalParameterContext optionalParameter() throws RecognitionException {
		OptionalParameterContext _localctx = new OptionalParameterContext(_ctx, getState());
		enterRule(_localctx, 52, RULE_optionalParameter);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(294);
			typeType();
			setState(295);
			variableDeclaratorId();
			setState(296);
			match(LARROW);
			setState(297);
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
		public LastFormalParameterContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_lastFormalParameter; }
	}

	public final LastFormalParameterContext lastFormalParameter() throws RecognitionException {
		LastFormalParameterContext _localctx = new LastFormalParameterContext(_ctx, getState());
		enterRule(_localctx, 54, RULE_lastFormalParameter);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(299);
			typeType();
			setState(300);
			match(ELLIPSIS);
			setState(301);
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
	}

	public final LiteralContext literal() throws RecognitionException {
		LiteralContext _localctx = new LiteralContext(_ctx, getState());
		enterRule(_localctx, 56, RULE_literal);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(303);
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
	}

	public final BlockContext block() throws RecognitionException {
		BlockContext _localctx = new BlockContext(_ctx, getState());
		enterRule(_localctx, 58, RULE_block);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(305);
			match(LBRACE);
			setState(309);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << BOOLEAN) | (1L << COUNT_UP_TO) | (1L << CONSTANT) | (1L << DECIMAL_NUMBER) | (1L << DO_IN_ORDER) | (1L << DO_TOGETHER) | (1L << FOR_EACH) | (1L << EACH_TOGETHER) | (1L << IF) | (1L << NEW) | (1L << NUMBER) | (1L << RETURN) | (1L << SUPER) | (1L << THIS) | (1L << TEXT_STRING) | (1L << WHILE) | (1L << WHOLE_NUMBER) | (1L << DECIMAL_LITERAL) | (1L << FLOAT_LITERAL) | (1L << BOOL_LITERAL) | (1L << STRING_LITERAL) | (1L << NULL_LITERAL) | (1L << LPAREN) | (1L << LT) | (1L << BANG) | (1L << ADD) | (1L << SUB))) != 0) || _la==NODE_DISABLE || _la==IDENTIFIER) {
				{
				{
				setState(306);
				blockStatement();
				}
				}
				setState(311);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(312);
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
		public TerminalNode NODE_DISABLE() { return getToken(TweedleParser.NODE_DISABLE, 0); }
		public BlockStatementContext blockStatement() {
			return getRuleContext(BlockStatementContext.class,0);
		}
		public TerminalNode NODE_ENABLE() { return getToken(TweedleParser.NODE_ENABLE, 0); }
		public BlockStatementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_blockStatement; }
	}

	public final BlockStatementContext blockStatement() throws RecognitionException {
		BlockStatementContext _localctx = new BlockStatementContext(_ctx, getState());
		enterRule(_localctx, 60, RULE_blockStatement);
		try {
			setState(322);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,31,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(314);
				localVariableDeclaration();
				setState(315);
				match(SEMI);
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(317);
				statement();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(318);
				match(NODE_DISABLE);
				setState(319);
				blockStatement();
				setState(320);
				match(NODE_ENABLE);
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
		public VariableDeclaratorContext variableDeclarator() {
			return getRuleContext(VariableDeclaratorContext.class,0);
		}
		public TerminalNode CONSTANT() { return getToken(TweedleParser.CONSTANT, 0); }
		public LocalVariableDeclarationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_localVariableDeclaration; }
	}

	public final LocalVariableDeclarationContext localVariableDeclaration() throws RecognitionException {
		LocalVariableDeclarationContext _localctx = new LocalVariableDeclarationContext(_ctx, getState());
		enterRule(_localctx, 62, RULE_localVariableDeclaration);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(325);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==CONSTANT) {
				{
				setState(324);
				match(CONSTANT);
				}
			}

			setState(327);
			typeType();
			setState(328);
			variableDeclarator();
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
		public ExpressionContext statementExpression;
		public TerminalNode COUNT_UP_TO() { return getToken(TweedleParser.COUNT_UP_TO, 0); }
		public TerminalNode IDENTIFIER() { return getToken(TweedleParser.IDENTIFIER, 0); }
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public List<BlockContext> block() {
			return getRuleContexts(BlockContext.class);
		}
		public BlockContext block(int i) {
			return getRuleContext(BlockContext.class,i);
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
		public StatementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_statement; }
	}

	public final StatementContext statement() throws RecognitionException {
		StatementContext _localctx = new StatementContext(_ctx, getState());
		enterRule(_localctx, 64, RULE_statement);
		int _la;
		try {
			setState(373);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case COUNT_UP_TO:
				enterOuterAlt(_localctx, 1);
				{
				setState(330);
				match(COUNT_UP_TO);
				setState(331);
				match(LPAREN);
				setState(332);
				match(IDENTIFIER);
				setState(333);
				match(LT);
				setState(334);
				expression(0);
				setState(335);
				match(RPAREN);
				setState(336);
				block();
				}
				break;
			case IF:
				enterOuterAlt(_localctx, 2);
				{
				setState(338);
				match(IF);
				setState(339);
				parExpression();
				setState(340);
				block();
				setState(343);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==ELSE) {
					{
					setState(341);
					match(ELSE);
					setState(342);
					block();
					}
				}

				}
				break;
			case FOR_EACH:
				enterOuterAlt(_localctx, 3);
				{
				setState(345);
				match(FOR_EACH);
				setState(346);
				match(LPAREN);
				setState(347);
				forControl();
				setState(348);
				match(RPAREN);
				setState(349);
				block();
				}
				break;
			case EACH_TOGETHER:
				enterOuterAlt(_localctx, 4);
				{
				setState(351);
				match(EACH_TOGETHER);
				setState(352);
				match(LPAREN);
				setState(353);
				forControl();
				setState(354);
				match(RPAREN);
				setState(355);
				block();
				}
				break;
			case WHILE:
				enterOuterAlt(_localctx, 5);
				{
				setState(357);
				match(WHILE);
				setState(358);
				parExpression();
				setState(359);
				block();
				}
				break;
			case DO_IN_ORDER:
				enterOuterAlt(_localctx, 6);
				{
				setState(361);
				match(DO_IN_ORDER);
				setState(362);
				block();
				}
				break;
			case DO_TOGETHER:
				enterOuterAlt(_localctx, 7);
				{
				setState(363);
				match(DO_TOGETHER);
				setState(364);
				block();
				}
				break;
			case RETURN:
				enterOuterAlt(_localctx, 8);
				{
				setState(365);
				match(RETURN);
				setState(367);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << BOOLEAN) | (1L << DECIMAL_NUMBER) | (1L << NEW) | (1L << NUMBER) | (1L << SUPER) | (1L << THIS) | (1L << TEXT_STRING) | (1L << WHOLE_NUMBER) | (1L << DECIMAL_LITERAL) | (1L << FLOAT_LITERAL) | (1L << BOOL_LITERAL) | (1L << STRING_LITERAL) | (1L << NULL_LITERAL) | (1L << LPAREN) | (1L << BANG) | (1L << ADD) | (1L << SUB))) != 0) || _la==IDENTIFIER) {
					{
					setState(366);
					expression(0);
					}
				}

				setState(369);
				match(SEMI);
				}
				break;
			case BOOLEAN:
			case DECIMAL_NUMBER:
			case NEW:
			case NUMBER:
			case SUPER:
			case THIS:
			case TEXT_STRING:
			case WHOLE_NUMBER:
			case DECIMAL_LITERAL:
			case FLOAT_LITERAL:
			case BOOL_LITERAL:
			case STRING_LITERAL:
			case NULL_LITERAL:
			case LPAREN:
			case BANG:
			case ADD:
			case SUB:
			case IDENTIFIER:
				enterOuterAlt(_localctx, 9);
				{
				setState(370);
				((StatementContext)_localctx).statementExpression = expression(0);
				setState(371);
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
	}

	public final ForControlContext forControl() throws RecognitionException {
		ForControlContext _localctx = new ForControlContext(_ctx, getState());
		enterRule(_localctx, 66, RULE_forControl);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(375);
			typeType();
			setState(376);
			variableDeclaratorId();
			setState(377);
			match(IN);
			setState(378);
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
	}

	public final ParExpressionContext parExpression() throws RecognitionException {
		ParExpressionContext _localctx = new ParExpressionContext(_ctx, getState());
		enterRule(_localctx, 68, RULE_parExpression);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(380);
			match(LPAREN);
			setState(381);
			expression(0);
			setState(382);
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

	public static class UnlabeledExpressionListContext extends ParserRuleContext {
		public List<ExpressionContext> expression() {
			return getRuleContexts(ExpressionContext.class);
		}
		public ExpressionContext expression(int i) {
			return getRuleContext(ExpressionContext.class,i);
		}
		public UnlabeledExpressionListContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_unlabeledExpressionList; }
	}

	public final UnlabeledExpressionListContext unlabeledExpressionList() throws RecognitionException {
		UnlabeledExpressionListContext _localctx = new UnlabeledExpressionListContext(_ctx, getState());
		enterRule(_localctx, 70, RULE_unlabeledExpressionList);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(384);
			expression(0);
			setState(389);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(385);
				match(COMMA);
				setState(386);
				expression(0);
				}
				}
				setState(391);
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
	}

	public final LabeledExpressionListContext labeledExpressionList() throws RecognitionException {
		LabeledExpressionListContext _localctx = new LabeledExpressionListContext(_ctx, getState());
		enterRule(_localctx, 72, RULE_labeledExpressionList);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(392);
			labeledExpression();
			setState(397);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(393);
				match(COMMA);
				setState(394);
				labeledExpression();
				}
				}
				setState(399);
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
	}

	public final LabeledExpressionContext labeledExpression() throws RecognitionException {
		LabeledExpressionContext _localctx = new LabeledExpressionContext(_ctx, getState());
		enterRule(_localctx, 74, RULE_labeledExpression);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(400);
			((LabeledExpressionContext)_localctx).expressionLabel = match(IDENTIFIER);
			setState(401);
			match(COLON);
			setState(402);
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

	public static class MethodCallContext extends ParserRuleContext {
		public TerminalNode IDENTIFIER() { return getToken(TweedleParser.IDENTIFIER, 0); }
		public LabeledExpressionListContext labeledExpressionList() {
			return getRuleContext(LabeledExpressionListContext.class,0);
		}
		public MethodCallContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_methodCall; }
	}

	public final MethodCallContext methodCall() throws RecognitionException {
		MethodCallContext _localctx = new MethodCallContext(_ctx, getState());
		enterRule(_localctx, 76, RULE_methodCall);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(404);
			match(IDENTIFIER);
			setState(405);
			match(LPAREN);
			setState(407);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==IDENTIFIER) {
				{
				setState(406);
				labeledExpressionList();
				}
			}

			setState(409);
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

	public static class LambdaCallContext extends ParserRuleContext {
		public UnlabeledExpressionListContext unlabeledExpressionList() {
			return getRuleContext(UnlabeledExpressionListContext.class,0);
		}
		public LambdaCallContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_lambdaCall; }
	}

	public final LambdaCallContext lambdaCall() throws RecognitionException {
		LambdaCallContext _localctx = new LambdaCallContext(_ctx, getState());
		enterRule(_localctx, 78, RULE_lambdaCall);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(411);
			match(LPAREN);
			setState(414);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,39,_ctx) ) {
			case 1:
				{
				setState(412);
				match(SUB);
				}
				break;
			case 2:
				{
				setState(413);
				unlabeledExpressionList();
				}
				break;
			}
			setState(416);
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
		public Token bracket;
		public PrimaryContext primary() {
			return getRuleContext(PrimaryContext.class,0);
		}
		public TerminalNode NEW() { return getToken(TweedleParser.NEW, 0); }
		public CreatorContext creator() {
			return getRuleContext(CreatorContext.class,0);
		}
		public MethodCallContext methodCall() {
			return getRuleContext(MethodCallContext.class,0);
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
		public TerminalNode LARROW() { return getToken(TweedleParser.LARROW, 0); }
		public TerminalNode IDENTIFIER() { return getToken(TweedleParser.IDENTIFIER, 0); }
		public LambdaCallContext lambdaCall() {
			return getRuleContext(LambdaCallContext.class,0);
		}
		public ExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_expression; }
	}

	public final ExpressionContext expression() throws RecognitionException {
		return expression(0);
	}

	private ExpressionContext expression(int _p) throws RecognitionException {
		ParserRuleContext _parentctx = _ctx;
		int _parentState = getState();
		ExpressionContext _localctx = new ExpressionContext(_ctx, _parentState);
		ExpressionContext _prevctx = _localctx;
		int _startState = 80;
		enterRecursionRule(_localctx, 80, RULE_expression, _p);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(428);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,40,_ctx) ) {
			case 1:
				{
				setState(419);
				primary();
				}
				break;
			case 2:
				{
				setState(420);
				match(NEW);
				setState(421);
				creator();
				}
				break;
			case 3:
				{
				setState(422);
				methodCall();
				}
				break;
			case 4:
				{
				setState(423);
				((ExpressionContext)_localctx).prefix = _input.LT(1);
				_la = _input.LA(1);
				if ( !(_la==ADD || _la==SUB) ) {
					((ExpressionContext)_localctx).prefix = (Token)_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				setState(424);
				expression(11);
				}
				break;
			case 5:
				{
				setState(425);
				((ExpressionContext)_localctx).prefix = match(BANG);
				setState(426);
				expression(10);
				}
				break;
			case 6:
				{
				setState(427);
				lambdaExpression();
				}
				break;
			}
			_ctx.stop = _input.LT(-1);
			setState(469);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,43,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) triggerExitRuleEvent();
					_prevctx = _localctx;
					{
					setState(467);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,42,_ctx) ) {
					case 1:
						{
						_localctx = new ExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(430);
						if (!(precpred(_ctx, 9))) throw new FailedPredicateException(this, "precpred(_ctx, 9)");
						setState(431);
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
						setState(432);
						expression(10);
						}
						break;
					case 2:
						{
						_localctx = new ExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(433);
						if (!(precpred(_ctx, 8))) throw new FailedPredicateException(this, "precpred(_ctx, 8)");
						setState(434);
						((ExpressionContext)_localctx).bop = match(CONCAT);
						setState(435);
						expression(9);
						}
						break;
					case 3:
						{
						_localctx = new ExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(436);
						if (!(precpred(_ctx, 7))) throw new FailedPredicateException(this, "precpred(_ctx, 7)");
						setState(437);
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
						setState(438);
						expression(8);
						}
						break;
					case 4:
						{
						_localctx = new ExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(439);
						if (!(precpred(_ctx, 6))) throw new FailedPredicateException(this, "precpred(_ctx, 6)");
						setState(440);
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
						setState(441);
						expression(7);
						}
						break;
					case 5:
						{
						_localctx = new ExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(442);
						if (!(precpred(_ctx, 5))) throw new FailedPredicateException(this, "precpred(_ctx, 5)");
						setState(443);
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
						setState(444);
						expression(6);
						}
						break;
					case 6:
						{
						_localctx = new ExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(445);
						if (!(precpred(_ctx, 4))) throw new FailedPredicateException(this, "precpred(_ctx, 4)");
						setState(446);
						((ExpressionContext)_localctx).bop = match(AND);
						setState(447);
						expression(5);
						}
						break;
					case 7:
						{
						_localctx = new ExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(448);
						if (!(precpred(_ctx, 3))) throw new FailedPredicateException(this, "precpred(_ctx, 3)");
						setState(449);
						((ExpressionContext)_localctx).bop = match(OR);
						setState(450);
						expression(4);
						}
						break;
					case 8:
						{
						_localctx = new ExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(451);
						if (!(precpred(_ctx, 2))) throw new FailedPredicateException(this, "precpred(_ctx, 2)");
						setState(452);
						((ExpressionContext)_localctx).bop = match(LARROW);
						setState(453);
						expression(2);
						}
						break;
					case 9:
						{
						_localctx = new ExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(454);
						if (!(precpred(_ctx, 16))) throw new FailedPredicateException(this, "precpred(_ctx, 16)");
						setState(455);
						((ExpressionContext)_localctx).bop = match(DOT);
						setState(458);
						_errHandler.sync(this);
						switch ( getInterpreter().adaptivePredict(_input,41,_ctx) ) {
						case 1:
							{
							setState(456);
							match(IDENTIFIER);
							}
							break;
						case 2:
							{
							setState(457);
							methodCall();
							}
							break;
						}
						}
						break;
					case 10:
						{
						_localctx = new ExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(460);
						if (!(precpred(_ctx, 15))) throw new FailedPredicateException(this, "precpred(_ctx, 15)");
						setState(461);
						((ExpressionContext)_localctx).bracket = match(LBRACK);
						setState(462);
						expression(0);
						setState(463);
						match(RBRACK);
						}
						break;
					case 11:
						{
						_localctx = new ExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(465);
						if (!(precpred(_ctx, 13))) throw new FailedPredicateException(this, "precpred(_ctx, 13)");
						setState(466);
						lambdaCall();
						}
						break;
					}
					} 
				}
				setState(471);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,43,_ctx);
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
		public BlockContext block() {
			return getRuleContext(BlockContext.class,0);
		}
		public LambdaExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_lambdaExpression; }
	}

	public final LambdaExpressionContext lambdaExpression() throws RecognitionException {
		LambdaExpressionContext _localctx = new LambdaExpressionContext(_ctx, getState());
		enterRule(_localctx, 82, RULE_lambdaExpression);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(472);
			lambdaParameters();
			setState(473);
			match(ARROW);
			setState(474);
			block();
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
		public List<RequiredParameterContext> requiredParameter() {
			return getRuleContexts(RequiredParameterContext.class);
		}
		public RequiredParameterContext requiredParameter(int i) {
			return getRuleContext(RequiredParameterContext.class,i);
		}
		public LambdaParametersContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_lambdaParameters; }
	}

	public final LambdaParametersContext lambdaParameters() throws RecognitionException {
		LambdaParametersContext _localctx = new LambdaParametersContext(_ctx, getState());
		enterRule(_localctx, 84, RULE_lambdaParameters);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(476);
			match(LPAREN);
			setState(485);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << BOOLEAN) | (1L << DECIMAL_NUMBER) | (1L << NUMBER) | (1L << TEXT_STRING) | (1L << WHOLE_NUMBER) | (1L << LT))) != 0) || _la==IDENTIFIER) {
				{
				setState(477);
				requiredParameter();
				setState(482);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==COMMA) {
					{
					{
					setState(478);
					match(COMMA);
					setState(479);
					requiredParameter();
					}
					}
					setState(484);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				}
			}

			setState(487);
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

	public static class LambdaTypeSignatureContext extends ParserRuleContext {
		public TypeListContext typeList() {
			return getRuleContext(TypeListContext.class,0);
		}
		public TypeTypeOrVoidContext typeTypeOrVoid() {
			return getRuleContext(TypeTypeOrVoidContext.class,0);
		}
		public LambdaTypeSignatureContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_lambdaTypeSignature; }
	}

	public final LambdaTypeSignatureContext lambdaTypeSignature() throws RecognitionException {
		LambdaTypeSignatureContext _localctx = new LambdaTypeSignatureContext(_ctx, getState());
		enterRule(_localctx, 86, RULE_lambdaTypeSignature);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(489);
			match(LT);
			setState(490);
			typeList();
			setState(491);
			match(ARROW);
			setState(492);
			typeTypeOrVoid();
			setState(493);
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
	}

	public final TypeListContext typeList() throws RecognitionException {
		TypeListContext _localctx = new TypeListContext(_ctx, getState());
		enterRule(_localctx, 88, RULE_typeList);
		int _la;
		try {
			setState(509);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,47,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(495);
				match(LPAREN);
				setState(496);
				match(RPAREN);
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(497);
				typeType();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(498);
				match(LPAREN);
				setState(499);
				typeType();
				setState(504);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==COMMA) {
					{
					{
					setState(500);
					match(COMMA);
					setState(501);
					typeType();
					}
					}
					setState(506);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				setState(507);
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

	public static class PrimaryContext extends ParserRuleContext {
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public TerminalNode THIS() { return getToken(TweedleParser.THIS, 0); }
		public TerminalNode SUPER() { return getToken(TweedleParser.SUPER, 0); }
		public SuperSuffixContext superSuffix() {
			return getRuleContext(SuperSuffixContext.class,0);
		}
		public LiteralContext literal() {
			return getRuleContext(LiteralContext.class,0);
		}
		public TerminalNode IDENTIFIER() { return getToken(TweedleParser.IDENTIFIER, 0); }
		public PrimitiveTypeContext primitiveType() {
			return getRuleContext(PrimitiveTypeContext.class,0);
		}
		public PrimaryContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_primary; }
	}

	public final PrimaryContext primary() throws RecognitionException {
		PrimaryContext _localctx = new PrimaryContext(_ctx, getState());
		enterRule(_localctx, 90, RULE_primary);
		try {
			setState(521);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case LPAREN:
				enterOuterAlt(_localctx, 1);
				{
				setState(511);
				match(LPAREN);
				setState(512);
				expression(0);
				setState(513);
				match(RPAREN);
				}
				break;
			case THIS:
				enterOuterAlt(_localctx, 2);
				{
				setState(515);
				match(THIS);
				}
				break;
			case SUPER:
				enterOuterAlt(_localctx, 3);
				{
				setState(516);
				match(SUPER);
				setState(517);
				superSuffix();
				}
				break;
			case DECIMAL_LITERAL:
			case FLOAT_LITERAL:
			case BOOL_LITERAL:
			case STRING_LITERAL:
			case NULL_LITERAL:
				enterOuterAlt(_localctx, 4);
				{
				setState(518);
				literal();
				}
				break;
			case IDENTIFIER:
				enterOuterAlt(_localctx, 5);
				{
				setState(519);
				match(IDENTIFIER);
				}
				break;
			case BOOLEAN:
			case DECIMAL_NUMBER:
			case NUMBER:
			case TEXT_STRING:
			case WHOLE_NUMBER:
				enterOuterAlt(_localctx, 6);
				{
				setState(520);
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

	public static class CreatorContext extends ParserRuleContext {
		public CreatedNameContext createdName() {
			return getRuleContext(CreatedNameContext.class,0);
		}
		public ArrayCreatorRestContext arrayCreatorRest() {
			return getRuleContext(ArrayCreatorRestContext.class,0);
		}
		public ClassCreatorRestContext classCreatorRest() {
			return getRuleContext(ClassCreatorRestContext.class,0);
		}
		public CreatorContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_creator; }
	}

	public final CreatorContext creator() throws RecognitionException {
		CreatorContext _localctx = new CreatorContext(_ctx, getState());
		enterRule(_localctx, 92, RULE_creator);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(523);
			createdName();
			setState(526);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case LBRACK:
				{
				setState(524);
				arrayCreatorRest();
				}
				break;
			case LPAREN:
				{
				setState(525);
				classCreatorRest();
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

	public static class CreatedNameContext extends ParserRuleContext {
		public TerminalNode IDENTIFIER() { return getToken(TweedleParser.IDENTIFIER, 0); }
		public PrimitiveTypeContext primitiveType() {
			return getRuleContext(PrimitiveTypeContext.class,0);
		}
		public CreatedNameContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_createdName; }
	}

	public final CreatedNameContext createdName() throws RecognitionException {
		CreatedNameContext _localctx = new CreatedNameContext(_ctx, getState());
		enterRule(_localctx, 94, RULE_createdName);
		try {
			setState(530);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case IDENTIFIER:
				enterOuterAlt(_localctx, 1);
				{
				setState(528);
				match(IDENTIFIER);
				}
				break;
			case BOOLEAN:
			case DECIMAL_NUMBER:
			case NUMBER:
			case TEXT_STRING:
			case WHOLE_NUMBER:
				enterOuterAlt(_localctx, 2);
				{
				setState(529);
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

	public static class ArrayCreatorRestContext extends ParserRuleContext {
		public ArrayInitializerContext arrayInitializer() {
			return getRuleContext(ArrayInitializerContext.class,0);
		}
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public ArrayCreatorRestContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_arrayCreatorRest; }
	}

	public final ArrayCreatorRestContext arrayCreatorRest() throws RecognitionException {
		ArrayCreatorRestContext _localctx = new ArrayCreatorRestContext(_ctx, getState());
		enterRule(_localctx, 96, RULE_arrayCreatorRest);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(532);
			match(LBRACK);
			setState(538);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case RBRACK:
				{
				setState(533);
				match(RBRACK);
				setState(534);
				arrayInitializer();
				}
				break;
			case BOOLEAN:
			case DECIMAL_NUMBER:
			case NEW:
			case NUMBER:
			case SUPER:
			case THIS:
			case TEXT_STRING:
			case WHOLE_NUMBER:
			case DECIMAL_LITERAL:
			case FLOAT_LITERAL:
			case BOOL_LITERAL:
			case STRING_LITERAL:
			case NULL_LITERAL:
			case LPAREN:
			case BANG:
			case ADD:
			case SUB:
			case IDENTIFIER:
				{
				setState(535);
				expression(0);
				setState(536);
				match(RBRACK);
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
		public ClassCreatorRestContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_classCreatorRest; }
	}

	public final ClassCreatorRestContext classCreatorRest() throws RecognitionException {
		ClassCreatorRestContext _localctx = new ClassCreatorRestContext(_ctx, getState());
		enterRule(_localctx, 98, RULE_classCreatorRest);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(540);
			arguments();
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
		public ClassTypeContext classType() {
			return getRuleContext(ClassTypeContext.class,0);
		}
		public PrimitiveTypeContext primitiveType() {
			return getRuleContext(PrimitiveTypeContext.class,0);
		}
		public LambdaTypeSignatureContext lambdaTypeSignature() {
			return getRuleContext(LambdaTypeSignatureContext.class,0);
		}
		public TypeTypeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_typeType; }
	}

	public final TypeTypeContext typeType() throws RecognitionException {
		TypeTypeContext _localctx = new TypeTypeContext(_ctx, getState());
		enterRule(_localctx, 100, RULE_typeType);
		int _la;
		try {
			setState(551);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case BOOLEAN:
			case DECIMAL_NUMBER:
			case NUMBER:
			case TEXT_STRING:
			case WHOLE_NUMBER:
			case IDENTIFIER:
				enterOuterAlt(_localctx, 1);
				{
				setState(544);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case IDENTIFIER:
					{
					setState(542);
					classType();
					}
					break;
				case BOOLEAN:
				case DECIMAL_NUMBER:
				case NUMBER:
				case TEXT_STRING:
				case WHOLE_NUMBER:
					{
					setState(543);
					primitiveType();
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				setState(548);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==LBRACK) {
					{
					setState(546);
					match(LBRACK);
					setState(547);
					match(RBRACK);
					}
				}

				}
				break;
			case LT:
				enterOuterAlt(_localctx, 2);
				{
				setState(550);
				lambdaTypeSignature();
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

	public static class PrimitiveTypeContext extends ParserRuleContext {
		public TerminalNode BOOLEAN() { return getToken(TweedleParser.BOOLEAN, 0); }
		public TerminalNode DECIMAL_NUMBER() { return getToken(TweedleParser.DECIMAL_NUMBER, 0); }
		public TerminalNode WHOLE_NUMBER() { return getToken(TweedleParser.WHOLE_NUMBER, 0); }
		public TerminalNode NUMBER() { return getToken(TweedleParser.NUMBER, 0); }
		public TerminalNode TEXT_STRING() { return getToken(TweedleParser.TEXT_STRING, 0); }
		public PrimitiveTypeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_primitiveType; }
	}

	public final PrimitiveTypeContext primitiveType() throws RecognitionException {
		PrimitiveTypeContext _localctx = new PrimitiveTypeContext(_ctx, getState());
		enterRule(_localctx, 102, RULE_primitiveType);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(553);
			_la = _input.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << BOOLEAN) | (1L << DECIMAL_NUMBER) | (1L << NUMBER) | (1L << TEXT_STRING) | (1L << WHOLE_NUMBER))) != 0)) ) {
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

	public static class SuperSuffixContext extends ParserRuleContext {
		public ArgumentsContext arguments() {
			return getRuleContext(ArgumentsContext.class,0);
		}
		public TerminalNode IDENTIFIER() { return getToken(TweedleParser.IDENTIFIER, 0); }
		public SuperSuffixContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_superSuffix; }
	}

	public final SuperSuffixContext superSuffix() throws RecognitionException {
		SuperSuffixContext _localctx = new SuperSuffixContext(_ctx, getState());
		enterRule(_localctx, 104, RULE_superSuffix);
		try {
			setState(561);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case LPAREN:
				enterOuterAlt(_localctx, 1);
				{
				setState(555);
				arguments();
				}
				break;
			case DOT:
				enterOuterAlt(_localctx, 2);
				{
				setState(556);
				match(DOT);
				setState(557);
				match(IDENTIFIER);
				setState(559);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,55,_ctx) ) {
				case 1:
					{
					setState(558);
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

	public static class ArgumentsContext extends ParserRuleContext {
		public LabeledExpressionListContext labeledExpressionList() {
			return getRuleContext(LabeledExpressionListContext.class,0);
		}
		public ArgumentsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_arguments; }
	}

	public final ArgumentsContext arguments() throws RecognitionException {
		ArgumentsContext _localctx = new ArgumentsContext(_ctx, getState());
		enterRule(_localctx, 106, RULE_arguments);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(563);
			match(LPAREN);
			setState(565);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==IDENTIFIER) {
				{
				setState(564);
				labeledExpressionList();
				}
			}

			setState(567);
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
		case 40:
			return expression_sempred((ExpressionContext)_localctx, predIndex);
		}
		return true;
	}
	private boolean expression_sempred(ExpressionContext _localctx, int predIndex) {
		switch (predIndex) {
		case 0:
			return precpred(_ctx, 9);
		case 1:
			return precpred(_ctx, 8);
		case 2:
			return precpred(_ctx, 7);
		case 3:
			return precpred(_ctx, 6);
		case 4:
			return precpred(_ctx, 5);
		case 5:
			return precpred(_ctx, 4);
		case 6:
			return precpred(_ctx, 3);
		case 7:
			return precpred(_ctx, 2);
		case 8:
			return precpred(_ctx, 16);
		case 9:
			return precpred(_ctx, 15);
		case 10:
			return precpred(_ctx, 13);
		}
		return true;
	}

	public static final String _serializedATN =
		"\3\u608b\ua72a\u8133\ub9ed\u417c\u3be7\u7786\u5964\3\\\u023c\4\2\t\2\4"+
		"\3\t\3\4\4\t\4\4\5\t\5\4\6\t\6\4\7\t\7\4\b\t\b\4\t\t\t\4\n\t\n\4\13\t"+
		"\13\4\f\t\f\4\r\t\r\4\16\t\16\4\17\t\17\4\20\t\20\4\21\t\21\4\22\t\22"+
		"\4\23\t\23\4\24\t\24\4\25\t\25\4\26\t\26\4\27\t\27\4\30\t\30\4\31\t\31"+
		"\4\32\t\32\4\33\t\33\4\34\t\34\4\35\t\35\4\36\t\36\4\37\t\37\4 \t \4!"+
		"\t!\4\"\t\"\4#\t#\4$\t$\4%\t%\4&\t&\4\'\t\'\4(\t(\4)\t)\4*\t*\4+\t+\4"+
		",\t,\4-\t-\4.\t.\4/\t/\4\60\t\60\4\61\t\61\4\62\t\62\4\63\t\63\4\64\t"+
		"\64\4\65\t\65\4\66\t\66\4\67\t\67\3\2\5\2p\n\2\3\2\3\2\5\2t\n\2\3\2\5"+
		"\2w\n\2\3\3\3\3\5\3{\n\3\3\4\3\4\3\4\3\5\3\5\3\6\3\6\3\6\3\6\5\6\u0086"+
		"\n\6\3\6\3\6\5\6\u008a\n\6\3\6\3\6\3\7\3\7\3\b\3\b\3\b\3\b\5\b\u0094\n"+
		"\b\3\b\5\b\u0097\n\b\3\b\5\b\u009a\n\b\3\b\3\b\3\t\3\t\3\t\7\t\u00a1\n"+
		"\t\f\t\16\t\u00a4\13\t\3\n\3\n\5\n\u00a8\n\n\3\13\3\13\7\13\u00ac\n\13"+
		"\f\13\16\13\u00af\13\13\3\f\3\f\7\f\u00b3\n\f\f\f\16\f\u00b6\13\f\3\f"+
		"\3\f\3\r\3\r\7\r\u00bc\n\r\f\r\16\r\u00bf\13\r\3\r\5\r\u00c2\n\r\3\16"+
		"\3\16\3\16\5\16\u00c7\n\16\3\17\3\17\3\17\3\17\3\17\7\17\u00ce\n\17\f"+
		"\17\16\17\u00d1\13\17\3\17\3\17\3\20\3\20\5\20\u00d7\n\20\3\21\3\21\5"+
		"\21\u00db\n\21\3\22\3\22\3\22\3\22\3\23\3\23\3\23\3\23\3\24\3\24\3\24"+
		"\5\24\u00e8\n\24\3\25\3\25\3\25\7\25\u00ed\n\25\f\25\16\25\u00f0\13\25"+
		"\3\26\3\26\5\26\u00f4\n\26\3\27\3\27\5\27\u00f8\n\27\3\27\3\27\3\30\3"+
		"\30\3\31\3\31\5\31\u0100\n\31\3\31\3\31\3\32\3\32\3\32\7\32\u0107\n\32"+
		"\f\32\16\32\u010a\13\32\3\32\3\32\7\32\u010e\n\32\f\32\16\32\u0111\13"+
		"\32\3\32\3\32\5\32\u0115\n\32\3\32\3\32\3\32\7\32\u011a\n\32\f\32\16\32"+
		"\u011d\13\32\3\32\3\32\5\32\u0121\n\32\3\32\5\32\u0124\n\32\3\33\3\33"+
		"\3\33\3\34\3\34\3\34\3\34\3\34\3\35\3\35\3\35\3\35\3\36\3\36\3\37\3\37"+
		"\7\37\u0136\n\37\f\37\16\37\u0139\13\37\3\37\3\37\3 \3 \3 \3 \3 \3 \3"+
		" \3 \5 \u0145\n \3!\5!\u0148\n!\3!\3!\3!\3\"\3\"\3\"\3\"\3\"\3\"\3\"\3"+
		"\"\3\"\3\"\3\"\3\"\3\"\5\"\u015a\n\"\3\"\3\"\3\"\3\"\3\"\3\"\3\"\3\"\3"+
		"\"\3\"\3\"\3\"\3\"\3\"\3\"\3\"\3\"\3\"\3\"\3\"\3\"\3\"\5\"\u0172\n\"\3"+
		"\"\3\"\3\"\3\"\5\"\u0178\n\"\3#\3#\3#\3#\3#\3$\3$\3$\3$\3%\3%\3%\7%\u0186"+
		"\n%\f%\16%\u0189\13%\3&\3&\3&\7&\u018e\n&\f&\16&\u0191\13&\3\'\3\'\3\'"+
		"\3\'\3(\3(\3(\5(\u019a\n(\3(\3(\3)\3)\3)\5)\u01a1\n)\3)\3)\3*\3*\3*\3"+
		"*\3*\3*\3*\3*\3*\3*\5*\u01af\n*\3*\3*\3*\3*\3*\3*\3*\3*\3*\3*\3*\3*\3"+
		"*\3*\3*\3*\3*\3*\3*\3*\3*\3*\3*\3*\3*\3*\3*\3*\5*\u01cd\n*\3*\3*\3*\3"+
		"*\3*\3*\3*\7*\u01d6\n*\f*\16*\u01d9\13*\3+\3+\3+\3+\3,\3,\3,\3,\7,\u01e3"+
		"\n,\f,\16,\u01e6\13,\5,\u01e8\n,\3,\3,\3-\3-\3-\3-\3-\3-\3.\3.\3.\3.\3"+
		".\3.\3.\7.\u01f9\n.\f.\16.\u01fc\13.\3.\3.\5.\u0200\n.\3/\3/\3/\3/\3/"+
		"\3/\3/\3/\3/\3/\5/\u020c\n/\3\60\3\60\3\60\5\60\u0211\n\60\3\61\3\61\5"+
		"\61\u0215\n\61\3\62\3\62\3\62\3\62\3\62\3\62\5\62\u021d\n\62\3\63\3\63"+
		"\3\64\3\64\5\64\u0223\n\64\3\64\3\64\5\64\u0227\n\64\3\64\5\64\u022a\n"+
		"\64\3\65\3\65\3\66\3\66\3\66\3\66\5\66\u0232\n\66\5\66\u0234\n\66\3\67"+
		"\3\67\5\67\u0238\n\67\3\67\3\67\3\67\2\3R8\2\4\6\b\n\f\16\20\22\24\26"+
		"\30\32\34\36 \"$&(*,.\60\62\64\668:<>@BDFHJLNPRTVXZ\\^`bdfhjl\2\t\5\2"+
		"\20\20\27\27\35\35\3\2!%\3\2>?\4\2@AEE\4\2\60\61\678\4\2\66\6699\7\2\3"+
		"\3\7\7\26\26\34\34  \2\u025b\2v\3\2\2\2\4z\3\2\2\2\6|\3\2\2\2\b\177\3"+
		"\2\2\2\n\u0081\3\2\2\2\f\u008d\3\2\2\2\16\u008f\3\2\2\2\20\u009d\3\2\2"+
		"\2\22\u00a5\3\2\2\2\24\u00a9\3\2\2\2\26\u00b0\3\2\2\2\30\u00c1\3\2\2\2"+
		"\32\u00c6\3\2\2\2\34\u00c8\3\2\2\2\36\u00d6\3\2\2\2 \u00da\3\2\2\2\"\u00dc"+
		"\3\2\2\2$\u00e0\3\2\2\2&\u00e4\3\2\2\2(\u00e9\3\2\2\2*\u00f3\3\2\2\2,"+
		"\u00f5\3\2\2\2.\u00fb\3\2\2\2\60\u00fd\3\2\2\2\62\u0123\3\2\2\2\64\u0125"+
		"\3\2\2\2\66\u0128\3\2\2\28\u012d\3\2\2\2:\u0131\3\2\2\2<\u0133\3\2\2\2"+
		">\u0144\3\2\2\2@\u0147\3\2\2\2B\u0177\3\2\2\2D\u0179\3\2\2\2F\u017e\3"+
		"\2\2\2H\u0182\3\2\2\2J\u018a\3\2\2\2L\u0192\3\2\2\2N\u0196\3\2\2\2P\u019d"+
		"\3\2\2\2R\u01ae\3\2\2\2T\u01da\3\2\2\2V\u01de\3\2\2\2X\u01eb\3\2\2\2Z"+
		"\u01ff\3\2\2\2\\\u020b\3\2\2\2^\u020d\3\2\2\2`\u0214\3\2\2\2b\u0216\3"+
		"\2\2\2d\u021e\3\2\2\2f\u0229\3\2\2\2h\u022b\3\2\2\2j\u0233\3\2\2\2l\u0235"+
		"\3\2\2\2np\5\6\4\2on\3\2\2\2op\3\2\2\2ps\3\2\2\2qt\5\n\6\2rt\5\16\b\2"+
		"sq\3\2\2\2sr\3\2\2\2tw\3\2\2\2uw\7,\2\2vo\3\2\2\2vu\3\2\2\2w\3\3\2\2\2"+
		"x{\5\6\4\2y{\7\31\2\2zx\3\2\2\2zy\3\2\2\2{\5\3\2\2\2|}\7S\2\2}~\5\b\5"+
		"\2~\7\3\2\2\2\177\u0080\t\2\2\2\u0080\t\3\2\2\2\u0081\u0082\7\4\2\2\u0082"+
		"\u0085\5\f\7\2\u0083\u0084\7\r\2\2\u0084\u0086\5f\64\2\u0085\u0083\3\2"+
		"\2\2\u0085\u0086\3\2\2\2\u0086\u0089\3\2\2\2\u0087\u0088\7\24\2\2\u0088"+
		"\u008a\7\\\2\2\u0089\u0087\3\2\2\2\u0089\u008a\3\2\2\2\u008a\u008b\3\2"+
		"\2\2\u008b\u008c\5\26\f\2\u008c\13\3\2\2\2\u008d\u008e\7\\\2\2\u008e\r"+
		"\3\2\2\2\u008f\u0090\7\f\2\2\u0090\u0091\5\f\7\2\u0091\u0093\7(\2\2\u0092"+
		"\u0094\5\20\t\2\u0093\u0092\3\2\2\2\u0093\u0094\3\2\2\2\u0094\u0096\3"+
		"\2\2\2\u0095\u0097\7-\2\2\u0096\u0095\3\2\2\2\u0096\u0097\3\2\2\2\u0097"+
		"\u0099\3\2\2\2\u0098\u009a\5\24\13\2\u0099\u0098\3\2\2\2\u0099\u009a\3"+
		"\2\2\2\u009a\u009b\3\2\2\2\u009b\u009c\7)\2\2\u009c\17\3\2\2\2\u009d\u00a2"+
		"\5\22\n\2\u009e\u009f\7-\2\2\u009f\u00a1\5\22\n\2\u00a0\u009e\3\2\2\2"+
		"\u00a1\u00a4\3\2\2\2\u00a2\u00a0\3\2\2\2\u00a2\u00a3\3\2\2\2\u00a3\21"+
		"\3\2\2\2\u00a4\u00a2\3\2\2\2\u00a5\u00a7\5\f\7\2\u00a6\u00a8\5l\67\2\u00a7"+
		"\u00a6\3\2\2\2\u00a7\u00a8\3\2\2\2\u00a8\23\3\2\2\2\u00a9\u00ad\7,\2\2"+
		"\u00aa\u00ac\5\30\r\2\u00ab\u00aa\3\2\2\2\u00ac\u00af\3\2\2\2\u00ad\u00ab"+
		"\3\2\2\2\u00ad\u00ae\3\2\2\2\u00ae\25\3\2\2\2\u00af\u00ad\3\2\2\2\u00b0"+
		"\u00b4\7(\2\2\u00b1\u00b3\5\30\r\2\u00b2\u00b1\3\2\2\2\u00b3\u00b6\3\2"+
		"\2\2\u00b4\u00b2\3\2\2\2\u00b4\u00b5\3\2\2\2\u00b5\u00b7\3\2\2\2\u00b6"+
		"\u00b4\3\2\2\2\u00b7\u00b8\7)\2\2\u00b8\27\3\2\2\2\u00b9\u00c2\7,\2\2"+
		"\u00ba\u00bc\5\4\3\2\u00bb\u00ba\3\2\2\2\u00bc\u00bf\3\2\2\2\u00bd\u00bb"+
		"\3\2\2\2\u00bd\u00be\3\2\2\2\u00be\u00c0\3\2\2\2\u00bf\u00bd\3\2\2\2\u00c0"+
		"\u00c2\5\32\16\2\u00c1\u00b9\3\2\2\2\u00c1\u00bd\3\2\2\2\u00c2\31\3\2"+
		"\2\2\u00c3\u00c7\5\34\17\2\u00c4\u00c7\5$\23\2\u00c5\u00c7\5\"\22\2\u00c6"+
		"\u00c3\3\2\2\2\u00c6\u00c4\3\2\2\2\u00c6\u00c5\3\2\2\2\u00c7\33\3\2\2"+
		"\2\u00c8\u00c9\5 \21\2\u00c9\u00ca\7\\\2\2\u00ca\u00cf\5\60\31\2\u00cb"+
		"\u00cc\7*\2\2\u00cc\u00ce\7+\2\2\u00cd\u00cb\3\2\2\2\u00ce\u00d1\3\2\2"+
		"\2\u00cf\u00cd\3\2\2\2\u00cf\u00d0\3\2\2\2\u00d0\u00d2\3\2\2\2\u00d1\u00cf"+
		"\3\2\2\2\u00d2\u00d3\5\36\20\2\u00d3\35\3\2\2\2\u00d4\u00d7\5<\37\2\u00d5"+
		"\u00d7\7,\2\2\u00d6\u00d4\3\2\2\2\u00d6\u00d5\3\2\2\2\u00d7\37\3\2\2\2"+
		"\u00d8\u00db\5f\64\2\u00d9\u00db\7\36\2\2\u00da\u00d8\3\2\2\2\u00da\u00d9"+
		"\3\2\2\2\u00db!\3\2\2\2\u00dc\u00dd\7\\\2\2\u00dd\u00de\5\60\31\2\u00de"+
		"\u00df\5<\37\2\u00df#\3\2\2\2\u00e0\u00e1\5f\64\2\u00e1\u00e2\5&\24\2"+
		"\u00e2\u00e3\7,\2\2\u00e3%\3\2\2\2\u00e4\u00e7\5(\25\2\u00e5\u00e6\7V"+
		"\2\2\u00e6\u00e8\5*\26\2\u00e7\u00e5\3\2\2\2\u00e7\u00e8\3\2\2\2\u00e8"+
		"\'\3\2\2\2\u00e9\u00ee\7\\\2\2\u00ea\u00eb\7*\2\2\u00eb\u00ed\7+\2\2\u00ec"+
		"\u00ea\3\2\2\2\u00ed\u00f0\3\2\2\2\u00ee\u00ec\3\2\2\2\u00ee\u00ef\3\2"+
		"\2\2\u00ef)\3\2\2\2\u00f0\u00ee\3\2\2\2\u00f1\u00f4\5,\27\2\u00f2\u00f4"+
		"\5R*\2\u00f3\u00f1\3\2\2\2\u00f3\u00f2\3\2\2\2\u00f4+\3\2\2\2\u00f5\u00f7"+
		"\7(\2\2\u00f6\u00f8\5H%\2\u00f7\u00f6\3\2\2\2\u00f7\u00f8\3\2\2\2\u00f8"+
		"\u00f9\3\2\2\2\u00f9\u00fa\7)\2\2\u00fa-\3\2\2\2\u00fb\u00fc\7\\\2\2\u00fc"+
		"/\3\2\2\2\u00fd\u00ff\7&\2\2\u00fe\u0100\5\62\32\2\u00ff\u00fe\3\2\2\2"+
		"\u00ff\u0100\3\2\2\2\u0100\u0101\3\2\2\2\u0101\u0102\7\'\2\2\u0102\61"+
		"\3\2\2\2\u0103\u0108\5\64\33\2\u0104\u0105\7-\2\2\u0105\u0107\5\64\33"+
		"\2\u0106\u0104\3\2\2\2\u0107\u010a\3\2\2\2\u0108\u0106\3\2\2\2\u0108\u0109"+
		"\3\2\2\2\u0109\u010f\3\2\2\2\u010a\u0108\3\2\2\2\u010b\u010c\7-\2\2\u010c"+
		"\u010e\5\66\34\2\u010d\u010b\3\2\2\2\u010e\u0111\3\2\2\2\u010f\u010d\3"+
		"\2\2\2\u010f\u0110\3\2\2\2\u0110\u0114\3\2\2\2\u0111\u010f\3\2\2\2\u0112"+
		"\u0113\7-\2\2\u0113\u0115\58\35\2\u0114\u0112\3\2\2\2\u0114\u0115\3\2"+
		"\2\2\u0115\u0124\3\2\2\2\u0116\u011b\5\66\34\2\u0117\u0118\7-\2\2\u0118"+
		"\u011a\5\66\34\2\u0119\u0117\3\2\2\2\u011a\u011d\3\2\2\2\u011b\u0119\3"+
		"\2\2\2\u011b\u011c\3\2\2\2\u011c\u0120\3\2\2\2\u011d\u011b\3\2\2\2\u011e"+
		"\u011f\7-\2\2\u011f\u0121\58\35\2\u0120\u011e\3\2\2\2\u0120\u0121\3\2"+
		"\2\2\u0121\u0124\3\2\2\2\u0122\u0124\58\35\2\u0123\u0103\3\2\2\2\u0123"+
		"\u0116\3\2\2\2\u0123\u0122\3\2\2\2\u0124\63\3\2\2\2\u0125\u0126\5f\64"+
		"\2\u0126\u0127\5(\25\2\u0127\65\3\2\2\2\u0128\u0129\5f\64\2\u0129\u012a"+
		"\5(\25\2\u012a\u012b\7V\2\2\u012b\u012c\5R*\2\u012c\67\3\2\2\2\u012d\u012e"+
		"\5f\64\2\u012e\u012f\7U\2\2\u012f\u0130\5(\25\2\u01309\3\2\2\2\u0131\u0132"+
		"\t\3\2\2\u0132;\3\2\2\2\u0133\u0137\7(\2\2\u0134\u0136\5> \2\u0135\u0134"+
		"\3\2\2\2\u0136\u0139\3\2\2\2\u0137\u0135\3\2\2\2\u0137\u0138\3\2\2\2\u0138"+
		"\u013a\3\2\2\2\u0139\u0137\3\2\2\2\u013a\u013b\7)\2\2\u013b=\3\2\2\2\u013c"+
		"\u013d\5@!\2\u013d\u013e\7,\2\2\u013e\u0145\3\2\2\2\u013f\u0145\5B\"\2"+
		"\u0140\u0141\7Z\2\2\u0141\u0142\5> \2\u0142\u0143\7[\2\2\u0143\u0145\3"+
		"\2\2\2\u0144\u013c\3\2\2\2\u0144\u013f\3\2\2\2\u0144\u0140\3\2\2\2\u0145"+
		"?\3\2\2\2\u0146\u0148\7\6\2\2\u0147\u0146\3\2\2\2\u0147\u0148\3\2\2\2"+
		"\u0148\u0149\3\2\2\2\u0149\u014a\5f\64\2\u014a\u014b\5&\24\2\u014bA\3"+
		"\2\2\2\u014c\u014d\7\5\2\2\u014d\u014e\7&\2\2\u014e\u014f\7\\\2\2\u014f"+
		"\u0150\7\61\2\2\u0150\u0151\5R*\2\u0151\u0152\7\'\2\2\u0152\u0153\5<\37"+
		"\2\u0153\u0178\3\2\2\2\u0154\u0155\7\21\2\2\u0155\u0156\5F$\2\u0156\u0159"+
		"\5<\37\2\u0157\u0158\7\13\2\2\u0158\u015a\5<\37\2\u0159\u0157\3\2\2\2"+
		"\u0159\u015a\3\2\2\2\u015a\u0178\3\2\2\2\u015b\u015c\7\16\2\2\u015c\u015d"+
		"\7&\2\2\u015d\u015e\5D#\2\u015e\u015f\7\'\2\2\u015f\u0160\5<\37\2\u0160"+
		"\u0178\3\2\2\2\u0161\u0162\7\17\2\2\u0162\u0163\7&\2\2\u0163\u0164\5D"+
		"#\2\u0164\u0165\7\'\2\2\u0165\u0166\5<\37\2\u0166\u0178\3\2\2\2\u0167"+
		"\u0168\7\37\2\2\u0168\u0169\5F$\2\u0169\u016a\5<\37\2\u016a\u0178\3\2"+
		"\2\2\u016b\u016c\7\b\2\2\u016c\u0178\5<\37\2\u016d\u016e\7\t\2\2\u016e"+
		"\u0178\5<\37\2\u016f\u0171\7\30\2\2\u0170\u0172\5R*\2\u0171\u0170\3\2"+
		"\2\2\u0171\u0172\3\2\2\2\u0172\u0173\3\2\2\2\u0173\u0178\7,\2\2\u0174"+
		"\u0175\5R*\2\u0175\u0176\7,\2\2\u0176\u0178\3\2\2\2\u0177\u014c\3\2\2"+
		"\2\u0177\u0154\3\2\2\2\u0177\u015b\3\2\2\2\u0177\u0161\3\2\2\2\u0177\u0167"+
		"\3\2\2\2\u0177\u016b\3\2\2\2\u0177\u016d\3\2\2\2\u0177\u016f\3\2\2\2\u0177"+
		"\u0174\3\2\2\2\u0178C\3\2\2\2\u0179\u017a\5f\64\2\u017a\u017b\5(\25\2"+
		"\u017b\u017c\7\22\2\2\u017c\u017d\5R*\2\u017dE\3\2\2\2\u017e\u017f\7&"+
		"\2\2\u017f\u0180\5R*\2\u0180\u0181\7\'\2\2\u0181G\3\2\2\2\u0182\u0187"+
		"\5R*\2\u0183\u0184\7-\2\2\u0184\u0186\5R*\2\u0185\u0183\3\2\2\2\u0186"+
		"\u0189\3\2\2\2\u0187\u0185\3\2\2\2\u0187\u0188\3\2\2\2\u0188I\3\2\2\2"+
		"\u0189\u0187\3\2\2\2\u018a\u018f\5L\'\2\u018b\u018c\7-\2\2\u018c\u018e"+
		"\5L\'\2\u018d\u018b\3\2\2\2\u018e\u0191\3\2\2\2\u018f\u018d\3\2\2\2\u018f"+
		"\u0190\3\2\2\2\u0190K\3\2\2\2\u0191\u018f\3\2\2\2\u0192\u0193\7\\\2\2"+
		"\u0193\u0194\7\65\2\2\u0194\u0195\5R*\2\u0195M\3\2\2\2\u0196\u0197\7\\"+
		"\2\2\u0197\u0199\7&\2\2\u0198\u019a\5J&\2\u0199\u0198\3\2\2\2\u0199\u019a"+
		"\3\2\2\2\u019a\u019b\3\2\2\2\u019b\u019c\7\'\2\2\u019cO\3\2\2\2\u019d"+
		"\u01a0\7&\2\2\u019e\u01a1\7?\2\2\u019f\u01a1\5H%\2\u01a0\u019e\3\2\2\2"+
		"\u01a0\u019f\3\2\2\2\u01a1\u01a2\3\2\2\2\u01a2\u01a3\7\'\2\2\u01a3Q\3"+
		"\2\2\2\u01a4\u01a5\b*\1\2\u01a5\u01af\5\\/\2\u01a6\u01a7\7\25\2\2\u01a7"+
		"\u01af\5^\60\2\u01a8\u01af\5N(\2\u01a9\u01aa\t\4\2\2\u01aa\u01af\5R*\r"+
		"\u01ab\u01ac\7\62\2\2\u01ac\u01af\5R*\f\u01ad\u01af\5T+\2\u01ae\u01a4"+
		"\3\2\2\2\u01ae\u01a6\3\2\2\2\u01ae\u01a8\3\2\2\2\u01ae\u01a9\3\2\2\2\u01ae"+
		"\u01ab\3\2\2\2\u01ae\u01ad\3\2\2\2\u01af\u01d7\3\2\2\2\u01b0\u01b1\f\13"+
		"\2\2\u01b1\u01b2\t\5\2\2\u01b2\u01d6\5R*\f\u01b3\u01b4\f\n\2\2\u01b4\u01b5"+
		"\7T\2\2\u01b5\u01d6\5R*\13\u01b6\u01b7\f\t\2\2\u01b7\u01b8\t\4\2\2\u01b8"+
		"\u01d6\5R*\n\u01b9\u01ba\f\b\2\2\u01ba\u01bb\t\6\2\2\u01bb\u01d6\5R*\t"+
		"\u01bc\u01bd\f\7\2\2\u01bd\u01be\t\7\2\2\u01be\u01d6\5R*\b\u01bf\u01c0"+
		"\f\6\2\2\u01c0\u01c1\7:\2\2\u01c1\u01d6\5R*\7\u01c2\u01c3\f\5\2\2\u01c3"+
		"\u01c4\7;\2\2\u01c4\u01d6\5R*\6\u01c5\u01c6\f\4\2\2\u01c6\u01c7\7V\2\2"+
		"\u01c7\u01d6\5R*\4\u01c8\u01c9\f\22\2\2\u01c9\u01cc\7.\2\2\u01ca\u01cd"+
		"\7\\\2\2\u01cb\u01cd\5N(\2\u01cc\u01ca\3\2\2\2\u01cc\u01cb\3\2\2\2\u01cd"+
		"\u01d6\3\2\2\2\u01ce\u01cf\f\21\2\2\u01cf\u01d0\7*\2\2\u01d0\u01d1\5R"+
		"*\2\u01d1\u01d2\7+\2\2\u01d2\u01d6\3\2\2\2\u01d3\u01d4\f\17\2\2\u01d4"+
		"\u01d6\5P)\2\u01d5\u01b0\3\2\2\2\u01d5\u01b3\3\2\2\2\u01d5\u01b6\3\2\2"+
		"\2\u01d5\u01b9\3\2\2\2\u01d5\u01bc\3\2\2\2\u01d5\u01bf\3\2\2\2\u01d5\u01c2"+
		"\3\2\2\2\u01d5\u01c5\3\2\2\2\u01d5\u01c8\3\2\2\2\u01d5\u01ce\3\2\2\2\u01d5"+
		"\u01d3\3\2\2\2\u01d6\u01d9\3\2\2\2\u01d7\u01d5\3\2\2\2\u01d7\u01d8\3\2"+
		"\2\2\u01d8S\3\2\2\2\u01d9\u01d7\3\2\2\2\u01da\u01db\5V,\2\u01db\u01dc"+
		"\7Q\2\2\u01dc\u01dd\5<\37\2\u01ddU\3\2\2\2\u01de\u01e7\7&\2\2\u01df\u01e4"+
		"\5\64\33\2\u01e0\u01e1\7-\2\2\u01e1\u01e3\5\64\33\2\u01e2\u01e0\3\2\2"+
		"\2\u01e3\u01e6\3\2\2\2\u01e4\u01e2\3\2\2\2\u01e4\u01e5\3\2\2\2\u01e5\u01e8"+
		"\3\2\2\2\u01e6\u01e4\3\2\2\2\u01e7\u01df\3\2\2\2\u01e7\u01e8\3\2\2\2\u01e8"+
		"\u01e9\3\2\2\2\u01e9\u01ea\7\'\2\2\u01eaW\3\2\2\2\u01eb\u01ec\7\61\2\2"+
		"\u01ec\u01ed\5Z.\2\u01ed\u01ee\7Q\2\2\u01ee\u01ef\5 \21\2\u01ef\u01f0"+
		"\7\60\2\2\u01f0Y\3\2\2\2\u01f1\u01f2\7&\2\2\u01f2\u0200\7\'\2\2\u01f3"+
		"\u0200\5f\64\2\u01f4\u01f5\7&\2\2\u01f5\u01fa\5f\64\2\u01f6\u01f7\7-\2"+
		"\2\u01f7\u01f9\5f\64\2\u01f8\u01f6\3\2\2\2\u01f9\u01fc\3\2\2\2\u01fa\u01f8"+
		"\3\2\2\2\u01fa\u01fb\3\2\2\2\u01fb\u01fd\3\2\2\2\u01fc\u01fa\3\2\2\2\u01fd"+
		"\u01fe\7\'\2\2\u01fe\u0200\3\2\2\2\u01ff\u01f1\3\2\2\2\u01ff\u01f3\3\2"+
		"\2\2\u01ff\u01f4\3\2\2\2\u0200[\3\2\2\2\u0201\u0202\7&\2\2\u0202\u0203"+
		"\5R*\2\u0203\u0204\7\'\2\2\u0204\u020c\3\2\2\2\u0205\u020c\7\33\2\2\u0206"+
		"\u0207\7\32\2\2\u0207\u020c\5j\66\2\u0208\u020c\5:\36\2\u0209\u020c\7"+
		"\\\2\2\u020a\u020c\5h\65\2\u020b\u0201\3\2\2\2\u020b\u0205\3\2\2\2\u020b"+
		"\u0206\3\2\2\2\u020b\u0208\3\2\2\2\u020b\u0209\3\2\2\2\u020b\u020a\3\2"+
		"\2\2\u020c]\3\2\2\2\u020d\u0210\5`\61\2\u020e\u0211\5b\62\2\u020f\u0211"+
		"\5d\63\2\u0210\u020e\3\2\2\2\u0210\u020f\3\2\2\2\u0211_\3\2\2\2\u0212"+
		"\u0215\7\\\2\2\u0213\u0215\5h\65\2\u0214\u0212\3\2\2\2\u0214\u0213\3\2"+
		"\2\2\u0215a\3\2\2\2\u0216\u021c\7*\2\2\u0217\u0218\7+\2\2\u0218\u021d"+
		"\5,\27\2\u0219\u021a\5R*\2\u021a\u021b\7+\2\2\u021b\u021d\3\2\2\2\u021c"+
		"\u0217\3\2\2\2\u021c\u0219\3\2\2\2\u021dc\3\2\2\2\u021e\u021f\5l\67\2"+
		"\u021fe\3\2\2\2\u0220\u0223\5.\30\2\u0221\u0223\5h\65\2\u0222\u0220\3"+
		"\2\2\2\u0222\u0221\3\2\2\2\u0223\u0226\3\2\2\2\u0224\u0225\7*\2\2\u0225"+
		"\u0227\7+\2\2\u0226\u0224\3\2\2\2\u0226\u0227\3\2\2\2\u0227\u022a\3\2"+
		"\2\2\u0228\u022a\5X-\2\u0229\u0222\3\2\2\2\u0229\u0228\3\2\2\2\u022ag"+
		"\3\2\2\2\u022b\u022c\t\b\2\2\u022ci\3\2\2\2\u022d\u0234\5l\67\2\u022e"+
		"\u022f\7.\2\2\u022f\u0231\7\\\2\2\u0230\u0232\5l\67\2\u0231\u0230\3\2"+
		"\2\2\u0231\u0232\3\2\2\2\u0232\u0234\3\2\2\2\u0233\u022d\3\2\2\2\u0233"+
		"\u022e\3\2\2\2\u0234k\3\2\2\2\u0235\u0237\7&\2\2\u0236\u0238\5J&\2\u0237"+
		"\u0236\3\2\2\2\u0237\u0238\3\2\2\2\u0238\u0239\3\2\2\2\u0239\u023a\7\'"+
		"\2\2\u023am\3\2\2\2<osvz\u0085\u0089\u0093\u0096\u0099\u00a2\u00a7\u00ad"+
		"\u00b4\u00bd\u00c1\u00c6\u00cf\u00d6\u00da\u00e7\u00ee\u00f3\u00f7\u00ff"+
		"\u0108\u010f\u0114\u011b\u0120\u0123\u0137\u0144\u0147\u0159\u0171\u0177"+
		"\u0187\u018f\u0199\u01a0\u01ae\u01cc\u01d5\u01d7\u01e4\u01e7\u01fa\u01ff"+
		"\u020b\u0210\u0214\u021c\u0222\u0226\u0229\u0231\u0233\u0237";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}
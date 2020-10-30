using System;
using System.IO;
using System.Runtime.Serialization;
using mini_java_compiler.Parse.lalr;
using mini_java_compiler.Utilidades;
using mini_java_compiler.Parse;
using System.Windows.Forms;

namespace mini_java_compiler
{

    [Serializable()]
    public class SymbolException : System.Exception
    {
        public SymbolException(string message) : base(message)
        {
        }

        public SymbolException(string message,
            Exception inner) : base(message, inner)
        {
        }

        protected SymbolException(SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

    }

    [Serializable()]
    public class RuleException : System.Exception
    {

        public RuleException(string message) : base(message)
        {
        }

        public RuleException(string message,
                             Exception inner) : base(message, inner)
        {
        }

        protected RuleException(SerializationInfo info,
                                StreamingContext context) : base(info, context)
        {
        }

    }

    enum SymbolConstants : int
    {
        SYMBOL_EOF                                  =  0, // (EOF)
        SYMBOL_ERROR                                =  1, // (Error)
        SYMBOL_COMMENT                              =  2, // Comment
        SYMBOL_NEWLINE                              =  3, // NewLine
        SYMBOL_WHITESPACE                           =  4, // Whitespace
        SYMBOL_TIMESDIV                             =  5, // '*/'
        SYMBOL_DIVTIMES                             =  6, // '/*'
        SYMBOL_DIVDIV                               =  7, // '//'
        SYMBOL_MINUS                                =  8, // '-'
        SYMBOL_EXCLAM                               =  9, // '!'
        SYMBOL_EXCLAMEQ                             = 10, // '!='
        SYMBOL_PERCENT                              = 11, // '%'
        SYMBOL_LPAREN                               = 12, // '('
        SYMBOL_RPAREN                               = 13, // ')'
        SYMBOL_COMMA                                = 14, // ','
        SYMBOL_DOT                                  = 15, // '.'
        SYMBOL_DIV                                  = 16, // '/'
        SYMBOL_SEMI                                 = 17, // ';'
        SYMBOL_LBRACKETRBRACKET                     = 18, // '[]'
        SYMBOL_LBRACE                               = 19, // '{'
        SYMBOL_PIPEPIPE                             = 20, // '||'
        SYMBOL_RBRACE                               = 21, // '}'
        SYMBOL_PLUS                                 = 22, // '+'
        SYMBOL_EQ                                   = 23, // '='
        SYMBOL_GT                                   = 24, // '>'
        SYMBOL_GTEQ                                 = 25, // '>='
        SYMBOL_BOOLEAN                              = 26, // boolean
        SYMBOL_BOOLEANLITERAL                       = 27, // BooleanLiteral
        SYMBOL_BREAK                                = 28, // break
        SYMBOL_CLASS                                = 29, // class
        SYMBOL_DOUBLE                               = 30, // double
        SYMBOL_ELSE                                 = 31, // else
        SYMBOL_EXTENDS                              = 32, // extends
        SYMBOL_FLOATINGPOINTLITERAL                 = 33, // FloatingPointLiteral
        SYMBOL_FLOATINGPOINTLITERALEXPONENT         = 34, // FloatingPointLiteralExponent
        SYMBOL_FOR                                  = 35, // for
        SYMBOL_HEXESCAPECHARLITERAL                 = 36, // HexEscapeCharLiteral
        SYMBOL_HEXINTEGERLITERAL                    = 37, // HexIntegerLiteral
        SYMBOL_IDENTIFIER                           = 38, // Identifier
        SYMBOL_IF                                   = 39, // if
        SYMBOL_IMPLEMENTS                           = 40, // implements
        SYMBOL_INDIRECTCHARLITERAL                  = 41, // IndirectCharLiteral
        SYMBOL_INT                                  = 42, // int
        SYMBOL_INTERFACE                            = 43, // interface
        SYMBOL_NEW                                  = 44, // New
        SYMBOL_NULLLITERAL                          = 45, // NullLiteral
        SYMBOL_OCTALESCAPECHARLITERAL               = 46, // OctalEscapeCharLiteral
        SYMBOL_OCTALINTEGERLITERAL                  = 47, // OctalIntegerLiteral
        SYMBOL_RETURN                               = 48, // return
        SYMBOL_STANDARDESCAPECHARLITERAL            = 49, // StandardEscapeCharLiteral
        SYMBOL_STARTWITHNOZERODECIMALINTEGERLITERAL = 50, // StartWithNoZeroDecimalIntegerLiteral
        SYMBOL_STARTWITHZERODECIMALINTEGERLITERAL   = 51, // StartWithZeroDecimalIntegerLiteral
        SYMBOL_STATIC                               = 52, // static
        SYMBOL_STRING                               = 53, // string
        SYMBOL_STRINGLITERAL                        = 54, // StringLiteral
        SYMBOL_SYSTEMDOTOUTDOTPRINTLN               = 55, // 'System.out.println'
        SYMBOL_THIS                                 = 56, // this
        SYMBOL_VOID                                 = 57, // void
        SYMBOL_WHILE                                = 58, // while
        SYMBOL_BREAKSTMT                            = 59, // <BreakStmt>
        SYMBOL_CHARACTERLITERAL                     = 60, // <CharacterLiteral>
        SYMBOL_CLASSDECL                            = 61, // <ClassDecl>
        SYMBOL_CONSTANT                             = 62, // <Constant>
        SYMBOL_CONSTDECL                            = 63, // <ConstDecl>
        SYMBOL_CONSTTYPE                            = 64, // <ConstType>
        SYMBOL_DECIMALINTEGERLITERAL                = 65, // <DecimalIntegerLiteral>
        SYMBOL_DECL                                 = 66, // <Decl>
        SYMBOL_DECLPRIMA                            = 67, // <DeclPrima>
        SYMBOL_ELSESTMT                             = 68, // <ElseStmt>
        SYMBOL_EXPR                                 = 69, // <Expr>
        SYMBOL_EXPRPRIMA                            = 70, // <ExprPrima>
        SYMBOL_EXPRSTMT                             = 71, // <ExprStmt>
        SYMBOL_EXTENDS2                             = 72, // <Extends>
        SYMBOL_FIELD                                = 73, // <Field>
        SYMBOL_FLOATPOINTLITERAL                    = 74, // <FloatPointLiteral>
        SYMBOL_FORMALS                              = 75, // <Formals>
        SYMBOL_FORSTMT                              = 76, // <ForStmt>
        SYMBOL_FUNCTIONDECL                         = 77, // <FunctionDecl>
        SYMBOL_IFSTMT                               = 78, // <ifStmt>
        SYMBOL_IMPLEMENTS2                          = 79, // <Implements>
        SYMBOL_IMPLEMENTSPRIMA                      = 80, // <ImplementsPrima>
        SYMBOL_INTEGERLITERAL                       = 81, // <IntegerLiteral>
        SYMBOL_INTEGRALTYPE                         = 82, // <IntegralType>
        SYMBOL_INTERFACEDECL                        = 83, // <InterfaceDecl>
        SYMBOL_LITERAL                              = 84, // <Literal>
        SYMBOL_LVALUE                               = 85, // <LValue>
        SYMBOL_NUMERICTYPE                          = 86, // <NumericType>
        SYMBOL_PRINTSTMT                            = 87, // <PrintStmt>
        SYMBOL_PROGRAM                              = 88, // <Program>
        SYMBOL_PROTOTYPE                            = 89, // <Prototype>
        SYMBOL_RETURNSTMT                           = 90, // <ReturnStmt>
        SYMBOL_STMT                                 = 91, // <Stmt>
        SYMBOL_STMTBLOCK                            = 92, // <StmtBlock>
        SYMBOL_TYPE                                 = 93, // <Type>
        SYMBOL_VARIABLE                             = 94, // <Variable>
        SYMBOL_VARIABLEDECL                         = 95, // <VariableDecl>
        SYMBOL_WHILESTMT                            = 96  // <WhileStmt>
    };

    enum RuleConstants : int
    {
        RULE_CHARACTERLITERAL_INDIRECTCHARLITERAL                       =  0, // <CharacterLiteral> ::= IndirectCharLiteral
        RULE_CHARACTERLITERAL_STANDARDESCAPECHARLITERAL                 =  1, // <CharacterLiteral> ::= StandardEscapeCharLiteral
        RULE_CHARACTERLITERAL_OCTALESCAPECHARLITERAL                    =  2, // <CharacterLiteral> ::= OctalEscapeCharLiteral
        RULE_CHARACTERLITERAL_HEXESCAPECHARLITERAL                      =  3, // <CharacterLiteral> ::= HexEscapeCharLiteral
        RULE_DECIMALINTEGERLITERAL_STARTWITHZERODECIMALINTEGERLITERAL   =  4, // <DecimalIntegerLiteral> ::= StartWithZeroDecimalIntegerLiteral
        RULE_DECIMALINTEGERLITERAL_STARTWITHNOZERODECIMALINTEGERLITERAL =  5, // <DecimalIntegerLiteral> ::= StartWithNoZeroDecimalIntegerLiteral
        RULE_FLOATPOINTLITERAL_FLOATINGPOINTLITERAL                     =  6, // <FloatPointLiteral> ::= FloatingPointLiteral
        RULE_FLOATPOINTLITERAL_FLOATINGPOINTLITERALEXPONENT             =  7, // <FloatPointLiteral> ::= FloatingPointLiteralExponent
        RULE_INTEGERLITERAL                                             =  8, // <IntegerLiteral> ::= <DecimalIntegerLiteral>
        RULE_INTEGERLITERAL_HEXINTEGERLITERAL                           =  9, // <IntegerLiteral> ::= HexIntegerLiteral
        RULE_INTEGERLITERAL_OCTALINTEGERLITERAL                         = 10, // <IntegerLiteral> ::= OctalIntegerLiteral
        RULE_LITERAL                                                    = 11, // <Literal> ::= <IntegerLiteral>
        RULE_LITERAL2                                                   = 12, // <Literal> ::= <FloatPointLiteral>
        RULE_LITERAL_BOOLEANLITERAL                                     = 13, // <Literal> ::= BooleanLiteral
        RULE_LITERAL3                                                   = 14, // <Literal> ::= <CharacterLiteral>
        RULE_LITERAL_STRINGLITERAL                                      = 15, // <Literal> ::= StringLiteral
        RULE_LITERAL_NULLLITERAL                                        = 16, // <Literal> ::= NullLiteral
        RULE_NUMERICTYPE                                                = 17, // <NumericType> ::= <IntegralType>
        RULE_PROGRAM                                                    = 18, // <Program> ::= <Decl> <DeclPrima>
        RULE_DECLPRIMA                                                  = 19, // <DeclPrima> ::= <Decl>
        RULE_DECL                                                       = 20, // <Decl> ::= <VariableDecl>
        RULE_DECL2                                                      = 21, // <Decl> ::= <FunctionDecl>
        RULE_DECL3                                                      = 22, // <Decl> ::= <ConstDecl>
        RULE_DECL4                                                      = 23, // <Decl> ::= <ClassDecl>
        RULE_DECL5                                                      = 24, // <Decl> ::= <InterfaceDecl>
        RULE_VARIABLEDECL_SEMI                                          = 25, // <VariableDecl> ::= <Variable> ';' <VariableDecl>
        RULE_VARIABLE_IDENTIFIER                                        = 26, // <Variable> ::= <Type> Identifier
        RULE_CONSTDECL_STATIC_IDENTIFIER_SEMI                           = 27, // <ConstDecl> ::= static <IntegralType> Identifier ';' <ConstDecl>
        RULE_INTEGRALTYPE_INT                                           = 28, // <IntegralType> ::= int
        RULE_CONSTTYPE_DOUBLE                                           = 29, // <ConstType> ::= double
        RULE_CONSTTYPE_BOOLEAN                                          = 30, // <ConstType> ::= boolean
        RULE_CONSTTYPE_STRING                                           = 31, // <ConstType> ::= string
        RULE_TYPE_INT                                                   = 32, // <Type> ::= int
        RULE_TYPE_DOUBLE                                                = 33, // <Type> ::= double
        RULE_TYPE_BOOLEAN                                               = 34, // <Type> ::= boolean
        RULE_TYPE_STRING                                                = 35, // <Type> ::= string
        RULE_TYPE_IDENTIFIER                                            = 36, // <Type> ::= Identifier
        RULE_TYPE_LBRACKETRBRACKET                                      = 37, // <Type> ::= <Type> '[]'
        RULE_FUNCTIONDECL_IDENTIFIER_LPAREN_RPAREN                      = 38, // <FunctionDecl> ::= <Type> Identifier '(' <Formals> ')' <StmtBlock>
        RULE_FUNCTIONDECL_VOID_IDENTIFIER_LPAREN_RPAREN                 = 39, // <FunctionDecl> ::= void Identifier '(' <Formals> ')' <StmtBlock>
        RULE_FORMALS_COMMA                                              = 40, // <Formals> ::= <Variable> ',' <Formals>
        RULE_FORMALS                                                    = 41, // <Formals> ::= <Variable>
        RULE_CLASSDECL_CLASS_IDENTIFIER_LBRACE_RBRACE                   = 42, // <ClassDecl> ::= class Identifier <Extends> <Implements> '{' <Field> '}'
        RULE_EXTENDS_EXTENDS_IDENTIFIER                                 = 43, // <Extends> ::= extends Identifier
        RULE_EXTENDS                                                    = 44, // <Extends> ::= 
        RULE_IMPLEMENTS_IMPLEMENTS_IDENTIFIER_COMMA                     = 45, // <Implements> ::= implements Identifier ',' <ImplementsPrima>
        RULE_IMPLEMENTSPRIMA                                            = 46, // <ImplementsPrima> ::= <Implements>
        RULE_FIELD                                                      = 47, // <Field> ::= <VariableDecl> <Field>
        RULE_FIELD2                                                     = 48, // <Field> ::= <FunctionDecl> <Field>
        RULE_FIELD3                                                     = 49, // <Field> ::= <ClassDecl> <Field>
        RULE_INTERFACEDECL_INTERFACE_IDENTIFIER_LBRACE_RBRACE           = 50, // <InterfaceDecl> ::= interface Identifier '{' <Prototype> '}'
        RULE_PROTOTYPE_IDENTIFIER_LPAREN_RPAREN_SEMI                    = 51, // <Prototype> ::= <Type> Identifier '(' <Formals> ')' ';' <Prototype>
        RULE_PROTOTYPE_VOID_IDENTIFIER_LPAREN_RPAREN_SEMI               = 52, // <Prototype> ::= void Identifier '(' <Formals> ')' ';' <Prototype>
        RULE_STMTBLOCK_LBRACE_RBRACE                                    = 53, // <StmtBlock> ::= '{' <VariableDecl> <ConstDecl> <Stmt> '}'
        RULE_STMT_SEMI                                                  = 54, // <Stmt> ::= <ExprStmt> ';' <Stmt>
        RULE_EXPRSTMT                                                   = 55, // <ExprStmt> ::= <Expr>
        RULE_STMT                                                       = 56, // <Stmt> ::= <ifStmt> <Stmt>
        RULE_STMT2                                                      = 57, // <Stmt> ::= <WhileStmt> <Stmt>
        RULE_STMT3                                                      = 58, // <Stmt> ::= <ForStmt> <Stmt>
        RULE_STMT4                                                      = 59, // <Stmt> ::= <BreakStmt> <Stmt>
        RULE_STMT5                                                      = 60, // <Stmt> ::= <ReturnStmt> <Stmt>
        RULE_STMT6                                                      = 61, // <Stmt> ::= <PrintStmt> <Stmt>
        RULE_STMT7                                                      = 62, // <Stmt> ::= <StmtBlock> <Stmt>
        RULE_IFSTMT_IF_LPAREN_RPAREN                                    = 63, // <ifStmt> ::= if '(' <Expr> ')' <Stmt> <ElseStmt>
        RULE_ELSESTMT_ELSE                                              = 64, // <ElseStmt> ::= else <Stmt>
        RULE_ELSESTMT                                                   = 65, // <ElseStmt> ::= 
        RULE_WHILESTMT_WHILE_LPAREN_RPAREN                              = 66, // <WhileStmt> ::= while '(' <Expr> ')' <Stmt>
        RULE_FORSTMT_FOR_LPAREN_SEMI_SEMI_RPAREN                        = 67, // <ForStmt> ::= for '(' <Expr> ';' <Expr> ';' <Expr> ')' <Stmt>
        RULE_RETURNSTMT_RETURN_SEMI                                     = 68, // <ReturnStmt> ::= return <Expr> ';'
        RULE_BREAKSTMT_BREAK_SEMI                                       = 69, // <BreakStmt> ::= break ';'
        RULE_PRINTSTMT_SYSTEMDOTOUTDOTPRINTLN_LPAREN_COMMA_RPAREN_SEMI  = 70, // <PrintStmt> ::= 'System.out.println' '(' <Expr> <ExprPrima> ',' ')' ';'
        RULE_EXPRPRIMA                                                  = 71, // <ExprPrima> ::= <Expr>
        RULE_EXPR_EQ                                                    = 72, // <Expr> ::= <LValue> '=' <Expr>
        RULE_EXPR                                                       = 73, // <Expr> ::= <Constant>
        RULE_EXPR2                                                      = 74, // <Expr> ::= <LValue>
        RULE_EXPR_THIS                                                  = 75, // <Expr> ::= this
        RULE_EXPR_LPAREN_RPAREN                                         = 76, // <Expr> ::= '(' <Expr> ')'
        RULE_EXPR_PLUS                                                  = 77, // <Expr> ::= <Expr> '+' <Expr>
        RULE_EXPR_DIV                                                   = 78, // <Expr> ::= <Expr> '/' <Expr>
        RULE_EXPR_PERCENT                                               = 79, // <Expr> ::= <Expr> '%' <Expr>
        RULE_EXPR_MINUS                                                 = 80, // <Expr> ::= '-' <Expr>
        RULE_EXPR_GT                                                    = 81, // <Expr> ::= <Expr> '>' <Expr>
        RULE_EXPR_GTEQ                                                  = 82, // <Expr> ::= <Expr> '>=' <Expr>
        RULE_EXPR_EXCLAMEQ                                              = 83, // <Expr> ::= <Expr> '!=' <Expr>
        RULE_EXPR_PIPEPIPE                                              = 84, // <Expr> ::= <Expr> '||' <Expr>
        RULE_EXPR_EXCLAM                                                = 85, // <Expr> ::= '!' <Expr>
        RULE_EXPR_NEW_LPAREN_IDENTIFIER_RPAREN                          = 86, // <Expr> ::= New '(' Identifier ')'
        RULE_LVALUE_IDENTIFIER                                          = 87, // <LValue> ::= Identifier
        RULE_LVALUE_DOT_IDENTIFIER                                      = 88, // <LValue> ::= <Expr> '.' Identifier
        RULE_CONSTANT                                                   = 89, // <Constant> ::= <IntegerLiteral>
        RULE_CONSTANT2                                                  = 90, // <Constant> ::= <FloatPointLiteral>
        RULE_CONSTANT_BOOLEANLITERAL                                    = 91, // <Constant> ::= BooleanLiteral
        RULE_CONSTANT_STRINGLITERAL                                     = 92, // <Constant> ::= StringLiteral
        RULE_CONSTANT_NULLLITERAL                                       = 93  // <Constant> ::= NullLiteral
    };

    public class AnalizadorSintactico
    {
        private LR1Parser parser;

        public AnalizadorSintactico(string filename)
        {
            FileStream stream = new FileStream(filename,
                                               FileMode.Open, 
                                               FileAccess.Read, 
                                               FileShare.Read);
            Init(stream);
            stream.Close();
        }

        public AnalizadorSintactico(string baseName, string resourceName)
        {
            byte[] buffer = ResourceUtil.GetByteArrayResource(
                System.Reflection.Assembly.GetExecutingAssembly(),
                baseName,
                resourceName);
            MemoryStream stream = new MemoryStream(buffer);
            Init(stream);
            stream.Close();
        }

        public AnalizadorSintactico(Stream stream)
        {
            Init(stream);
        }

        private void Init(Stream stream)
        {
            CGTReader reader = new CGTReader(stream);
            parser = reader.CreateNewParser();
            parser.TrimReductions = false;
            parser.StoreTokens = LR1Parser.StoreTokensMode.NoUserObject;

            parser.OnTokenError += new LR1Parser.TokenErrorHandler(TokenErrorEvent);
            parser.OnParseError += new LR1Parser.ParseErrorHandler(ParseErrorEvent);
        }

        public void Parse(string source)
        {
            NonterminalToken token = parser.Parse(source);
            if (token != null)
            {
                Object obj = CreateObject(token);
                //todo: Use your object any way you like
                MENSAJE = MessageBox.Show("Se analizo bien correctamente");
            }
        }

        private Object CreateObject(Token token)
        {
            if (token is TerminalToken)
                return CreateObjectFromTerminal((TerminalToken)token);
            else
                return CreateObjectFromNonterminal((NonterminalToken)token);
        }

        private Object CreateObjectFromTerminal(TerminalToken token)
        {
            switch (token.Symbol.Id)
            {
                case (int)SymbolConstants.SYMBOL_EOF :
                //(EOF)
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ERROR :
                //(Error)
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_COMMENT :
                //Comment
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_NEWLINE :
                //NewLine
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_WHITESPACE :
                //Whitespace
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_TIMESDIV :
                //'*/'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DIVTIMES :
                //'/*'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DIVDIV :
                //'//'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_MINUS :
                //'-'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EXCLAM :
                //'!'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EXCLAMEQ :
                //'!='
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PERCENT :
                //'%'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LPAREN :
                //'('
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_RPAREN :
                //')'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_COMMA :
                //','
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DOT :
                //'.'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DIV :
                //'/'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_SEMI :
                //';'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LBRACKETRBRACKET :
                //'[]'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LBRACE :
                //'{'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PIPEPIPE :
                //'||'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_RBRACE :
                //'}'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PLUS :
                //'+'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EQ :
                //'='
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_GT :
                //'>'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_GTEQ :
                //'>='
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_BOOLEAN :
                //boolean
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_BOOLEANLITERAL :
                //BooleanLiteral
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_BREAK :
                //break
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_CLASS :
                //class
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DOUBLE :
                //double
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ELSE :
                //else
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EXTENDS :
                //extends
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FLOATINGPOINTLITERAL :
                //FloatingPointLiteral
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FLOATINGPOINTLITERALEXPONENT :
                //FloatingPointLiteralExponent
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FOR :
                //for
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_HEXESCAPECHARLITERAL :
                //HexEscapeCharLiteral
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_HEXINTEGERLITERAL :
                //HexIntegerLiteral
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_IDENTIFIER :
                //Identifier
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_IF :
                //if
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_IMPLEMENTS :
                //implements
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_INDIRECTCHARLITERAL :
                //IndirectCharLiteral
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_INT :
                //int
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_INTERFACE :
                //interface
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_NEW :
                //New
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_NULLLITERAL :
                //NullLiteral
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_OCTALESCAPECHARLITERAL :
                //OctalEscapeCharLiteral
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_OCTALINTEGERLITERAL :
                //OctalIntegerLiteral
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_RETURN :
                //return
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_STANDARDESCAPECHARLITERAL :
                //StandardEscapeCharLiteral
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_STARTWITHNOZERODECIMALINTEGERLITERAL :
                //StartWithNoZeroDecimalIntegerLiteral
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_STARTWITHZERODECIMALINTEGERLITERAL :
                //StartWithZeroDecimalIntegerLiteral
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_STATIC :
                //static
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_STRING :
                //string
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_STRINGLITERAL :
                //StringLiteral
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_SYSTEMDOTOUTDOTPRINTLN :
                //'System.out.println'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_THIS :
                //this
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_VOID :
                //void
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_WHILE :
                //while
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_BREAKSTMT :
                //<BreakStmt>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_CHARACTERLITERAL :
                //<CharacterLiteral>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_CLASSDECL :
                //<ClassDecl>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_CONSTANT :
                //<Constant>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_CONSTDECL :
                //<ConstDecl>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_CONSTTYPE :
                //<ConstType>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DECIMALINTEGERLITERAL :
                //<DecimalIntegerLiteral>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DECL :
                //<Decl>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DECLPRIMA :
                //<DeclPrima>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ELSESTMT :
                //<ElseStmt>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EXPR :
                //<Expr>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EXPRPRIMA :
                //<ExprPrima>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EXPRSTMT :
                //<ExprStmt>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EXTENDS2 :
                //<Extends>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FIELD :
                //<Field>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FLOATPOINTLITERAL :
                //<FloatPointLiteral>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FORMALS :
                //<Formals>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FORSTMT :
                //<ForStmt>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FUNCTIONDECL :
                //<FunctionDecl>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_IFSTMT :
                //<ifStmt>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_IMPLEMENTS2 :
                //<Implements>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_IMPLEMENTSPRIMA :
                //<ImplementsPrima>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_INTEGERLITERAL :
                //<IntegerLiteral>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_INTEGRALTYPE :
                //<IntegralType>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_INTERFACEDECL :
                //<InterfaceDecl>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LITERAL :
                //<Literal>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LVALUE :
                //<LValue>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_NUMERICTYPE :
                //<NumericType>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PRINTSTMT :
                //<PrintStmt>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PROGRAM :
                //<Program>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PROTOTYPE :
                //<Prototype>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_RETURNSTMT :
                //<ReturnStmt>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_STMT :
                //<Stmt>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_STMTBLOCK :
                //<StmtBlock>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_TYPE :
                //<Type>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_VARIABLE :
                //<Variable>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_VARIABLEDECL :
                //<VariableDecl>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_WHILESTMT :
                //<WhileStmt>
                //todo: Create a new object that corresponds to the symbol
                return null;

            }
            throw new SymbolException("Unknown symbol");
        }

        public Object CreateObjectFromNonterminal(NonterminalToken token)
        {
            switch (token.Rule.Id)
            {
                case (int)RuleConstants.RULE_CHARACTERLITERAL_INDIRECTCHARLITERAL :
                //<CharacterLiteral> ::= IndirectCharLiteral
                
                return null;

                case (int)RuleConstants.RULE_CHARACTERLITERAL_STANDARDESCAPECHARLITERAL :
                //<CharacterLiteral> ::= StandardEscapeCharLiteral
                
                return null;

                case (int)RuleConstants.RULE_CHARACTERLITERAL_OCTALESCAPECHARLITERAL :
                //<CharacterLiteral> ::= OctalEscapeCharLiteral
                
                return null;

                case (int)RuleConstants.RULE_CHARACTERLITERAL_HEXESCAPECHARLITERAL :
                //<CharacterLiteral> ::= HexEscapeCharLiteral
                
                return null;

                case (int)RuleConstants.RULE_DECIMALINTEGERLITERAL_STARTWITHZERODECIMALINTEGERLITERAL :
                //<DecimalIntegerLiteral> ::= StartWithZeroDecimalIntegerLiteral
                
                return null;

                case (int)RuleConstants.RULE_DECIMALINTEGERLITERAL_STARTWITHNOZERODECIMALINTEGERLITERAL :
                //<DecimalIntegerLiteral> ::= StartWithNoZeroDecimalIntegerLiteral
                
                return null;

                case (int)RuleConstants.RULE_FLOATPOINTLITERAL_FLOATINGPOINTLITERAL :
                //<FloatPointLiteral> ::= FloatingPointLiteral
                
                return null;

                case (int)RuleConstants.RULE_FLOATPOINTLITERAL_FLOATINGPOINTLITERALEXPONENT :
                //<FloatPointLiteral> ::= FloatingPointLiteralExponent
                
                return null;

                case (int)RuleConstants.RULE_INTEGERLITERAL :
                //<IntegerLiteral> ::= <DecimalIntegerLiteral>
                
                return null;

                case (int)RuleConstants.RULE_INTEGERLITERAL_HEXINTEGERLITERAL :
                //<IntegerLiteral> ::= HexIntegerLiteral
                
                return null;

                case (int)RuleConstants.RULE_INTEGERLITERAL_OCTALINTEGERLITERAL :
                //<IntegerLiteral> ::= OctalIntegerLiteral
                
                return null;

                case (int)RuleConstants.RULE_LITERAL :
                //<Literal> ::= <IntegerLiteral>
                
                return null;

                case (int)RuleConstants.RULE_LITERAL2 :
                //<Literal> ::= <FloatPointLiteral>
                
                return null;

                case (int)RuleConstants.RULE_LITERAL_BOOLEANLITERAL :
                //<Literal> ::= BooleanLiteral
                
                return null;

                case (int)RuleConstants.RULE_LITERAL3 :
                //<Literal> ::= <CharacterLiteral>
                
                return null;

                case (int)RuleConstants.RULE_LITERAL_STRINGLITERAL :
                //<Literal> ::= StringLiteral
                
                return null;

                case (int)RuleConstants.RULE_LITERAL_NULLLITERAL :
                //<Literal> ::= NullLiteral
                
                return null;

                case (int)RuleConstants.RULE_NUMERICTYPE :
                //<NumericType> ::= <IntegralType>
                
                return null;

                case (int)RuleConstants.RULE_PROGRAM :
                //<Program> ::= <Decl> <DeclPrima>
                
                return null;

                case (int)RuleConstants.RULE_DECLPRIMA :
                //<DeclPrima> ::= <Decl>
                
                return null;

                case (int)RuleConstants.RULE_DECL :
                //<Decl> ::= <VariableDecl>
                
                return null;

                case (int)RuleConstants.RULE_DECL2 :
                //<Decl> ::= <FunctionDecl>
                
                return null;

                case (int)RuleConstants.RULE_DECL3 :
                //<Decl> ::= <ConstDecl>
                
                return null;

                case (int)RuleConstants.RULE_DECL4 :
                //<Decl> ::= <ClassDecl>
                
                return null;

                case (int)RuleConstants.RULE_DECL5 :
                //<Decl> ::= <InterfaceDecl>
                
                return null;

                case (int)RuleConstants.RULE_VARIABLEDECL_SEMI :
                //<VariableDecl> ::= <Variable> ';' <VariableDecl>
                
                return null;

                case (int)RuleConstants.RULE_VARIABLE_IDENTIFIER :
                //<Variable> ::= <Type> Identifier
                
                return null;

                case (int)RuleConstants.RULE_CONSTDECL_STATIC_IDENTIFIER_SEMI :
                //<ConstDecl> ::= static <IntegralType> Identifier ';' <ConstDecl>
                
                return null;

                case (int)RuleConstants.RULE_INTEGRALTYPE_INT :
                //<IntegralType> ::= int
                
                return null;

                case (int)RuleConstants.RULE_CONSTTYPE_DOUBLE :
                //<ConstType> ::= double
                
                return null;

                case (int)RuleConstants.RULE_CONSTTYPE_BOOLEAN :
                //<ConstType> ::= boolean
                
                return null;

                case (int)RuleConstants.RULE_CONSTTYPE_STRING :
                //<ConstType> ::= string
                
                return null;

                case (int)RuleConstants.RULE_TYPE_INT :
                //<Type> ::= int
                
                return null;

                case (int)RuleConstants.RULE_TYPE_DOUBLE :
                //<Type> ::= double
                
                return null;

                case (int)RuleConstants.RULE_TYPE_BOOLEAN :
                //<Type> ::= boolean
                
                return null;

                case (int)RuleConstants.RULE_TYPE_STRING :
                //<Type> ::= string
                
                return null;

                case (int)RuleConstants.RULE_TYPE_IDENTIFIER :
                //<Type> ::= Identifier
                
                return null;

                case (int)RuleConstants.RULE_TYPE_LBRACKETRBRACKET :
                //<Type> ::= <Type> '[]'
                
                return null;

                case (int)RuleConstants.RULE_FUNCTIONDECL_IDENTIFIER_LPAREN_RPAREN :
                //<FunctionDecl> ::= <Type> Identifier '(' <Formals> ')' <StmtBlock>
                
                return null;

                case (int)RuleConstants.RULE_FUNCTIONDECL_VOID_IDENTIFIER_LPAREN_RPAREN :
                //<FunctionDecl> ::= void Identifier '(' <Formals> ')' <StmtBlock>
                
                return null;

                case (int)RuleConstants.RULE_FORMALS_COMMA :
                //<Formals> ::= <Variable> ',' <Formals>
                
                return null;

                case (int)RuleConstants.RULE_FORMALS :
                //<Formals> ::= <Variable>
                
                return null;

                case (int)RuleConstants.RULE_CLASSDECL_CLASS_IDENTIFIER_LBRACE_RBRACE :
                //<ClassDecl> ::= class Identifier <Extends> <Implements> '{' <Field> '}'
                
                return null;

                case (int)RuleConstants.RULE_EXTENDS_EXTENDS_IDENTIFIER :
                //<Extends> ::= extends Identifier
                
                return null;

                case (int)RuleConstants.RULE_EXTENDS :
                //<Extends> ::= 
                
                return null;

                case (int)RuleConstants.RULE_IMPLEMENTS_IMPLEMENTS_IDENTIFIER_COMMA :
                //<Implements> ::= implements Identifier ',' <ImplementsPrima>
                
                return null;

                case (int)RuleConstants.RULE_IMPLEMENTSPRIMA :
                //<ImplementsPrima> ::= <Implements>
                
                return null;

                case (int)RuleConstants.RULE_FIELD :
                //<Field> ::= <VariableDecl> <Field>
                
                return null;

                case (int)RuleConstants.RULE_FIELD2 :
                //<Field> ::= <FunctionDecl> <Field>
                
                return null;

                case (int)RuleConstants.RULE_FIELD3 :
                //<Field> ::= <ClassDecl> <Field>
                
                return null;

                case (int)RuleConstants.RULE_INTERFACEDECL_INTERFACE_IDENTIFIER_LBRACE_RBRACE :
                //<InterfaceDecl> ::= interface Identifier '{' <Prototype> '}'
                
                return null;

                case (int)RuleConstants.RULE_PROTOTYPE_IDENTIFIER_LPAREN_RPAREN_SEMI :
                //<Prototype> ::= <Type> Identifier '(' <Formals> ')' ';' <Prototype>
                
                return null;

                case (int)RuleConstants.RULE_PROTOTYPE_VOID_IDENTIFIER_LPAREN_RPAREN_SEMI :
                //<Prototype> ::= void Identifier '(' <Formals> ')' ';' <Prototype>
                
                return null;

                case (int)RuleConstants.RULE_STMTBLOCK_LBRACE_RBRACE :
                //<StmtBlock> ::= '{' <VariableDecl> <ConstDecl> <Stmt> '}'
                
                return null;

                case (int)RuleConstants.RULE_STMT_SEMI :
                //<Stmt> ::= <ExprStmt> ';' <Stmt>
                
                return null;

                case (int)RuleConstants.RULE_EXPRSTMT :
                //<ExprStmt> ::= <Expr>
                
                return null;

                case (int)RuleConstants.RULE_STMT :
                //<Stmt> ::= <ifStmt> <Stmt>
                
                return null;

                case (int)RuleConstants.RULE_STMT2 :
                //<Stmt> ::= <WhileStmt> <Stmt>
                
                return null;

                case (int)RuleConstants.RULE_STMT3 :
                //<Stmt> ::= <ForStmt> <Stmt>
                
                return null;

                case (int)RuleConstants.RULE_STMT4 :
                //<Stmt> ::= <BreakStmt> <Stmt>
                
                return null;

                case (int)RuleConstants.RULE_STMT5 :
                //<Stmt> ::= <ReturnStmt> <Stmt>
                
                return null;

                case (int)RuleConstants.RULE_STMT6 :
                //<Stmt> ::= <PrintStmt> <Stmt>
                
                return null;

                case (int)RuleConstants.RULE_STMT7 :
                //<Stmt> ::= <StmtBlock> <Stmt>
                
                return null;

                case (int)RuleConstants.RULE_IFSTMT_IF_LPAREN_RPAREN :
                //<ifStmt> ::= if '(' <Expr> ')' <Stmt> <ElseStmt>
                
                return null;

                case (int)RuleConstants.RULE_ELSESTMT_ELSE :
                //<ElseStmt> ::= else <Stmt>
                
                return null;

                case (int)RuleConstants.RULE_ELSESTMT :
                //<ElseStmt> ::= 
                
                return null;

                case (int)RuleConstants.RULE_WHILESTMT_WHILE_LPAREN_RPAREN :
                //<WhileStmt> ::= while '(' <Expr> ')' <Stmt>
                
                return null;

                case (int)RuleConstants.RULE_FORSTMT_FOR_LPAREN_SEMI_SEMI_RPAREN :
                //<ForStmt> ::= for '(' <Expr> ';' <Expr> ';' <Expr> ')' <Stmt>
                
                return null;

                case (int)RuleConstants.RULE_RETURNSTMT_RETURN_SEMI :
                //<ReturnStmt> ::= return <Expr> ';'
                
                return null;

                case (int)RuleConstants.RULE_BREAKSTMT_BREAK_SEMI :
                //<BreakStmt> ::= break ';'
                
                return null;

                case (int)RuleConstants.RULE_PRINTSTMT_SYSTEMDOTOUTDOTPRINTLN_LPAREN_COMMA_RPAREN_SEMI :
                //<PrintStmt> ::= 'System.out.println' '(' <Expr> <ExprPrima> ',' ')' ';'
                
                return null;

                case (int)RuleConstants.RULE_EXPRPRIMA :
                //<ExprPrima> ::= <Expr>
                
                return null;

                case (int)RuleConstants.RULE_EXPR_EQ :
                //<Expr> ::= <LValue> '=' <Expr>
                
                return null;

                case (int)RuleConstants.RULE_EXPR :
                //<Expr> ::= <Constant>
                
                return null;

                case (int)RuleConstants.RULE_EXPR2 :
                //<Expr> ::= <LValue>
                
                return null;

                case (int)RuleConstants.RULE_EXPR_THIS :
                //<Expr> ::= this
                
                return null;

                case (int)RuleConstants.RULE_EXPR_LPAREN_RPAREN :
                //<Expr> ::= '(' <Expr> ')'
                
                return null;

                case (int)RuleConstants.RULE_EXPR_PLUS :
                //<Expr> ::= <Expr> '+' <Expr>
                
                return null;

                case (int)RuleConstants.RULE_EXPR_DIV :
                //<Expr> ::= <Expr> '/' <Expr>
                
                return null;

                case (int)RuleConstants.RULE_EXPR_PERCENT :
                //<Expr> ::= <Expr> '%' <Expr>
                
                return null;

                case (int)RuleConstants.RULE_EXPR_MINUS :
                //<Expr> ::= '-' <Expr>
                
                return null;

                case (int)RuleConstants.RULE_EXPR_GT :
                //<Expr> ::= <Expr> '>' <Expr>
                
                return null;

                case (int)RuleConstants.RULE_EXPR_GTEQ :
                //<Expr> ::= <Expr> '>=' <Expr>
                
                return null;

                case (int)RuleConstants.RULE_EXPR_EXCLAMEQ :
                //<Expr> ::= <Expr> '!=' <Expr>
                
                return null;

                case (int)RuleConstants.RULE_EXPR_PIPEPIPE :
                //<Expr> ::= <Expr> '||' <Expr>
                
                return null;

                case (int)RuleConstants.RULE_EXPR_EXCLAM :
                //<Expr> ::= '!' <Expr>
                
                return null;

                case (int)RuleConstants.RULE_EXPR_NEW_LPAREN_IDENTIFIER_RPAREN :
                //<Expr> ::= New '(' Identifier ')'
                
                return null;

                case (int)RuleConstants.RULE_LVALUE_IDENTIFIER :
                //<LValue> ::= Identifier
                
                return null;

                case (int)RuleConstants.RULE_LVALUE_DOT_IDENTIFIER :
                //<LValue> ::= <Expr> '.' Identifier
               
                return null;

                case (int)RuleConstants.RULE_CONSTANT :
                //<Constant> ::= <IntegerLiteral>
                
                return null;

                case (int)RuleConstants.RULE_CONSTANT2 :
                //<Constant> ::= <FloatPointLiteral>
                
                return null;

                case (int)RuleConstants.RULE_CONSTANT_BOOLEANLITERAL :
                //<Constant> ::= BooleanLiteral
               
                return null;

                case (int)RuleConstants.RULE_CONSTANT_STRINGLITERAL :
                //<Constant> ::= StringLiteral
               
                return null;

                case (int)RuleConstants.RULE_CONSTANT_NULLLITERAL :
                //<Constant> ::= NullLiteral
                
                return null;

            }
            throw new RuleException("Regla desconocida");
        }

        private void TokenErrorEvent(LR1Parser parser, TokenErrorEventArgs args)
        { 
            string message = "Error de token no reconocido: '"+args.Token.ToString()+"'";
        }

        private void ParseErrorEvent(LR1Parser parser, ParseErrorEventArgs args)
        {
            string message = "No se pudo reconocer el token: '" + args.UnexpectedToken.ToString() + "'" + "en : "  +args.UnexpectedToken.Location ; 
            MessageBox.Show(message);
        }

    }
}

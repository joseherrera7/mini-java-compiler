using System;
using System.IO;
using System.Runtime.Serialization;
using com.calitha.commons;
using com.calitha.goldparser;

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
        SYMBOL_EOF                    =  0, // (EOF)
        SYMBOL_ERROR                  =  1, // (Error)
        SYMBOL_WHITESPACE             =  2, // Whitespace
        SYMBOL_MINUS                  =  3, // '-'
        SYMBOL_EXCLAM                 =  4, // '!'
        SYMBOL_EXCLAMEQ               =  5, // '!='
        SYMBOL_PERCENT                =  6, // '%'
        SYMBOL_LPAREN                 =  7, // '('
        SYMBOL_RPAREN                 =  8, // ')'
        SYMBOL_COMMA                  =  9, // ','
        SYMBOL_DOT                    = 10, // '.'
        SYMBOL_DIV                    = 11, // '/'
        SYMBOL_SEMI                   = 12, // ';'
        SYMBOL_LBRACKETRBRACKET       = 13, // '[]'
        SYMBOL_LBRACE                 = 14, // '{'
        SYMBOL_PIPEPIPE               = 15, // '||'
        SYMBOL_RBRACE                 = 16, // '}'
        SYMBOL_PLUS                   = 17, // '+'
        SYMBOL_EQ                     = 18, // '='
        SYMBOL_GT                     = 19, // '>'
        SYMBOL_GTEQ                   = 20, // '>='
        SYMBOL_BOOLCONSTANT           = 21, // boolConstant
        SYMBOL_BOOLEAN                = 22, // boolean
        SYMBOL_BREAK                  = 23, // break
        SYMBOL_CLASS                  = 24, // class
        SYMBOL_DOUBLE                 = 25, // double
        SYMBOL_DOUBLECONSTANT         = 26, // doubleConstant
        SYMBOL_ELSE                   = 27, // else
        SYMBOL_EXTENDS                = 28, // extends
        SYMBOL_FOR                    = 29, // for
        SYMBOL_IDENT                  = 30, // ident
        SYMBOL_IF                     = 31, // if
        SYMBOL_IMPLEMENTS             = 32, // implements
        SYMBOL_INT                    = 33, // int
        SYMBOL_INTCONSTANT            = 34, // intConstant
        SYMBOL_INTERFACE              = 35, // interface
        SYMBOL_NEW                    = 36, // New
        SYMBOL_NULL                   = 37, // null
        SYMBOL_RETURN                 = 38, // return
        SYMBOL_STATIC                 = 39, // static
        SYMBOL_STRING                 = 40, // string
        SYMBOL_STRINGCONSTANT         = 41, // stringConstant
        SYMBOL_SYSTEMDOTOUTDOTPRINTLN = 42, // 'System.out.println'
        SYMBOL_THIS                   = 43, // this
        SYMBOL_VOID                   = 44, // void
        SYMBOL_WHILE                  = 45, // while
        SYMBOL_BREAKSTMT              = 46, // <BreakStmt>
        SYMBOL_CLASSDECL              = 47, // <ClassDecl>
        SYMBOL_CONSTANT               = 48, // <Constant>
        SYMBOL_CONSTDECL              = 49, // <ConstDecl>
        SYMBOL_CONSTTYPE              = 50, // <ConstType>
        SYMBOL_DECL                   = 51, // <Decl>
        SYMBOL_DECLPRIMA              = 52, // <DeclPrima>
        SYMBOL_ELSESTMT               = 53, // <ElseStmt>
        SYMBOL_EXPR                   = 54, // <Expr>
        SYMBOL_EXPRPRIMA              = 55, // <ExprPrima>
        SYMBOL_EXPRSTMT               = 56, // <ExprStmt>
        SYMBOL_EXTENDS2               = 57, // <Extends>
        SYMBOL_FIELD                  = 58, // <Field>
        SYMBOL_FORMALS                = 59, // <Formals>
        SYMBOL_FORSTMT                = 60, // <ForStmt>
        SYMBOL_FUNCTIONDECL           = 61, // <FunctionDecl>
        SYMBOL_IFSTMT                 = 62, // <ifStmt>
        SYMBOL_IMPLEMENTS2            = 63, // <Implements>
        SYMBOL_IMPLEMENTSPRIMA        = 64, // <ImplementsPrima>
        SYMBOL_INTERFACEDECL          = 65, // <InterfaceDecl>
        SYMBOL_LVALUE                 = 66, // <LValue>
        SYMBOL_PRINTSTMT              = 67, // <PrintStmt>
        SYMBOL_PROGRAM                = 68, // <Program>
        SYMBOL_PROTOTYPE              = 69, // <Prototype>
        SYMBOL_RETURNSTMT             = 70, // <ReturnStmt>
        SYMBOL_STMT                   = 71, // <Stmt>
        SYMBOL_STMTBLOCK              = 72, // <StmtBlock>
        SYMBOL_TYPE                   = 73, // <Type>
        SYMBOL_VARIABLE               = 74, // <Variable>
        SYMBOL_VARIABLEDECL           = 75, // <VariableDecl>
        SYMBOL_WHILESTMT              = 76  // <WhileStmt>
    };

    enum RuleConstants : int
    {
        RULE_PROGRAM                                                   =  0, // <Program> ::= <Decl> <DeclPrima>
        RULE_DECLPRIMA                                                 =  1, // <DeclPrima> ::= <Decl>
        RULE_DECLPRIMA2                                                =  2, // <DeclPrima> ::= 
        RULE_DECL                                                      =  3, // <Decl> ::= <VariableDecl>
        RULE_DECL2                                                     =  4, // <Decl> ::= <FunctionDecl>
        RULE_DECL3                                                     =  5, // <Decl> ::= <ConstDecl>
        RULE_DECL4                                                     =  6, // <Decl> ::= <ClassDecl>
        RULE_DECL5                                                     =  7, // <Decl> ::= <InterfaceDecl>
        RULE_VARIABLEDECL_SEMI                                         =  8, // <VariableDecl> ::= <Variable> ';' <VariableDecl>
        RULE_VARIABLE_IDENT                                            =  9, // <Variable> ::= <Type> ident
        RULE_CONSTDECL_STATIC_IDENT_SEMI                               = 10, // <ConstDecl> ::= static <ConstType> ident ';' <ConstDecl>
        RULE_CONSTTYPE_INT                                             = 11, // <ConstType> ::= int
        RULE_CONSTTYPE_DOUBLE                                          = 12, // <ConstType> ::= double
        RULE_CONSTTYPE_BOOLEAN                                         = 13, // <ConstType> ::= boolean
        RULE_CONSTTYPE_STRING                                          = 14, // <ConstType> ::= string
        RULE_TYPE_INT                                                  = 15, // <Type> ::= int
        RULE_TYPE_DOUBLE                                               = 16, // <Type> ::= double
        RULE_TYPE_BOOLEAN                                              = 17, // <Type> ::= boolean
        RULE_TYPE_STRING                                               = 18, // <Type> ::= string
        RULE_TYPE_IDENT                                                = 19, // <Type> ::= ident
        RULE_TYPE_LBRACKETRBRACKET                                     = 20, // <Type> ::= <Type> '[]'
        RULE_FUNCTIONDECL_IDENT_LPAREN_RPAREN                          = 21, // <FunctionDecl> ::= <Type> ident '(' <Formals> ')' <StmtBlock>
        RULE_FUNCTIONDECL_VOID_IDENT_LPAREN_RPAREN                     = 22, // <FunctionDecl> ::= void ident '(' <Formals> ')' <StmtBlock>
        RULE_FORMALS_COMMA                                             = 23, // <Formals> ::= <Variable> ',' <Formals>
        RULE_FORMALS                                                   = 24, // <Formals> ::= <Variable>
        RULE_CLASSDECL_CLASS_IDENT_LBRACE_RBRACE                       = 25, // <ClassDecl> ::= class ident <Extends> <Implements> '{' <Field> '}'
        RULE_EXTENDS_EXTENDS_IDENT                                     = 26, // <Extends> ::= extends ident
        RULE_EXTENDS                                                   = 27, // <Extends> ::= 
        RULE_IMPLEMENTS_IMPLEMENTS_IDENT_COMMA                         = 28, // <Implements> ::= implements ident ',' <ImplementsPrima>
        RULE_IMPLEMENTSPRIMA                                           = 29, // <ImplementsPrima> ::= <Implements>
        RULE_FIELD                                                     = 30, // <Field> ::= <VariableDecl> <Field>
        RULE_FIELD2                                                    = 31, // <Field> ::= <FunctionDecl> <Field>
        RULE_FIELD3                                                    = 32, // <Field> ::= <ClassDecl> <Field>
        RULE_INTERFACEDECL_INTERFACE_IDENT_LBRACE_RBRACE               = 33, // <InterfaceDecl> ::= interface ident '{' <Prototype> '}'
        RULE_PROTOTYPE_IDENT_LPAREN_RPAREN_SEMI                        = 34, // <Prototype> ::= <Type> ident '(' <Formals> ')' ';' <Prototype>
        RULE_PROTOTYPE_VOID_IDENT_LPAREN_RPAREN_SEMI                   = 35, // <Prototype> ::= void ident '(' <Formals> ')' ';' <Prototype>
        RULE_STMTBLOCK_LBRACE_RBRACE                                   = 36, // <StmtBlock> ::= '{' <VariableDecl> <ConstDecl> <Stmt> '}'
        RULE_STMT_SEMI                                                 = 37, // <Stmt> ::= <ExprStmt> ';' <Stmt>
        RULE_EXPRSTMT                                                  = 38, // <ExprStmt> ::= <Expr>
        RULE_EXPRSTMT2                                                 = 39, // <ExprStmt> ::= 
        RULE_STMT                                                      = 40, // <Stmt> ::= <ifStmt> <Stmt>
        RULE_STMT2                                                     = 41, // <Stmt> ::= <WhileStmt> <Stmt>
        RULE_STMT3                                                     = 42, // <Stmt> ::= <ForStmt> <Stmt>
        RULE_STMT4                                                     = 43, // <Stmt> ::= <BreakStmt> <Stmt>
        RULE_STMT5                                                     = 44, // <Stmt> ::= <ReturnStmt> <Stmt>
        RULE_STMT6                                                     = 45, // <Stmt> ::= <PrintStmt> <Stmt>
        RULE_STMT7                                                     = 46, // <Stmt> ::= <StmtBlock> <Stmt>
        RULE_IFSTMT_IF_LPAREN_RPAREN                                   = 47, // <ifStmt> ::= if '(' <Expr> ')' <Stmt> <ElseStmt>
        RULE_ELSESTMT_ELSE                                             = 48, // <ElseStmt> ::= else <Stmt>
        RULE_ELSESTMT                                                  = 49, // <ElseStmt> ::= 
        RULE_WHILESTMT_WHILE_LPAREN_RPAREN                             = 50, // <WhileStmt> ::= while '(' <Expr> ')' <Stmt>
        RULE_FORSTMT_FOR_LPAREN_SEMI_SEMI_RPAREN                       = 51, // <ForStmt> ::= for '(' <Expr> ';' <Expr> ';' <Expr> ')' <Stmt>
        RULE_RETURNSTMT_RETURN_SEMI                                    = 52, // <ReturnStmt> ::= return <Expr> ';'
        RULE_BREAKSTMT_BREAK_SEMI                                      = 53, // <BreakStmt> ::= break ';'
        RULE_PRINTSTMT_SYSTEMDOTOUTDOTPRINTLN_LPAREN_COMMA_RPAREN_SEMI = 54, // <PrintStmt> ::= 'System.out.println' '(' <Expr> <ExprPrima> ',' ')' ';'
        RULE_EXPRPRIMA                                                 = 55, // <ExprPrima> ::= <Expr>
        RULE_EXPRPRIMA2                                                = 56, // <ExprPrima> ::= 
        RULE_EXPR_EQ                                                   = 57, // <Expr> ::= <LValue> '=' <Expr>
        RULE_EXPR                                                      = 58, // <Expr> ::= <Constant>
        RULE_EXPR2                                                     = 59, // <Expr> ::= <LValue>
        RULE_EXPR_THIS                                                 = 60, // <Expr> ::= this
        RULE_EXPR_LPAREN_RPAREN                                        = 61, // <Expr> ::= '(' <Expr> ')'
        RULE_EXPR_PLUS                                                 = 62, // <Expr> ::= <Expr> '+' <Expr>
        RULE_EXPR_DIV                                                  = 63, // <Expr> ::= <Expr> '/' <Expr>
        RULE_EXPR_PERCENT                                              = 64, // <Expr> ::= <Expr> '%' <Expr>
        RULE_EXPR_MINUS                                                = 65, // <Expr> ::= '-' <Expr>
        RULE_EXPR_GT                                                   = 66, // <Expr> ::= <Expr> '>' <Expr>
        RULE_EXPR_GTEQ                                                 = 67, // <Expr> ::= <Expr> '>=' <Expr>
        RULE_EXPR_EXCLAMEQ                                             = 68, // <Expr> ::= <Expr> '!=' <Expr>
        RULE_EXPR_PIPEPIPE                                             = 69, // <Expr> ::= <Expr> '||' <Expr>
        RULE_EXPR_EXCLAM                                               = 70, // <Expr> ::= '!' <Expr>
        RULE_EXPR_NEW_LPAREN_IDENT_RPAREN                              = 71, // <Expr> ::= New '(' ident ')'
        RULE_LVALUE_IDENT                                              = 72, // <LValue> ::= ident
        RULE_LVALUE_DOT_IDENT                                          = 73, // <LValue> ::= <Expr> '.' ident
        RULE_CONSTANT_INTCONSTANT                                      = 74, // <Constant> ::= intConstant
        RULE_CONSTANT_DOUBLECONSTANT                                   = 75, // <Constant> ::= doubleConstant
        RULE_CONSTANT_BOOLCONSTANT                                     = 76, // <Constant> ::= boolConstant
        RULE_CONSTANT_STRINGCONSTANT                                   = 77, // <Constant> ::= stringConstant
        RULE_CONSTANT_NULL                                             = 78  // <Constant> ::= null
    };

    public class MyParser
    {
        private LALRParser parser;

        public MyParser(string filename)
        {
            FileStream stream = new FileStream(filename,
                                               FileMode.Open, 
                                               FileAccess.Read, 
                                               FileShare.Read);
            Init(stream);
            stream.Close();
        }

        public MyParser(string baseName, string resourceName)
        {
            byte[] buffer = ResourceUtil.GetByteArrayResource(
                System.Reflection.Assembly.GetExecutingAssembly(),
                baseName,
                resourceName);
            MemoryStream stream = new MemoryStream(buffer);
            Init(stream);
            stream.Close();
        }

        public MyParser(Stream stream)
        {
            Init(stream);
        }

        private void Init(Stream stream)
        {
            CGTReader reader = new CGTReader(stream);
            parser = reader.CreateNewParser();
            parser.TrimReductions = false;
            parser.StoreTokens = LALRParser.StoreTokensMode.NoUserObject;

            parser.OnTokenError += new LALRParser.TokenErrorHandler(TokenErrorEvent);
            parser.OnParseError += new LALRParser.ParseErrorHandler(ParseErrorEvent);
        }

        public void Parse(string source)
        {
            NonterminalToken token = parser.Parse(source);
            if (token != null)
            {
                Object obj = CreateObject(token);
                //todo: Use your object any way you like
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

                case (int)SymbolConstants.SYMBOL_WHITESPACE :
                //Whitespace
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

                case (int)SymbolConstants.SYMBOL_BOOLCONSTANT :
                //boolConstant
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_BOOLEAN :
                //boolean
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

                case (int)SymbolConstants.SYMBOL_DOUBLECONSTANT :
                //doubleConstant
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

                case (int)SymbolConstants.SYMBOL_FOR :
                //for
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_IDENT :
                //ident
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

                case (int)SymbolConstants.SYMBOL_INT :
                //int
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_INTCONSTANT :
                //intConstant
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

                case (int)SymbolConstants.SYMBOL_NULL :
                //null
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_RETURN :
                //return
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

                case (int)SymbolConstants.SYMBOL_STRINGCONSTANT :
                //stringConstant
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

                case (int)SymbolConstants.SYMBOL_INTERFACEDECL :
                //<InterfaceDecl>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LVALUE :
                //<LValue>
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
                case (int)RuleConstants.RULE_PROGRAM :
                //<Program> ::= <Decl> <DeclPrima>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_DECLPRIMA :
                //<DeclPrima> ::= <Decl>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_DECLPRIMA2 :
                //<DeclPrima> ::= 
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_DECL :
                //<Decl> ::= <VariableDecl>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_DECL2 :
                //<Decl> ::= <FunctionDecl>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_DECL3 :
                //<Decl> ::= <ConstDecl>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_DECL4 :
                //<Decl> ::= <ClassDecl>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_DECL5 :
                //<Decl> ::= <InterfaceDecl>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_VARIABLEDECL_SEMI :
                //<VariableDecl> ::= <Variable> ';' <VariableDecl>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_VARIABLE_IDENT :
                //<Variable> ::= <Type> ident
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CONSTDECL_STATIC_IDENT_SEMI :
                //<ConstDecl> ::= static <ConstType> ident ';' <ConstDecl>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CONSTTYPE_INT :
                //<ConstType> ::= int
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CONSTTYPE_DOUBLE :
                //<ConstType> ::= double
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CONSTTYPE_BOOLEAN :
                //<ConstType> ::= boolean
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CONSTTYPE_STRING :
                //<ConstType> ::= string
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_TYPE_INT :
                //<Type> ::= int
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_TYPE_DOUBLE :
                //<Type> ::= double
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_TYPE_BOOLEAN :
                //<Type> ::= boolean
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_TYPE_STRING :
                //<Type> ::= string
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_TYPE_IDENT :
                //<Type> ::= ident
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_TYPE_LBRACKETRBRACKET :
                //<Type> ::= <Type> '[]'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FUNCTIONDECL_IDENT_LPAREN_RPAREN :
                //<FunctionDecl> ::= <Type> ident '(' <Formals> ')' <StmtBlock>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FUNCTIONDECL_VOID_IDENT_LPAREN_RPAREN :
                //<FunctionDecl> ::= void ident '(' <Formals> ')' <StmtBlock>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FORMALS_COMMA :
                //<Formals> ::= <Variable> ',' <Formals>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FORMALS :
                //<Formals> ::= <Variable>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CLASSDECL_CLASS_IDENT_LBRACE_RBRACE :
                //<ClassDecl> ::= class ident <Extends> <Implements> '{' <Field> '}'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXTENDS_EXTENDS_IDENT :
                //<Extends> ::= extends ident
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXTENDS :
                //<Extends> ::= 
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_IMPLEMENTS_IMPLEMENTS_IDENT_COMMA :
                //<Implements> ::= implements ident ',' <ImplementsPrima>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_IMPLEMENTSPRIMA :
                //<ImplementsPrima> ::= <Implements>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FIELD :
                //<Field> ::= <VariableDecl> <Field>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FIELD2 :
                //<Field> ::= <FunctionDecl> <Field>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FIELD3 :
                //<Field> ::= <ClassDecl> <Field>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_INTERFACEDECL_INTERFACE_IDENT_LBRACE_RBRACE :
                //<InterfaceDecl> ::= interface ident '{' <Prototype> '}'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_PROTOTYPE_IDENT_LPAREN_RPAREN_SEMI :
                //<Prototype> ::= <Type> ident '(' <Formals> ')' ';' <Prototype>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_PROTOTYPE_VOID_IDENT_LPAREN_RPAREN_SEMI :
                //<Prototype> ::= void ident '(' <Formals> ')' ';' <Prototype>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STMTBLOCK_LBRACE_RBRACE :
                //<StmtBlock> ::= '{' <VariableDecl> <ConstDecl> <Stmt> '}'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STMT_SEMI :
                //<Stmt> ::= <ExprStmt> ';' <Stmt>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPRSTMT :
                //<ExprStmt> ::= <Expr>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPRSTMT2 :
                //<ExprStmt> ::= 
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STMT :
                //<Stmt> ::= <ifStmt> <Stmt>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STMT2 :
                //<Stmt> ::= <WhileStmt> <Stmt>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STMT3 :
                //<Stmt> ::= <ForStmt> <Stmt>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STMT4 :
                //<Stmt> ::= <BreakStmt> <Stmt>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STMT5 :
                //<Stmt> ::= <ReturnStmt> <Stmt>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STMT6 :
                //<Stmt> ::= <PrintStmt> <Stmt>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STMT7 :
                //<Stmt> ::= <StmtBlock> <Stmt>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_IFSTMT_IF_LPAREN_RPAREN :
                //<ifStmt> ::= if '(' <Expr> ')' <Stmt> <ElseStmt>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ELSESTMT_ELSE :
                //<ElseStmt> ::= else <Stmt>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ELSESTMT :
                //<ElseStmt> ::= 
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_WHILESTMT_WHILE_LPAREN_RPAREN :
                //<WhileStmt> ::= while '(' <Expr> ')' <Stmt>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FORSTMT_FOR_LPAREN_SEMI_SEMI_RPAREN :
                //<ForStmt> ::= for '(' <Expr> ';' <Expr> ';' <Expr> ')' <Stmt>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_RETURNSTMT_RETURN_SEMI :
                //<ReturnStmt> ::= return <Expr> ';'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_BREAKSTMT_BREAK_SEMI :
                //<BreakStmt> ::= break ';'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_PRINTSTMT_SYSTEMDOTOUTDOTPRINTLN_LPAREN_COMMA_RPAREN_SEMI :
                //<PrintStmt> ::= 'System.out.println' '(' <Expr> <ExprPrima> ',' ')' ';'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPRPRIMA :
                //<ExprPrima> ::= <Expr>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPRPRIMA2 :
                //<ExprPrima> ::= 
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPR_EQ :
                //<Expr> ::= <LValue> '=' <Expr>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPR :
                //<Expr> ::= <Constant>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPR2 :
                //<Expr> ::= <LValue>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPR_THIS :
                //<Expr> ::= this
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPR_LPAREN_RPAREN :
                //<Expr> ::= '(' <Expr> ')'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPR_PLUS :
                //<Expr> ::= <Expr> '+' <Expr>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPR_DIV :
                //<Expr> ::= <Expr> '/' <Expr>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPR_PERCENT :
                //<Expr> ::= <Expr> '%' <Expr>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPR_MINUS :
                //<Expr> ::= '-' <Expr>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPR_GT :
                //<Expr> ::= <Expr> '>' <Expr>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPR_GTEQ :
                //<Expr> ::= <Expr> '>=' <Expr>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPR_EXCLAMEQ :
                //<Expr> ::= <Expr> '!=' <Expr>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPR_PIPEPIPE :
                //<Expr> ::= <Expr> '||' <Expr>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPR_EXCLAM :
                //<Expr> ::= '!' <Expr>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPR_NEW_LPAREN_IDENT_RPAREN :
                //<Expr> ::= New '(' ident ')'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_LVALUE_IDENT :
                //<LValue> ::= ident
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_LVALUE_DOT_IDENT :
                //<LValue> ::= <Expr> '.' ident
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CONSTANT_INTCONSTANT :
                //<Constant> ::= intConstant
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CONSTANT_DOUBLECONSTANT :
                //<Constant> ::= doubleConstant
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CONSTANT_BOOLCONSTANT :
                //<Constant> ::= boolConstant
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CONSTANT_STRINGCONSTANT :
                //<Constant> ::= stringConstant
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CONSTANT_NULL :
                //<Constant> ::= null
                //todo: Create a new object using the stored tokens.
                return null;

            }
            throw new RuleException("Unknown rule");
        }

        private void TokenErrorEvent(LALRParser parser, TokenErrorEventArgs args)
        {
            string message = "Token error with input: '"+args.Token.ToString()+"'";
            //todo: Report message to UI?
        }

        private void ParseErrorEvent(LALRParser parser, ParseErrorEventArgs args)
        {
            string message = "Parse error caused by token: '"+args.UnexpectedToken.ToString()+"'";
            //todo: Report message to UI?
        }

    }
}

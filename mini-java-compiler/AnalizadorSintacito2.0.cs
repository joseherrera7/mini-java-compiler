using System;
using System.IO;
using System.Runtime.Serialization;
using com.calitha.goldparser.lalr;
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
        SYMBOL_EOF                                  =   0, // (EOF)
        SYMBOL_ERROR                                =   1, // (Error)
        SYMBOL_WHITESPACE                           =   2, // Whitespace
        SYMBOL_COMMENTEND                           =   3, // 'Comment End'
        SYMBOL_COMMENTLINE                          =   4, // 'Comment Line'
        SYMBOL_COMMENTSTART                         =   5, // 'Comment Start'
        SYMBOL_MINUS                                =   6, // '-'
        SYMBOL_MINUSMINUS                           =   7, // '--'
        SYMBOL_EXCLAM                               =   8, // '!'
        SYMBOL_EXCLAMEQ                             =   9, // '!='
        SYMBOL_PERCENT                              =  10, // '%'
        SYMBOL_PERCENTEQ                            =  11, // '%='
        SYMBOL_AMP                                  =  12, // '&'
        SYMBOL_AMPAMP                               =  13, // '&&'
        SYMBOL_AMPEQ                                =  14, // '&='
        SYMBOL_LPAREN                               =  15, // '('
        SYMBOL_RPAREN                               =  16, // ')'
        SYMBOL_TIMES                                =  17, // '*'
        SYMBOL_TIMESEQ                              =  18, // '*='
        SYMBOL_COMMA                                =  19, // ','
        SYMBOL_DOT                                  =  20, // '.'
        SYMBOL_DIV                                  =  21, // '/'
        SYMBOL_DIVEQ                                =  22, // '/='
        SYMBOL_COLON                                =  23, // ':'
        SYMBOL_SEMI                                 =  24, // ';'
        SYMBOL_QUESTION                             =  25, // '?'
        SYMBOL_LBRACKET                             =  26, // '['
        SYMBOL_RBRACKET                             =  27, // ']'
        SYMBOL_CARET                                =  28, // '^'
        SYMBOL_CARETEQ                              =  29, // '^='
        SYMBOL_LBRACE                               =  30, // '{'
        SYMBOL_PIPE                                 =  31, // '|'
        SYMBOL_PIPEPIPE                             =  32, // '||'
        SYMBOL_PIPEEQ                               =  33, // '|='
        SYMBOL_RBRACE                               =  34, // '}'
        SYMBOL_TILDE                                =  35, // '~'
        SYMBOL_PLUS                                 =  36, // '+'
        SYMBOL_PLUSPLUS                             =  37, // '++'
        SYMBOL_PLUSEQ                               =  38, // '+='
        SYMBOL_LT                                   =  39, // '<'
        SYMBOL_LTLT                                 =  40, // '<<'
        SYMBOL_LTLTEQ                               =  41, // '<<='
        SYMBOL_LTEQ                                 =  42, // '<='
        SYMBOL_EQ                                   =  43, // '='
        SYMBOL_MINUSEQ                              =  44, // '-='
        SYMBOL_EQEQ                                 =  45, // '=='
        SYMBOL_GT                                   =  46, // '>'
        SYMBOL_GTEQ                                 =  47, // '>='
        SYMBOL_GTGT                                 =  48, // '>>'
        SYMBOL_GTGTEQ                               =  49, // '>>='
        SYMBOL_GTGTGT                               =  50, // '>>>'
        SYMBOL_GTGTGTEQ                             =  51, // '>>>='
        SYMBOL_ABSTRACT                             =  52, // abstract
        SYMBOL_BOOLEAN                              =  53, // boolean
        SYMBOL_BOOLEANLITERAL                       =  54, // BooleanLiteral
        SYMBOL_BREAK                                =  55, // break
        SYMBOL_BYTE                                 =  56, // byte
        SYMBOL_CASE                                 =  57, // case
        SYMBOL_CATCH                                =  58, // catch
        SYMBOL_CHAR                                 =  59, // char
        SYMBOL_CLASS                                =  60, // class
        SYMBOL_CONTINUE                             =  61, // continue
        SYMBOL_DEFAULT                              =  62, // default
        SYMBOL_DO                                   =  63, // do
        SYMBOL_DOUBLE                               =  64, // double
        SYMBOL_ELSE                                 =  65, // else
        SYMBOL_EXTENDS                              =  66, // extends
        SYMBOL_FINAL                                =  67, // final
        SYMBOL_FINALLY                              =  68, // finally
        SYMBOL_FLOAT                                =  69, // float
        SYMBOL_FLOATINGPOINTLITERAL                 =  70, // FloatingPointLiteral
        SYMBOL_FLOATINGPOINTLITERALEXPONENT         =  71, // FloatingPointLiteralExponent
        SYMBOL_FOR                                  =  72, // for
        SYMBOL_HEXESCAPECHARLITERAL                 =  73, // HexEscapeCharLiteral
        SYMBOL_HEXINTEGERLITERAL                    =  74, // HexIntegerLiteral
        SYMBOL_IDENTIFIER                           =  75, // Identifier
        SYMBOL_IF                                   =  76, // if
        SYMBOL_IMPLEMENTS                           =  77, // implements
        SYMBOL_IMPORT                               =  78, // import
        SYMBOL_INDIRECTCHARLITERAL                  =  79, // IndirectCharLiteral
        SYMBOL_INSTANCEOF                           =  80, // instanceof
        SYMBOL_INT                                  =  81, // int
        SYMBOL_INTERFACE                            =  82, // interface
        SYMBOL_LONG                                 =  83, // long
        SYMBOL_NATIVE                               =  84, // native
        SYMBOL_NEW                                  =  85, // new
        SYMBOL_NULLLITERAL                          =  86, // NullLiteral
        SYMBOL_OCTALESCAPECHARLITERAL               =  87, // OctalEscapeCharLiteral
        SYMBOL_OCTALINTEGERLITERAL                  =  88, // OctalIntegerLiteral
        SYMBOL_PACKAGE                              =  89, // package
        SYMBOL_PRIVATE                              =  90, // private
        SYMBOL_PROTECTED                            =  91, // protected
        SYMBOL_PUBLIC                               =  92, // public
        SYMBOL_RETURN                               =  93, // return
        SYMBOL_SHORT                                =  94, // short
        SYMBOL_STANDARDESCAPECHARLITERAL            =  95, // StandardEscapeCharLiteral
        SYMBOL_STARTWITHNOZERODECIMALINTEGERLITERAL =  96, // StartWithNoZeroDecimalIntegerLiteral
        SYMBOL_STARTWITHZERODECIMALINTEGERLITERAL   =  97, // StartWithZeroDecimalIntegerLiteral
        SYMBOL_STATIC                               =  98, // static
        SYMBOL_STRINGLITERAL                        =  99, // StringLiteral
        SYMBOL_SUPER                                = 100, // super
        SYMBOL_SWITCH                               = 101, // switch
        SYMBOL_SYNCHRONIZED                         = 102, // synchronized
        SYMBOL_THIS                                 = 103, // this
        SYMBOL_THROW                                = 104, // throw
        SYMBOL_THROWS                               = 105, // throws
        SYMBOL_TRANSIENT                            = 106, // transient
        SYMBOL_TRY                                  = 107, // try
        SYMBOL_VOID                                 = 108, // void
        SYMBOL_VOLATILE                             = 109, // volatile
        SYMBOL_WHILE                                = 110, // while
        SYMBOL_ABSTRACTMETHODDECLARATION            = 111, // <AbstractMethodDeclaration>
        SYMBOL_ADDITIVEEXPRESSION                   = 112, // <AdditiveExpression>
        SYMBOL_ANDEXPRESSION                        = 113, // <AndExpression>
        SYMBOL_ARGUMENTLIST                         = 114, // <ArgumentList>
        SYMBOL_ARRAYACCESS                          = 115, // <ArrayAccess>
        SYMBOL_ARRAYCREATIONEXPRESSION              = 116, // <ArrayCreationExpression>
        SYMBOL_ARRAYINITIALIZER                     = 117, // <ArrayInitializer>
        SYMBOL_ARRAYTYPE                            = 118, // <ArrayType>
        SYMBOL_ASSIGNMENT                           = 119, // <Assignment>
        SYMBOL_ASSIGNMENTEXPRESSION                 = 120, // <AssignmentExpression>
        SYMBOL_ASSIGNMENTOPERATOR                   = 121, // <AssignmentOperator>
        SYMBOL_BLOCK                                = 122, // <Block>
        SYMBOL_BLOCKSTATEMENT                       = 123, // <BlockStatement>
        SYMBOL_BLOCKSTATEMENTS                      = 124, // <BlockStatements>
        SYMBOL_BREAKSTATEMENT                       = 125, // <BreakStatement>
        SYMBOL_CASTEXPRESSION                       = 126, // <CastExpression>
        SYMBOL_CATCHCLAUSE                          = 127, // <CatchClause>
        SYMBOL_CATCHES                              = 128, // <Catches>
        SYMBOL_CHARACTERLITERAL                     = 129, // <CharacterLiteral>
        SYMBOL_CLASSBODY                            = 130, // <ClassBody>
        SYMBOL_CLASSBODYDECLARATION                 = 131, // <ClassBodyDeclaration>
        SYMBOL_CLASSBODYDECLARATIONS                = 132, // <ClassBodyDeclarations>
        SYMBOL_CLASSDECLARATION                     = 133, // <ClassDeclaration>
        SYMBOL_CLASSINSTANCECREATIONEXPRESSION      = 134, // <ClassInstanceCreationExpression>
        SYMBOL_CLASSMEMBERDECLARATION               = 135, // <ClassMemberDeclaration>
        SYMBOL_CLASSORINTERFACETYPE                 = 136, // <ClassOrInterfaceType>
        SYMBOL_CLASSTYPE                            = 137, // <ClassType>
        SYMBOL_CLASSTYPELIST                        = 138, // <ClassTypeList>
        SYMBOL_COMPILATIONUNIT                      = 139, // <CompilationUnit>
        SYMBOL_CONDITIONALANDEXPRESSION             = 140, // <ConditionalAndExpression>
        SYMBOL_CONDITIONALEXPRESSION                = 141, // <ConditionalExpression>
        SYMBOL_CONDITIONALOREXPRESSION              = 142, // <ConditionalOrExpression>
        SYMBOL_CONSTANTDECLARATION                  = 143, // <ConstantDeclaration>
        SYMBOL_CONSTANTEXPRESSION                   = 144, // <ConstantExpression>
        SYMBOL_CONSTRUCTORBODY                      = 145, // <ConstructorBody>
        SYMBOL_CONSTRUCTORDECLARATION               = 146, // <ConstructorDeclaration>
        SYMBOL_CONSTRUCTORDECLARATOR                = 147, // <ConstructorDeclarator>
        SYMBOL_CONTINUESTATEMENT                    = 148, // <ContinueStatement>
        SYMBOL_DECIMALINTEGERLITERAL                = 149, // <DecimalIntegerLiteral>
        SYMBOL_DIMEXPR                              = 150, // <DimExpr>
        SYMBOL_DIMEXPRS                             = 151, // <DimExprs>
        SYMBOL_DIMS                                 = 152, // <Dims>
        SYMBOL_DOSTATEMENT                          = 153, // <DoStatement>
        SYMBOL_EMPTYSTATEMENT                       = 154, // <EmptyStatement>
        SYMBOL_EQUALITYEXPRESSION                   = 155, // <EqualityExpression>
        SYMBOL_EXCLUSIVEOREXPRESSION                = 156, // <ExclusiveOrExpression>
        SYMBOL_EXPLICITCONSTRUCTORINVOCATION        = 157, // <ExplicitConstructorInvocation>
        SYMBOL_EXPRESSION                           = 158, // <Expression>
        SYMBOL_EXPRESSIONSTATEMENT                  = 159, // <ExpressionStatement>
        SYMBOL_EXTENDSINTERFACES                    = 160, // <ExtendsInterfaces>
        SYMBOL_FIELDACCESS                          = 161, // <FieldAccess>
        SYMBOL_FIELDDECLARATION                     = 162, // <FieldDeclaration>
        SYMBOL_FINALLY2                             = 163, // <Finally>
        SYMBOL_FLOATINGPOINTTYPE                    = 164, // <FloatingPointType>
        SYMBOL_FLOATPOINTLITERAL                    = 165, // <FloatPointLiteral>
        SYMBOL_FORINIT                              = 166, // <ForInit>
        SYMBOL_FORMALPARAMETER                      = 167, // <FormalParameter>
        SYMBOL_FORMALPARAMETERLIST                  = 168, // <FormalParameterList>
        SYMBOL_FORSTATEMENT                         = 169, // <ForStatement>
        SYMBOL_FORSTATEMENTNOSHORTIF                = 170, // <ForStatementNoShortIf>
        SYMBOL_FORUPDATE                            = 171, // <ForUpdate>
        SYMBOL_IFTHENELSESTATEMENT                  = 172, // <IfThenElseStatement>
        SYMBOL_IFTHENELSESTATEMENTNOSHORTIF         = 173, // <IfThenElseStatementNoShortIf>
        SYMBOL_IFTHENSTATEMENT                      = 174, // <IfThenStatement>
        SYMBOL_IMPORTDECLARATION                    = 175, // <ImportDeclaration>
        SYMBOL_IMPORTDECLARATIONS                   = 176, // <ImportDeclarations>
        SYMBOL_INCLUSIVEOREXPRESSION                = 177, // <InclusiveOrExpression>
        SYMBOL_INTEGERLITERAL                       = 178, // <IntegerLiteral>
        SYMBOL_INTEGRALTYPE                         = 179, // <IntegralType>
        SYMBOL_INTERFACEBODY                        = 180, // <InterfaceBody>
        SYMBOL_INTERFACEDECLARATION                 = 181, // <InterfaceDeclaration>
        SYMBOL_INTERFACEMEMBERDECLARATION           = 182, // <InterfaceMemberDeclaration>
        SYMBOL_INTERFACEMEMBERDECLARATIONS          = 183, // <InterfaceMemberDeclarations>
        SYMBOL_INTERFACES                           = 184, // <Interfaces>
        SYMBOL_INTERFACETYPE                        = 185, // <InterfaceType>
        SYMBOL_INTERFACETYPELIST                    = 186, // <InterfaceTypeList>
        SYMBOL_LABELEDSTATEMENT                     = 187, // <LabeledStatement>
        SYMBOL_LABELEDSTATEMENTNOSHORTIF            = 188, // <LabeledStatementNoShortIf>
        SYMBOL_LEFTHANDSIDE                         = 189, // <LeftHandSide>
        SYMBOL_LITERAL                              = 190, // <Literal>
        SYMBOL_LOCALVARIABLEDECLARATION             = 191, // <LocalVariableDeclaration>
        SYMBOL_LOCALVARIABLEDECLARATIONSTATEMENT    = 192, // <LocalVariableDeclarationStatement>
        SYMBOL_METHODBODY                           = 193, // <MethodBody>
        SYMBOL_METHODDECLARATION                    = 194, // <MethodDeclaration>
        SYMBOL_METHODDECLARATOR                     = 195, // <MethodDeclarator>
        SYMBOL_METHODHEADER                         = 196, // <MethodHeader>
        SYMBOL_METHODINVOCATION                     = 197, // <MethodInvocation>
        SYMBOL_MODIFIER                             = 198, // <Modifier>
        SYMBOL_MODIFIERS                            = 199, // <Modifiers>
        SYMBOL_MULTIPLICATIVEEXPRESSION             = 200, // <MultiplicativeExpression>
        SYMBOL_NAME                                 = 201, // <Name>
        SYMBOL_NUMERICTYPE                          = 202, // <NumericType>
        SYMBOL_PACKAGEDECLARATION                   = 203, // <PackageDeclaration>
        SYMBOL_POSTDECREMENTEXPRESSION              = 204, // <PostDecrementExpression>
        SYMBOL_POSTFIXEXPRESSION                    = 205, // <PostfixExpression>
        SYMBOL_POSTINCREMENTEXPRESSION              = 206, // <PostIncrementExpression>
        SYMBOL_PREDECREMENTEXPRESSION               = 207, // <PreDecrementExpression>
        SYMBOL_PREINCREMENTEXPRESSION               = 208, // <PreIncrementExpression>
        SYMBOL_PRIMARY                              = 209, // <Primary>
        SYMBOL_PRIMARYNONEWARRAY                    = 210, // <PrimaryNoNewArray>
        SYMBOL_PRIMITIVETYPE                        = 211, // <PrimitiveType>
        SYMBOL_QUALIFIEDNAME                        = 212, // <QualifiedName>
        SYMBOL_REFERENCETYPE                        = 213, // <ReferenceType>
        SYMBOL_RELATIONALEXPRESSION                 = 214, // <RelationalExpression>
        SYMBOL_RETURNSTATEMENT                      = 215, // <ReturnStatement>
        SYMBOL_SHIFTEXPRESSION                      = 216, // <ShiftExpression>
        SYMBOL_SIMPLENAME                           = 217, // <SimpleName>
        SYMBOL_SINGLETYPEIMPORTDECLARATION          = 218, // <SingleTypeImportDeclaration>
        SYMBOL_STATEMENT                            = 219, // <Statement>
        SYMBOL_STATEMENTEXPRESSION                  = 220, // <StatementExpression>
        SYMBOL_STATEMENTEXPRESSIONLIST              = 221, // <StatementExpressionList>
        SYMBOL_STATEMENTNOSHORTIF                   = 222, // <StatementNoShortIf>
        SYMBOL_STATEMENTWITHOUTTRAILINGSUBSTATEMENT = 223, // <StatementWithoutTrailingSubstatement>
        SYMBOL_STATICINITIALIZER                    = 224, // <StaticInitializer>
        SYMBOL_SUPER2                               = 225, // <Super>
        SYMBOL_SWITCHBLOCK                          = 226, // <SwitchBlock>
        SYMBOL_SWITCHBLOCKSTATEMENTGROUP            = 227, // <SwitchBlockStatementGroup>
        SYMBOL_SWITCHBLOCKSTATEMENTGROUPS           = 228, // <SwitchBlockStatementGroups>
        SYMBOL_SWITCHLABEL                          = 229, // <SwitchLabel>
        SYMBOL_SWITCHLABELS                         = 230, // <SwitchLabels>
        SYMBOL_SWITCHSTATEMENT                      = 231, // <SwitchStatement>
        SYMBOL_SYNCHRONIZEDSTATEMENT                = 232, // <SynchronizedStatement>
        SYMBOL_THROWS2                              = 233, // <Throws>
        SYMBOL_THROWSTATEMENT                       = 234, // <ThrowStatement>
        SYMBOL_TRYSTATEMENT                         = 235, // <TryStatement>
        SYMBOL_TYPE                                 = 236, // <Type>
        SYMBOL_TYPEDECLARATION                      = 237, // <TypeDeclaration>
        SYMBOL_TYPEDECLARATIONS                     = 238, // <TypeDeclarations>
        SYMBOL_TYPEIMPORTONDEMANDDECLARATION        = 239, // <TypeImportOnDemandDeclaration>
        SYMBOL_UNARYEXPRESSION                      = 240, // <UnaryExpression>
        SYMBOL_UNARYEXPRESSIONNOTPLUSMINUS          = 241, // <UnaryExpressionNotPlusMinus>
        SYMBOL_VARIABLEDECLARATOR                   = 242, // <VariableDeclarator>
        SYMBOL_VARIABLEDECLARATORID                 = 243, // <VariableDeclaratorId>
        SYMBOL_VARIABLEDECLARATORS                  = 244, // <VariableDeclarators>
        SYMBOL_VARIABLEINITIALIZER                  = 245, // <VariableInitializer>
        SYMBOL_VARIABLEINITIALIZERS                 = 246, // <VariableInitializers>
        SYMBOL_WHILESTATEMENT                       = 247, // <WhileStatement>
        SYMBOL_WHILESTATEMENTNOSHORTIF              = 248  // <WhileStatementNoShortIf>
    };

    enum RuleConstants : int
    {
        RULE_CHARACTERLITERAL_INDIRECTCHARLITERAL                       =   0, // <CharacterLiteral> ::= IndirectCharLiteral
        RULE_CHARACTERLITERAL_STANDARDESCAPECHARLITERAL                 =   1, // <CharacterLiteral> ::= StandardEscapeCharLiteral
        RULE_CHARACTERLITERAL_OCTALESCAPECHARLITERAL                    =   2, // <CharacterLiteral> ::= OctalEscapeCharLiteral
        RULE_CHARACTERLITERAL_HEXESCAPECHARLITERAL                      =   3, // <CharacterLiteral> ::= HexEscapeCharLiteral
        RULE_DECIMALINTEGERLITERAL_STARTWITHZERODECIMALINTEGERLITERAL   =   4, // <DecimalIntegerLiteral> ::= StartWithZeroDecimalIntegerLiteral
        RULE_DECIMALINTEGERLITERAL_STARTWITHNOZERODECIMALINTEGERLITERAL =   5, // <DecimalIntegerLiteral> ::= StartWithNoZeroDecimalIntegerLiteral
        RULE_FLOATPOINTLITERAL_FLOATINGPOINTLITERAL                     =   6, // <FloatPointLiteral> ::= FloatingPointLiteral
        RULE_FLOATPOINTLITERAL_FLOATINGPOINTLITERALEXPONENT             =   7, // <FloatPointLiteral> ::= FloatingPointLiteralExponent
        RULE_INTEGERLITERAL                                             =   8, // <IntegerLiteral> ::= <DecimalIntegerLiteral>
        RULE_INTEGERLITERAL_HEXINTEGERLITERAL                           =   9, // <IntegerLiteral> ::= HexIntegerLiteral
        RULE_INTEGERLITERAL_OCTALINTEGERLITERAL                         =  10, // <IntegerLiteral> ::= OctalIntegerLiteral
        RULE_LITERAL                                                    =  11, // <Literal> ::= <IntegerLiteral>
        RULE_LITERAL2                                                   =  12, // <Literal> ::= <FloatPointLiteral>
        RULE_LITERAL_BOOLEANLITERAL                                     =  13, // <Literal> ::= BooleanLiteral
        RULE_LITERAL3                                                   =  14, // <Literal> ::= <CharacterLiteral>
        RULE_LITERAL_STRINGLITERAL                                      =  15, // <Literal> ::= StringLiteral
        RULE_LITERAL_NULLLITERAL                                        =  16, // <Literal> ::= NullLiteral
        RULE_TYPE                                                       =  17, // <Type> ::= <PrimitiveType>
        RULE_TYPE2                                                      =  18, // <Type> ::= <ReferenceType>
        RULE_PRIMITIVETYPE                                              =  19, // <PrimitiveType> ::= <NumericType>
        RULE_PRIMITIVETYPE_BOOLEAN                                      =  20, // <PrimitiveType> ::= boolean
        RULE_NUMERICTYPE                                                =  21, // <NumericType> ::= <IntegralType>
        RULE_NUMERICTYPE2                                               =  22, // <NumericType> ::= <FloatingPointType>
        RULE_INTEGRALTYPE_BYTE                                          =  23, // <IntegralType> ::= byte
        RULE_INTEGRALTYPE_SHORT                                         =  24, // <IntegralType> ::= short
        RULE_INTEGRALTYPE_INT                                           =  25, // <IntegralType> ::= int
        RULE_INTEGRALTYPE_LONG                                          =  26, // <IntegralType> ::= long
        RULE_INTEGRALTYPE_CHAR                                          =  27, // <IntegralType> ::= char
        RULE_FLOATINGPOINTTYPE_FLOAT                                    =  28, // <FloatingPointType> ::= float
        RULE_FLOATINGPOINTTYPE_DOUBLE                                   =  29, // <FloatingPointType> ::= double
        RULE_REFERENCETYPE                                              =  30, // <ReferenceType> ::= <ClassOrInterfaceType>
        RULE_REFERENCETYPE2                                             =  31, // <ReferenceType> ::= <ArrayType>
        RULE_CLASSORINTERFACETYPE                                       =  32, // <ClassOrInterfaceType> ::= <Name>
        RULE_CLASSTYPE                                                  =  33, // <ClassType> ::= <ClassOrInterfaceType>
        RULE_INTERFACETYPE                                              =  34, // <InterfaceType> ::= <ClassOrInterfaceType>
        RULE_ARRAYTYPE_LBRACKET_RBRACKET                                =  35, // <ArrayType> ::= <PrimitiveType> '[' ']'
        RULE_ARRAYTYPE_LBRACKET_RBRACKET2                               =  36, // <ArrayType> ::= <Name> '[' ']'
        RULE_ARRAYTYPE_LBRACKET_RBRACKET3                               =  37, // <ArrayType> ::= <ArrayType> '[' ']'
        RULE_NAME                                                       =  38, // <Name> ::= <SimpleName>
        RULE_NAME2                                                      =  39, // <Name> ::= <QualifiedName>
        RULE_SIMPLENAME_IDENTIFIER                                      =  40, // <SimpleName> ::= Identifier
        RULE_QUALIFIEDNAME_DOT_IDENTIFIER                               =  41, // <QualifiedName> ::= <Name> '.' Identifier
        RULE_COMPILATIONUNIT                                            =  42, // <CompilationUnit> ::= <PackageDeclaration> <ImportDeclarations> <TypeDeclarations>
        RULE_COMPILATIONUNIT2                                           =  43, // <CompilationUnit> ::= <PackageDeclaration> <ImportDeclarations>
        RULE_COMPILATIONUNIT3                                           =  44, // <CompilationUnit> ::= <PackageDeclaration> <TypeDeclarations>
        RULE_COMPILATIONUNIT4                                           =  45, // <CompilationUnit> ::= <PackageDeclaration>
        RULE_COMPILATIONUNIT5                                           =  46, // <CompilationUnit> ::= <ImportDeclarations> <TypeDeclarations>
        RULE_COMPILATIONUNIT6                                           =  47, // <CompilationUnit> ::= <ImportDeclarations>
        RULE_COMPILATIONUNIT7                                           =  48, // <CompilationUnit> ::= <TypeDeclarations>
        RULE_COMPILATIONUNIT8                                           =  49, // <CompilationUnit> ::= 
        RULE_IMPORTDECLARATIONS                                         =  50, // <ImportDeclarations> ::= <ImportDeclaration>
        RULE_IMPORTDECLARATIONS2                                        =  51, // <ImportDeclarations> ::= <ImportDeclarations> <ImportDeclaration>
        RULE_TYPEDECLARATIONS                                           =  52, // <TypeDeclarations> ::= <TypeDeclaration>
        RULE_TYPEDECLARATIONS2                                          =  53, // <TypeDeclarations> ::= <TypeDeclarations> <TypeDeclaration>
        RULE_PACKAGEDECLARATION_PACKAGE_SEMI                            =  54, // <PackageDeclaration> ::= package <Name> ';'
        RULE_IMPORTDECLARATION                                          =  55, // <ImportDeclaration> ::= <SingleTypeImportDeclaration>
        RULE_IMPORTDECLARATION2                                         =  56, // <ImportDeclaration> ::= <TypeImportOnDemandDeclaration>
        RULE_SINGLETYPEIMPORTDECLARATION_IMPORT_SEMI                    =  57, // <SingleTypeImportDeclaration> ::= import <Name> ';'
        RULE_TYPEIMPORTONDEMANDDECLARATION_IMPORT_DOT_TIMES_SEMI        =  58, // <TypeImportOnDemandDeclaration> ::= import <Name> '.' '*' ';'
        RULE_TYPEDECLARATION                                            =  59, // <TypeDeclaration> ::= <ClassDeclaration>
        RULE_TYPEDECLARATION2                                           =  60, // <TypeDeclaration> ::= <InterfaceDeclaration>
        RULE_TYPEDECLARATION_SEMI                                       =  61, // <TypeDeclaration> ::= ';'
        RULE_MODIFIERS                                                  =  62, // <Modifiers> ::= <Modifier>
        RULE_MODIFIERS2                                                 =  63, // <Modifiers> ::= <Modifiers> <Modifier>
        RULE_MODIFIER_PUBLIC                                            =  64, // <Modifier> ::= public
        RULE_MODIFIER_PROTECTED                                         =  65, // <Modifier> ::= protected
        RULE_MODIFIER_PRIVATE                                           =  66, // <Modifier> ::= private
        RULE_MODIFIER_STATIC                                            =  67, // <Modifier> ::= static
        RULE_MODIFIER_ABSTRACT                                          =  68, // <Modifier> ::= abstract
        RULE_MODIFIER_FINAL                                             =  69, // <Modifier> ::= final
        RULE_MODIFIER_NATIVE                                            =  70, // <Modifier> ::= native
        RULE_MODIFIER_SYNCHRONIZED                                      =  71, // <Modifier> ::= synchronized
        RULE_MODIFIER_TRANSIENT                                         =  72, // <Modifier> ::= transient
        RULE_MODIFIER_VOLATILE                                          =  73, // <Modifier> ::= volatile
        RULE_CLASSDECLARATION_CLASS_IDENTIFIER                          =  74, // <ClassDeclaration> ::= <Modifiers> class Identifier <Super> <Interfaces> <ClassBody>
        RULE_CLASSDECLARATION_CLASS_IDENTIFIER2                         =  75, // <ClassDeclaration> ::= <Modifiers> class Identifier <Super> <ClassBody>
        RULE_CLASSDECLARATION_CLASS_IDENTIFIER3                         =  76, // <ClassDeclaration> ::= <Modifiers> class Identifier <Interfaces> <ClassBody>
        RULE_CLASSDECLARATION_CLASS_IDENTIFIER4                         =  77, // <ClassDeclaration> ::= <Modifiers> class Identifier <ClassBody>
        RULE_CLASSDECLARATION_CLASS_IDENTIFIER5                         =  78, // <ClassDeclaration> ::= class Identifier <Super> <Interfaces> <ClassBody>
        RULE_CLASSDECLARATION_CLASS_IDENTIFIER6                         =  79, // <ClassDeclaration> ::= class Identifier <Super> <ClassBody>
        RULE_CLASSDECLARATION_CLASS_IDENTIFIER7                         =  80, // <ClassDeclaration> ::= class Identifier <Interfaces> <ClassBody>
        RULE_CLASSDECLARATION_CLASS_IDENTIFIER8                         =  81, // <ClassDeclaration> ::= class Identifier <ClassBody>
        RULE_SUPER_EXTENDS                                              =  82, // <Super> ::= extends <ClassType>
        RULE_INTERFACES_IMPLEMENTS                                      =  83, // <Interfaces> ::= implements <InterfaceTypeList>
        RULE_INTERFACETYPELIST                                          =  84, // <InterfaceTypeList> ::= <InterfaceType>
        RULE_INTERFACETYPELIST_COMMA                                    =  85, // <InterfaceTypeList> ::= <InterfaceTypeList> ',' <InterfaceType>
        RULE_CLASSBODY_LBRACE_RBRACE                                    =  86, // <ClassBody> ::= '{' <ClassBodyDeclarations> '}'
        RULE_CLASSBODY_LBRACE_RBRACE2                                   =  87, // <ClassBody> ::= '{' '}'
        RULE_CLASSBODYDECLARATIONS                                      =  88, // <ClassBodyDeclarations> ::= <ClassBodyDeclaration>
        RULE_CLASSBODYDECLARATIONS2                                     =  89, // <ClassBodyDeclarations> ::= <ClassBodyDeclarations> <ClassBodyDeclaration>
        RULE_CLASSBODYDECLARATION                                       =  90, // <ClassBodyDeclaration> ::= <ClassMemberDeclaration>
        RULE_CLASSBODYDECLARATION2                                      =  91, // <ClassBodyDeclaration> ::= <StaticInitializer>
        RULE_CLASSBODYDECLARATION3                                      =  92, // <ClassBodyDeclaration> ::= <ConstructorDeclaration>
        RULE_CLASSMEMBERDECLARATION                                     =  93, // <ClassMemberDeclaration> ::= <FieldDeclaration>
        RULE_CLASSMEMBERDECLARATION2                                    =  94, // <ClassMemberDeclaration> ::= <MethodDeclaration>
        RULE_FIELDDECLARATION_SEMI                                      =  95, // <FieldDeclaration> ::= <Modifiers> <Type> <VariableDeclarators> ';'
        RULE_FIELDDECLARATION_SEMI2                                     =  96, // <FieldDeclaration> ::= <Type> <VariableDeclarators> ';'
        RULE_VARIABLEDECLARATORS                                        =  97, // <VariableDeclarators> ::= <VariableDeclarator>
        RULE_VARIABLEDECLARATORS_COMMA                                  =  98, // <VariableDeclarators> ::= <VariableDeclarators> ',' <VariableDeclarator>
        RULE_VARIABLEDECLARATOR                                         =  99, // <VariableDeclarator> ::= <VariableDeclaratorId>
        RULE_VARIABLEDECLARATOR_EQ                                      = 100, // <VariableDeclarator> ::= <VariableDeclaratorId> '=' <VariableInitializer>
        RULE_VARIABLEDECLARATORID_IDENTIFIER                            = 101, // <VariableDeclaratorId> ::= Identifier
        RULE_VARIABLEDECLARATORID_LBRACKET_RBRACKET                     = 102, // <VariableDeclaratorId> ::= <VariableDeclaratorId> '[' ']'
        RULE_VARIABLEINITIALIZER                                        = 103, // <VariableInitializer> ::= <Expression>
        RULE_VARIABLEINITIALIZER2                                       = 104, // <VariableInitializer> ::= <ArrayInitializer>
        RULE_METHODDECLARATION                                          = 105, // <MethodDeclaration> ::= <MethodHeader> <MethodBody>
        RULE_METHODHEADER                                               = 106, // <MethodHeader> ::= <Modifiers> <Type> <MethodDeclarator> <Throws>
        RULE_METHODHEADER2                                              = 107, // <MethodHeader> ::= <Modifiers> <Type> <MethodDeclarator>
        RULE_METHODHEADER3                                              = 108, // <MethodHeader> ::= <Type> <MethodDeclarator> <Throws>
        RULE_METHODHEADER4                                              = 109, // <MethodHeader> ::= <Type> <MethodDeclarator>
        RULE_METHODHEADER_VOID                                          = 110, // <MethodHeader> ::= <Modifiers> void <MethodDeclarator> <Throws>
        RULE_METHODHEADER_VOID2                                         = 111, // <MethodHeader> ::= <Modifiers> void <MethodDeclarator>
        RULE_METHODHEADER_VOID3                                         = 112, // <MethodHeader> ::= void <MethodDeclarator> <Throws>
        RULE_METHODHEADER_VOID4                                         = 113, // <MethodHeader> ::= void <MethodDeclarator>
        RULE_METHODDECLARATOR_IDENTIFIER_LPAREN_RPAREN                  = 114, // <MethodDeclarator> ::= Identifier '(' <FormalParameterList> ')'
        RULE_METHODDECLARATOR_IDENTIFIER_LPAREN_RPAREN2                 = 115, // <MethodDeclarator> ::= Identifier '(' ')'
        RULE_METHODDECLARATOR_LBRACKET_RBRACKET                         = 116, // <MethodDeclarator> ::= <MethodDeclarator> '[' ']'
        RULE_FORMALPARAMETERLIST                                        = 117, // <FormalParameterList> ::= <FormalParameter>
        RULE_FORMALPARAMETERLIST_COMMA                                  = 118, // <FormalParameterList> ::= <FormalParameterList> ',' <FormalParameter>
        RULE_FORMALPARAMETER                                            = 119, // <FormalParameter> ::= <Type> <VariableDeclaratorId>
        RULE_THROWS_THROWS                                              = 120, // <Throws> ::= throws <ClassTypeList>
        RULE_CLASSTYPELIST                                              = 121, // <ClassTypeList> ::= <ClassType>
        RULE_CLASSTYPELIST_COMMA                                        = 122, // <ClassTypeList> ::= <ClassTypeList> ',' <ClassType>
        RULE_METHODBODY                                                 = 123, // <MethodBody> ::= <Block>
        RULE_METHODBODY_SEMI                                            = 124, // <MethodBody> ::= ';'
        RULE_STATICINITIALIZER_STATIC                                   = 125, // <StaticInitializer> ::= static <Block>
        RULE_CONSTRUCTORDECLARATION                                     = 126, // <ConstructorDeclaration> ::= <Modifiers> <ConstructorDeclarator> <Throws> <ConstructorBody>
        RULE_CONSTRUCTORDECLARATION2                                    = 127, // <ConstructorDeclaration> ::= <Modifiers> <ConstructorDeclarator> <ConstructorBody>
        RULE_CONSTRUCTORDECLARATION3                                    = 128, // <ConstructorDeclaration> ::= <ConstructorDeclarator> <Throws> <ConstructorBody>
        RULE_CONSTRUCTORDECLARATION4                                    = 129, // <ConstructorDeclaration> ::= <ConstructorDeclarator> <ConstructorBody>
        RULE_CONSTRUCTORDECLARATOR_LPAREN_RPAREN                        = 130, // <ConstructorDeclarator> ::= <SimpleName> '(' <FormalParameterList> ')'
        RULE_CONSTRUCTORDECLARATOR_LPAREN_RPAREN2                       = 131, // <ConstructorDeclarator> ::= <SimpleName> '(' ')'
        RULE_CONSTRUCTORBODY_LBRACE_RBRACE                              = 132, // <ConstructorBody> ::= '{' <ExplicitConstructorInvocation> <BlockStatements> '}'
        RULE_CONSTRUCTORBODY_LBRACE_RBRACE2                             = 133, // <ConstructorBody> ::= '{' <ExplicitConstructorInvocation> '}'
        RULE_CONSTRUCTORBODY_LBRACE_RBRACE3                             = 134, // <ConstructorBody> ::= '{' <BlockStatements> '}'
        RULE_CONSTRUCTORBODY_LBRACE_RBRACE4                             = 135, // <ConstructorBody> ::= '{' '}'
        RULE_EXPLICITCONSTRUCTORINVOCATION_THIS_LPAREN_RPAREN_SEMI      = 136, // <ExplicitConstructorInvocation> ::= this '(' <ArgumentList> ')' ';'
        RULE_EXPLICITCONSTRUCTORINVOCATION_THIS_LPAREN_RPAREN_SEMI2     = 137, // <ExplicitConstructorInvocation> ::= this '(' ')' ';'
        RULE_EXPLICITCONSTRUCTORINVOCATION_SUPER_LPAREN_RPAREN_SEMI     = 138, // <ExplicitConstructorInvocation> ::= super '(' <ArgumentList> ')' ';'
        RULE_EXPLICITCONSTRUCTORINVOCATION_SUPER_LPAREN_RPAREN_SEMI2    = 139, // <ExplicitConstructorInvocation> ::= super '(' ')' ';'
        RULE_INTERFACEDECLARATION_INTERFACE_IDENTIFIER                  = 140, // <InterfaceDeclaration> ::= <Modifiers> interface Identifier <ExtendsInterfaces> <InterfaceBody>
        RULE_INTERFACEDECLARATION_INTERFACE_IDENTIFIER2                 = 141, // <InterfaceDeclaration> ::= <Modifiers> interface Identifier <InterfaceBody>
        RULE_INTERFACEDECLARATION_INTERFACE_IDENTIFIER3                 = 142, // <InterfaceDeclaration> ::= interface Identifier <ExtendsInterfaces> <InterfaceBody>
        RULE_INTERFACEDECLARATION_INTERFACE_IDENTIFIER4                 = 143, // <InterfaceDeclaration> ::= interface Identifier <InterfaceBody>
        RULE_EXTENDSINTERFACES_EXTENDS                                  = 144, // <ExtendsInterfaces> ::= extends <InterfaceType>
        RULE_EXTENDSINTERFACES_COMMA                                    = 145, // <ExtendsInterfaces> ::= <ExtendsInterfaces> ',' <InterfaceType>
        RULE_INTERFACEBODY_LBRACE_RBRACE                                = 146, // <InterfaceBody> ::= '{' <InterfaceMemberDeclarations> '}'
        RULE_INTERFACEBODY_LBRACE_RBRACE2                               = 147, // <InterfaceBody> ::= '{' '}'
        RULE_INTERFACEMEMBERDECLARATIONS                                = 148, // <InterfaceMemberDeclarations> ::= <InterfaceMemberDeclaration>
        RULE_INTERFACEMEMBERDECLARATIONS2                               = 149, // <InterfaceMemberDeclarations> ::= <InterfaceMemberDeclarations> <InterfaceMemberDeclaration>
        RULE_INTERFACEMEMBERDECLARATION                                 = 150, // <InterfaceMemberDeclaration> ::= <ConstantDeclaration>
        RULE_INTERFACEMEMBERDECLARATION2                                = 151, // <InterfaceMemberDeclaration> ::= <AbstractMethodDeclaration>
        RULE_CONSTANTDECLARATION                                        = 152, // <ConstantDeclaration> ::= <FieldDeclaration>
        RULE_ABSTRACTMETHODDECLARATION_SEMI                             = 153, // <AbstractMethodDeclaration> ::= <MethodHeader> ';'
        RULE_ARRAYINITIALIZER_LBRACE_COMMA_RBRACE                       = 154, // <ArrayInitializer> ::= '{' <VariableInitializers> ',' '}'
        RULE_ARRAYINITIALIZER_LBRACE_RBRACE                             = 155, // <ArrayInitializer> ::= '{' <VariableInitializers> '}'
        RULE_ARRAYINITIALIZER_LBRACE_COMMA_RBRACE2                      = 156, // <ArrayInitializer> ::= '{' ',' '}'
        RULE_ARRAYINITIALIZER_LBRACE_RBRACE2                            = 157, // <ArrayInitializer> ::= '{' '}'
        RULE_VARIABLEINITIALIZERS                                       = 158, // <VariableInitializers> ::= <VariableInitializer>
        RULE_VARIABLEINITIALIZERS_COMMA                                 = 159, // <VariableInitializers> ::= <VariableInitializers> ',' <VariableInitializer>
        RULE_BLOCK_LBRACE_RBRACE                                        = 160, // <Block> ::= '{' <BlockStatements> '}'
        RULE_BLOCK_LBRACE_RBRACE2                                       = 161, // <Block> ::= '{' '}'
        RULE_BLOCKSTATEMENTS                                            = 162, // <BlockStatements> ::= <BlockStatement>
        RULE_BLOCKSTATEMENTS2                                           = 163, // <BlockStatements> ::= <BlockStatements> <BlockStatement>
        RULE_BLOCKSTATEMENT                                             = 164, // <BlockStatement> ::= <LocalVariableDeclarationStatement>
        RULE_BLOCKSTATEMENT2                                            = 165, // <BlockStatement> ::= <Statement>
        RULE_LOCALVARIABLEDECLARATIONSTATEMENT_SEMI                     = 166, // <LocalVariableDeclarationStatement> ::= <LocalVariableDeclaration> ';'
        RULE_LOCALVARIABLEDECLARATION                                   = 167, // <LocalVariableDeclaration> ::= <Type> <VariableDeclarators>
        RULE_STATEMENT                                                  = 168, // <Statement> ::= <StatementWithoutTrailingSubstatement>
        RULE_STATEMENT2                                                 = 169, // <Statement> ::= <LabeledStatement>
        RULE_STATEMENT3                                                 = 170, // <Statement> ::= <IfThenStatement>
        RULE_STATEMENT4                                                 = 171, // <Statement> ::= <IfThenElseStatement>
        RULE_STATEMENT5                                                 = 172, // <Statement> ::= <WhileStatement>
        RULE_STATEMENT6                                                 = 173, // <Statement> ::= <ForStatement>
        RULE_STATEMENTNOSHORTIF                                         = 174, // <StatementNoShortIf> ::= <StatementWithoutTrailingSubstatement>
        RULE_STATEMENTNOSHORTIF2                                        = 175, // <StatementNoShortIf> ::= <LabeledStatementNoShortIf>
        RULE_STATEMENTNOSHORTIF3                                        = 176, // <StatementNoShortIf> ::= <IfThenElseStatementNoShortIf>
        RULE_STATEMENTNOSHORTIF4                                        = 177, // <StatementNoShortIf> ::= <WhileStatementNoShortIf>
        RULE_STATEMENTNOSHORTIF5                                        = 178, // <StatementNoShortIf> ::= <ForStatementNoShortIf>
        RULE_STATEMENTWITHOUTTRAILINGSUBSTATEMENT                       = 179, // <StatementWithoutTrailingSubstatement> ::= <Block>
        RULE_STATEMENTWITHOUTTRAILINGSUBSTATEMENT2                      = 180, // <StatementWithoutTrailingSubstatement> ::= <EmptyStatement>
        RULE_STATEMENTWITHOUTTRAILINGSUBSTATEMENT3                      = 181, // <StatementWithoutTrailingSubstatement> ::= <ExpressionStatement>
        RULE_STATEMENTWITHOUTTRAILINGSUBSTATEMENT4                      = 182, // <StatementWithoutTrailingSubstatement> ::= <SwitchStatement>
        RULE_STATEMENTWITHOUTTRAILINGSUBSTATEMENT5                      = 183, // <StatementWithoutTrailingSubstatement> ::= <DoStatement>
        RULE_STATEMENTWITHOUTTRAILINGSUBSTATEMENT6                      = 184, // <StatementWithoutTrailingSubstatement> ::= <BreakStatement>
        RULE_STATEMENTWITHOUTTRAILINGSUBSTATEMENT7                      = 185, // <StatementWithoutTrailingSubstatement> ::= <ContinueStatement>
        RULE_STATEMENTWITHOUTTRAILINGSUBSTATEMENT8                      = 186, // <StatementWithoutTrailingSubstatement> ::= <ReturnStatement>
        RULE_STATEMENTWITHOUTTRAILINGSUBSTATEMENT9                      = 187, // <StatementWithoutTrailingSubstatement> ::= <SynchronizedStatement>
        RULE_STATEMENTWITHOUTTRAILINGSUBSTATEMENT10                     = 188, // <StatementWithoutTrailingSubstatement> ::= <ThrowStatement>
        RULE_STATEMENTWITHOUTTRAILINGSUBSTATEMENT11                     = 189, // <StatementWithoutTrailingSubstatement> ::= <TryStatement>
        RULE_EMPTYSTATEMENT_SEMI                                        = 190, // <EmptyStatement> ::= ';'
        RULE_LABELEDSTATEMENT_IDENTIFIER_COLON                          = 191, // <LabeledStatement> ::= Identifier ':' <Statement>
        RULE_LABELEDSTATEMENTNOSHORTIF_IDENTIFIER_COLON                 = 192, // <LabeledStatementNoShortIf> ::= Identifier ':' <StatementNoShortIf>
        RULE_EXPRESSIONSTATEMENT_SEMI                                   = 193, // <ExpressionStatement> ::= <StatementExpression> ';'
        RULE_STATEMENTEXPRESSION                                        = 194, // <StatementExpression> ::= <Assignment>
        RULE_STATEMENTEXPRESSION2                                       = 195, // <StatementExpression> ::= <PreIncrementExpression>
        RULE_STATEMENTEXPRESSION3                                       = 196, // <StatementExpression> ::= <PreDecrementExpression>
        RULE_STATEMENTEXPRESSION4                                       = 197, // <StatementExpression> ::= <PostIncrementExpression>
        RULE_STATEMENTEXPRESSION5                                       = 198, // <StatementExpression> ::= <PostDecrementExpression>
        RULE_STATEMENTEXPRESSION6                                       = 199, // <StatementExpression> ::= <MethodInvocation>
        RULE_STATEMENTEXPRESSION7                                       = 200, // <StatementExpression> ::= <ClassInstanceCreationExpression>
        RULE_IFTHENSTATEMENT_IF_LPAREN_RPAREN                           = 201, // <IfThenStatement> ::= if '(' <Expression> ')' <Statement>
        RULE_IFTHENELSESTATEMENT_IF_LPAREN_RPAREN_ELSE                  = 202, // <IfThenElseStatement> ::= if '(' <Expression> ')' <StatementNoShortIf> else <Statement>
        RULE_IFTHENELSESTATEMENTNOSHORTIF_IF_LPAREN_RPAREN_ELSE         = 203, // <IfThenElseStatementNoShortIf> ::= if '(' <Expression> ')' <StatementNoShortIf> else <StatementNoShortIf>
        RULE_SWITCHSTATEMENT_SWITCH_LPAREN_RPAREN                       = 204, // <SwitchStatement> ::= switch '(' <Expression> ')' <SwitchBlock>
        RULE_SWITCHBLOCK_LBRACE_RBRACE                                  = 205, // <SwitchBlock> ::= '{' <SwitchBlockStatementGroups> <SwitchLabels> '}'
        RULE_SWITCHBLOCK_LBRACE_RBRACE2                                 = 206, // <SwitchBlock> ::= '{' <SwitchBlockStatementGroups> '}'
        RULE_SWITCHBLOCK_LBRACE_RBRACE3                                 = 207, // <SwitchBlock> ::= '{' <SwitchLabels> '}'
        RULE_SWITCHBLOCK_LBRACE_RBRACE4                                 = 208, // <SwitchBlock> ::= '{' '}'
        RULE_SWITCHBLOCKSTATEMENTGROUPS                                 = 209, // <SwitchBlockStatementGroups> ::= <SwitchBlockStatementGroup>
        RULE_SWITCHBLOCKSTATEMENTGROUPS2                                = 210, // <SwitchBlockStatementGroups> ::= <SwitchBlockStatementGroups> <SwitchBlockStatementGroup>
        RULE_SWITCHBLOCKSTATEMENTGROUP                                  = 211, // <SwitchBlockStatementGroup> ::= <SwitchLabels> <BlockStatements>
        RULE_SWITCHLABELS                                               = 212, // <SwitchLabels> ::= <SwitchLabel>
        RULE_SWITCHLABELS2                                              = 213, // <SwitchLabels> ::= <SwitchLabels> <SwitchLabel>
        RULE_SWITCHLABEL_CASE_COLON                                     = 214, // <SwitchLabel> ::= case <ConstantExpression> ':'
        RULE_SWITCHLABEL_DEFAULT_COLON                                  = 215, // <SwitchLabel> ::= default ':'
        RULE_WHILESTATEMENT_WHILE_LPAREN_RPAREN                         = 216, // <WhileStatement> ::= while '(' <Expression> ')' <Statement>
        RULE_WHILESTATEMENTNOSHORTIF_WHILE_LPAREN_RPAREN                = 217, // <WhileStatementNoShortIf> ::= while '(' <Expression> ')' <StatementNoShortIf>
        RULE_DOSTATEMENT_DO_WHILE_LPAREN_RPAREN_SEMI                    = 218, // <DoStatement> ::= do <Statement> while '(' <Expression> ')' ';'
        RULE_FORSTATEMENT_FOR_LPAREN_SEMI_SEMI_RPAREN                   = 219, // <ForStatement> ::= for '(' <ForInit> ';' <Expression> ';' <ForUpdate> ')' <Statement>
        RULE_FORSTATEMENT_FOR_LPAREN_SEMI_SEMI_RPAREN2                  = 220, // <ForStatement> ::= for '(' <ForInit> ';' <Expression> ';' ')' <Statement>
        RULE_FORSTATEMENT_FOR_LPAREN_SEMI_SEMI_RPAREN3                  = 221, // <ForStatement> ::= for '(' <ForInit> ';' ';' <ForUpdate> ')' <Statement>
        RULE_FORSTATEMENT_FOR_LPAREN_SEMI_SEMI_RPAREN4                  = 222, // <ForStatement> ::= for '(' <ForInit> ';' ';' ')' <Statement>
        RULE_FORSTATEMENT_FOR_LPAREN_SEMI_SEMI_RPAREN5                  = 223, // <ForStatement> ::= for '(' ';' <Expression> ';' <ForUpdate> ')' <Statement>
        RULE_FORSTATEMENT_FOR_LPAREN_SEMI_SEMI_RPAREN6                  = 224, // <ForStatement> ::= for '(' ';' <Expression> ';' ')' <Statement>
        RULE_FORSTATEMENT_FOR_LPAREN_SEMI_SEMI_RPAREN7                  = 225, // <ForStatement> ::= for '(' ';' ';' <ForUpdate> ')' <Statement>
        RULE_FORSTATEMENT_FOR_LPAREN_SEMI_SEMI_RPAREN8                  = 226, // <ForStatement> ::= for '(' ';' ';' ')' <Statement>
        RULE_FORSTATEMENTNOSHORTIF_FOR_LPAREN_SEMI_SEMI_RPAREN          = 227, // <ForStatementNoShortIf> ::= for '(' <ForInit> ';' <Expression> ';' <ForUpdate> ')' <StatementNoShortIf>
        RULE_FORSTATEMENTNOSHORTIF_FOR_LPAREN_SEMI_SEMI_RPAREN2         = 228, // <ForStatementNoShortIf> ::= for '(' <ForInit> ';' <Expression> ';' ')' <StatementNoShortIf>
        RULE_FORSTATEMENTNOSHORTIF_FOR_LPAREN_SEMI_SEMI_RPAREN3         = 229, // <ForStatementNoShortIf> ::= for '(' <ForInit> ';' ';' <ForUpdate> ')' <StatementNoShortIf>
        RULE_FORSTATEMENTNOSHORTIF_FOR_LPAREN_SEMI_SEMI_RPAREN4         = 230, // <ForStatementNoShortIf> ::= for '(' <ForInit> ';' ';' ')' <StatementNoShortIf>
        RULE_FORSTATEMENTNOSHORTIF_FOR_LPAREN_SEMI_SEMI_RPAREN5         = 231, // <ForStatementNoShortIf> ::= for '(' ';' <Expression> ';' <ForUpdate> ')' <StatementNoShortIf>
        RULE_FORSTATEMENTNOSHORTIF_FOR_LPAREN_SEMI_SEMI_RPAREN6         = 232, // <ForStatementNoShortIf> ::= for '(' ';' <Expression> ';' ')' <StatementNoShortIf>
        RULE_FORSTATEMENTNOSHORTIF_FOR_LPAREN_SEMI_SEMI_RPAREN7         = 233, // <ForStatementNoShortIf> ::= for '(' ';' ';' <ForUpdate> ')' <StatementNoShortIf>
        RULE_FORSTATEMENTNOSHORTIF_FOR_LPAREN_SEMI_SEMI_RPAREN8         = 234, // <ForStatementNoShortIf> ::= for '(' ';' ';' ')' <StatementNoShortIf>
        RULE_FORINIT                                                    = 235, // <ForInit> ::= <StatementExpressionList>
        RULE_FORINIT2                                                   = 236, // <ForInit> ::= <LocalVariableDeclaration>
        RULE_FORUPDATE                                                  = 237, // <ForUpdate> ::= <StatementExpressionList>
        RULE_STATEMENTEXPRESSIONLIST                                    = 238, // <StatementExpressionList> ::= <StatementExpression>
        RULE_STATEMENTEXPRESSIONLIST_COMMA                              = 239, // <StatementExpressionList> ::= <StatementExpressionList> ',' <StatementExpression>
        RULE_BREAKSTATEMENT_BREAK_IDENTIFIER_SEMI                       = 240, // <BreakStatement> ::= break Identifier ';'
        RULE_BREAKSTATEMENT_BREAK_SEMI                                  = 241, // <BreakStatement> ::= break ';'
        RULE_CONTINUESTATEMENT_CONTINUE_IDENTIFIER_SEMI                 = 242, // <ContinueStatement> ::= continue Identifier ';'
        RULE_CONTINUESTATEMENT_CONTINUE_SEMI                            = 243, // <ContinueStatement> ::= continue ';'
        RULE_RETURNSTATEMENT_RETURN_SEMI                                = 244, // <ReturnStatement> ::= return <Expression> ';'
        RULE_RETURNSTATEMENT_RETURN_SEMI2                               = 245, // <ReturnStatement> ::= return ';'
        RULE_THROWSTATEMENT_THROW_SEMI                                  = 246, // <ThrowStatement> ::= throw <Expression> ';'
        RULE_SYNCHRONIZEDSTATEMENT_SYNCHRONIZED_LPAREN_RPAREN           = 247, // <SynchronizedStatement> ::= synchronized '(' <Expression> ')' <Block>
        RULE_TRYSTATEMENT_TRY                                           = 248, // <TryStatement> ::= try <Block> <Catches>
        RULE_TRYSTATEMENT_TRY2                                          = 249, // <TryStatement> ::= try <Block> <Catches> <Finally>
        RULE_TRYSTATEMENT_TRY3                                          = 250, // <TryStatement> ::= try <Block> <Finally>
        RULE_CATCHES                                                    = 251, // <Catches> ::= <CatchClause>
        RULE_CATCHES2                                                   = 252, // <Catches> ::= <Catches> <CatchClause>
        RULE_CATCHCLAUSE_CATCH_LPAREN_RPAREN                            = 253, // <CatchClause> ::= catch '(' <FormalParameter> ')' <Block>
        RULE_FINALLY_FINALLY                                            = 254, // <Finally> ::= finally <Block>
        RULE_PRIMARY                                                    = 255, // <Primary> ::= <PrimaryNoNewArray>
        RULE_PRIMARY2                                                   = 256, // <Primary> ::= <ArrayCreationExpression>
        RULE_PRIMARYNONEWARRAY                                          = 257, // <PrimaryNoNewArray> ::= <Literal>
        RULE_PRIMARYNONEWARRAY_THIS                                     = 258, // <PrimaryNoNewArray> ::= this
        RULE_PRIMARYNONEWARRAY_LPAREN_RPAREN                            = 259, // <PrimaryNoNewArray> ::= '(' <Expression> ')'
        RULE_PRIMARYNONEWARRAY2                                         = 260, // <PrimaryNoNewArray> ::= <ClassInstanceCreationExpression>
        RULE_PRIMARYNONEWARRAY3                                         = 261, // <PrimaryNoNewArray> ::= <FieldAccess>
        RULE_PRIMARYNONEWARRAY4                                         = 262, // <PrimaryNoNewArray> ::= <MethodInvocation>
        RULE_PRIMARYNONEWARRAY5                                         = 263, // <PrimaryNoNewArray> ::= <ArrayAccess>
        RULE_CLASSINSTANCECREATIONEXPRESSION_NEW_LPAREN_RPAREN          = 264, // <ClassInstanceCreationExpression> ::= new <ClassType> '(' <ArgumentList> ')'
        RULE_CLASSINSTANCECREATIONEXPRESSION_NEW_LPAREN_RPAREN2         = 265, // <ClassInstanceCreationExpression> ::= new <ClassType> '(' ')'
        RULE_ARGUMENTLIST                                               = 266, // <ArgumentList> ::= <Expression>
        RULE_ARGUMENTLIST_COMMA                                         = 267, // <ArgumentList> ::= <ArgumentList> ',' <Expression>
        RULE_ARRAYCREATIONEXPRESSION_NEW                                = 268, // <ArrayCreationExpression> ::= new <PrimitiveType> <DimExprs> <Dims>
        RULE_ARRAYCREATIONEXPRESSION_NEW2                               = 269, // <ArrayCreationExpression> ::= new <PrimitiveType> <DimExprs>
        RULE_ARRAYCREATIONEXPRESSION_NEW3                               = 270, // <ArrayCreationExpression> ::= new <ClassOrInterfaceType> <DimExprs> <Dims>
        RULE_ARRAYCREATIONEXPRESSION_NEW4                               = 271, // <ArrayCreationExpression> ::= new <ClassOrInterfaceType> <DimExprs>
        RULE_DIMEXPRS                                                   = 272, // <DimExprs> ::= <DimExpr>
        RULE_DIMEXPRS2                                                  = 273, // <DimExprs> ::= <DimExprs> <DimExpr>
        RULE_DIMEXPR_LBRACKET_RBRACKET                                  = 274, // <DimExpr> ::= '[' <Expression> ']'
        RULE_DIMS_LBRACKET_RBRACKET                                     = 275, // <Dims> ::= '[' ']'
        RULE_DIMS_LBRACKET_RBRACKET2                                    = 276, // <Dims> ::= <Dims> '[' ']'
        RULE_FIELDACCESS_DOT_IDENTIFIER                                 = 277, // <FieldAccess> ::= <Primary> '.' Identifier
        RULE_FIELDACCESS_SUPER_DOT_IDENTIFIER                           = 278, // <FieldAccess> ::= super '.' Identifier
        RULE_METHODINVOCATION_LPAREN_RPAREN                             = 279, // <MethodInvocation> ::= <Name> '(' <ArgumentList> ')'
        RULE_METHODINVOCATION_LPAREN_RPAREN2                            = 280, // <MethodInvocation> ::= <Name> '(' ')'
        RULE_METHODINVOCATION_DOT_IDENTIFIER_LPAREN_RPAREN              = 281, // <MethodInvocation> ::= <Primary> '.' Identifier '(' <ArgumentList> ')'
        RULE_METHODINVOCATION_DOT_IDENTIFIER_LPAREN_RPAREN2             = 282, // <MethodInvocation> ::= <Primary> '.' Identifier '(' ')'
        RULE_METHODINVOCATION_SUPER_DOT_IDENTIFIER_LPAREN_RPAREN        = 283, // <MethodInvocation> ::= super '.' Identifier '(' <ArgumentList> ')'
        RULE_METHODINVOCATION_SUPER_DOT_IDENTIFIER_LPAREN_RPAREN2       = 284, // <MethodInvocation> ::= super '.' Identifier '(' ')'
        RULE_ARRAYACCESS_LBRACKET_RBRACKET                              = 285, // <ArrayAccess> ::= <Name> '[' <Expression> ']'
        RULE_ARRAYACCESS_LBRACKET_RBRACKET2                             = 286, // <ArrayAccess> ::= <PrimaryNoNewArray> '[' <Expression> ']'
        RULE_POSTFIXEXPRESSION                                          = 287, // <PostfixExpression> ::= <Primary>
        RULE_POSTFIXEXPRESSION2                                         = 288, // <PostfixExpression> ::= <Name>
        RULE_POSTFIXEXPRESSION3                                         = 289, // <PostfixExpression> ::= <PostIncrementExpression>
        RULE_POSTFIXEXPRESSION4                                         = 290, // <PostfixExpression> ::= <PostDecrementExpression>
        RULE_POSTINCREMENTEXPRESSION_PLUSPLUS                           = 291, // <PostIncrementExpression> ::= <PostfixExpression> '++'
        RULE_POSTDECREMENTEXPRESSION_MINUSMINUS                         = 292, // <PostDecrementExpression> ::= <PostfixExpression> '--'
        RULE_UNARYEXPRESSION                                            = 293, // <UnaryExpression> ::= <PreIncrementExpression>
        RULE_UNARYEXPRESSION2                                           = 294, // <UnaryExpression> ::= <PreDecrementExpression>
        RULE_UNARYEXPRESSION_PLUS                                       = 295, // <UnaryExpression> ::= '+' <UnaryExpression>
        RULE_UNARYEXPRESSION_MINUS                                      = 296, // <UnaryExpression> ::= '-' <UnaryExpression>
        RULE_UNARYEXPRESSION3                                           = 297, // <UnaryExpression> ::= <UnaryExpressionNotPlusMinus>
        RULE_PREINCREMENTEXPRESSION_PLUSPLUS                            = 298, // <PreIncrementExpression> ::= '++' <UnaryExpression>
        RULE_PREDECREMENTEXPRESSION_MINUSMINUS                          = 299, // <PreDecrementExpression> ::= '--' <UnaryExpression>
        RULE_UNARYEXPRESSIONNOTPLUSMINUS                                = 300, // <UnaryExpressionNotPlusMinus> ::= <PostfixExpression>
        RULE_UNARYEXPRESSIONNOTPLUSMINUS_TILDE                          = 301, // <UnaryExpressionNotPlusMinus> ::= '~' <UnaryExpression>
        RULE_UNARYEXPRESSIONNOTPLUSMINUS_EXCLAM                         = 302, // <UnaryExpressionNotPlusMinus> ::= '!' <UnaryExpression>
        RULE_UNARYEXPRESSIONNOTPLUSMINUS2                               = 303, // <UnaryExpressionNotPlusMinus> ::= <CastExpression>
        RULE_CASTEXPRESSION_LPAREN_RPAREN                               = 304, // <CastExpression> ::= '(' <PrimitiveType> <Dims> ')' <UnaryExpression>
        RULE_CASTEXPRESSION_LPAREN_RPAREN2                              = 305, // <CastExpression> ::= '(' <PrimitiveType> ')' <UnaryExpression>
        RULE_CASTEXPRESSION_LPAREN_RPAREN3                              = 306, // <CastExpression> ::= '(' <Expression> ')' <UnaryExpressionNotPlusMinus>
        RULE_CASTEXPRESSION_LPAREN_RPAREN4                              = 307, // <CastExpression> ::= '(' <Name> <Dims> ')' <UnaryExpressionNotPlusMinus>
        RULE_MULTIPLICATIVEEXPRESSION                                   = 308, // <MultiplicativeExpression> ::= <UnaryExpression>
        RULE_MULTIPLICATIVEEXPRESSION_TIMES                             = 309, // <MultiplicativeExpression> ::= <MultiplicativeExpression> '*' <UnaryExpression>
        RULE_MULTIPLICATIVEEXPRESSION_DIV                               = 310, // <MultiplicativeExpression> ::= <MultiplicativeExpression> '/' <UnaryExpression>
        RULE_MULTIPLICATIVEEXPRESSION_PERCENT                           = 311, // <MultiplicativeExpression> ::= <MultiplicativeExpression> '%' <UnaryExpression>
        RULE_ADDITIVEEXPRESSION                                         = 312, // <AdditiveExpression> ::= <MultiplicativeExpression>
        RULE_ADDITIVEEXPRESSION_PLUS                                    = 313, // <AdditiveExpression> ::= <AdditiveExpression> '+' <MultiplicativeExpression>
        RULE_ADDITIVEEXPRESSION_MINUS                                   = 314, // <AdditiveExpression> ::= <AdditiveExpression> '-' <MultiplicativeExpression>
        RULE_SHIFTEXPRESSION                                            = 315, // <ShiftExpression> ::= <AdditiveExpression>
        RULE_SHIFTEXPRESSION_LTLT                                       = 316, // <ShiftExpression> ::= <ShiftExpression> '<<' <AdditiveExpression>
        RULE_SHIFTEXPRESSION_GTGT                                       = 317, // <ShiftExpression> ::= <ShiftExpression> '>>' <AdditiveExpression>
        RULE_SHIFTEXPRESSION_GTGTGT                                     = 318, // <ShiftExpression> ::= <ShiftExpression> '>>>' <AdditiveExpression>
        RULE_RELATIONALEXPRESSION                                       = 319, // <RelationalExpression> ::= <ShiftExpression>
        RULE_RELATIONALEXPRESSION_LT                                    = 320, // <RelationalExpression> ::= <RelationalExpression> '<' <ShiftExpression>
        RULE_RELATIONALEXPRESSION_GT                                    = 321, // <RelationalExpression> ::= <RelationalExpression> '>' <ShiftExpression>
        RULE_RELATIONALEXPRESSION_LTEQ                                  = 322, // <RelationalExpression> ::= <RelationalExpression> '<=' <ShiftExpression>
        RULE_RELATIONALEXPRESSION_GTEQ                                  = 323, // <RelationalExpression> ::= <RelationalExpression> '>=' <ShiftExpression>
        RULE_RELATIONALEXPRESSION_INSTANCEOF                            = 324, // <RelationalExpression> ::= <RelationalExpression> instanceof <ReferenceType>
        RULE_EQUALITYEXPRESSION                                         = 325, // <EqualityExpression> ::= <RelationalExpression>
        RULE_EQUALITYEXPRESSION_EQEQ                                    = 326, // <EqualityExpression> ::= <EqualityExpression> '==' <RelationalExpression>
        RULE_EQUALITYEXPRESSION_EXCLAMEQ                                = 327, // <EqualityExpression> ::= <EqualityExpression> '!=' <RelationalExpression>
        RULE_ANDEXPRESSION                                              = 328, // <AndExpression> ::= <EqualityExpression>
        RULE_ANDEXPRESSION_AMP                                          = 329, // <AndExpression> ::= <AndExpression> '&' <EqualityExpression>
        RULE_EXCLUSIVEOREXPRESSION                                      = 330, // <ExclusiveOrExpression> ::= <AndExpression>
        RULE_EXCLUSIVEOREXPRESSION_CARET                                = 331, // <ExclusiveOrExpression> ::= <ExclusiveOrExpression> '^' <AndExpression>
        RULE_INCLUSIVEOREXPRESSION                                      = 332, // <InclusiveOrExpression> ::= <ExclusiveOrExpression>
        RULE_INCLUSIVEOREXPRESSION_PIPE                                 = 333, // <InclusiveOrExpression> ::= <InclusiveOrExpression> '|' <ExclusiveOrExpression>
        RULE_CONDITIONALANDEXPRESSION                                   = 334, // <ConditionalAndExpression> ::= <InclusiveOrExpression>
        RULE_CONDITIONALANDEXPRESSION_AMPAMP                            = 335, // <ConditionalAndExpression> ::= <ConditionalAndExpression> '&&' <InclusiveOrExpression>
        RULE_CONDITIONALOREXPRESSION                                    = 336, // <ConditionalOrExpression> ::= <ConditionalAndExpression>
        RULE_CONDITIONALOREXPRESSION_PIPEPIPE                           = 337, // <ConditionalOrExpression> ::= <ConditionalOrExpression> '||' <ConditionalAndExpression>
        RULE_CONDITIONALEXPRESSION                                      = 338, // <ConditionalExpression> ::= <ConditionalOrExpression>
        RULE_CONDITIONALEXPRESSION_QUESTION_COLON                       = 339, // <ConditionalExpression> ::= <ConditionalOrExpression> '?' <Expression> ':' <ConditionalExpression>
        RULE_ASSIGNMENTEXPRESSION                                       = 340, // <AssignmentExpression> ::= <ConditionalExpression>
        RULE_ASSIGNMENTEXPRESSION2                                      = 341, // <AssignmentExpression> ::= <Assignment>
        RULE_ASSIGNMENT                                                 = 342, // <Assignment> ::= <LeftHandSide> <AssignmentOperator> <AssignmentExpression>
        RULE_LEFTHANDSIDE                                               = 343, // <LeftHandSide> ::= <Name>
        RULE_LEFTHANDSIDE2                                              = 344, // <LeftHandSide> ::= <FieldAccess>
        RULE_LEFTHANDSIDE3                                              = 345, // <LeftHandSide> ::= <ArrayAccess>
        RULE_ASSIGNMENTOPERATOR_EQ                                      = 346, // <AssignmentOperator> ::= '='
        RULE_ASSIGNMENTOPERATOR_TIMESEQ                                 = 347, // <AssignmentOperator> ::= '*='
        RULE_ASSIGNMENTOPERATOR_DIVEQ                                   = 348, // <AssignmentOperator> ::= '/='
        RULE_ASSIGNMENTOPERATOR_PERCENTEQ                               = 349, // <AssignmentOperator> ::= '%='
        RULE_ASSIGNMENTOPERATOR_PLUSEQ                                  = 350, // <AssignmentOperator> ::= '+='
        RULE_ASSIGNMENTOPERATOR_MINUSEQ                                 = 351, // <AssignmentOperator> ::= '-='
        RULE_ASSIGNMENTOPERATOR_LTLTEQ                                  = 352, // <AssignmentOperator> ::= '<<='
        RULE_ASSIGNMENTOPERATOR_GTGTEQ                                  = 353, // <AssignmentOperator> ::= '>>='
        RULE_ASSIGNMENTOPERATOR_GTGTGTEQ                                = 354, // <AssignmentOperator> ::= '>>>='
        RULE_ASSIGNMENTOPERATOR_AMPEQ                                   = 355, // <AssignmentOperator> ::= '&='
        RULE_ASSIGNMENTOPERATOR_CARETEQ                                 = 356, // <AssignmentOperator> ::= '^='
        RULE_ASSIGNMENTOPERATOR_PIPEEQ                                  = 357, // <AssignmentOperator> ::= '|='
        RULE_EXPRESSION                                                 = 358, // <Expression> ::= <AssignmentExpression>
        RULE_CONSTANTEXPRESSION                                         = 359  // <ConstantExpression> ::= <Expression>
    };

    public class AnalizadorSintactico
    {
        private LALRParser parser;

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
                return token.Text;

                case (int)SymbolConstants.SYMBOL_ERROR :
                //(Error)
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_WHITESPACE :
                //Whitespace
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_COMMENTEND :
                //'Comment End'
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_COMMENTLINE :
                //'Comment Line'
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_COMMENTSTART :
                //'Comment Start'
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_MINUS :
                //'-'
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_MINUSMINUS :
                //'--'
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_EXCLAM :
                //'!'
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_EXCLAMEQ :
                //'!='
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_PERCENT :
                //'%'
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_PERCENTEQ :
                //'%='
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_AMP :
                //'&'
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_AMPAMP :
                //'&&'
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_AMPEQ :
                //'&='
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_LPAREN :
                //'('
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_RPAREN :
                //')'
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_TIMES :
                //'*'
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_TIMESEQ :
                //'*='
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_COMMA :
                //','
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_DOT :
                //'.'
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_DIV :
                //'/'
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_DIVEQ :
                //'/='
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_COLON :
                //':'
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_SEMI :
                //';'
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_QUESTION :
                //'?'
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_LBRACKET :
                //'['
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_RBRACKET :
                //']'
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_CARET :
                //'^'
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_CARETEQ :
                //'^='
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_LBRACE :
                //'{'
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_PIPE :
                //'|'
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_PIPEPIPE :
                //'||'
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_PIPEEQ :
                //'|='
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_RBRACE :
                //'}'
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_TILDE :
                //'~'
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_PLUS :
                //'+'
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_PLUSPLUS :
                //'++'
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_PLUSEQ :
                //'+='
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_LT :
                //'<'
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_LTLT :
                //'<<'
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_LTLTEQ :
                //'<<='
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_LTEQ :
                //'<='
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_EQ :
                //'='
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_MINUSEQ :
                //'-='
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_EQEQ :
                //'=='
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_GT :
                //'>'
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_GTEQ :
                //'>='
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_GTGT :
                //'>>'
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_GTGTEQ :
                //'>>='
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_GTGTGT :
                //'>>>'
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_GTGTGTEQ :
                //'>>>='
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_ABSTRACT :
                //abstract
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_BOOLEAN :
                //boolean
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_BOOLEANLITERAL :
                //BooleanLiteral
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_BREAK :
                //break
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_BYTE :
                //byte
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_CASE :
                //case
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_CATCH :
                //catch
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_CHAR :
                //char
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_CLASS :
                //class
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_CONTINUE :
                //continue
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_DEFAULT :
                //default
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_DO :
                //do
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_DOUBLE :
                //double
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_ELSE :
                //else
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_EXTENDS :
                //extends
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_FINAL :
                //final
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_FINALLY :
                //finally
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_FLOAT :
                //float
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_FLOATINGPOINTLITERAL :
                //FloatingPointLiteral
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_FLOATINGPOINTLITERALEXPONENT :
                //FloatingPointLiteralExponent
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_FOR :
                //for
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_HEXESCAPECHARLITERAL :
                //HexEscapeCharLiteral
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_HEXINTEGERLITERAL :
                //HexIntegerLiteral
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_IDENTIFIER :
                //Identifier
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_IF :
                //if
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_IMPLEMENTS :
                //implements
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_IMPORT :
                //import
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_INDIRECTCHARLITERAL :
                //IndirectCharLiteral
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_INSTANCEOF :
                //instanceof
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_INT :
                //int
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_INTERFACE :
                //interface
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_LONG :
                //long
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_NATIVE :
                //native
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_NEW :
                //new
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_NULLLITERAL :
                //token.TextLiteral
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_OCTALESCAPECHARLITERAL :
                //OctalEscapeCharLiteral
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_OCTALINTEGERLITERAL :
                //OctalIntegerLiteral
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_PACKAGE :
                //package
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_PRIVATE :
                //private
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_PROTECTED :
                //protected
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_PUBLIC :
                //public
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_RETURN :
                //return
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_SHORT :
                //short
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_STANDARDESCAPECHARLITERAL :
                //StandardEscapeCharLiteral
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_STARTWITHNOZERODECIMALINTEGERLITERAL :
                //StartWithNoZeroDecimalIntegerLiteral
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_STARTWITHZERODECIMALINTEGERLITERAL :
                //StartWithZeroDecimalIntegerLiteral
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_STATIC :
                //static
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_STRINGLITERAL :
                //StringLiteral
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_SUPER :
                //super
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_SWITCH :
                //switch
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_SYNCHRONIZED :
                //synchronized
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_THIS :
                //this
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_THROW :
                //throw
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_THROWS :
                //throws
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_TRANSIENT :
                //transient
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_TRY :
                //try
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_VOID :
                //void
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_VOLATILE :
                //volatile
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_WHILE :
                //while
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_ABSTRACTMETHODDECLARATION :
                //<AbstractMethodDeclaration>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_ADDITIVEEXPRESSION :
                //<AdditiveExpression>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_ANDEXPRESSION :
                //<AndExpression>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_ARGUMENTLIST :
                //<ArgumentList>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_ARRAYACCESS :
                //<ArrayAccess>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_ARRAYCREATIONEXPRESSION :
                //<ArrayCreationExpression>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_ARRAYINITIALIZER :
                //<ArrayInitializer>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_ARRAYTYPE :
                //<ArrayType>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_ASSIGNMENT :
                //<Assignment>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_ASSIGNMENTEXPRESSION :
                //<AssignmentExpression>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_ASSIGNMENTOPERATOR :
                //<AssignmentOperator>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_BLOCK :
                //<Block>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_BLOCKSTATEMENT :
                //<BlockStatement>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_BLOCKSTATEMENTS :
                //<BlockStatements>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_BREAKSTATEMENT :
                //<BreakStatement>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_CASTEXPRESSION :
                //<CastExpression>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_CATCHCLAUSE :
                //<CatchClause>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_CATCHES :
                //<Catches>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_CHARACTERLITERAL :
                //<CharacterLiteral>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_CLASSBODY :
                //<ClassBody>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_CLASSBODYDECLARATION :
                //<ClassBodyDeclaration>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_CLASSBODYDECLARATIONS :
                //<ClassBodyDeclarations>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_CLASSDECLARATION :
                //<ClassDeclaration>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_CLASSINSTANCECREATIONEXPRESSION :
                //<ClassInstanceCreationExpression>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_CLASSMEMBERDECLARATION :
                //<ClassMemberDeclaration>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_CLASSORINTERFACETYPE :
                //<ClassOrInterfaceType>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_CLASSTYPE :
                //<ClassType>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_CLASSTYPELIST :
                //<ClassTypeList>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_COMPILATIONUNIT :
                //<CompilationUnit>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_CONDITIONALANDEXPRESSION :
                //<ConditionalAndExpression>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_CONDITIONALEXPRESSION :
                //<ConditionalExpression>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_CONDITIONALOREXPRESSION :
                //<ConditionalOrExpression>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_CONSTANTDECLARATION :
                //<ConstantDeclaration>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_CONSTANTEXPRESSION :
                //<ConstantExpression>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_CONSTRUCTORBODY :
                //<ConstructorBody>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_CONSTRUCTORDECLARATION :
                //<ConstructorDeclaration>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_CONSTRUCTORDECLARATOR :
                //<ConstructorDeclarator>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_CONTINUESTATEMENT :
                //<ContinueStatement>
                //todo: Create a new object that corresponds to the symbol
                return token.Text;

                case (int)SymbolConstants.SYMBOL_DECIMALINTEGERLITERAL :
                //<DecimalIntegerLiteral>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DIMEXPR :
                //<DimExpr>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DIMEXPRS :
                //<DimExprs>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DIMS :
                //<Dims>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DOSTATEMENT :
                //<DoStatement>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EMPTYSTATEMENT :
                //<EmptyStatement>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EQUALITYEXPRESSION :
                //<EqualityExpression>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EXCLUSIVEOREXPRESSION :
                //<ExclusiveOrExpression>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EXPLICITCONSTRUCTORINVOCATION :
                //<ExplicitConstructorInvocation>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EXPRESSION :
                //<Expression>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EXPRESSIONSTATEMENT :
                //<ExpressionStatement>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EXTENDSINTERFACES :
                //<ExtendsInterfaces>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FIELDACCESS :
                //<FieldAccess>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FIELDDECLARATION :
                //<FieldDeclaration>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FINALLY2 :
                //<Finally>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FLOATINGPOINTTYPE :
                //<FloatingPointType>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FLOATPOINTLITERAL :
                //<FloatPointLiteral>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FORINIT :
                //<ForInit>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FORMALPARAMETER :
                //<FormalParameter>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FORMALPARAMETERLIST :
                //<FormalParameterList>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FORSTATEMENT :
                //<ForStatement>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FORSTATEMENTNOSHORTIF :
                //<ForStatementNoShortIf>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FORUPDATE :
                //<ForUpdate>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_IFTHENELSESTATEMENT :
                //<IfThenElseStatement>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_IFTHENELSESTATEMENTNOSHORTIF :
                //<IfThenElseStatementNoShortIf>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_IFTHENSTATEMENT :
                //<IfThenStatement>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_IMPORTDECLARATION :
                //<ImportDeclaration>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_IMPORTDECLARATIONS :
                //<ImportDeclarations>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_INCLUSIVEOREXPRESSION :
                //<InclusiveOrExpression>
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

                case (int)SymbolConstants.SYMBOL_INTERFACEBODY :
                //<InterfaceBody>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_INTERFACEDECLARATION :
                //<InterfaceDeclaration>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_INTERFACEMEMBERDECLARATION :
                //<InterfaceMemberDeclaration>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_INTERFACEMEMBERDECLARATIONS :
                //<InterfaceMemberDeclarations>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_INTERFACES :
                //<Interfaces>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_INTERFACETYPE :
                //<InterfaceType>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_INTERFACETYPELIST :
                //<InterfaceTypeList>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LABELEDSTATEMENT :
                //<LabeledStatement>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LABELEDSTATEMENTNOSHORTIF :
                //<LabeledStatementNoShortIf>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LEFTHANDSIDE :
                //<LeftHandSide>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LITERAL :
                //<Literal>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LOCALVARIABLEDECLARATION :
                //<LocalVariableDeclaration>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LOCALVARIABLEDECLARATIONSTATEMENT :
                //<LocalVariableDeclarationStatement>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_METHODBODY :
                //<MethodBody>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_METHODDECLARATION :
                //<MethodDeclaration>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_METHODDECLARATOR :
                //<MethodDeclarator>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_METHODHEADER :
                //<MethodHeader>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_METHODINVOCATION :
                //<MethodInvocation>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_MODIFIER :
                //<Modifier>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_MODIFIERS :
                //<Modifiers>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_MULTIPLICATIVEEXPRESSION :
                //<MultiplicativeExpression>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_NAME :
                //<Name>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_NUMERICTYPE :
                //<NumericType>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PACKAGEDECLARATION :
                //<PackageDeclaration>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_POSTDECREMENTEXPRESSION :
                //<PostDecrementExpression>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_POSTFIXEXPRESSION :
                //<PostfixExpression>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_POSTINCREMENTEXPRESSION :
                //<PostIncrementExpression>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PREDECREMENTEXPRESSION :
                //<PreDecrementExpression>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PREINCREMENTEXPRESSION :
                //<PreIncrementExpression>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PRIMARY :
                //<Primary>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PRIMARYNONEWARRAY :
                //<PrimaryNoNewArray>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PRIMITIVETYPE :
                //<PrimitiveType>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_QUALIFIEDNAME :
                //<QualifiedName>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_REFERENCETYPE :
                //<ReferenceType>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_RELATIONALEXPRESSION :
                //<RelationalExpression>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_RETURNSTATEMENT :
                //<ReturnStatement>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_SHIFTEXPRESSION :
                //<ShiftExpression>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_SIMPLENAME :
                //<SimpleName>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_SINGLETYPEIMPORTDECLARATION :
                //<SingleTypeImportDeclaration>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_STATEMENT :
                //<Statement>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_STATEMENTEXPRESSION :
                //<StatementExpression>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_STATEMENTEXPRESSIONLIST :
                //<StatementExpressionList>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_STATEMENTNOSHORTIF :
                //<StatementNoShortIf>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_STATEMENTWITHOUTTRAILINGSUBSTATEMENT :
                //<StatementWithoutTrailingSubstatement>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_STATICINITIALIZER :
                //<StaticInitializer>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_SUPER2 :
                //<Super>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_SWITCHBLOCK :
                //<SwitchBlock>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_SWITCHBLOCKSTATEMENTGROUP :
                //<SwitchBlockStatementGroup>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_SWITCHBLOCKSTATEMENTGROUPS :
                //<SwitchBlockStatementGroups>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_SWITCHLABEL :
                //<SwitchLabel>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_SWITCHLABELS :
                //<SwitchLabels>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_SWITCHSTATEMENT :
                //<SwitchStatement>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_SYNCHRONIZEDSTATEMENT :
                //<SynchronizedStatement>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_THROWS2 :
                //<Throws>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_THROWSTATEMENT :
                //<ThrowStatement>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_TRYSTATEMENT :
                //<TryStatement>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_TYPE :
                //<Type>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_TYPEDECLARATION :
                //<TypeDeclaration>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_TYPEDECLARATIONS :
                //<TypeDeclarations>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_TYPEIMPORTONDEMANDDECLARATION :
                //<TypeImportOnDemandDeclaration>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_UNARYEXPRESSION :
                //<UnaryExpression>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_UNARYEXPRESSIONNOTPLUSMINUS :
                //<UnaryExpressionNotPlusMinus>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_VARIABLEDECLARATOR :
                //<VariableDeclarator>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_VARIABLEDECLARATORID :
                //<VariableDeclaratorId>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_VARIABLEDECLARATORS :
                //<VariableDeclarators>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_VARIABLEINITIALIZER :
                //<VariableInitializer>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_VARIABLEINITIALIZERS :
                //<VariableInitializers>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_WHILESTATEMENT :
                //<WhileStatement>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_WHILESTATEMENTNOSHORTIF :
                //<WhileStatementNoShortIf>
                //todo: Create a new object that corresponds to the symbol
                return token.UserObject;

            }
            throw new SymbolException("Unknown symbol");
        }

        public Object CreateObjectFromNonterminal(NonterminalToken token)
        {
            switch (token.Rule.Id)
            {
                case (int)RuleConstants.RULE_CHARACTERLITERAL_INDIRECTCHARLITERAL :
                //<CharacterLiteral> ::= IndirectCharLiteral
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CHARACTERLITERAL_STANDARDESCAPECHARLITERAL :
                //<CharacterLiteral> ::= StandardEscapeCharLiteral
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CHARACTERLITERAL_OCTALESCAPECHARLITERAL :
                //<CharacterLiteral> ::= OctalEscapeCharLiteral
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CHARACTERLITERAL_HEXESCAPECHARLITERAL :
                //<CharacterLiteral> ::= HexEscapeCharLiteral
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_DECIMALINTEGERLITERAL_STARTWITHZERODECIMALINTEGERLITERAL :
                //<DecimalIntegerLiteral> ::= StartWithZeroDecimalIntegerLiteral
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_DECIMALINTEGERLITERAL_STARTWITHNOZERODECIMALINTEGERLITERAL :
                //<DecimalIntegerLiteral> ::= StartWithNoZeroDecimalIntegerLiteral
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FLOATPOINTLITERAL_FLOATINGPOINTLITERAL :
                //<FloatPointLiteral> ::= FloatingPointLiteral
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FLOATPOINTLITERAL_FLOATINGPOINTLITERALEXPONENT :
                //<FloatPointLiteral> ::= FloatingPointLiteralExponent
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_INTEGERLITERAL :
                //<IntegerLiteral> ::= <DecimalIntegerLiteral>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_INTEGERLITERAL_HEXINTEGERLITERAL :
                //<IntegerLiteral> ::= HexIntegerLiteral
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_INTEGERLITERAL_OCTALINTEGERLITERAL :
                //<IntegerLiteral> ::= OctalIntegerLiteral
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_LITERAL :
                //<Literal> ::= <IntegerLiteral>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_LITERAL2 :
                //<Literal> ::= <FloatPointLiteral>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_LITERAL_BOOLEANLITERAL :
                //<Literal> ::= BooleanLiteral
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_LITERAL3 :
                //<Literal> ::= <CharacterLiteral>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_LITERAL_STRINGLITERAL :
                //<Literal> ::= StringLiteral
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_LITERAL_NULLLITERAL :
                //<Literal> ::= NullLiteral
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_TYPE :
                //<Type> ::= <PrimitiveType>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_TYPE2 :
                //<Type> ::= <ReferenceType>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_PRIMITIVETYPE :
                //<PrimitiveType> ::= <NumericType>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_PRIMITIVETYPE_BOOLEAN :
                //<PrimitiveType> ::= boolean
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_NUMERICTYPE :
                //<NumericType> ::= <IntegralType>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_NUMERICTYPE2 :
                //<NumericType> ::= <FloatingPointType>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_INTEGRALTYPE_BYTE :
                //<IntegralType> ::= byte
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_INTEGRALTYPE_SHORT :
                //<IntegralType> ::= short
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_INTEGRALTYPE_INT :
                //<IntegralType> ::= int
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_INTEGRALTYPE_LONG :
                //<IntegralType> ::= long
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_INTEGRALTYPE_CHAR :
                //<IntegralType> ::= char
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FLOATINGPOINTTYPE_FLOAT :
                //<FloatingPointType> ::= float
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FLOATINGPOINTTYPE_DOUBLE :
                //<FloatingPointType> ::= double
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_REFERENCETYPE :
                //<ReferenceType> ::= <ClassOrInterfaceType>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_REFERENCETYPE2 :
                //<ReferenceType> ::= <ArrayType>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CLASSORINTERFACETYPE :
                //<ClassOrInterfaceType> ::= <Name>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CLASSTYPE :
                //<ClassType> ::= <ClassOrInterfaceType>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_INTERFACETYPE :
                //<InterfaceType> ::= <ClassOrInterfaceType>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ARRAYTYPE_LBRACKET_RBRACKET :
                //<ArrayType> ::= <PrimitiveType> '[' ']'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ARRAYTYPE_LBRACKET_RBRACKET2 :
                //<ArrayType> ::= <Name> '[' ']'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ARRAYTYPE_LBRACKET_RBRACKET3 :
                //<ArrayType> ::= <ArrayType> '[' ']'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_NAME :
                //<Name> ::= <SimpleName>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_NAME2 :
                //<Name> ::= <QualifiedName>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_SIMPLENAME_IDENTIFIER :
                //<SimpleName> ::= Identifier
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_QUALIFIEDNAME_DOT_IDENTIFIER :
                //<QualifiedName> ::= <Name> '.' Identifier
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_COMPILATIONUNIT :
                //<CompilationUnit> ::= <PackageDeclaration> <ImportDeclarations> <TypeDeclarations>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_COMPILATIONUNIT2 :
                //<CompilationUnit> ::= <PackageDeclaration> <ImportDeclarations>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_COMPILATIONUNIT3 :
                //<CompilationUnit> ::= <PackageDeclaration> <TypeDeclarations>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_COMPILATIONUNIT4 :
                //<CompilationUnit> ::= <PackageDeclaration>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_COMPILATIONUNIT5 :
                //<CompilationUnit> ::= <ImportDeclarations> <TypeDeclarations>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_COMPILATIONUNIT6 :
                //<CompilationUnit> ::= <ImportDeclarations>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_COMPILATIONUNIT7 :
                //<CompilationUnit> ::= <TypeDeclarations>
                //todo: Create a new object using the stored tokens.
                return token.Tokens[0];

                case (int)RuleConstants.RULE_COMPILATIONUNIT8 :
                //<CompilationUnit> ::= 
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_IMPORTDECLARATIONS :
                //<ImportDeclarations> ::= <ImportDeclaration>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_IMPORTDECLARATIONS2 :
                //<ImportDeclarations> ::= <ImportDeclarations> <ImportDeclaration>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_TYPEDECLARATIONS :
                //<TypeDeclarations> ::= <TypeDeclaration>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_TYPEDECLARATIONS2 :
                //<TypeDeclarations> ::= <TypeDeclarations> <TypeDeclaration>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_PACKAGEDECLARATION_PACKAGE_SEMI :
                //<PackageDeclaration> ::= package <Name> ';'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_IMPORTDECLARATION :
                //<ImportDeclaration> ::= <SingleTypeImportDeclaration>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_IMPORTDECLARATION2 :
                //<ImportDeclaration> ::= <TypeImportOnDemandDeclaration>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_SINGLETYPEIMPORTDECLARATION_IMPORT_SEMI :
                //<SingleTypeImportDeclaration> ::= import <Name> ';'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_TYPEIMPORTONDEMANDDECLARATION_IMPORT_DOT_TIMES_SEMI :
                //<TypeImportOnDemandDeclaration> ::= import <Name> '.' '*' ';'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_TYPEDECLARATION :
                //<TypeDeclaration> ::= <ClassDeclaration>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_TYPEDECLARATION2 :
                //<TypeDeclaration> ::= <InterfaceDeclaration>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_TYPEDECLARATION_SEMI :
                //<TypeDeclaration> ::= ';'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_MODIFIERS :
                //<Modifiers> ::= <Modifier>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_MODIFIERS2 :
                //<Modifiers> ::= <Modifiers> <Modifier>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_MODIFIER_PUBLIC :
                //<Modifier> ::= public
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_MODIFIER_PROTECTED :
                //<Modifier> ::= protected
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_MODIFIER_PRIVATE :
                //<Modifier> ::= private
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_MODIFIER_STATIC :
                //<Modifier> ::= static
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_MODIFIER_ABSTRACT :
                //<Modifier> ::= abstract
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_MODIFIER_FINAL :
                //<Modifier> ::= final
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_MODIFIER_NATIVE :
                //<Modifier> ::= native
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_MODIFIER_SYNCHRONIZED :
                //<Modifier> ::= synchronized
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_MODIFIER_TRANSIENT :
                //<Modifier> ::= transient
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_MODIFIER_VOLATILE :
                //<Modifier> ::= volatile
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CLASSDECLARATION_CLASS_IDENTIFIER :
                //<ClassDeclaration> ::= <Modifiers> class Identifier <Super> <Interfaces> <ClassBody>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CLASSDECLARATION_CLASS_IDENTIFIER2 :
                //<ClassDeclaration> ::= <Modifiers> class Identifier <Super> <ClassBody>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CLASSDECLARATION_CLASS_IDENTIFIER3 :
                //<ClassDeclaration> ::= <Modifiers> class Identifier <Interfaces> <ClassBody>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CLASSDECLARATION_CLASS_IDENTIFIER4 :
                //<ClassDeclaration> ::= <Modifiers> class Identifier <ClassBody>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CLASSDECLARATION_CLASS_IDENTIFIER5 :
                //<ClassDeclaration> ::= class Identifier <Super> <Interfaces> <ClassBody>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CLASSDECLARATION_CLASS_IDENTIFIER6 :
                //<ClassDeclaration> ::= class Identifier <Super> <ClassBody>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CLASSDECLARATION_CLASS_IDENTIFIER7 :
                //<ClassDeclaration> ::= class Identifier <Interfaces> <ClassBody>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CLASSDECLARATION_CLASS_IDENTIFIER8 :
                //<ClassDeclaration> ::= class Identifier <ClassBody>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_SUPER_EXTENDS :
                //<Super> ::= extends <ClassType>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_INTERFACES_IMPLEMENTS :
                //<Interfaces> ::= implements <InterfaceTypeList>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_INTERFACETYPELIST :
                //<InterfaceTypeList> ::= <InterfaceType>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_INTERFACETYPELIST_COMMA :
                //<InterfaceTypeList> ::= <InterfaceTypeList> ',' <InterfaceType>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CLASSBODY_LBRACE_RBRACE :
                //<ClassBody> ::= '{' <ClassBodyDeclarations> '}'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CLASSBODY_LBRACE_RBRACE2 :
                //<ClassBody> ::= '{' '}'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CLASSBODYDECLARATIONS :
                //<ClassBodyDeclarations> ::= <ClassBodyDeclaration>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CLASSBODYDECLARATIONS2 :
                //<ClassBodyDeclarations> ::= <ClassBodyDeclarations> <ClassBodyDeclaration>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CLASSBODYDECLARATION :
                //<ClassBodyDeclaration> ::= <ClassMemberDeclaration>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CLASSBODYDECLARATION2 :
                //<ClassBodyDeclaration> ::= <StaticInitializer>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CLASSBODYDECLARATION3 :
                //<ClassBodyDeclaration> ::= <ConstructorDeclaration>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CLASSMEMBERDECLARATION :
                //<ClassMemberDeclaration> ::= <FieldDeclaration>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CLASSMEMBERDECLARATION2 :
                //<ClassMemberDeclaration> ::= <MethodDeclaration>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FIELDDECLARATION_SEMI :
                //<FieldDeclaration> ::= <Modifiers> <Type> <VariableDeclarators> ';'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FIELDDECLARATION_SEMI2 :
                //<FieldDeclaration> ::= <Type> <VariableDeclarators> ';'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_VARIABLEDECLARATORS :
                //<VariableDeclarators> ::= <VariableDeclarator>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_VARIABLEDECLARATORS_COMMA :
                //<VariableDeclarators> ::= <VariableDeclarators> ',' <VariableDeclarator>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_VARIABLEDECLARATOR :
                //<VariableDeclarator> ::= <VariableDeclaratorId>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_VARIABLEDECLARATOR_EQ :
                //<VariableDeclarator> ::= <VariableDeclaratorId> '=' <VariableInitializer>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_VARIABLEDECLARATORID_IDENTIFIER :
                //<VariableDeclaratorId> ::= Identifier
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_VARIABLEDECLARATORID_LBRACKET_RBRACKET :
                //<VariableDeclaratorId> ::= <VariableDeclaratorId> '[' ']'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_VARIABLEINITIALIZER :
                //<VariableInitializer> ::= <Expression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_VARIABLEINITIALIZER2 :
                //<VariableInitializer> ::= <ArrayInitializer>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_METHODDECLARATION :
                //<MethodDeclaration> ::= <MethodHeader> <MethodBody>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_METHODHEADER :
                //<MethodHeader> ::= <Modifiers> <Type> <MethodDeclarator> <Throws>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_METHODHEADER2 :
                //<MethodHeader> ::= <Modifiers> <Type> <MethodDeclarator>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_METHODHEADER3 :
                //<MethodHeader> ::= <Type> <MethodDeclarator> <Throws>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_METHODHEADER4 :
                //<MethodHeader> ::= <Type> <MethodDeclarator>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_METHODHEADER_VOID :
                //<MethodHeader> ::= <Modifiers> void <MethodDeclarator> <Throws>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_METHODHEADER_VOID2 :
                //<MethodHeader> ::= <Modifiers> void <MethodDeclarator>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_METHODHEADER_VOID3 :
                //<MethodHeader> ::= void <MethodDeclarator> <Throws>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_METHODHEADER_VOID4 :
                //<MethodHeader> ::= void <MethodDeclarator>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_METHODDECLARATOR_IDENTIFIER_LPAREN_RPAREN :
                //<MethodDeclarator> ::= Identifier '(' <FormalParameterList> ')'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_METHODDECLARATOR_IDENTIFIER_LPAREN_RPAREN2 :
                //<MethodDeclarator> ::= Identifier '(' ')'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_METHODDECLARATOR_LBRACKET_RBRACKET :
                //<MethodDeclarator> ::= <MethodDeclarator> '[' ']'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FORMALPARAMETERLIST :
                //<FormalParameterList> ::= <FormalParameter>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FORMALPARAMETERLIST_COMMA :
                //<FormalParameterList> ::= <FormalParameterList> ',' <FormalParameter>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FORMALPARAMETER :
                //<FormalParameter> ::= <Type> <VariableDeclaratorId>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_THROWS_THROWS :
                //<Throws> ::= throws <ClassTypeList>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CLASSTYPELIST :
                //<ClassTypeList> ::= <ClassType>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CLASSTYPELIST_COMMA :
                //<ClassTypeList> ::= <ClassTypeList> ',' <ClassType>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_METHODBODY :
                //<MethodBody> ::= <Block>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_METHODBODY_SEMI :
                //<MethodBody> ::= ';'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATICINITIALIZER_STATIC :
                //<StaticInitializer> ::= static <Block>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CONSTRUCTORDECLARATION :
                //<ConstructorDeclaration> ::= <Modifiers> <ConstructorDeclarator> <Throws> <ConstructorBody>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CONSTRUCTORDECLARATION2 :
                //<ConstructorDeclaration> ::= <Modifiers> <ConstructorDeclarator> <ConstructorBody>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CONSTRUCTORDECLARATION3 :
                //<ConstructorDeclaration> ::= <ConstructorDeclarator> <Throws> <ConstructorBody>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CONSTRUCTORDECLARATION4 :
                //<ConstructorDeclaration> ::= <ConstructorDeclarator> <ConstructorBody>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CONSTRUCTORDECLARATOR_LPAREN_RPAREN :
                //<ConstructorDeclarator> ::= <SimpleName> '(' <FormalParameterList> ')'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CONSTRUCTORDECLARATOR_LPAREN_RPAREN2 :
                //<ConstructorDeclarator> ::= <SimpleName> '(' ')'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CONSTRUCTORBODY_LBRACE_RBRACE :
                //<ConstructorBody> ::= '{' <ExplicitConstructorInvocation> <BlockStatements> '}'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CONSTRUCTORBODY_LBRACE_RBRACE2 :
                //<ConstructorBody> ::= '{' <ExplicitConstructorInvocation> '}'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CONSTRUCTORBODY_LBRACE_RBRACE3 :
                //<ConstructorBody> ::= '{' <BlockStatements> '}'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CONSTRUCTORBODY_LBRACE_RBRACE4 :
                //<ConstructorBody> ::= '{' '}'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPLICITCONSTRUCTORINVOCATION_THIS_LPAREN_RPAREN_SEMI :
                //<ExplicitConstructorInvocation> ::= this '(' <ArgumentList> ')' ';'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPLICITCONSTRUCTORINVOCATION_THIS_LPAREN_RPAREN_SEMI2 :
                //<ExplicitConstructorInvocation> ::= this '(' ')' ';'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPLICITCONSTRUCTORINVOCATION_SUPER_LPAREN_RPAREN_SEMI :
                //<ExplicitConstructorInvocation> ::= super '(' <ArgumentList> ')' ';'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPLICITCONSTRUCTORINVOCATION_SUPER_LPAREN_RPAREN_SEMI2 :
                //<ExplicitConstructorInvocation> ::= super '(' ')' ';'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_INTERFACEDECLARATION_INTERFACE_IDENTIFIER :
                //<InterfaceDeclaration> ::= <Modifiers> interface Identifier <ExtendsInterfaces> <InterfaceBody>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_INTERFACEDECLARATION_INTERFACE_IDENTIFIER2 :
                //<InterfaceDeclaration> ::= <Modifiers> interface Identifier <InterfaceBody>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_INTERFACEDECLARATION_INTERFACE_IDENTIFIER3 :
                //<InterfaceDeclaration> ::= interface Identifier <ExtendsInterfaces> <InterfaceBody>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_INTERFACEDECLARATION_INTERFACE_IDENTIFIER4 :
                //<InterfaceDeclaration> ::= interface Identifier <InterfaceBody>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXTENDSINTERFACES_EXTENDS :
                //<ExtendsInterfaces> ::= extends <InterfaceType>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXTENDSINTERFACES_COMMA :
                //<ExtendsInterfaces> ::= <ExtendsInterfaces> ',' <InterfaceType>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_INTERFACEBODY_LBRACE_RBRACE :
                //<InterfaceBody> ::= '{' <InterfaceMemberDeclarations> '}'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_INTERFACEBODY_LBRACE_RBRACE2 :
                //<InterfaceBody> ::= '{' '}'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_INTERFACEMEMBERDECLARATIONS :
                //<InterfaceMemberDeclarations> ::= <InterfaceMemberDeclaration>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_INTERFACEMEMBERDECLARATIONS2 :
                //<InterfaceMemberDeclarations> ::= <InterfaceMemberDeclarations> <InterfaceMemberDeclaration>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_INTERFACEMEMBERDECLARATION :
                //<InterfaceMemberDeclaration> ::= <ConstantDeclaration>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_INTERFACEMEMBERDECLARATION2 :
                //<InterfaceMemberDeclaration> ::= <AbstractMethodDeclaration>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CONSTANTDECLARATION :
                //<ConstantDeclaration> ::= <FieldDeclaration>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ABSTRACTMETHODDECLARATION_SEMI :
                //<AbstractMethodDeclaration> ::= <MethodHeader> ';'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ARRAYINITIALIZER_LBRACE_COMMA_RBRACE :
                //<ArrayInitializer> ::= '{' <VariableInitializers> ',' '}'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ARRAYINITIALIZER_LBRACE_RBRACE :
                //<ArrayInitializer> ::= '{' <VariableInitializers> '}'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ARRAYINITIALIZER_LBRACE_COMMA_RBRACE2 :
                //<ArrayInitializer> ::= '{' ',' '}'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ARRAYINITIALIZER_LBRACE_RBRACE2 :
                //<ArrayInitializer> ::= '{' '}'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_VARIABLEINITIALIZERS :
                //<VariableInitializers> ::= <VariableInitializer>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_VARIABLEINITIALIZERS_COMMA :
                //<VariableInitializers> ::= <VariableInitializers> ',' <VariableInitializer>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_BLOCK_LBRACE_RBRACE :
                //<Block> ::= '{' <BlockStatements> '}'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_BLOCK_LBRACE_RBRACE2 :
                //<Block> ::= '{' '}'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_BLOCKSTATEMENTS :
                //<BlockStatements> ::= <BlockStatement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_BLOCKSTATEMENTS2 :
                //<BlockStatements> ::= <BlockStatements> <BlockStatement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_BLOCKSTATEMENT :
                //<BlockStatement> ::= <LocalVariableDeclarationStatement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_BLOCKSTATEMENT2 :
                //<BlockStatement> ::= <Statement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_LOCALVARIABLEDECLARATIONSTATEMENT_SEMI :
                //<LocalVariableDeclarationStatement> ::= <LocalVariableDeclaration> ';'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_LOCALVARIABLEDECLARATION :
                //<LocalVariableDeclaration> ::= <Type> <VariableDeclarators>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENT :
                //<Statement> ::= <StatementWithoutTrailingSubstatement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENT2 :
                //<Statement> ::= <LabeledStatement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENT3 :
                //<Statement> ::= <IfThenStatement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENT4 :
                //<Statement> ::= <IfThenElseStatement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENT5 :
                //<Statement> ::= <WhileStatement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENT6 :
                //<Statement> ::= <ForStatement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENTNOSHORTIF :
                //<StatementNoShortIf> ::= <StatementWithoutTrailingSubstatement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENTNOSHORTIF2 :
                //<StatementNoShortIf> ::= <LabeledStatementNoShortIf>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENTNOSHORTIF3 :
                //<StatementNoShortIf> ::= <IfThenElseStatementNoShortIf>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENTNOSHORTIF4 :
                //<StatementNoShortIf> ::= <WhileStatementNoShortIf>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENTNOSHORTIF5 :
                //<StatementNoShortIf> ::= <ForStatementNoShortIf>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENTWITHOUTTRAILINGSUBSTATEMENT :
                //<StatementWithoutTrailingSubstatement> ::= <Block>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENTWITHOUTTRAILINGSUBSTATEMENT2 :
                //<StatementWithoutTrailingSubstatement> ::= <EmptyStatement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENTWITHOUTTRAILINGSUBSTATEMENT3 :
                //<StatementWithoutTrailingSubstatement> ::= <ExpressionStatement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENTWITHOUTTRAILINGSUBSTATEMENT4 :
                //<StatementWithoutTrailingSubstatement> ::= <SwitchStatement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENTWITHOUTTRAILINGSUBSTATEMENT5 :
                //<StatementWithoutTrailingSubstatement> ::= <DoStatement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENTWITHOUTTRAILINGSUBSTATEMENT6 :
                //<StatementWithoutTrailingSubstatement> ::= <BreakStatement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENTWITHOUTTRAILINGSUBSTATEMENT7 :
                //<StatementWithoutTrailingSubstatement> ::= <ContinueStatement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENTWITHOUTTRAILINGSUBSTATEMENT8 :
                //<StatementWithoutTrailingSubstatement> ::= <ReturnStatement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENTWITHOUTTRAILINGSUBSTATEMENT9 :
                //<StatementWithoutTrailingSubstatement> ::= <SynchronizedStatement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENTWITHOUTTRAILINGSUBSTATEMENT10 :
                //<StatementWithoutTrailingSubstatement> ::= <ThrowStatement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENTWITHOUTTRAILINGSUBSTATEMENT11 :
                //<StatementWithoutTrailingSubstatement> ::= <TryStatement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EMPTYSTATEMENT_SEMI :
                //<EmptyStatement> ::= ';'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_LABELEDSTATEMENT_IDENTIFIER_COLON :
                //<LabeledStatement> ::= Identifier ':' <Statement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_LABELEDSTATEMENTNOSHORTIF_IDENTIFIER_COLON :
                //<LabeledStatementNoShortIf> ::= Identifier ':' <StatementNoShortIf>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPRESSIONSTATEMENT_SEMI :
                //<ExpressionStatement> ::= <StatementExpression> ';'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENTEXPRESSION :
                //<StatementExpression> ::= <Assignment>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENTEXPRESSION2 :
                //<StatementExpression> ::= <PreIncrementExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENTEXPRESSION3 :
                //<StatementExpression> ::= <PreDecrementExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENTEXPRESSION4 :
                //<StatementExpression> ::= <PostIncrementExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENTEXPRESSION5 :
                //<StatementExpression> ::= <PostDecrementExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENTEXPRESSION6 :
                //<StatementExpression> ::= <MethodInvocation>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENTEXPRESSION7 :
                //<StatementExpression> ::= <ClassInstanceCreationExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_IFTHENSTATEMENT_IF_LPAREN_RPAREN :
                //<IfThenStatement> ::= if '(' <Expression> ')' <Statement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_IFTHENELSESTATEMENT_IF_LPAREN_RPAREN_ELSE :
                //<IfThenElseStatement> ::= if '(' <Expression> ')' <StatementNoShortIf> else <Statement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_IFTHENELSESTATEMENTNOSHORTIF_IF_LPAREN_RPAREN_ELSE :
                //<IfThenElseStatementNoShortIf> ::= if '(' <Expression> ')' <StatementNoShortIf> else <StatementNoShortIf>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_SWITCHSTATEMENT_SWITCH_LPAREN_RPAREN :
                //<SwitchStatement> ::= switch '(' <Expression> ')' <SwitchBlock>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_SWITCHBLOCK_LBRACE_RBRACE :
                //<SwitchBlock> ::= '{' <SwitchBlockStatementGroups> <SwitchLabels> '}'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_SWITCHBLOCK_LBRACE_RBRACE2 :
                //<SwitchBlock> ::= '{' <SwitchBlockStatementGroups> '}'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_SWITCHBLOCK_LBRACE_RBRACE3 :
                //<SwitchBlock> ::= '{' <SwitchLabels> '}'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_SWITCHBLOCK_LBRACE_RBRACE4 :
                //<SwitchBlock> ::= '{' '}'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_SWITCHBLOCKSTATEMENTGROUPS :
                //<SwitchBlockStatementGroups> ::= <SwitchBlockStatementGroup>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_SWITCHBLOCKSTATEMENTGROUPS2 :
                //<SwitchBlockStatementGroups> ::= <SwitchBlockStatementGroups> <SwitchBlockStatementGroup>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_SWITCHBLOCKSTATEMENTGROUP :
                //<SwitchBlockStatementGroup> ::= <SwitchLabels> <BlockStatements>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_SWITCHLABELS :
                //<SwitchLabels> ::= <SwitchLabel>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_SWITCHLABELS2 :
                //<SwitchLabels> ::= <SwitchLabels> <SwitchLabel>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_SWITCHLABEL_CASE_COLON :
                //<SwitchLabel> ::= case <ConstantExpression> ':'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_SWITCHLABEL_DEFAULT_COLON :
                //<SwitchLabel> ::= default ':'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_WHILESTATEMENT_WHILE_LPAREN_RPAREN :
                //<WhileStatement> ::= while '(' <Expression> ')' <Statement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_WHILESTATEMENTNOSHORTIF_WHILE_LPAREN_RPAREN :
                //<WhileStatementNoShortIf> ::= while '(' <Expression> ')' <StatementNoShortIf>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_DOSTATEMENT_DO_WHILE_LPAREN_RPAREN_SEMI :
                //<DoStatement> ::= do <Statement> while '(' <Expression> ')' ';'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FORSTATEMENT_FOR_LPAREN_SEMI_SEMI_RPAREN :
                //<ForStatement> ::= for '(' <ForInit> ';' <Expression> ';' <ForUpdate> ')' <Statement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FORSTATEMENT_FOR_LPAREN_SEMI_SEMI_RPAREN2 :
                //<ForStatement> ::= for '(' <ForInit> ';' <Expression> ';' ')' <Statement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FORSTATEMENT_FOR_LPAREN_SEMI_SEMI_RPAREN3 :
                //<ForStatement> ::= for '(' <ForInit> ';' ';' <ForUpdate> ')' <Statement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FORSTATEMENT_FOR_LPAREN_SEMI_SEMI_RPAREN4 :
                //<ForStatement> ::= for '(' <ForInit> ';' ';' ')' <Statement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FORSTATEMENT_FOR_LPAREN_SEMI_SEMI_RPAREN5 :
                //<ForStatement> ::= for '(' ';' <Expression> ';' <ForUpdate> ')' <Statement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FORSTATEMENT_FOR_LPAREN_SEMI_SEMI_RPAREN6 :
                //<ForStatement> ::= for '(' ';' <Expression> ';' ')' <Statement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FORSTATEMENT_FOR_LPAREN_SEMI_SEMI_RPAREN7 :
                //<ForStatement> ::= for '(' ';' ';' <ForUpdate> ')' <Statement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FORSTATEMENT_FOR_LPAREN_SEMI_SEMI_RPAREN8 :
                //<ForStatement> ::= for '(' ';' ';' ')' <Statement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FORSTATEMENTNOSHORTIF_FOR_LPAREN_SEMI_SEMI_RPAREN :
                //<ForStatementNoShortIf> ::= for '(' <ForInit> ';' <Expression> ';' <ForUpdate> ')' <StatementNoShortIf>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FORSTATEMENTNOSHORTIF_FOR_LPAREN_SEMI_SEMI_RPAREN2 :
                //<ForStatementNoShortIf> ::= for '(' <ForInit> ';' <Expression> ';' ')' <StatementNoShortIf>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FORSTATEMENTNOSHORTIF_FOR_LPAREN_SEMI_SEMI_RPAREN3 :
                //<ForStatementNoShortIf> ::= for '(' <ForInit> ';' ';' <ForUpdate> ')' <StatementNoShortIf>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FORSTATEMENTNOSHORTIF_FOR_LPAREN_SEMI_SEMI_RPAREN4 :
                //<ForStatementNoShortIf> ::= for '(' <ForInit> ';' ';' ')' <StatementNoShortIf>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FORSTATEMENTNOSHORTIF_FOR_LPAREN_SEMI_SEMI_RPAREN5 :
                //<ForStatementNoShortIf> ::= for '(' ';' <Expression> ';' <ForUpdate> ')' <StatementNoShortIf>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FORSTATEMENTNOSHORTIF_FOR_LPAREN_SEMI_SEMI_RPAREN6 :
                //<ForStatementNoShortIf> ::= for '(' ';' <Expression> ';' ')' <StatementNoShortIf>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FORSTATEMENTNOSHORTIF_FOR_LPAREN_SEMI_SEMI_RPAREN7 :
                //<ForStatementNoShortIf> ::= for '(' ';' ';' <ForUpdate> ')' <StatementNoShortIf>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FORSTATEMENTNOSHORTIF_FOR_LPAREN_SEMI_SEMI_RPAREN8 :
                //<ForStatementNoShortIf> ::= for '(' ';' ';' ')' <StatementNoShortIf>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FORINIT :
                //<ForInit> ::= <StatementExpressionList>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FORINIT2 :
                //<ForInit> ::= <LocalVariableDeclaration>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FORUPDATE :
                //<ForUpdate> ::= <StatementExpressionList>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENTEXPRESSIONLIST :
                //<StatementExpressionList> ::= <StatementExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENTEXPRESSIONLIST_COMMA :
                //<StatementExpressionList> ::= <StatementExpressionList> ',' <StatementExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_BREAKSTATEMENT_BREAK_IDENTIFIER_SEMI :
                //<BreakStatement> ::= break Identifier ';'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_BREAKSTATEMENT_BREAK_SEMI :
                //<BreakStatement> ::= break ';'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CONTINUESTATEMENT_CONTINUE_IDENTIFIER_SEMI :
                //<ContinueStatement> ::= continue Identifier ';'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CONTINUESTATEMENT_CONTINUE_SEMI :
                //<ContinueStatement> ::= continue ';'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_RETURNSTATEMENT_RETURN_SEMI :
                //<ReturnStatement> ::= return <Expression> ';'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_RETURNSTATEMENT_RETURN_SEMI2 :
                //<ReturnStatement> ::= return ';'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_THROWSTATEMENT_THROW_SEMI :
                //<ThrowStatement> ::= throw <Expression> ';'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_SYNCHRONIZEDSTATEMENT_SYNCHRONIZED_LPAREN_RPAREN :
                //<SynchronizedStatement> ::= synchronized '(' <Expression> ')' <Block>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_TRYSTATEMENT_TRY :
                //<TryStatement> ::= try <Block> <Catches>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_TRYSTATEMENT_TRY2 :
                //<TryStatement> ::= try <Block> <Catches> <Finally>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_TRYSTATEMENT_TRY3 :
                //<TryStatement> ::= try <Block> <Finally>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CATCHES :
                //<Catches> ::= <CatchClause>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CATCHES2 :
                //<Catches> ::= <Catches> <CatchClause>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CATCHCLAUSE_CATCH_LPAREN_RPAREN :
                //<CatchClause> ::= catch '(' <FormalParameter> ')' <Block>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FINALLY_FINALLY :
                //<Finally> ::= finally <Block>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_PRIMARY :
                //<Primary> ::= <PrimaryNoNewArray>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_PRIMARY2 :
                //<Primary> ::= <ArrayCreationExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_PRIMARYNONEWARRAY :
                //<PrimaryNoNewArray> ::= <Literal>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_PRIMARYNONEWARRAY_THIS :
                //<PrimaryNoNewArray> ::= this
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_PRIMARYNONEWARRAY_LPAREN_RPAREN :
                //<PrimaryNoNewArray> ::= '(' <Expression> ')'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_PRIMARYNONEWARRAY2 :
                //<PrimaryNoNewArray> ::= <ClassInstanceCreationExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_PRIMARYNONEWARRAY3 :
                //<PrimaryNoNewArray> ::= <FieldAccess>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_PRIMARYNONEWARRAY4 :
                //<PrimaryNoNewArray> ::= <MethodInvocation>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_PRIMARYNONEWARRAY5 :
                //<PrimaryNoNewArray> ::= <ArrayAccess>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CLASSINSTANCECREATIONEXPRESSION_NEW_LPAREN_RPAREN :
                //<ClassInstanceCreationExpression> ::= new <ClassType> '(' <ArgumentList> ')'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CLASSINSTANCECREATIONEXPRESSION_NEW_LPAREN_RPAREN2 :
                //<ClassInstanceCreationExpression> ::= new <ClassType> '(' ')'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ARGUMENTLIST :
                //<ArgumentList> ::= <Expression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ARGUMENTLIST_COMMA :
                //<ArgumentList> ::= <ArgumentList> ',' <Expression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ARRAYCREATIONEXPRESSION_NEW :
                //<ArrayCreationExpression> ::= new <PrimitiveType> <DimExprs> <Dims>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ARRAYCREATIONEXPRESSION_NEW2 :
                //<ArrayCreationExpression> ::= new <PrimitiveType> <DimExprs>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ARRAYCREATIONEXPRESSION_NEW3 :
                //<ArrayCreationExpression> ::= new <ClassOrInterfaceType> <DimExprs> <Dims>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ARRAYCREATIONEXPRESSION_NEW4 :
                //<ArrayCreationExpression> ::= new <ClassOrInterfaceType> <DimExprs>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_DIMEXPRS :
                //<DimExprs> ::= <DimExpr>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_DIMEXPRS2 :
                //<DimExprs> ::= <DimExprs> <DimExpr>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_DIMEXPR_LBRACKET_RBRACKET :
                //<DimExpr> ::= '[' <Expression> ']'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_DIMS_LBRACKET_RBRACKET :
                //<Dims> ::= '[' ']'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_DIMS_LBRACKET_RBRACKET2 :
                //<Dims> ::= <Dims> '[' ']'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FIELDACCESS_DOT_IDENTIFIER :
                //<FieldAccess> ::= <Primary> '.' Identifier
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FIELDACCESS_SUPER_DOT_IDENTIFIER :
                //<FieldAccess> ::= super '.' Identifier
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_METHODINVOCATION_LPAREN_RPAREN :
                //<MethodInvocation> ::= <Name> '(' <ArgumentList> ')'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_METHODINVOCATION_LPAREN_RPAREN2 :
                //<MethodInvocation> ::= <Name> '(' ')'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_METHODINVOCATION_DOT_IDENTIFIER_LPAREN_RPAREN :
                //<MethodInvocation> ::= <Primary> '.' Identifier '(' <ArgumentList> ')'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_METHODINVOCATION_DOT_IDENTIFIER_LPAREN_RPAREN2 :
                //<MethodInvocation> ::= <Primary> '.' Identifier '(' ')'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_METHODINVOCATION_SUPER_DOT_IDENTIFIER_LPAREN_RPAREN :
                //<MethodInvocation> ::= super '.' Identifier '(' <ArgumentList> ')'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_METHODINVOCATION_SUPER_DOT_IDENTIFIER_LPAREN_RPAREN2 :
                //<MethodInvocation> ::= super '.' Identifier '(' ')'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ARRAYACCESS_LBRACKET_RBRACKET :
                //<ArrayAccess> ::= <Name> '[' <Expression> ']'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ARRAYACCESS_LBRACKET_RBRACKET2 :
                //<ArrayAccess> ::= <PrimaryNoNewArray> '[' <Expression> ']'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_POSTFIXEXPRESSION :
                //<PostfixExpression> ::= <Primary>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_POSTFIXEXPRESSION2 :
                //<PostfixExpression> ::= <Name>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_POSTFIXEXPRESSION3 :
                //<PostfixExpression> ::= <PostIncrementExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_POSTFIXEXPRESSION4 :
                //<PostfixExpression> ::= <PostDecrementExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_POSTINCREMENTEXPRESSION_PLUSPLUS :
                //<PostIncrementExpression> ::= <PostfixExpression> '++'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_POSTDECREMENTEXPRESSION_MINUSMINUS :
                //<PostDecrementExpression> ::= <PostfixExpression> '--'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_UNARYEXPRESSION :
                //<UnaryExpression> ::= <PreIncrementExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_UNARYEXPRESSION2 :
                //<UnaryExpression> ::= <PreDecrementExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_UNARYEXPRESSION_PLUS :
                //<UnaryExpression> ::= '+' <UnaryExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_UNARYEXPRESSION_MINUS :
                //<UnaryExpression> ::= '-' <UnaryExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_UNARYEXPRESSION3 :
                //<UnaryExpression> ::= <UnaryExpressionNotPlusMinus>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_PREINCREMENTEXPRESSION_PLUSPLUS :
                //<PreIncrementExpression> ::= '++' <UnaryExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_PREDECREMENTEXPRESSION_MINUSMINUS :
                //<PreDecrementExpression> ::= '--' <UnaryExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_UNARYEXPRESSIONNOTPLUSMINUS :
                //<UnaryExpressionNotPlusMinus> ::= <PostfixExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_UNARYEXPRESSIONNOTPLUSMINUS_TILDE :
                //<UnaryExpressionNotPlusMinus> ::= '~' <UnaryExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_UNARYEXPRESSIONNOTPLUSMINUS_EXCLAM :
                //<UnaryExpressionNotPlusMinus> ::= '!' <UnaryExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_UNARYEXPRESSIONNOTPLUSMINUS2 :
                //<UnaryExpressionNotPlusMinus> ::= <CastExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CASTEXPRESSION_LPAREN_RPAREN :
                //<CastExpression> ::= '(' <PrimitiveType> <Dims> ')' <UnaryExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CASTEXPRESSION_LPAREN_RPAREN2 :
                //<CastExpression> ::= '(' <PrimitiveType> ')' <UnaryExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CASTEXPRESSION_LPAREN_RPAREN3 :
                //<CastExpression> ::= '(' <Expression> ')' <UnaryExpressionNotPlusMinus>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CASTEXPRESSION_LPAREN_RPAREN4 :
                //<CastExpression> ::= '(' <Name> <Dims> ')' <UnaryExpressionNotPlusMinus>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_MULTIPLICATIVEEXPRESSION :
                //<MultiplicativeExpression> ::= <UnaryExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_MULTIPLICATIVEEXPRESSION_TIMES :
                //<MultiplicativeExpression> ::= <MultiplicativeExpression> '*' <UnaryExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_MULTIPLICATIVEEXPRESSION_DIV :
                //<MultiplicativeExpression> ::= <MultiplicativeExpression> '/' <UnaryExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_MULTIPLICATIVEEXPRESSION_PERCENT :
                //<MultiplicativeExpression> ::= <MultiplicativeExpression> '%' <UnaryExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ADDITIVEEXPRESSION :
                //<AdditiveExpression> ::= <MultiplicativeExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ADDITIVEEXPRESSION_PLUS :
                //<AdditiveExpression> ::= <AdditiveExpression> '+' <MultiplicativeExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ADDITIVEEXPRESSION_MINUS :
                //<AdditiveExpression> ::= <AdditiveExpression> '-' <MultiplicativeExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_SHIFTEXPRESSION :
                //<ShiftExpression> ::= <AdditiveExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_SHIFTEXPRESSION_LTLT :
                //<ShiftExpression> ::= <ShiftExpression> '<<' <AdditiveExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_SHIFTEXPRESSION_GTGT :
                //<ShiftExpression> ::= <ShiftExpression> '>>' <AdditiveExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_SHIFTEXPRESSION_GTGTGT :
                //<ShiftExpression> ::= <ShiftExpression> '>>>' <AdditiveExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_RELATIONALEXPRESSION :
                //<RelationalExpression> ::= <ShiftExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_RELATIONALEXPRESSION_LT :
                //<RelationalExpression> ::= <RelationalExpression> '<' <ShiftExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_RELATIONALEXPRESSION_GT :
                //<RelationalExpression> ::= <RelationalExpression> '>' <ShiftExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_RELATIONALEXPRESSION_LTEQ :
                //<RelationalExpression> ::= <RelationalExpression> '<=' <ShiftExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_RELATIONALEXPRESSION_GTEQ :
                //<RelationalExpression> ::= <RelationalExpression> '>=' <ShiftExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_RELATIONALEXPRESSION_INSTANCEOF :
                //<RelationalExpression> ::= <RelationalExpression> instanceof <ReferenceType>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EQUALITYEXPRESSION :
                //<EqualityExpression> ::= <RelationalExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EQUALITYEXPRESSION_EQEQ :
                //<EqualityExpression> ::= <EqualityExpression> '==' <RelationalExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EQUALITYEXPRESSION_EXCLAMEQ :
                //<EqualityExpression> ::= <EqualityExpression> '!=' <RelationalExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ANDEXPRESSION :
                //<AndExpression> ::= <EqualityExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ANDEXPRESSION_AMP :
                //<AndExpression> ::= <AndExpression> '&' <EqualityExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXCLUSIVEOREXPRESSION :
                //<ExclusiveOrExpression> ::= <AndExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXCLUSIVEOREXPRESSION_CARET :
                //<ExclusiveOrExpression> ::= <ExclusiveOrExpression> '^' <AndExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_INCLUSIVEOREXPRESSION :
                //<InclusiveOrExpression> ::= <ExclusiveOrExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_INCLUSIVEOREXPRESSION_PIPE :
                //<InclusiveOrExpression> ::= <InclusiveOrExpression> '|' <ExclusiveOrExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CONDITIONALANDEXPRESSION :
                //<ConditionalAndExpression> ::= <InclusiveOrExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CONDITIONALANDEXPRESSION_AMPAMP :
                //<ConditionalAndExpression> ::= <ConditionalAndExpression> '&&' <InclusiveOrExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CONDITIONALOREXPRESSION :
                //<ConditionalOrExpression> ::= <ConditionalAndExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CONDITIONALOREXPRESSION_PIPEPIPE :
                //<ConditionalOrExpression> ::= <ConditionalOrExpression> '||' <ConditionalAndExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CONDITIONALEXPRESSION :
                //<ConditionalExpression> ::= <ConditionalOrExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CONDITIONALEXPRESSION_QUESTION_COLON :
                //<ConditionalExpression> ::= <ConditionalOrExpression> '?' <Expression> ':' <ConditionalExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ASSIGNMENTEXPRESSION :
                //<AssignmentExpression> ::= <ConditionalExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ASSIGNMENTEXPRESSION2 :
                //<AssignmentExpression> ::= <Assignment>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ASSIGNMENT :
                //<Assignment> ::= <LeftHandSide> <AssignmentOperator> <AssignmentExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_LEFTHANDSIDE :
                //<LeftHandSide> ::= <Name>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_LEFTHANDSIDE2 :
                //<LeftHandSide> ::= <FieldAccess>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_LEFTHANDSIDE3 :
                //<LeftHandSide> ::= <ArrayAccess>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ASSIGNMENTOPERATOR_EQ :
                //<AssignmentOperator> ::= '='
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ASSIGNMENTOPERATOR_TIMESEQ :
                //<AssignmentOperator> ::= '*='
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ASSIGNMENTOPERATOR_DIVEQ :
                //<AssignmentOperator> ::= '/='
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ASSIGNMENTOPERATOR_PERCENTEQ :
                //<AssignmentOperator> ::= '%='
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ASSIGNMENTOPERATOR_PLUSEQ :
                //<AssignmentOperator> ::= '+='
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ASSIGNMENTOPERATOR_MINUSEQ :
                //<AssignmentOperator> ::= '-='
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ASSIGNMENTOPERATOR_LTLTEQ :
                //<AssignmentOperator> ::= '<<='
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ASSIGNMENTOPERATOR_GTGTEQ :
                //<AssignmentOperator> ::= '>>='
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ASSIGNMENTOPERATOR_GTGTGTEQ :
                //<AssignmentOperator> ::= '>>>='
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ASSIGNMENTOPERATOR_AMPEQ :
                //<AssignmentOperator> ::= '&='
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ASSIGNMENTOPERATOR_CARETEQ :
                //<AssignmentOperator> ::= '^='
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ASSIGNMENTOPERATOR_PIPEEQ :
                //<AssignmentOperator> ::= '|='
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION :
                //<Expression> ::= <AssignmentExpression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CONSTANTEXPRESSION :
                //<ConstantExpression> ::= <Expression>
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

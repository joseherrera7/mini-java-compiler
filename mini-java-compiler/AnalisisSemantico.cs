using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Irony.Parsing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace mini_java_compiler
{
    class AnalisisSemantico
    {
        SymbolTables SymbolTable = new SymbolTables();
        public static ParseTree Analize(string entry)
        {
            var grammar = new Grammar();
            var syntactic = new Parser(grammar);

            return syntactic.Parse(entry);
        }

        public bool ValidateRegex(string chain, string regex)
        {
            Match validation = Regex.Match(chain, regex);
            return validation.Success;
        }
        public bool CheckDup(SymbolTables table)
        {
            var counters = new Dictionary<string, int>();

            foreach (Symbol Symbol in table.Symbols)
            {
                string id = Symbol.Id;

                if (!counters.ContainsKey(id))
                    counters[id] = 0;

                counters[id] += 1;

                if (counters[id] > 1)
                    return false;
            }

            return true;
        }

        public bool CheckType(SymbolTables table)
        {
            foreach (Symbol Symbol in table.Symbols)
            {
                string type = Symbol.type;
                string value = Symbol.value;

                if (value == null)
                    continue;

                // Si el valor del símbolo actual es un id
                if (ValidateRegex(value, Gramatica.RegularExpressions.IdRegex) && !ValidateRegex(value, Gramatica.RegularExpressions.StringRegex))
                {
                    // Primero, checamos si el identificador existe
                    if (!table.ContainsSymbol(value))
                        return false;

                    // Despues, tenemos que obtener el valor de dicho id para comprobar su tipo
                    value = ValueOf(table, value);
                }

                switch (type)
                {
                    case Gramatica.Terminals.Int:
                        {
                            if (!ValidateRegex(value, Gramatica.RegularExpressions.RegexNumber))
                                return false;

                            if (value.Contains('.'))
                                return false;

                            break;
                        }

                    case Gramatica.Terminals.Float:
                        {
                            if (!ValidateRegex(value, Gramatica.RegularExpressions.RegexNumber))
                                return false;

                            break;
                        }

                    case Gramatica.Terminals.Double:
                        {
                            if (!ValidateRegex(value, Gramatica.RegularExpressions.RegexNumber))
                                return false;

                            break;
                        }

                    case Gramatica.Terminals.Bool:
                        {
                            if (!value.Equals(Gramatica.Terminals.True) || !value.Equals(Gramatica.Terminals.False))
                                return false;

                            break;
                        }

                    case Gramatica.Terminals.String:
                        {
                            if (!ValidateRegex(value, Gramatica.RegularExpressions.StringRegex))
                                return false;

                            break;
                        }
                }
            }

            return true;
        }

        public void SemanticAnalyzer(string text)
        {   
            ParseTree parseTree = Analize(text);

            if (parseTree.Root == null)
            {
                MessageBox.Show("Existe un error sintáctico");
                return;
            }

            // hacer arbol
            var Tree = new Tree(parseTree);

            // revisar tabla de simbolos
            SymbolTable = GenerateTable(Tree);

            bool duplicates = CheckDup(SymbolTable);

            if (duplicates == false)
            {
                MessageBox.Show("Hay variables duplicadas");
                return;
            }

            bool types = CheckType(SymbolTable);

            if (types == false)
            {
                MessageBox.Show("Existe error de tipo");
                return;
            }

            

            var stringBuilder = new StringBuilder();

            foreach (var s in SymbolTable.Symbols)
            {
                stringBuilder.Append(s.ToString()).Append('\n');
            }

            MessageBox.Show(stringBuilder.ToString());
        }

        private string ValueOf(SymbolTables table, string id)
        {
            Console.WriteLine("/ = / = / = / = / = / = / = / = / = / = / = / = / = / = / = / = / = / = / = / = / = /");
            Console.WriteLine($"Revisando tipo del id '{id}'");

            Symbol Symbol = table.SearchSymbol(id);

            Console.WriteLine($"El valor del id actual '{id}' es '{Symbol.value}'");

            if (id.Equals(Symbol.value))
                return null;

            if (Symbol.value == null)
                return null;

            if (ValidateRegex(Symbol.value, Gramatica.RegularExpressions.IdRegex) && !ValidateRegex(Symbol.value, Gramatica.RegularExpressions.StringRegex))
            {
                Console.WriteLine("Recursando...");
                return ValueOf(table, Symbol.value);
            }

            Console.WriteLine($"Se encontro el tipo del id '{id}'. tipo es '{Symbol.type}'");
            return Symbol.value;
        }

        public SymbolTables GenerateTable(Tree Tree)
        {
            var table = new SymbolTables();
            List<ParseTreeNode> nodes = Tree.Travel(Gramatica.NoTerminals.DeclVar);

            foreach (ParseTreeNode node in nodes)
            {
                List<Symbol> Symbols = CreateSymbol(Tree, node);
                table.AgregateSymbols(Symbols);
            }

            return table;
        }

        private List<Symbol> CreateSymbol(Tree Tree, ParseTreeNode node)
        {
            var Symbols = new List<Symbol>();

            List<ParseTreeNode> types = Tree.Travel(node, Gramatica.NoTerminals.Type);
            List<ParseTreeNode> ids = Tree.Travel(node, Gramatica.RegularExpressions.Id);
            List<ParseTreeNode> Assignmentes = Tree.Travel(node, Gramatica.NoTerminals.Assign);
            var listAssignables = new List<List<ParseTreeNode>>();

            Assignmentes.ForEach(Node =>
            {
                List<ParseTreeNode> leaf = Tree.leafDe(Node);
                listAssignables.Add(leaf);
            });

            // Crear simbolos
            for (int i = 0; i < ids.Count; i++)
            {
                string type = types[0].FindTokenAndGetText();
                string id = ids[i].FindTokenAndGetText();

                if (listAssignables.Count == 0)
                    Symbols.Add(new Symbol(type, id));

                else
                {
                    var stringBuilder = new StringBuilder();

                    listAssignables[i].ForEach(token =>
                    {
                        stringBuilder.Append($"{token.FindTokenAndGetText()} ");
                    });

                    string Assignable = stringBuilder.ToString().Trim();
                    Symbols.Add(new Symbol(type, id, Assignable));
                }
            }

            return Symbols;
        }

    }

    public class SymbolTables
    {
        public List<Symbol> Symbols = new List<Symbol>();

        public SymbolTables() { }

        public void AgregateSymbol(Symbol Symbol)
        {
            Symbols.Add(Symbol);
        }

        public void AgregateSymbols(List<Symbol> Symbols)
        {
            Symbols.AddRange(Symbols);
        }

        public Symbol SearchSymbol(string identifier)
        {
            return Symbols.Find((Symbol) => Symbol.Id.Equals(identifier)) ?? null;
        }

        public bool ContainsSymbol(string identifier)
        {
            foreach (Symbol Symbol in Symbols)
                if (Symbol.Id.Equals(identifier))
                    return true;

            return false;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("SymbolTable{\n");

            foreach (Symbol Symbol in Symbols)
            {
                stringBuilder.Append('\t').Append(Symbol.ToString()).Append('\n');
            }

            stringBuilder.Append('}');

            return stringBuilder.ToString();
        }
    }

    public class Symbol
    {
        public string type;
        public string Id;
        public string value;

        public Symbol(string type, string identifier)
        {
            this.type = type;
            this.Id = identifier;
        }

        public Symbol(string type, string identifier, string value)
        {
            this.type = type;
            this.Id = identifier;
            this.value = value;
        }

        public override string ToString()
        {
            return "Symbol{'" + type + "', '" + Id + "', '" + value + "'}";
        }
    }

    public class Tree
    {
        private readonly ParseTree Trees;

        public Tree(ParseTree Tree)
        {
            this.Trees = Tree;
        }

        public bool IsLeaf(ParseTreeNode node)
        {
            if (node.ChildNodes == null)
                return true;

            return node.ChildNodes.Count == 0;
        }

        /// <summary>
        /// Obtener las hojas de la raiz especificada
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public List<ParseTreeNode> leafDe(ParseTreeNode root)
        {
            var nodes = new List<ParseTreeNode>();
            leafDe(root, nodes);
            return nodes;
        }

        private void leafDe(ParseTreeNode root, List<ParseTreeNode> nodes)
        {
            if (IsLeaf(root))
                nodes.Add(root);

            root.ChildNodes.ForEach(node =>
            {
                leafDe(node, nodes);
            });
        }

        public string Printnode(ParseTreeNode node)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder
            .Append("Tag                   | ").Append(node.Tag).Append('\n')
            .Append("Term                  | ").Append(node.Term).Append('\n')
            .Append("Token                 | ").Append(node.Token).Append('\n')
            .Append("FindToken()           | ").Append(node.FindToken()).Append('\n')
            .Append("FindTokenAndGetText() | ").Append(node.FindTokenAndGetText()).Append('\n')
            .Append("ToString()            | ").Append(node.ToString());

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Obtener nodos los nodos del arbol
        /// </summary>
        /// <returns></returns>
        public List<ParseTreeNode> Travel()
        {
            var nodes = new List<ParseTreeNode>();
            Travel(Trees.Root, nodes);
            return nodes;
        }

        /// <summary>
        /// Obtener nodos los nodes del arbol recorriendo desde la raiz especificada
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public List<ParseTreeNode> Travel(ParseTreeNode root)
        {
            var nodes = new List<ParseTreeNode>();
            Travel(root, nodes);
            return nodes;
        }

        private void Travel(ParseTreeNode root, List<ParseTreeNode> nodes)
        {
            nodes.Add(root);
            root.ChildNodes.ForEach(node =>
            {
                Travel(node, nodes);
            });
        }

        /// <summary>
        /// Obtener los nodos que conforman una el lado derecho de una Asignacion (despues del '=')
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public List<ParseTreeNode> TravelAssignables(ParseTreeNode root)
        {
            var nodes = new List<ParseTreeNode>();
            TravelAssignables(root, nodes);
            return nodes;
        }

        private void TravelAssignables(ParseTreeNode root, List<ParseTreeNode> nodes)
        {
            nodes.Add(root);
            root.ChildNodes.ForEach(node =>
            {
                TravelAssignables(node, nodes);
            });
        }

        /// <summary>
        /// Obtener nodos los nodos que pertenezcan al termino especificado
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public List<ParseTreeNode> Travel(string term)
        {
            List<ParseTreeNode> nodes =
                Travel()
                .FindAll(node => node.Term.ToString().Equals(term));

            return nodes;
        }

        /// <summary>
        /// Obtener nodos los nodos desde recorriendo desde la raiz especificada y que pertenezcan al termino especificado
        /// </summary>
        /// <param name="root"></param>
        /// <param name="term"></param>
        /// <returns></returns>
        public List<ParseTreeNode> Travel(ParseTreeNode root, string term)
        {
            List<ParseTreeNode> nodes =
                Travel(root)
                .FindAll(node => node.Term.ToString().Equals(term));

            return nodes;
        }

        public TokenList Tokens()
        {
            return Trees.Tokens;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            Travel().ForEach(node =>
            {
                stringBuilder.Append(node.ToString()).Append("\n");
            });

            return stringBuilder.ToString();
        }
    }

    class Gramatica : Grammar
    {
        public static class NoTerminals
        {
            public const string Begin = "<inicio>";
            public const string DeclVar = "<declaracion-variable>";
            public const string ListDeclVar = "<lista-declaracion-variables>";
            public const string ListDeclVarValue = "<lista-declaracion-variables-valores>";
            public const string ListDeclVarDinamic = "<lista-declaracion-variables-dinamica>";
            public const string DeclConstant = "<declaracion-constante>";
            public const string ListDeclConstant = "<lista-declaracion-constante>";
            public const string Type = "<tipo>";
            public const string Asignation = "<asignacion>";
            public const string AsignationSentence = "<asignacion-sentencia>";
            public const string Assign = "<asignable>";
            public const string ListAssign = "<lista-asignable>";
            public const string ArithmeticExpression = "<expresion-aritemtica>";
            public const string ArithmeticOperator = "<operador-aritmetico>";
            public const string RelationalExpression = "<expresion-relacional>";
            public const string RelationalOperator = "<operador-relacional>";
            public const string FunctionCall = "<llamada-funcion>";
            public const string IdFunctionCall = "<id-llamada-funcion>";
            public const string FunctionDecl = "<declaracion-funcion>";
            public const string FunctionDecl2 = "<declaracion-funcion2>";
            public const string FunctionDecl3 = "<declaracion-funcion3>";
            public const string FunctionDecl4 = "<declaracion-funcion4>";
            public const string FunctionDecl5 = "<declaracion-funcion5>";
            public const string FunctionDecl6 = "<declaracion-funcion6>";
            public const string FunctionDecl7 = "<declaracion-funcion7>";
            public const string FunctionDecl8 = "<declaracion-funcion8>";
            public const string FunctionType = "<tipo-funcion>";
            public const string FunctionSection = "<bloque-funcion>";
            public const string Parameter = "<parametro>";
            public const string ParameterList = "<lista-parametro>";
            public const string ParameterListImplements = "<lista-parametro-implements>";
            public const string Sentence = "<sentencia>";
            public const string SentenceList = "<lista-sentencia>";
            public const string FlowController = "<controlador-flujo>";
            public const string IfSentence = "<if>";
            public const string IfSection = "<bloque-if>";
            public const string ElseSentence = "<else>";
            public const string WhenSentence = "<when>";
            public const string WhenSection = "<bloque-when>";
            public const string WhenOption = "<opcion-when>";
            public const string WhenOptionList = "<lista-opcion-when>";
            public const string DefaultWhen = "<default-when>";
            public const string WhileSentence = "<sentencia-while>";
            public const string WhileSection = "<bloque-while>";
            public const string ForSentence = "<for>";
            public const string ForParameter = "<parametros-for>";
            public const string ForParameter1 = "<parametro-for-1>";
            public const string ForParameter2 = "<parametro-for-2>";
            public const string ForParameter3 = "<parametro-for-3>";
            public const string ForSection = "<bloque-for>";
        }

        public static class Terminals
        {
            public const string Void = "void";
            public const string Return = "return";
            public const string Var = "var";
            public const string Const = "const";
            public const string Null = "null";
            public const string True = "true";
            public const string False = "false";

            public const string If = "if";
            public const string Else = "else";
            public const string When = "when";
            public const string Matches = "matches";
            public const string Default = "default";
            public const string While = "while";
            public const string For = "for";

            public const string Int = "int";
            public const string Float = "float";
            public const string Double = "double";
            public const string Bool = "bool";
            public const string String = "string";

            public const string And = "and";
            public const string Or = "or";
            public const string Not = "not";

            public const string Add = "+=";
            public const string Sub = "-=";
            public const string Mul = "*=";
            public const string Div = "/=";
            public const string Mod = "%=";
            public const string Pow = "^=";
            public const string Roo = "~=";

            public const string EqualEqual = "==";
            public const string Different = "<>";
            public const string Greater = ">";
            public const string GreaterEqual = ">=";
            public const string Less = "<";
            public const string LessEqual = "<=";

            public const string Plus = "+";
            public const string Minus = "-";
            public const string Per = "*";
            public const string Between = "/";
            public const string Module = "%";
            public const string Potency = "^";
            public const string Root = "~";

            public const string Point = ".";
            public const string Coma = ",";
            public const string DoublePoint = ":";
            public const string PointComa = ";";
            public const string TwoDoublePoint = "::";
            public const string OpenParenthesis = "(";
            public const string ClosedParenthesis = ")";
            public const string OpenKey = "{";
            public const string ClosedKey = "}";
            public const string Equal = "=";
        }

        public static class RegularExpressions
        {
            public const string Comment = "comentario";
            public const string RegexComment = "\\/\\*[\\s\\S]*?\\*\\/";
            public const string Id = "id";
            public const string IdRegex = "([a-zA-Z]|_*[a-zA-Z]){1}[a-zA-Z0-9_]*";
            public const string IdAsignable = "id-asignable";
            public const string IdAsignableRegex = "([a-zA-Z]|_*[a-zA-Z]){1}[a-zA-Z0-9_]*";
            public const string Number = "numero";
            public const string RegexNumber = "\\d+[f|d]?(\\.\\d+[f|d]?)?";
            public const string String = "string";
            public const string StringRegex = "\"[^\"]*\"";
        }

        public Gramatica() : base()
        {
            #region No terminales
            var inicio = new NonTerminal(NoTerminals.Begin);
            var declaracionVariable = new NonTerminal(NoTerminals.DeclVar);
            var listaDeclaracionVariable = new NonTerminal(NoTerminals.ListDeclVar);
            var listaDeclaracionVariableValores = new NonTerminal(NoTerminals.ListDeclVarValue);
            var listaParametroImplements = new NonTerminal(NoTerminals.ParameterListImplements);
            var listaDeclaracionVariableDinamica = new NonTerminal(NoTerminals.ListDeclVarDinamic);
            var declaracionConstante = new NonTerminal(NoTerminals.DeclConstant);
            var listaDeclaracionConstante = new NonTerminal(NoTerminals.ListDeclConstant);
            var tipo = new NonTerminal(NoTerminals.Type);
            var asignacion = new NonTerminal(NoTerminals.Asignation);
            var asignacionSentencia = new NonTerminal(NoTerminals.AsignationSentence);
            var asignable = new NonTerminal(NoTerminals.Assign);
            var listaAsignable = new NonTerminal(NoTerminals.ListAssign);
            var expresionAritmetica = new NonTerminal(NoTerminals.ArithmeticExpression);
            var operadorAritmetico = new NonTerminal(NoTerminals.ArithmeticOperator);
            var expresionRelacional = new NonTerminal(NoTerminals.RelationalExpression);
            var operadorRelacional = new NonTerminal(NoTerminals.RelationalOperator);
            var llamadaFuncion = new NonTerminal(NoTerminals.FunctionCall);
            var idLlamadaFuncion = new NonTerminal(NoTerminals.IdFunctionCall);
            var declaracionFuncion = new NonTerminal(NoTerminals.FunctionDecl);
            var declaracionFuncion2 = new NonTerminal(NoTerminals.FunctionDecl2);
            var declaracionFuncion3 = new NonTerminal(NoTerminals.FunctionDecl3);
            var declaracionFuncion4 = new NonTerminal(NoTerminals.FunctionDecl4);
            var declaracionFuncion5 = new NonTerminal(NoTerminals.FunctionDecl5);
            var declaracionFuncion6 = new NonTerminal(NoTerminals.FunctionDecl6);
            var declaracionFuncion7 = new NonTerminal(NoTerminals.FunctionDecl7);
            var declaracionFuncion8 = new NonTerminal(NoTerminals.FunctionDecl8);
            var tipoFuncion = new NonTerminal(NoTerminals.FunctionType);
            var bloqueFuncion = new NonTerminal(NoTerminals.FunctionSection);
            var parametro = new NonTerminal(NoTerminals.Parameter);
            var listaParametro = new NonTerminal(NoTerminals.ParameterList);
            var sentencia = new NonTerminal(NoTerminals.Sentence);
            var listaSentencia = new NonTerminal(NoTerminals.SentenceList);
            var controladorFlujo = new NonTerminal(NoTerminals.FlowController);
            var sentenciaIf = new NonTerminal(NoTerminals.IfSentence);
            var bloqueIf = new NonTerminal(NoTerminals.IfSection);
            var sentenciaElse = new NonTerminal(NoTerminals.ElseSentence);
            var sentenciaWhen = new NonTerminal(NoTerminals.WhenSentence);
            var bloqueWhen = new NonTerminal(NoTerminals.WhenSection);
            var opcionWhen = new NonTerminal(NoTerminals.WhenOption);
            var listaOpcionWhen = new NonTerminal(NoTerminals.WhenOptionList);
            var defaultWhen = new NonTerminal(NoTerminals.DefaultWhen);
            var sentenciaWhile = new NonTerminal(NoTerminals.WhileSentence);
            var bloqueWhile = new NonTerminal(NoTerminals.WhileSection);
            var sentenciaFor = new NonTerminal(NoTerminals.ForSentence);
            var parametrosFor = new NonTerminal(NoTerminals.ForParameter);
            var parametroFor1 = new NonTerminal(NoTerminals.ForParameter1);
            var parametroFor2 = new NonTerminal(NoTerminals.ForParameter2);
            var parametroFor3 = new NonTerminal(NoTerminals.ForParameter3);
            var bloqueFor = new NonTerminal(NoTerminals.ForSection);
            #endregion

            #region Terminals

            // palabras reservadas
            var void_ = ToTerm(Terminals.Void);
            var return_ = ToTerm(Terminals.Return);
            var var_ = ToTerm(Terminals.Var);
            var const_ = ToTerm(Terminals.Const);
            var null_ = ToTerm(Terminals.Null);
            var true_ = ToTerm(Terminals.True);
            var false_ = ToTerm(Terminals.False);

            // flow controllers
            var if_ = ToTerm(Terminals.If);
            var else_ = ToTerm(Terminals.Else);
            var when_ = ToTerm(Terminals.When);
            var matches_ = ToTerm(Terminals.Matches);
            var default_ = ToTerm(Terminals.Default);
            var while_ = ToTerm(Terminals.While);
            var for_ = ToTerm(Terminals.For);

            // data types
            var int_ = ToTerm(Terminals.Int);
            var float_ = ToTerm(Terminals.Float);
            var double_ = ToTerm(Terminals.Double);
            var bool_ = ToTerm(Terminals.Bool);
            var string_ = ToTerm(Terminals.String);

            // logical operators
            var and_ = ToTerm(Terminals.And);
            var or_ = ToTerm(Terminals.Or);
            var not_ = ToTerm(Terminals.Not);

            // math operators
            var add_ = ToTerm(Terminals.Add);
            var sub_ = ToTerm(Terminals.Sub);
            var mul_ = ToTerm(Terminals.Mul);
            var div_ = ToTerm(Terminals.Div);
            var mod_ = ToTerm(Terminals.Mod);
            var pow_ = ToTerm(Terminals.Pow);
            var roo_ = ToTerm(Terminals.Roo);

            // relational operators
            var igualIgual_ = ToTerm(Terminals.EqualEqual);
            var diferente_ = ToTerm(Terminals.Different);
            var mayor_ = ToTerm(Terminals.Greater);
            var mayorIgual_ = ToTerm(Terminals.GreaterEqual);
            var menor_ = ToTerm(Terminals.Less);
            var menorIgual_ = ToTerm(Terminals.LessEqual);

            // arithmetic operators
            var mas_ = ToTerm(Terminals.Plus);
            var menos_ = ToTerm(Terminals.Minus);
            var por_ = ToTerm(Terminals.Per);
            var entre_ = ToTerm(Terminals.Between);
            var modulo_ = ToTerm(Terminals.Module);
            var potencia_ = ToTerm(Terminals.Potency);
            var raiz_ = ToTerm(Terminals.Root);

            // others
            var punto_ = ToTerm(Terminals.Point);
            var coma_ = ToTerm(Terminals.Coma);
            var dosPuntos_ = ToTerm(Terminals.DoublePoint);
            var puntoComa_ = ToTerm(Terminals.PointComa);
            var dosPuntosDoble_ = ToTerm(Terminals.TwoDoublePoint);
            var parentesisAbrir_ = ToTerm(Terminals.OpenParenthesis);
            var parentesisCerrar_ = ToTerm(Terminals.ClosedParenthesis);
            var llavesAbrir_ = ToTerm(Terminals.OpenKey);
            var llavesCerrar_ = ToTerm(Terminals.ClosedKey);
            var igual_ = ToTerm(Terminals.Equal);
            #endregion

            #region Regex
            var comentario = new RegexBasedTerminal(RegularExpressions.Comment, RegularExpressions.RegexComment);
            var id = new RegexBasedTerminal(RegularExpressions.Id, RegularExpressions.IdRegex);
            var idAsignable = new RegexBasedTerminal(RegularExpressions.IdAsignable, RegularExpressions.IdAsignableRegex);
            var numero = new RegexBasedTerminal(RegularExpressions.Number, RegularExpressions.RegexNumber);
            var stringRegex = new RegexBasedTerminal(RegularExpressions.String, RegularExpressions.StringRegex);
            #endregion

            #region Production rules

            inicio.Rule =
                declaracionFuncion + inicio |
                declaracionFuncion |
                declaracionFuncion2 |
                declaracionFuncion3 |
                declaracionFuncion4 |
                declaracionFuncion5 |
                declaracionFuncion6 |
                declaracionFuncion7 |
                declaracionFuncion8;

            declaracionFuncion.Rule =
                ToTerm("public") + ToTerm("class") + ToTerm("Program") + llavesAbrir_ + ToTerm("public") + ToTerm("static") + ToTerm("void") + ToTerm("main") + parentesisAbrir_ + string_ + ToTerm("[") + ToTerm("]") + ToTerm("args") + parentesisCerrar_ + bloqueFuncion + llavesCerrar_;

            declaracionFuncion2.Rule =
                tipoFuncion + id + parentesisAbrir_ + parentesisCerrar_ + bloqueFuncion;

            declaracionFuncion3.Rule =
                ToTerm("public") + ToTerm("static") + ToTerm("void") + ToTerm("main") + parentesisAbrir_ + string_ + ToTerm("[") + ToTerm("]") + ToTerm("args") + parentesisCerrar_ + bloqueFuncion;

            declaracionFuncion4.Rule =
                tipoFuncion + id + parentesisAbrir_ + listaParametro + parentesisCerrar_ + bloqueFuncion;

            declaracionFuncion5.Rule =
                ToTerm("class") + id + bloqueFuncion;

            declaracionFuncion6.Rule =
                ToTerm("interface") + id + bloqueFuncion;

            declaracionFuncion7.Rule = ToTerm("class") + id + ToTerm("extends") + id + bloqueFuncion |
                ToTerm("class") + id + ToTerm("extends") + id + ToTerm("implements") + listaParametroImplements + bloqueFuncion;

            declaracionFuncion8.Rule =
                ToTerm("class") + id + ToTerm("implements") + listaParametroImplements + bloqueFuncion;

            tipoFuncion.Rule =
                void_ |
                tipo;

            tipo.Rule =
                int_ |
                float_ |
                double_ |
                bool_ |
                string_ |
                ToTerm("static") + int_ |
                ToTerm("static") + float_ |
                ToTerm("static") + double_ |
                ToTerm("static") + bool_ |
                ToTerm("static") + string_;

            sentencia.Rule =
                declaracionVariable;

            bloqueFuncion.Rule =
                llavesAbrir_ + llavesCerrar_ |
                llavesAbrir_ + listaSentencia + llavesCerrar_;

            listaParametro.Rule = tipo + id + coma_ + listaParametro |
                tipo + id;

            listaParametroImplements.Rule = id + coma_ + listaParametroImplements |
                id;


            listaSentencia.Rule =
                sentencia + puntoComa_ + listaSentencia |
                sentencia + puntoComa_ |

                controladorFlujo + listaSentencia |
                controladorFlujo;

            declaracionVariable.Rule =
                tipo + listaDeclaracionVariable |
                tipo + listaDeclaracionVariableValores;

            listaDeclaracionVariable.Rule =
                id + coma_ + listaDeclaracionVariable |
                id;

            listaDeclaracionVariableValores.Rule =
                asignacion + coma_ + listaDeclaracionVariableValores |
                asignacion;

            asignacion.Rule =
                id + igual_ + asignable |
                id + mas_ + mas_ |
                id + menos_ + menos_;

            asignacionSentencia.Rule =
                id + igual_ + asignable;

            asignable.Rule =
                idAsignable |
                expresionAritmetica |
                stringRegex |
                llamadaFuncion |
                bool_ |
                true_ |
                false_;

            listaAsignable.Rule =
                asignable + coma_ + listaAsignable |
                asignable;

            expresionAritmetica.Rule =
                numero |
                id |
                llamadaFuncion |
                parentesisAbrir_ + expresionAritmetica + parentesisCerrar_ |
                expresionAritmetica + operadorAritmetico + expresionAritmetica;

            operadorAritmetico.Rule =
                mas_ |
                menos_ |
                por_ |
                entre_ |
                modulo_ |
                potencia_ |
                raiz_;

            controladorFlujo.Rule =
                sentenciaIf |
                sentenciaWhen |
                sentenciaWhile |
                sentenciaFor;

            sentenciaIf.Rule =
                if_ + parentesisAbrir_ + expresionRelacional + parentesisCerrar_ + bloqueIf |
                if_ + parentesisAbrir_ + expresionRelacional + parentesisCerrar_ + bloqueIf + else_ + sentenciaIf |
                if_ + parentesisAbrir_ + expresionRelacional + parentesisCerrar_ + bloqueIf + else_ + bloqueIf;

            bloqueIf.Rule =
                bloqueFuncion |
                sentencia + puntoComa_ |
                controladorFlujo;

            expresionRelacional.Rule =
                asignable + operadorRelacional + asignable;

            operadorRelacional.Rule =
                igualIgual_ |
                diferente_ |
                mayorIgual_ |
                mayor_ |
                menorIgual_ |
                menor_;

            sentenciaWhen.Rule =
                when_ + parentesisAbrir_ + id + parentesisCerrar_ + bloqueWhen;

            bloqueWhen.Rule =
                llavesAbrir_ + llavesCerrar_ |
                llavesAbrir_ + listaOpcionWhen + llavesCerrar_;

            listaOpcionWhen.Rule =
                opcionWhen + listaOpcionWhen |
                opcionWhen |
                defaultWhen;

            opcionWhen.Rule =
                matches_ + listaAsignable + bloqueFuncion;

            defaultWhen.Rule =
                default_ + bloqueFuncion;

            sentenciaWhile.Rule =
                while_ + parentesisAbrir_ + expresionRelacional + parentesisCerrar_ + bloqueWhile |
                while_ + parentesisAbrir_ + id + parentesisCerrar_ + bloqueWhile |
                while_ + parentesisAbrir_ + bool_ + parentesisCerrar_ + bloqueWhile;

            bloqueWhile.Rule =
                bloqueFuncion |
                sentencia + puntoComa_ |
                controladorFlujo;

            sentenciaFor.Rule =
                for_ + parentesisAbrir_ + parametrosFor + parentesisCerrar_ + bloqueFor;

            parametrosFor.Rule =
                puntoComa_ + puntoComa_ |
                parametroFor1 + puntoComa_ + puntoComa_ |
                puntoComa_ + parametroFor2 + puntoComa_ |
                parametroFor1 + puntoComa_ + parametroFor2 + puntoComa_ |
                puntoComa_ + puntoComa_ + parametroFor3 |
                parametroFor1 + puntoComa_ + puntoComa_ + parametroFor3 |
                puntoComa_ + parametroFor2 + puntoComa_ + parametroFor3 |
                parametroFor1 + puntoComa_ + parametroFor2 + puntoComa_ + parametroFor3;

            parametroFor1.Rule =
                declaracionVariable;

            parametroFor2.Rule =
                expresionRelacional;

            parametroFor3.Rule =
                asignacion;

            bloqueFor.Rule =
                bloqueFuncion |
                sentencia + puntoComa_ |
                controladorFlujo;

            llamadaFuncion.Rule =
                idLlamadaFuncion + parentesisAbrir_ + parentesisCerrar_ |
                idLlamadaFuncion + parentesisAbrir_ + listaAsignable + parentesisCerrar_;

            idLlamadaFuncion.Rule =
                id + punto_ + idLlamadaFuncion |
                id + dosPuntosDoble_ + idLlamadaFuncion |
                id;

            #endregion

            #region Preferences
            Root = inicio;
            #endregion
        }
    }
}

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
            var grammar = new grammar();
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

                // Si el value del símbolo actual es un id
                if (ValidateRegex(value, grammar.RegularExpressions.IdRegex) && !ValidateRegex(value, grammar.RegularExpressions.StringRegex))
                {
                    // Primero, checamos si el identifier existe
                    if (!table.ContainsSymbol(value))
                        return false;

                    // Despues, tenemos que obtener el value de dicho id para comprobar su tipo
                    value = ValueOf(table, value);
                }

                switch (type)
                {
                    case grammar.Terminals.Int:
                        {
                            if (!ValidateRegex(value, grammar.RegularExpressions.RegexNumber))
                                return false;

                            if (value.Contains('.'))
                                return false;

                            break;
                        }

                    case grammar.Terminals.Float:
                        {
                            if (!ValidateRegex(value, grammar.RegularExpressions.RegexNumber))
                                return false;

                            break;
                        }

                    case grammar.Terminals.Double:
                        {
                            if (!ValidateRegex(value, grammar.RegularExpressions.RegexNumber))
                                return false;

                            break;
                        }

                    case grammar.Terminals.Bool:
                        {
                            if (!value.Equals(grammar.Terminals.True) || !value.Equals(grammar.Terminals.False))
                                return false;

                            break;
                        }

                    case grammar.Terminals.String:
                        {
                            if (!ValidateRegex(value, grammar.RegularExpressions.StringRegex))
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

            // checar tabla de simbolos
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

            if (ValidateRegex(Symbol.value, grammar.RegularExpressions.IdRegex) && !ValidateRegex(Symbol.value, grammar.RegularExpressions.StringRegex))
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
            List<ParseTreeNode> nodes = Tree.Travel(grammar.NoTerminals.VariableDeclaration);

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

            List<ParseTreeNode> types = Tree.Travel(node, grammar.NoTerminals.type);
            List<ParseTreeNode> ids = Tree.Travel(node, grammar.RegularExpressions.Id);
            List<ParseTreeNode> Assignmentes = Tree.Travel(node, grammar.NoTerminals.Assignable);
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
            stringBuilder.Append("tableSymbols{\n");

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

    class grammar : Grammar
    {
        public static class NoTerminals
        {
            public const string Begin = "<inicio>";
            public const string VariableDeclaration = "<declaracion-variables>";
            public const string listVariableDeclaration = "<lista-declaracion-variables>";
            public const string listVariableDeclarationvaluees = "<lista-declaracion-variables-valores>";
            public const string listVariableDeclarationDynamic = "<lista-declaracion-variables-dinamico>";
            public const string DeclarationConstant = "<declaracion-constante>";
            public const string listDeclarationConstant = "<lista-declaracion-constante>";
            public const string type = "<tipo>";
            public const string Assignment = "<asignacion>";
            public const string AssignmentSentence = "<asignacion-sentencia>";
            public const string Assignable = "<asignacion>";
            public const string listAssignable = "<lista-asignable>";
            public const string ExpressionArithmetic = "<expresion-aritmetica>";
            public const string OperatorArithmetic = "<operador-aritmetica>";
            public const string ExpressionRelational = "<expresion-relacional>";
            public const string OperatorRelational = "<operador-relacional>";
            public const string CallFunction = "<llamada-funcion>";
            public const string IdCallFunction = "<id-llamada-funcion>";
            public const string DeclarationFunction = "<declaracion-funcion>";
            public const string typeFunction = "<tipo-funcion>";
            public const string SectionFunction = "<bloque-funcion>";
            public const string Parameter = "<parametro>";
            public const string listParameter = "<lista-parametro>";
            public const string Sentence = "<sentencia>";
            public const string listSentence = "<lista-sentencia>";
            public const string ControllerFlow = "<controlador-flujo>";
            public const string SentenceIf = "<if>";
            public const string SectionIf = "<seccion-if>";
            public const string SentenceElse = "<else>";
            public const string SentenceWhen = "<when>";
            public const string SectionWhen = "<seccion-when>";
            public const string OptionWhen = "<opcion-when>";
            public const string listOptionWhen = "<lista-opcion-when>";
            public const string DefaultWhen = "<default-when>";
            public const string SentenceWhile = "<sentencia-while>";
            public const string SectionWhile = "<bloque-while>";
            public const string SentenceFor = "<for>";
            public const string ParametersFor = "<parametros-for>";
            public const string ParameterFor1 = "<parametros-for-1>";
            public const string ParameterFor2 = "<parametros-for-2>";
            public const string ParameterFor3 = "<parametros-for-3>";
            public const string SectionFor = "<bloque-for>";
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
            public const string String = "String";

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

            public const string SameSame = "==";
            public const string Different = "<>";
            public const string Higher = ">";
            public const string HigherSame = ">=";
            public const string Less = "<";
            public const string LessSame = "<=";

            public const string More = "+";
            public const string Fewer = "-";
            public const string Per = "*";
            public const string Between = "/";
            public const string Module = "%";
            public const string Potency = "^";
            public const string root = "~";

            public const string Dot = ".";
            public const string Coma = ",";
            public const string TwoDots = ":";
            public const string DotComa = ";";
            public const string TwoDotsDouble = "::";
            public const string ParenthesisOpen = "(";
            public const string ParenthesisClose = ")";
            public const string KeyOpen = "{";
            public const string KeyClose = "}";
            public const string Same = "=";
        }

        public static class RegularExpressions
        {
            public const string Comment = "Comentario";
            public const string CommentRegex = "\\/\\*[\\s\\S]*?\\*\\/";
            public const string Id = "id";
            public const string IdRegex = "([a-zA-Z]|_*[a-zA-Z]){1}[a-zA-Z0-9_]*";
            public const string IdAssignable = "id-asignable";
            public const string IdAssignableRegex = "([a-zA-Z]|_*[a-zA-Z]){1}[a-zA-Z0-9_]*";
            public const string Number = "numero";
            public const string RegexNumber = "\\d+[f|d]?(\\.\\d+[f|d]?)?";
            public const string String = "string";
            public const string StringRegex = "\"[^\"]*\"";
        }

        public grammar() : base()
        {
            #region No Terminals
            var Begin = new NonTerminal(NoTerminals.Begin);
            var VariableDeclaration = new NonTerminal(NoTerminals.VariableDeclaration);
            var listVariableDeclaration = new NonTerminal(NoTerminals.listVariableDeclaration);
            var listVariableDeclarationvaluees = new NonTerminal(NoTerminals.listVariableDeclarationvaluees);
            var listVariableDeclarationDynamic = new NonTerminal(NoTerminals.listVariableDeclarationDynamic);
            var DeclarationConstant = new NonTerminal(NoTerminals.DeclarationConstant);
            var listDeclarationConstant = new NonTerminal(NoTerminals.listDeclarationConstant);
            var type = new NonTerminal(NoTerminals.type);
            var Assignment = new NonTerminal(NoTerminals.Assignment);
            var AssignmentSentence = new NonTerminal(NoTerminals.AssignmentSentence);
            var Assignable = new NonTerminal(NoTerminals.Assignable);
            var listAssignable = new NonTerminal(NoTerminals.listAssignable);
            var ExpressionArithmetic = new NonTerminal(NoTerminals.ExpressionArithmetic);
            var OperatorArithmetic = new NonTerminal(NoTerminals.OperatorArithmetic);
            var ExpressionRelational = new NonTerminal(NoTerminals.ExpressionRelational);
            var OperatorRelational = new NonTerminal(NoTerminals.OperatorRelational);
            var CallFunction = new NonTerminal(NoTerminals.CallFunction);
            var idCallFunction = new NonTerminal(NoTerminals.IdCallFunction);
            var DeclarationFunction = new NonTerminal(NoTerminals.DeclarationFunction);
            var typeFunction = new NonTerminal(NoTerminals.typeFunction);
            var SectionFunction = new NonTerminal(NoTerminals.SectionFunction);
            var Parameter = new NonTerminal(NoTerminals.Parameter);
            var listParameter = new NonTerminal(NoTerminals.listParameter);
            var Sentence = new NonTerminal(NoTerminals.Sentence);
            var listSentence = new NonTerminal(NoTerminals.listSentence);
            var ControllerFlow = new NonTerminal(NoTerminals.ControllerFlow);
            var SentenceIf = new NonTerminal(NoTerminals.SentenceIf);
            var SectionIf = new NonTerminal(NoTerminals.SectionIf);
            var SentenceElse = new NonTerminal(NoTerminals.SentenceElse);
            var SentenceWhen = new NonTerminal(NoTerminals.SentenceWhen);
            var SectionWhen = new NonTerminal(NoTerminals.SectionWhen);
            var OptionWhen = new NonTerminal(NoTerminals.OptionWhen);
            var listOptionWhen = new NonTerminal(NoTerminals.listOptionWhen);
            var defaultWhen = new NonTerminal(NoTerminals.DefaultWhen);
            var SentenceWhile = new NonTerminal(NoTerminals.SentenceWhile);
            var SectionWhile = new NonTerminal(NoTerminals.SectionWhile);
            var SentenceFor = new NonTerminal(NoTerminals.SentenceFor);
            var ParametersFor = new NonTerminal(NoTerminals.ParametersFor);
            var ParameterFor1 = new NonTerminal(NoTerminals.ParameterFor1);
            var ParameterFor2 = new NonTerminal(NoTerminals.ParameterFor2);
            var ParameterFor3 = new NonTerminal(NoTerminals.ParameterFor3);
            var SectionFor = new NonTerminal(NoTerminals.SectionFor);
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

            // controladores de flujo
            var if_ = ToTerm(Terminals.If);
            var else_ = ToTerm(Terminals.Else);
            var when_ = ToTerm(Terminals.When);
            var matches_ = ToTerm(Terminals.Matches);
            var default_ = ToTerm(Terminals.Default);
            var while_ = ToTerm(Terminals.While);
            var for_ = ToTerm(Terminals.For);

            // tipos de dato
            var int_ = ToTerm(Terminals.Int);
            var float_ = ToTerm(Terminals.Float);
            var double_ = ToTerm(Terminals.Double);
            var bool_ = ToTerm(Terminals.Bool);
            var string_ = ToTerm(Terminals.String);

            // operadores logicos
            var and_ = ToTerm(Terminals.And);
            var or_ = ToTerm(Terminals.Or);
            var not_ = ToTerm(Terminals.Not);

            // operadores de coincidencia
            var add_ = ToTerm(Terminals.Add);
            var sub_ = ToTerm(Terminals.Sub);
            var mul_ = ToTerm(Terminals.Mul);
            var div_ = ToTerm(Terminals.Div);
            var mod_ = ToTerm(Terminals.Mod);
            var pow_ = ToTerm(Terminals.Pow);
            var roo_ = ToTerm(Terminals.Roo);

            // operadores relacionales
            var SameSame_ = ToTerm(Terminals.Same);
            var Different_ = ToTerm(Terminals.Different);
            var Higher_ = ToTerm(Terminals.Higher);
            var HigherSame_ = ToTerm(Terminals.HigherSame);
            var Less_ = ToTerm(Terminals.Less);
            var LessSame_ = ToTerm(Terminals.LessSame);

            // operadores aritmeticos
            var More_ = ToTerm(Terminals.More);
            var Fewer_ = ToTerm(Terminals.Fewer);
            var Per_ = ToTerm(Terminals.Per);
            var Between_ = ToTerm(Terminals.Between);
            var Module_ = ToTerm(Terminals.Module);
            var Potency_ = ToTerm(Terminals.Potency);
            var root_ = ToTerm(Terminals.root);

            // otros
            var Dot_ = ToTerm(Terminals.Dot);
            var coma_ = ToTerm(Terminals.Coma);
            var TwoDots_ = ToTerm(Terminals.TwoDots);
            var DotComa_ = ToTerm(Terminals.DotComa);
            var TwoDotsDouble_ = ToTerm(Terminals.TwoDotsDouble);
            var ParenthesisOpen_ = ToTerm(Terminals.ParenthesisOpen);
            var ParenthesisClose_ = ToTerm(Terminals.ParenthesisClose);
            var KeyOpen_ = ToTerm(Terminals.KeyOpen);
            var KeyClose_ = ToTerm(Terminals.KeyClose);
            var Same_ = ToTerm(Terminals.Same);
            #endregion

            #region Regex
            var Comment = new RegexBasedTerminal(RegularExpressions.Comment, RegularExpressions.CommentRegex);
            var id = new RegexBasedTerminal(RegularExpressions.Id, RegularExpressions.IdRegex);
            var idAssignable = new RegexBasedTerminal(RegularExpressions.IdAssignable, RegularExpressions.IdAssignableRegex);
            var numero = new RegexBasedTerminal(RegularExpressions.Number, RegularExpressions.RegexNumber);
            var stringRegex = new RegexBasedTerminal(RegularExpressions.String, RegularExpressions.StringRegex);
            #endregion

            #region Production rules

            Begin.Rule =
                DeclarationFunction + Begin |
                typeFunction + Begin |
                typeFunction |
                DeclarationFunction;

            DeclarationFunction.Rule =
                ToTerm("public") + ToTerm("class") + id + SectionFunction; //|
                //ToTerm("public") + ToTerm("static") + ToTerm("void") + ToTerm("main") + ParenthesisOpen_ + string_ + ToTerm("[") + ToTerm("]") + ToTerm("args") + ParenthesisClose_ + SectionFunction;

            //DeclarationFunction.Rule =
                
            //DeclarationFunction.Rule =
                //ToTerm("public") + ToTerm("class") + ToTerm("Main") + KeyOpen_ + ToTerm("public") + ToTerm("static") + ToTerm("void") + ToTerm("main") + ParenthesisOpen_ + string_ + ToTerm("[") + ToTerm("]") + ToTerm("args") + ParenthesisClose_ + SectionFunction + KeyClose_;

            typeFunction.Rule =
                void_ |
                type;

            type.Rule =
                ToTerm("public") +  int_ |
                ToTerm("public") + float_ |
                ToTerm("public") + double_ |
                ToTerm("public") + bool_ |
                ToTerm("public") + string_ |
                ToTerm("private") + int_ |
                ToTerm("private") + float_ |
                ToTerm("private") + double_ |
                ToTerm("private") + bool_ |
                ToTerm("private") + string_ |
                 int_ |
                float_ |
                double_ |
                bool_ |
                string_; 

            SectionFunction.Rule =
                KeyOpen_ + KeyClose_ |
                KeyOpen_ + listSentence + KeyClose_;

            listSentence.Rule =
                Sentence + DotComa_ + listSentence |
                Sentence + DotComa_ |

                ControllerFlow + listSentence |
                ControllerFlow;

            Sentence.Rule =
                VariableDeclaration;

            VariableDeclaration.Rule =
                type + listVariableDeclaration |
                type + listVariableDeclarationvaluees;

            listVariableDeclaration.Rule =
                id + coma_ + listVariableDeclaration |
                id;

            listVariableDeclarationvaluees.Rule =
                Assignment + coma_ + listVariableDeclarationvaluees |
                Assignment;

            Assignment.Rule =
                id + Same_ + Assignable;

            AssignmentSentence.Rule =
                id + Same_ + Assignable;

            Assignable.Rule =
                bool_ |
                idAssignable |
                ExpressionArithmetic |
                stringRegex |
                CallFunction;

            listAssignable.Rule =
                Assignable + coma_ + listAssignable |
                Assignable;

            ExpressionArithmetic.Rule =
                numero |
                id |
                CallFunction |
                ParenthesisOpen_ + ExpressionArithmetic + ParenthesisClose_ |
                ExpressionArithmetic + OperatorArithmetic + ExpressionArithmetic;

            OperatorArithmetic.Rule =
                More_ |
                Fewer_ |
                Per_ |
                Between_ |
                Module_ |
                Potency_ |
                root_;

            ControllerFlow.Rule =
                SentenceIf |
                SentenceWhen |
                SentenceWhile |
                SentenceFor;

            SentenceIf.Rule =
                if_ + ParenthesisOpen_ + ExpressionRelational + ParenthesisClose_ + SectionIf |
                if_ + ParenthesisOpen_ + ExpressionRelational + ParenthesisClose_ + SectionIf + else_ + SentenceIf |
                if_ + ParenthesisOpen_ + ExpressionRelational + ParenthesisClose_ + SectionIf + else_ + SectionIf;

            SectionIf.Rule =
                SectionFunction |
                Sentence + DotComa_ |
                ControllerFlow;

            ExpressionRelational.Rule =
                Assignable + OperatorRelational + Assignable;

            OperatorRelational.Rule =
                Same_ |
                Different_ |
                HigherSame_ |
                Higher_ |
                LessSame_ |
                Less_;

            SentenceWhen.Rule =
                when_ + ParenthesisOpen_ + id + ParenthesisClose_ + SectionWhen;

            SectionWhen.Rule =
                KeyOpen_ + KeyClose_ |
                KeyOpen_ + listOptionWhen + KeyClose_;

            listOptionWhen.Rule =
                OptionWhen + listOptionWhen |
                OptionWhen |
                defaultWhen;

            OptionWhen.Rule =
                matches_ + listAssignable + SectionFunction;

            defaultWhen.Rule =
                default_ + SectionFunction;

            SentenceWhile.Rule =
                while_ + ParenthesisOpen_ + ExpressionRelational + ParenthesisClose_ + SectionWhile |
                while_ + ParenthesisOpen_ + id + ParenthesisClose_ + SectionWhile |
                while_ + ParenthesisOpen_ + bool_ + ParenthesisClose_ + SectionWhile;

            SectionWhile.Rule =
                SectionFunction |
                Sentence + DotComa_ |
                ControllerFlow;

            SentenceFor.Rule =
                for_ + ParenthesisOpen_ + ParametersFor + ParenthesisClose_ + SectionFor;

            ParametersFor.Rule =
                DotComa_ + DotComa_ |
                ParameterFor1 + DotComa_ + DotComa_ |
                DotComa_ + ParameterFor2 + DotComa_ |
                ParameterFor1 + DotComa_ + ParameterFor2 + DotComa_ |
                DotComa_ + DotComa_ + ParameterFor3 |
                ParameterFor1 + DotComa_ + DotComa_ + ParameterFor3 |
                DotComa_ + ParameterFor2 + DotComa_ + ParameterFor3 |
                ParameterFor1 + DotComa_ + ParameterFor2 + DotComa_ + ParameterFor3;

            ParameterFor1.Rule =
                VariableDeclaration;

            ParameterFor2.Rule =
                ExpressionRelational;

            ParameterFor3.Rule =
                Assignment;

            SectionFor.Rule =
                SectionFunction |
                Sentence + DotComa_ |
                ControllerFlow;

            CallFunction.Rule =
                idCallFunction + ParenthesisOpen_ + ParenthesisClose_ |
                idCallFunction + ParenthesisOpen_ + listAssignable + ParenthesisClose_;

            idCallFunction.Rule =
                id + Dot_ + idCallFunction |
                id + TwoDotsDouble_ + idCallFunction |
                id;

            #endregion

            #region Preferences
            Root = Begin;
            #endregion
        }
    }
}

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

                // Si el value del símbolo actual es un id
                if (ValidateRegex(value, Grammar.RegularExpressions.IdRegex) && !ValidateRegex(value, Grammar.RegularExpressions.StringRegex))
                {
                    // Primero, checamos si el identifier existe
                    if (!table.ContainsSymbol(value))
                        return false;

                    // Despues, tenemos que obtener el value de dicho id para comprobar su tipo
                    value = ValueOf(table, value);
                }

                switch (type)
                {
                    case Grammar.Terminals.Int:
                        {
                            if (!ValidateRegex(value, Grammar.RegularExpressions.RegexNumber))
                                return false;

                            if (value.Contains('.'))
                                return false;

                            break;
                        }

                    case Grammar.Terminals.Float:
                        {
                            if (!ValidateRegex(value, Grammar.RegularExpressions.RegexNumber))
                                return false;

                            break;
                        }

                    case Grammar.Terminals.Double:
                        {
                            if (!ValidateRegex(value, Grammar.RegularExpressions.RegexNumber))
                                return false;

                            break;
                        }

                    case Grammar.Terminals.Bool:
                        {
                            if (!value.Equals(Grammar.Terminals.True) || !value.Equals(Grammar.Terminals.False))
                                return false;

                            break;
                        }

                    case Grammar.Terminals.String:
                        {
                            if (!ValidateRegex(value, Grammar.RegularExpressions.StringRegex))
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

            if (ValidateRegex(Symbol.value, Grammar.RegularExpressions.IdRegex) && !ValidateRegex(Symbol.value, Grammar.RegularExpressions.StringRegex))
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
            List<ParseTreeNode> nodes = Tree.Travel(Grammar.NoTerminals.VariableDeclaration);

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

            List<ParseTreeNode> types = Tree.Travel(node, Grammar.NoTerminals.type);
            List<ParseTreeNode> ids = Tree.Travel(node, Grammar.RegularExpressions.Id);
            List<ParseTreeNode> Assignmentes = Tree.Travel(node, Grammar.NoTerminals.Assignable);
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

    class Gramatica : Grammar
    {
        public static class NoTerminales
        {
            public const string Inicio = "<inicio>";
            public const string DeclaracionVariable = "<declaracion-variable>";
            public const string ListaDeclaracionVariable = "<lista-declaracion-variables>";
            public const string ListaDeclaracionVariableValores = "<lista-declaracion-variables-valores>";
            public const string ListaDeclaracionVariableDinamica = "<lista-declaracion-variables-dinamica>";
            public const string DeclaracionConstante = "<declaracion-constante>";
            public const string ListaDeclaracionConstante = "<lista-declaracion-constante>";
            public const string Tipo = "<tipo>";
            public const string Asignacion = "<asignacion>";
            public const string AsignacionSentencia = "<asignacion-sentencia>";
            public const string Asignable = "<asignable>";
            public const string ListaAsignable = "<lista-asignable>";
            public const string ExpresionAritmetica = "<expresion-aritemtica>";
            public const string OperadorAritmetico = "<operador-aritmetico>";
            public const string ExpresionRelacional = "<expresion-relacional>";
            public const string OperadorRelacional = "<operador-relacional>";
            public const string LlamadaFuncion = "<llamada-funcion>";
            public const string IdLlamadaFuncion = "<id-llamada-funcion>";
            public const string DeclaracionFuncion = "<declaracion-funcion>";
            public const string DeclaracionFuncion2 = "<declaracion-funcion2>";
            public const string DeclaracionFuncion3 = "<declaracion-funcion3>";
            public const string DeclaracionFuncion4 = "<declaracion-funcion4>";
            public const string DeclaracionFuncion5 = "<declaracion-funcion5>";
            public const string DeclaracionFuncion6 = "<declaracion-funcion6>";
            public const string DeclaracionFuncion7 = "<declaracion-funcion7>";
            public const string DeclaracionFuncion8 = "<declaracion-funcion8>";
            public const string TipoFuncion = "<tipo-funcion>";
            public const string BloqueFuncion = "<bloque-funcion>";
            public const string Parametro = "<parametro>";
            public const string ListaParametro = "<lista-parametro>";
            public const string ListaParametroImplements = "<lista-parametro-implements>";
            public const string Sentencia = "<sentencia>";
            public const string ListaSentencia = "<lista-sentencia>";
            public const string ControladorFlujo = "<controlador-flujo>";
            public const string SentenciaIf = "<if>";
            public const string BloqueIf = "<bloque-if>";
            public const string SentenciaElse = "<else>";
            public const string SentenciaWhen = "<when>";
            public const string BloqueWhen = "<bloque-when>";
            public const string OpcionWhen = "<opcion-when>";
            public const string ListaOpcionWhen = "<lista-opcion-when>";
            public const string DefaultWhen = "<default-when>";
            public const string SentenciaWhile = "<sentencia-while>";
            public const string BloqueWhile = "<bloque-while>";
            public const string SentenciaFor = "<for>";
            public const string ParametrosFor = "<parametros-for>";
            public const string ParametroFor1 = "<parametro-for-1>";
            public const string ParametroFor2 = "<parametro-for-2>";
            public const string ParametroFor3 = "<parametro-for-3>";
            public const string BloqueFor = "<bloque-for>";
        }

        public static class Terminales
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
            //public const string Iterate = "iterate";

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

            public const string IgualIgual = "==";
            public const string Diferente = "<>";
            public const string Mayor = ">";
            public const string MayorIgual = ">=";
            public const string Menor = "<";
            public const string MenorIgual = "<=";

            public const string Mas = "+";
            public const string Menos = "-";
            public const string Por = "*";
            public const string Entre = "/";
            public const string Modulo = "%";
            public const string Potencia = "^";
            public const string Raiz = "~";

            public const string Punto = ".";
            public const string Coma = ",";
            public const string DosPuntos = ":";
            public const string PuntoComa = ";";
            public const string DosPuntosDoble = "::";
            public const string ParentesisAbrir = "(";
            public const string ParentesisCerrar = ")";
            public const string LlavesAbrir = "{";
            public const string LlavesCerrar = "}";
            public const string Igual = "=";
        }

        public static class ExpresionesRegulares
        {
            public const string Comentario = "comentario";
            public const string ComentarioRegex = "\\/\\*[\\s\\S]*?\\*\\/";
            public const string Id = "id";
            public const string IdRegex = "([a-zA-Z]|_*[a-zA-Z]){1}[a-zA-Z0-9_]*";
            public const string IdAsignable = "id-asignable";
            public const string IdAsignableRegex = "([a-zA-Z]|_*[a-zA-Z]){1}[a-zA-Z0-9_]*";
            public const string Numero = "numero";
            public const string NumeroRegex = "\\d+[f|d]?(\\.\\d+[f|d]?)?";
            public const string String = "string";
            public const string StringRegex = "\"[^\"]*\"";
        }

        public Gramatica() : base()
        {
            #region No terminales
            var inicio = new NonTerminal(NoTerminales.Inicio);
            var declaracionVariable = new NonTerminal(NoTerminales.DeclaracionVariable);
            var listaDeclaracionVariable = new NonTerminal(NoTerminales.ListaDeclaracionVariable);
            var listaDeclaracionVariableValores = new NonTerminal(NoTerminales.ListaDeclaracionVariableValores);
            var listaParametroImplements = new NonTerminal(NoTerminales.ListaParametroImplements);
            var listaDeclaracionVariableDinamica = new NonTerminal(NoTerminales.ListaDeclaracionVariableDinamica);
            var declaracionConstante = new NonTerminal(NoTerminales.DeclaracionConstante);
            var listaDeclaracionConstante = new NonTerminal(NoTerminales.ListaDeclaracionConstante);
            var tipo = new NonTerminal(NoTerminales.Tipo);
            var asignacion = new NonTerminal(NoTerminales.Asignacion);
            var asignacionSentencia = new NonTerminal(NoTerminales.AsignacionSentencia);
            var asignable = new NonTerminal(NoTerminales.Asignable);
            var listaAsignable = new NonTerminal(NoTerminales.ListaAsignable);
            var expresionAritmetica = new NonTerminal(NoTerminales.ExpresionAritmetica);
            var operadorAritmetico = new NonTerminal(NoTerminales.OperadorAritmetico);
            var expresionRelacional = new NonTerminal(NoTerminales.ExpresionRelacional);
            var operadorRelacional = new NonTerminal(NoTerminales.OperadorRelacional);
            //var mathAssignment = new NonTerminal("<math-assignment>");
            //var mathOperator = new NonTerminal("<math-operator>");
            //var mathAssignable = new NonTerminal("<math-assignable>");
            var llamadaFuncion = new NonTerminal(NoTerminales.LlamadaFuncion);
            var idLlamadaFuncion = new NonTerminal(NoTerminales.IdLlamadaFuncion);
            var declaracionFuncion = new NonTerminal(NoTerminales.DeclaracionFuncion);
            var declaracionFuncion2 = new NonTerminal(NoTerminales.DeclaracionFuncion2);
            var declaracionFuncion3 = new NonTerminal(NoTerminales.DeclaracionFuncion3);
            var declaracionFuncion4 = new NonTerminal(NoTerminales.DeclaracionFuncion4);
            var declaracionFuncion5 = new NonTerminal(NoTerminales.DeclaracionFuncion5);
            var declaracionFuncion6 = new NonTerminal(NoTerminales.DeclaracionFuncion6);
            var declaracionFuncion7 = new NonTerminal(NoTerminales.DeclaracionFuncion7);
            var declaracionFuncion8 = new NonTerminal(NoTerminales.DeclaracionFuncion8);
            var tipoFuncion = new NonTerminal(NoTerminales.TipoFuncion);
            var bloqueFuncion = new NonTerminal(NoTerminales.BloqueFuncion);
            var parametro = new NonTerminal(NoTerminales.Parametro);
            var listaParametro = new NonTerminal(NoTerminales.ListaParametro);
            var sentencia = new NonTerminal(NoTerminales.Sentencia);
            var listaSentencia = new NonTerminal(NoTerminales.ListaSentencia);
            var controladorFlujo = new NonTerminal(NoTerminales.ControladorFlujo);
            var sentenciaIf = new NonTerminal(NoTerminales.SentenciaIf);
            var bloqueIf = new NonTerminal(NoTerminales.BloqueIf);
            var sentenciaElse = new NonTerminal(NoTerminales.SentenciaElse);
            var sentenciaWhen = new NonTerminal(NoTerminales.SentenciaWhen);
            var bloqueWhen = new NonTerminal(NoTerminales.BloqueWhen);
            var opcionWhen = new NonTerminal(NoTerminales.OpcionWhen);
            var listaOpcionWhen = new NonTerminal(NoTerminales.ListaOpcionWhen);
            var defaultWhen = new NonTerminal(NoTerminales.DefaultWhen);
            var sentenciaWhile = new NonTerminal(NoTerminales.SentenciaWhile);
            var bloqueWhile = new NonTerminal(NoTerminales.BloqueWhile);
            var sentenciaFor = new NonTerminal(NoTerminales.SentenciaFor);
            var parametrosFor = new NonTerminal(NoTerminales.ParametrosFor);
            var parametroFor1 = new NonTerminal(NoTerminales.ParametroFor1);
            var parametroFor2 = new NonTerminal(NoTerminales.ParametroFor2);
            var parametroFor3 = new NonTerminal(NoTerminales.ParametroFor3);
            var bloqueFor = new NonTerminal(NoTerminales.BloqueFor);
            #endregion

            #region Terminals

            // palabras reservadas
            var void_ = ToTerm(Terminales.Void);
            var return_ = ToTerm(Terminales.Return);
            var var_ = ToTerm(Terminales.Var);
            var const_ = ToTerm(Terminales.Const);
            var null_ = ToTerm(Terminales.Null);
            var true_ = ToTerm(Terminales.True);
            var false_ = ToTerm(Terminales.False);

            // flow controllers
            var if_ = ToTerm(Terminales.If);
            var else_ = ToTerm(Terminales.Else);
            var when_ = ToTerm(Terminales.When);
            var matches_ = ToTerm(Terminales.Matches);
            var default_ = ToTerm(Terminales.Default);
            var while_ = ToTerm(Terminales.While);
            var for_ = ToTerm(Terminales.For);

            // data types
            var int_ = ToTerm(Terminales.Int);
            var float_ = ToTerm(Terminales.Float);
            var double_ = ToTerm(Terminales.Double);
            var bool_ = ToTerm(Terminales.Bool);
            var string_ = ToTerm(Terminales.String);

            // logical operators
            var and_ = ToTerm(Terminales.And);
            var or_ = ToTerm(Terminales.Or);
            var not_ = ToTerm(Terminales.Not);

            // math operators
            var add_ = ToTerm(Terminales.Add);
            var sub_ = ToTerm(Terminales.Sub);
            var mul_ = ToTerm(Terminales.Mul);
            var div_ = ToTerm(Terminales.Div);
            var mod_ = ToTerm(Terminales.Mod);
            var pow_ = ToTerm(Terminales.Pow);
            var roo_ = ToTerm(Terminales.Roo);

            // relational operators
            var igualIgual_ = ToTerm(Terminales.IgualIgual);
            var diferente_ = ToTerm(Terminales.Diferente);
            var mayor_ = ToTerm(Terminales.Mayor);
            var mayorIgual_ = ToTerm(Terminales.MayorIgual);
            var menor_ = ToTerm(Terminales.Menor);
            var menorIgual_ = ToTerm(Terminales.MenorIgual);

            // arithmetic operators
            var mas_ = ToTerm(Terminales.Mas);
            var menos_ = ToTerm(Terminales.Menos);
            var por_ = ToTerm(Terminales.Por);
            var entre_ = ToTerm(Terminales.Entre);
            var modulo_ = ToTerm(Terminales.Modulo);
            var potencia_ = ToTerm(Terminales.Potencia);
            var raiz_ = ToTerm(Terminales.Raiz);

            // others
            var punto_ = ToTerm(Terminales.Punto);
            var coma_ = ToTerm(Terminales.Coma);
            var dosPuntos_ = ToTerm(Terminales.DosPuntos);
            var puntoComa_ = ToTerm(Terminales.PuntoComa);
            var dosPuntosDoble_ = ToTerm(Terminales.DosPuntosDoble);
            var parentesisAbrir_ = ToTerm(Terminales.ParentesisAbrir);
            var parentesisCerrar_ = ToTerm(Terminales.ParentesisCerrar);
            var llavesAbrir_ = ToTerm(Terminales.LlavesAbrir);
            var llavesCerrar_ = ToTerm(Terminales.LlavesCerrar);
            var igual_ = ToTerm(Terminales.Igual);
            #endregion

            #region Regex
            var comentario = new RegexBasedTerminal(ExpresionesRegulares.Comentario, ExpresionesRegulares.ComentarioRegex);
            var id = new RegexBasedTerminal(ExpresionesRegulares.Id, ExpresionesRegulares.IdRegex);
            var idAsignable = new RegexBasedTerminal(ExpresionesRegulares.IdAsignable, ExpresionesRegulares.IdAsignableRegex);
            var numero = new RegexBasedTerminal(ExpresionesRegulares.Numero, ExpresionesRegulares.NumeroRegex);
            var stringRegex = new RegexBasedTerminal(ExpresionesRegulares.String, ExpresionesRegulares.StringRegex);
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

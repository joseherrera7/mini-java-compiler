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
    class sintesis : Grammar
    {

        public static bool analizar(string cadena)
        {
            Gramatica gramatica = new Gramatica();
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(cadena);
            ParseTreeNode raiz = arbol.Root;
            if (raiz == null)
                return false;
            return true;
        }

        public static ParseTreeNode analizarArbol(string cadena)
        {
            Gramatica gramatica = new Gramatica();
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(cadena);
            ParseTreeNode raiz = arbol.Root;
            return arbol.Root;
        }

        public static ParseTree AnalisisSemantico(string entrada)
        {
            var gramatica = new Gramatica();
            var sintactico = new Parser(gramatica);
            return sintactico.Parse(entrada);
        }
    }

    public class SYMBOL
    {
        public string TYPE;
        public string IDENTIFIER;
        public string VALUE;

        public SYMBOL(string Typee, string IDENT)
        {
            this.TYPE = Typee;
            this.IDENTIFIER = IDENT;
        }

        public SYMBOL(string TY, string IDENT, string VAL)
        {
            this.TYPE = TY;
            this.IDENTIFIER = IDENT;
            this.VALUE = VAL;
        }

        public override string ToString()
        {
            return "|||\t\t'" + TYPE + "'\t\t|||\t\t'" + IDENTIFIER + "'\t\t|||\t\t'" + VALUE + "'\t\t|||\t\t";
        }
    }

    public class TableSymbol
    {
        public List<SYMBOL> SYMBOLS = new List<SYMBOL>();

        public TableSymbol() { }

        public void ADDSYMBOL(SYMBOL SYM)
        {
            SYMBOLS.Add(SYM);
        }

        public void REMOVESYMBOL(string IDENTIFIER)
        {
            var index = SYMBOLS.IndexOf(SEARCHSYMBOL(IDENTIFIER));
            SYMBOLS.RemoveAt(index);
        }

        public void ADDSYMBOL(List<SYMBOL> simbolos)
        {
            SYMBOLS.AddRange(simbolos);
        }

        public SYMBOL SEARCHSYMBOL(string IDENTIFIER)
        {
            return SYMBOLS.Find((SYM) => SYM.IDENTIFIER.Equals(IDENTIFIER)) ?? null;
        }

        public bool CONTAINSYMBOL(string IDENTIFIER)
        {
            foreach (SYMBOL simbolo in SYMBOLS)
                if (simbolo.IDENTIFIER.Equals(IDENTIFIER))
                    return true;

            return false;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("TablaSimbolos\n|||\t\tTipo\t\t|||\t\tIdentificador\t\t|||\t\tValor\t\t|||\n");

            foreach (SYMBOL simbolo in SYMBOLS)
            {
                sb.Append(simbolo.ToString()).Append('\n');
            }

            sb.Append(' ');

            return sb.ToString();
        }
    }
    class AnalisisSemantico
    {
        TableSymbol ts = new TableSymbol();
        public void Dfs(ParseTreeNode raiz, List<ParseTreeNode> nodos)
        {
            nodos.Add(raiz);
            raiz.ChildNodes.ForEach(nodo =>
            {
                Dfs(nodo, nodos);
            });
        }

        public List<ParseTreeNode> Dfs(ParseTreeNode raiz)
        {
            List<ParseTreeNode> nodos = new List<ParseTreeNode>();
            Dfs(raiz, nodos);
            return nodos;
        }

        public bool VerificarDuplicados(TableSymbol tabla)
        {
            var contadores = new Dictionary<string, int>();

            foreach (SYMBOL simbolo in tabla.SYMBOLS)
            {
                string id = simbolo.IDENTIFIER;

                if (!contadores.ContainsKey(id))
                    contadores[id] = 0;

                contadores[id] += 1;

                if (contadores[id] > 1)
                    return false;
            }

            return true;
        }

        public Dictionary<string, int> ObtenerDuplicados(TableSymbol tabla)
        {
            var contadores = new Dictionary<string, int>();

            foreach (SYMBOL simbolo in tabla.SYMBOLS)
            {
                string id = simbolo.IDENTIFIER;

                if (!contadores.ContainsKey(id))
                    contadores[id] = 0;

                contadores[id] += 1;

                if (contadores[id] > 1)
                    return contadores;
            }

            return null;
        }

       
        public class Arbol
        {
            private readonly ParseTree arbol;

            public Arbol(ParseTree arbol)
            {
                this.arbol = arbol;
            }

            public bool EsHoja(ParseTreeNode nodo)
            {
                if (nodo.ChildNodes == null)
                    return true;

                return nodo.ChildNodes.Count == 0;
            }

            /// <summary>
            /// Obtener las hojas de la raiz especificada
            /// </summary>
            /// <param name="raiz"></param>
            /// <returns></returns>
            public List<ParseTreeNode> HojasDe(ParseTreeNode raiz)
            {
                var nodos = new List<ParseTreeNode>();
                HojasDe(raiz, nodos);
                return nodos;
            }

            private void HojasDe(ParseTreeNode raiz, List<ParseTreeNode> nodos)
            {
                if (EsHoja(raiz))
                    nodos.Add(raiz);

                raiz.ChildNodes.ForEach(nodo =>
                {
                    HojasDe(nodo, nodos);
                });
            }

            public string ImprimirNodo(ParseTreeNode nodo)
            {
                var sb = new StringBuilder();

                sb
                .Append("Tag                   | ").Append(nodo.Tag).Append('\n')
                .Append("Term                  | ").Append(nodo.Term).Append('\n')
                .Append("Token                 | ").Append(nodo.Token).Append('\n')
                .Append("FindToken()           | ").Append(nodo.FindToken()).Append('\n')
                .Append("FindTokenAndGetText() | ").Append(nodo.FindTokenAndGetText()).Append('\n')
                .Append("ToString()            | ").Append(nodo.ToString());

                return sb.ToString();
            }

            /// <summary>
            /// Obtener todos los nodos del arbol
            /// </summary>
            /// <returns></returns>
            public List<ParseTreeNode> Recorrer()
            {
                var nodos = new List<ParseTreeNode>();
                Recorrer(arbol.Root, nodos);
                return nodos;
            }

            /// <summary>
            /// Obtener todos los nodos del arbol recorriendo desde la raiz especificada
            /// </summary>
            /// <param name="raiz"></param>
            /// <returns></returns>
            public List<ParseTreeNode> Recorrer(ParseTreeNode raiz)
            {
                var nodos = new List<ParseTreeNode>();
                Recorrer(raiz, nodos);
                return nodos;
            }

            private void Recorrer(ParseTreeNode raiz, List<ParseTreeNode> nodos)
            {
                nodos.Add(raiz);
                raiz.ChildNodes.ForEach(nodo =>
                {
                    Recorrer(nodo, nodos);
                });
            }

            /// <summary>
            /// Obtener todos los nodos que conforman una el lado derecho de una asignacion (despues del '=')
            /// </summary>
            /// <param name="raiz"></param>
            /// <returns></returns>
            public List<ParseTreeNode> RecorrerAsignables(ParseTreeNode raiz)
            {
                var nodos = new List<ParseTreeNode>();
                RecorrerAsignables(raiz, nodos);
                return nodos;
            }

            private void RecorrerAsignables(ParseTreeNode raiz, List<ParseTreeNode> nodos)
            {
                nodos.Add(raiz);
                raiz.ChildNodes.ForEach(nodo =>
                {
                    RecorrerAsignables(nodo, nodos);
                });
            }

            /// <summary>
            /// Obtener todos los nodos que pertenezcan al termino especificado
            /// </summary>
            /// <param name="termino"></param>
            /// <returns></returns>
            public List<ParseTreeNode> Recorrer(string termino)
            {
                List<ParseTreeNode> nodos =
                    Recorrer()
                    .FindAll(nodo => nodo.Term.ToString().Equals(termino));

                return nodos;
            }

            /// <summary>
            /// Obtener todos los nodos desde recorriendo desde la raiz especificada y que pertenezcan al termino especificado
            /// </summary>
            /// <param name="raiz"></param>
            /// <param name="termino"></param>
            /// <returns></returns>
            public List<ParseTreeNode> Recorrer(ParseTreeNode raiz, string termino)
            {
                List<ParseTreeNode> nodos =
                    Recorrer(raiz)
                    .FindAll(nodo => nodo.Term.ToString().Equals(termino));

                return nodos;
            }

            public TokenList Tokens()
            {
                return arbol.Tokens;
            }

            public override string ToString()
            {
                var sb = new StringBuilder();

                Recorrer().ForEach(node =>
                {
                    sb.Append(node.ToString()).Append("\n");
                });

                return sb.ToString();
            }
        }

        public void analisisSemantico(string Text)
        {
            ParseTree parseTree = sintesis.AnalisisSemantico(Text);

            if (parseTree.Root == null)
            {
                MessageBox.Show("Error sintactico");
                
                return;
            }

            // hacer arbol
            var arbol = new Arbol(parseTree);
            //instrucciones(parseTree.Root.ChildNodes.ElementAt(0));


            // verificar tabla de simbolos
            ts = GenerarTablaSimbolos(arbol);

            bool duplicados = VerificarDuplicados(ts);
            Dictionary<string, int> cont = ObtenerDuplicados(ts);

            if (duplicados == false)
            {
                for (int i = 0; i < cont.Count; i++)
                {
                    if (cont.ToList()[i].Value > 1)
                    {
                        MessageBox.Show("Variables duplicadas y declaradas con el mismo nombre " + cont.ToList()[i].Key);
                    }
                }
            }

            bool tipos = VerficarTipos(ts);

            if (tipos == false)
            {
                MessageBox.Show("Error en conversión de tipo");
            }

            var sb = new StringBuilder();

            MessageBox.Show(ts.ToString());
            foreach (var s in ts.SYMBOLS)
            {
                //agregar la tabla de simbolos
                
            }

        }

        public void instrucciones(ParseTreeNode actual)
        {
            if (actual.ChildNodes.Count == 2)
            {
                instruccion(actual.ChildNodes.ElementAt(0));
                instrucciones(actual.ChildNodes.ElementAt(1));
            }
            else
            {
                instruccion(actual.ChildNodes.ElementAt(0));
            }
        }

        public void instruccion(ParseTreeNode actual)
        {
            System.Diagnostics.Debug.WriteLine("El valor de la expresion es: " + expresion(actual.ChildNodes.ElementAt(2)));
        }


        public double expresion(ParseTreeNode actual)
        {
            if (actual.ChildNodes.Count == 3)
            {
                string tokenOperador = actual.ChildNodes.ElementAt(1).ToString().Split(' ')[0];
                switch (tokenOperador)
                {
                    case "+":
                        return expresion(actual.ChildNodes.ElementAt(0)) + expresion(actual.ChildNodes.ElementAt(2));
                    case "-":
                        return expresion(actual.ChildNodes.ElementAt(0)) - expresion(actual.ChildNodes.ElementAt(2));
                    case "*":
                        return expresion(actual.ChildNodes.ElementAt(0)) * expresion(actual.ChildNodes.ElementAt(2));
                    case "/":
                        return expresion(actual.ChildNodes.ElementAt(0)) / expresion(actual.ChildNodes.ElementAt(2));
                    default:
                        return expresion(actual.ChildNodes.ElementAt(1));
                }

            }
            else if (actual.ChildNodes.Count == 2)
            {
                return -1 * expresion(actual.ChildNodes.ElementAt(1));
            }
            else
            {
                return Double.Parse(actual.ChildNodes.ElementAt(0).ToString().Split(' ')[0]);
            }
        }

        public TableSymbol GenerarTablaSimbolos(Arbol arbol)
        {
            var tabla = new TableSymbol();
            List<ParseTreeNode> nodos = arbol.Recorrer(Gramatica.NoTerminales.DeclaracionVariable);

            foreach (ParseTreeNode nodo in nodos)
            {
                MessageBox.Show(arbol.ImprimirNodo(nodo));
                List<SYMBOL> simbolos = CrearSimbolos(arbol, nodo);
                tabla.ADDSYMBOL(simbolos);
            }

            return tabla;
        }

        public bool ValidarRegex(string cadena, string regex)
        {
            Match validacion = Regex.Match(cadena, regex);
            return validacion.Success;
        }


        private string ValorDe(TableSymbol tabla, string id)
        {
            Console.WriteLine("/ = / = / = / = / = / = / = / = / = / = / = / = / = / = / = / = / = / = / = / = / = /");
            Console.WriteLine($"Checando tipo del id '{id}'");

            SYMBOL simbolo = tabla.SEARCHSYMBOL(id);

            Console.WriteLine($"El valor del id actual '{id}' es '{simbolo.VALUE}'");

            if (id.Equals(simbolo.VALUE))
                return null;

            if (simbolo.VALUE == null)
                return null;

            /*if (ValidarRegex(simbolo.Valor, Gramatica.ExpresionesRegulares.IdRegex) && !ValidarRegex(simbolo.Valor, Gramatica.ExpresionesRegulares.StringRegex))
            {
                Console.WriteLine("Recursando...");
                return ValorDe(tabla, simbolo.Valor);
            }*/

            Console.WriteLine($"Se encontro el tipo del id '{id}'. Tipo es '{simbolo.TYPE}'");
            return simbolo.VALUE;
        }

        public bool VerficarTipos(TableSymbol tabla)
        {
            foreach (SYMBOL simbolo in tabla.SYMBOLS)
            {
                string tipo = simbolo.TYPE;
                string valor = simbolo.VALUE;
                string id = simbolo.IDENTIFIER;

                if (valor == null)
                    continue;

                // Si el valor del simbolo actual es un id
                if (ValidarRegex(valor, Gramatica.ExpresionesRegulares.IdRegex) && !ValidarRegex(valor, Gramatica.ExpresionesRegulares.StringRegex))
                {
                    // Primero, checamos si el identificador existe
                    if (!tabla.CONTAINSYMBOL(id))
                        return false;

                    // Despues, tenemos que obtener el valor de dicho id para comprobar su tipo
                    valor = ValorDe(tabla, id);
                }

                switch (tipo)
                {
                    case Gramatica.Terminales.Int:
                        {
                            if (!ValidarRegex(valor, Gramatica.ExpresionesRegulares.NumeroRegex))
                                return false;

                            if (valor.Contains('.'))
                                return false;

                            break;
                        }

                    case Gramatica.Terminales.Float:
                        {
                            if (!ValidarRegex(valor, Gramatica.ExpresionesRegulares.NumeroRegex))
                                return false;

                            break;
                        }

                    case Gramatica.Terminales.Double:
                        {
                            if (!ValidarRegex(valor, Gramatica.ExpresionesRegulares.NumeroRegex))
                                return false;

                            break;
                        }

                    case Gramatica.Terminales.Bool:
                        {
                            if (!valor.Equals(Gramatica.Terminales.True) && !valor.Equals(Gramatica.Terminales.False))
                                return false;

                            break;
                        }

                    case Gramatica.Terminales.String:
                        {
                            if (!ValidarRegex(valor, Gramatica.ExpresionesRegulares.StringRegex))
                                return false;

                            break;
                        }
                }
            }

            return true;
        }

        private List<SYMBOL> CrearSimbolos(Arbol arbol, ParseTreeNode nodo)
        {
            var simbolos = new List<SYMBOL>();

            List<ParseTreeNode> tipos = arbol.Recorrer(nodo, Gramatica.NoTerminales.Tipo);
            List<ParseTreeNode> ids = arbol.Recorrer(nodo, Gramatica.ExpresionesRegulares.Id);
            List<ParseTreeNode> asignaciones = arbol.Recorrer(nodo, Gramatica.NoTerminales.Asignable);
            var listaAsignables = new List<List<ParseTreeNode>>();

            

            asignaciones.ForEach(node =>
            {
                List<ParseTreeNode> hojas = arbol.HojasDe(node);
                listaAsignables.Add(hojas);
            });

            // Create symbols
            
            for (int i = 0; i < 1; i++)
            {
                string tipo = "";

                if (tipos[0].ChildNodes.Count > 1)
                {
                    tipo = tipos[0].FindTokenAndGetText() + " " + tipos[0].ChildNodes[1];
                }
                else
                {
                    tipo = tipos[0].FindTokenAndGetText();
                }


                string id = ids[i].FindTokenAndGetText();

                if (listaAsignables.Count == 0)
                    simbolos.Add(new SYMBOL(tipo, id));

                else
                {
                    var sb = new StringBuilder();

                    listaAsignables[0].ForEach(token =>
                    {
                        sb.Append($"{token.FindTokenAndGetText()} ");
                    });

                    string asignable = sb.ToString().Trim();



                    simbolos.Add(new SYMBOL(tipo, id, asignable));
                }
            }

            return simbolos;
        }
    }

    class Gramatica : Grammar
    {
        public static class NoTerminales
        {
            public const string Inicio = "<inicio>";
            public const string DeclaracionVariable = "<declaracion-variable>";
            public const string ListaDeclaracionVariable = "<lista-declaracion-variables>";
            public const string ListaDeclaracionFuncion = "<lista-declaracion-funcion>";
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

            //RegisterOperators(1, Mas, Menos);

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
            var ListaDeclaracionFuncion = new NonTerminal(NoTerminales.ListaDeclaracionFuncion);
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
                listaSentencia + inicio |
                listaSentencia;

            declaracionFuncion.Rule =
                ToTerm("public") + ToTerm("class") + id + llavesAbrir_ + ToTerm("public") + ToTerm("static") + ToTerm("void") + ToTerm("main") + parentesisAbrir_ + string_ + ToTerm("[") + ToTerm("]") + ToTerm("args") + parentesisCerrar_ + bloqueFuncion + llavesCerrar_ |
                tipoFuncion + id + parentesisAbrir_ + parentesisCerrar_ + bloqueFuncion |
                ToTerm("public") + ToTerm("static") + ToTerm("void") + ToTerm("main") + parentesisAbrir_ + string_ + ToTerm("[") + ToTerm("]") + ToTerm("args") + parentesisCerrar_ + bloqueFuncion |
                tipoFuncion + id + parentesisAbrir_ + listaParametro + parentesisCerrar_ + bloqueFuncion |
                ToTerm("class") + id + bloqueFuncion |
                ToTerm("interface") + id + bloqueFuncion |
                ToTerm("class") + id + ToTerm("extends") + id + ToTerm("implements") + listaParametroImplements + bloqueFuncion |
                ToTerm("class") + id + ToTerm("extends") + id + bloqueFuncion |
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

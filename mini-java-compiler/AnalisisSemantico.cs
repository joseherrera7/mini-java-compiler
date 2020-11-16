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
        SymbolTableS ts = new SymbolTableS();
        public static ParseTree Analize(string entrada)
        {
            var gramatica = new Gramatica();
            var sintactico = new Parser(gramatica);

            return sintactico.Parse(entrada);
        }

        public bool ValidateRegex(string cadena, string regex)
        {
            Match validacion = Regex.Match(cadena, regex);
            return validacion.Success;
        }
        public bool CheckDup(SymbolTableS tabla)
        {
            var contadores = new Dictionary<string, int>();

            foreach (Simbolo simbolo in tabla.Simbolos)
            {
                string id = simbolo.Id;

                if (!contadores.ContainsKey(id))
                    contadores[id] = 0;

                contadores[id] += 1;

                if (contadores[id] > 1)
                    return false;
            }

            return true;
        }

        public bool CheckType(SymbolTableS tabla)
        {
            foreach (Simbolo simbolo in tabla.Simbolos)
            {
                string tipo = simbolo.Tipo;
                string valor = simbolo.Valor;

                if (valor == null)
                    continue;

                // Si el valor del simbolo actual es un id
                if (ValidateRegex(valor, Gramatica.ExpresionesRegulares.IdRegex) && !ValidateRegex(valor, Gramatica.ExpresionesRegulares.StringRegex))
                {
                    // Primero, checamos si el identificador existe
                    if (!tabla.ContieneSimbolo(valor))
                        return false;

                    // Despues, tenemos que obtener el valor de dicho id para comprobar su tipo
                    valor = ValueOf(tabla, valor);
                }

                switch (tipo)
                {
                    case Gramatica.Terminales.Int:
                        {
                            if (!ValidateRegex(valor, Gramatica.ExpresionesRegulares.NumeroRegex))
                                return false;

                            if (valor.Contains('.'))
                                return false;

                            break;
                        }

                    case Gramatica.Terminales.Float:
                        {
                            if (!ValidateRegex(valor, Gramatica.ExpresionesRegulares.NumeroRegex))
                                return false;

                            break;
                        }

                    case Gramatica.Terminales.Double:
                        {
                            if (!ValidateRegex(valor, Gramatica.ExpresionesRegulares.NumeroRegex))
                                return false;

                            break;
                        }

                    case Gramatica.Terminales.Bool:
                        {
                            if (!valor.Equals(Gramatica.Terminales.True) || !valor.Equals(Gramatica.Terminales.False))
                                return false;

                            break;
                        }

                    case Gramatica.Terminales.String:
                        {
                            if (!ValidateRegex(valor, Gramatica.ExpresionesRegulares.StringRegex))
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
                MessageBox.Show("Error sintactico");
                return;
            }

            // hacer arbol
            var arbol = new Arbol(parseTree);

            // checar tabla de simbolos
            ts = GenerateTable(arbol);

            bool duplicados = CheckDup(ts);

            if (duplicados == false)
            {
                MessageBox.Show("Variables duplicadas");
                return;
            }

            bool tipos = CheckType(ts);

            if (tipos == false)
            {
                MessageBox.Show("Error de tipo");
                return;
            }

            

            var sb = new StringBuilder();

            foreach (var s in ts.Simbolos)
            {
                sb.Append(s.ToString()).Append('\n');
            }

            MessageBox.Show(sb.ToString());
        }

        private string ValueOf(SymbolTableS tabla, string id)
        {
            Console.WriteLine("/ = / = / = / = / = / = / = / = / = / = / = / = / = / = / = / = / = / = / = / = / = /");
            Console.WriteLine($"Checando tipo del id '{id}'");

            Simbolo simbolo = tabla.BuscarSimbolo(id);

            Console.WriteLine($"El valor del id actual '{id}' es '{simbolo.Valor}'");

            if (id.Equals(simbolo.Valor))
                return null;

            if (simbolo.Valor == null)
                return null;

            if (ValidateRegex(simbolo.Valor, Gramatica.ExpresionesRegulares.IdRegex) && !ValidateRegex(simbolo.Valor, Gramatica.ExpresionesRegulares.StringRegex))
            {
                Console.WriteLine("Recursando...");
                return ValueOf(tabla, simbolo.Valor);
            }

            Console.WriteLine($"Se encontro el tipo del id '{id}'. Tipo es '{simbolo.Tipo}'");
            return simbolo.Valor;
        }

        public SymbolTableS GenerateTable(Arbol arbol)
        {
            var tabla = new SymbolTableS();
            List<ParseTreeNode> nodos = arbol.Recorrer(Gramatica.NoTerminales.DeclaracionVariable);

            foreach (ParseTreeNode nodo in nodos)
            {
                List<Simbolo> simbolos = CreateSymbol(arbol, nodo);
                tabla.AgregarSimbolos(simbolos);
            }

            return tabla;
        }

        private List<Simbolo> CreateSymbol(Arbol arbol, ParseTreeNode nodo)
        {
            var simbolos = new List<Simbolo>();

            List<ParseTreeNode> tipos = arbol.Recorrer(nodo, Gramatica.NoTerminales.Tipo);
            List<ParseTreeNode> ids = arbol.Recorrer(nodo, Gramatica.ExpresionesRegulares.Id);
            List<ParseTreeNode> asignaciones = arbol.Recorrer(nodo, Gramatica.NoTerminales.Asignable);
            var listaAsignables = new List<List<ParseTreeNode>>();

            asignaciones.ForEach(node =>
            {
                List<ParseTreeNode> hojas = arbol.HojasDe(node);
                listaAsignables.Add(hojas);
            });

            // Crear simbolos
            for (int i = 0; i < ids.Count; i++)
            {
                string tipo = tipos[0].FindTokenAndGetText();
                string id = ids[i].FindTokenAndGetText();

                if (listaAsignables.Count == 0)
                    simbolos.Add(new Simbolo(tipo, id));

                else
                {
                    var sb = new StringBuilder();

                    listaAsignables[i].ForEach(token =>
                    {
                        sb.Append($"{token.FindTokenAndGetText()} ");
                    });

                    string asignable = sb.ToString().Trim();
                    simbolos.Add(new Simbolo(tipo, id, asignable));
                }
            }

            return simbolos;
        }

    }

    public class SymbolTableS
    {
        public List<Simbolo> Simbolos = new List<Simbolo>();

        public SymbolTableS() { }

        public void AgregarSimbolo(Simbolo simbolo)
        {
            Simbolos.Add(simbolo);
        }

        public void AgregarSimbolos(List<Simbolo> simbolos)
        {
            Simbolos.AddRange(simbolos);
        }

        public Simbolo BuscarSimbolo(string identificador)
        {
            return Simbolos.Find((simbolo) => simbolo.Id.Equals(identificador)) ?? null;
        }

        public bool ContieneSimbolo(string identificador)
        {
            foreach (Simbolo simbolo in Simbolos)
                if (simbolo.Id.Equals(identificador))
                    return true;

            return false;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("TablaSimbolos{\n");

            foreach (Simbolo simbolo in Simbolos)
            {
                sb.Append('\t').Append(simbolo.ToString()).Append('\n');
            }

            sb.Append('}');

            return sb.ToString();
        }
    }

    public class Simbolo
    {
        public string Tipo;
        public string Id;
        public string Valor;

        public Simbolo(string tipo, string identificador)
        {
            this.Tipo = tipo;
            this.Id = identificador;
        }

        public Simbolo(string tipo, string identificador, string valor)
        {
            this.Tipo = tipo;
            this.Id = identificador;
            this.Valor = valor;
        }

        public override string ToString()
        {
            return "Simbolo{'" + Tipo + "', '" + Id + "', '" + Valor + "'}";
        }
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
            public const string TipoFuncion = "<tipo-funcion>";
            public const string BloqueFuncion = "<bloque-funcion>";
            public const string Parametro = "<parametro>";
            public const string ListaParametro = "<lista-parametro>";
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
                declaracionFuncion;

            /*declaracionFuncion.Rule =
                tipoFuncion + id + parentesisAbrir_ + parentesisCerrar_ + bloqueFuncion;*/

            // TODO Esta regla si funciona
            /*declaracionFuncion.Rule =
                ToTerm("public") + ToTerm("static") + ToTerm("void") + ToTerm("main") + parentesisAbrir_ + string_ + ToTerm("[") + ToTerm("]") + ToTerm("args") + parentesisCerrar_ + bloqueFuncion;*/

            declaracionFuncion.Rule =
                ToTerm("public") + ToTerm("class") + ToTerm("Main") + llavesAbrir_ + ToTerm("public") + ToTerm("static") + ToTerm("void") + ToTerm("main") + parentesisAbrir_ + string_ + ToTerm("[") + ToTerm("]") + ToTerm("args") + parentesisCerrar_ + bloqueFuncion + llavesCerrar_;

            tipoFuncion.Rule =
                void_ |
                tipo;

            tipo.Rule =
                int_ |
                float_ |
                double_ |
                bool_ |
                string_;

            bloqueFuncion.Rule =
                llavesAbrir_ + llavesCerrar_ |
                llavesAbrir_ + listaSentencia + llavesCerrar_;

            listaSentencia.Rule =
                sentencia + puntoComa_ + listaSentencia |
                sentencia + puntoComa_ |

                controladorFlujo + listaSentencia |
                controladorFlujo;

            sentencia.Rule =
                declaracionVariable;

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
                id + igual_ + asignable;

            asignacionSentencia.Rule =
                id + igual_ + asignable;

            asignable.Rule =
                bool_ |
                idAsignable |
                expresionAritmetica |
                stringRegex |
                llamadaFuncion;

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace mini_java_compiler
{
    class Reader
    {
        //INCLUÍR MÁS OPERADORES SI ME HIZO FALTA ALGUNO
        private string[] operadores = { "+","-","*", "/", "%", "<", "<=", ">", ">=", "=", "==", "!=", "&&", "||", "!", ";", ",", ".", "[", "]",
            "(", ")", "{", "}",
    "[]", "()", "{}" };
        private int columnCounter = 0;
        //GUARDA EL ERROR
        public int ERROR = 0;
        private string writer = String.Empty;
        public bool esMultilinea = false;
        private bool cadenaAbrir = false;
        private string errores = String.Empty;

        private Dictionary<string, string> tokens = new Dictionary<string, string>();

        private string[] reserved = { "void", "int", "double", "boolean",
                "string", "class", "const", "interface", "null", "this",
                "extends", "implements", "for", "while", "if", "else",
                "return", "break", "New", "System", "out", "println" };

        /// <summary>
        /// Read Line per line the file, and this calls another methods to confirm
        /// </summary>
        /// <param name="line">the line</param>
        public void ReadProgram(string line, int lineNumber)
        {
            columnCounter = 1;
            int columnStart = 0;
            int columnEnd = 0;
            string buffer = "";
            foreach (var c in line)
            {
                if (buffer.Equals("/") && c.Equals('*'))
                {
                    esMultilinea = true;
                    buffer += c;
                }
                else if (buffer.Equals("*") && c.Equals('/') && esMultilinea && !CadenaAbrir)
                {
                    esMultilinea = false;
                }
                else if ((operadores.Contains(c.ToString()) || operadores.Contains(buffer)) && !c.Equals('/') && !CadenaAbrir && !buffer.Equals("\"")) //verfica que sea un operador
                {
                    if (line.Length == columnCounter)
                    {
                        if (operadores.Contains(c.ToString()) && buffer.Equals(""))
                        {
                            AddOperator(c.ToString(), columnCounter, columnCounter, lineNumber);
                        }
                        else if (operadores.Contains(c.ToString()) && !buffer.Equals("") && !operadores.Contains(buffer + c))
                        {
                            if (operadores.Contains(c.ToString()) && operadores.Contains(buffer))
                            {
                                AddOperator(buffer, columnStart, columnCounter - 1, lineNumber);
                                AddOperator(c.ToString(), columnCounter, columnCounter, lineNumber);
                                buffer = "";
                            }
                            else
                            {
                                columnEnd = columnCounter - 1;
                                AddTokens(buffer, columnStart, columnEnd, lineNumber);
                                AddOperator(c.ToString(), columnCounter, columnCounter, lineNumber);
                                columnStart = columnCounter;
                                buffer = "";
                            }
                        }
                        else
                        {
                            if (operadores.Contains(buffer + c))
                            {
                                AddOperator(buffer + c, columnCounter, columnCounter, lineNumber);
                                buffer = "";
                            }
                            else
                            {
                                columnEnd = columnCounter - 1;
                                AddTokens(c.ToString(), columnStart, columnCounter, lineNumber);
                            }

                        }
                    }
                    else
                    {
                        if (operadores.Contains(buffer)) // si el operador está en el bufer 
                        {
                            if (operadores.Contains(buffer + c)) // prueba si es un operador doble
                            {
                                if (buffer.Contains("")) // si está vacio mete el operador e inicializa columna
                                {
                                    columnStart = columnCounter;
                                    buffer += c;
                                }
                                else
                                {
                                    buffer += c;
                                }
                            }
                            else if (operadores.Contains(c.ToString()) && operadores.Contains(buffer))
                            {

                                AddOperator(buffer, columnCounter - 1, columnCounter - 1, lineNumber);
                                AddOperator(c.ToString(), columnCounter, columnCounter, lineNumber);
                                buffer = "";
                            }
                            else // sino es operador doble lo mete a los tokens el operador del buffer y continua
                            {
                                AddOperator(buffer, columnCounter, columnCounter, lineNumber);
                                columnStart = columnCounter;
                                buffer = c.ToString();
                            }
                        }
                        else // sino es sigue
                        {
                            if (buffer.Equals("")) // sino hay nada en el bufer sigue
                            {
                                columnStart = columnCounter;
                                buffer = c.ToString();
                            }
                            else //si el buffer es el toro vuelve a llamar si es una reservada
                            {
                                columnEnd = columnCounter - 1;
                                AddTokens(buffer, columnStart, columnEnd, lineNumber);
                                buffer = c.ToString();
                                columnStart = columnCounter;
                            }
                        }
                    }
                }
                /*else if (buffer.Equals("\"\""))
                {
                    AddTokens(buffer, columnStart, columnCounter - 1, lineNumber);
                    columnStart = columnCounter;
                    buffer = c.ToString();
                }*/
                else if (buffer.Equals("\"") || c.Equals('\"'))
                {
                    if (buffer.Equals("") && c.Equals('\"'))
                    {
                        buffer = c.ToString();
                        columnStart = columnCounter;
                    }
                    else if (line.Length == columnCounter)
                    {
                        CadenaAbrir = !CadenaAbrir;
                        AddTokens(buffer + c, columnStart, columnCounter, lineNumber);
                        buffer = "";
                    }
                    else
                    {
                        CadenaAbrir = !CadenaAbrir;
                        buffer += c;
                    }
                }
                else if (buffer.Equals("/") && !CadenaAbrir) //verfica que es un comentario y se lo salta
                {
                    if (c.Equals('/'))
                    {
                        return;
                    }
                }
                else if (!BufferEsCorrecto(buffer) && !operadores.Contains(buffer) && !esMultilinea && !CadenaAbrir)
                {
                    AddTokens(buffer, columnCounter-1, columnCounter-1, lineNumber);
                    buffer = c.ToString();
                    columnStart = columnCounter;
                }
                else if (Char.IsWhiteSpace(c) && !CadenaAbrir) // ignore blankspaces
                {
                    if (!buffer.Equals("") && !operadores.Contains(buffer))
                    {
                        columnEnd = columnCounter - 1;
                        AddTokens(buffer, columnStart, columnEnd, lineNumber);
                        buffer = "";
                    }
                    else if (operadores.Contains(buffer))
                    {
                        columnEnd = columnCounter - 1;
                        AddOperator(buffer, columnStart, columnEnd, lineNumber);
                        buffer = "";
                    }
                }
                
                /*else if ()
                {

                }*/
                else // mete el simbolo sino detecta un token
                {
                    if (buffer.Equals("")) // inicia el contador de columna
                    {
                        if (line.Length == columnCounter)
                        {
                            if (operadores.Contains(c.ToString()))
                            {
                                AddOperator(c.ToString(), columnCounter-1, columnCounter-1, lineNumber);
                                AddOperator(c.ToString(), columnCounter, columnCounter, lineNumber);
                            }
                            else
                            {
                                AddTokens(c.ToString(), columnCounter, columnCounter, lineNumber);
                            }
                        }
                        else
                        {
                            buffer += c;
                            columnStart = columnCounter;
                        }
                    }
                    else if (operadores.Contains(buffer))
                    {
                        if (operadores.Contains(buffer + c)) // prueba si es un operador doble
                        {
                            if (buffer.Contains("")) // si está vacio mete el operador e inicializa columna
                            {
                                columnStart = columnCounter;
                                buffer += c;
                            }
                            else
                            {
                                buffer += c;
                            }
                        }
                        else // sino es operador doble lo mete a los tokens el operador del buffer y continua
                        {
                            if (line.Length == columnCounter)
                            {
                                if (operadores.Contains(c.ToString()) && operadores.Contains(buffer))
                                {
                                    AddOperator(buffer, columnCounter - 1, columnCounter - 1, lineNumber);
                                    AddOperator(c.ToString(), columnCounter, columnCounter, lineNumber);
                                    buffer = "";
                                }
                                else if (operadores.Contains(c.ToString()) && !operadores.Contains(buffer))
                                {
                                    AddTokens(buffer, columnStart, columnCounter - 1, lineNumber);
                                    AddOperator(c.ToString(), columnCounter, columnCounter, lineNumber);
                                    buffer = "";
                                }
                                else if (!operadores.Contains(c.ToString()) && operadores.Contains(buffer))
                                {
                                    AddOperator(buffer, columnStart, columnCounter - 1, lineNumber);
                                    AddTokens(c.ToString(), columnCounter, columnCounter, lineNumber);                                    
                                    buffer = "";
                                }
                                else
                                {
                                    AddTokens(buffer, columnStart, columnCounter - 1, lineNumber);
                                    AddTokens(c.ToString(), columnCounter, columnCounter, lineNumber);
                                    buffer = "";
                                }
                            }
                            else
                            {
                                columnStart = columnCounter;
                                columnEnd = columnCounter;
                                AddOperator(buffer, columnStart, columnEnd, lineNumber);
                                buffer = "";
                            }
                        }
                    }
                    else
                    {
                        if (columnCounter == line.Length)
                        {
                            AddTokens(buffer+c, columnStart, columnCounter, lineNumber);
                            buffer = "";
                        }
                        else
                        {
                            buffer += c;
                        }  
                    }
                }
                columnCounter++;
            }
        }

        /// <summary>
        /// Method to organice what is the token
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private int Segmentation(string buffer, int lineNumber)
        {
            var idPattern = "^[A-Za-z_$]{1}[a-zA-Z\\d$_]*$";
            var constBooleana = "true|false";
            var constEntera = "(0[xX][0-9a-fA-F]+)|([0-9]+)";
            var consDouble = "\\d+\\.(\\d*(E\\+\\d?))";
            var consString = "\".*\"";
            string cadenaInvalida = "[\0\r\n\\\"]";
            string cadenaSinTerminar = "\\\"(.*)?"; // string sin terminar 
            if (reserved.Contains(buffer)) return 1;
            else if (buffer == "") return 7;
            else
            {
                if (Regex.IsMatch(buffer, constBooleana))
                {
                    return 3;
                }
                else if (Regex.IsMatch(buffer, idPattern))
                {
                    return 2;
                }
                else if (Regex.IsMatch(buffer, constEntera))
                {
                    return 4;
                }
                else if (Regex.IsMatch(buffer, consDouble))
                {
                    return 5;
                }
                else if (Regex.IsMatch(buffer, consString) || Regex.IsMatch(buffer, cadenaSinTerminar)/*Regex.IsMatch(buffer, consString)*/)
                {                
                    return VerificarCadena(buffer, consString, cadenaInvalida, cadenaSinTerminar, lineNumber);
                }
                else
                {
                    return 0;
                }
            }
        }
        private int Segmentation2(string buffer)
        {
            var idPattern = "^[A-Za-z_$]{1}[a-zA-Z\\d$_]*$";
            var constBooleana = "true|false";
            var constEntera = "(0[xX][0-9a-fA-F]+)|([0-9]+)";
            var consDouble = "\\d+\\.(\\d*(E\\+\\d?))";
            var consString = "\".*\"";
            string cadenaInvalida = "[\0\r\n\\\"]";
            string cadenaSinTerminar = "\\\"(.*)?"; // string sin terminar 
            if (reserved.Contains(buffer)) return 1;
            else if (buffer == "") return 7;
            else
            {
                if (Regex.IsMatch(buffer, constBooleana))
                {
                    return 3;
                }
                else if (Regex.IsMatch(buffer, idPattern))
                {
                    return 2;
                }
                else if (Regex.IsMatch(buffer, constEntera))
                {
                    return 4;
                }
                else if (Regex.IsMatch(buffer, consDouble))
                {
                    return 5;
                }
                else if (Regex.IsMatch(buffer, consString) || Regex.IsMatch(buffer, cadenaSinTerminar)/*Regex.IsMatch(buffer, consString)*/)
                {
                    return VerificarCadena2(buffer, consString, cadenaInvalida, cadenaSinTerminar);
                }
                else
                {
                    return 0;
                }
            }
        }
        private int VerificarCadena2(string buffer, string consString, string cadenaInvalida, string cadenaSinTerminar)
        {
            int start = buffer.IndexOf(buffer) + 1;

            if (Regex.IsMatch(buffer, consString))
            {
                string test = buffer.Substring(1, buffer.Length - 2);
                if (!Regex.IsMatch(test, cadenaInvalida))
                {
                    return 6;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                if (Regex.IsMatch(buffer, cadenaSinTerminar))
                {
                    return 0;
                }
                else
                {
                    return 0;
                }
            }
        }
        private int VerificarCadena(string buffer, string consString, string cadenaInvalida, string cadenaSinTerminar, int lineNumber)
        {
            int start = buffer.IndexOf(buffer) + 1;

            if (Regex.IsMatch(buffer,consString))
            {
                string test = buffer.Substring(1, buffer.Length - 2);
                if (!Regex.IsMatch(test, cadenaInvalida))
                {
                    return 6;
                }
                else
                {
                    errores += "*** ERROR: Cadena invalida *** en linea: " + lineNumber + " \n";
                    writer += "*** ERROR: Cadena invalida ***  ";
                    return 0;
                }
            } 
            else
            {
                if (Regex.IsMatch(buffer, cadenaSinTerminar))
                {
                    errores += "*** ERROR: Cadena sin terminar*** en linea: " +  lineNumber +" \n";
                    writer += "*** ERROR: Cadena sin terminar***  ";
                    return 0;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// Mete los tokens al diccionario de tokens
        /// </summary>
        /// <param name="buffer"></param>
        private void AddTokens(string buffer, int columnStart, int columnEnd, int lineNumber)
        {
            if (!esMultilinea)
            {
                buffer = buffer.Trim();
                var bufferIs = Segmentation(buffer, lineNumber); //call segmentacion to return into the switch para ver si es reservada o id
                switch (bufferIs)
                {
                    case 0:
                        if (!tokens.ContainsKey(buffer))
                        {
                            tokens.Add(buffer, "T_ERROR");
                        }
                        MakeWriter(buffer, columnStart, columnEnd, "T_ERROR", lineNumber);
                        errores += "*** ERROR: No se reconoció " + buffer + " en linea: " + lineNumber + " \n";
                        break;
                    case 1:
                        if (!tokens.ContainsKey(buffer))
                        {
                            tokens.Add(buffer, $"T_{buffer}");
                        }
                        MakeWriter(buffer, columnStart, columnEnd, $"T_{buffer}", lineNumber);
                        break;
                    case 2:
                        if (!tokens.ContainsKey(buffer))
                        {
                            tokens.Add(buffer, "T_ID");
                        }

                        MakeWriter(buffer, columnStart, columnEnd, "T_ID", lineNumber);
                        break;
                    case 3:
                        if (!tokens.ContainsKey(buffer))
                        {
                            tokens.Add(buffer, "T_BOOLEAN");
                        }

                        MakeWriter(buffer, columnStart, columnEnd, "T_BOOLEAN", lineNumber);
                        break;
                    case 4:
                        if (!tokens.ContainsKey(buffer))
                        {
                            tokens.Add(buffer, "T_ENTERO");
                        }

                        MakeWriter(buffer, columnStart, columnEnd, "T_ENTERO", lineNumber);
                        break;
                    case 5:
                        if (!tokens.ContainsKey(buffer))
                        {
                            tokens.Add(buffer, "T_DOUBLE");
                        }
                        MakeWriter(buffer, columnStart, columnEnd, "T_DOUBLE", lineNumber);
                        break;
                    case 6:
                        if (!tokens.ContainsKey(buffer))
                        {
                            tokens.Add(buffer, "T_CADENA");
                        }

                        MakeWriter(buffer, columnStart, columnEnd, "T_CADENA", lineNumber);
                        break;
                }
            }
        }
        /// <summary>
        /// Mete los operadores al diccionario de tokens
        /// </summary>
        /// <param name="buffer"></param>
        private void AddOperator(string buffer, int columnStart, int columnEnd, int lineNumber)
        {
            //code to add operator
            if (!esMultilinea)
            {
                if (!tokens.ContainsKey(buffer))
                {
                    tokens.Add(buffer, "T_OPERADOR");
                }
                MakeWriter(buffer, columnStart, columnEnd, "T_OPERADOR", lineNumber);
            }
        }

        /// <summary>
        /// This class writes all the things 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="columnStart"></param>
        /// <param name="columnEnd"></param>
        /// <param name="token"></param>
        /// <param name="lineNumber"></param>
        public void MakeWriter(string buffer, int columnStart, int columnEnd, string token, int lineNumber)
        {
            writer += buffer + "        Token: " + token + "        cols " + columnStart + "-" + columnEnd + "          en linea: " + lineNumber + "\n";
        }

        public string Writer { get => writer; set => writer = value; }
        public bool CadenaAbrir { get => cadenaAbrir; set => cadenaAbrir = value; }
        public string Errores { get => errores; set => errores = value; }

        public bool getComentarioAbierto()
        {
            return esMultilinea;
        }

        private bool BufferEsCorrecto(string buffer)
        {
            return Segmentation2(buffer) != 0 ? true : false; 
        }
    }
}

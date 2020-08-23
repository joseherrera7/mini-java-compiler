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
        private string writer = "";
        private bool esMultilinea = false;

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
            bool cadenaAbrir = false;
            foreach (var c in line)
            {
                if (buffer.Equals("/") && c.Equals('*'))
                {
                    esMultilinea = true;
                    buffer += c;
                }
                else if (Char.IsWhiteSpace(c) && !cadenaAbrir) // ignore blankspaces
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
                else if ((operadores.Contains(c.ToString()) || operadores.Contains(buffer)) && !c.Equals('/')) //verfica que sea un operador
                {
                    if (line.Length == columnCounter)
                    {
                        if (operadores.Contains(c.ToString()))
                        {
                            AddOperator(c.ToString(), columnStart, columnEnd, lineNumber);
                        }
                        else
                        {
                            AddTokens(c.ToString(), columnStart, columnEnd, lineNumber);
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
                            else // sino es operador doble lo mete a los tokens el operador del buffer y continua
                            {
                                AddOperator(buffer, columnStart, columnEnd, lineNumber);
                                buffer = c.ToString();
                                columnEnd = columnCounter - 1;
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
                                AddTokens(buffer, columnStart, columnEnd, lineNumber);
                                buffer = c.ToString();
                            }
                        }
                    }
                }
                else if (buffer.Equals("*") && c.Equals('/'))
                {
                    esMultilinea = false;
                }
                else if (buffer.Equals("\""))
                {
                    cadenaAbrir = !cadenaAbrir;
                    buffer += c;
                }
                else if (buffer.Equals("/")) //verfica que es un comentario y se lo salta
                {
                    if (c.Equals('/'))
                    {
                        return;
                    }
                }
                else // mete el simbolo sino detecta un token
                {
                    if (buffer.Equals("")) // inicia el contador de columna
                    {
                        if (line.Length == columnCounter)
                        {
                            if (operadores.Contains(c.ToString()))
                            {
                                AddOperator(c.ToString(), columnStart, columnEnd, lineNumber);
                                AddOperator(c.ToString(), columnStart, columnEnd, lineNumber);
                            }
                            else
                            {
                                AddTokens(c.ToString(), columnStart, columnEnd, lineNumber);
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
                                    AddOperator(c.ToString(), columnStart, columnEnd - 1, lineNumber);
                                    AddOperator(buffer, columnCounter, columnCounter, lineNumber);
                                }
                                else if (operadores.Contains(c.ToString()) && !operadores.Contains(buffer))
                                {
                                    AddOperator(c.ToString(), columnStart, columnEnd - 1, lineNumber);
                                    AddTokens(buffer, columnCounter, columnCounter, lineNumber);
                                }
                                else if (!operadores.Contains(c.ToString()) && operadores.Contains(buffer))
                                {
                                    AddTokens(c.ToString(), columnStart, columnEnd - 1, lineNumber);
                                    AddOperator(buffer, columnCounter, columnCounter, lineNumber);
                                }
                                else
                                {
                                    AddTokens(c.ToString(), columnStart, columnEnd - 1, lineNumber);
                                    AddTokens(buffer, columnCounter, columnCounter, lineNumber);
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
                        buffer += c;
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
        private int Segmentation(string buffer)
        {
            var idPattern = "^[A-Za-z_$]{1}[a-zA-Z\\d$_]*$";
            var constBooleana = "true|false";
            var constEntera = "(0[xX][0-9a-fA-F]+)|([0-9]+)";
            var consDouble = "\\d+\\.(\\d*(E\\+\\d?))";
            var consString = "\".*\"";

            if (reserved.Contains(buffer)) return 1;
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
                else if (Regex.IsMatch(buffer, consString))
                {
                    return 6;
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
                var bufferIs = Segmentation(buffer); //call segmentacion to return into the switch para ver si es reservada o id
                switch (bufferIs)
                {
                    case 0:
                        if (!tokens.ContainsKey(buffer))
                        {
                            tokens.Add(buffer, "ERROR");
                        }
                        MakeWriter(buffer, columnStart, columnEnd, "ERROR", lineNumber);
                        break;
                    case 1:
                        if (!tokens.ContainsKey(buffer))
                        {
                            tokens.Add(buffer, "RESERVADA");
                        }
                        MakeWriter(buffer, columnStart, columnEnd, "RESERVADA", lineNumber);
                        break;
                    case 2:
                        if (!tokens.ContainsKey(buffer))
                        {
                            tokens.Add(buffer, "IDENTIFICADOR");
                        }

                        MakeWriter(buffer, columnStart, columnEnd, "IDENTIFICADOR", lineNumber);
                        break;
                    case 3:
                        if (!tokens.ContainsKey(buffer))
                        {
                            tokens.Add(buffer, "CONSTANTE_BOOLEANA");
                        }

                        MakeWriter(buffer, columnStart, columnEnd, "CONSTANTE_BOOLEANA", lineNumber);
                        break;
                    case 4:
                        if (!tokens.ContainsKey(buffer))
                        {
                            tokens.Add(buffer, "CONSTANTE_ENTERO");
                        }

                        MakeWriter(buffer, columnStart, columnEnd, "CONSTANTE_ENTERO", lineNumber);
                        break;
                    case 5:
                        if (!tokens.ContainsKey(buffer))
                        {
                            tokens.Add(buffer, "CONSTANTE_DOUBLE");
                        }
                        MakeWriter(buffer, columnStart, columnEnd, "CONSTANTE_DOUBLE", lineNumber);
                        break;
                    case 6:
                        if (!tokens.ContainsKey(buffer))
                        {
                            tokens.Add(buffer, "CONSTANTE_CADENA");
                        }

                        MakeWriter(buffer, columnStart, columnEnd, "CONSTANTE_CADENA", lineNumber);
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
                    tokens.Add(buffer, "OPERADOR");
                }
                MakeWriter(buffer, columnStart, columnEnd, "OPERADOR", lineNumber);
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

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace mini_java_compiler
{
    class Reader
    {
        //INCLUÍR MÁS OPERADORES SI ME HIZO FALTA ALGUNO
        string[] operadores = { "+","-","*", "/", "%", "<", "<=", ">", ">=", "=", "==", "!=", "&&", "||", "!", ";", ",", ".", "[", "]",
            "(", ")", "{", "}",
    "[]", "()", "{}" };
        int columnCounter = 0;
        //GUARDA EL ERROR
        public int ERROR = 0;
        string writer = "";

        Dictionary<string, string> tokens = new Dictionary<string, string>();
        
        string[] reserved = { "void", "int", "double", "boolean",
                "string", "class", "const", "interface", "null", "this",
                "extends", "implements", "for", "while", "if", "else",
                "return", "break", "New", "System", "out", "println" };
        
        /// <summary>
        /// Read Line per line the file, and this calls another methods to confirm
        /// </summary>
        /// <param name="line">the line</param>
        public void ReadProgram(string line)
        {
            columnCounter = 1;
            int columnStart = 0;
            int columnEnd = 0;
            string buffer = "";
            foreach (var c in line)
            {
                if (Char.IsWhiteSpace(c)) // ignore blankspaces
                {
                    if (!buffer.Equals("") && !operadores.Contains(buffer))
                    {
                        columnEnd = columnCounter - 1;
                        AddTokens(buffer, columnStart, columnEnd);
                        buffer = "";
                    } 
                    else if (operadores.Contains(buffer))
                    {
                        columnEnd = columnCounter - 1;
                        AddOperator(buffer, columnStart, columnEnd);
                        buffer = "";
                    }
                }  
                else if ((operadores.Contains(c.ToString()) || operadores.Contains(buffer)) && ! c.Equals('/')) //verfica que sea un operador
                {
                    if (line.Length == columnCounter)
                    {
                        if (operadores.Contains(c.ToString()))
                        {
                            AddOperator(c.ToString(), columnStart, columnEnd);
                        }
                        else
                        {
                            AddTokens(c.ToString(), columnStart, columnEnd);
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
                                AddOperator(buffer, columnStart, columnEnd);
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
                                AddTokens(buffer, columnStart, columnEnd);
                                buffer = c.ToString();
                            }
                        }
                    }
                }
                else if (buffer.Equals("/")) //verfica que es un comentario y se lo salta
                {
                    if(c.Equals('/'))
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
                                AddOperator(c.ToString(), columnStart, columnEnd);
                            }
                            else
                            {
                                AddTokens(c.ToString(), columnStart, columnEnd);
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
                                    AddOperator(c.ToString(), columnStart, columnEnd-1);
                                    AddOperator(buffer, columnCounter, columnCounter);
                                }
                                else if (operadores.Contains(c.ToString()) && !operadores.Contains(buffer))
                                {
                                    AddOperator(c.ToString(), columnStart, columnEnd - 1);
                                    AddTokens(buffer, columnCounter, columnCounter);
                                }
                                else if (!operadores.Contains(c.ToString()) && operadores.Contains(buffer))
                                {
                                    AddTokens(c.ToString(), columnStart, columnEnd - 1);
                                    AddOperator(buffer, columnCounter, columnCounter);
                                }
                                else
                                {
                                    AddTokens(c.ToString(), columnStart, columnEnd - 1);
                                    AddTokens(buffer, columnCounter, columnCounter);
                                }
                            }
                            else
                            {
                                columnStart = columnCounter;
                                columnEnd = columnCounter;
                                AddOperator(buffer, columnStart, columnEnd);
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
            var consString = "^\"[a-zA-Z0-9]+\"$";

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
        private void AddTokens(string buffer, int columnStart, int columnEnd)
        {
            if (!tokens.ContainsKey(buffer))
            {
                var bufferIs = Segmentation(buffer); //call segmentacion to return into the switch para ver si es reservada o id
                switch (bufferIs)
                {
                    case 0:
                        tokens.Add(buffer, "ERROR");
                        break;
                    case 1:
                        tokens.Add(buffer, "RESERVADA");
                        break;
                    case 2:
                        tokens.Add(buffer, "IDENTIFICADOR");
                        break;
                    case 3:
                        tokens.Add(buffer, "CONSTANTE_BOOLEANA");
                        break;
                    case 4:
                        tokens.Add(buffer, "CONSTANTE_ENTERO");
                        break;
                    case 5:
                        tokens.Add(buffer, "CONSTANTE_DOUBLE");
                        break;
                    case 6:
                        tokens.Add(buffer, "CONSTANTE_CADENA");
                        break;
                }
            } 
            else
            {

            }
        }
        /// <summary>
        /// Mete los operadores al diccionario de tokens
        /// </summary>
        /// <param name="buffer"></param>
        private void AddOperator(string buffer, int columnStart, int columnEnd)
        {
            //code to add operator
            if (!tokens.ContainsKey(buffer))
            {
                tokens.Add(buffer, "OPERADOR");
            }
            else
            {

            }
        }

        private void MakeWriter(string buffer, int columnStart, int columnEnd, string token)
        {
            writer.Concat(buffer).Concat("      Token: ").Concat(token).Concat(" cols ");
        }

        
        string blockComments = @"/\*(.*?)\*/";

        public string Writer { get => writer; set => writer = value; }
    }
}

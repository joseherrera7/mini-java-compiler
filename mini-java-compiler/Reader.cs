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
        //RECONOCER COMENTARIOS
        string comentario = "//";
        //LLEVA EL CONTROL SI ACTION CONTIENE LLAVE DE APERTURA Y CIERRE
        int contReservadas = 1;
        bool actionLlaves = false;
        int columnCounter = 0;
        //GUARDA EL ERROR
        public int ERROR = 0;
        //VERIFICA SI SE LEYÓ LA SECCIÓN ERROR
        bool bError = false;
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
                    if (!buffer.Equals(""))
                    {
                        columnEnd = columnCounter - 1;
                        addTokens(buffer);
                        buffer = "";
                    }                 
                }  
                else if ((operadores.Contains(c.ToString()) || operadores.Contains(buffer)) && ! c.Equals('/')) //verfica que sea un operador
                {
                    if (operadores.Contains(buffer)) // si el operador está en el bufer 
                    {
                        if (operadores.Contains(buffer+c)) // prueba si es un operador doble
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
                            addOperator(buffer);
                            buffer = c.ToString();
                            columnEnd = columnCounter-1;
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
                            addTokens(buffer);
                            buffer = c.ToString();
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
                        buffer += c;
                        columnStart = columnCounter;
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
            var consTrue = "true";
            var constFalse = "false";
            var consDecimal = "[0-9]+([0-9][0-9]?)?"; 
            var consHexa = "regex para entero hexadecimal"; 
            var consDouble = "regex para double"; 
            var consString = "regex para cadena";

            if (reserved.Contains(buffer)) return 1;
            else
            {
                if (Regex.IsMatch(buffer, idPattern))
                {
                    return 2;
                }
                else if (Regex.IsMatch(buffer, consTrue))
                {
                    return 1;
                }
                else if (Regex.IsMatch(buffer, constFalse))
                {
                    return 1;
                }
                else if (Regex.IsMatch(buffer, consDecimal))
                {
                    return 3;
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
        private void addTokens(string buffer)
        {
            var bufferIs = Segmentation(buffer); //call segmentacion to return into the switch para ver si es reservada o id
            switch (bufferIs)
            {
                case 0:
                    tokens.Add(buffer,"ERROR");
                    break;
                case 1:
                    tokens.Add(buffer, "RESERVADA");
                    break;
                case 2:
                    tokens.Add(buffer, "IDENTIFICADOR");
                    break;
                case 3:
                    tokens.Add(buffer, "ENTERO DECIMAL CONSTANTE");
                    break;
            }
        }
        /// <summary>
        /// Mete los operadores al diccionario de tokens
        /// </summary>
        /// <param name="buffer"></param>
        private void addOperator(string buffer)
        {
            //code to add operator
            tokens.Add(buffer, "OPERADOR");
        }
        string blockComments = @"/\*(.*?)\*/";
    }
}

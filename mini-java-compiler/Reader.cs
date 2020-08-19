﻿using System;
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
        Hashtable hashtable = new Hashtable();
        char[] charsToTrim = { '\t', ' ', '\n', '\r' };
        
        string[] reserved = { "void", "int", "double", "boolean",
                "string", "class", "const", "interface", "null", "this",
                "extends", "implements", "for", "while", "if", "else",
                "return", "break", "New", "System", "out", "println", 
                "abstract", "assert", "byte", "case", "catch", "char", 
                "continue", "default", "do", "enum", "exports", "final",
                "finally", "float", "import", "instanceof", "long", "module",
                "native", "new", "package", "private", "protected", "public",
                "requires", "short", "static", "strictfp", "super", "switch",
                "synchronized", "throw", "throws", "transient", "try", "volatile",
                "true", "false", "var", "goto", "const"
    };
        
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
                if (Char.IsWhiteSpace(c) || c.Equals(';')) // ignore blankspaces
                {
                    columnEnd = columnCounter-1;
                    idOrReserved(buffer);
                    buffer = "";
                }  
                else if (operadores.Contains(c.ToString()) || operadores.Contains(buffer))
                {
                    if (operadores.Contains(buffer))
                    {
                        if (operadores.Contains(buffer+c))
                        {
                            if (buffer.Contains(""))
                            {
                                columnStart = columnCounter;
                                buffer += c;
                            }
                            else
                            {
                                buffer += c;
                            }
                        }
                        else
                        {
                            addOperator(buffer);
                            buffer = c.ToString();
                            columnEnd = columnCounter-1;
                        }
                    }
                    else
                    {
                        if (buffer.Equals(""))
                        {
                            columnStart = columnCounter;
                            buffer = c.ToString();
                        }
                        else
                        {
                            idOrReserved(buffer);
                            buffer = c.ToString();
                        }
                    }
                }
                else
                {
                    if (buffer.Equals(""))
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
        /// Method to organice what is the 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private int Segmentation(string buffer)
        {
            var idPattern = "^[A-Za-z_$]{1}[a-zA-Z\\d$_]*$";
            
            if (reserved.Contains(buffer)) return 1;
            else
            {
                if (Regex.IsMatch(buffer, idPattern))
                {
                    return 2;
                }
                else
                {
                    return 0;
                }
            }
        }

        private void idOrReserved(string buffer)
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
            }
        }

        private void addOperator(string buffer)
        {
            //code to add operator
            tokens.Add(buffer, "OPERADOR");
        }
        string blockComments = @"/\*(.*?)\*/";
        string lineComments = @"//(.*?)\r?\n";
    }
}

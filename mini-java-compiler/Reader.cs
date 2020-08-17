using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace mini_java_compiler
{
    class Reader
    {
        //INCLUÍR MÁS OPERADORES SI ME HIZO FALTA ALGUNO
        string[] operadores = { "|", "*", "+", "?", "•", "(", ")" };

        //LLEVA EL CONTROL SI ACTION CONTIENE LLAVE DE APERTURA Y CIERRE
        int contReservadas = 1;
        bool actionLlaves = false;
        //GUARDA EL ERROR
        public int ERROR = 0;
        //VERIFICA SI SE LEYÓ LA SECCIÓN ERROR
        bool bError = false;


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
        List<String> tokens = new List<string>();
        

        public void ReadProgram(string line)
        {
            string buffer = "";
            line = line.Trim(charsToTrim);
            foreach (var c in line)
            {
                if (Char.IsWhiteSpace(c) || c.Equals(';'))
                {
                    tokens.Add(buffer);
                    buffer = "";
                }
                buffer += c;
                
            }
        } 
    }
}

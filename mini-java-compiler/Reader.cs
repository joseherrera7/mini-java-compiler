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
        Hashtable hashtable = new Hashtable();
        char[] charsToTrim = { '\t', ' ', '\n', '\r' };
        string[] reserved = { "void", "int", "double", "boolean",
                "string", "class", "const", "interface", "null", "this",
                "extends", "implements", "for", "while", "if", "else",
                "return", "break", "New", "System", "out", "println"
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

using System;
using System.Collections.Generic;
using System.IO;

namespace mini_java_compiler
{
    class Operacion
    {
        // EXPR := Number {Operator Number}
        // Operador   := "+" | "-"
        // Numero     := Digit{Digit}
        // Digito      := "0" | "1" | "2" | "3" | "4" | "5" | "6" | "7" | "8" | "9" 

        private StringReader _reader;

        public IEnumerable<Token> Scan(string expression)
        {
            _reader = new StringReader(expression);

            var tokens = new List<Token>();
            while (_reader.Peek() != -1)
            {
                var c = (char)_reader.Peek();
                if (Char.IsWhiteSpace(c))
                {
                    _reader.Read();
                    continue;
                }

                if (Char.IsDigit(c))
                {
                    var nr = ParseNumber();
                    tokens.Add(new NumberConstantToken(nr));
                }
                else if (c == '-')
                {
                    tokens.Add(new MinusToken());
                    _reader.Read();
                }
                else if (c == '+')
                {
                    tokens.Add(new PlusToken());
                    _reader.Read();
                }
                else
                    throw new Exception("Caracter desconocido en expresion: " + c);
            }

            return tokens;
        }

        private int ParseNumber()
        {
            var digits = new List<int>();
            while (Char.IsDigit((char)_reader.Peek()))
            {
                var digit = (char)_reader.Read();
                int i;
                if (int.TryParse(Char.ToString(digit), out i))
                {
                    digits.Add(i);
                }
                else
                    throw new Exception("No se pudo analizar el número entero al analizar el dígito: " + digit);
            }

            var nr = 0;
            var mul = 1;
            digits.Reverse();
            digits.ForEach(d =>
            {
                nr += d * mul;
                mul *= 10;
            });

            return nr;
        }
    }
}


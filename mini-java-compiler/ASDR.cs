using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mini_java_compiler
{
    class ASDR
    {
        // EXPR := Number {Operator Number}
        // Operador   := "+" | "-"
        // Numero     := Digit{Digit}
        // Digito      := "0" | "1" | "2" | "3" | "4" | "5" | "6" | "7" | "8" | "9" 
        public class PlusMinusParser
        {
            private readonly IEnumerator<Token> _tokens;

            public PlusMinusParser(IEnumerable<Token> tokens)
            {
                _tokens = tokens.GetEnumerator();
            }

            public int Parse()
            {
                var result = 0;
                while (_tokens.MoveNext())
                {
                    result = ParseExpression();

                    if (_tokens.MoveNext())
                    {
                        if (_tokens.Current is OperatorToken)
                        {
                            var op = _tokens.Current;

                            var secondNumber = Parse();
                            if (op is PlusToken)
                            {
                                return result + secondNumber;
                            }
                            if (op is MinusToken)
                                return result - secondNumber;

                            throw new Exception("Operador no admitido: " + op);

                        }
                        throw new Exception("Esperando operador tras expresión, pero se obtuvo " + _tokens.Current);
                    }
                }

                return result;
            }

            private int ParseExpression()
            {
                var number = ParseNumber();
                if (!_tokens.MoveNext())
                    return number;

                if (_tokens.Current is OperatorToken)
                {
                    var op = _tokens.Current;
                    _tokens.MoveNext();

                    var secondNumber = ParseNumber();
                    if (op is PlusToken)
                    {
                        return number + secondNumber;
                    }
                    if (op is MinusToken)
                        return number - secondNumber;

                    throw new Exception("Operador no admitido: " + op);
                }

                throw new Exception("Esperando operador tras expresión, pero se obtuvo " + _tokens.Current);
            }

            private int ParseNumber()
            {
                if (_tokens.Current is NumberConstantToken)
                {
                    return (_tokens.Current as NumberConstantToken).Value;
                }

                throw new Exception("Se esperaba una constante numérica pero se encontró " + _tokens.Current);
            }
        }
    }
}
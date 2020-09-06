using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mini_java_compiler
{
        public abstract class Token
        {
        }

        public class OperatorToken : Token
        {

        }
        public class PlusToken : OperatorToken
        {
        }

        public class MinusToken : OperatorToken
        {
        }

        public class NumberConstantToken : Token
        {
            private readonly int _value;

            public NumberConstantToken(int value)
            {
                _value = value;
            }

            public int Value
            {
                get { return _value; }
            }
        }
    }



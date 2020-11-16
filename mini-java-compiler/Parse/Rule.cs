using System;
using System.Collections;

namespace mini_java_compiler.Parse
{


    public class RuleCollection : IEnumerable
    {
        private IList list;

        public RuleCollection()
        {
            list = new ArrayList();
        }

        public IEnumerator GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public void Add(Rule rule)
        {
            list.Add(rule);
        }

        public Rule Get(int index)
        {
            return list[index] as Rule;
        }

        public Rule this[int index]
        {
            get { return Get(index); }
        }
    }

    public class Rule
    {
        private int id;
        private SymbolNonterminal lhs;
        private Symbol[] rhs;

        public Rule(int id, SymbolNonterminal lhs, Symbol[] rhs)
        {
            this.id = id;
            this.lhs = lhs;
            this.rhs = rhs;
        }

        public override String ToString()
        {
            String str = lhs + " ::= ";
            for (int i = 0; i < rhs.Length; i++)
            {
                str += rhs[i] + " ";
            }
            return str.Substring(0, str.Length - 1);
        }

        public int Id { get { return id; } }
        public SymbolNonterminal Lhs { get { return lhs; } }
        public Symbol[] Rhs { get { return rhs; } }
    }
}

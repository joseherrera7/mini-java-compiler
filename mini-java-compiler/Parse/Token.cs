using System;

namespace mini_java_compiler.Parse
{

    public abstract class Token
    {
        private Object userObject;

        public Token()
        {
            this.userObject = null;
        }
        public Object UserObject
        {
            get { return userObject; }
            set { this.userObject = value; }
        }

    }
    public class TerminalToken : Token
    {
        private SymbolTerminal symbol;
        private string text;
        private Location location;

        public TerminalToken(SymbolTerminal symbol, string text, Location location)
        {
            this.symbol = symbol;
            this.text = text;
            this.location = location;
        }

        public override String ToString()
        {
            return text;
        }

        public SymbolTerminal Symbol { get { return symbol; } }

        public string Text { get { return text; } }

        public Location Location { get { return location; } }
    }

    public class NonterminalToken : Token
    {
        private Token[] tokens;
        private Rule rule;
        public NonterminalToken(Rule rule, Token[] tokens)
        {
            this.rule = rule;
            this.tokens = tokens;
        }

        public void ClearTokens()
        {
            tokens = new Token[0];
        }

        public override string ToString()
        {
            String str = rule.Lhs + " = [";
            for (int i = 0; i < tokens.Length; i++)
            {
                str += tokens[i] + "]";
            }
            return str;
        }

        public SymbolNonterminal Symbol { get { return rule.Lhs; } }
        public Token[] Tokens { get { return tokens; } }
        public Rule Rule { get { return rule; } }

    }


}

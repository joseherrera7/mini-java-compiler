using mini_java_compiler.Parse.lalr;
using System;

namespace mini_java_compiler.Parse
{

    public class TokenReadEventArgs : EventArgs
    {
        private TerminalToken token;
        private bool contin;

        public TokenReadEventArgs(TerminalToken token)
        {
            this.token = token;
            contin = true;
        }

        public TerminalToken Token { get { return token; } }

        public bool Continue
        {
            get { return contin; }
            set { contin = value; }
        }

    }

    public class ShiftEventArgs : EventArgs
    {
        private TerminalToken token;
        private State newState;

        public ShiftEventArgs(TerminalToken token, State newState)
        {
            this.token = token;
            this.newState = newState;
        }

        public TerminalToken Token { get { return token; } }

        public State NewState { get { return newState; } }
    }

    public class ReduceEventArgs : EventArgs
    {
        private Rule rule;
        private NonterminalToken token;
        private State newState;
        private bool contin;

        public ReduceEventArgs(Rule rule, NonterminalToken token, State newState)
        {
            this.rule = rule;
            this.token = token;
            this.newState = newState;
            this.contin = true;
        }

        public Rule Rule { get { return rule; } }

        public NonterminalToken Token { get { return token; } }

        public State NewState { get { return newState; } }

        public bool Continue
        {
            get { return contin; }
            set { contin = value; }
        }
    }


    public class GotoEventArgs : EventArgs
    {
        private SymbolNonterminal symbol;
        private State newState;

        public GotoEventArgs(SymbolNonterminal symbol, State newState)
        {
            this.symbol = symbol;
            this.newState = newState;
        }

        public SymbolNonterminal Symbol { get { return symbol; } }

        public State NewState { get { return newState; } }
    }

    public class AcceptEventArgs : EventArgs
    {
        private NonterminalToken token;

        public AcceptEventArgs(NonterminalToken token)
        {
            this.token = token;
        }

        public NonterminalToken Token { get { return token; } }
    }

    public class TokenErrorEventArgs : EventArgs
    {
        private TerminalToken token;
        private bool contin;

        public TokenErrorEventArgs(TerminalToken token)
        {
            this.token = token;
            this.contin = false;
        }

        public TerminalToken Token { get { return token; } }

        public bool Continue
        {
            get { return contin; }
            set { this.contin = value; }
        }
    }

    public enum ContinueMode { Stop, Insert, Skip }

    public class ParseErrorEventArgs : EventArgs
    {
        private TerminalToken unexpectedToken;
        private SymbolCollection expectedTokens;
        private ContinueMode contin;
        private TerminalToken nextToken;

        public ParseErrorEventArgs(TerminalToken unexpectedToken,
                                   SymbolCollection expectedTokens)
        {
            this.unexpectedToken = unexpectedToken;
            this.expectedTokens = expectedTokens;
            this.contin = ContinueMode.Stop;
            this.nextToken = null;
        }

        public TerminalToken UnexpectedToken { get { return unexpectedToken; } }

        public SymbolCollection ExpectedTokens { get { return expectedTokens; } }

        public ContinueMode Continue
        {
            get { return contin; }
            set { this.contin = value; }
        }

        public TerminalToken NextToken
        {
            get { return nextToken; }
            set { this.nextToken = value; }
        }

    }

    public class CommentReadEventArgs : EventArgs
    {
        private string comment;
        private string content;
        private bool lineComment;

        public CommentReadEventArgs(string comment,
                                    string content,
                                    bool lineComment)
        {
            this.comment = comment;
            this.content = content;
            this.lineComment = lineComment;
        }

        public string Comment { get { return comment; } }

        public string Content { get { return content; } }

        public bool LineComment { get { return lineComment; } }

    }


}

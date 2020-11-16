namespace mini_java_compiler.Parse.lalr
{


    public class GotoAction : Action
    {
        private State state;


        public GotoAction(SymbolNonterminal symbol, State state)
        {
            this.symbol = symbol;
            this.state = state;
        }

        public SymbolNonterminal Symbol { get { return (SymbolNonterminal)symbol; } }

        public State State { get { return state; } }
    }
}

namespace mini_java_compiler.Parse.lalr
{
    public class ShiftAction : Action
    {
        private State state;

        public ShiftAction(SymbolTerminal symbol, State state)
        {
            this.symbol = symbol;
            this.state = state;
        }

        public SymbolTerminal Symbol { get { return (SymbolTerminal)symbol; } }

        public State State { get { return state; } }

    }
}

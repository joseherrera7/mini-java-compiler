namespace mini_java_compiler.Parse.lalr
{


    public class ReduceAction : Action
    {
        private Rule rule;


        public ReduceAction(SymbolTerminal symbol, Rule rule)
        {
            this.symbol = symbol;
            this.rule = rule;
        }

        public SymbolTerminal Symbol { get { return (SymbolTerminal)symbol; } }


        public Rule Rule { get { return rule; } }
    }
}

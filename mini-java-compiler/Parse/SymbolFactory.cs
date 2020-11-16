using mini_java_compiler.Parse.content;

namespace mini_java_compiler.Parse
{
    public sealed class SymbolFactory
    {
        private SymbolFactory()
        {
        }

        static public Symbol CreateSymbol(SymbolRecord symbolRecord)
        {
            Symbol symbol;
            switch (symbolRecord.Kind)
            {
                case 0:
                    symbol = new SymbolNonterminal(symbolRecord.Index, symbolRecord.Name);
                    break;
                case 1:
                    symbol = new SymbolTerminal(symbolRecord.Index, symbolRecord.Name);
                    break;
                case 2:
                    symbol = new SymbolWhiteSpace(symbolRecord.Index);
                    break;
                case 3:
                    symbol = SymbolCollection.EOF;
                    break;
                case 4:
                    symbol = new SymbolCommentStart(symbolRecord.Index);
                    break;
                case 5:
                    symbol = new SymbolCommentEnd(symbolRecord.Index);
                    break;
                case 6:
                    symbol = new SymbolCommentLine(symbolRecord.Index);
                    break;
                case 7:
                    symbol = SymbolCollection.ERROR;
                    break;
                default:
                    symbol = new SymbolError(-1);
                    break;
            }
            return symbol;
        }
    }


}

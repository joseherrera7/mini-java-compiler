using System.Collections.Generic;

namespace mini_java_compiler.Parse
{
    public class Gramatica
    {
        /// <summary>
        /// Accessing all symbols with string
        /// </summary>
        private Dictionary<string, ISimbolo> Symbols { get; set; }

        public IEnumerable<ISimbolo> SymbolList => Symbols.Values;

        public Variable HeadVariable { get; set; }



        public Gramatica()
        {
            Symbols = new Dictionary<string, ISimbolo>();
        }
        /// <summary>
        /// access a symbol with string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="symbolType"></param>
        /// <returns></returns>
        public ISimbolo GetOrCreateSymbol(string value, TipoSimbolo symbolType)
        {
            if (symbolType == TipoSimbolo.Terminal)
            {
                if (value == Terminal.EndOfFile.Value)
                    return Terminal.EndOfFile;
                if (value == "")
                    return Terminal.Epsilon;
                if (!Symbols.ContainsKey(value))
                    Symbols.Add(value, new Terminal(value));

                return Symbols[value];
            }
            // else    
            if (!Symbols.ContainsKey(value))
                Symbols.Add(value, new Variable(value));
            return Symbols[value];

        }
    }
}

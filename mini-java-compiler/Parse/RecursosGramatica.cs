using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace mini_java_compiler.Parse
{
    public class RecursosGramatica
    {
        // Variable -> ProducedRule 
        // Produced Rule is a list of variable or terminals
        private const string Head = "Head";
        private readonly Gramatica _grammarRules;

        private string Data { get; set; }

        public RecursosGramatica(string data)
        {
            Data = data;
            _grammarRules = new Gramatica();
        }

        public Gramatica TokenizeGrammar()
        {
            //My Wife insisted that first rule should be the head!
            _grammarRules.GetOrCreateSymbol(Head, TipoSimbolo.Variable);

            var lines = Data.Split('\n');
            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                    LineTokenExtractor(line);
            }

            return _grammarRules;
        }

        public List<Terminal> TokenizeInputText()
        {
            var lines = Data.Split('\n');

            return AddEndSymbol((from line in lines
                .Where(s => !string.IsNullOrEmpty(s))
                                 from item in line.Split(' ')
                                 select new Terminal(item)).ToList());
        }

        public List<Terminal> AddEndSymbol(List<Terminal> terminals)
        {
            terminals.Add(Terminal.EndOfFile);
            return terminals;
        }

        private void LineTokenExtractor(string line)
        {
            Regex text = new Regex(@"<(?<variable>[\w-]+)>|""(?<terminal>[^""<>]+)?""", RegexOptions.Compiled);

            var matches = text.Matches(line);
            var firstVariable = matches[0].Groups["variable"];

            if (!firstVariable.Success) return;
            var headVariable = _grammarRules.GetOrCreateSymbol(firstVariable.Value, TipoSimbolo.Variable);

            var symbols = new List<ISimbolo>();
            for (var index = 1; index < matches.Count; index++)
            {
                Match match = matches[index];
                var variable = match.Groups["variable"];
                if (variable.Success)
                {
                    symbols.Add(_grammarRules.GetOrCreateSymbol(variable.Value, TipoSimbolo.Variable));
                    continue;
                }

                var terminal = match.Groups["terminal"];
                if (terminal.Success)
                {
                    symbols.Add(_grammarRules.GetOrCreateSymbol(terminal.Value, TipoSimbolo.Terminal));
                    continue;
                }

                //if it comes here then it's epsilon
                symbols.Add(_grammarRules.GetOrCreateSymbol("", TipoSimbolo.Terminal));
            }


            if (_grammarRules.HeadVariable == null)
            {
                Variable addedVariable = (Variable)_grammarRules.GetOrCreateSymbol(Head, TipoSimbolo.Variable);
                addedVariable.RuleSet.Definitions.Add(new List<ISimbolo>() { headVariable });
                _grammarRules.HeadVariable = addedVariable;
                //_grammarRules.HeadVariable = (Variable)headVariable;
            }
            ((Variable)headVariable).RuleSet.Definitions.Add(symbols);
        }

    }


    public interface ISimbolo
    {
        TipoSimbolo SymbolType { get; }
        string Value { get; }
    }
    public enum TipoSimbolo
    {
        Terminal = 0,
        Variable = 1,
    }

    public class Regla
    {
        public Regla(Variable variable)
        {
            Variable = variable;
            Definitions = new List<IEnumerable<ISimbolo>>();
        }

        public List<IEnumerable<ISimbolo>> Definitions { get; set; }

        public Variable Variable { get; }
    }

    public class Variable : ISimbolo
    {
        public TipoSimbolo SymbolType { get; }

        public string Value { get; }

        public List<Terminal> Firsts { get; set; }
        public List<Terminal> Follows { get; set; }

        public bool IsCalculatingFirst { get; set; }
        public bool IsCalculatingFollow { get; set; }

        public bool FirstReady { get; set; }
        //Can't check if follow is null because $ should be added first
        public bool FollowReady { get; set; }

        public Regla RuleSet { get; }

        public Variable(string value)
        {
            SymbolType = TipoSimbolo.Variable;
            Value = value;
            Firsts = new List<Terminal>();
            Follows = new List<Terminal>();
            FirstReady = false;
            FollowReady = false;
            RuleSet = new Regla(this);
        }

        public string ShowRules()
        {
            return $"{this} ==> " +
                   string.Join(" | ", (
                       from ruleSet in RuleSet.Definitions
                       select string.Join("", ruleSet)));
        }

        public string ShowFirsts()
        {
            return $"{this} ==> " + string.Join(", ", Firsts);
        }

        public string ShowFollows()
        {
            return $"{this} ==> " + string.Join(", ", Follows);
        }

        public override string ToString()
        {
            return $" <{Value}> ";
        }

        public override bool Equals(object obj)
        {
            if (obj is Variable var)
            {
                return var.Value.Equals(Value);
            }

            return false;
        }


        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }


    }

    public class Terminal : ISimbolo
    {
        public static Terminal EndOfFile { get; } = new Terminal("$");

        public static Terminal Epsilon { get; } = new Terminal("ε");

        public static Terminal Error { get; } = new Terminal("ERR");

        public TipoSimbolo SymbolType { get; }

        public string Value { get; }

        public Terminal(string value)
        {
            Value = value;
            SymbolType = TipoSimbolo.Terminal;
        }

        public override bool Equals(object obj)
        {
            if (obj is Terminal term)
            {
                return term.Value.Equals(Value);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return $" \"{Value}\" ";
        }
    }
}

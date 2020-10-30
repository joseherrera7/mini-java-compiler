using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Resources;
using mini_java_compiler.Utilidades;
using mini_java_compiler.Parse.content;
using mini_java_compiler.Parse.structure;
using mini_java_compiler.Parse.lalr;
using DFA = mini_java_compiler.Parse.dfa;
using System.Runtime.Serialization;

namespace mini_java_compiler.Parse
{
	
	public class CGTReader
	{
		private Stream stream;
	    private CGTStructure structure;
	    private CGTContent content;
		private DFA.StateCollection dfaStates;
		private StateCollection parserStates;
		private SymbolCollection symbols;
		private RuleCollection rules;

		public CGTReader(Stream stream)
		{
			ReadFile(stream);
		}

		public CGTReader(String filename)
		{
			ReadFile(new FileStream(filename,FileMode.Open,FileAccess.Read));
		}

		private void Reset()
		{
			stream = null;
			structure = null;
			content = null;
			dfaStates = null;
			parserStates = null;
			symbols = null;
			rules = null;
		}

		private void ReadFile(Stream stream)
		{
			try
			{
				Reset();
				this.stream = stream;
				CalithaBinReader reader = new CalithaBinReader(stream);
				string header = "";
				try
				{
					header = reader.ReadUnicodeString();
					if (! header.StartsWith("Mini Compilador Java"))
						throw new CGTStructureException("El encabezado del archivo es inválido");
				}
				catch (EndOfStreamException e)
				{
					throw new CGTStructureException("El encabezado del archivo es inválido", e);
				}
				RecordCollection records = new RecordCollection();
				while (!(stream.Position == stream.Length))
				{
					records.Add(ReadRecord(reader));
				}
				structure = new CGTStructure(header,records);
				content = new CGTContent(structure);
				dfaStates = CreateDFAStates(content);
				parserStates = CreateParserStates(content);
			}
			finally
			{
				stream.Close();
			}
		}

		public StringTokenizer CreateNewTokenizer()
		{
			DFA.State startState = dfaStates[content.InitialStates.DFA];
			DFA.DFA dfa = new DFA.DFA(dfaStates,startState);
			return new StringTokenizer(dfa);
		}
		public LR1Parser CreateNewParser()
		{
			State startState = parserStates[content.InitialStates.LALR];
			return new LR1Parser(CreateNewTokenizer(),
			                      parserStates,
			                      startState,
			                      Symbols);
		}

		private SymbolCollection CreateSymbols(CGTContent content)
		{
			SymbolCollection symbols = new SymbolCollection();
			foreach (SymbolRecord symbolRecord in content.SymbolTable)
			{
				Symbol symbol = SymbolFactory.CreateSymbol(symbolRecord);
				symbols.Add(symbol);
			}
			return symbols;
		}
		
		private DFA.StateCollection CreateDFAStates(CGTContent content)
		{
			symbols = CreateSymbols(content);
			DFA.StateCollection states = new DFA.StateCollection();
			foreach (DFAStateRecord stateRecord in content.DFAStateTable)
			{
				DFA.State state;
				if (stateRecord.AcceptState)
				{
					Symbol symbol = symbols[stateRecord.AcceptIndex];

					state = new DFA.EndState(stateRecord.Index,(SymbolTerminal)symbol);
				}
				else
				{
					state = new DFA.State(stateRecord.Index);
				}
				states.Add(state);				
			}
			
			foreach (DFAStateRecord stateRecord in content.DFAStateTable)
			{
				foreach (EdgeSubRecord edgeRecord in stateRecord.EdgeSubRecords)
				{
					DFA.State source = states[stateRecord.Index];
					DFA.State target = states[edgeRecord.TargetIndex];
					CharacterSetRecord charsetRec = content.CharacterSetTable[edgeRecord.CharacterSetIndex];
					DFA.Transition transition = new DFA.Transition(target,charsetRec.Characters);
					source.Transitions.Add(transition);
				}
			}
			return states;
		}

		private RuleCollection CreateRules(CGTContent content)
		{
			RuleCollection rules = new RuleCollection();
			foreach (RuleRecord ruleRecord in content.RuleTable)
			{
				SymbolNonterminal lhs = symbols[ruleRecord.Nonterminal] as SymbolNonterminal;
				Symbol[] rhs = new Symbol[ruleRecord.Symbols.Count];
				for (int i = 0; i< rhs.Length; i++)
				{
					rhs[i] = symbols[ruleRecord.Symbols[i]];
				}

				Rule rule = new Rule(ruleRecord.Index,lhs,rhs);
				rules.Add(rule);
			}
			return rules;
		}

		private StateCollection CreateParserStates(CGTContent content)
		{
			rules = CreateRules(content);

			StateCollection states = new StateCollection();
			foreach (LR1StateRecord record in content.LALRStateTable)
			{
				State state = new State(record.Index);
				states.Add(state);
			}
			
			foreach (LR1StateRecord record in content.LALRStateTable)
			{
				State state = states[record.Index];
				foreach (ActionSubRecord subRecord in record.ActionSubRecords)
				{
                    lalr.Action action =
						ActionFactory.CreateAction(subRecord,
						                           states,
						                           symbols,
						                           rules);
					state.Actions.Add(action);
				}

			}
			return states;
		}

		private Record ReadRecord(CalithaBinReader reader)
		{
			Record record = new Record();
            byte entriesHeader = reader.ReadByte();
            if (entriesHeader != 77)
            {
				throw new CGTStructureException("Invalid entries header at byte "+(stream.Position-1));
            }
            ushort entriesCount = reader.ReadUInt16();
            
            for (int i=0;i<entriesCount;i++)
            {
                record.Entries.Add(ReadEntry(reader));
            }
            return record;
		}
		
		private Entry ReadEntry(CalithaBinReader reader)
		{
			Entry entry = EntryFactory.CreateEntry(reader);
			if (entry == null)
				throw new CGTStructureException("Invalid entry type at byte "+(stream.Position-1));
            return entry;
		}
		
		private CGTStructure Structure {get{return structure;}}
		private CGTContent Content {get{return content;}}
		private SymbolCollection Symbols {get{return symbols;}}
		public RuleCollection Rules {get{return rules;}}

	}
}

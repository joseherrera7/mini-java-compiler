using System;
using System.Collections;
using mini_java_compiler.Parse.content;
using mini_java_compiler.Parse.structure;

namespace mini_java_compiler.Parse.content
{

	public class ActionSubRecordCollection : IEnumerable
	{
		private IList list;

		public ActionSubRecordCollection(Record record, int start)
		{
			list = new ArrayList();
			if ((record.Entries.Count-start) % 4 != 0)
				throw new CGTContentException("Numero invalido de estados en LR1");
			for (int i=start;i<record.Entries.Count;i=i+4)
			{
				ActionSubRecord actionRecord = new ActionSubRecord(record.Entries[i],
																   record.Entries[i+1],
																   record.Entries[i+2]);
				list.Add(actionRecord);
			}
		}

		public IEnumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}

	}

	public class ActionSubRecord
	{
		public ActionSubRecord(Entry symbolEntry, Entry actionEntry, Entry targetEntry)
		{
			this.symbolIndex = symbolEntry.ToIntValue();
			this.action = actionEntry.ToIntValue();
			this.target = targetEntry.ToIntValue();
		}

		protected int symbolIndex;
		protected int action;
		protected int target;
		public int SymbolIndex{get{return symbolIndex;}}
		public int Action{get{return action;}}
		public int Target{get{return target;}}
	}
	public class TableCounts
	{
		private int symbolTable;
		private int characterSetTable;
		private int ruleTable;
		private int dfaTable;
		private int lalrTable;

		public TableCounts(Record record)
		{
			if (record.Entries.Count != 6)
				throw new CGTContentException("Numero de tablas incorrecto");
			byte header = record.Entries[0].ToByteValue();
			if (header != 84) //'T'
				throw new CGTContentException("Invalido");
			this.symbolTable = record.Entries[1].ToIntValue();
			this.characterSetTable = record.Entries[2].ToIntValue();
			this.ruleTable = record.Entries[3].ToIntValue();
			this.dfaTable = record.Entries[4].ToIntValue();
			this.lalrTable = record.Entries[5].ToIntValue();
		}

		public int SymbolTable { get { return symbolTable; } }
		public int CharacterSetTable { get { return characterSetTable; } }
		public int RuleTable { get { return ruleTable; } }
		public int DFATable { get { return dfaTable; } }
		public int LALRTable { get { return lalrTable; } }
	}

	public class CGTContent
	{
		private Parameters parameters;
		private TableCounts tableCounts;
		private SymbolTable symbolTable;
		private CharacterSetTable characterSetTable;
		private RuleTable ruleTable;
		private InitialStatesRecord initialStates;
		private DFAStateTable dfaStateTable;
		private LR1StateTable lalrStateTable;

		public CGTContent(CGTStructure structure)
		{
			if (structure.Records.Count < 3)
				throw new CGTContentException("El archivo no tiene nada");
			parameters = new Parameters(structure.Records[0]);
			tableCounts = new TableCounts(structure.Records[1]);

			int initialStatesStart = 2;
			int characterSetStart = initialStatesStart + 1;
			int symbolStart = characterSetStart + TableCounts.CharacterSetTable;
			int ruleStart = symbolStart + TableCounts.SymbolTable;
			int dfaStart = ruleStart + TableCounts.RuleTable;
			int lalrStart = dfaStart + TableCounts.DFATable;
			int specifiedRecordCount = lalrStart + TableCounts.LALRTable;
			if (structure.Records.Count != specifiedRecordCount)
				throw new CGTContentException("Numero invalido de entradas");

			characterSetTable = new CharacterSetTable(structure,
													  characterSetStart,
													  TableCounts.CharacterSetTable);
			symbolTable = new SymbolTable(structure,
				symbolStart,
				TableCounts.SymbolTable);

			ruleTable = new RuleTable(structure,
									  ruleStart,
									  TableCounts.RuleTable);
			initialStates = new InitialStatesRecord(structure.Records[initialStatesStart]);
			dfaStateTable = new DFAStateTable(structure, dfaStart, TableCounts.DFATable);
			lalrStateTable = new LR1StateTable(structure, lalrStart, TableCounts.LALRTable);
		}


		public Parameters Parameters { get { return parameters; } }
		public TableCounts TableCounts { get { return tableCounts; } }
		public SymbolTable SymbolTable { get { return symbolTable; } }
		public CharacterSetTable CharacterSetTable { get { return characterSetTable; } }
		public RuleTable RuleTable { get { return ruleTable; } }
		public InitialStatesRecord InitialStates { get { return initialStates; } }
		public DFAStateTable DFAStateTable { get { return dfaStateTable; } }
		public LR1StateTable LALRStateTable { get { return lalrStateTable; } }
	}
}

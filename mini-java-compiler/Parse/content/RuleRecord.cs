using System;
using mini_java_compiler.Utilidades;
using mini_java_compiler.Parse.content;
using mini_java_compiler.Parse.structure;

namespace mini_java_compiler.Parse.content
{
	public class RuleRecord
	{
		private int index;
		private int nonterminal;
		private IntegerList symbols;

		public RuleRecord(Record record)
		{
			if (record.Entries.Count < 4)
				throw new CGTContentException("Numero invalido de reglas");
			byte header = record.Entries[0].ToByteValue();
			if (header != 82) //'R'
				throw new CGTContentException("Numero invalido de reglas");
			this.index = record.Entries[1].ToIntValue();
			this.nonterminal = record.Entries[2].ToIntValue();		
			this.symbols = new IntegerList();
			for (int i=4;i<record.Entries.Count;i++)
			{
				int symbol = record.Entries[i].ToIntValue();
				symbols.Add(symbol);
			}
		}
		
		public int Index{get{return index;}}
		public int Nonterminal{get{return nonterminal;}}
		public IntegerList Symbols{get{return symbols;}}
	}
}

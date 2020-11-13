using System;
using mini_java_compiler.Parse.content;
using mini_java_compiler.Parse.structure;

namespace mini_java_compiler.Parse.content
{
	public class InitialStatesRecord
	{
		private int dfa;
		private int lalr;

		public InitialStatesRecord(Record record)
		{
			if (record.Entries.Count != 3)
				throw new ContentException("Numero invalido");
			byte header = record.Entries[0].ToByteValue();
			if (header != 73) //'I'
				throw new ContentException("Numero invalido");
			this.dfa   = record.Entries[1].ToIntValue();
			this.lalr  = record.Entries[2].ToIntValue();
		}
		
		public int DFA{get{return dfa;}}
		public int LALR{get{return lalr;}}
	}
}

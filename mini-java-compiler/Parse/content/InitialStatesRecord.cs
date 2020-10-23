using System;
using mini_java_compiler.Parse.content;
using mini_java_compiler.Parse.structure;

namespace mini_java_compiler.Parse.content
{
	/// <summary>
	/// The InitialStatesRecord identifies the starting states for the DFA and LALR parser.
	/// </summary>
	public class InitialStatesRecord
	{
		private int dfa;
		private int lalr;

		public InitialStatesRecord(Record record)
		{
			if (record.Entries.Count != 3)
				throw new CGTContentException("Invalid number of entries for initial states");
			byte header = record.Entries[0].ToByteValue();
			if (header != 73) //'I'
				throw new CGTContentException("Invalid initial states header");
			this.dfa   = record.Entries[1].ToIntValue();
			this.lalr  = record.Entries[2].ToIntValue();
		}
		
		public int DFA{get{return dfa;}}
		public int LALR{get{return lalr;}}
	}
}

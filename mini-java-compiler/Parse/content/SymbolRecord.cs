using System;
using mini_java_compiler.Parse.content;
using mini_java_compiler.Parse.structure;

namespace mini_java_compiler.Parse.content
{
	public class SymbolRecord
	{
		private int index;
		private string name;
		private int kind;

		public SymbolRecord(Record record)
		{
			if (record.Entries.Count != 4)
				throw new CGTContentException("Numero de entradas incorrecto");
			byte header = record.Entries[0].ToByteValue();
			if (header != 83) //'S'
				throw new CGTContentException("Numero de simbolo incorrecto");
			this.index  = record.Entries[1].ToIntValue();
			this.name   = record.Entries[2].ToStringValue();
			this.kind   = record.Entries[3].ToIntValue();
		}
		
		public int Index{get{return index;}}
		public string Name{get{return name;}}
		public int Kind{get{return kind;}}
	}
}

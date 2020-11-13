using System;
using mini_java_compiler.Parse.content;
using mini_java_compiler.Parse.structure;

namespace mini_java_compiler.Parse.content
{
	public class CharacterSetRecord
	{
		private int index;
		private string characters;

		public CharacterSetRecord(Record record)
		{
			if (record.Entries.Count != 3)
				throw new ContentException("Numero invalido de caracteres de entrada");
			byte header = record.Entries[0].ToByteValue();
			if (header != 67) //'C'
				throw new ContentException("Caracter invalido");
			this.index = record.Entries[1].ToIntValue();
			this.characters = record.Entries[2].ToStringValue();
		}
		
		public int Index{get{return index;}}
		public string Characters{get{return characters;}}
	}
}

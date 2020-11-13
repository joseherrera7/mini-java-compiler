using System;
using mini_java_compiler.Parse.content;
using mini_java_compiler.Parse.structure;

namespace mini_java_compiler.Parse.content
{
	public class Parameters
	{
		private string name;
		private string version;
		private string author;
		private string about;
		private bool caseSensitive;
		private int startSymbol;

		public Parameters(Record record)
		{
			if (record.Entries.Count != 7)
				throw new ContentException("Numero invalido de parametros");
			byte header = record.Entries[0].ToByteValue();
			if (header != 80) //'P'
				throw new ContentException("Numero invalido de parametros");
			this.name           = record.Entries[1].ToStringValue();
			this.version        = record.Entries[2].ToStringValue();
			this.author         = record.Entries[3].ToStringValue();
			this.about          = record.Entries[4].ToStringValue();
			this.caseSensitive  = record.Entries[5].ToBoolValue();
			this.startSymbol    = record.Entries[6].ToIntValue();
		}
	
		public string Name {get{return name;}}
		public string Version {get{return version;}}
		public string Author {get{return author;}}
		public string About {get{return about;}}
		public bool CaseSensitive {get{return caseSensitive;}}
		public int StartSymbol {get{return startSymbol;}}
	}
}

using System;
using System.Collections;
using System.Text;
using mini_java_compiler.Parse.content;
using mini_java_compiler.Parse.structure;

namespace mini_java_compiler.Parse.structure
{
	/// <summary>
	/// The CGTStructure contains the header and records that are in the
	/// compiled grammar table.
	/// </summary>
	public class CGTStructure
	{
		private string header;
		private RecordCollection records;

		public CGTStructure(string header, RecordCollection records)
		{
		    this.header = header;
		    this.records = records;
		}
		
        public override string ToString()
        {
			return header.ToString()+"\n"+records.ToString();
		}
		
		public string Header{ get{return header;} }
		public RecordCollection Records{ get{return records;} }
		
	}
}

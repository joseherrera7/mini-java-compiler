using System;
using mini_java_compiler.Parse.content;
using mini_java_compiler.Parse.structure;

namespace mini_java_compiler.Parse.content
{
	public class LR1StateRecord
	{
		private int index;
		private ActionSubRecordCollection actionSubRecords;

		public LR1StateRecord(Record record)
		{
			if (record.Entries.Count < 3)
				throw new CGTContentException("Numero invalido");
			byte header = record.Entries[0].ToByteValue();
			if (header != 76) //'L'
				throw new CGTContentException("Numero invalido");
			this.index = record.Entries[1].ToIntValue();
			actionSubRecords = new ActionSubRecordCollection(record,3);
		}

		public int Index{get{return index;}}
		public ActionSubRecordCollection ActionSubRecords{get{return actionSubRecords;}}
	}
}

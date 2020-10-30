using System;
using System.Collections;
using mini_java_compiler.Parse.content;
using mini_java_compiler.Parse.structure;

namespace mini_java_compiler.Parse.content
{
	public class DFAStateTable : IEnumerable
	{
		private IList list;

		public DFAStateTable(CGTStructure structure, int start, int count)
		{
			list = new ArrayList();
			for (int i=start;i<start+count;i++)
			{
				DFAStateRecord dfaState = new DFAStateRecord(structure.Records[i]);
				list.Add(dfaState);
			}			
		}

		public IEnumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}

		public DFAStateRecord Get(int index)
		{
			return list[index] as DFAStateRecord;
		}

		public DFAStateRecord this[int index]
		{
			get
			{
				return Get(index);
			}
		}

		public int Count {get{return list.Count;}}

	}
}

using System;
using System.Collections;
using mini_java_compiler.Parse.content;
using mini_java_compiler.Parse.structure;

namespace mini_java_compiler.Parse.content
{
	public class LR1StateTable : IEnumerable
	{
		private IList list;

		public LR1StateTable(CGTStructure structure, int start, int count)
		{
			list = new ArrayList();
			for (int i=start;i<start+count;i++)
			{
				LR1StateRecord lalrState = new LR1StateRecord(structure.Records[i]);
				list.Add(lalrState);
			}			
		}

		public IEnumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}

		public LR1StateRecord Get(int index)
		{
			return list[index] as LR1StateRecord;
		}

		public LR1StateRecord this[int index]
		{
			get
			{
				return Get(index);
			}
		}

		public int Count {get{return list.Count;}}

	}
}

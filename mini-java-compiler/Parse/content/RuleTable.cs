using System;
using System.Collections;
using mini_java_compiler.Parse.content;
using mini_java_compiler.Parse.structure;

namespace mini_java_compiler.Parse.content
{
	public class RuleTable : IEnumerable
	{
		private IList list;

		public RuleTable(CGTStructure structure, int start, int count)
		{
			list = new ArrayList();
			for (int i=start;i<start+count;i++)
			{
				RuleRecord ruleRecord = new RuleRecord(structure.Records[i]);
				list.Add(ruleRecord);
			}			

		}
		
		public IEnumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}

		public RuleRecord Get(int index)
		{
			return list[index] as RuleRecord;
		}

		public RuleRecord this[int index]
		{
			get
			{
				return Get(index);
			}
		}
	}
}

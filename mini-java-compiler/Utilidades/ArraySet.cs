using System;
using System.Collections;

namespace mini_java_compiler.Utilidades
{

	public class ArraySet : AbstractSet
	{
		private IList list;

		public ArraySet() : base(new ArrayList())
		{
			list = (ArrayList)collection;
		}

		public override void Add(Object obj)
		{
			list.Add(obj);
		}

		public override void Clear()
		{
			list.Clear();
		}

		public override bool Contains(Object obj)
		{
			return list.Contains(obj);
		}

		public override void Remove(Object obj)
		{
			list.Remove(obj);
		}

		public override IEnumerator GetEnumerator()
		{
			return collection.GetEnumerator();
		}


	}

}

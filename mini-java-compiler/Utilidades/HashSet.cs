using System;
using System.Collections;

namespace mini_java_compiler.Utilidades
{
	public class HashSet : AbstractSet
	{
		private IDictionary map;

		public HashSet() : base(new Hashtable())
		{
			map = (Hashtable)collection;
		}

		public override void Add(Object obj)
		{
			
			map.Add(obj,null);
		}

		public override void Clear()
		{
			map.Clear();
		}

		public override bool Contains(Object obj)
		{
			return map.Contains(obj);
		}

		public override void Remove(Object obj)
		{
			map.Remove(obj);
		}

		public override IEnumerator GetEnumerator()
		{
			return map.Keys.GetEnumerator();
		}


	}
}

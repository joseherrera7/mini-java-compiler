using System;
using System.Collections;
using System.Collections.Specialized;
using mini_java_compiler.Parse;

namespace mini_java_compiler.Parse.lalr
{

	public class ActionCollection : IEnumerable
	{
		private IDictionary table;

		public ActionCollection()
		{
			table = new HybridDictionary();
		}

		public IEnumerator GetEnumerator()
		{
			return table.Values.GetEnumerator();
		}

		public void Add(Action action)
		{
			table.Add(action.symbol,action);
		}

		public Action Get(Symbol symbol)
		{
			return table[symbol] as Action;
		}

		public Action this[Symbol symbol]
		{
			get { return Get(symbol);}
		}
	}


	public abstract class Action
	{
		internal Symbol symbol;
	}
}

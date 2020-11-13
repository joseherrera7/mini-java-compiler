using System;
using System.Collections;
using mini_java_compiler.Utilidades;

namespace mini_java_compiler.Parse.lalr
{

	public class State
	{
		private int id;
		private ActionCollection actions;

		public State(int id)
		{
			this.id = id;
			actions = new ActionCollection();
		}

		public int Id {get{return id;}}

		public ActionCollection Actions {get{return actions;}}
	}

	public class StateCollection : IEnumerable
	{
		
		private IList list;
		
		public StateCollection()
		{
			list = new ArrayList();
		}
		
		public IEnumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}
		
		public void Add(State state)
		{
			list.Add(state);
		}
		
		public State Get(int index)
		{
			return list[index] as State;
		}

		public State this[int index]
		{
			get {return Get(index);}
		}
	}

}

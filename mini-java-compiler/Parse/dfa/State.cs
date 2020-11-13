using System;
using System.Collections;
using mini_java_compiler.Utilidades;

namespace mini_java_compiler.Parse.dfa
{
	public class State
	{
		private int id;
		private TransitionCollection transitions;

		public State(int id)
		{
			this.id = id;
			transitions = new TransitionCollection();
		}	

		public int Id {get {return id;}}

		public TransitionCollection Transitions {get {return transitions;}}
	}
	
	public class EndState : State
	{
		private SymbolTerminal acceptSymbol;

		public EndState(int id, SymbolTerminal acceptSymbol) : base(id)
		{
			this.acceptSymbol = acceptSymbol;
		}
		
		public SymbolTerminal AcceptSymbol {get {return acceptSymbol;}}
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
			get { return Get(index);}
		}

	}

	
}

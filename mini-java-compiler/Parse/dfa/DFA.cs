using System;

namespace mini_java_compiler.Parse.dfa
{
	public interface IDFA
	{
		void Reset();

		State GotoNext(char ch);


		State CurrentState {get;}
	}
	

	public class DFA : IDFA
	{
		private StateCollection states;
		private State startState;
		private State currentState;


		public DFA(StateCollection states, State startState)
		{
			this.states       = states;
			this.startState   = startState;
			this.currentState = startState;
		}
		

		public void Reset()
		{
			this.currentState = startState;
		}
		

		public State GotoNext(char ch)
		{
			Transition transition = currentState.Transitions.Find(ch);
			if (transition != null)
			{
				currentState = transition.Target;
				return currentState;
			}
			else
				return null;
		}

		public State CurrentState {get {return currentState;}}
	}
}

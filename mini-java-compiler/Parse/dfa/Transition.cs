using System;
using System.Collections;
using System.Collections.Specialized;
using mini_java_compiler.Utilidades;

namespace mini_java_compiler.Parse.dfa
{

	public class Transition
	{
		private State target;
		private ISet charset;

		public Transition(State target, string characters)
		{
			this.target = target;
			if (characters.Length > 10)
				this.charset = new HashSet();
			else
				this.charset = new ArraySet();
			char[] ca = characters.ToCharArray();
			foreach (Char ch in ca)
			{
				this.charset.Add(ch);
			}
		}

		public State Target {get {return target;}}

		public ISet CharSet {get {return charset;}}
	}
	

	public class TransitionCollection : IEnumerable
	{	
		private IList list;
		private IDictionary map;

		public TransitionCollection()
		{
			list = new ArrayList();
			map = new HybridDictionary();
		}
		
		public IEnumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}

		public int Add(Transition transition)
		{
			IEnumerator enumerator = transition.CharSet.GetEnumerator();
			while (enumerator.MoveNext())
			{
				char ch = (char)enumerator.Current;
				map.Add(ch, transition);
			}
			return list.Add(transition);
		}
	
		public Transition Find(char ch)
		{
			return map[ch] as Transition;
		}

	}
	
}

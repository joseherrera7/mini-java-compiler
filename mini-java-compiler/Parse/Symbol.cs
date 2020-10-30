using System;
using System.Text;
using System.Collections;
using mini_java_compiler.Utilidades;

namespace mini_java_compiler.Parse
{

	public abstract class Symbol
	{
		private int id;
		private string name;

		protected Symbol(int id, string name)
		{
			this.id = id;
			this.name = name;
		}

		public override bool Equals(Object obj)
		{
			TripleState result = Util.EqualsNoState(this, obj);
			if (result == TripleState.TRUE)
				return true;
			if (result == TripleState.FALSE)
				return false;
			else
			{
				Symbol other = (Symbol)obj;
				return (this.id == other.id);
			}
		}

		public override int GetHashCode()
		{
			return id;
		}
		
		public override String ToString()
		{
			return name;
		}

		public int Id {get {return id;}}
		public string Name {get {return name;}}
	}
	
	public class SymbolNonterminal : Symbol
	{
		public SymbolNonterminal(int id, string name) : base(id,name)
		{
		}

		public override String ToString()
		{
			return "<"+base.ToString()+">";
		}
	}
	
	public class SymbolTerminal : Symbol
	{
		public SymbolTerminal(int id, string name) : base(id,name)
		{
		}
	}
	
	public class SymbolWhiteSpace : SymbolTerminal
	{
		public SymbolWhiteSpace(int id) : base(id,"(Espacio en blanco)")
		{
		}
	}

	public class SymbolEnd : SymbolTerminal
	{
		public SymbolEnd(int id) : base(id,"(EOF)")
		{}
	}
	public class SymbolCommentStart : SymbolTerminal
	{
		public SymbolCommentStart(int id) : base(id,"(Inicio de Comentario)")
		{}
	}
	public class SymbolCommentEnd : SymbolTerminal
	{
		public SymbolCommentEnd(int id) : base(id,"(Fin de comentario)")
		{}
	}

	public class SymbolCommentLine : SymbolTerminal
	{
		public SymbolCommentLine(int id) : base(id,"(Linea de Comentario)")
		{}
	}

	public class SymbolError : SymbolTerminal
	{
		public SymbolError(int id) : base(id,"(ERROR)")
		{
		}
	}

	public class SymbolCollection : IEnumerable
	{
		static public SymbolEnd EOF = new SymbolEnd(0);
		static public SymbolError ERROR = new SymbolError(1);

		protected IList list;
	
		public SymbolCollection()
		{
			list = new ArrayList();
		}
	
		public IEnumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}

		public void Add(Symbol symbol)
		{
			list.Add(symbol);
		}	

		public Symbol Get(int index)
		{
			return list[index] as Symbol;
		}

		public override string ToString()
		{
			StringBuilder str = new StringBuilder();
			foreach(Symbol symbol in this)
			{
				str.Append(symbol.ToString());
				str.Append(" ");
			}
			if (str.Length > 0)
				str.Remove(str.Length-1,1);
			return str.ToString();
		}


		public Symbol this[int index]
		{
			get {return Get(index);}
		}

	}

}

using System;
using System.Collections;

namespace mini_java_compiler.Parse.lalr
{


	public class AcceptAction  : Action
	{

		public AcceptAction(SymbolTerminal symbol)
		{
			this.symbol = symbol;
		}

		public SymbolTerminal Symbol{get{return (SymbolTerminal)symbol;}}

	}
}

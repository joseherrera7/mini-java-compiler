using System;
using mini_java_compiler.Parse.dfa;

namespace mini_java_compiler.Parse
{
	public interface IStringTokenizer
	{
		string GetInput();
		void SetInput(string input);
		Location GetCurrentLocation();
        void SetCurrentLocation(Location location);
        TerminalToken RetrieveToken();
		bool SkipToChar(char ch);
		bool SkipAfterChar(char ch);
    }

	public class StringTokenizer : IStringTokenizer
	{
		private DFA dfa;
		private DFAInput input;
		public StringTokenizer(DFA dfa)
		{
			this.dfa = dfa;
		}
		public void SetInput(string input)
		{
			this.input = new DFAInput(input);
		}
		public string GetInput()
		{
			return this.input.Text;
		}

		public Location GetCurrentLocation()
		{
			return this.input.Location.Clone();
		}

        public void SetCurrentLocation(Location location)
        {
            this.input.Location = location.Clone();
        }
		public TerminalToken RetrieveToken()
		{
			dfa.Reset();
			Location startLocation = input.Location.Clone();
			AcceptInfo acceptInfo = null;

			if (input.Position >= input.Text.Length)
			{
				return new TerminalToken(SymbolCollection.EOF,
					SymbolCollection.EOF.Name,
					startLocation);
			}

			State newState = dfa.GotoNext(input.ReadChar());
			while (newState != null)
			{
				if (newState is EndState)
				{
					acceptInfo = new AcceptInfo((EndState)newState,input.Location.Clone());
				}
				if (input.IsEof())
					newState = null;
				else
					newState = dfa.GotoNext(input.ReadChar());
			}
			
			if (acceptInfo == null)
			{
				int len = input.Location.Position - startLocation.Position;
				string text = input.Text.Substring(startLocation.Position,len);
				return new TerminalToken(SymbolCollection.ERROR,text,startLocation);
			}
			else
			{
				input.Location = acceptInfo.Location;
				int len = acceptInfo.Location.Position - startLocation.Position;
				string text = input.Text.Substring(startLocation.Position,len);
				return new TerminalToken(acceptInfo.State.AcceptSymbol,text,startLocation);
			}
		}
		public bool SkipToChar(char ch)
		{
			return input.SkipToChar(ch);
		}
		public bool SkipAfterChar(char ch)
		{
			return input.SkipAfterChar(ch);
		}
		

	}
	class DFAInput
	{
		private string text;
		private Location location;
		public DFAInput(string text)
		{
			this.text = text;
			location = new Location(0,0,0);
		}

		public char ReadChar()
		{
			char result = text[Position];
			if (result == '\n')
				location.NextLine();
			else
				location.NextColumn();
			return result;
		}
		public char ReadCharNoUpdate()
		{
			char result = text[Position];
			return result;
		}
		public bool SkipToChar(char ch)
		{
			while (! IsEof())
			{
				char result = ReadCharNoUpdate();
				if (result == ch)
					return true; 
				if (result == '\n')
					location.NextLine();
				else
					location.NextColumn();
			}
			return false;
		}
		public bool SkipAfterChar(char ch)
		{
			while (! IsEof())
			{
				char result = ReadChar();
				if (result == ch)
					return true; 
			}
			return false;
		}
		public bool IsEof()
		{
			return (Position >= text.Length);
		}
		public string Text {get {return text;}}
		public Location Location
		{
			get
			{ return location; }
			set
			{ location = value; }
		}
		public int Position
		{
			get{ return location.Position; }
		}
	}
	class AcceptInfo
	{
		private EndState state;
		private Location location;

		public AcceptInfo(EndState state, Location location)
		{
			this.state    = state;
			this.location = location;
		}
		public EndState State
		{
			get{ return state; }
		}

		public Location Location
		{
			get{ return location;}
		}
	}
	
}

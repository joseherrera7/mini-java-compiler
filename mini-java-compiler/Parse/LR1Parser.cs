using System;
using System.Text;
using mini_java_compiler.Parse.lalr;


namespace mini_java_compiler.Parse
{
	public class LR1Parser
	{

		public enum StoreTokensMode {Always, NoUserObject, Never}

		private IStringTokenizer tokenizer;
		private StateCollection states;
		private State startState;
		private StateStack stateStack;
		private TokenStack tokenStack;
		private TerminalToken lookahead;
		private bool continueParsing;
		private bool accepted;
		private bool trimReductions;
		private StoreTokensMode storeTokens;
		private SymbolCollection symbols;

		public LR1Parser(IStringTokenizer tokenizer,
			StateCollection states,
			State startState,
			SymbolCollection symbols)
		{
			this.tokenizer = tokenizer;
			this.states = states;
			this.startState = startState;
            this.symbols = symbols;
            storeTokens = StoreTokensMode.NoUserObject;
		}

		private void Reset()
		{
			stateStack = new StateStack();
			stateStack.Push(startState);
			tokenStack = new TokenStack();
			lookahead = null;
			continueParsing = true;
			accepted = false;
		}

		public NonterminalToken Parse(String input)
		{
			Reset();
			tokenizer.SetInput(input);

			while (continueParsing)
			{
				TerminalToken token = GetLookahead();
				if (token != null)
					ParseTerminal(token);
			}
			if (accepted)
				return (NonterminalToken)tokenStack.Pop();
			else
				return null;
		}

		private void DoShift(TerminalToken token, ShiftAction action)
		{
			stateStack.Push(action.State);
			tokenStack.Push(token);
			lookahead = null;
			if (OnShift != null)
				OnShift(this,new ShiftEventArgs(token,action.State));
		}

		private void DoReduce(Token token, ReduceAction action)
		{
			int reduceLength = action.Rule.Rhs.Length;

			State currentState;
			bool skipReduce = ((TrimReductions) &&
				(reduceLength == 1) && ( action.Rule.Rhs[0] is SymbolNonterminal));
			if (skipReduce)
			{
				stateStack.Pop();
				currentState = stateStack.Peek();
			}
			else
			{
				Token[] tokens = new Token[reduceLength];
				for (int i = 0; i < reduceLength; i++)
				{
					stateStack.Pop();
					tokens[reduceLength-i-1] = tokenStack.Pop();
				}
				NonterminalToken nttoken = new NonterminalToken(action.Rule,tokens);
				tokenStack.Push(nttoken);
				currentState = stateStack.Peek();

				if (OnReduce != null)
				{
					ReduceEventArgs args = new ReduceEventArgs(action.Rule,
						                                       nttoken,
						                                       currentState);
					OnReduce(this,args);
					DoReleaseTokens(args.Token);

					continueParsing = args.Continue;
				}
			}
            lalr.Action gotoAction = currentState.Actions.Get(action.Rule.Lhs);

			if (gotoAction is GotoAction)
			{
				DoGoto(token,(GotoAction)gotoAction);
			}
			else
			{
				throw new ParserException("Tabla de acciones inválida en el estado");
			}
		}

		private void DoReleaseTokens(NonterminalToken token)
		{
			if ((StoreTokens == StoreTokensMode.Never) ||
				(StoreTokens == StoreTokensMode.NoUserObject &&
				token.UserObject != null))
			{
				token.ClearTokens();
			}
		}

		private void DoAccept(Token token, AcceptAction action)
		{
			continueParsing = false;
			accepted = true;
			if (OnAccept != null)
				OnAccept(this, new AcceptEventArgs((NonterminalToken)tokenStack.Peek()));
		}

		private void DoGoto(Token token, GotoAction action)
		{
			stateStack.Push(action.State);
			if (OnGoto != null)
			{
				OnGoto(this,new GotoEventArgs(action.Symbol,stateStack.Peek()));
			}
		}

		private void ParseTerminal(TerminalToken token)
		{
			State currentState = stateStack.Peek();

            lalr.Action action = currentState.Actions.Get(token.Symbol);

			if (action is ShiftAction)
				DoShift(token,(ShiftAction)action);
			else if (action is ReduceAction)
				DoReduce(token,(ReduceAction)action);
			else if (action is AcceptAction)
				DoAccept(token,(AcceptAction)action);
			else
			{
				continueParsing = false;
				FireParseError(token);
			}
		}

		private void FireParseError(TerminalToken token)
		{
			if (OnParseError != null)
			{
				ParseErrorEventArgs e = 
					new ParseErrorEventArgs(token, FindExpectedTokens());
				OnParseError(this, e);
				continueParsing = e.Continue != ContinueMode.Stop;
				lookahead = e.NextToken;
				if ((e.NextToken != null) && (e.Continue == ContinueMode.Insert))
				    tokenizer.SetCurrentLocation(token.Location);
			}
		}

		private void FireEOFError()
		{
			TerminalToken eofToken = new TerminalToken(SymbolCollection.EOF,
				SymbolCollection.EOF.Name,
				tokenizer.GetCurrentLocation());
			FireParseError(eofToken);
		}

		private SymbolCollection FindExpectedTokens()
		{
			SymbolCollection symbols = new SymbolCollection();
			State state = stateStack.Peek();
			foreach(lalr.Action action in state.Actions)
			{
				if ((action is ShiftAction) || (action is ReduceAction) 
					|| (action is AcceptAction))
				{
					symbols.Add(action.symbol);
				}
			}
			return symbols;
		}

		private bool SkipToEndOfLine()
		{
			bool result = tokenizer.SkipAfterChar('\n');
			if (! result)
			{
				FireEOFError();
			}
			return result;
		}

		private TerminalToken SkipAfterCommentEnd()
		{
			int commentDepth = 1;
			TerminalToken token = null;
			while (commentDepth > 0)
			{
				token = tokenizer.RetrieveToken();
				if (token.Symbol is SymbolCommentEnd)
				{
					commentDepth--;
				}
                else if (token.Symbol is SymbolCommentStart)
                {
                    commentDepth++;
                }
                else if (token.Symbol is SymbolEnd)
				{
					FireEOFError();
					break;
				}
			}
			if (commentDepth == 0)
			    return token;
			else
			    return null;
		}

		private TerminalToken GetLookahead()
		{
			if (lookahead != null)
			{
				return lookahead;
			}
			do
			{
				TerminalToken token = tokenizer.RetrieveToken();
				if (token.Symbol is SymbolCommentLine)
				{
					if (!ProcessCommentLine(token))
						continueParsing = false;
				}
				else if (token.Symbol is SymbolCommentStart)
				{
					if (!ProcessCommentStart(token))
						continueParsing = false;
				}
				else if (token.Symbol is SymbolWhiteSpace)
				{
					if (!ProcessWhiteSpace(token))
						continueParsing = false;
				}
				else if (token.Symbol is SymbolError)
				{
					if (!ProcessError(token))
						continueParsing = false;
				}
				else
				{
					lookahead = token;
				}
				if (!continueParsing)
					break;
			} while (lookahead == null);

			if ((lookahead != null) && (OnTokenRead != null))
			{
				TokenReadEventArgs args = new TokenReadEventArgs(lookahead);
				OnTokenRead(this, args);
				if (args.Continue == false)
				{
					continueParsing = false;
					lookahead = null;
				}
			}
			return lookahead;
		}

		private bool ProcessCommentLine(TerminalToken token)
		{
			if (OnCommentRead == null)
			{
				return SkipToEndOfLine();
			}
			else
			{
				Location start = this.tokenizer.GetCurrentLocation();
				bool result = SkipToEndOfLine();
				if (result)
				{
					Location end = this.tokenizer.GetCurrentLocation();
					string str = this.tokenizer.GetInput();
					int len = end.Position - start.Position;
					string comment = str.Substring(start.Position, len);
					CommentReadEventArgs args = new CommentReadEventArgs(token.Text + comment,
						                                                 comment,
						                                                 true);
					OnCommentRead(this, args);
				}
				return result;
			}
		}

		private bool ProcessCommentStart(TerminalToken token)
		{
			if (OnCommentRead == null)
				return (SkipAfterCommentEnd() != null);
			else
			{
				Location start = this.tokenizer.GetCurrentLocation();
				TerminalToken commentEnd = SkipAfterCommentEnd();
				bool result = commentEnd != null;
				if (result)
				{
					Location end = this.tokenizer.GetCurrentLocation();
					string str = this.tokenizer.GetInput();
					int len = end.Position - start.Position;
					string comment = str.Substring(start.Position, len - commentEnd.Text.Length);
					CommentReadEventArgs args = new CommentReadEventArgs(token.Text + comment,
						                                                 comment,
						                                                 false);
					OnCommentRead(this, args);
				}
				return result;
			}
		}

		private bool ProcessWhiteSpace(Token token)
		{
			return true;
		}

		private bool ProcessError(TerminalToken token)
		{
			if (OnTokenError != null)
			{
				TokenErrorEventArgs e = new TokenErrorEventArgs(token);
				OnTokenError(this, e);
				return e.Continue;
			}
			else
				return false;
		}

		public bool TrimReductions
		{
			get {return trimReductions;} 
			set {this.trimReductions = value;}
		}

		public StoreTokensMode StoreTokens
		{
			get {return storeTokens;}
			set {storeTokens = value;}
		}

        public SymbolCollection Symbols {get{return symbols;}}

		public delegate void TokenReadHandler(LR1Parser parser, TokenReadEventArgs args);
		public delegate void ShiftHandler(LR1Parser parser, ShiftEventArgs args);
		public delegate	void ReduceHandler(LR1Parser parser, ReduceEventArgs args);
		public delegate	void GotoHandler(LR1Parser parser, GotoEventArgs args);
		public delegate void AcceptHandler(LR1Parser parser, AcceptEventArgs args);
		public delegate void TokenErrorHandler(LR1Parser parser, TokenErrorEventArgs args);
		public delegate void ParseErrorHandler(LR1Parser parser, ParseErrorEventArgs args);
		public delegate void CommentReadHandler(LR1Parser parser, CommentReadEventArgs args);

 		public event TokenReadHandler OnTokenRead;

		public event ShiftHandler OnShift;

		public event ReduceHandler OnReduce;

		public event GotoHandler OnGoto;

		public event AcceptHandler OnAccept;

		public event TokenErrorHandler OnTokenError;

		public event ParseErrorHandler OnParseError;

		public event CommentReadHandler OnCommentRead;

	}
}

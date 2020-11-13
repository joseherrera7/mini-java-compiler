using System;
using System.Text.RegularExpressions;

namespace mini_java_compiler.Utilidades
{
	public sealed class StringUtil
	{

		private StringUtil()
		{}

		public static string ShowEscapeChars(string str)
		{
			Regex regex = new Regex("\0|\a|\b|\f|\n|\r|\t|\v");
			MatchEvaluator evaluator = 
				new MatchEvaluator((new StringUtil()).MatchEvent);
			return regex.Replace(str,evaluator);
		}

		private string MatchEvent(Match match)
		{
			string m = match.ToString();
			if (m == "\0")
				return "\\0";
			else if (m == "\a")
				return "\\a";
			else if (m == "\b")
				return "\\b";
			else if (m == "\f")
				return "\\f";
			else if (m == "\n")
				return "\\n";
			else if (m == "\r")
				return "\\r";
			else if (m == "\t")
				return "\\t";
			else if (m == "\v")
				return "\\v";
			else
				return m;
		}


	}
}

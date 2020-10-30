using System;
using System.Runtime.Serialization;

namespace mini_java_compiler.Parse
{

	[Serializable()]
	public class ParserException : System.ApplicationException
	{
		public ParserException(string message) : base(message)
		{
		}

		public ParserException(string message,
			                   Exception inner) : base(message, inner)
		{
		}

		protected ParserException(SerializationInfo info,
			                      StreamingContext context) : base(info, context)
		{
		}

	}

}

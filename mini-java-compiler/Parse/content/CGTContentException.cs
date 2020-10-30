using System;
using System.Runtime.Serialization;
using mini_java_compiler.Parse.content;
using mini_java_compiler.Parse.structure;

namespace mini_java_compiler.Parse.content
{
	[Serializable()]
	public class CGTContentException : System.IO.IOException
	{
		public CGTContentException(string message) : base(message)
		{
		}

		public CGTContentException(string message,
			Exception inner) : base(message, inner)
		{
		}

		protected CGTContentException(SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}

	}
}

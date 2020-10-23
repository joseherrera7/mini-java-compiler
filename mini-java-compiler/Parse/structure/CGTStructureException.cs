using System;
using System.Runtime.Serialization;
using mini_java_compiler.Parse.content;
using mini_java_compiler.Parse.structure;

namespace mini_java_compiler.Parse.structure
{
	/// <summary>
	/// This exception will be thrown when something is wrong in the cgt structure.
	/// For example if the entry type is unknown.
	/// </summary>
	[Serializable()]
	public class CGTStructureException : System.IO.IOException
	{
		public CGTStructureException(string message) : base(message)
		{
		}

		public CGTStructureException(string message,
			Exception inner) : base(message, inner)
		{
		}

		protected CGTStructureException(SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}

	}

}

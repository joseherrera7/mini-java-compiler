using System;
using System.Runtime.Serialization;

namespace mini_java_compiler.Parse.structure
{
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

using System;
using System.Runtime.Serialization;

namespace mini_java_compiler.Parse.content
{
    [Serializable()]
    public class ContentException : System.IO.IOException
    {
        public ContentException(string message) : base(message)
        {
        }

        public ContentException(string message,
            Exception inner) : base(message, inner)
        {
        }

        protected ContentException(SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

    }
}

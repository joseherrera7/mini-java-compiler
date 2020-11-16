using System.IO;
using System.Text;

namespace mini_java_compiler.Utilidades
{

    public class ReaderBin : System.IO.BinaryReader
    {
        public ReaderBin(Stream input) : base(input)
        {
        }

        public ReaderBin(Stream input, Encoding encoding) : base(input, encoding)
        {
        }

        public string ReadUnicodeString()
        {
            StringBuilder builder = new StringBuilder();
            ushort ch = ReadUInt16();
            while (ch != 0)
            {
                builder.Append((char)ch);
                ch = ReadUInt16();
            }
            return builder.ToString();
        }

    }
}

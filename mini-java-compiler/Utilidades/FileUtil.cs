using System.IO;

namespace mini_java_compiler.Utilidades
{

    public sealed class FileUtil
    {
        private FileUtil()
        {
        }

        public static bool IsUTF16LE(Stream stream)
        {
            byte[] startBytes = new byte[2];
            int count = stream.Read(startBytes, 0, startBytes.Length);
            return (count == 2) && (startBytes[0] == 0xFF) && (startBytes[1] == 0xFE);
        }


        public static bool IsUTF16LE(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open);
            bool result = IsUTF16LE(fs);
            fs.Close();
            return result;
        }

    }
}

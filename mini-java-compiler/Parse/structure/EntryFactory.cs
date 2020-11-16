using mini_java_compiler.Utilidades;

namespace mini_java_compiler.Parse.structure
{
    public sealed class EntryFactory
    {
        private EntryFactory()
        {
        }

        static public Entry CreateEntry(ReaderBin reader)
        {
            Entry entry = null;
            byte entryType = reader.ReadByte();
            switch (entryType)
            {
                case 69: // 'E'
                    entry = new EmptyEntry();
                    break;
                case 98: // 'b'
                    entry = new ByteEntry(reader);
                    break;
                case 66: // 'B'
                    entry = new BooleanEntry(reader);
                    break;
                case 73: // 'I'
                    entry = new IntegerEntry(reader);
                    break;
                case 83: // 'S'
                    entry = new StringEntry(reader);
                    break;
                default: //Unknown
                    throw new CGTStructureException("No se sabe el tipo");
            }
            return entry;
        }

    }
}

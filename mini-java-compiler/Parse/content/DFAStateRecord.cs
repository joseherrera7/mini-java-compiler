using mini_java_compiler.Parse.structure;

namespace mini_java_compiler.Parse.content
{
    public class DFAStateRecord
    {
        private int index;
        private bool acceptState;
        private int acceptIndex;
        private EdgeSubRecordCollection edgeSubRecords;

        public DFAStateRecord(Record record)
        {
            if (record.Entries.Count < 5)
                throw new ContentException("Numero invalido de estados");
            byte header = record.Entries[0].ToByteValue();
            if (header != 68) //'D'
                throw new ContentException("Estado invalido ");
            this.index = record.Entries[1].ToIntValue();
            this.acceptState = record.Entries[2].ToBoolValue();
            this.acceptIndex = record.Entries[3].ToIntValue();
            edgeSubRecords = new EdgeSubRecordCollection(record, 5);
        }

        public int Index { get { return index; } }
        public bool AcceptState { get { return acceptState; } }
        public int AcceptIndex { get { return acceptIndex; } }
        public EdgeSubRecordCollection EdgeSubRecords { get { return edgeSubRecords; } }
    }

}

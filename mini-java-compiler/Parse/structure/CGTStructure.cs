namespace mini_java_compiler.Parse.structure
{
    public class CGTStructure
    {
        private string header;
        private RecordCollection records;

        public CGTStructure(string header, RecordCollection records)
        {
            this.header = header;
            this.records = records;
        }

        public override string ToString()
        {
            return header.ToString() + "\n" + records.ToString();
        }

        public string Header { get { return header; } }
        public RecordCollection Records { get { return records; } }

    }
}

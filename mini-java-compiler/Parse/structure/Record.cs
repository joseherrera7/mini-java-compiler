using System.Collections;
using System.Text;

namespace mini_java_compiler.Parse.structure
{

    public class RecordCollection : IEnumerable
    {
        private IList list;

        public RecordCollection()
        {
            list = new ArrayList();
        }

        public IEnumerator GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public int Add(Record record)
        {
            return list.Add(record);
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append("Records:\n");
            foreach (Record record in this)
            {
                str.Append("***START RECORD***\n");
                str.Append(record.ToString());
                str.Append("***END RECORD***\n");
            }
            return str.ToString();
        }

        public Record Get(int index)
        {
            if (index < 0 || index >= list.Count)
                return null;
            else
                return list[index] as Record;
        }



        public Record this[int index]
        {
            get
            {
                return Get(index);
            }
        }

        public int Count { get { return list.Count; } }
    }

    public class Record
    {
        private EntryCollection entries;

        public Record()
        {
            this.entries = new EntryCollection();
        }

        public override string ToString()
        {
            return entries.ToString();
        }

        public EntryCollection Entries { get { return entries; } }
    }
}

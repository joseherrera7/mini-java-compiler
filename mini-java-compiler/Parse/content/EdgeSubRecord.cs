using mini_java_compiler.Parse.structure;
using System.Collections;

namespace mini_java_compiler.Parse.content
{
    public class EdgeSubRecordCollection : IEnumerable
    {
        private IList list;

        public EdgeSubRecordCollection(Record record, int start)
        {
            list = new ArrayList();
            if ((record.Entries.Count - start) % 3 != 0)
                throw new ContentException("Numero invalido de estados");
            for (int i = start; i < record.Entries.Count; i = i + 3)
            {
                EdgeSubRecord edgeRecord = new EdgeSubRecord(record.Entries[i], record.Entries[i + 1]);
                list.Add(edgeRecord);
            }
        }

        public IEnumerator GetEnumerator()
        {
            return list.GetEnumerator();
        }

    }

    public class EdgeSubRecord
    {
        public EdgeSubRecord(Entry charSetEntry, Entry targetEntry)
        {
            this.characterSetIndex = charSetEntry.ToIntValue();
            this.targetIndex = targetEntry.ToIntValue();
        }

        protected int characterSetIndex;
        protected int targetIndex;
        public int CharacterSetIndex { get { return characterSetIndex; } }
        public int TargetIndex { get { return targetIndex; } }
    }
}

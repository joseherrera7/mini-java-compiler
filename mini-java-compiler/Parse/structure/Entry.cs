using System;
using System.Text;
using System.Collections;
using mini_java_compiler.Utilidades;
using mini_java_compiler.Parse.content;
using mini_java_compiler.Parse.structure;

namespace mini_java_compiler.Parse.structure
{
	public class EntryCollection : IEnumerable
	{
		private IList list;

		public EntryCollection()
		{
			list = new ArrayList();
		}
		
		public IEnumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}
		
		public int Add(Entry entry)
		{
			return list.Add(entry);
		}

		public override string ToString()
		{
			StringBuilder str = new StringBuilder();
			foreach (Entry entry in this)
			{
				str.Append(entry.ToString());
				str.Append("\n");
			}
			return str.ToString();
		}
		
		public Entry Get(int index)
		{
			if (index < 0 || index >= list.Count)
				return null;
			else
				return list[index] as Entry;
		}

		public Entry this[int index]
		{
			get
			{
				return Get(index);
			}
		}
		
		public int Count { get{return list.Count;} }
	}

	abstract public class Entry
	{
		public byte ToByteValue()
		{
			ByteEntry entry = this as ByteEntry;
			if (entry == null)
				throw new ContentException("No es byte");
			return entry.Value;
		}

		public bool ToBoolValue()
		{
			BooleanEntry entry = this as BooleanEntry;
			if (entry == null)
				throw new ContentException("No es boolean");
			return entry.Value;
		}

		public int ToIntValue()
		{
			IntegerEntry entry = this as IntegerEntry;
			if (entry == null)
				throw new ContentException("No es integer");
			return entry.Value;
		}
	
		public string ToStringValue()
		{
			StringEntry entry = this as StringEntry;
			if (entry == null)
				throw new ContentException("No es string");
			return entry.Value;
		}
	}
	
	public class EmptyEntry : Entry
	{
	    public EmptyEntry()
	    {
	    }
    
        public override string ToString()
        {
            return "Empty";
         }
    }
	
	public class ByteEntry : Entry
	{
	    private byte value;
	    
	    public ByteEntry(ReaderBin reader)
	    {
	        value = reader.ReadByte();
	    }

        public override string ToString()
        {
            return (String.Format("{0}: {1}", this.GetType(), value));
        }   

        public byte Value {get {return value;}}
	    
	}
	
	public class BooleanEntry : Entry
	{
	    private bool value;
	    
	    public BooleanEntry(ReaderBin reader)
	    {
	        value = reader.ReadBoolean();
	    }
	    
        public override string ToString()
        {
            return (String.Format("{0}: {1}", this.GetType(), value));
        }   
        
        public bool Value {get {return value;}}
	}
	
	public class IntegerEntry : Entry
	{
	    private short value;
	    
	    public IntegerEntry(ReaderBin reader)
	    {
	        value = reader.ReadInt16();
	    }
	    
        public override string ToString()
        {
            return (String.Format("{0}: {1}", this.GetType(), value));
        }   
        
        public short Value {get {return value;}}
	}
	
	public class StringEntry : Entry
	{
	    private string value;
	    
	    public StringEntry(ReaderBin reader)
	    {
	        value = reader.ReadUnicodeString();
	    }
	    
        public override string ToString()
        {
            return (String.Format("{0}: {1}", this.GetType(), value));
        }   
        
        public string Value {get {return value;}}
	}
}

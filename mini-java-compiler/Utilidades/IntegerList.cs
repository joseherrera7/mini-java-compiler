using System;
using System.Collections;
using System.Text;

namespace mini_java_compiler.Utilidades
{

    public class IntegerList
    {
        private ArrayList list;


        public IntegerList()
        {
            list = new ArrayList();
        }

        public int Add(int value)
        {
            return list.Add(value);
        }

        public void Clear()
        {
            list.Clear();
        }

        public virtual object Clone()
        {
            IntegerList result = new IntegerList();
            result.list = (ArrayList)list.Clone();
            return result;
        }

        public bool Contains(int value)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (this[i] == value)
                    return true;
            }
            return false;
        }

        public void CopyTo(Array array)
        {
            list.CopyTo(array);
        }

        public void CopyTo(Array array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public void CopyTo(int index, Array array, int arrayIndex, int count)
        {
            list.CopyTo(index, array, arrayIndex, count);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is IntegerList))
                return false;
            IntegerList otherList = (IntegerList)obj;
            if (otherList.Count != this.Count)
                return false;
            for (int i = 0; i < this.list.Count; i++)
            {
                if (this[i] != otherList[i])
                    return false;
            }
            return true;
        }

        public new static bool Equals(object objA, object objB)
        {
            return objA.Equals(objB);
        }

        public IEnumerator GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public IEnumerator GetEnumerator(int index, int count)
        {
            return list.GetEnumerator(index, count);
        }

        public override int GetHashCode()
        {
            return (list.Count * (this[0] + this[list.Count - 1]).GetHashCode());
        }

        public IntegerList GetRange(int index, int count)
        {
            IntegerList result = new IntegerList();
            result.list = this.list.GetRange(index, count);
            return result;
        }

        public int IndexOf(int value)
        {
            return IndexOf(value, 0, list.Count);
        }

        public int IndexOf(int value, int startIndex)
        {
            return IndexOf(value, startIndex, list.Count - startIndex);
        }

        public int IndexOf(int value, int startIndex, int count)
        {
            if ((startIndex < 0) || (startIndex >= list.Count))
                throw new ArgumentOutOfRangeException();
            if (count < 0)
                throw new ArgumentOutOfRangeException();
            if (startIndex + count > list.Count)
                throw new ArgumentOutOfRangeException();
            for (int i = startIndex; i < startIndex + count; i++)
            {
                if (this[i] == value)
                    return i;
            }
            return -1;
        }

        public void Insert(int index, int value)
        {
            list.Insert(index, value);
        }

        public void InsertRange(int index, IntegerList other)
        {
            for (int i = 0; i < other.Count; i++)
            {
                list[index + i] = other[i];
            }
        }

        public int LastIndexOf(int value)
        {
            return LastIndexOf(value, list.Count - 1, list.Count);
        }


        public int LastIndexOf(int value, int startIndex)
        {
            return LastIndexOf(value, startIndex, startIndex + 1);
        }


        public int LastIndexOf(int value, int startIndex, int count)
        {
            if ((startIndex < 0) || (startIndex >= list.Count))
                throw new ArgumentOutOfRangeException();
            if (count < 0)
                throw new ArgumentOutOfRangeException();
            if (startIndex - count < -1)
                throw new ArgumentOutOfRangeException();
            for (int i = startIndex; i < startIndex - count; i--)
            {
                if (this[i] == value)
                    return i;
            }
            return -1;
        }

        public void Remove(int value)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (this[i] == value)
                {
                    list.RemoveAt(i);
                    return;
                }
            }
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        public void RemoveRange(int index, int count)
        {
            list.RemoveRange(index, count);
        }

        public static IntegerList Repeat(int value, int count)
        {
            IntegerList result = new IntegerList();
            for (int i = 0; i < count; i++)
            {
                result.Add(value);
            }
            return result;
        }

        public void Reverse()
        {
            list.Reverse();
        }

        public void Reverse(int index, int count)
        {
            list.Reverse(index, count);
        }

        public void SetRange(int index, IntegerList otherList)
        {
            list.SetRange(index, otherList.list);
        }

        public void Sort()
        {
            list.Sort();
        }

        public void Sort(IComparer comparer)
        {
            list.Sort(comparer);
        }

        public void Sort(int index, int count, IComparer comparer)
        {
            list.Sort(index, count, comparer);
        }

        public static IntegerList Synchronized(IntegerList list)
        {
            IntegerList result = new IntegerList();
            result.list = ArrayList.Synchronized(list.list);
            return result;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            for (int i = 0; i < list.Count; i++)
            {
                builder.Append(this[i]);
                if (i < list.Count - 1)
                    builder.Append(", ");
            }
            builder.Append("]");
            return builder.ToString();
        }

        public void TrimToSize()
        {
            list.TrimToSize();
        }

        public int Capacity { get { return list.Capacity; } }

        public int Count { get { return list.Count; } }

        public bool IsFixedSize { get { return list.IsFixedSize; } }

        public bool IsReadOnly { get { return list.IsReadOnly; } }

        public bool IsSynchronized { get { return list.IsSynchronized; } }

        public int this[int index] { get { return (Int32)list[index]; } }

        public object SyncRoot { get { return list.SyncRoot; } }


    }
}

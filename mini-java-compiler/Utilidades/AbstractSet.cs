using System;
using System.Collections;

namespace mini_java_compiler.Utilidades
{

    public abstract class AbstractSet : ISet
    {

        protected ICollection collection;


        public AbstractSet(ICollection collection)
        {
            this.collection = collection;
        }

        public virtual int Count
        {
            get { return collection.Count; }
        }


        public bool IsSynchronized
        {
            get { return collection.IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return SyncRoot; }
        }


        public void CopyTo(
            Array array,
            int index
            )
        {
            collection.CopyTo(array, index);
        }

        public abstract void Add(Object obj);
        public abstract void Clear();
        public abstract bool Contains(Object obj);
        public abstract void Remove(Object obj);

        abstract public IEnumerator GetEnumerator();


    }
}

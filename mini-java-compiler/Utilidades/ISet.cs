using System;
using System.Collections;

namespace mini_java_compiler.Utilidades
{
    public interface ISet : ICollection
    {
        void Add(Object obj);

        void Clear();
        bool Contains(Object obj);

        void Remove(Object obj);


    }
}

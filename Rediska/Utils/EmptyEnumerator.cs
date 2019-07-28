using System;
using System.Collections;
using System.Collections.Generic;

namespace Rediska.Utils
{
    internal sealed class EmptyEnumerator<T> : IEnumerator<T>
    {
        public static EmptyEnumerator<T> Singleton { get; } = new EmptyEnumerator<T>();

        public void Dispose()
        {
        }

        public bool MoveNext() => false;

        public void Reset()
        {
        }

        public T Current => throw new InvalidOperationException();
        object IEnumerator.Current => Current;
    }
}
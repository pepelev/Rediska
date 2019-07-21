using System;

namespace Rediska.Tests
{
    public struct Resource<T> : IDisposable
    {
        private sealed class NullDisposable : IDisposable
        {
            public static NullDisposable Singleton { get; } = new NullDisposable();

            public void Dispose()
            {
            }
        }

        public T Value { get; }
        private readonly IDisposable disposable;

        public Resource(T value)
            : this(value, NullDisposable.Singleton)
        {
        }

        public Resource(T value, IDisposable disposable)
        {
            Value = value;
            this.disposable = disposable;
        }

        public void Dispose()
        {
            disposable.Dispose();
        }

        public static implicit operator T(Resource<T> disposable) => disposable.Value;
    }
}
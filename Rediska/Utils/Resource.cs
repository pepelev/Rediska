using System;

namespace Rediska.Utils
{
    public struct Resource<T> : IDisposable
    {
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

        public Resource<TAnother> Move<TAnother>(TAnother another) => new Resource<TAnother>(another, disposable);

        public void Dispose()
        {
            disposable.Dispose();
        }

        public static implicit operator T(Resource<T> disposable) => disposable.Value;
        public static implicit operator Resource<T>(T value) => new Resource<T>(value);
    }
}
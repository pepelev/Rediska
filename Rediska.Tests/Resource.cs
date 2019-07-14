using System;

namespace Rediska.Tests
{
    public struct Resource<T> : IDisposable
    {
        public T Value { get; }
        private readonly IDisposable disposable;

        public Resource(T value, IDisposable disposable)
        {
            this.disposable = disposable;
            Value = value;
        }

        public void Dispose()
        {
            disposable.Dispose();
        }

        public static implicit operator T(Resource<T> disposable) => disposable.Value;
    }
}
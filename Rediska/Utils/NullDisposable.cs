using System;

namespace Rediska.Utils
{
    internal sealed class NullDisposable : IDisposable
    {
        public static NullDisposable Singleton { get; } = new NullDisposable();

        public void Dispose()
        {
        }
    }
}
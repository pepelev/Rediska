using System;

namespace Rediska.Tests
{
    public sealed class ChunkList<T>
    {
        public struct Cursor
        {
            public void Cut()
            {
                throw new NotImplementedException();
            }
        }

        public void Add(ArraySegment<T> chunk)
        {
            
        }
    }
}
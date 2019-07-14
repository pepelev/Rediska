using System.IO;

namespace Rediska.Tests
{
    public abstract class BulkStringResponse
    {
        public static BulkStringResponse Null { get; } = new NullResponse();

        public abstract bool IsNull { get; }
        public abstract long Length { get; }
        public abstract void Write(Stream stream);

        private sealed class NullResponse : BulkStringResponse
        {
            public override bool IsNull => true;
            public override long Length => -1;

            public override void Write(Stream stream)
            {
            }
        }
    }
}
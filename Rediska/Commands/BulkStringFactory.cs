namespace Rediska.Commands
{
    using Protocol;
    using Scripting;

    public abstract class BulkStringFactory
    {
        public static BulkStringFactory Plain { get; } = new PlainFactory();
        public abstract BulkString Utf8(string content);
        public abstract BulkString Create(byte[] content);
        public abstract BulkString Create(double content);
        public abstract BulkString Create(long content);
        public abstract BulkString Create(in Sha1 content);

        private sealed class PlainFactory : BulkStringFactory
        {
            public override BulkString Utf8(string content) => new PlainBulkString(content);
            public override BulkString Create(byte[] content) => new PlainBulkString(content);
            public override BulkString Create(double content) => content.ToBulkString();
            public override BulkString Create(long content) => content.ToBulkString();
            public override BulkString Create(in Sha1 content) => content.ToBulkString();
        }
    }
}
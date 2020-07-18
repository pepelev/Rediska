namespace Rediska.Commands.Streams
{
    using Auxiliary;
    using Protocol;

    public readonly struct Offset
    {
        public static Offset EndOfStream => new Offset(Type.EndOfStream, default);
        private readonly Type type;
        private readonly Id id;

        private Offset(Type type, Id id)
        {
            this.type = type;
            this.id = id;
        }

        public static implicit operator Offset(Id value) => FromId(value);
        public static Offset FromId(in Id value) => new Offset(Type.Id, value);

        public BulkString ToBulkString(BulkStringFactory factory) => type switch
        {
            Type.EndOfStream => new PlainBulkString("$"),
            _ => id.ToBulkString(factory, Id.Print.SkipMinimalLow)
        };

        public override string ToString() => new PlainCommand(ToBulkString(BulkStringFactory.Plain)).ToString();

        private enum Type : byte
        {
            EndOfStream = 0,
            Id = 1
        }
    }
}
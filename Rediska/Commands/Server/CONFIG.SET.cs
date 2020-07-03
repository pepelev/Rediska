namespace Rediska.Commands.Server
{
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public static partial class CONFIG
    {
        public sealed class SET : Command<None>
        {
            private static readonly PlainBulkString subName = new PlainBulkString("SET");
            private readonly string parameter;
            private readonly string value;

            public SET(string parameter, string value)
            {
                this.parameter = parameter;
                this.value = value;
            }

            public override DataType Request => new PlainArray(
                name,
                subName,
                new PlainBulkString(parameter),
                new PlainBulkString(value)
            );

            public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
        }
    }
}
namespace Rediska.Commands.Server
{
    using System.Collections.Generic;
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

            public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
            {
                name,
                subName,
                factory.Utf8(parameter),
                factory.Utf8(value)
            };

            public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
        }
    }
}
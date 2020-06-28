namespace Rediska.Commands.Geo
{
    using System.Collections.Generic;
    using System.Linq;
    using Protocol;
    using Protocol.Visitors;

    public sealed class OnlyName : ResponseFormat<Key>
    {
        public static OnlyName Singleton { get; } = new OnlyName();

        public override IEnumerable<BulkString> AdditionalArguments => Enumerable.Empty<BulkString>();

        public override Key Parse(DataType item) => new Key.BulkString(
            item.Accept(BulkStringExpectation.Singleton)
        );
    }
}
using System.Collections.Generic;
using Rediska.Protocol.Requests;
using Rediska.Protocol.Responses.Visitors;
using Rediska.Utils;

namespace Rediska.Commands.Keys
{
    public sealed class DEL : Command<long>
    {
        private readonly IReadOnlyCollection<Key> keys;

        public DEL(params Key[] keys)
            : this(keys as IReadOnlyCollection<Key>)
        {
        }

        public DEL(IReadOnlyCollection<Key> keys)
        {
            this.keys = keys;
        }

        public override DataType Request => new Array(
            new PrefixedCollection<DataType>(
                new BulkString("DEL"),
                new ProjectingCollection<Key, DataType>(
                    keys,
                    key => key.ToBulkString()
                )
            )
        );

        public override Visitor<long> ResponseStructure => IntegerExpectation.Singleton;
    }
}
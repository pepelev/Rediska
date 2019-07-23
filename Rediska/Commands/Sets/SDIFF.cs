using System.Collections.Generic;
using Rediska.Protocol.Responses;
using Rediska.Protocol.Responses.Visitors;
using Rediska.Utils;
using Array = Rediska.Protocol.Requests.Array;
using DataType = Rediska.Protocol.Requests.DataType;

namespace Rediska.Commands.Sets
{
    public sealed class SDIFF : Command<IReadOnlyList<BulkString>>
    {
        private static readonly Protocol.Requests.BulkString name = new Protocol.Requests.BulkString("SDIFF");

        private static readonly ListVisitor<BulkString> responseStructure = new ListVisitor<BulkString>(
            ArrayExpectation.Singleton,
            BulkStringExpectation.Singleton
        );

        private readonly Key minuend;
        private readonly IReadOnlyCollection<Key> subtrahends;

        public SDIFF(Key minuend, params Key[] subtrahends)
            : this(minuend, subtrahends as IReadOnlyCollection<Key>)
        {
        }

        public SDIFF(Key minuend, IReadOnlyCollection<Key> subtrahends)
        {
            this.minuend = minuend;
            this.subtrahends = subtrahends;
        }

        public override DataType Request => new Array(
            new PrefixedCollection<DataType>(
                name,
                new ProjectingCollection<Key, DataType>(
                    new PrefixedCollection<Key>(
                        minuend,
                        subtrahends
                    ),
                    key => key.ToBulkString()
                )
            )
        );

        public override Visitor<IReadOnlyList<BulkString>> ResponseStructure => responseStructure;
    }
}
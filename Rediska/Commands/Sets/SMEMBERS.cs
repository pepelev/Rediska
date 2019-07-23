using System.Collections.Generic;
using Rediska.Protocol.Responses;
using Rediska.Protocol.Responses.Visitors;
using Array = Rediska.Protocol.Requests.Array;
using DataType = Rediska.Protocol.Requests.DataType;

namespace Rediska.Commands.Sets
{
    public sealed class SMEMBERS : Command<IReadOnlyList<BulkString>>
    {
        private static readonly Protocol.Requests.BulkString name = new Protocol.Requests.BulkString("SMEMBERS");

        private static readonly ListVisitor<BulkString> responseStructure = new ListVisitor<BulkString>(
            ArrayExpectation.Singleton,
            BulkStringExpectation.Singleton
        );

        private readonly Key key;

        public SMEMBERS(Key key)
        {
            this.key = key;
        }

        public override DataType Request => new Array(
            name,
            key.ToBulkString()
        );

        public override Visitor<IReadOnlyList<BulkString>> ResponseStructure => responseStructure;
    }
}
using System;
using Rediska.Utils;

namespace Rediska.Protocol.Responses.Visitors
{
    public sealed class OkExpectation : Visitor<None>
    {
        public static OkExpectation Singleton { get; } = new OkExpectation();

        public override None Visit(Integer integer)
        {
            throw new ArgumentException("OK expected");
        }

        public override None Visit(SimpleString simpleString)
        {
            if (simpleString.Content == "OK")
                return new None();

            throw new ArgumentException("OK expected");
        }

        public override None Visit(Error error)
        {
            throw new ArgumentException("OK expected");
        }

        public override None Visit(Array array)
        {
            throw new ArgumentException("OK expected");
        }

        public override None Visit(BulkString bulkString)
        {
            throw new ArgumentException("OK expected");
        }
    }
}
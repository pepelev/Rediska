namespace Rediska.Commands.Connection
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net;
    using Protocol;
    using Protocol.Visitors;

    public static partial class CLIENT
    {
        public sealed class KILL : Command<KILL.Response>
        {
            private static readonly PlainBulkString subName = new PlainBulkString("KILL");
            private static readonly ResponseVisitor responseStructure = new ResponseVisitor();
            private static readonly PlainBulkString skipMe = new PlainBulkString("SKIPME");
            private static readonly PlainBulkString no = new PlainBulkString("no");
            private readonly IReadOnlyList<Filter> filters;
            private readonly Mode mode;

            public KILL(IPEndPoint endPoint)
                : this(new SimpleAddress(endPoint))
            {
            }

            public KILL(params Filter[] filters)
                : this(filters, Mode.SkipMe)
            {
            }

            public KILL(IReadOnlyList<Filter> filters, Mode mode)
            {
                if (filters == null)
                    throw new ArgumentNullException(nameof(filters));

                if (filters.Count == 0)
                    throw new ArgumentException("Must contain items", nameof(filters));

                if (mode != Mode.SkipMe && mode != Mode.NotSkipMe)
                    throw new ArgumentException($"Must be either SkipMe or NotSkipMe, but {mode} found", nameof(mode));

                this.filters = filters;
                this.mode = mode;
            }

            public override IEnumerable<BulkString> Request(BulkStringFactory factory)
            {
                yield return name;
                yield return subName;
                foreach (var filter in filters)
                {
                    foreach (var argument in filter.Arguments(factory))
                    {
                        yield return argument;
                    }
                }

                if (mode == Mode.NotSkipMe)
                {
                    yield return skipMe;
                    yield return no;
                }
            }

            public override Visitor<Response> ResponseStructure => responseStructure;

            public enum Mode : byte
            {
                SkipMe = 0,
                NotSkipMe = 1
            }

            public readonly struct Response
            {
                public Response(long clientsKilled)
                {
                    ClientsKilled = clientsKilled;
                }

                public long ClientsKilled { get; }

                public override string ToString() =>
                    $"{nameof(ClientsKilled)}: {ClientsKilled.ToString(CultureInfo.InvariantCulture)}";
            }

            private sealed class ResponseVisitor : Expectation<Response>
            {
                public override string Message => "OK or integer";
                public override Response Visit(Integer integer) => new Response(integer.Value);
                public override Response Visit(SimpleString simpleString) => new Response(1);
            }

            public abstract class Filter
            {
                public abstract IEnumerable<BulkString> Arguments(BulkStringFactory factory);
            }

            private sealed class SimpleAddress : Filter
            {
                private readonly IPEndPoint endPoint;

                public SimpleAddress(IPEndPoint endPoint)
                {
                    this.endPoint = endPoint;
                }

                public override IEnumerable<BulkString> Arguments(BulkStringFactory factory)
                {
                    var address = $"{endPoint.Address}:{endPoint.Port.ToString(CultureInfo.InvariantCulture)}";
                    yield return factory.Utf8(address);
                }
            }

            public sealed class Id : Filter
            {
                private static readonly PlainBulkString id = new PlainBulkString("ID");
                private readonly ClientId clientId;

                public Id(ClientId clientId)
                {
                    this.clientId = clientId;
                }

                public override IEnumerable<BulkString> Arguments(BulkStringFactory factory)
                {
                    yield return id;
                    yield return clientId.ToBulkString(factory);
                }
            }

            public sealed class Type : Filter
            {
                private static readonly PlainBulkString type = new PlainBulkString("TYPE");
                public static Type Normal { get; } = new Type("normal");
                public static Type Master { get; } = new Type("master");
                public static Type Slave { get; } = new Type("slave");
                public static Type Replica { get; } = new Type("replica");
                public static Type Pubsub { get; } = new Type("pubsub");
                private readonly BulkString[] arguments;

                public Type(BulkString argument)
                {
                    arguments = new[] {type, argument};
                }

                public override IEnumerable<BulkString> Arguments(BulkStringFactory factory) => arguments;
            }

            public sealed class Address : Filter
            {
                private static readonly PlainBulkString addressArgument = new PlainBulkString("ADDR");
                private readonly IPEndPoint endPoint;

                public Address(IPEndPoint endPoint)
                {
                    this.endPoint = endPoint;
                }

                public override IEnumerable<BulkString> Arguments(BulkStringFactory factory)
                {
                    yield return addressArgument;
                    var address = $"{endPoint.Address}:{endPoint.Port.ToString(CultureInfo.InvariantCulture)}";
                    yield return factory.Utf8(address);
                }
            }
        }
    }
}
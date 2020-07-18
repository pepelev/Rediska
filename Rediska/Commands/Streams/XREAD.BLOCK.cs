namespace Rediska.Commands.Streams
{
    using System.Collections;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed partial class XREAD
    {
        public sealed class BLOCK : Command<BLOCK.Response>
        {
            private static readonly PlainBulkString block = new PlainBulkString("BLOCK");

            private static readonly Visitor<Response> blockResponseStructure = Protocol.Visitors.Id.Singleton
                .Then(reply => new Response(reply));

            private readonly Count count;
            private readonly MillisecondsTimeout blockTimeout;
            private readonly IReadOnlyList<(Key Key, Offset Offset)> streams;

            public BLOCK(Count count, MillisecondsTimeout blockTimeout, params (Key Key, Offset Offset)[] streams)
                : this(count, blockTimeout, streams as IReadOnlyList<(Key Key, Offset Offset)>)
            {
            }

            public BLOCK(
                Count count,
                MillisecondsTimeout blockTimeout,
                IReadOnlyList<(Key Key, Offset Offset)> streams)
            {
                this.count = count;
                this.blockTimeout = blockTimeout;
                this.streams = streams;
            }

            public override IEnumerable<BulkString> Request(BulkStringFactory factory)
            {
                yield return name;
                foreach (var argument in count.Arguments(factory))
                {
                    yield return argument;
                }

                yield return block;
                yield return blockTimeout.ToBulkString(factory);

                yield return streamsArgument;
                foreach (var (key, _) in streams)
                {
                    yield return key.ToBulkString(factory);
                }

                foreach (var (_, offset) in streams)
                {
                    yield return offset.ToBulkString(factory);
                }
            }

            public override Visitor<Response> ResponseStructure => blockResponseStructure;

            public readonly struct Response : IReadOnlyList<Entries>
            {
                private readonly DataType reply;

                public Response(DataType reply)
                {
                    this.reply = reply;
                }

                public Outcome Outcome => Count > 0
                    ? Outcome.Ok
                    : Outcome.Timeout;

                private IReadOnlyList<Entries> Content => reply.Accept(ResponseVisitor.Singleton);
                public IEnumerator<Entries> GetEnumerator() => Content.GetEnumerator();
                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
                public int Count => Content.Count;
                public Entries this[int index] => Content[index];
            }

            private sealed class ResponseVisitor : Expectation<IReadOnlyList<Entries>>
            {
                public static ResponseVisitor Singleton { get; } = new ResponseVisitor();
                public override string Message => "Array or null";

                public override IReadOnlyList<Entries> Visit(Array array)
                {
                    return array.IsNull
                        ? System.Array.Empty<Entries>()
                        : array.Accept(responseStructure);
                }

                public override IReadOnlyList<Entries> Visit(BulkString bulkString)
                {
                    return bulkString.IsNull
                        ? System.Array.Empty<Entries>()
                        : throw Exception(bulkString);
                }
            }

            public enum Outcome : byte
            {
                Ok = 0,
                Timeout = 1
            }
        }
    }
}
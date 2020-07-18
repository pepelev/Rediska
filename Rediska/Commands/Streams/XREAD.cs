namespace Rediska.Commands.Streams
{
    using System.Collections;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed partial class XREAD : Command<IReadOnlyList<Entries>>
    {
        private static readonly PlainBulkString name = new PlainBulkString("XREAD");
        private static readonly PlainBulkString streamsArgument = new PlainBulkString("STREAMS");
        private readonly Count count;
        private readonly IReadOnlyList<(Key Key, Id Id)> streams;

        private static readonly ListVisitor<Entries> responseStructure = new ListVisitor<Entries>(
            ArrayExpectation.Singleton,
            ArrayExpectation2.Singleton.Then(array => new Entries(array))
        );

        public XREAD(Count count, params (Key Key, Id Id)[] streams)
            : this(count, streams as IReadOnlyList<(Key Key, Id Id)>)
        {
        }

        public XREAD(Count count, IReadOnlyList<(Key Key, Id Id)> streams)
        {
            this.count = count;
            this.streams = streams;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory)
        {
            yield return name;
            foreach (var argument in count.Arguments(factory))
            {
                yield return argument;
            }

            yield return streamsArgument;
            foreach (var (key, _) in streams)
            {
                yield return key.ToBulkString(factory);
            }

            foreach (var (_, id) in streams)
            {
                yield return id.ToBulkString(factory, Id.Print.SkipMinimalLow);
            }
        }

        public override Visitor<IReadOnlyList<Entries>> ResponseStructure => responseStructure;
    }
}
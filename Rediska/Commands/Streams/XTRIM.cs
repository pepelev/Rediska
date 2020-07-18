namespace Rediska.Commands.Streams
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class XTRIM : Command<XTRIM.Response>
    {
        private static readonly PlainBulkString name = new PlainBulkString("XTRIM");

        private static readonly Visitor<Response> responseStructure = IntegerExpectation.Singleton
            .Then(deletedEntries => new Response(deletedEntries));

        private readonly Key key;
        private readonly MaximumLengthTrim trim;

        public XTRIM(Key key, MaximumLengthTrim trim)
        {
            this.key = key ?? throw new ArgumentNullException(nameof(key));
            this.trim = trim;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new ConcatList<BulkString>(
            new[]
            {
                name,
                key.ToBulkString(factory)
            },
            trim.ToBulkStrings(factory)
        );

        public override Visitor<Response> ResponseStructure => responseStructure;

        public readonly struct Response
        {
            public Response(long deletedEntries)
            {
                DeletedEntries = deletedEntries;
            }

            public long DeletedEntries { get; }

            public override string ToString() =>
                $"{nameof(DeletedEntries)}: {DeletedEntries.ToString(CultureInfo.InvariantCulture)}";
        }
    }
}
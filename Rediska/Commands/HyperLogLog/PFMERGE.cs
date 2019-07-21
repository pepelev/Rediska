using System.Collections.Generic;
using Rediska.Protocol.Requests;
using Rediska.Protocol.Responses.Visitors;
using Rediska.Utils;

namespace Rediska.Commands.HyperLogLog
{
    public sealed class PFMERGE : Command<None>
    {
        private readonly Key destination;
        private readonly IReadOnlyCollection<Key> sources;

        public PFMERGE(Key destination, IReadOnlyCollection<Key> sources)
        {
            this.destination = destination;
            this.sources = sources;
        }

        public PFMERGE(Key destination, params Key[] sources)
            : this(destination, sources as IReadOnlyCollection<Key>)
        {
        }

        public override DataType Request => new Array(
            new ConcatCollection<DataType>(
                new DataType[]
                {
                    new BulkString("PFMERGE"),
                    destination.ToBulkString()
                },
                new ProjectingCollection<Key, DataType>(
                    sources,
                    source => source.ToBulkString()
                )
            )
        );

        public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
    }
}
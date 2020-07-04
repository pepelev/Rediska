namespace Rediska.Commands.Scripting
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public static partial class SCRIPT
    {
        public sealed class EXISTS : Command<IReadOnlyList<(Sha1 Sha1, bool Exists)>>
        {
            private static readonly PlainBulkString subName = new PlainBulkString("EXISTS");
            private static readonly PlainBulkString[] prefix = {name, subName};
            private readonly IReadOnlyList<Sha1> hashes;

            public EXISTS(params Sha1[] hashes)
                : this(hashes as IReadOnlyList<Sha1>)
            {
            }

            public EXISTS(IReadOnlyList<Sha1> hashes)
            {
                if (hashes.Count < 1)
                {
                    throw new ArgumentException("Must contain elements", nameof(hashes));
                }

                this.hashes = hashes;
            }

            public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new ConcatList<BulkString>(
                prefix,
                new ProjectingReadOnlyList<Sha1, BulkString>(
                    hashes,
                    sha1 => factory.Create(sha1)
                )
            );

            public override Visitor<IReadOnlyList<(Sha1 Sha1, bool Exists)>> ResponseStructure => CompositeVisitors.IntegerList
                .Then(
                    response => new PrettyReadOnlyList<(Sha1 Sha1, bool Exists)>(
                        new PairwiseReadOnlyList<Sha1, bool>(
                            hashes,
                            new ProjectingReadOnlyList<long, bool>(
                                response,
                                integer => integer switch
                                {
                                    0 => false,
                                    1 => true,
                                    _ => throw new ArgumentOutOfRangeException(nameof(integer), integer, "Must be either 0 or 1")
                                }
                            )
                        )
                    ) as IReadOnlyList<(Sha1 Sha1, bool Exists)>
                );
        }
    }
}
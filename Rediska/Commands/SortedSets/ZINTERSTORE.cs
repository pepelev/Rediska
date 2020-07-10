namespace Rediska.Commands.SortedSets
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;
    using Array = System.Array;

    public sealed class ZINTERSTORE : Command<ZINTERSTORE.Response>
    {
        private static readonly PlainBulkString name = new PlainBulkString("ZINTERSTORE");
        private static readonly PlainBulkString weightsOption = new PlainBulkString("WEIGHTS");
        private static readonly PlainBulkString aggregate = new PlainBulkString("AGGREGATE");
        private static readonly PlainBulkString min = new PlainBulkString("MIN");
        private static readonly PlainBulkString max = new PlainBulkString("MAX");

        private static readonly Visitor<Response> responseStructure = IntegerExpectation.Singleton
            .Then(numberOfMembers => new Response(numberOfMembers));

        private readonly Key destination;
        private readonly IReadOnlyList<Key> keys;
        private readonly IReadOnlyList<double> weights;
        private readonly Aggregation aggregation;

        public ZINTERSTORE(Key destination, IReadOnlyList<Key> keys, Aggregation aggregation = Aggregation.Sum)
            : this(destination, keys, Array.Empty<double>(), aggregation)
        {
        }

        public ZINTERSTORE(
            Key destination,
            IReadOnlyList<(double Weight, Key Key)> sources,
            Aggregation aggregation = Aggregation.Sum)
            : this(
                destination,
                new ProjectingReadOnlyList<(double Weight, Key Key), Key>(sources, pair => pair.Key),
                new ProjectingReadOnlyList<(double Weight, Key Key), double>(sources, pair => pair.Weight),
                aggregation
            )
        {
        }

        private ZINTERSTORE(
            Key destination,
            IReadOnlyList<Key> keys,
            IReadOnlyList<double> weights,
            Aggregation aggregation)
        {
            ValidateAggregation(aggregation);
            this.destination = destination;
            this.keys = keys;
            this.weights = weights;
            this.aggregation = aggregation;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory)
        {
            yield return name;
            yield return destination.ToBulkString(factory);
            yield return factory.Create(keys.Count);

            foreach (var key in keys)
            {
                yield return key.ToBulkString(factory);
            }

            if (weights.Count > 0)
            {
                yield return weightsOption;
                foreach (var weight in weights)
                {
                    yield return factory.Create(weight);
                }
            }

            if (aggregation == Aggregation.Minimum)
            {
                yield return aggregate;
                yield return min;
            }
            else if (aggregation == Aggregation.Maximum)
            {
                yield return aggregate;
                yield return max;
            }
        }

        public override Visitor<Response> ResponseStructure => responseStructure;

        private static void ValidateAggregation(Aggregation aggregation)
        {
            switch (aggregation)
            {
                case Aggregation.Sum:
                case Aggregation.Minimum:
                case Aggregation.Maximum:
                    break;
                default:
                {
                    throw new ArgumentException(
                        $"Must be Sum, Minimum or Maximum, but {aggregation} found",
                        nameof(aggregation)
                    );
                }
            }
        }

        public readonly struct Response
        {
            public Response(long numberOfMembersInResultingSet)
            {
                NumberOfMembersInResultingSet = numberOfMembersInResultingSet;
            }

            public long NumberOfMembersInResultingSet { get; }

            public override string ToString() =>
                $"{nameof(NumberOfMembersInResultingSet)}: {NumberOfMembersInResultingSet}";
        }
    }
}
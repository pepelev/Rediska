namespace Rediska.Commands.Geo
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public static class GEORADIUSBYMEMBER
    {
        internal static PlainBulkString Name { get; } = new PlainBulkString("GEORADIUSBYMEMBER");
        internal static PlainBulkString Ascending { get; } = new PlainBulkString("ASC");
        internal static PlainBulkString Descending { get; } = new PlainBulkString("DESC");
        internal static PlainBulkString Store { get; } = new PlainBulkString("STORE");
        internal static PlainBulkString StoreDistance { get; } = new PlainBulkString("STOREDIST");

        public static GEORADIUSBYMEMBER<Key> OnlyNames(
            Key key,
            Key member,
            Distance radius,
            long? count = null,
            Sorting sorting = Sorting.None)
            => new GEORADIUSBYMEMBER<Key>(key, member, radius, OnlyName.Singleton, count, sorting);

        public sealed class STORE : Command<StoreResponse>
        {
            private readonly Key key;
            private readonly Location location;
            private readonly Distance radius;
            private readonly long? count;
            private readonly StoringMode storingMode;
            private readonly Key target;

            public STORE(
                Key key,
                Location location,
                Distance radius,
                long? count,
                StoringMode storingMode,
                Key target)
            {
                if (count is { } @long && @long <= 0)
                    throw new ArgumentOutOfRangeException(nameof(count), count, "Must be positive");

                if (storingMode != StoringMode.WithRawGeohash && storingMode != StoringMode.WithDistance)
                {
                    throw new ArgumentException(
                        $"Expected either WithRawGeohash or WithDistance, but was {storingMode}",
                        nameof(storingMode)
                    );
                }

                this.key = key;
                this.location = location;
                this.radius = radius;
                this.target = target;
                this.count = count;
                this.storingMode = storingMode;
            }

            public override IEnumerable<BulkString> Request(BulkStringFactory factory)
            {
                yield return Name;
                yield return key.ToBulkString(factory);
                yield return factory.Create(location.Longitude);
                yield return factory.Create(location.Latitude);
                yield return factory.Create(radius.Value);
                yield return radius.Unit.ToBulkString();

                if (count is { } value)
                    yield return factory.Create(value);

                if (storingMode == StoringMode.WithRawGeohash)
                    yield return Store;
                else
                    yield return StoreDistance;

                yield return target.ToBulkString(factory);
            }

            public override Visitor<StoreResponse> ResponseStructure => IntegerExpectation.Singleton
                .Then(itemsSaved => new StoreResponse(target, itemsSaved));
        }
    }

    public sealed class GEORADIUSBYMEMBER<T> : Command<IReadOnlyList<T>>
    {
        private readonly Key key;
        private readonly Key member;
        private readonly Distance radius;
        private readonly ResponseFormat<T> format;
        private readonly long? count;
        private readonly Sorting sorting;

        public GEORADIUSBYMEMBER(
            Key key,
            Key member,
            Distance radius,
            ResponseFormat<T> format,
            long? count = null,
            Sorting sorting = Sorting.None)
        {
            if (count is { } @long && @long <= 0)
                throw new ArgumentOutOfRangeException(nameof(count), count, "Must be positive");

            this.key = key;
            this.member = member;
            this.radius = radius;
            this.format = format;
            this.count = count;
            this.sorting = sorting;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory)
        {
            yield return GEORADIUS.Name;
            yield return key.ToBulkString(factory);
            yield return member.ToBulkString(factory);
            yield return factory.Create(radius.Value);
            yield return radius.Unit.ToBulkString();
            foreach (var argument in format.AdditionalArguments)
            {
                yield return argument;
            }

            if (count is { } value)
                yield return factory.Create(value);

            if (sorting == Sorting.AscendingByDistance)
                yield return GEORADIUS.Ascending;
            else if (sorting == Sorting.DescendingByDistance)
                yield return GEORADIUS.Descending;
        }

        public override Visitor<IReadOnlyList<T>> ResponseStructure => ArrayExpectation.Singleton
            .Then(response => new ProjectingReadOnlyList<DataType, T>(response, format.Parse) as IReadOnlyList<T>);
    }
}
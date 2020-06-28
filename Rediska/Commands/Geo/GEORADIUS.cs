﻿namespace Rediska.Commands.Geo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public static class GEORADIUS
    {
        internal static PlainBulkString Name { get; } = new PlainBulkString("GEORADIUS");
        internal static PlainBulkString Ascending { get; } = new PlainBulkString("ASC");
        internal static PlainBulkString Descending { get; } = new PlainBulkString("DESC");
        internal static PlainBulkString Store { get; } = new PlainBulkString("STORE");
        internal static PlainBulkString StoreDistance { get; } = new PlainBulkString("STOREDIST");

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

            private IEnumerable<BulkString> Query
            {
                get
                {
                    yield return Name;
                    yield return key.ToBulkString();
                    yield return location.Longitude.ToBulkString();
                    yield return location.Latitude.ToBulkString();
                    yield return radius.Value.ToBulkString();
                    yield return radius.Unit.ToBulkString();

                    if (count is { } value)
                        yield return value.ToBulkString();

                    if (storingMode == StoringMode.WithRawGeohash)
                        yield return Store;
                    else
                        yield return StoreDistance;

                    yield return target.ToBulkString();
                }
            }

            public override DataType Request => new PlainArray(Query.ToList());

            public override Visitor<StoreResponse> ResponseStructure => IntegerExpectation.Singleton
                .Then(itemsSaved => new StoreResponse(target, itemsSaved));
        }

        public static GEORADIUS<Key> OnlyNames(
            Key key,
            Location location,
            Distance radius,
            long? count = null,
            Sorting sorting = Sorting.None)
            => new GEORADIUS<Key>(key, location, radius, OnlyName.Singleton, count, sorting);
    }

    public sealed class GEORADIUS<T> : Command<IReadOnlyList<T>>
    {
        private readonly Key key;
        private readonly Location location;
        private readonly Distance radius;
        private readonly ResponseFormat<T> format;
        private readonly long? count;
        private readonly Sorting sorting;

        public GEORADIUS(
            Key key,
            Location location,
            Distance radius,
            ResponseFormat<T> format,
            long? count = null,
            Sorting sorting = Sorting.None)
        {
            if (count is {} @long && @long <= 0)
                throw new ArgumentOutOfRangeException(nameof(count), count, "Must be positive");

            this.key = key;
            this.location = location;
            this.radius = radius;
            this.format = format;
            this.count = count;
            this.sorting = sorting;
        }

        private IEnumerable<BulkString> Query
        {
            get
            {
                yield return GEORADIUS.Name;
                yield return key.ToBulkString();
                yield return location.Longitude.ToBulkString();
                yield return location.Latitude.ToBulkString();
                yield return radius.Value.ToBulkString();
                yield return radius.Unit.ToBulkString();
                foreach (var argument in format.AdditionalArguments)
                {
                    yield return argument;
                }

                if (count is {} value)
                    yield return value.ToBulkString();

                if (sorting == Sorting.AscendingByDistance)
                    yield return GEORADIUS.Ascending;
                else if (sorting == Sorting.DescendingByDistance)
                    yield return GEORADIUS.Descending;
            }
        }

        public override DataType Request => new PlainArray(Query.ToList());

        public override Visitor<IReadOnlyList<T>> ResponseStructure => ArrayExpectation.Singleton
            .Then(response => new ProjectingReadOnlyList<DataType, T>(response, format.Parse) as IReadOnlyList<T>);
    }
}
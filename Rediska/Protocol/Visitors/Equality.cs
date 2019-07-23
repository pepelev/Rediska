using System;

namespace Rediska.Protocol.Visitors
{
    internal sealed class Equality : Visitor<bool>
    {
        private readonly Func<Array, bool> arrayEquality;
        private readonly Func<BulkString, bool> bulkStringEquality;
        private readonly Func<Error, bool> errorEquality;
        private readonly Func<Integer, bool> integerEquality;
        private readonly Func<SimpleString, bool> simpleStringEquality;

        public Equality(
            Func<Integer, bool> integerEquality,
            Func<SimpleString, bool> simpleStringEquality,
            Func<Error, bool> errorEquality,
            Func<Array, bool> arrayEquality,
            Func<BulkString, bool> bulkStringEquality)
        {
            this.integerEquality = integerEquality;
            this.simpleStringEquality = simpleStringEquality;
            this.errorEquality = errorEquality;
            this.arrayEquality = arrayEquality;
            this.bulkStringEquality = bulkStringEquality;
        }

        public override bool Visit(Integer integer) => integerEquality(integer);
        public override bool Visit(SimpleString simpleString) => simpleStringEquality(simpleString);
        public override bool Visit(Error error) => errorEquality(error);
        public override bool Visit(Array array) => arrayEquality(array);
        public override bool Visit(BulkString bulkString) => bulkStringEquality(bulkString);

        public sealed class Bootstrap : Visitor<Equality>
        {
            private readonly Func<DataType, bool> @false = _ => false;
            public static Bootstrap Singleton { get; } = new Bootstrap();

            public override Equality Visit(Integer integer) => new Equality(
                another => another.Value == integer.Value,
                @false,
                @false,
                @false,
                @false
            );

            public override Equality Visit(SimpleString simpleString) => new Equality(
                @false,
                another => another.Content == simpleString.Content,
                @false,
                @false,
                @false
            );

            public override Equality Visit(Error error) => new Equality(
                @false,
                @false,
                another => another.Content == error.Content,
                @false,
                @false
            );

            public override Equality Visit(Array array) => new Equality(
                @false,
                @false,
                @false,
                another => Equals(another, array),
                @false
            );

            public override Equality Visit(BulkString bulkString) => new Equality(
                @false,
                @false,
                @false,
                @false,
                another => Equals(another, bulkString)
            );

            private static bool Equals(Array a, Array b)
            {
                if (a.IsNull && b.IsNull)
                    return true;

                if (a.IsNull || b.IsNull)
                    return false;

                if (a.Count != b.Count)
                    return false;

                for (var i = 0; i < a.Count; i++)
                {
                    if (a[i] != b[i])
                        return false;
                }

                return true;
            }

            private static bool Equals(BulkString a, BulkString b)
            {
                if (a.IsNull && b.IsNull)
                    return true;

                if (a.IsNull || b.IsNull)
                    return false;

                if (a.Length != b.Length)
                    return false;

                var aBytes = a.ToBytes();
                var bBytes = b.ToBytes();
                for (var i = 0; i < aBytes.Length; i++)
                {
                    if (aBytes[i] != bBytes[i])
                        return false;
                }

                return true;
            }
        }
    }
}
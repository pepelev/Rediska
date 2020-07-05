namespace Rediska.Commands.Strings
{
    using System.Collections.Generic;
    using System.Linq;
    using Protocol;
    using Protocol.Visitors;

    public sealed class SET : Command<SET.Response>
    {
        private static readonly PlainBulkString name = new PlainBulkString("SET");
        private static readonly PlainBulkString expire = new PlainBulkString("EX");
        private static readonly PlainBulkString expireMilliseconds = new PlainBulkString("PX");
        private static readonly PlainBulkString notExists = new PlainBulkString("NX");
        private static readonly PlainBulkString alreadyExists = new PlainBulkString("XX");
        private readonly Key key;
        private readonly BulkString value;
        private readonly Expiration expiration;
        private readonly Condition? condition;

        public SET(Key key, BulkString value)
            : this(key, value, None.Singleton)
        {
        }

        public SET(Key key, BulkString value, Expiration expiration, Condition? condition = null)
        {
            this.key = key;
            this.value = value;
            this.expiration = expiration;
            this.condition = condition;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory)
        {
            yield return name;
            yield return key.ToBulkString(factory);
            yield return value;
            foreach (var argument in expiration.Arguments(factory))
            {
                yield return argument;
            }

            switch (condition)
            {
                case Condition.SetOnlyIfKeyDoesNotExists:
                    yield return notExists;
                    break;

                case Condition.SetOnlyIfKeyExists:
                    yield return alreadyExists;
                    break;
            }
        }

        public override Visitor<Response> ResponseStructure => ResponseVisitor.Singleton;

        public enum Response : byte
        {
            OperationPerformed = 0,
            OperationNotPerformedDueToCondition = 1
        }

        public abstract class Expiration
        {
            public abstract IEnumerable<BulkString> Arguments(BulkStringFactory factory);
        }

        public sealed class None : Expiration
        {
            public static None Singleton { get; } = new None();

            public override IEnumerable<BulkString> Arguments(BulkStringFactory factory) =>
                Enumerable.Empty<BulkString>();
        }

        public sealed class Seconds : Expiration
        {
            private readonly long value;

            public Seconds(long value)
            {
                this.value = value;
            }

            public override IEnumerable<BulkString> Arguments(BulkStringFactory factory)
            {
                yield return expire;
                yield return factory.Create(value);
            }
        }

        public sealed class Milliseconds : Expiration
        {
            private readonly long value;

            public Milliseconds(long value)
            {
                this.value = value;
            }

            public override IEnumerable<BulkString> Arguments(BulkStringFactory factory)
            {
                yield return expireMilliseconds;
                yield return factory.Create(value);
            }
        }

        private sealed class ResponseVisitor : Expectation<Response>
        {
            public static ResponseVisitor Singleton { get; } = new ResponseVisitor();
            public override string Message => "Expected OK or null bulk string";

            public override Response Visit(SimpleString simpleString)
            {
                if (simpleString.Content == "OK")
                    return Response.OperationPerformed;

                throw Exception(simpleString);
            }

            public override Response Visit(BulkString bulkString)
            {
                if (bulkString.IsNull)
                    return Response.OperationNotPerformedDueToCondition;

                throw Exception(bulkString);
            }
        }
    }
}
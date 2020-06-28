namespace Rediska.Commands.Strings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Protocol;
    using Protocol.Visitors;
    using Array = Protocol.Array;

    public sealed class BITFIELD : Command<IReadOnlyList<long?>>
    {
        private static readonly PlainBulkString name = new PlainBulkString("BITFIELD");
        private static readonly PlainBulkString overflow = new PlainBulkString("OVERFLOW");
        private static readonly PlainBulkString wrap = new PlainBulkString("WRAP");
        private static readonly PlainBulkString saturate = new PlainBulkString("SAT");
        private static readonly PlainBulkString fail = new PlainBulkString("FAIL");
        private readonly Key key;
        private readonly IReadOnlyList<SubCommand> subCommands;

        public BITFIELD(Key key, params SubCommand[] subCommands)
            : this(key, subCommands as IReadOnlyList<SubCommand>)
        {
        }

        public BITFIELD(Key key, IReadOnlyList<SubCommand> subCommands)
        {
            this.key = key;
            this.subCommands = subCommands;
        }

        public override DataType Request => new PlainArray(
            Query().ToList()
        );

        public override Visitor<IReadOnlyList<long?>> ResponseStructure => new ListVisitor<long?>(
            ArrayExpectation.Singleton,
            NullableInteger.Singleton
        );

        private IEnumerable<BulkString> Query()
        {
            yield return name;
            yield return key.ToBulkString();
            var currentOverflow = Overflow.Wrap;
            foreach (var subCommand in subCommands)
            {
                if (subCommand.DesiredOverflow is {} desiredOverflow && desiredOverflow != currentOverflow)
                {
                    yield return overflow;
                    yield return desiredOverflow switch
                    {
                        Overflow.Wrap => wrap,
                        Overflow.Saturate => saturate,
                        Overflow.Fail => fail,
                        _ => throw new Exception($"Unexpected desired overflow {desiredOverflow}")
                    };
                    currentOverflow = desiredOverflow;
                }

                foreach (var segment in subCommand.Query())
                {
                    yield return segment;
                }
            }
        }

        public abstract class SubCommand
        {
            public abstract Overflow? DesiredOverflow { get; }
            public abstract IEnumerable<BulkString> Query();
        }

        private sealed class GET : SubCommand
        {
            private static readonly PlainBulkString name = new PlainBulkString("GET");
            private readonly Type type;
            private readonly Offset offset;

            public GET(Type type, Offset offset)
            {
                this.type = type;
                this.offset = offset;
            }

            public override Overflow? DesiredOverflow => null;

            public override IEnumerable<BulkString> Query()
            {
                yield return name;
                // todo type and kind
            }
        }

        private sealed class NullableInteger : Visitor<long?>
        {
            public static NullableInteger Singleton { get; } = new NullableInteger();
            public override long? Visit(Integer integer) => integer.Value;
            public override long? Visit(SimpleString simpleString) => throw new VisitException("Integer or null expected", simpleString);
            public override long? Visit(Error error) => throw new VisitException("Integer or null expected", error);

            public override long? Visit(Array array) => array.IsNull
                ? default(long?)
                : throw new VisitException("Integer or null expected", array);

            public override long? Visit(BulkString bulkString) => bulkString.IsNull
                ? default(long?)
                : throw new VisitException("Integer or null expected", bulkString);
        }
    }
}
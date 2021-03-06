﻿namespace Rediska.Commands.Strings
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

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

        public override IEnumerable<BulkString> Request(BulkStringFactory factory)
        {
            yield return name;
            yield return key.ToBulkString(factory);
            var currentOverflow = Overflow.Wrap;
            foreach (var subCommand in subCommands)
            {
                if (subCommand.DesiredOverflow is { } desiredOverflow && desiredOverflow != currentOverflow)
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

                foreach (var argument in subCommand.Arguments(factory))
                {
                    yield return argument;
                }
            }
        }

        public override Visitor<IReadOnlyList<long?>> ResponseStructure => new ListVisitor<long?>(
            ArrayExpectation.Singleton,
            NullableIntegerExpectation.Singleton
        );

        public abstract class SubCommand
        {
            public abstract Overflow? DesiredOverflow { get; }
            public abstract IEnumerable<BulkString> Arguments(BulkStringFactory factory);
        }

        public sealed class GET : SubCommand
        {
            private static readonly PlainBulkString subCommandName = new PlainBulkString("GET");
            private readonly Type type;
            private readonly Offset offset;

            public GET(Type type, Offset offset)
            {
                this.type = type;
                this.offset = offset;
            }

            public override Overflow? DesiredOverflow => null;

            public override IEnumerable<BulkString> Arguments(BulkStringFactory factory)
            {
                yield return subCommandName;
                yield return type.ToBulkString(factory);
                yield return offset.ToBulkString(factory);
            }
        }

        public sealed class SET : SubCommand
        {
            private static readonly PlainBulkString subCommandName = new PlainBulkString("SET");
            private readonly Type type;
            private readonly Offset offset;
            private readonly long value;

            public SET(Type type, Offset offset, long value)
            {
                if (!type.Fit(value))
                {
                    //todo
                    //throw new ArgumentOutOfRangeException(nameof(value), value, $"Value must fit type range {type.Range}");
                }

                this.type = type;
                this.offset = offset;
                this.value = value;
            }

            public override Overflow? DesiredOverflow => null;

            public override IEnumerable<BulkString> Arguments(BulkStringFactory factory)
            {
                yield return subCommandName;
                yield return type.ToBulkString(factory);
                yield return offset.ToBulkString(factory);
                yield return factory.Create(value);
            }
        }

        public sealed class INCRBY : SubCommand
        {
            private static readonly PlainBulkString subCommandName = new PlainBulkString("INCRBY");
            private readonly Type type;
            private readonly Offset offset;
            private readonly long increment;
            private readonly Overflow desiredOverflow;

            public INCRBY(Type type, Offset offset, long increment, Overflow desiredOverflow = Overflow.Wrap)
            {
                this.type = type;
                this.offset = offset;
                this.increment = increment;
                this.desiredOverflow = desiredOverflow;
            }

            public override Overflow? DesiredOverflow => desiredOverflow;

            public override IEnumerable<BulkString> Arguments(BulkStringFactory factory)
            {
                yield return subCommandName;
                yield return type.ToBulkString(factory);
                yield return offset.ToBulkString(factory);
                yield return factory.Create(increment);
            }
        }
    }
}
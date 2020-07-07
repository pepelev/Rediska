namespace Rediska.Commands.SortedSets
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed partial class ZADD : Command<ZADD.Response>
    {
        private static readonly PlainBulkString name = new PlainBulkString("ZADD");
        private static readonly PlainBulkString notExists = new PlainBulkString("NX");
        private static readonly PlainBulkString alreadyExists = new PlainBulkString("XX");
        private static readonly Visitor<Response> responseStructure = IntegerExpectation.Singleton.Then(added => new Response(added));
        private readonly Key key;
        private readonly Mode mode;
        private readonly IReadOnlyList<(double Score, BulkString Member)> members;

        public ZADD(Key key, params (double Score, BulkString Member)[] members)
            : this(key, Mode.AddOrUpdateScore, members)
        {
        }

        public ZADD(Key key, Mode mode, params (double Score, BulkString Member)[] members)
            : this(key, mode, members as IReadOnlyList<(double Score, BulkString Member)>)
        {
        }

        public ZADD(Key key, Mode mode, IReadOnlyList<(double Score, BulkString Member)> members)
        {
            ValidateMode(mode);
            this.key = key;
            this.mode = mode;
            this.members = members;
        }

        private static void ValidateMode(Mode mode)
        {
            switch (mode)
            {
                case Mode.AddOrUpdateScore:
                case Mode.AddMembersOnly:
                case Mode.UpdateScoreOnly:
                    break;
                default:
                {
                    throw new ArgumentException(
                        $"Must be AddOrUpdateScore, AddMembersOnly or UpdateScoreOnly, but {mode} found",
                        nameof(mode)
                    );
                }
            }
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory)
        {
            yield return name;
            yield return key.ToBulkString(factory);

            if (mode == Mode.AddMembersOnly)
            {
                yield return notExists;
            }
            else if (mode == Mode.UpdateScoreOnly)
            {
                yield return alreadyExists;
            }

            foreach (var (score, member) in members)
            {
                yield return factory.Create(score);
                yield return member;
            }
        }

        public override Visitor<Response> ResponseStructure => responseStructure;

        public enum Mode : byte
        {
            AddOrUpdateScore = 0,
            AddMembersOnly = 1,
            UpdateScoreOnly = 2
        }

        public readonly struct Response
        {
            public Response(long membersAdded)
            {
                MembersAdded = membersAdded;
            }

            public long MembersAdded { get; }
            public override string ToString() => $"{nameof(MembersAdded)}: {MembersAdded}";
        }
    }
}
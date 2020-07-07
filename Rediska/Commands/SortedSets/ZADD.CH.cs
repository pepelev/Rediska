namespace Rediska.Commands.SortedSets
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed partial class ZADD
    {
        public sealed class CH : Command<CH.Response>
        {
            private static readonly PlainBulkString changed = new PlainBulkString("CH");

            private static readonly Visitor<Response> responseStructure = IntegerExpectation.Singleton
                .Then(membersChanged => new Response(membersChanged));

            private readonly Key key;
            private readonly Mode mode;
            private readonly IReadOnlyList<(double Score, BulkString Member)> members;

            public CH(Key key, Mode mode, IReadOnlyList<(double Score, BulkString Member)> members)
            {
                ValidateMode(mode);
                this.key = key;
                this.mode = mode;
                this.members = members;
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

                yield return changed;

                foreach (var (score, member) in members)
                {
                    yield return factory.Create(score);
                    yield return member;
                }
            }

            public override Visitor<Response> ResponseStructure => responseStructure;

            public readonly struct Response
            {
                public Response(long membersChanged)
                {
                    MembersChanged = membersChanged;
                }

                public long MembersChanged { get; }
                public override string ToString() => $"{nameof(MembersChanged)}: {MembersChanged}";
            }
        }
    }
}
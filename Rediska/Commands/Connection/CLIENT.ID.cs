namespace Rediska.Commands.Connection
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net;
    using System.Runtime.CompilerServices;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public static partial class CLIENT
    {
        public sealed class ID : Command<ClientId>
        {
            private static readonly PlainBulkString subName = new PlainBulkString("ID");
            private static readonly PlainBulkString[] request = {name, subName};

            private static readonly Visitor<ClientId> responseStructure = IntegerExpectation.Singleton
                .Then(clientId => new ClientId(clientId));

            public override IEnumerable<BulkString> Request(BulkStringFactory factory) => request;
            public override Visitor<ClientId> ResponseStructure => responseStructure;
        }

        public sealed class LIST : Command<IReadOnlyList<Description>>
        {
            private static readonly PlainBulkString subName = new PlainBulkString("LIST");
            private static readonly PlainBulkString typeOption = new PlainBulkString("TYPE");
            private static readonly PlainBulkString normal = new PlainBulkString("normal");
            private static readonly PlainBulkString master = new PlainBulkString("master");
            private static readonly PlainBulkString replica = new PlainBulkString("replica");
            private static readonly PlainBulkString pubsub = new PlainBulkString("pubsub");
            private static readonly PlainBulkString[] simpleRequest = {name, subName};
            private readonly Type type;
            private static readonly char[] clientSeparator = new[] {'\n'};

            public LIST(Type type)
            {
                Validate(type);
                this.type = type;
            }

            private static void Validate(Type type)
            {
                switch (type)
                {
                    case Type.All:
                    case Type.Normal:
                    case Type.Master:
                    case Type.Replica:
                    case Type.PubSub:
                        return;
                    default:
                        throw new ArgumentException(
                            $"Must be All, Normal, Master, Replica or PubSub, but {type} found",
                            nameof(type)
                        );
                }
            }

            public override IEnumerable<BulkString> Request(BulkStringFactory factory) => type == Type.All
                ? simpleRequest
                : new[]
                {
                    name,
                    subName,
                    typeOption,
                    TypeArgument
                };

            public PlainBulkString TypeArgument => type switch
            {
                Type.Normal => normal,
                Type.Master => master,
                Type.Replica => replica,
                _ => pubsub
            };

            public override Visitor<IReadOnlyList<Description>> ResponseStructure => BulkStringExpectation.Singleton.Then(
                @string => new ProjectingReadOnlyList<string, Description>(
                    @string
                        .ToString()
                        .Split(clientSeparator, StringSplitOptions.RemoveEmptyEntries),
                    client => 
                )
            );

            public enum Type : byte
            {
                All = 0,
                Normal = 1,
                Master = 2,
                Replica = 3,
                PubSub = 4
            }
        }

        public sealed class Description
        {
            private readonly Dictionary<string, string> fields;

            public Description(Dictionary<string, string> fields)
            {
                this.fields = fields;
            }

            private string String(string key, [CallerMemberName] string member = "") => fields.TryGetValue(key, out var value)
                ? value
                : throw new InvalidOperationException($"Value for {member} at key {key} not present in the reply");

            private long Long(string key, [CallerMemberName] string member = "") => long.Parse(String(key, member));

            public ClientId Id => new ClientId(Long("id"));
            public string Name => String("name");
            //public IPEndPoint Address => Value("addr")
            public long FileDescriptor => Long("fd");
            public TimeSpan Age => new TimeSpan(TimeSpan.TicksPerSecond * Long("age"));
            public TimeSpan IdleTime => new TimeSpan(TimeSpan.TicksPerSecond * Long("idle"));

            public IReadOnlyList<Flag> Flags => new PrettyReadOnlyList<Flag>(
                new ProjectingReadOnlyList<char, Flag>(
                    String("flags").ToCharArray(),
                    letter => new Flag(letter)
                )
            );

            public DatabaseNumber CurrentDatabase => new DatabaseNumber(Long("db"));
            public long ChannelSubscription => Long("sub");
            public long PatternSubscription => Long("psub");

            //public long MULTI => Long("psub");
            public long QueryBufferLength => Long("qbuf");
            public long FreeSpaceOfQueryBufferLength => Long("qbuf-free");
            public long OutputBufferLength => Long("obl");
            public long OutputListLength => Long("oll");
            public long OutputBufferMemoryUsage => Long("omem");
            //public long Events => Long("omem");
            public string LastCommand => String("cmd");
        }

        public readonly struct Flag : IEquatable<Flag>
        {
            private readonly char letter;

            public Flag(char letter)
            {
                this.letter = letter;
            }

            // todo readable ToString()
            public override string ToString() => letter.ToString(CultureInfo.InvariantCulture);
            public bool Equals(Flag other) => letter == other.letter;
            public override bool Equals(object obj) => obj is Flag other && Equals(other);
            public override int GetHashCode() => letter.GetHashCode();
            public static bool operator ==(Flag left, Flag right) => left.Equals(right);
            public static bool operator !=(Flag left, Flag right) => !left.Equals(right);
        }
    }
}
namespace Rediska.Commands.Connection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public static partial class CLIENT
    {
        public sealed class LIST : Command<IReadOnlyList<Description>>
        {
            private static readonly PlainBulkString subName = new PlainBulkString("LIST");
            private static readonly PlainBulkString typeOption = new PlainBulkString("TYPE");
            private static readonly PlainBulkString normal = new PlainBulkString("normal");
            private static readonly PlainBulkString master = new PlainBulkString("master");
            private static readonly PlainBulkString replica = new PlainBulkString("replica");
            private static readonly PlainBulkString pubsub = new PlainBulkString("pubsub");
            private static readonly PlainBulkString[] simpleRequest = {name, subName};
            private static readonly char[] clientSeparator = {'\n'};
            private static readonly char[] fieldSeparator = {' '};
            private static readonly char[] keyValueSeparator = {'='};
            private readonly Type type;

            public LIST(Type type = Type.All)
            {
                Validate(type);
                this.type = type;
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

            public override Visitor<IReadOnlyList<Description>> ResponseStructure => BulkStringExpectation.Singleton.Then(
                @string => new ProjectingReadOnlyList<string, Description>(
                    @string
                        .ToString()
                        .Split(clientSeparator, StringSplitOptions.RemoveEmptyEntries),
                    client =>
                    {
                        var rawFields = client.Split(fieldSeparator, StringSplitOptions.RemoveEmptyEntries);
                        var fields = rawFields
                            .Select(field => field.Split(keyValueSeparator, 2, StringSplitOptions.RemoveEmptyEntries))
                            .ToDictionary(pair => pair[0], pair => pair[1]);
                        return new Description(fields);
                    }
                ) as IReadOnlyList<Description>
            );

            public PlainBulkString TypeArgument => type switch
            {
                Type.Normal => normal,
                Type.Master => master,
                Type.Replica => replica,
                _ => pubsub
            };

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

            public enum Type : byte
            {
                All = 0,
                Normal = 1,
                Master = 2,
                Replica = 3,
                PubSub = 4
            }
        }
    }
}
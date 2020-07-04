namespace Rediska.Commands.Keys
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Protocol;
    using Protocol.Visitors;

    public sealed class MIGRATE : Command<MIGRATE.Response>
    {
        private static readonly PlainBulkString name = new PlainBulkString("MIGRATE");
        private static readonly PlainBulkString copySegment = new PlainBulkString("COPY");
        private static readonly PlainBulkString replaceSegment = new PlainBulkString("REPLACE");
        private static readonly PlainBulkString keysSegment = new PlainBulkString("KEYS");
        private static readonly PlainBulkString emptyKeySegment = new PlainBulkString("");
        private readonly IPEndPoint ipEndPoint;
        private readonly IReadOnlyList<Key> keys;
        private readonly DatabaseNumber destinationDb;
        private readonly MillisecondsTimeout timeout;
        private readonly SourceKeyBehavior sourceKeyBehavior;
        private readonly DestinationKeyBehavior destinationKeyBehavior;
        private readonly Auth auth;

        public MIGRATE(
            IPEndPoint ipEndPoint,
            IReadOnlyList<Key> keys,
            DatabaseNumber destinationDb,
            MillisecondsTimeout timeout)
            : this(
                ipEndPoint,
                keys,
                destinationDb,
                timeout,
                SourceKeyBehavior.Move,
                DestinationKeyBehavior.EnsureKeyExists,
                NoAuth.Singleton
            )
        {
        }

        public MIGRATE(
            IPEndPoint ipEndPoint,
            IReadOnlyList<Key> keys,
            DatabaseNumber destinationDb,
            MillisecondsTimeout timeout,
            SourceKeyBehavior sourceKeyBehavior,
            DestinationKeyBehavior destinationKeyBehavior,
            Auth auth)
        {
            if (sourceKeyBehavior != SourceKeyBehavior.Move && sourceKeyBehavior != SourceKeyBehavior.Copy)
            {
                throw new ArgumentException(
                    $"Must be either Move or Copy, but {sourceKeyBehavior} found",
                    nameof(sourceKeyBehavior)
                );
            }

            if (destinationKeyBehavior != DestinationKeyBehavior.Replace &&
                destinationKeyBehavior != DestinationKeyBehavior.EnsureKeyExists)
            {
                throw new ArgumentException(
                    $"Must be either Replace or EnsureKeyExists, but {destinationKeyBehavior} found",
                    nameof(destinationKeyBehavior)
                );
            }

            this.ipEndPoint = ipEndPoint;
            this.keys = keys;
            this.destinationDb = destinationDb;
            this.timeout = timeout;
            this.sourceKeyBehavior = sourceKeyBehavior;
            this.destinationKeyBehavior = destinationKeyBehavior;
            this.auth = auth;
        }

        public override Visitor<Response> ResponseStructure => SimpleStringExpectation.Singleton
            .Then(
                response => response switch
                {
                    "OK" => Response.Success,
                    "NOKEY" => Response.KeysNotFound,
                    _ => throw new ArgumentException("Must be either OK or NOKEY", nameof(response))
                }
            );

        public override IEnumerable<BulkString> Request(BulkStringFactory factory)
        {
            yield return name;
            yield return factory.Utf8(ipEndPoint.ToString());
            yield return factory.Create(ipEndPoint.Port);
            yield return keys.Count == 1
                ? keys[0].ToBulkString()
                : emptyKeySegment;

            yield return factory.Create(destinationDb.Value);
            yield return factory.Create(timeout.Milliseconds);
            if (sourceKeyBehavior == SourceKeyBehavior.Copy)
            {
                yield return copySegment;
            }

            if (destinationKeyBehavior == DestinationKeyBehavior.Replace)
            {
                yield return replaceSegment;
            }

            foreach (var argument in auth.Arguments())
            {
                yield return argument;
            }

            if (keys.Count > 1)
            {
                yield return keysSegment;
                foreach (var key in keys)
                {
                    yield return key.ToBulkString();
                }
            }
        }

        public enum DestinationKeyBehavior : byte
        {
            EnsureKeyExists = 0,
            Replace = 1
        }

        public enum Response : byte
        {
            Success = 0,
            KeysNotFound = 1
        }

        public enum SourceKeyBehavior : byte
        {
            Move = 0,
            Copy = 1
        }

        // todo make normal auth
        public abstract class Auth
        {
            public abstract IEnumerable<BulkString> Arguments();
        }

        public sealed class NoAuth : Auth
        {
            public static NoAuth Singleton { get; } = new NoAuth();
            public override IEnumerable<BulkString> Arguments() => Enumerable.Empty<BulkString>();
        }
    }
}
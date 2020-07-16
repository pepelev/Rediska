namespace Rediska.Commands.Connection
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using Utils;

    public static partial class CLIENT
    {
        public sealed class Description : IReadOnlyDictionary<string, string>
        {
            private static readonly Regex addressPattern = new Regex(@"^(?<Ip>[0-9\.]+):(?<Port>[0-9]+)$", RegexOptions.CultureInvariant);
            private readonly Dictionary<string, string> fields;

            public Description(Dictionary<string, string> fields)
            {
                this.fields = fields;
            }

            public ClientId Id => new ClientId(Long("id"));
            public string Name => String("name");

            public IPEndPoint Address
            {
                get
                {
                    var address = String("addr");
                    var match = addressPattern.Match(address);
                    if (!match.Success)
                        throw new InvalidOperationException("Address has wrong format");

                    return new IPEndPoint(
                        IPAddress.Parse(match.Groups["Ip"].Value),
                        int.Parse(match.Groups["Port"].Value)
                    );
                }
            }

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
            public long ChannelSubscriptions => Long("sub");
            public long PatternSubscriptions => Long("psub");
            public long CommandsInTransactionContext => Long("multi");
            public long QueryBufferLength => Long("qbuf");
            public long FreeSpaceOfQueryBuffer => Long("qbuf-free");
            public long OutputBufferLength => Long("obl");
            public long OutputListLength => Long("oll");
            public long OutputBufferMemoryUsage => Long("omem");

            public FileDescriptorEvents Events
            {
                get
                {
                    var rawEvents = String("events");
                    var events = FileDescriptorEvents.None;
                    if (rawEvents.Contains('r'))
                        events |= FileDescriptorEvents.Readable;

                    if (rawEvents.Contains('w'))
                        events |= FileDescriptorEvents.Writable;

                    return events;
                }
            }

            public string LastCommand => String("cmd");
            public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => fields.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            public int Count => fields.Count;
            public bool ContainsKey(string key) => fields.ContainsKey(key);
            public bool TryGetValue(string key, out string value) => fields.TryGetValue(key, out value);
            public string this[string key] => fields[key];
            public IEnumerable<string> Keys => fields.Keys;
            public IEnumerable<string> Values => fields.Values;

            private string String(string key, [CallerMemberName] string member = "") => fields.TryGetValue(key, out var value)
                ? value
                : throw new InvalidOperationException($"Value for {member} at key {key} not present in the reply");

            private long Long(string key, [CallerMemberName] string member = "") => long.Parse(String(key, member));
        }
    }
}
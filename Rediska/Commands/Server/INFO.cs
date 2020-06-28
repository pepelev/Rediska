namespace Rediska.Commands.Server
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Protocol;
    using Protocol.Visitors;

    public sealed class INFO : Command<INFO.SectionList>
    {
        public const string DefaultSection = "default";
        public const string AllSections = "all";
        private static readonly PlainBulkString name = new PlainBulkString("INFO");
        private readonly string section;

        public INFO(string section = DefaultSection)
        {
            this.section = section ?? throw new ArgumentNullException(nameof(section));
        }

        public sealed class SectionList : IEnumerable<Section>
        {
            private readonly string response;

            internal SectionList(string response)
            {
                this.response = response;
            }

            public IEnumerator<Section> GetEnumerator()
            {
                using var feed = new StringReader(response);
                while (true)
                {
                    var line = feed.ReadLine();
                    if (line == null)
                        yield break;

                    if (line.StartsWith("#"))
                    {
                        var name = line.Substring(2);
                        var properties = new List<KeyValuePair<string, string>>();
                        while (true)
                        {
                            var propertyLine = feed.ReadLine();
                            if (propertyLine == null)
                            {
                                yield return new Section(name, properties);
                                yield break;
                            }
                            if (string.IsNullOrWhiteSpace(propertyLine))
                            {
                                yield return new Section(name, properties);
                                break;
                            }

                            var segments = propertyLine.Split(':');
                            if (segments.Length != 2)
                                throw new Exception("Expected single colon");

                            var item = new KeyValuePair<string, string>(segments[0], segments[1]);
                            properties.Add(item);
                        }
                    }
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            public override string ToString() => response;
        }

        public sealed class Section : IEnumerable<KeyValuePair<string, string>>
        {
            internal Section(string name, IReadOnlyList<KeyValuePair<string, string>> properties)
            {
                Name = name;
                this.properties = properties;
            }

            public string Name { get; }
            private readonly IReadOnlyList<KeyValuePair<string, string>> properties;
            public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => properties.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public override string ToString()
            {
                if (properties.Count == 0)
                    return $"# {Name}";

                var tail = string.Join(
                    Environment.NewLine,
                    this.Select(property => $"{property.Key}:{property.Value}")
                );
                return $"# {Name}{Environment.NewLine}{tail}";
            }
        }

        public override DataType Request => section == DefaultSection
            ? new PlainArray(name)
            : new PlainArray(name, new PlainBulkString(section));

        public override Visitor<SectionList> ResponseStructure => BulkStringExpectation.Singleton
            .Then(response => new SectionList(response.ToString()));
    }
}
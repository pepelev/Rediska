namespace Rediska.Commands.Server
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
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

            public SectionList(string response)
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
                        yield return new Section(name);
                    }
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            public override string ToString() => response;
        }

        public sealed class Section : IEnumerable<KeyValuePair<string, string>>
        {
            private readonly string response;
            private readonly int bodyStart;
            private readonly int bodyEnd;

            public Section(string name)
            {
                Name = name;
            }

            public string Name { get; }
            public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
            {
                yield break;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public override DataType Request => section == DefaultSection
            ? new PlainArray(name)
            : new PlainArray(name, new PlainBulkString(section));

        public override Visitor<SectionList> ResponseStructure => BulkStringExpectation.Singleton
            .Then(response => new SectionList(response.ToString()));
    }
}
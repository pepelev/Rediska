namespace Rediska.Commands.Server
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Array = Protocol.Array;

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
        }

        public sealed class Section : IEnumerable<KeyValuePair<string, string>>
        {
            public string Name { get; }
        }

        public override DataType Request => section == DefaultSection
            ? new PlainArray(name)
            : new PlainArray(name, new PlainBulkString(section));

        public override Visitor<SectionList> ResponseStructure => BulkStringExpectation.Singleton
            .Then(response => new SectionList(response.ToString()));
    }
}
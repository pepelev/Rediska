namespace Rediska.Tests.Commands.Utility
{
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using Protocol;
    using Rediska.Commands;
    using Rediska.Commands.Auxiliary;

    public class PlainCommand_Should
    {
        private static TestCaseData Case(string query, params BulkString[] segments)
        {
            return new TestCaseData(query).Returns(segments);
        }

        public static TestCaseData[] ParseCases()
        {
            return new[]
            {
                Case("FLUSHDB", "FLUSHDB").SetName("Regular single segment"),
                Case("SET a 10", "SET", "a", "10").SetName("Regular three segments"),
                Case("'SET EX' a 500", "SET EX", "a", "500").SetName("Escape space in th beginning"),
                Case("SET 'a key' 100", "SET", "a key", "100").SetName("Escape space in the middle"),
                Case("SET a 'value with spaces'", "SET", "a", "value with spaces").SetName("Escape space in the end"),
                Case("SET\ta 100", "SET", "a", "100").SetName("Tab as separator"),
                Case("SET  a 100", "SET", "a", "100").SetName("Several spaces"),
                Case("SET\t a", "SET", "a").SetName("Mixed whitespace")
            };
        }

        [Test]
        [TestCaseSource(nameof(ParseCases))]
        public IEnumerable<BulkString> Parse(string query)
        {
            var command = PlainCommand.Parse(query);
            return command.Request(BulkStringFactory.Plain);
        }
    }
}
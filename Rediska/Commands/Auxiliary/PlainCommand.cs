namespace Rediska.Commands.Auxiliary
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Protocol;
    using Protocol.Visitors;

    public sealed class PlainCommand : Command<DataType>
    {
        private readonly IReadOnlyList<BulkString> arguments;

        public PlainCommand(params BulkString[] arguments)
            : this(arguments as IReadOnlyList<BulkString>)
        {
        }

        public PlainCommand(IReadOnlyList<BulkString> arguments)
        {
            this.arguments = arguments;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => arguments;
        public override Visitor<DataType> ResponseStructure => Id.Singleton;

        public static PlainCommand Parse(string query)
        {
            var resultSegments = new List<BulkString>();
            var segment = new StringBuilder();
            var escape = false;
            for (var i = 0; i < query.Length; i++)
            {
                var @char = query[i];
                switch (@char, escape)
                {
                    case ('\'', false):
                        if (segment.Length != 0)
                            throw new FormatException($"Unexpected ' at index {i}");

                        escape = true;
                        break;
                    case ('\'', true):
                        if (segment.Length == 0)
                            resultSegments.Add(PlainBulkString.Empty);
                        else
                        {
                            resultSegments.Add(new PlainBulkString(segment.ToString()));
                            segment.Clear();
                            escape = false;
                        }

                        break;
                    case ('\\', _):
                        i++;
                        if (i >= query.Length)
                        {
                            throw new FormatException("Unterminated escape at the end of query");
                        }

                        var appendChar = query[i] switch
                        {
                            '\'' => '\'',
                            '\\' => '\\',
                            var invalidEscape =>
                            throw new FormatException($"Invalid escape sequence {invalidEscape} at index {i}")
                        };

                        segment.Append(appendChar);
                        break;
                    case (var currentChar, false) when char.IsWhiteSpace(currentChar):
                        if (segment.Length > 0)
                        {
                            resultSegments.Add(new PlainBulkString(segment.ToString()));
                            segment.Clear();
                        }

                        break;
                    case (var currentChar, true) when char.IsWhiteSpace(currentChar):
                        segment.Append(currentChar);
                        break;
                    case var (currentChar, _):
                        segment.Append(currentChar);
                        break;
                }
            }

            if (escape)
                throw new FormatException("Unterminated apostrophe at the end of the query");

            if (segment.Length > 0)
                resultSegments.Add(segment.ToString());

            return new PlainCommand(resultSegments);
        }
    }
}
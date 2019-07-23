using System.Text;

namespace Rediska.Protocol.Visitors
{
    public sealed class PrintVisitor : Visitor<string>
    {
        public static PrintVisitor Root { get; } = new PrintVisitor("");

        private readonly string indentation;

        public PrintVisitor(string indentation)
        {
            this.indentation = indentation;
        }

        public override string Visit(Integer integer) => $"{indentation}:{integer.Value}";
        public override string Visit(SimpleString simpleString) => $"{indentation}+{simpleString.Content}";
        public override string Visit(Error error) => $"{indentation}-{error.Content}";

        public override string Visit(Array array)
        {
            if (array.IsNull)
                return $"{indentation}*null array";

            if (array.Count == 0)
                return $"{indentation}*empty array";

            var result = new StringBuilder();
            result.AppendLine($"{indentation}*array of {array.Count} items");
            for (var i = 0; i < array.Count; i++)
            {
                var visitor = new PrintVisitor($"{indentation}{i + 1}) ");
                var print = array[i].Accept(visitor);
                result.AppendLine(print);
            }

            return result.ToString();
        }

        public override string Visit(BulkString bulkString) => indentation + "$" + Encoding.UTF8.GetString(
            bulkString.ToBytes()
        );
    }
}
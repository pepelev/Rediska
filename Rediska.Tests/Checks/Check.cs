using System.IO;
using System.Linq;
using Rediska.Protocol;
using Rediska.Protocol.Inputs;

namespace Rediska.Tests.Checks
{
    public abstract class Check
    {
        public abstract void Verify(Input input);
    }

    public sealed class BulkStringCheck : Check
    {
        private readonly byte[] expected;

        public BulkStringCheck(byte[] expected)
        {
            this.expected = expected;
        }

        public override void Verify(Input input)
        {
            var bulkString = input.ReadBulkString();
            if (bulkString.IsNull && expected == null)
                return;

            if (expected == null)
                throw new CheckException("Actual BulkString is not null", this, input, bulkString);

            if (bulkString.IsNull)
                throw new CheckException("Actual BulkString is null", this, input, bulkString);

            if (bulkString.Length != expected.Length)
                throw new CheckException("Length does not match expected", this, input, bulkString);

            using (var stream = new MemoryStream())
            {
                bulkString.WriteContent(stream);
                var bulkStringContent = stream.ToArray();
                if (!expected.SequenceEqual(bulkStringContent))
                    throw new CheckException("Different content", this, input, bulkString);
            }
        }
    }
}
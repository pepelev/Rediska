using Rediska.Protocol;
using Rediska.Protocol.Inputs;

namespace Rediska.Tests.Checks
{
    public sealed class IntegerCheck : Check
    {
        private readonly long expected;

        public IntegerCheck(long expected)
        {
            this.expected = expected;
        }

        public override void Verify(Input input)
        {
            var actual = input.ReadInteger();
            if (actual != expected)
                throw new CheckException("Different integer", this, input, actual);
        }
    }
}
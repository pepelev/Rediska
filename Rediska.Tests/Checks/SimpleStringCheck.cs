using Rediska.Protocol;

namespace Rediska.Tests.Checks
{
    public sealed class SimpleStringCheck : Check
    {
        private readonly string expected;

        public SimpleStringCheck(string expected)
        {
            this.expected = expected;
        }

        public override void Verify(Input input)
        {
            var actual = input.ReadSimpleString();
            if (actual != expected)
                throw new CheckException("Different string", this, input, actual);
        }
    }
}
using FluentAssertions;
using NUnit.Framework;
using Rediska.Protocol;
using Rediska.Protocol.Outputs;

namespace Rediska.Tests
{
    public sealed class ArrayShould
    {
        [Test]
        public void BeVerified()
        {
            var array = new PlainArray(
                new PlainBulkString("bosya"),
                new Integer(500)
            );

            var verifyingOutput = new VerifyingOutput();
            array.Write(verifyingOutput);
            verifyingOutput.Completed().Should().BeTrue();
        }
    }
}
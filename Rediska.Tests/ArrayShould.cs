using FluentAssertions;
using NUnit.Framework;
using Rediska.Protocol;
using Rediska.Protocol.Requests;

namespace Rediska.Tests
{
    public sealed class ArrayShould
    {
        [Test]
        public void BeVerified()
        {
            var array = new Array(
                new BulkString("bosya"),
                new Integer(500)
            );

            var verifyingOutput = new VerifyingOutput();
            array.Write(verifyingOutput);
            verifyingOutput.Completed().Should().BeTrue();
        }
    }
}
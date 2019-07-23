using System;
using FluentAssertions;
using NUnit.Framework;
using Rediska.Protocol;

namespace Rediska.Tests
{
    [TestFixture]
    public sealed class ResponseShould
    {
        [Test]
        public void NotBeReadyWhenCreated()
        {
            new Response().IsReady.Should().BeFalse();
        }

        [Test]
        public void Test()
        {
            var response = new Response();

            var bytes = new Content(
                new Integer(10)
            ).AsBytes();

            response.Feed(new ArraySegment<byte>(bytes));
            response.IsReady.Should().BeTrue();
        }
    }
}
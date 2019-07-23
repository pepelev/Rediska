﻿using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Rediska.Commands;

namespace Rediska.Tests.Tests
{
    public sealed class ConnectionShould
    {
        private SimpleConnection sut;

        [SetUp]
        public void SetUp()
        {
            sut = new SimpleConnection();
        }

        [Test]
        public async Task Test()
        {
            var command = new ECHO("foo");
            var request = command.Request;
            using (var response = await sut.SendAsync(request).ConfigureAwait(false))
            {
                var dataType = await response.Value.ReadAsync().ConfigureAwait(false);
                var echo = dataType.Accept(command.ResponseStructure);
                echo.Should().Be("foo");
            }
        }
    }
}
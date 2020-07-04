namespace Rediska.Tests.Commands.Strings
{
    using System;
    using System.Threading.Tasks;
    using Fixtures;
    using FluentAssertions;
    using NUnit.Framework;
    using Protocol;
    using Rediska.Commands.Strings;
    using Rediska.Commands.Utility;
    using Sets;

    [TestFixtureSource(typeof(ConnectionCollection))]
    public sealed class MGET_Should
    {
        private readonly Connection connection;

        public MGET_Should(Connection connection)
        {
            this.connection = connection;
        }

        [Test]
        public async Task Deny_Zero_Arguments()
        {
            var command = new PlainCommand("MGET");
            var response = await connection.ExecuteAsync(command).ConfigureAwait(false);
            response.Should().Be(new Error("ERR wrong number of arguments for 'mget' command"));
        }

        [Test]
        [Because(nameof(Deny_Zero_Arguments))]
        public void Deny_Creation_With_Empty_Keys()
        {
            Assert.Throws<ArgumentException>(
                // ReSharper disable once ObjectCreationAsStatement
                () => new MGET()
            );
        }

        [Test]
        public async Task Return_Single_Key()
        {
            var key = new Key.Utf8("key-a");
            await connection.ExecuteAsync(new SET(key, "value-a")).ConfigureAwait(false);

            var values = await connection.ExecuteAsync(new MGET(key)).ConfigureAwait(false);

            values.Should().Equal("value-a");
        }

        [Test]
        public async Task Return_Several_Keys()
        {
            var key1 = new Key.Utf8("key-1");
            var key2 = new Key.Utf8("key-2");
            var key3 = new Key.Utf8("key-3");

            foreach (var key in new[] {key1, key2, key3})
            {
                await connection.ExecuteAsync(new SET(key, key.ToString())).ConfigureAwait(false);
            }

            var values = await connection.ExecuteAsync(new MGET(key1, key2, key3)).ConfigureAwait(false);

            values.Should().Equal("key-1", "key-2", "key-3");
        }

        [Test]
        public async Task Allow_To_Query_Same_Key()
        {
            var key = new Key.Utf8("key-a");
            await connection.ExecuteAsync(new SET(key, "value-a")).ConfigureAwait(false);

            var values = await connection.ExecuteAsync(new MGET(key, key)).ConfigureAwait(false);

            values.Should().Equal("value-a", "value-a");
        }

        [Test]
        public async Task Return_NullString_When_Query_Non_Existing_Key()
        {
            var key = new Key.Utf8("key-a");
            await connection.ExecuteAsync(new SET(key, "value-a")).ConfigureAwait(false);

            var values = await connection.ExecuteAsync(new MGET(key, "non-existent-key")).ConfigureAwait(false);

            values.Should().Equal("value-a", BulkString.Null);
        }
    }
}
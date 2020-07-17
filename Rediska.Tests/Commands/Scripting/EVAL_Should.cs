namespace Rediska.Tests.Commands.Scripting
{
    using System.Threading.Tasks;
    using Fixtures;
    using FluentAssertions;
    using NUnit.Framework;
    using Protocol;
    using Rediska.Commands.Scripting;
    using Rediska.Commands.Strings;
    using Array = System.Array;

    [TestFixtureSource(typeof(ConnectionCollection))]
    public class EVAL_Should
    {
        public static TestCaseData[] ReturnCases { get; } =
        {
            new TestCaseData("")
                .Returns(BulkString.Null)
                .SetName("Null_Bulk_String_When_There_Is_No_Return_Statement_In_Script"),
            new TestCaseData("return #KEYS")
                .Returns(new Integer(1))
                .SetName("Integer_When_Keys_Count_Is_Returned"),
            new TestCaseData("return KEYS[1]")
                .Returns(new PlainBulkString("some-key"))
                .SetName("Bulk_String_When_Key_Is_Returned"),
            new TestCaseData("return ARGV[1]")
                .Returns(new PlainBulkString("some-argument"))
                .SetName("Bulk_String_When_Argument_Is_Returned"),
            new TestCaseData("return NonExistingVariable")
                .Returns(new Error("ERR Error running script (call to f_2e082aa2c5cab2bfce336cccc8db2434ff83b936): @enable_strict_lua:15: user_script:1: Script attempted to access nonexistent global variable 'NonExistingVariable' "))
                .SetName("Error_When_Non_Existing_Variable_Is_Accessed"),
            new TestCaseData("return KEYS")
                .Returns(new PlainArray(new PlainBulkString("some-key")))
                .SetName("Array_Of_Bulk_String_When_Keys_Are_Returned"),
            new TestCaseData("return 'Hello'")
                .Returns(new PlainBulkString("Hello"))
                .SetName("Bulk_String_When_String_Is_Returned"),
            new TestCaseData("return {10, 'Cat', {42, 100}}")
                .Returns(
                    new PlainArray(
                        new Integer(10),
                        new PlainBulkString("Cat"),
                        new PlainArray(
                            new Integer(42),
                            new Integer(100)
                        )
                    )
                )
                .SetName("Nested_Arrays_When_Complex_Data_Structure_Is_Returned"),
            new TestCaseData("return true")
                .Returns(new Integer(1))
                .SetName("One_When_True_Is_Returned"),
            new TestCaseData("return false")
                .Returns(BulkString.Null)
                .SetName("Zero_When_False_Is_Returned"),
        };

        private readonly Connection connection;

        public EVAL_Should(Connection connection)
        {
            this.connection = connection;
        }

        [Test]
        public async Task Run_Script_Server_Side()
        {
            var command = new EVAL(
                @"
for i,key in ipairs(KEYS) do
    redis.call('SET', key, 'Hello ' .. key)
end
",
                new Key[] {"keys:1", "keys:2"},
                Array.Empty<BulkString>()
            );
            await connection.ExecuteAsync(command).ConfigureAwait(false);

            var values = await connection.ExecuteAsync(
                new MGET("keys:1", "keys:2")
            ).ConfigureAwait(false);
            values.Should().Equal(
                "Hello keys:1",
                "Hello keys:2"
            );
        }

        [Test]
        [TestCaseSource(nameof(ReturnCases))]
        public async Task<DataType> Return(string script)
        {
            var command = new EVAL(script, new Key[] {"some-key"}, new BulkString[] {"some-argument"});
            return await connection.ExecuteAsync(command).ConfigureAwait(false);
        }

        [Test]
        public async Task Cause_Exists_To_Return_True()
        {
            const string script = "return redis.call('LOLWUT')";
            var eval = new EVAL(script, Array.Empty<Key>(), Array.Empty<BulkString>());
            await connection.ExecuteAsync(eval).ConfigureAwait(false);

            var scriptHash = Sha1.Create(script);
            var scriptExists = new SCRIPT.EXISTS(scriptHash);
            var response = await connection.ExecuteAsync(scriptExists).ConfigureAwait(false);

            response.Should().Equal((scriptHash, true));
        }
    }
}
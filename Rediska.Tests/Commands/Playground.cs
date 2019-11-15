namespace Rediska.Tests.Commands
{
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Rediska.Commands.Lists;

    public sealed class Playground
    {
        [Test]
        public async Task Test()
        {
            var connection = new SimpleConnection();
            var command = new LINSERT(
                "name:sub-name",
                LINSERT.Where.BEFORE,
                "Pivot",
                "BeforePivot"
            );
            var result = await connection.ExecuteAsync(command).ConfigureAwait(false);

        }
    }
}
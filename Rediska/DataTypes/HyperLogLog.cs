namespace Rediska.DataTypes
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Commands.HyperLogLog;
    using Protocol;
    using Utils;

    public sealed class HyperLogLog
    {
        private readonly Connection connection;

        public HyperLogLog(Connection connection, Key key)
        {
            this.connection = connection;
            Key = key;
        }

        public Key Key { get; }

        public async Task<PFADD.Response> AddAsync(IReadOnlyList<BulkString> values)
        {
            var command = new PFADD(Key, values);
            return await connection.ExecuteAsync(command).ConfigureAwait(false);
        }

        public async Task<long> CountAsync()
        {
            var command = new PFCOUNT(Key);
            return await connection.ExecuteAsync(command).ConfigureAwait(false);
        }

        public async Task<HyperLogLog> CopyAsync(Key destination)
        {
            var command = new PFMERGE(destination, Key);
            await connection.ExecuteAsync(command).ConfigureAwait(false);
            return new HyperLogLog(connection, destination);
        }

        public async Task<HyperLogLog> MergeAsync(Key destination, IReadOnlyList<HyperLogLog> others)
        {
            var command = new PFMERGE(
                destination,
                new PrefixedList<Key>(
                    Key,
                    others.Select(hyperLogLog => hyperLogLog.Key).ToList()
                )
            );
            await connection.ExecuteAsync(command).ConfigureAwait(false);
            return new HyperLogLog(connection, destination);
        }
    }
}
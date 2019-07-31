using Rediska.Protocol;

namespace Rediska.Commands.Sets
{
    public static partial class SRANDMEMBER
    {
        private static readonly PlainBulkString name = new PlainBulkString("SRANDMEMBER");
    }
}
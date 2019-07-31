namespace Rediska.Commands.Sets
{
    using Protocol;

    public static partial class SRANDMEMBER
    {
        private static readonly PlainBulkString name = new PlainBulkString("SRANDMEMBER");
    }
}
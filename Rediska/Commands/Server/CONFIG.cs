namespace Rediska.Commands.Server
{
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public static partial class CONFIG
    {
        private static readonly PlainBulkString name = new PlainBulkString("CONFIG");
    }
}
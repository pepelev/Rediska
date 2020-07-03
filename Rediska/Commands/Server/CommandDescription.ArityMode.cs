namespace Rediska.Commands.Server
{
    public sealed partial class CommandDescription
    {
        public enum ArityMode : byte
        {
            FixedArgumentCount = 0,
            MinimumArgumentCount = 1
        }
    }
}
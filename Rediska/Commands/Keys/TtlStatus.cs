namespace Rediska.Commands.Keys
{
    public enum TtlStatus : byte
    {
        Ok = 0,
        KeyNotExists = 1,
        NoExpiration = 2
    }
}
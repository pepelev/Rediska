namespace Rediska.Tests.Checks
{
    public sealed class CRLFCheck : Check
    {
        public static CRLFCheck Singleton { get; } = new CRLFCheck();

        public override void Verify(Input input)
        {
            input.ReadCRLF();
        }
    }
}
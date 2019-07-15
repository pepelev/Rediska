using System.IO;

namespace Rediska.Tests.Reading
{
    public sealed class End : State
    {
        public static readonly End Singleton = new End();

        public override State Transit(Stream stream)
        {
            return this;
        }

        public override bool IsTerminal => true;
    }
}
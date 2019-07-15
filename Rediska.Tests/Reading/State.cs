using System.IO;

namespace Rediska.Tests.Reading
{
    public abstract class State
    {
        public abstract State Transit(Stream stream);
        public abstract bool IsTerminal { get; }
    }
}
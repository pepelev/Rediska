using System;

namespace Rediska.Protocol.Visitors
{
    public sealed class VisitException : Exception
    {
        public VisitException(string message, DataType subject)
            : base($"{message}, but found '{subject}'")
        {
            Subject = subject;
        }

        public DataType Subject { get; }
    }
}
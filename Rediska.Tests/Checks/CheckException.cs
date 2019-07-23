using System;
using Rediska.Protocol;
using Rediska.Protocol.Inputs;

namespace Rediska.Tests.Checks
{
    public sealed class CheckException : Exception
    {
        public Check Check { get; }
        public Input Input { get; }
        public object Subject { get; }

        public CheckException(string message, Check check, Input input)
            : this(message, check, input, null)
        {
        }

        public CheckException(string message, Check check, Input input, object subject)
            : base(message)
        {
            Check = check;
            Input = input;
            Subject = subject;
        }
    }
}
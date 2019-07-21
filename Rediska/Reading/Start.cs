using System;
using System.IO;
using Rediska.Protocol;

namespace Rediska.Reading
{
    public sealed class Start : State
    {
        public static Start Singleton { get; } = new Start();

        public override State Transit(Stream stream)
        {
            var firstByte = stream.ReadByte();
            if (firstByte == -1)
                return this;

            var type = new Magic((byte) firstByte);
            if (type == Magic.SimpleString || type == Magic.Error || type == Magic.Integer)
            {
                return SimpleString.Singleton;
            }

            if (type == Magic.BulkString)
            {
                return BulkStringStart.Singleton;
            }

            if (type == Magic.Array)
            {
                return ArrayStart.Singleton;
            }

            throw new ArgumentException("Invalid magic");
        }

        public override bool IsTerminal => false;
    }
}
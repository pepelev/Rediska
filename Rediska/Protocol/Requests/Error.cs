using System;
using System.Text;

namespace Rediska.Protocol.Requests
{
    public sealed class Error : DataType
    {
        private static readonly char[] CRLF = { '\r', '\n' };
        private readonly string content;

        public Error(string content)
        {
            if (content.IndexOfAny(CRLF) >= 0)
                throw new ArgumentException("Content must not contain CRLF");

            this.content = content;
        }

        public override void Write(Output output)
        {
            output.Write(Magic.Error);
            var bytes = Encoding.ASCII.GetBytes(content);
            output.Write(bytes);
            output.WriteCRLF();
        }
    }
}
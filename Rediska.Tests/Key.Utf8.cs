using System.Text;

namespace Rediska.Tests
{
    public abstract partial class Key
    {
        public sealed class Utf8 : Key
        {
            private readonly string value;

            public Utf8(string value)
            {
                this.value = value;
            }

            public override byte[] ToBytes() => Encoding.UTF8.GetBytes(value);
            public override string ToString() => value;
        }
    }
}
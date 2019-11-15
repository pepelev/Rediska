namespace Rediska.Commands.PublishSubscribe
{
    using System;
    using System.Text;

    public readonly struct Channel : IEquatable<Channel>
    {
        private readonly string name;

        public Channel(string name)
        {
            this.name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public byte[] ToBytes() => Encoding.UTF8.GetBytes(name);
        public override int GetHashCode() => name.GetHashCode();
        public override string ToString() => name;
        public bool Equals(Channel other) => name == other.name;
        public override bool Equals(object obj) => obj is Channel other && Equals(other);
        public static implicit operator Channel(string name) => new Channel(name);
    }
}
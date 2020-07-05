namespace Rediska.Commands.PublishSubscribe
{
    using System;

    public readonly struct Channel : IEquatable<Channel>
    {
        public Channel(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; }
        public bool Equals(Channel other) => Name == other.Name;
        public static implicit operator Channel(string name) => new Channel(name);

        // todo Name can be null
        public override int GetHashCode() => Name.GetHashCode();
        public override string ToString() => Name;
        public override bool Equals(object obj) => obj is Channel other && Equals(other);
    }
}
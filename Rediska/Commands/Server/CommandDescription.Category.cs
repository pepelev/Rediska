namespace Rediska.Commands.Server
{
    using System;

    public sealed partial class CommandDescription
    {
        public readonly struct Category : IEquatable<Category>
        {
            private static readonly StringComparer equality = StringComparer.CurrentCultureIgnoreCase;
            public static Category Admin => new Category("admin");
            public static Category Bitmap => new Category("bitmap");
            public static Category Blocking => new Category("blocking");
            public static Category Connection => new Category("connection");
            public static Category Dangerous => new Category("dangerous");
            public static Category Fast => new Category("fast");
            public static Category Geo => new Category("geo");
            public static Category Hash => new Category("hash");
            public static Category HyperLogLog => new Category("hyperloglog");
            public static Category Keyspace => new Category("keyspace");
            public static Category List => new Category("list");
            public static Category PubSub => new Category("pubsub");
            public static Category Read => new Category("read");
            public static Category Scripting => new Category("scripting");
            public static Category Set => new Category("set");
            public static Category Slow => new Category("slow");
            public static Category SortedSet => new Category("sortedset");
            public static Category Stream => new Category("stream");
            public static Category String => new Category("string");
            public static Category Transaction => new Category("transaction");
            public static Category Write => new Category("write");
            private readonly string name;

            public Category(string name)
            {
                this.name = name;
            }

            public string Name => name ?? "";
            public bool Equals(Category other) => equality.Equals(Name, other.Name);
            public static bool operator ==(Category left, Category right) => left.Equals(right);
            public static bool operator !=(Category left, Category right) => !left.Equals(right);
            public override string ToString() => $"@{Name}";
            public override bool Equals(object obj) => obj is Category other && Equals(other);
            public override int GetHashCode() => equality.GetHashCode(Name);
        }
    }
}
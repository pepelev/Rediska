namespace Rediska.Commands.Server
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class CommandDescription
    {
        private readonly IReadOnlyList<DataType> array;

        public CommandDescription(IReadOnlyList<DataType> array)
        {
            this.array = array;
        }

        public string Name => array[0].Accept(BulkStringExpectation.Singleton).ToString();
        public long Artiry => array[1].Accept(IntegerExpectation.Singleton);

        public IReadOnlyList<Flag> Flags => new ProjectingReadOnlyList<string, Flag>(
            array[2].Accept(CompositeVisitors.SimpleStringList),
            flagName => new Flag(flagName)
        );

        public IReadOnlyList<Category> Categories { get; }

        public readonly struct Category
        {

        }

        public readonly struct Flag : IEquatable<Flag>
        {
            private static readonly StringComparer equality = StringComparer.CurrentCultureIgnoreCase;
            private readonly string name;
            private string Name => name ?? "";

            public Flag(string name)
            {
                this.name = name;
            }

            public bool Equals(Flag other) => equality.Equals(Name, other.Name);
            public override bool Equals(object obj) => obj is Flag other && Equals(other);
            public override int GetHashCode() => equality.GetHashCode(Name);
            public static bool operator ==(Flag left, Flag right) => left.Equals(right);
            public static bool operator !=(Flag left, Flag right) => !left.Equals(right);
            public override string ToString() => Name;
        }
    }
}
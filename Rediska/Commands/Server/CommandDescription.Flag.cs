namespace Rediska.Commands.Server
{
    using System;

    public sealed partial class CommandDescription
    {
        public readonly struct Flag : IEquatable<Flag>
        {
            private static readonly StringComparer equality = StringComparer.CurrentCultureIgnoreCase;
            private readonly string name;
            private string Name => name ?? "";

            public Flag(string name)
            {
                this.name = name;
            }

            public bool Equals(Flag other) => equality.Equals(Name, (string) other.Name);
            public override bool Equals(object obj) => obj is Flag other && Equals(other);
            public override int GetHashCode() => equality.GetHashCode(Name);
            public static bool operator ==(Flag left, Flag right) => left.Equals(right);
            public static bool operator !=(Flag left, Flag right) => !left.Equals(right);
            public override string ToString() => Name;
            public static Flag Admin => new Flag("admin");
            public static Flag Asking => new Flag("asking");
            public static Flag DenyOutOfMemory => new Flag("denyoom");
            public static Flag Fast => new Flag("fast");
            public static Flag Loading => new Flag("loading");
            public static Flag MovableKeys => new Flag("movablekeys");
            public static Flag NoAuth => new Flag("no_auth");
            public static Flag NoScript => new Flag("noscript");
            public static Flag PubSub => new Flag("pubsub");
            public static Flag Random => new Flag("random");
            public static Flag Readonly => new Flag("readonly");
            public static Flag SkipMonitor => new Flag("skip_monitor");
            public static Flag SkipSlowLog => new Flag("skip_slowlog");
            public static Flag SortForScript => new Flag("sort_for_script");
            public static Flag Stale => new Flag("stale");
            public static Flag Write => new Flag("write");
        }
    }
}
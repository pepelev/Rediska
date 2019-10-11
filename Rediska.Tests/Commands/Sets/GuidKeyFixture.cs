namespace Rediska.Tests.Commands.Sets
{
    using System;

    public sealed class GuidKeyFixture : KeyFixture
    {
        public static GuidKeyFixture Singleton { get; } = new GuidKeyFixture();
        public override Key Create() => Guid.NewGuid().ToString();
    }
}
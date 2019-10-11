namespace Rediska.Tests.Commands.Sets
{
    using NUnit.Framework;

    public sealed class TestNameKeyFixture : KeyFixture
    {
        private readonly KeyFixture randomFixture;

        public TestNameKeyFixture(KeyFixture randomFixture)
        {
            this.randomFixture = randomFixture;
        }

        public override Key Create() => $"{TestContext.CurrentContext.Test.FullName}:{randomFixture.Create()}";
    }
}
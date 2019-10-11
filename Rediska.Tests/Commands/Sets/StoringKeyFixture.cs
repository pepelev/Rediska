namespace Rediska.Tests.Commands.Sets
{
    using System.Collections;
    using System.Collections.Generic;

    public sealed class StoringKeyFixture : KeyFixture, IEnumerable<Key>
    {
        private readonly KeyFixture fixture;
        private readonly List<Key> keys = new List<Key>();

        public StoringKeyFixture(KeyFixture fixture)
        {
            this.fixture = fixture;
        }

        public override Key Create()
        {
            var key = fixture.Create();
            keys.Add(key);
            return key;
        }

        public IEnumerator<Key> GetEnumerator() => keys.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
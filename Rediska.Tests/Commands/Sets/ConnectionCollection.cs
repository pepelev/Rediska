namespace Rediska.Tests.Commands.Sets
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Net;
    using NUnit.Framework;
    using Utilities;

    public sealed class ConnectionCollection : IEnumerable<TestFixtureData>
    {
        public IEnumerator<TestFixtureData> GetEnumerator()
        {
            yield return new TestFixtureData(
                new LazyConnection(
                    new IPEndPoint(IPAddress.Loopback, 6379)
                )
            );
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
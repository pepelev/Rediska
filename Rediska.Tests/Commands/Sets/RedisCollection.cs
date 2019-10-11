using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using NUnit.Framework;

namespace Rediska.Tests.Commands.Sets
{
    public sealed class RedisCollection : IEnumerable<TestFixtureData>
    {
        public IEnumerator<TestFixtureData> GetEnumerator()
        {
            var ubuntu = new IPAddress(
                new byte[] {192, 168, 56, 1}
            );
            return new[]
                {
                    new TestFixtureData(new IPEndPoint(ubuntu, 50260)).SetArgDisplayNames("redis-2.6"),
                    new TestFixtureData(new IPEndPoint(ubuntu, 50280)).SetArgDisplayNames("redis-2.8"),
                    new TestFixtureData(new IPEndPoint(ubuntu, 50320)).SetArgDisplayNames("redis-3.2"),
                    new TestFixtureData(new IPEndPoint(ubuntu, 50400)).SetArgDisplayNames("redis-4.0"),
                    new TestFixtureData(new IPEndPoint(ubuntu, 50500)).SetArgDisplayNames("redis-5.0")
                }
                .AsEnumerable()
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public sealed class LocalRedis : IEnumerable<TestFixtureData>
    {
        public IEnumerator<TestFixtureData> GetEnumerator()
        {
            var ubuntu = new IPAddress(
                new byte[] { 192, 168, 56, 1 }
            );
            return new[]
                {
                    new TestFixtureData(
                        new SimpleConnectionFixture(
                            new IPEndPoint(IPAddress.Loopback, 6379)
                        )
                    ).SetArgDisplayNames("local")
                }
                .AsEnumerable()
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
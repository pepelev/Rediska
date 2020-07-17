using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using Rediska.Protocol;
using Rediska.Tests.Checks;
using Rediska.Utils;

namespace Rediska.Tests
{
    public sealed class TemporaryResponseShould
    {


        [Test]
        public void Array()
        {
            var array = new PlainArray(
                Enumerable.Range(0, 1200)
                    .Select(_ => new PlainBulkString(new byte[1024]))
                    .ToArray()
            );
            var bytes = new Content(
                array
            ).AsBytes();

            var generate = Stopwatch.StartNew();
            var sut = new TemporaryResponse();
            var collection = sut.Feed(new ArraySegment<byte>(bytes));
            var generateElapsed = generate.Elapsed;
            Console.WriteLine(generateElapsed);
        }

        [Test]
        public void Test()
        {
            for (int i = 0; i < 10; i++)
            {
                var randomContent = new RandomContent(
                    0.7,
                    12,
                    new Random(47)
                );

                var generate = Stopwatch.StartNew();
                var dataType = randomContent.Generate();
                var bytes = new Content(
                    dataType
                ).AsBytes();
                var generateElapsed = generate.Elapsed;

                generate.Restart();
                var sut = new TemporaryResponse();
                var collection = sut.Feed(new ArraySegment<byte>(bytes));

                var feed = generate.Elapsed;

                collection.Should().HaveCount(1);

                Console.WriteLine($"{generateElapsed} {feed}");
            }
        }

        [Test]
        public void ReadSimpleString()
        {
            var bytes = new Content(
                new SimpleString("hello world")
            ).AsBytes();

            var sut = new TemporaryResponse();
            var collection = sut.Feed(new ArraySegment<byte>(bytes));
            collection.Should().HaveCount(1);
        }

        [Test]
        [Explicit("investigation")]
        public void ReadArray()
        {
            var bytes = new Content(
                new PlainArray(
                    new PlainBulkString("key"),
                    new Integer(500)
                )
            ).AsBytes();

            var response = new TemporaryResponse();
            var readOnlyCollection = response.Feed(new ArraySegment<byte>(bytes));
            var input = readOnlyCollection.Single();

            var check = new CompositeCheck(
                new MagicCheck(Magic.Array),
                new IntegerCheck(2),
                CRLFCheck.Singleton,

                new MagicCheck(Magic.BulkString),
                new IntegerCheck(3),
                CRLFCheck.Singleton,
                new BulkStringCheck(Encoding.UTF8.GetBytes("key")),
                CRLFCheck.Singleton,

                new MagicCheck(Magic.Integer),
                new IntegerCheck(500),
                CRLFCheck.Singleton
            );

            try
            {
                check.Verify(input);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Test]
        [Explicit("investigation")]
        public void ReadError()
        {
            var bytes = new Content(
                new Error("ERR connection refused")
            ).AsBytes();

            var sut = new TemporaryResponse();
            var collection = sut.Feed(new ArraySegment<byte>(bytes));
            collection.Should().HaveCount(1);

            var check = new CompositeCheck(
                new MagicCheck(Magic.Error),
                new SimpleStringCheck("ERR connection refused"),
                CRLFCheck.Singleton
            );

            check.Verify(collection.Single());
        }

        [Test]
        public void ReadNestedArray()
        {
            var bytes = new Content(
                new PlainArray(
                    new PlainBulkString("one"),
                    new PlainBulkString("two"),
                    new PlainBulkString("three"),
                    new PlainArray(
                        new PlainBulkString("three-one"),
                        new PlainBulkString("three-two"),
                        new PlainBulkString("three-three")
                    ),
                    new PlainBulkString("four")
                )
            ).AsBytes();

            var sut = new TemporaryResponse();
            var collection = sut.Feed(new ArraySegment<byte>(bytes));
            collection.Should().HaveCount(1);
        }

        [Test]
        [Explicit("investigation")]
        public void ReadInteger()
        {
            var bytes = new Content(
                new Integer(700)
            ).AsBytes();

            var sut = new TemporaryResponse();
            var collection = sut.Feed(new ArraySegment<byte>(bytes));
            collection.Should().HaveCount(1);

            var check = new CompositeCheck(
                new MagicCheck(Magic.Integer),
                new IntegerCheck(700),
                CRLFCheck.Singleton
            );

            check.Verify(collection.Single());
        }

        [Test]
        [Explicit("investigation")]
        public void ReadBulkString()
        {
            var bytes = new Content(
                new PlainBulkString(new byte[] {1,2,3,4})
            ).AsBytes();

            var sut = new TemporaryResponse();
            var collection = sut.Feed(new ArraySegment<byte>(bytes));
            collection.Should().HaveCount(1);

            var check = new CompositeCheck(
                new MagicCheck(Magic.BulkString),
                new BulkStringCheck(new byte[] { 1, 2, 3, 4 }),
                CRLFCheck.Singleton
            );

            check.Verify(collection.Single());
        }
    }
}
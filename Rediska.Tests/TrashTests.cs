using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rediska.Commands;
using Rediska.Commands.Sets;
using Rediska.Protocol;
using Rediska.Protocol.Requests;
using Rediska.Protocol.Responses.Visitors;
using Rediska.Tests.Checks;
using Array = Rediska.Protocol.Requests.Array;

namespace Rediska.Tests
{
    public class TrashTests
    {
        [Test]
        public async Task Test()
        {
            var connection = new SimpleConnection();
            Rediska.Key key = "set-test";
            var add = new SADD(
                key,
                new[]
                {
                    new BulkString("one"),
                    new BulkString("two"),
                    new BulkString("three"),
                    new BulkString("four")
                }
            );
            var added = await connection.ExecuteAsync(add).ConfigureAwait(false);
            var pop = new SPOP.Multiple("dos", 2);
            var popped = await connection.ExecuteAsync(pop).ConfigureAwait(false);

        }

        [Test]
        [TestCase("bosya:12", ExpectedResult = "think")]
        public async Task<object> GetString(string key)
        {
            var redis = new SimpleConnection();
            return await redis.ExecuteAsync(new GET(key)).ConfigureAwait(false);
        }

        [Test]
        public async Task EchoTest()
        {
            var redis = new SimpleConnection();
            var foobar = await redis.ExecuteAsync(new ECHO("foobar")).ConfigureAwait(false);
            Console.WriteLine(foobar);
        }

        [Test]
        public async Task Bosyata2()
        {
            var redis = new SimpleConnection();
            await redis.ExecuteAsync(new Add()).ConfigureAwait(false);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            await redis.ExecuteAsync(new Add()).ConfigureAwait(false);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
        }

        private sealed class Add : Command<object>
        {
            public override DataType Request
            {
                get
                {
                    return new Array(
                        new DataType[]
                            {
                                new BulkString("PFADD"),
                                new BulkString("persik")
                            }
                            .Concat(Generate())
                            .ToList()
                    );

                    IEnumerable<DataType> Generate()
                    {
                        for (var i = 0; i < 100_000; i++)
                        {
                            yield return new BulkString(Guid.NewGuid().ToByteArray());
                        }
                    }
                }
            }

            public override Visitor<object> ResponseStructure => new ConstVisitor<object>(null);
        }

        [Test]
        public void Bosyata()
        {
            var bytes = new Content(
                new Array(
                    new Integer(40),
                    new Integer(50),
                    new Integer(60),
                    new Integer(70),
                    new BulkString(@"bo\syata"),
                    new Array(
                        new Integer(40),
                        new Integer(50),
                        new Integer(60),
                        new Integer(70),
                        new Array(
                            new Integer(40),
                            new Integer(50),
                            new Integer(60),
                            new Integer(70),
                            new Array(
                                new Integer(40),
                                new Integer(50),
                                new Integer(60),
                                new Integer(70),
                                new Array(
                                    new Integer(40),
                                    new Integer(50),
                                    new Integer(60),
                                    new Integer(70)
                                )
                            )
                        )
                    )
                )
            ).AsBytes();
            Console.WriteLine(
                Encoding.ASCII.GetString(bytes)
            );
        }

        

        public sealed class Structure<T>
        {
            private readonly Func<string, T> errorOutcome;
            private readonly T protocolViolation;
            private readonly Func<string, T> stringOutcome;

            public Structure(Func<string, T> errorOutcome, Func<string, T> stringOutcome, T protocolViolation)
            {
                this.errorOutcome = errorOutcome;
                this.stringOutcome = stringOutcome;
                this.protocolViolation = protocolViolation;
            }

            public T Match(Input input)
            {
                var magic = input.ReadMagic();
                if (magic == Magic.Error)
                {
                    var message = input.ReadSimpleString();
                    return errorOutcome(message);
                }

                if (magic == Magic.BulkString)
                {
                    var text = Encoding.UTF8.GetString(new byte[0]);
                    return stringOutcome(text);
                }

                return protocolViolation;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rediska.Tests.Checks;

namespace Rediska.Tests
{
    public class TrashTests
    {
        [Test]
        [TestCase("bosya:12", ExpectedResult = "5\r\nthink")]
        public async Task<string> GetString(string key)
        {
            var redis = new Redis();
            return await redis.ExecuteAsync(new Get(key)).ConfigureAwait(false);
        }

        [Test]
        public async Task EchoTest()
        {
            var redis = new Redis();
            var foobar = await redis.ExecuteAsync(new Echo("foobar")).ConfigureAwait(false);
            Console.WriteLine(foobar);
        }

        [Test]
        public async Task Bosyata2()
        {
            var redis = new Redis();
            await redis.ExecuteAsync(new Add()).ConfigureAwait(false);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            await redis.ExecuteAsync(new Add()).ConfigureAwait(false);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
        }

        private sealed class Add : Command<object>
        {
            public override void Write(Output request)
            {
                new Array(
                    new DataType[]
                        {
                            new BulkString("PFADD"),
                            new BulkString("persik")
                        }
                        .Concat(Generate())
                        .ToList()
                ).Write(request);

                IEnumerable<DataType> Generate()
                {
                    for (var i = 0; i < 100_000; i++)
                    {
                        yield return new BulkString(Guid.NewGuid().ToByteArray());
                    }
                }
            }

            public override object Read(Input response)
            {
                return null;
            }
        }

        public sealed class Echo : Command<string>
        {
            private readonly string message;

            public Echo(string message)
            {
                this.message = message;
            }

            public override void Write(Output request)
            {
                new Array(
                    new BulkString("ECHO"),
                    new BulkString(message)
                ).Write(request);
            }

            public override string Read(Input response)
            {
                var type = response.ReadMagic();
                if (type == Magic.BulkString)
                {
                    var bulkString = response.ReadBulkString();
                    if (bulkString.IsNull)
                        return null;

                    return Encoding.UTF8.GetString(
                        bulkString.ToArray()
                    );
                }

                throw new Exception("tudum"); // todo
            }
        }

        public sealed class Get : Command<string>
        {
            private readonly string key;

            public Get(string key)
            {
                this.key = key;
            }

            public override void Write(Output request)
            {
                new Array(
                    new BulkString("GET"),
                    new BulkString(key)
                ).Write(request);
            }

            public override string Read(Input response)
            {
                return new Structure<string>(
                    text => "EGOR: " + text,
                    result => result,
                    "protocol violation"
                ).Match(response);
            }
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
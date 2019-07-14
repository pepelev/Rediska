using System;
using FluentAssertions;
using NUnit.Framework;

namespace Rediska.Tests
{
    public sealed class VerifyingOutputShould
    {
        [Test]
        public void VerifyArrayWithBulkString()
        {
            var sut = new VerifyingOutput();
            sut.Write(Magic.Array);
            {
                sut.Write(1);
                sut.WriteCRLF();

                sut.Write(Magic.BulkString);
                {
                    sut.Write(5);
                    sut.WriteCRLF();

                    sut.Write(new byte[] {0x01, 0x02, 0x03, 0x04, 0x05});
                }
                sut.WriteCRLF();
            }
            sut.WriteCRLF();

            sut.VerifyCompleted().Should().BeTrue();
        }

        [Test]
        public void VerifyArrayWithInteger()
        {
            var sut = new VerifyingOutput();
            sut.Write(Magic.Array);
            {
                sut.Write(1);
                sut.WriteCRLF();

                sut.Write(Magic.Integer);
                sut.Write(10);
                sut.WriteCRLF();
            }
            sut.WriteCRLF();

            sut.VerifyCompleted().Should().BeTrue();
        }

        [Test]
        public void VerifyEmptyArray()
        {
            var sut = new VerifyingOutput();
            sut.Write(Magic.Array);
            sut.Write(0);
            sut.WriteCRLF();
            sut.WriteCRLF();

            sut.VerifyCompleted().Should().BeTrue();
        }

        private static readonly Magic[] NonArrayMagics =
        {
            Magic.BulkString,
            Magic.Error,
            Magic.Integer,
            Magic.SimpleString
        };

        [Test]
        public void DenyStartMagicToBeNonArray([ValueSource(nameof(NonArrayMagics))] Magic magic)
        {
            var sut = new VerifyingOutput();
            Assert.Throws<InvalidOperationException>(
                () => sut.Write(magic)
            );
        }

        [Test]
        public void VerifyArrayLength()
        {
            // todo
        }
    }
}
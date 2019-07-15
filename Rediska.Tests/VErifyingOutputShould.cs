using System;
using FluentAssertions;
using NUnit.Framework;
using Rediska.Tests.Checks;

namespace Rediska.Tests
{
    public sealed class VerifyingOutputShould
    {
        private static readonly Magic[] NonArrayMagics =
        {
            Magic.BulkString,
            Magic.Error,
            Magic.Integer,
            Magic.SimpleString
        };

        private static readonly Magic[] AllowedMagics =
        {
            Magic.Array,
            Magic.Integer, 
            Magic.BulkString
        };

        private VerifyingOutput sut;

        [SetUp]
        public void SetUp()
        {
            sut = new VerifyingOutput();
        }

        [Test]
        public void AllowToWriteBulkStringInMultipleWrites()
        {
            sut.Write(Magic.Array);
            {
                sut.Write(1);
                sut.WriteCRLF();

                sut.Write(Magic.BulkString);
                {
                    sut.Write(5);
                    sut.WriteCRLF();

                    sut.Write(new byte[] {0x01, 0x02});
                    sut.Write(new byte[] {0x03});
                    sut.Write(new byte[] {0x04, 0x05});
                }
                sut.WriteCRLF();
            }

            sut.Completed().Should().BeTrue();
        }

        [Test]
        public void VerifyArrayWithBulkString()
        {
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

            sut.Completed().Should().BeTrue();
        }

        [Test]
        public void VerifyArrayWithInteger()
        {
            sut.Write(Magic.Array);
            {
                sut.Write(1);
                sut.WriteCRLF();

                sut.Write(Magic.Integer);
                sut.Write(10);
                sut.WriteCRLF();
            }

            sut.Completed().Should().BeTrue();
        }

        [Test]
        public void VerifyEmptyArray()
        {
            sut.Write(Magic.Array);
            sut.Write(0);
            sut.WriteCRLF();

            sut.Completed().Should().BeTrue();
        }

        [Test]
        public void VerifyNullArray()
        {
            sut.Write(Magic.Array);
            sut.Write(-1);
            sut.WriteCRLF();

            sut.Completed().Should().BeTrue();
        }

        [Test]
        public void DenyStartMagicToBeNonArray([ValueSource(nameof(NonArrayMagics))] Magic magic)
        {
            Assert.Throws<InvalidOperationException>(
                () => sut.Write(magic)
            );
        }

        [Test]
        public void DenyIntegerAtStart()
        {
            Assert.Throws<InvalidOperationException>(
                () => sut.Write(10L)
            );
        }

        [Test]
        public void DenyBytesAtStart()
        {
            Assert.Throws<InvalidOperationException>(
                () => sut.Write(new byte[] {0x00, 0x01})
            );
        }

        [Test]
        public void DenyCRLFAtStart()
        {
            Assert.Throws<InvalidOperationException>(
                () => sut.WriteCRLF()
            );
        }

        [Test]
        public void DenyLessElementsInArrayThatDeclared()
        {
            sut.Write(Magic.Array);
            {
                sut.Write(2);
                sut.WriteCRLF();

                sut.Write(Magic.Integer);
                sut.Write(42L);
                sut.WriteCRLF();
            }
            Assert.Throws<InvalidOperationException>(
                sut.WriteCRLF
            );
        }

        [Test]
        public void DenyMoreElementsInArrayThatDeclared([ValueSource(nameof(AllowedMagics))] Magic magic)
        {
            sut.Write(Magic.Array);
            {
                sut.Write(2);
                sut.WriteCRLF();

                sut.Write(Magic.Integer);
                sut.Write(42L);
                sut.WriteCRLF();

                sut.Write(Magic.Integer);
                sut.Write(43L);
                sut.WriteCRLF();
                
                Assert.Throws<InvalidOperationException>(
                    () => sut.Write(magic)
                );
            }
        }
    }
}
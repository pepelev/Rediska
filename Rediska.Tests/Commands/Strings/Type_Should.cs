namespace Rediska.Tests.Commands.Strings
{
    using NUnit.Framework;
    using Rediska.Commands.Strings;
    using static Rediska.Commands.Strings.TypeKind;

    public sealed class Type_Should
    {
        public static TestCaseData[] RangeCases => new[]
        {
            new TestCaseData(Signed, 1).Returns((-1L, 0L)),
            new TestCaseData(Signed, 5).Returns((-16L, 15L)),
            new TestCaseData(Signed, 63).Returns((long.MinValue / 2, long.MaxValue / 2)),
            new TestCaseData(Signed, 64).Returns((long.MinValue, long.MaxValue)),

            new TestCaseData(Unsigned, 1).Returns((0, 1L)),
            new TestCaseData(Unsigned, 7).Returns((0, 127L)),
            new TestCaseData(Unsigned, 63).Returns((0, long.MaxValue)),
        };

        [Test]
        [TestCaseSource(nameof(RangeCases))]
        public (long Left, long Right) Give_Range(TypeKind kind, int bits)
        {
            var sut = new Type(kind, checked((byte) bits));
            return sut.Range;
        }
    }
}
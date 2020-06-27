namespace Rediska.Tests.Utilities
{
    using System.Threading;

    internal sealed class AtomicLong
    {
        private long value;

        public AtomicLong()
            : this(0)
        {
        }

        public AtomicLong(long value)
        {
            this.value = value;
        }

        public long Read() => Interlocked.Read(ref value);
        public long Write(long newValue) => Interlocked.Exchange(ref value, newValue);
        public Condition IfEqualTo(long expectedValue) => new Condition(expectedValue, this);

        public struct Condition
        {
            private readonly AtomicLong @long;
            private readonly long expectedValue;

            public Condition(long expectedValue, AtomicLong @long)
            {
                this.expectedValue = expectedValue;
                this.@long = @long;
            }

            public Result Write(long value)
            {
                var oldValue = Interlocked.CompareExchange(
                    ref @long.value,
                    value,
                    expectedValue
                );
                return new Result(
                    expectedValue,
                    oldValue,
                    value
                );
            }
        }

        public struct Result
        {
            private readonly long writeIntention;

            public Result(long expectedValue, long oldValue, long writeIntention)
            {
                ExpectedValue = expectedValue;
                OldValue = oldValue;
                this.writeIntention = writeIntention;
            }

            public long ExpectedValue { get; }
            public long OldValue { get; }

            public long NewValue => ExpectedValue == OldValue
                ? writeIntention
                : OldValue;

            public bool Success => ExpectedValue == OldValue;
        }

        public IncrementResult Increment()
        {
            var newValue = Interlocked.Increment(ref value);
            return new IncrementResult(newValue);
        }

        public struct IncrementResult
        {
            public IncrementResult(long newValue)
            {
                NewValue = newValue;
            }

            public long NewValue { get; }
            public long OldValue => NewValue - 1;
        }
    }
}
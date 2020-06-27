using System;
using System.Collections;
using System.Collections.Generic;
using Rediska.Protocol;
using Rediska.Utils;

namespace Rediska.Commands
{
    public abstract class Match : IReadOnlyList<DataType>
    {
        public static Match All { get; } = new AllMatch();
        public static Match Pattern(string pattern) => new PatternMatch(pattern);

        public abstract IEnumerator<DataType> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public abstract int Count { get; }
        public abstract DataType this[int index] { get; }

        private sealed class AllMatch : Match
        {
            public override int Count => 0;
            public override DataType this[int index] => throw new IndexOutOfRangeException();
            public override IEnumerator<DataType> GetEnumerator() => EmptyEnumerator<DataType>.Singleton;
            public override string ToString() => "*";
        }

        private sealed class PatternMatch : Match
        {
            private static readonly PlainBulkString keyWord = new PlainBulkString("MATCH");

            private readonly string pattern;

            public PatternMatch(string pattern)
            {
                this.pattern = pattern;
            }

            public override IEnumerator<DataType> GetEnumerator()
            {
                yield return keyWord;
                yield return Pattern;
            }

            public override int Count => 2;

            public override DataType this[int index]
            {
                get
                {
                    return index switch
                    {
                        0 => keyWord,
                        1 => Pattern,
                        _ => throw new IndexOutOfRangeException()
                    };
                }
            }

            private DataType Pattern => new PlainBulkString(pattern);
        }
    }
}
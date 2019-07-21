using System;
using System.Linq;
using Rediska.Protocol.Requests;
using Array = Rediska.Protocol.Requests.Array;

namespace Rediska.Tests
{
    public sealed class RandomContent
    {
        private static int deepest = 0;
        private static ulong items = 0;

        private readonly double arrayThreshold;
        private readonly int maxArrayElements;
        private readonly Random random;
        private readonly int deep;

        public RandomContent(double arrayThreshold, int maxArrayElements, Random random)
            : this(arrayThreshold, maxArrayElements, random, 1)
        {
        }

        private RandomContent(double arrayThreshold, int maxArrayElements, Random random, int deep)
        {
            this.arrayThreshold = arrayThreshold;
            this.maxArrayElements = maxArrayElements;
            this.random = random;
            this.deep = deep;
            deepest = Math.Max(deep, deepest);
        }

        public DataType Generate()
        {
            var next = random.NextDouble();
            if (arrayThreshold > next)
            {
                var count = random.Next(-1, maxArrayElements + 1);
                if (count == -1)
                    return Array.Null;

                return new Array(
                    Enumerable
                        .Repeat(0, count)
                        .Select(_ => SubGenerate())
                        .ToList()
                );
            }

            checked
            {
                items++;
            }

            var other = 1.0 - arrayThreshold;
            var chunk = other / 4;
            var item = next - arrayThreshold;
            if (item < chunk)
                return new SimpleString("dos");
            if (item < 2 * chunk)
                return new Integer(random.Next(int.MinValue, int.MaxValue));
            if (item < 3 * chunk)
                return new BulkString(new byte[2]);
            return new Error("ERR");
        }

        private DataType SubGenerate()
        {
            var content = new RandomContent(
                arrayThreshold * 0.9,
                maxArrayElements,
                random,
                deep + 1
            );
            return content.Generate();
        }
    }
}
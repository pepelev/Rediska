using BenchmarkDotNet.Running;

namespace Rediska.Tests.Performance
{
    public static class EntryPoint
    {
        public static int Main()
        {
            var summary = BenchmarkRunner.Run<CallCost>();
            return 0;
        }
    }
}
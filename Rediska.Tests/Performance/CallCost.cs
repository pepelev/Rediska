using System;
using BenchmarkDotNet.Attributes;

namespace Rediska.Tests.Performance
{
    
    public class CallCost
    {
        private readonly Base @abstract = new Child();
        private readonly IBase @interface = new InterfaceChild();

        private int Method()
        {
            return 10;
        }

        [Benchmark]
        public int Baseline()
        {
            return Method();
        }

        [Benchmark]
        public int AbstractGenericMethod()
        {
            return @abstract.GenericMethod<Guid>();
        }

        [Benchmark]
        public int InterfaceGenericMethod()
        {
            return @interface.GenericMethod<Guid>();
        }

        [Benchmark]
        public int AbstractMethod()
        {
            return @abstract.Method();
        }

        [Benchmark]
        public int InterfaceMethod()
        {
            return @interface.Method();
        }

        private interface IBase
        {
            int GenericMethod<T>();
            int Method();
        }

        private class InterfaceChild : IBase
        {
            public int GenericMethod<T>()
            {
                return 10;
            }

            public int Method()
            {
                return 10;
            }
        }

        private abstract class Base
        {
            public abstract int GenericMethod<T>();
            public abstract int Method();
        }

        private sealed class Child : Base
        {
            public override int GenericMethod<T>()
            {
                return 10;
            }

            public override int Method()
            {
                return 10;
            }
        }
    }
}
namespace Rediska.Commands.Server
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed partial class CommandDescription
    {
        private readonly IReadOnlyList<DataType> array;

        public CommandDescription(IReadOnlyList<DataType> array)
        {
            this.array = array;
        }

        public string Name => array[0].Accept(BulkStringExpectation.Singleton).ToString();
        public ArityDescription Aritry => new ArityDescription(array[1].Accept(IntegerExpectation.Singleton));

        public IReadOnlyList<Flag> Flags => new PrettyReadOnlyList<Flag>(
            new ProjectingReadOnlyList<string, Flag>(
                array[2].Accept(CompositeVisitors.SimpleStringList),
                flagName => new Flag(flagName)
            )
        );

        public Index FirstKeyPosition => array[3].Accept(IntegerExpectation.Singleton);
        public Index LastKeyPosition => array[4].Accept(IntegerExpectation.Singleton);
        public long KeyStepCount => array[5].Accept(IntegerExpectation.Singleton);

        public IReadOnlyList<Category> Categories => array.Count <= 6
            ? new Category[0] as IReadOnlyList<Category>
            : new PrettyReadOnlyList<Category>(
                new ProjectingReadOnlyList<string, Category>(
                    array[6].Accept(CompositeVisitors.SimpleStringList),
                    categoryName =>
                    {
                        if (categoryName.StartsWith("@"))
                        {
                            return new Category(categoryName.Substring(1));
                        }

                        throw new ArgumentException("Category must starts with @", nameof(categoryName));
                    }
                )
            );

        public override string ToString() => Name;
    }
}
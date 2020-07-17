namespace Rediska.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using FluentAssertions;
    using NUnit.Framework;
    using Protocol;
    using Rediska.Commands;

    [Explicit("Need to be fixed")]
    public class StaticBulkStringDuplicates
    {
        [Test]
        public void METHOD()
        {
            var duplicates =
            (
                from type in typeof(Command<>).Assembly.DefinedTypes
                from field in type.GetFields(Flags)
                where typeof(BulkString).IsAssignableFrom(field.FieldType)
                let value = (BulkString) field.GetValue(null)
                group field by value
                into @group
                where @group.Count() > 1
                select @group
            ).ToList();

            Console.WriteLine(
                string.Join(
                    Environment.NewLine,
                    duplicates.Select(group => $"{@group.Key}: {string.Join(", ", @group.Select(Print))}")
                )
            );

            duplicates.Should().BeEmpty();

            string Print(FieldInfo field) => $"{field.DeclaringType?.FullName}.{field.Name}";
        }

        private static BindingFlags Flags => BindingFlags.Static |
                                             BindingFlags.Public |
                                             BindingFlags.NonPublic |
                                             BindingFlags.DeclaredOnly;
    }
}
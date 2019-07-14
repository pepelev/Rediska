using System.Collections.Generic;

namespace Rediska.Tests
{
    public static class StackExtensions
    {
        public static void ReplaceTop<T>(this Stack<T> stack, T element)
        {
            stack.Pop();
            stack.Push(element);
        }
    }
}
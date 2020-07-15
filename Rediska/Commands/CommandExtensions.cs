namespace Rediska.Commands
{
    using Auxiliary;
    using Protocol.Visitors;
    using Utils;

    public static class CommandExtensions
    {
        public static Command<None> ResponseIgnoring<T>(this Command<T> command) => new ResponseIgnoring<T>(command);

        public static Command<TResult> WithResponseStructure<TInner, TResult>(
            this Command<TInner> command,
            Visitor<TResult> responseStructure)
            => new CustomResponseHandling<TInner, TResult>(command, responseStructure);
    }
}
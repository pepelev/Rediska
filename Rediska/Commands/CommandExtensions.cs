namespace Rediska.Commands
{
    using Auxiliary;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public static class CommandExtensions
    {
        public static Command<None> ResponseIgnoring<T>(this Command<T> command) => new ResponseIgnoring<T>(command);

        public static Command<TResult> WithResponseStructure<TInner, TResult>(
            this Command<TInner> command,
            Visitor<TResult> responseStructure)
            => new CustomResponseHandling<TInner, TResult>(command, responseStructure);

        public static Command<DataType> WithRawResponse<TInner>(this Command<TInner> command)
            => new CustomResponseHandling<TInner, DataType>(command, Id.Singleton);
    }
}
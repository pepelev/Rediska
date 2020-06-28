namespace Rediska.Commands.Geo
{
    using System.Collections.Generic;
    using Protocol;

    public abstract class ResponseFormat<T>
    {
        public abstract IEnumerable<BulkString> AdditionalArguments { get; }
        public abstract T Parse(DataType item);
    }
}
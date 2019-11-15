namespace Rediska.Protocol.Inputs
{
    using System;
    using Array = Protocol.Array;

    public static class InputExtensions
    {
        public static DataType Read(this Input input)
        {
            var type = input.ReadMagic();
            if (type == Magic.SimpleString)
            {
                var content = input.ReadSimpleString();
                return new SimpleString(content);
            }

            if (type == Magic.Error)
            {
                var content = input.ReadSimpleString();
                return new Error(content);
            }

            if (type == Magic.Integer)
            {
                var value = input.ReadInteger();
                return new Integer(value);
            }

            if (type == Magic.BulkString)
            {
                return input.ReadBulkString();
            }

            if (type == Magic.Array)
            {
                var count = input.ReadInteger();
                if (count == -1)
                    return Array.Null;

                var items = new DataType[count];
                for (var i = 0; i < items.Length; i++)
                {
                    items[i] = input.Read();
                }

                return new PlainArray(items);
            }

            throw new NotSupportedException($"Could not recognize magic: {type}");
        }
    }
}
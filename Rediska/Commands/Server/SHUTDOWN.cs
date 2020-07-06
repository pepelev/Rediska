namespace Rediska.Commands.Server
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class SHUTDOWN : Command<None>
    {
        private static readonly PlainBulkString name = new PlainBulkString("SHUTDOWN");
        private static readonly BulkString[] autoRequest = {name};
        private static readonly BulkString[] saveRequest = {name, new PlainBulkString("SAVE")};
        private static readonly BulkString[] noSaveRequest = {name, new PlainBulkString("NOSAVE")};
        private readonly ShutdownMode mode;

        public SHUTDOWN(ShutdownMode mode)
        {
            if (mode != ShutdownMode.Auto && mode != ShutdownMode.NoSave && mode != ShutdownMode.Save)
            {
                throw new ArgumentException(
                    $"Expected Auto, NoSave or Save, but {mode} found",
                    nameof(mode)
                );
            }

            this.mode = mode;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => mode switch
        {
            ShutdownMode.Auto => autoRequest,
            ShutdownMode.Save => saveRequest,
            _ => noSaveRequest
        };

        public override Visitor<None> ResponseStructure => ResponseVisitor.Singleton;

        private sealed class ResponseVisitor : Expectation<None>
        {
            public static ResponseVisitor Singleton { get; } = new ResponseVisitor();
            public override string Message => "no reply";
        }
    }
}
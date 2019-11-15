namespace Rediska.Commands.PublishSubscribe
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Protocol;
    using Protocol.Inputs;
    using Utils;

    public sealed class ListeningConnection
    {
        private readonly Stream stream;
        private readonly TemporaryResponse response = new TemporaryResponse();
        private readonly Queue<Input> inputs = new Queue<Input>();

        public ListeningConnection(Stream stream)
        {
            this.stream = stream;
        }

        public async Task<DataType> ListenAsync()
        {
            while (inputs.Count == 0)
            {
                var buffer = new byte[80 * 1024];
                var count = await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                var segment = new ArraySegment<byte>(buffer, 0, count);
                var receivedInputs = response.Feed(segment);
                foreach (var input in receivedInputs)
                {
                    inputs.Enqueue(input);
                }
            }

            return inputs.Dequeue().Read();
        }
    }
}
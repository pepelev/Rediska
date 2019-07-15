using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Rediska.Tests
{
    public sealed class BulkWriteStream : Stream
    {
        private readonly Stream target;
        private readonly Stream temporaryBuffer;

        public BulkWriteStream(
            Stream target,
            Stream temporaryBuffer)
        {
            this.target = target;
            this.temporaryBuffer = temporaryBuffer;
        }

        public override bool CanRead => false;
        public override bool CanSeek => false;
        public override bool CanWrite => true;
        public override long Length => target.Length + temporaryBuffer.Position;
        public override long Position
        {
            get => target.Position + temporaryBuffer.Position;
            set => throw new InvalidOperationException("Seek are not allowed");
        }

        public override async Task FlushAsync(CancellationToken cancellationToken)
        {
            temporaryBuffer.Position = 0L;
            var bufferSize = (int) Math.Min(81920, temporaryBuffer.Length);
            await temporaryBuffer.CopyToAsync(
                target,
                bufferSize,
                cancellationToken
            ).ConfigureAwait(false);
            temporaryBuffer.Position = 0L;
        }

        public override void Flush()
        {
            temporaryBuffer.Position = 0L;
            temporaryBuffer.CopyTo(target);
            temporaryBuffer.Position = 0L;
        }

        public override long Seek(long offset, SeekOrigin origin)
            => throw new InvalidOperationException("Seek are not allowed");

        public override void SetLength(long value) =>
            throw new InvalidOperationException("SetLength are not allowed");

        public override int Read(byte[] buffer, int offset, int count) => throw new InvalidOperationException("Read are not allowed");

        public override void Write(byte[] buffer, int offset, int count) =>
            temporaryBuffer.Write(buffer, offset, count);
    }
}
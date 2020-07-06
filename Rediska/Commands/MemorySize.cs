namespace Rediska.Commands
{
    using System.Globalization;
    using System.Linq;

    public readonly struct Size
    {
        private const ulong kibi = 1024;
        private const ulong mebi = kibi * kibi;
        private const ulong gibi = mebi * kibi;
        private const ulong tebi = gibi * kibi;
        private static CultureInfo Culture => CultureInfo.InvariantCulture;

        public Size(ulong totalBytes)
        {
            TotalBytes = totalBytes;
        }

        public ulong TotalBytes { get; }
        public ulong Bytes => TotalBytes % kibi;
        public ulong Kibibytes => TotalBytes % mebi / kibi;
        public ulong Mebibytes => TotalBytes % gibi / mebi;
        public ulong Gibibytes => TotalBytes % tebi / gibi;
        public ulong Tebibytes => TotalBytes / tebi;

        public override string ToString()
        {
            if (TotalBytes == 0)
                return "0B";

            var table = new (ulong Value, string Symbol)[]
            {
                (Tebibytes, "TiB"),
                (Gibibytes, "GiB"),
                (Mebibytes, "MiB"),
                (Kibibytes, "KiB"),
                (Bytes, "B")
            };
            var parts = table
                .Where(pair => pair.Value > 0)
                .Select(pair => $"{pair.Value.ToString("N0", Culture)}{pair.Symbol}");
            return string.Join(" ", parts);
        }
    }
}
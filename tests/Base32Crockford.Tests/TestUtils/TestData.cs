namespace Base32Crockford.Tests.TestUtils
{
    public record TestData
    {
        public required ulong Number { get; init; }
        public required string EncodedString { get; init; }
        public required char CheckSymbol { get; init; }
    }
}

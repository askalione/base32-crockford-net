namespace Base32Crockford.Tests.TestUtils
{
    internal static class StringExtensions
    {
        public static string SplitChunks(this string source, byte chunkSize)
        {
            int currentChunkSize = 0;
            string result = "";
            for (int i = 1; i <= source.Length; i++)
            {
                currentChunkSize++;
                result += source[i - 1];
                if (currentChunkSize == chunkSize && i != source.Length)
                {
                    currentChunkSize = 0;
                    result += "-";
                }
            }
            return result;
        }
    }
}

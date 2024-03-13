using System.Collections.Frozen;
using System.Text;

namespace Base32Crockford.Internal
{
    internal static class StringExtensions
    {
        public static IDictionary<char, char> Maketrans(this string source, string destionation)
        {
            ArgumentException.ThrowIfNullOrEmpty(source, nameof(source));
            ArgumentException.ThrowIfNullOrEmpty(destionation, nameof(destionation));

            IDictionary<char, char> result = Enumerable.Range(0, source.Length)
                .ToFrozenDictionary(i => source[i], i => destionation[i]);

            return result;
        }

        public static string Translate(this string source, IDictionary<char, char> translations)
        {
            ArgumentNullException.ThrowIfNull(translations, nameof(translations));

            var sb = new StringBuilder(source.Length);
            foreach(char sourceChar in source)
            {
                sb.Append(translations.ContainsKey(sourceChar) ? translations[sourceChar] : sourceChar);
            }
            return sb.ToString();
        }

        public static IEnumerable<int> Range(this string source, int step)
        {
            if (step < 1)
            {
                throw new ArgumentOutOfRangeException($"{nameof(step)} must be greater then 1");
            }

            int start = 0;
            int stop = source.Length;

            if (start < stop && step > 0)
            {
                for (int i = start; i < stop; i += step)
                {
                    yield return i;
                }
            }
            else if (start > stop && step < 0)
            {
                for (int i = start; i > stop; i += step)
                {
                    yield return i;
                }
            }
        }
    }
}

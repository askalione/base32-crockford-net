using System.Collections.Frozen;
using System.Text.RegularExpressions;
using Base32Crockford.Internal;

namespace Base32Crockford
{
    /// <summary>
    /// Provides methods to encode and decode numbers with Base32 Crockford algorithm.
    /// </summary>
    /// <remarks>
    /// See https://www.crockford.com/base32.html.
    /// </remarks>
    public sealed class Base32CrockfordEncoding
    {
        /// <summary>
        /// A symbol set of 10 digits and 22 letters for encoding alphabet.
        /// </summary>
        /// <remarks>
        /// Letters 'I', 'L', 'O' and 'U' were excluded.
        /// </remarks>
        public const string Symbols = "0123456789ABCDEFGHJKMNPQRSTVWXYZ";

        /// <summary>
        /// A set of 5 special characters needed only for checksum value.
        /// </summary>
        public const string CheckSymbols = "*~$=U";

        private static readonly Lazy<Base32CrockfordEncoding> _current = new Lazy<Base32CrockfordEncoding>(
            () => new Base32CrockfordEncoding());

        /// <summary>
        /// Returns singleton instance of <see cref="Base32CrockfordEncoding"/>.
        /// </summary>
        public static Base32CrockfordEncoding Current => _current.Value;

        private const string _allSymbols = Symbols + CheckSymbols;

        private readonly ulong _symbolsLength = (ulong)Symbols.Length;
        private readonly ulong _checkSymbolsLength = (ulong)_allSymbols.Length;

        private readonly IDictionary<ulong, char> _encodeSymbols = _allSymbols
            .Select((c, i) => (Char: c, Index: i))
            .ToFrozenDictionary(x => (ulong)x.Index, x => x.Char);
        private readonly IDictionary<char, ulong> _decodeSymbols = _allSymbols
            .Select((c, i) => (Char: c, Index: i))
            .ToFrozenDictionary(x => x.Char, x => (ulong)x.Index);

        private readonly IDictionary<char, char> _normalizeSymbols = "IiLlOo".Maketrans("111100");

        private readonly Regex _validSymbolsRegex = new Regex(
            string.Format(@"^[{0}]+[{1}]?$", Symbols, Regex.Escape(CheckSymbols)),
            RegexOptions.Compiled);

        /// <summary>
        /// Initializes a new instance of <see cref="Base32CrockfordEncoding"/>.
        /// </summary>
        public Base32CrockfordEncoding() { }

        /// <summary>
        /// Encode a number into a symbol string.
        /// </summary>
        /// <param name="number">Input number to encode.</param>
        /// <param name="checksum">The flag indicates to append checksum value to result string.</param>
        /// <param name="split">Size of characters group in result string split by hyphen.</param>
        /// <returns>An encoded symbol string.</returns>
        public string Encode(ulong number, bool checksum = false, byte split = 0)
        {
            char? checkSymbol = null;
            if (checksum)
            {
                checkSymbol = _encodeSymbols[number % _checkSymbolsLength];
            }

            if (number == 0)
            {
                return "0" + checkSymbol;
            }

            string result = string.Empty;
            while (number > 0)
            {
                ulong remainder = number % _symbolsLength;
                number = number / _symbolsLength;
                result = _encodeSymbols[remainder] + result;
            }
            result = result + checkSymbol;

            if (split > 0 && split < result.Length)
            {
                List<string> chunks = new List<string>();
                foreach (int start in result.Range(split))
                {
                    int length = result.Length - start;
                    int chunkLength = length > split ? split : length;
                    chunks.Add(result.Substring(start, chunkLength));
                }
                result = string.Join("-", chunks);
            }

            return result;
        }

        /// <summary>
        /// Decode an encoded symbol string to number.
        /// </summary>
        /// <param name="encodedString">Input encoded string to decode.</param>
        /// <param name="checksum">The flag indicates to validate trailing checksum symbol.</param>
        /// <param name="strict">The flag indicates that normalization step requires changes to the input string.</param>
        /// <returns>A decoded number.</returns>
        public ulong Decode(string encodedString, bool checksum = false, bool strict = false)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(encodedString, nameof(encodedString));

            encodedString = Normalize(encodedString, strict);

            char? checkSymbol = null;
            if (checksum)
            {
                checkSymbol = encodedString.Last();
                encodedString = encodedString.Substring(0, encodedString.Length - 1);
            }

            ulong result = 0;
            foreach (char symbol in encodedString)
            {
                result = result * _symbolsLength + _decodeSymbols[symbol];
            }

            if (checkSymbol != null)
            {
                ulong checkValue = _decodeSymbols[checkSymbol.Value];
                ulong checkRemainder = result % _checkSymbolsLength;
                if (checkValue != checkRemainder)
                {
                    throw new InvalidOperationException(
                        $"Invalid check symbol \"{checkSymbol}\" for string \"{encodedString}\".");
                }
            }

            return result;
        }

        private string Normalize(string encodedString, bool strict = false)
        {
            if (encodedString.All(char.IsAscii) == false)
            {
                throw new ArgumentException(
                    $"Argument \"{nameof(encodedString)}\" must contain only ASCII characters.");
            }

            string result = encodedString
                .Replace("-", "")
                .Translate(_normalizeSymbols)
                .ToUpper();

            if (_validSymbolsRegex.IsMatch(result) == false)
            {
                throw new InvalidOperationException(
                    $"Normalized string \"{result}\" contains invalid characters.");
            }

            if (strict && string.Equals(result, encodedString) == false)
            {
                throw new InvalidOperationException(
                    $"Encoded string \"{encodedString}\" requires normalization.");
            }

            return result;
        }
    }
}

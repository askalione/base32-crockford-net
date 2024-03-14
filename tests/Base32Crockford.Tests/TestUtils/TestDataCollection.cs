using System.Collections;

namespace Base32Crockford.Tests.TestUtils
{
    public class TestDataCollection : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new TestData { Number = 0, EncodedString = "0", CheckSymbol = '0' } };
            yield return new object[] { new TestData { Number = 1, EncodedString = "1", CheckSymbol = '1' } };
            yield return new object[] { new TestData { Number = 2, EncodedString = "2", CheckSymbol = '2' } };
            yield return new object[] { new TestData { Number = 3, EncodedString = "3", CheckSymbol = '3' } };
            yield return new object[] { new TestData { Number = 4, EncodedString = "4", CheckSymbol = '4' } };
            yield return new object[] { new TestData { Number = 5, EncodedString = "5", CheckSymbol = '5' } };
            yield return new object[] { new TestData { Number = 6, EncodedString = "6", CheckSymbol = '6' } };
            yield return new object[] { new TestData { Number = 7, EncodedString = "7", CheckSymbol = '7' } };
            yield return new object[] { new TestData { Number = 8, EncodedString = "8", CheckSymbol = '8' } };
            yield return new object[] { new TestData { Number = 9, EncodedString = "9", CheckSymbol = '9' } };
            yield return new object[] { new TestData { Number = 10, EncodedString = "A", CheckSymbol = 'A' } };
            yield return new object[] { new TestData { Number = 11, EncodedString = "B", CheckSymbol = 'B' } };
            yield return new object[] { new TestData { Number = 12, EncodedString = "C", CheckSymbol = 'C' } };
            yield return new object[] { new TestData { Number = 13, EncodedString = "D", CheckSymbol = 'D' } };
            yield return new object[] { new TestData { Number = 14, EncodedString = "E", CheckSymbol = 'E' } };
            yield return new object[] { new TestData { Number = 15, EncodedString = "F", CheckSymbol = 'F' } };
            yield return new object[] { new TestData { Number = 16, EncodedString = "G", CheckSymbol = 'G' } };
            yield return new object[] { new TestData { Number = 17, EncodedString = "H", CheckSymbol = 'H' } };
            yield return new object[] { new TestData { Number = 18, EncodedString = "J", CheckSymbol = 'J' } };
            yield return new object[] { new TestData { Number = 19, EncodedString = "K", CheckSymbol = 'K' } };
            yield return new object[] { new TestData { Number = 20, EncodedString = "M", CheckSymbol = 'M' } };
            yield return new object[] { new TestData { Number = 21, EncodedString = "N", CheckSymbol = 'N' } };
            yield return new object[] { new TestData { Number = 22, EncodedString = "P", CheckSymbol = 'P' } };
            yield return new object[] { new TestData { Number = 23, EncodedString = "Q", CheckSymbol = 'Q' } };
            yield return new object[] { new TestData { Number = 24, EncodedString = "R", CheckSymbol = 'R' } };
            yield return new object[] { new TestData { Number = 25, EncodedString = "S", CheckSymbol = 'S' } };
            yield return new object[] { new TestData { Number = 26, EncodedString = "T", CheckSymbol = 'T' } };
            yield return new object[] { new TestData { Number = 27, EncodedString = "V", CheckSymbol = 'V' } };
            yield return new object[] { new TestData { Number = 28, EncodedString = "W", CheckSymbol = 'W' } };
            yield return new object[] { new TestData { Number = 29, EncodedString = "X", CheckSymbol = 'X' } };
            yield return new object[] { new TestData { Number = 30, EncodedString = "Y", CheckSymbol = 'Y' } };
            yield return new object[] { new TestData { Number = 31, EncodedString = "Z", CheckSymbol = 'Z' } };
            yield return new object[] { new TestData { Number = 32, EncodedString = "10", CheckSymbol = '*' } };
            yield return new object[] { new TestData { Number = 33, EncodedString = "11", CheckSymbol = '~' } };
            yield return new object[] { new TestData { Number = 34, EncodedString = "12", CheckSymbol = '$' } };
            yield return new object[] { new TestData { Number = 35, EncodedString = "13", CheckSymbol = '=' } };
            yield return new object[] { new TestData { Number = 36, EncodedString = "14", CheckSymbol = 'U' } };
            yield return new object[] { new TestData { Number = 37, EncodedString = "15", CheckSymbol = '0' } };
            yield return new object[] { new TestData { Number = 64, EncodedString = "20", CheckSymbol = 'V' } };
            yield return new object[] { new TestData { Number = 586, EncodedString = "JA", CheckSymbol = 'Z' } };
            yield return new object[] { new TestData { Number = 1337, EncodedString = "19S", CheckSymbol = '5' } };
            yield return new object[] { new TestData { Number = 7392, EncodedString = "770", CheckSymbol = 'X' } };
            yield return new object[] { new TestData { Number = 65535, EncodedString = "1ZZZ", CheckSymbol = '8' } };
            yield return new object[] { new TestData { Number = 18446744073709551615, EncodedString = "FZZZZZZZZZZZZ", CheckSymbol = 'B' } };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

using Base32Crockford.Tests.TestUtils;

namespace Base32Crockford.Tests
{
    public class Base32CrockfordEncodingTests
    {
        [Theory]
        [ClassData(typeof(TestDataCollection))]
        public void Encode(TestData data)
        {
            // Arrange
            Base32CrockfordEncoding _encoding = new Base32CrockfordEncoding();
            ulong number = data.Number;

            // Act
            string encodedString = _encoding.Encode(number);

            // Assert
            encodedString.Should().BeEquivalentTo(data.EncodedString);
        }

        [Theory]
        [ClassData(typeof(TestDataCollection))]
        public void Encode_checksum(TestData data)
        {
            // Arrange
            Base32CrockfordEncoding _encoding = new Base32CrockfordEncoding();
            ulong number = data.Number;

            // Act
            string encodedString = _encoding.Encode(number, checksum: true);

            // Assert
            encodedString.Should().BeEquivalentTo(data.EncodedString + data.CheckSymbol);
        }

        [Theory]
        [ClassData(typeof(TestDataCollection))]
        public void Encode_split(TestData data)
        {
            // Arrange
            Base32CrockfordEncoding _encoding = new Base32CrockfordEncoding();
            ulong number = data.Number;
            byte split = 2;

            // Act
            string encodedString = _encoding.Encode(number, split: split);

            // Assert
            encodedString.Should().BeEquivalentTo(data.EncodedString.SplitChunks(split));
        }

        [Theory]
        [ClassData(typeof(TestDataCollection))]
        public void Encode_checksum_split(TestData data)
        {
            // Arrange
            Base32CrockfordEncoding _encoding = new Base32CrockfordEncoding();
            ulong number = data.Number;
            byte split = 3;

            // Act
            string encodedString = _encoding.Encode(number, checksum: true, split: split);

            // Assert
            encodedString.Should().BeEquivalentTo((data.EncodedString + data.CheckSymbol).SplitChunks(split));
        }

        [Theory]
        [ClassData(typeof(TestDataCollection))]
        public void Decode(TestData data)
        {
            // Arrange
            Base32CrockfordEncoding _encoding = new Base32CrockfordEncoding();
            string encodedString = data.EncodedString;

            // Act
            ulong number = _encoding.Decode(encodedString);

            // Assert
            number.Should().Be(data.Number);
        }

        [Theory]
        [ClassData(typeof(TestDataCollection))]
        public void Decode_checksum(TestData data)
        {
            // Arrange
            Base32CrockfordEncoding _encoding = new Base32CrockfordEncoding();
            string encodedString = data.EncodedString + data.CheckSymbol;

            // Act
            ulong number = _encoding.Decode(encodedString, checksum: true);

            // Assert
            number.Should().Be(data.Number);
        }

        [Fact]
        public void Decode_strict()
        {
            // Arrange
            Base32CrockfordEncoding _encoding = new Base32CrockfordEncoding();
            string encodedString = "LZZZ";

            // Act
            Action act = () => _encoding.Decode(encodedString, strict: true);

            // Assert
            act.Should().ThrowExactly<InvalidOperationException>();
        }

        [Fact]
        public void Decode_throws_when_invalid_checksum()
        {
            // Arrange
            Base32CrockfordEncoding _encoding = new Base32CrockfordEncoding();
            string encodedString = "1ZZZX";

            // Act
            Action act = () => _encoding.Decode(encodedString, checksum: true);

            // Assert
            act.Should().ThrowExactly<InvalidOperationException>();
        }

        [Fact]
        public void Decode_throws_when_invalid_character()
        {
            // Arrange
            Base32CrockfordEncoding _encoding = new Base32CrockfordEncoding();
            string encodedString = "1#ZZ";

            // Act
            Action act = () => _encoding.Decode(encodedString, strict: true);

            // Assert
            act.Should().ThrowExactly<InvalidOperationException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Decode_throws_when_empty_string(string encodedString)
        {
            // Arrange
            Base32CrockfordEncoding _encoding = new Base32CrockfordEncoding();

            // Act
            Action act = () => _encoding.Decode(encodedString);

            // Assert
            act.Should().Throw<ArgumentException>();
        }
    }
}

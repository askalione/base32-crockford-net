
using Base32Crockford;

Console.WriteLine(Base32CrockfordEncoding.Current.Encode(1337, checksum: true));
Console.WriteLine(Base32CrockfordEncoding.Current.Decode("19S5", checksum: true));

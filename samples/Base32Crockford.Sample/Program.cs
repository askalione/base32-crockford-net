using Base32Crockford;

// Encode
Console.WriteLine(
    Base32CrockfordEncoding.Current.Encode(1337));

// Decode
Console.WriteLine(
    Base32CrockfordEncoding.Current.Decode("19S5"));

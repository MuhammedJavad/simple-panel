using System.Security.Cryptography;

namespace Domain.Common.Types;

public class Cryptography
{
    public static byte[] ToRfc2898DeriveBytes(string input, ReadOnlySpan<byte> salt, int iteration, int outputLength)
    {
        return Rfc2898DeriveBytes.Pbkdf2(
            input,
            salt,
            iteration,
            HashAlgorithmName.SHA1,
            outputLength);
    }
}
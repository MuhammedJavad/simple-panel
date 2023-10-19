using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Domain.Common.Extensions;

namespace Domain.Common.Types;

public class PasswordManager
{
    private const int Pbkdf2IterCount = 1000; // default for Rfc2898DeriveBytes
    private const int Pbkdf2SubkeyLength = 256 / 8; // 256 bits
    private const int SaltSize = 128 / 8; // 128 bits

    public static string HashPassword(string password)
    {
        ArgumentException.ThrowIfNullOrEmpty(password, nameof(password));

        Span<byte> salt = RandomNumberGenerator.GetBytes(SaltSize);

        var buff = Cryptography.ToRfc2898DeriveBytes(
            password,
            salt,
            Pbkdf2IterCount,
            Pbkdf2SubkeyLength);

        return Convert.ToBase64String(buff);
    }

    public static bool VerifyHashedPassword(string hashedPassword, string password)
    {
        ArgumentException.ThrowIfNullOrEmpty(hashedPassword, nameof(hashedPassword));
        ArgumentException.ThrowIfNullOrEmpty(password, nameof(password));

        var hashedPasswordBytes = Convert.FromBase64String(hashedPassword);

        // Verify a version 0 (see comment above) text hash.

        if (hashedPasswordBytes.Length != (1 + SaltSize + Pbkdf2SubkeyLength) || hashedPasswordBytes[0] != 0x00)
        {
            // Wrong length or version header.
            return false;
        }

        var salt = new byte[SaltSize];
        Buffer.BlockCopy(hashedPasswordBytes, 1, salt, 0, SaltSize);
        var storedSubkey = new byte[Pbkdf2SubkeyLength];
        Buffer.BlockCopy(hashedPasswordBytes, 1 + SaltSize, storedSubkey, 0, Pbkdf2SubkeyLength);

        var generatedSubkey = Cryptography.ToRfc2898DeriveBytes(
            password,
            salt,
            Pbkdf2IterCount,
            Pbkdf2SubkeyLength);

        return ByteArraysEqual(storedSubkey, generatedSubkey);
    }

    // Compares two byte arrays for equality. The method is specifically written so that the loop is not optimized.
    [MethodImpl(MethodImplOptions.NoOptimization)]
    private static bool ByteArraysEqual(byte[]? a, byte[]? b)
    {
        if (ReferenceEquals(a, b))
        {
            return true;
        }

        if (a == null || b == null || a.Length != b.Length)
        {
            return false;
        }

        var areSame = true;
        for (var i = 0; i < a.Length; i++)
        {
            areSame &= (a[i] == b[i]);
        }

        return areSame;
    }
}
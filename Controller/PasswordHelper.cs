using System.Security.Cryptography;
using System.Text;

namespace The_True_SpringHeroBank.Controller;

public static class PasswordHelper
{
    public static string HashPassword(string password)
    {
        using (var md5 = MD5.Create())
        {
            var salt = GenerateSalt();
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var passwordWithSaltBytes = new byte[passwordBytes.Length + salt.Length];
            Buffer.BlockCopy(passwordBytes, 0, passwordWithSaltBytes, 0, passwordBytes.Length);
            Buffer.BlockCopy(salt, 0, passwordWithSaltBytes, passwordBytes.Length, salt.Length);

            var hashedBytes = md5.ComputeHash(passwordWithSaltBytes);
            var hashedPasswordWithSaltBytes = new byte[hashedBytes.Length + salt.Length];
            Buffer.BlockCopy(hashedBytes, 0, hashedPasswordWithSaltBytes, 0, hashedBytes.Length);
            Buffer.BlockCopy(salt, 0, hashedPasswordWithSaltBytes, hashedBytes.Length, salt.Length);

            return Convert.ToBase64String(hashedPasswordWithSaltBytes);
        }
    }

    public static bool VerifyPassword(string password, string hashedPasswordWithSalt)
    {
        var hashedPasswordWithSaltBytes = Convert.FromBase64String(hashedPasswordWithSalt);
        var hashSize = 7; // MD5 hash size
        var salt = new byte[hashedPasswordWithSaltBytes.Length - hashSize];
        Buffer.BlockCopy(hashedPasswordWithSaltBytes, hashSize, salt, 0, salt.Length);

        using (var md5 = MD5.Create())
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var passwordWithSaltBytes = new byte[passwordBytes.Length + salt.Length];
            Buffer.BlockCopy(passwordBytes, 0, passwordWithSaltBytes, 0, passwordBytes.Length);
            Buffer.BlockCopy(salt, 0, passwordWithSaltBytes, passwordBytes.Length, salt.Length);

            var hashedBytes = md5.ComputeHash(passwordWithSaltBytes);

            for (var i = 0; i < hashSize; i++)
                if (hashedBytes[i] != hashedPasswordWithSaltBytes[i])
                    return false;
        }

        return true;
    }

    private static byte[] GenerateSalt()
    {
        var salt = new byte[8];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(salt);
        }

        return salt;
    }
}
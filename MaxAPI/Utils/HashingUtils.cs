using Konscious.Security.Cryptography;
using MaxAPI.Models;
using MaxAPI.Models.Accounts;
using System.Security.Cryptography;
using System.Text;

namespace MaxAPI.Utils
{
    public class HashingUtils
    {
        /// <summary>
        /// Creates random salt
        /// </summary>
        /// <param name="size">Salt size</param>
        /// <returns>Salt</returns>
        public static byte[] CreateSalt(int size)
        {
            return RandomNumberGenerator.GetBytes(size);
        }

        /// <summary>
        /// Creates argon2 hash using <paramref name="password"/> and <paramref name="salt"/>
        /// </summary>
        /// <param name="password">Password</param>
        /// <param name="salt">Salt</param>
        /// <returns>Hashed input</returns>
        public static byte[] GetArgon2Hash(byte[] password, byte[] salt)
        {
            var argon2 = new Argon2id(password)
            {
                DegreeOfParallelism = 16,
                MemorySize = 8192,
                Iterations = 40,
                Salt = salt
            };

            return argon2.GetBytes(128);
        }

        /// <summary>
        /// <para>Creates argon2 hash using <paramref name="password"/> and <paramref name="salt"/></para>
        /// <para>Converts password to byte array using UTF8 encoding</para>
        /// </summary>
        /// <param name="password">Password</param>
        /// <param name="salt">Salt</param>
        /// <returns>Hashed input</returns>
        public static byte[] GetArgon2Hash(string password, byte[] salt)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var argon2 = new Argon2id(passwordBytes)
            {
                DegreeOfParallelism = 16,
                MemorySize = 8192,
                Iterations = 40,
                Salt = salt
            };

            return argon2.GetBytes(128);
        }



        /// <summary>
        /// Verifies if <paramref name="password"/> matches <see cref="User.Password"/> using argon2
        /// </summary>
        /// <param name="password">Password</param>
        /// <param name="user">User</param>
        /// <returns>Bool on whether <paramref name="password"/> matches <see cref="User.Password"/></returns>
        public static bool VerifyArgon2(string password, User user)
        {
            if (user.Salt is null || user.Password is null) return false;

            return Enumerable.SequenceEqual(GetArgon2Hash(Encoding.UTF8.GetBytes(password), user.Salt), user.Password);
        }
    }
}

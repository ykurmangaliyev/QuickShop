using System;
using System.Security.Cryptography;
using System.Text;

namespace QuickShop.Domain.Accounts.Authentication.HashingAlgorithm
{
    public class Sha512HashingAlgorithm : IHashingAlgorithm
    {
        public string Hash(string digest)
        {
            if (String.IsNullOrWhiteSpace(digest))
                throw new ArgumentException("Digest should be non-null non-whitespace string");

            using (SHA512 sha = SHA512.Create())
            {
                byte[] digestBytes = Encoding.Unicode.GetBytes(digest);
                byte[] resultBytes = sha.ComputeHash(digestBytes);

                return Convert.ToBase64String(resultBytes);
            }
        }
    }
}
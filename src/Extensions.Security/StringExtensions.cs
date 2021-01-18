using System;

namespace QuickShop.Extensions.Security
{
    public static class StringExtensions
    {
        public static bool TimeConstantEquals(this string a, string b)
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a));

            if (b == null)
                throw new ArgumentNullException(nameof(b));

            uint diff = (uint)a.Length ^ (uint)b.Length;

            for (int i = 0; i < a.Length && i < b.Length; i++)
                diff |= (uint)(a[i] ^ b[i]);

            return diff == 0;
        }
    }
}

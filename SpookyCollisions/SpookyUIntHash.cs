using SpookilySharp;

namespace SpookyCollisions
{
    public static class SpookyUIntHash
    {
        public const ulong SpookyConst = 0xDEADBEEFDEADBEEF;
        public static void Hash(uint n, ref ulong hash1, ref ulong hash2) {
            ulong a = hash1;
            ulong b = hash2;
            ulong c = SpookyConst;
            ulong d = SpookyConst;

            d += (ulong)4 << 56;
            c += n;

            d ^= c;
            c = c << 15 | c >> -15;
            d += c;
            a ^= d;
            d = d << 52 | d >> -52;
            a += d;
            b ^= a;
            a = a << 26 | a >> -26;
            b += a;
            c ^= b;
            b = b << 51 | b >> -51;
            c += b;
            d ^= c;
            c = c << 28 | c >> -28;
            d += c;
            a ^= d;
            d = d << 9 | d >> -9;
            a += d;
            b ^= a;
            a = a << 47 | a >> -47;
            b += a;
            c ^= b;
            b = b << 54 | b >> -54;
            c += b;
            d ^= c;
            c = c << 32 | c >> -32;
            d += c;
            a ^= d;
            d = d << 25 | d >> -25;
            a += d;
            b ^= a;
            a = a << 63 | a >> -63;
            b += a;

            hash2 = b;
            hash1 = a;
        }

        public static HashCode128 Hash(uint n) {
            ulong hash1 = SpookyConst;
            ulong hash2 = SpookyConst;
            Hash(n, ref hash1, ref hash2);
            return new HashCode128(hash1, hash2);
        }
    }
}

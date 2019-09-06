using System.Runtime.CompilerServices;
using SpookilySharp;

namespace SpookyCollisions
{
    public static class SpookyUIntHash
    {
        public const ulong SpookyConst = 0xDEADBEEFDEADBEEF;

        public static ulong Rot64(ulong n, int d) {
            return n << d | n >> -d;
        }

        public static ulong UnRot64(ulong n, int d) {
            return Rot64(n, 64 - d);
        }

        public static (ulong, ulong) Scramble(ulong x, ulong y, int shift) {
            ulong ny = Rot64(y, shift);
            ulong nx = (x ^ y) + ny;
            return (nx, ny);
        }

        public static (ulong, ulong) UnScramble(ulong nx, ulong ny, int shift) {
            ulong y = UnRot64(ny, shift);
            ulong x = (nx - ny) ^ y;
            return (x, y);
        }

        public static void Hash(uint n, ref ulong hash1, ref ulong hash2) {
            ulong a = hash1;
            ulong b = hash2;
            ulong c = SpookyConst + n;
            ulong d = SpookyConst + (4ul << 56);

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

        /// <summary>
        /// This hash function is slightly quicker than the one above because we skip the last
        /// operation. Scramble is invertible and we'll only get a collision in the last step
        /// if the values were already colliding before.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="hash1"></param>
        /// <param name="hash2"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void QuickHash(uint n, ref ulong hash1, ref ulong hash2) {
            ulong a = hash1;
            ulong b = hash2;
            ulong c = SpookyConst + n;
            ulong d = SpookyConst + (4ul << 56);

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

            hash2 = b;
            hash1 = a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static HashCode128 QuickHash(uint n) {
            ulong hash1 = SpookyConst;
            ulong hash2 = SpookyConst;
            QuickHash(n, ref hash1, ref hash2);
            return new HashCode128(hash1, hash2);
        }

        public static HashCode128 Hash(uint n) {
            ulong hash1 = SpookyConst;
            ulong hash2 = SpookyConst;
            Hash(n, ref hash1, ref hash2);
            return new HashCode128(hash1, hash2);
        }
    }
}

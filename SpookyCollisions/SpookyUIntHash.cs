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
            ulong a0 = hash1;
            ulong b0 = hash2;
            ulong c0 = SpookyConst + n;
            ulong d0 = SpookyConst + (4ul << 56);

            var (d1, c1) = Scramble(d0, c0, 15);
            var (a1, d2) = Scramble(a0, d1, 52);
            var (b1, a2) = Scramble(b0, a1, 26);
            var (c2, b2) = Scramble(c1, b1, 51);
            var (d3, c3) = Scramble(d2, c2, 28);
            var (a3, d4) = Scramble(a2, d3, 9);
            var (b3, a4) = Scramble(b2, a3, 47);
            var (c4, b4) = Scramble(c3, b3, 54);
            var (d5, c5) = Scramble(d4, c4, 32);
            var (a5, d6) = Scramble(a4, d5, 25);
            var (b5, a6) = Scramble(b4, a5, 63);

            hash2 = b5;
            hash1 = a6;
        }

        /// <summary>
        /// This hash function is slightly quicker than the one above because we skip the last
        /// operation. Scramble is invertible and we'll only get a collision in the last step
        /// if the values were already colliding before.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="hash1"></param>
        /// <param name="hash2"></param>
        public static void QuickHash(uint n, ref ulong hash1, ref ulong hash2) {
            ulong a0 = hash1;
            ulong b0 = hash2;
            ulong c0 = SpookyConst + n;
            ulong d0 = SpookyConst + (4ul << 56);

            var (d1, c1) = Scramble(d0, c0, 15);
            var (a1, d2) = Scramble(a0, d1, 52);
            var (b1, a2) = Scramble(b0, a1, 26);
            var (c2, b2) = Scramble(c1, b1, 51);
            var (d3, c3) = Scramble(d2, c2, 28);
            var (a3, d4) = Scramble(a2, d3, 9);
            var (b3, a4) = Scramble(b2, a3, 47);
            var (c4, b4) = Scramble(c3, b3, 54);
            var (d5, c5) = Scramble(d4, c4, 32);
            var (a5, d6) = Scramble(a4, d5, 25);

            hash2 = b4;
            hash1 = a5;
        }

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

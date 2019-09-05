using System;
using SpookilySharp;

namespace SpookyCollisions
{
    class Program
    {
        const ulong SpookyConst = 0xDEADBEEFDEADBEEF;

        static void Main(string[] args)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            uint even = 0;
            for (ulong c = 0; c <= uint.MaxValue; c++) {
                var result = Hash((uint)c);
                even += (uint)(result.UHash2 & 0x1);
            }
            watch.Stop();

            Console.WriteLine($"Result: {even} evens (of {uint.MaxValue}), that's {100 * (even/(double)uint.MaxValue)}%");
            Console.WriteLine($"Took {watch.ElapsedMilliseconds}ms\n{1000 * (uint.MaxValue / ((double)watch.ElapsedMilliseconds))} H/s");
        }

        private static void Hash(uint n, ref ulong hash1, ref ulong hash2) {
            unsafe {
                SpookyHash.Hash128(&n, sizeof(uint), ref hash1, ref hash2);
            }
        }

        private static HashCode128 Hash(uint n) {
            unsafe {
                ulong hash1 = SpookyConst;
                ulong hash2 = SpookyConst;
                Hash(n, ref hash1, ref hash2);
                return new HashCode128(hash1, hash2);
            }
        }
    }
}

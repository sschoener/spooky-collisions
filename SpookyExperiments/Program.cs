using System;

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
                var result = SpookyUIntHash.Hash((uint)c);
                even += (uint)(result.UHash2 & 0x1);
            }
            watch.Stop();

            Console.WriteLine($"Result: {even} evens (of {uint.MaxValue}), that's {100 * (even/(double)uint.MaxValue)}%");
            Console.WriteLine($"Took {watch.ElapsedMilliseconds}ms\n{1000 * (uint.MaxValue / ((double)watch.ElapsedMilliseconds))} H/s");
        }
    }
}

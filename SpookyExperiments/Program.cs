using System;
using System.Collections.Generic;
using System.Linq;
using SpookilySharp;

namespace SpookyCollisions
{
    class Program
    {      
        const int BucketBits = 8;
        const ulong ExpectedBucketSize = 0x1ul << (32 - BucketBits);
        const ulong NumBuckets = 0x1ul << (BucketBits);
        const ulong BucketMask = ~0x0ul >> (64 - BucketBits);
        const ulong NumHashesComputed = NumBuckets << 32;

        private static IEnumerable<(uint,uint)> FindCollisions(ulong bucket)
        {
            var elements = new List<(HashCode128, uint)>((int)ExpectedBucketSize);
            for (ulong v = 0; v <= uint.MaxValue; v++) {
                uint n = (uint)v;
                var hashCode = SpookyUIntHash.QuickHash(n);
                if ((hashCode.UHash1 & BucketMask) == bucket) {
                    elements.Add((hashCode, n));
                }
            }

            var hashes =  new Dictionary<HashCode128, uint>((int)ExpectedBucketSize);
            foreach (var tpl in elements) {
                if (!hashes.TryAdd(tpl.Item1, tpl.Item2)) {
                    yield return (tpl.Item2, hashes[tpl.Item1]);
                }
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine($"Bucket bits: {BucketBits}, num buckets: {NumBuckets}");
            float bucketSizeMB = 4 * ExpectedBucketSize / (float) (0x1u << 20);
            Console.WriteLine($"Expected bucket size: {ExpectedBucketSize} elements, {bucketSizeMB}mb");

            Console.WriteLine("Collisions: ");

            var watch = System.Diagnostics.Stopwatch.StartNew();
            foreach (var tuple in Enumerable.Range(0, (int) NumBuckets).AsParallel()
                                    .SelectMany(b => FindCollisions((ulong)b)) {
                Console.WriteLine(tuple);
            }
            watch.Stop();
            
            Console.WriteLine($"Took {watch.ElapsedMilliseconds}ms");
            double hashesPerSecond = NumHashesComputed / ((double)watch.ElapsedMilliseconds * 1E-3);
            Console.WriteLine($"About {hashesPerSecond} H/s");
        }
    }
}

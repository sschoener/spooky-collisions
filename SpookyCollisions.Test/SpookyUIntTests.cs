using NUnit.Framework;
using SpookilySharp;

namespace SpookyCollisions.Test
{
    public class Tests
    {
        [Test]
        public void HashIdentical()
        {
            for (int i = 0; i < 1000000; i++) {
                uint n = TestContext.CurrentContext.Random.NextUInt();
                Assert.AreEqual(SpookyUIntHash.Hash(n), HashOrig(n));
            }
        }

        private static void HashOrig(uint n, ref ulong hash1, ref ulong hash2) {
            unsafe {
                SpookyHash.Hash128(&n, sizeof(uint), ref hash1, ref hash2);
            }
        }

        private static HashCode128 HashOrig(uint n) {
            unsafe {
                ulong hash1 = SpookyUIntHash.SpookyConst;
                ulong hash2 = SpookyUIntHash.SpookyConst;
                HashOrig(n, ref hash1, ref hash2);
                return new HashCode128(hash1, hash2);
            }
        }
    }
}
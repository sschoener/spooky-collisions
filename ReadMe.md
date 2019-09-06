# Spooky Collisions
This repositoy contains a little script to check that the [SpookyHash](http://www.burtleburtle.net/bob/hash/spooky.html) function does not have any collisions for 32 bit inputs.
The entry point is in `SpookyExperiments`. I have simplified the hashing function by specialising it for inputs of 4 bytes in size (`SpookyCollisions/SpookyUIntHash.cs`) and removed a few redundant operations.

The general idea does not make any use of potential properties of the function itself and will work with any function (which means that there is probably a better way to answer this question). The algorithm works by bucketing the different hash values first and then detects collisions within buckets. Specifically:
 1. Select a number of bits `K`.
 2. Each `K` bit value is a bucket. For each bucket `b`, go over all 32bit values and compute their hashes. If the first `K` bits of the hash match the bits of `b`, record its hash value and check that we have not seen this hash value before (or record a collision).

By varying `K` you can choose a trade-off between having to calculate all hashes multiple times and limiting the memory requirements to do so. The program relies on the expectation that the lower `K` bits of the hash values are approximately uniformly distributed, so all buckets will have a similar size. For example, `K = 8` means 256 buckets with an expected size of 2^24 entries. On my machine, this produces the answer that there are no collisions in about 10 minutes.
using System;
using System.Threading;

namespace Util
{
    public static class ThreadLocalRandom
    {
        private static int _seed = Environment.TickCount;
     
        private static readonly ThreadLocal<Random> RandomWrapper = new ThreadLocal<Random>(() =>
            new Random(Interlocked.Increment(ref _seed))
        );

        public static Random Current()
        {
            return RandomWrapper.Value;
        }
    }
}
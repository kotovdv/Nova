using System;

public class ThreadLocalRandom
{
    private static readonly Random Global = new Random();

    [ThreadStatic]
    private static Random _local;

    public static Random Current()
    {
        var inst = _local;
        if (inst == null)
        {
            int seed;
            lock (Global) seed = Global.Next();
            _local = inst = new Random(seed);
        }

        return inst;
    }
}
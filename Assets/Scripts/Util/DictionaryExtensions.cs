using System;
using System.Collections.Generic;

public static class DictionaryExtensions
{
    public static TV GetOrDefault<TK, TV>(this IDictionary<TK, TV> dict, TK key, TV defaultValue = default(TV))
    {
        return dict.TryGetValue(key, out var value)
            ? value
            : defaultValue;
    }

    public static TV GetOrCompute<TK, TV>(this IDictionary<TK, TV> dict, TK key, Func<TV> creator)
    {
        dict.TryGetValue(key, out var value);
        if (value != null) return value;

        value = creator.Invoke();
        dict.Add(key, value);

        return value;
    }
}
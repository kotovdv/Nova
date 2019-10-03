using System.Collections.Generic;

namespace Core.Util
{
    public static class DictionaryExtensions
    {
        public static TV GetOrDefault<TK, TV>(this IDictionary<TK, TV> dict, TK key, TV defaultValue = default)
        {
            return dict.TryGetValue(key, out var value)
                ? value
                : defaultValue;
        }
    }
}
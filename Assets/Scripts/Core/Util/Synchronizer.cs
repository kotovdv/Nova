using System.Collections.Generic;

namespace Core.Util
{
    public class Synchronizer<T> where T : struct
    {
        private readonly object _commonLock = new object();
        private readonly Dictionary<T, object> _locks = new Dictionary<T, object>();

        public object this[T index]
        {
            get
            {
                lock (_commonLock)
                {
                    if (_locks.TryGetValue(index, out var result))
                        return result;

                    result = new object();
                    _locks[index] = result;
                    return result;
                }
            }
        }
    }
}
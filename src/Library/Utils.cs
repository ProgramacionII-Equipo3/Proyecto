using System;
using System.Collections.Generic;

namespace Library
{
    public static class Utils
    {
        public static Dictionary<TKey, TValue> DictionaryFromList<TKey, TValue>((TKey, TValue)[] list)
        {
            Dictionary<TKey, TValue> r = new Dictionary<TKey, TValue>();
            foreach(var (key, value) in list)
            {
                r.Add(key, value);
            }
            return r;
        }
    }
}
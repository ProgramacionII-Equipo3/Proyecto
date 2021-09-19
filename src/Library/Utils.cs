using System;
using System.Collections.Generic;

namespace Library
{
    public static class Utils
    {
        public static ConsoleText ToConsole(
            this string str,
            ConsoleColor? backgroundColor = null,
            ConsoleColor? foregroundColor = null
        ) =>
            ConsoleText.FromStrings((str, null, null));

        public static Dictionary<TKey, TValue> DictionaryFromList<TKey, TValue>((TKey, TValue)[] list)
        {
            Dictionary<TKey, TValue> r = new Dictionary<TKey, TValue>();
            foreach(var (key, value) in list)
            {
                r.Add(key, value);
            }
            return r;
        }

        public static string CheckStrings(params (string, string)[] argNames)
        {
            List<string> r = new List<string>();
            foreach(var (name, value) in argNames)
            {
                if(string.IsNullOrWhiteSpace(value))
                    r.Add(name);
            }
            if(r.Count == 0) return null;

            return "The following required arguments are missing: " + string.Join(", ", r);
        }
    }
}

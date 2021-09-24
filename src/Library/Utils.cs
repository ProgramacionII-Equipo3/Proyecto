using System;
using System.Collections.Generic;

namespace Library
{
    public static class Utils
    {
        /// <summary>
        /// Converts a string into a <see cref="Library.ConsoleText">ConsoleText</see>.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <param name="backgroundColor">The background color of the string. null by default.</param>
        /// <param name="foregroundColor">The foreground color of the string. null by default.</param>
        /// <returns></returns>
        public static ConsoleText ToConsole(
            this string str,
            ConsoleColor? backgroundColor = null,
            ConsoleColor? foregroundColor = null
        ) => ConsoleText.FromStrings((str, null, null));

        /// <summary>
        /// Transforms a list of key-value tuples into a dictionary.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <typeparam name="TKey">The type of the keys</typeparam>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <returns>A dictionary with the same data as the list (for repeated keys, the last value is taken into account).</returns>
        public static Dictionary<TKey, TValue> DictionaryFromList<TKey, TValue>((TKey, TValue)[] list)
        {
            Dictionary<TKey, TValue> r = new Dictionary<TKey, TValue>();
            foreach(var (key, value) in list)
                r.Add(key, value);
            return r;
        }

        /// <summary>
        /// Checks a list of strings, and returns an error string if at least one of them is missing.
        /// </summary>
        /// <param name="args">The list of tuples, in which the first item is the name of the argument and the second one is the value.</param>
        /// <returns>An error string for the arguments, or null if they are all ok.</returns>
        public static string CheckStrings(params (string, string)[] args)
        {
            List<string> r = new List<string>();
            foreach(var (name, value) in args)
                if(string.IsNullOrWhiteSpace(value))
                    r.Add(name);
            if(r.Count == 0) return null;
            return "The following required arguments are missing: " + string.Join(", ", r);
        }
    }
}

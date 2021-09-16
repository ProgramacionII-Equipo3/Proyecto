using System;
using System.Collections.Generic;

namespace Library.Tests
{
    /// <summary>
    /// This class represents a collection of characters coming from both input and output, which can be presented together
    /// </summary>
    public class IOStringBuilder
    {
        /// <summary>
        /// The list of characters, consecutively grouped, by whether they come from input or from output, into strings.
        /// The associated bool is true if they come from input, false otherwise.
        /// </summary>
        public List<(string, bool)> Text { get; private set; } = new List<(string, bool)>();

        /// <summary>
        /// Adds a char to the list.
        /// </summary>
        /// <param name="c">The char.</param>
        /// <param name="fromInput">Whether it comes from input or not.</param>
        private void addChar(char c, bool fromInput)
        {
            if(c == '\r') return;
            
            if(Text.Count == 0)
            {
                Text.Add((c.ToString(), fromInput));
                return;
            }

            if(Text[^1].Item2 == fromInput)
                Text[^1] = (Text[^1].Item1 + c, fromInput);
            else
                Text.Add((c.ToString(), fromInput));
        }

        /// <summary>
        /// Adds an output char to the list.
        /// </summary>
        /// <param name="c">The output char.</param>
        public void AddOutputChar(char c) =>
            addChar(c, false);

        /// <summary>
        /// Adds an input char to the list.
        /// </summary>
        /// <param name="c">The input char.</param>
        public void AddInputChar(char c) =>
            addChar(c, true);

        /// <summary>
        /// Prints the received text into the console, with input text in a different color.
        /// </summary>
        public void Print(ConsoleColor color)
        {
            foreach(var (s, i) in Text)
            {
                Console.ForegroundColor = i ? color : ConsoleColor.White;
                Console.Write(s);
            }
            Console.ResetColor();
            Console.WriteLine();
        }

    }
}
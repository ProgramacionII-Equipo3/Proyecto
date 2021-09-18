/*
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Library
{
    /// <summary>
    /// This struct represents a colored string for the console.
    /// </summary>
    public struct ConsoleTextElement
    {
        /// <summary>
        /// The text to be printed.
        /// </summary>
        public string Text;

        /// <summary>
        /// The background color of the text.
        /// </summary>
        public ConsoleColor? BackgroundColor;

        /// <summary>
        /// The foreground color of the text.
        /// </summary>
        public ConsoleColor? ForegroundColor;

        public ConsoleTextElement(string text, ConsoleColor? backgroundColor, ConsoleColor? foregroundColor)
        {
            this.Text = text;
            this.BackgroundColor = backgroundColor;
            this.ForegroundColor = foregroundColor;
        }
    }

    /// <summary>
    /// This class represents a text which is meant to be printed into console,
    /// allowing for several colors, both in background and in foreground.
    /// </summary>
    public class ConsoleText
    {
        /// <summary>
        /// The list of strings to print into console.
        /// </summary>
        private readonly List<ConsoleTextElement> _strings = new List<ConsoleTextElement>();

        /// <summary>
        /// A public readonly wrapper of the colored strings.
        /// </summary>
        public ReadOnlyCollection<ConsoleTextElement> Strings
        {
            get => _strings.AsReadOnly();
        }

        /// <summary>
        /// Adds a new colored string to the text.
        /// </summary>
        /// <param name="s">The new string</param>
        /// <param name="backgroundColor"></param>
        /// <param name="foregroundColor"></param>
        public void AddString(string s, ConsoleColor? backgroundColor = null, ConsoleColor? foregroundColor = null)
        {
            if(string.IsNullOrEmpty(s)) return;

            if(
                _strings.Count > 0
             && _strings[^1].BackgroundColor == backgroundColor
             && _strings[^1].ForegroundColor == foregroundColor
            )
            {
                _strings[^1] = new ConsoleTextElement(_strings[^1].Text + s, backgroundColor, foregroundColor);
                return;
            }

            _strings.Add(new ConsoleTextElement(s, backgroundColor, foregroundColor));
        }

        public void ConsolePrint()
        {
            foreach(ConsoleTextElement textEl in _strings)
            {
                Console.ResetColor();
                if(textEl.BackgroundColor is ConsoleColor background)
                    Console.BackgroundColor = background;
                if(textEl.ForegroundColor is ConsoleColor foreground)
                    Console.ForegroundColor = foreground;
                Console.Write(textEl.Text);
            }
            Console.ResetColor();
        }

        public static ConsoleText FromStrings(params (string, ConsoleColor?, ConsoleColor?)[] data)
        {
            ConsoleText r = new ConsoleText();
            foreach(var (s, back, fore) in data)
            {
                r.AddString(s, back, fore);
            }
        }
    }
}
*/
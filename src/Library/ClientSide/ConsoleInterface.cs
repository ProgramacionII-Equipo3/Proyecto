using Library.ServerSide;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.ClientSide
{
    /// <summary>
    /// This class represents a client interface which works through text flows.
    /// </summary>
    public class ConsoleInterface : IClientInterface
    {
        /// <summary>
        /// The foreground color of the input fields
        /// </summary>
        private const ConsoleColor INPUT_FOREGROUND = ConsoleColor.Red;

        /// <summary>
        /// This attribute stores the chain of operations the program's state is currently in.
        /// Its main purpose is to remind the user, in case they forget what they were doing.
        /// </summary>
        private List<string> stateStack = new List<string>();

        /// <summary>
        /// The stack state of the program, prepared to be printed into console.
        /// </summary>
        private ConsoleText stateString
        {
            get => ConsoleText.FromStrings((string.Join(" >> ", this.stateStack), ConsoleColor.White, ConsoleColor.Black));
        }
        
        /// <summary>
        /// Returns input from the user after showing a message.
        /// Together with GetChar(string), they are the only functions through which the object will interact with the console.
        /// Does not add line break to message.
        /// </summary>
        /// <param name="prevText">The message to show before asking for input. It can be very long.</param>
        /// <returns>The input given by the user.</returns>
        private string GetInput(ConsoleText prevText)
        {
            Console.Clear();
            stateString.ConsolePrint();
            Console.WriteLine();
            prevText.ConsolePrint();
            Console.ForegroundColor = INPUT_FOREGROUND;
            string r =  Console.ReadLine();
            Console.ResetColor();
            return r;
        }

        /// <summary>
        /// Returns a character from the user after showing a message.
        /// Together with GetInput(string), they are the only functions through which the object will interact with the console.
        /// Does not add line break to message.
        /// </summary>
        /// <param name="prevText">The message to show before asking for input. It can be very long.</param>
        /// <returns>The character given by the user.</returns>
        private char GetChar(ConsoleText prevText)
        {
            Console.Clear();
            stateString.ConsolePrint();
            Console.WriteLine();
            prevText.ConsolePrint();
            Console.ForegroundColor = INPUT_FOREGROUND;
            char r = (char)Console.Read();
            Console.ResetColor();
            return r;
        }

        /// <summary>
        /// Asks the user if they want to retry an operation after failing before.
        /// </summary>
        /// <returns>Whether the user wants to retry</returns>
        private bool TryAgain(string msg)
        {
            char r = GetChar(
                ConsoleText.FromStrings(
                    (msg,                                       null, null),
                    ("\nPress \"q\" to quit",                   null, null),
                    ("\nPress any other key to try again...\n", null, null)
                )
            );
            return !("qQ\uffff".Contains(r));
        }

        /// <summary>
        /// Repeatedly attempts to retrieve data specified by the function until it succeeds or the user doesn't want to try again.
        /// </summary>
        /// <param name="func">The function to execute to retrieve the data, or the error to print before asking to try again.</param>
        /// <typeparam name="T">The type of the data to retrieve.</typeparam>
        /// <returns>The data retrieved by func, if it did.</returns>
        private T TryUntilValid<T>(Func<(T, string)> func)
        {
            while(true)
            {
                var (r, msg) = func();
                if(r is T result) return result;
                if(msg != null && !TryAgain(msg))
                    return default(T);
            }
        }

        private Dictionary<string, string> GetFormInput(ConsoleText prevText, params string[] argNames) =>
            GetFormInput(prevText, argNames.Aggregate(
                new List<string>(),
                (list, v) =>
                {
                    if(!list.Contains(v)) list.Add(v);
                    return list;
                }
            ).Select<string, (string, string)>(name => (name, "")).ToArray());

        private Dictionary<string, string> GetFormInput(ConsoleText prevText, params (string, string)[] args)
        {
            int state = 0;
            bool writing = false;
            while(true)
            {
                Console.Clear();
                stateString.ConsolePrint();
                Console.WriteLine();
                prevText.ConsolePrint();
                Console.WriteLine();
                Console.WriteLine();
                foreach(var (name, value, i) in args.Select((data, i) => (data.Item1, data.Item2, i)))
                {
                    Console.BackgroundColor = i == state ? ConsoleColor.Cyan : ConsoleColor.Black;
                    Console.ForegroundColor = i == state ? ConsoleColor.Black : ConsoleColor.Cyan;
                    Console.WriteLine($"name: {value}");
                }
                Console.ResetColor();
                Console.WriteLine();
                Console.Write(args[state].Item1);
                Console.Write(": ");
                if(writing)
                {
                    args[state].Item2 = Console.ReadLine();
                    writing = false;
                } else
                {
                    Console.WriteLine(args[state].Item2);
                    while(true)
                    {
                        int c = Console.Read();
                        if(c == 0x26 && state > 0)
                        { // UP
                            state--;
                            continue;
                        }
                        else if(c == 0x28 && state < args.Length - 1)
                        { // DOWN
                            state++;
                            continue;
                        }
                        else if(c == 0x27)
                        { // RIGHT
                            writing = true;
                            continue;
                        }
                        else if(c == 0x0D)
                        {
                            return Utils.DictionaryFromList(args);
                        }
                    }
                }
            }
        }

        User IClientInterface.SignIn(IDatabaseConnection conn);
    }
}
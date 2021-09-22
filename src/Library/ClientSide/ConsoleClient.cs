using Library.ServerSide;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.ClientSide
{
    /// <summary>
    /// This class represents a client interface which works through text flows.
    /// </summary>
    public class ConsoleClient : IClient
    {
        /// <summary>
        /// The foreground color of the input fields
        /// </summary>
        private const ConsoleColor INPUT_FOREGROUND = ConsoleColor.Red;

        /// <summary>
        /// This attribute stores the chain of operations the program's state is currently in.
        /// Its main purpose is to remind the user, in case they forget what they were doing.
        /// </summary>
        private Stack<string> stateStack = new Stack<string>();

        /// <summary>
        /// The stack state of the program, prepared to be printed into console.
        /// </summary>
        private ConsoleText stateString
        {
            get => (string.Join(" >> ", this.stateStack) + '\n').ToConsole(ConsoleColor.Yellow, ConsoleColor.Black);
        }

        /// <summary>
        /// Clears the console and prints the stack string.
        /// </summary>
        private void resetConsole()
        {
            Console.Clear();
            stateString.ConsolePrint();
            Console.WriteLine();
        }
        
        string IClient.GetInput(string prevText)
        {
            resetConsole();
            Console.Write(prevText);
            Console.ForegroundColor = INPUT_FOREGROUND;
            string r =  Console.ReadLine();
            Console.ResetColor();
            return r;
        }

        /// <summary>
        /// Returns a character from user input.
        /// </summary>
        /// <returns>The character inserted by the user.</returns>
        private char getSingle(string prevText)
        {
            resetConsole();
            Console.Write(prevText);
            Console.ForegroundColor = INPUT_FOREGROUND;
            char r = Console.ReadKey(true).KeyChar;
            Console.ResetColor();
            return r;
        }

        bool IClient.TryAgain(string msg)
        {
            char r = getSingle(msg + "\nPress \"q\" to quit\nPress any other key to try again...\n");
            return !("qQ\uffff".Contains(r));
        }

        bool IClient.GetFormInput(string prevText, ref (string, string)[] args)
        {
            int fieldPointer = 0;
            bool writing = false;

            while(true)
            {
                resetConsole();
                Console.WriteLine(prevText);
                Console.WriteLine();
                foreach(var (name, value, i) in args.Select((data, i) => (data.Item1, data.Item2, i)))
                {
                    Console.BackgroundColor = i == fieldPointer ? ConsoleColor.Cyan : ConsoleColor.Black;
                    Console.ForegroundColor = i == fieldPointer ? ConsoleColor.Black : ConsoleColor.Cyan;
                    Console.Write(name);
                    Console.ResetColor();
                    Console.WriteLine(": " + value);
                }
                Console.WriteLine();
                Console.WriteLine(writing ? "WRITE" : "SELECT");
                Console.Write(args[fieldPointer].Item1);
                Console.Write(": ");
                if(writing)
                {
                    args[fieldPointer].Item2 = Console.ReadLine();
                    writing = false;
                } else
                {
                    Console.WriteLine(args[fieldPointer].Item2);
                    while(true)
                    {
                        ConsoleKey c = Console.ReadKey(true).Key;
                        if(c == ConsoleKey.UpArrow && fieldPointer > 0)
                        {
                            fieldPointer--;
                            break;
                        }
                        else if(c == ConsoleKey.DownArrow && fieldPointer < args.Length - 1)
                        {
                            fieldPointer++;
                            break;
                        }
                        else if(c == ConsoleKey.RightArrow)
                        {
                            writing = true;
                            break;
                        }
                        else if(c == ConsoleKey.Enter)
                            return true;
                        else if(c == ConsoleKey.Escape)
                            return false;
                    }
                }
            }
        }
    }
}

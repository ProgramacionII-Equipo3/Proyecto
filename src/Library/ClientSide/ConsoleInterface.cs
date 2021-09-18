using Library.ServerSide;
using System;
using System.Collections.Generic;
using System.IO;

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
        /// </summary>
        /// <param name="prevText">The message to show before asking for input. It can be very long.</param>
        /// <returns>The input given by the user.</returns>
        private string GetInput(string prevText)
        {
            Console.Clear();
            stateString.ConsolePrint();
            Console.WriteLine();
            Console.Write(prevText);
            Console.ForegroundColor = INPUT_FOREGROUND;
            string r =  Console.ReadLine();
            Console.ResetColor();
            return r;
        }

        /// <summary>
        /// Returns input from the user after showing a message.
        /// Together with GetChar(string), they are the only functions through which the object will interact with the console.
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


        private string GetFormInput(params ConsoleText[] prevText)
        {

        }

        User IClientInterface.SignIn(IDatabaseConnection conn)
        {

        }
    }
}
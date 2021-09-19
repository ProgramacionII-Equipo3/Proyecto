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
        
        /// <summary>
        /// Returns input from the user after showing a message.
        /// Together with GetChar and GetFormInput, they are the only functions through which the object will interact with the console.
        /// Does not add line break to message.
        /// </summary>
        /// <param name="prevText">The message to show before asking for input. It can be very long.</param>
        /// <returns>The input given by the user.</returns>
        private string getInput(string prevText)
        {
            resetConsole();
            Console.Write(prevText);
            Console.ForegroundColor = INPUT_FOREGROUND;
            string r =  Console.ReadLine();
            Console.ResetColor();
            return r;
        }

        /// <summary>
        /// Returns a character from the user after showing a message.
        /// Together with GetInput and GetFormInput, they are the only functions through which the object will interact with the console.
        /// Does not add line break to message.
        /// </summary>
        /// <param name="prevText">The message to show before asking for input. It can be very long.</param>
        /// <returns>The character given by the user.</returns>
        private char getChar(string prevText)
        {
            resetConsole();
            Console.Write(prevText);
            Console.ForegroundColor = INPUT_FOREGROUND;
            char r = Console.ReadKey(true).KeyChar;
            Console.ResetColor();
            return r;
        }

        /// <summary>
        /// Asks the user if they want to retry an operation after failing before.
        /// </summary>
        /// <returns>Whether the user wants to retry</returns>
        private bool tryAgain(string msg)
        {
            char r = getChar(msg + "\nPress \"q\" to quit\nPress any other key to try again...\n");
            return !("qQ\uffff".Contains(r));
        }

        /// <summary>
        /// Repeatedly attempts to retrieve data specified by the function until it succeeds or the user doesn't want to try again.
        /// </summary>
        /// <param name="func">The function to execute to retrieve the data, or the error to print before asking to try again.</param>
        /// <typeparam name="T">The type of the data to retrieve.</typeparam>
        /// <returns>The data retrieved by func, if it did.</returns>
        private T tryUntilValid<T>(Func<(T, string)> func)
        {
            while(true)
            {
                var (r, msg) = func();
                if(r is T result) return result;
                if(msg != null && !tryAgain(msg))
                    return default(T);
            }
        }

        /// <summary>
        /// Receives input about several data (as if it was a form)
        /// and returns the result of processing that information, or null if it was a failure.
        /// Together with GetInput and GetChar, they are the only functions through which the object will interact with the console.
        /// </summary>
        /// <param name="state">The state to push to the stateStack</param>
        /// <param name="prevText">A placeholder text to print before asking for data.</param>
        /// <param name="func">The function which describes what to do with the given data.</param>
        /// <param name="argNames">The name of the arguments asked to the user.</param>
        /// <typeparam name="T">The type returned by the processing function.</typeparam>
        /// <returns>The resulting object if there were no issues in the process, null if there were.</returns>
        private T getFormInput<T>(
            string state,
            string prevText,
            Func<Dictionary<string, string>, (T, string)> func,
            params string[] argNames)
        {
            stateStack.Push(state.Trim());

            var args = argNames.Aggregate(
                new List<string>(),
                (list, v) =>
                {
                    if(!list.Contains(v)) list.Add(v);
                    return list;
                }
            ).Where(name => !string.IsNullOrWhiteSpace(name))
             .Select<string, (string, string)>(name => (name.Trim(), ""))
             .ToArray();

            T result = tryUntilValid<T>(() => {
                if(!getFormInput(prevText, ref args)) return (default(T), null);
                return func(Utils.DictionaryFromList(args));
            });
            stateStack.Pop();
            return result;
        }

        /// <summary>
        /// Receives form data from the user to process, and stores it in a referenced array.
        /// </summary>
        /// <param name="prevText">A placeholder text to print before asking for data.</param>
        /// <param name="args">A reference the arguments asked to the user.</param>
        /// <returns>Whether the user didn't want to go back.</returns>
        private bool getFormInput(string prevText, ref (string, string)[] args)
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

        private (User, string) signIn(string name, string password, IDatabaseConnection conn)
        {
            name = name.Trim();
            password = password.Trim();
            if(
                Utils.CheckStrings( ("name", name), ("password", password) )
                is string error
            ) return (null, error);

            SignInResult response = conn.SignIn(name, password);
            switch(response)
            {
                case SignInResult.OkAdmin:         return (new User(UserType.Admin,       name), null);
                case SignInResult.OkEntrepeneur:   return (new User(UserType.Entrepeneur, name), null);
                case SignInResult.OkCompany:       return (new User(UserType.Company,     name), null);

                case SignInResult.NotFound:        return (null, "There isn't a user with the specified name.");
                case SignInResult.InvalidPassword: return (null, "Incorrect password.");

                default: throw new Exception();
            }
        }

        User IClientInterface.SignIn(IDatabaseConnection conn) =>
            getFormInput<User>(
                "SIGN-IN",
                "Please insert the necessary data.",
                args => signIn(args["name"], args["password"], conn),
                "name", "password"
            );
    }
}

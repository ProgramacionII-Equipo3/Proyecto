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
        private string stateString
        {
            get => string.Join(" >> ", this.stateStack);
        }

        private void resetConsole()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(stateString);
            Console.WriteLine();
            Console.ResetColor();
        }
        
        /// <summary>
        /// Returns input from the user after showing a message.
        /// Together with GetChar(string), they are the only functions through which the object will interact with the console.
        /// Does not add line break to message.
        /// </summary>
        /// <param name="prevText">The message to show before asking for input. It can be very long.</param>
        /// <returns>The input given by the user.</returns>
        private string GetInput(string prevText)
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
        /// Together with GetInput(string), they are the only functions through which the object will interact with the console.
        /// Does not add line break to message.
        /// </summary>
        /// <param name="prevText">The message to show before asking for input. It can be very long.</param>
        /// <returns>The character given by the user.</returns>
        private char GetChar(string prevText)
        {
            resetConsole();
            Console.Write(prevText);
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
            char r = GetChar(msg + "\nPress \"q\" to quit\nPress any other key to try again...\n");
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

        private T GetFormInput<T>(string state, string prevText, Func<Dictionary<string, string>, (T, string)> func, params string[] argNames)
        {
            stateStack.Push(state);

            var args = argNames.Aggregate(
                new List<string>(),
                (list, v) =>
                {
                    if(!list.Contains(v)) list.Add(v);
                    return list;
                }
            ).Select<string, (string, string)>(name => (name, "")).ToArray();

            while(true)
            {
                GetFormInput(prevText, ref args);
                var (r, msg) = func(Utils.DictionaryFromList(args));
                if(r is T result)
                {
                    stateStack.Pop();
                    return result;
                }
                resetConsole();
                if(msg != null && !TryAgain(msg))
                {
                    stateStack.Pop();
                    return default(T);
                }
            }
        }

        private void GetFormInput(string prevText, ref (string, string)[] args)
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
                    Console.Write("EXTRA:");
                    Console.ReadLine();
                } else
                {
                    Console.WriteLine(args[fieldPointer].Item2);
                    while(true)
                    {
                        ConsoleKey c = Console.ReadKey().Key;
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
                            return;
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
            GetFormInput<User>(
                "SIGN-IN",
                "Please insert the necessary data.",
                args => signIn(args["name"], args["password"], conn),
                "name", "password"
            );
    }
}

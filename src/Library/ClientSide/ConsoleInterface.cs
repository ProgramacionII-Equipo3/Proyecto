using System;
using Library.ServerSide;

namespace Library.ClientSide
{
    /// <summary>
    /// This class represents a client interface which works through the console.
    /// </summary>
    public class ConsoleInterface : IClientInterface
    {
        /// <summary>
        /// Retrieves data from the console after printing a prompt to it.
        /// </summary>
        /// <returns>The user input</returns>
        private static string getInput()
        {
            Console.Write($"> ");
            return Console.ReadLine();
        }

        /// <summary>
        /// Retrieves data from the console after printing a placeholder hint to it.
        /// </summary>
        /// <param name="name">The placeholder to print</param>
        /// <returns>The user input</returns>
        private static string getPlaceheldInput(string name)
        {
            Console.Write($"{name}: ");
            return Console.ReadLine();
        }

        /// <summary>
        /// Determines the user type referenced by the given user input
        /// </summary>
        /// <param name="option">The user input to transform into a user type</param>
        /// <returns>The user type (if it's not a valid user type, returns null)</returns>
        private static UserType? stringToUserType(string option)
        {
            switch(option.ToUpper())
            {
                case "A": return UserType.Admin;
                case "E": return UserType.Entrepeneur;
                case "C": return UserType.Company;
                default : return null;
            }
        }

        /// <summary>
        /// Asks the user if they want to retry an operation after failing before.
        /// </summary>
        /// <returns>Whether the user wants to retry</returns>
        private static bool TryAgain()
        {
            Console.WriteLine("Press \"q\" to quit...");
            Console.WriteLine("Press any other key to try again...");
            ConsoleKeyInfo r = Console.ReadKey(true);
            return !("Qq".Contains(r.KeyChar));
        }

        /// <summary>
        /// Repeatedly attempts to retrieve data specified by the function until it succeeds or the user doesn't want to try again.
        /// </summary>
        /// <param name="func">The function to execute to retrieve the data.</param>
        /// <param name="msg">An error message to print before asking to try again.</param>
        /// <typeparam name="T">The type of the data to retrieve.</typeparam>
        /// <returns>The data retrieved by func, if it did.</returns>
        private static T TryUntilValid<T>(Func<T> func, string msg = null)
        {
            while(true)
            {
                T r = func();
                if(r is T result) return result;
                if(msg != null) Console.WriteLine(msg);
                if(!TryAgain()) return default(T);
            }
        }

        /// <summary>
        /// Repeatedly attempts to retrieve data specified by the function until it succeeds or the user doesn't want to try again.
        /// </summary>
        /// <param name="func">The function to execute to retrieve the data, or the error to print before asking to try again.</param>
        /// <typeparam name="T">The type of the data to retrieve.</typeparam>
        /// <returns>The data retrieved by func, if it did.</returns>
        private static T TryUntilValid<T>(Func<(T, string)> func)
        {
            while(true)
            {
                var (r, msg) = func();
                if(r is T result) return result;
                if(msg != null) Console.WriteLine(msg);
                if(!TryAgain()) return default(T);
            }
        }

        private static UserType? GetUserType()
        {
            Console.Clear();
            Console.WriteLine("Welcome. What kind of user are you?");
            Console.WriteLine("\tA: Admin");
            Console.WriteLine("\tE: Entrepeneur");
            Console.WriteLine("\tC: Company");
            return stringToUserType( getInput() );
        }

        private (User, string) signIn(IDatabaseConnection conn)
        {
            if(
                   TryUntilValid(GetUserType, "Invalid answer.")                           is UserType type
                && TryUntilValid(() => getPlaceheldInput("Name"), "Invalid answer.")       is string   name
                && TryUntilValid(() => getPlaceheldInput("Password"), "Invalid password.") is string   password
            )
            {
                User user = new User(type, name);
                SignInResult response = conn.SignIn(user, password);
                switch(response)
                {
                    case SignInResult.Ok: return (user, null);
                    case SignInResult.NotFound: return (null, "There isn't a user with the specified data.");
                    case SignInResult.InvalidPassword: return (null, "The type, name, or password are invalid.")
                        
                }
            } else return (null, "");
        }

        User IClientInterface.SignIn(IDatabaseConnection conn)
        {
            return TryUntilValid<User>(() => signIn(conn));
        }
    }
}
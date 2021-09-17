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
        /// The current state stack of the interface.
        /// It expresses the paths of menus and options the user went through.
        /// Its main purpose is to help the player remember what it was doing.
        /// </summary>
        private List<string> stateStack = new List<string>();

        private string stateStackStr() =>
            string.Join(" >> ", this.stateStack);

        /// <summary>
        /// Retrieves data from the console after printing a prompt to it.
        /// </summary>
        /// <returns>The user input</returns>
        private string getInput()
        {
            Console.Write($"> ");
            return Console.ReadLine();
        }

        /// <summary>
        /// Retrieves data from the console after printing a placeholder hint to it.
        /// </summary>
        /// <param name="name">The placeholder to print</param>
        /// <returns>The user input</returns>
        private string getPlaceheldInput(string name)
        {
            Console.Write($"{name}: ");
            return Console.ReadLine();
        }

        /// <summary>
        /// Determines the user type referenced by the given user input.
        /// </summary>
        /// <param name="option">The user input to transform into a user type.</param>
        /// <returns>The user type (if it's not a valid user type, returns null).</returns>
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
        private bool TryAgain()
        {
            Console.WriteLine("Press \"q\" to quit...");
            Console.WriteLine("Press any other key to try again...");
            char r = (char)Console.Read();
            return !("qQ\uffff".Contains(r));
        }

        /// <summary>
        /// Repeatedly attempts to retrieve data specified by the function until it succeeds or the user doesn't want to try again.
        /// </summary>
        /// <param name="func">The function to execute to retrieve the data.</param>
        /// <param name="msg">An error message to print before asking to try again.</param>
        /// <typeparam name="T">The type of the data to retrieve.</typeparam>
        /// <returns>The data retrieved by func, if it did.</returns>
        private T TryUntilValid<T>(Func<T> func, string msg)
        {
            while(true)
            {
                T r = func();
                if(r is T result) return result;
                Console.WriteLine();
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
        private T TryUntilValid<T>(Func<(T, string)> func)
        {
            while(true)
            {
                var (r, msg) = func();
                if(r is T result) return result;
                if(msg != null) Console.WriteLine(msg);
                if(!TryAgain()) return default(T);
            }
        }

        /// <summary>
        /// Attempts to sign in with a concrete user.
        /// This is a separate method to facilitate debugging.
        /// </summary>
        /// <param name="name">The user's name.</param>
        /// <param name="password">The user's password.</param>
        /// <param name="conn">The connection to the database.</param>
        /// <returns>The resulting user if the operation is successful, null if not.</returns>
        private (User, string) signIn(string name, string password, IDatabaseConnection conn)
        {
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

        /// <summary>
        /// Performs the operation to sign in to the program.
        /// </summary>
        /// <param name="conn">The connection with which the interface accesses the database.</param>
        /// <returns>The user, if the operation succeeded.</returns>
        private (User, string) signIn(IDatabaseConnection conn)
        {
            if(
                   TryUntilValid(() => getPlaceheldInput("Name"), "Invalid answer.")       is string   name
                && TryUntilValid(() => getPlaceheldInput("Password"), "Invalid password.") is string   password
            )
                return signIn(name, password, conn);
            else
                return (null, null);
        }

        User IClientInterface.SignIn(IDatabaseConnection conn) =>
            TryUntilValid<User>(() => signIn(conn));
    }
}
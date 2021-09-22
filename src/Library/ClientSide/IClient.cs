using System;
using System.Collections.Generic;
using System.Linq;
using Library.ServerSide;

namespace Library.ClientSide
{
    /// <summary>
    /// This interface organizes the interactions between people and the program.
    /// </summary>
    /// <typeparam name="T">The type returned by getSingle.</typeparam>
    public interface IClient<T>
    {
        /// <summary>
        /// Returns a line of user input.
        /// </summary>
        /// <returns>The input given by the user.</returns>
        string GetInput();

        /// <summary>
        /// Returns a simple piece of user input, such as a character in the console, or a button enum in a GUI.
        /// </summary>
        /// <returns>The simple piece of user input.</returns>
        T GetSingle();

        /// <summary>
        /// Asks the user if they want to retry an operation after failing before.
        /// </summary>
        /// <param name="msg">An error message.</param>
        /// <returns>Whether the user wants to retry</returns>
        bool TryAgain(string msg);

        /// <summary>
        /// Repeatedly attempts to retrieve data specified by the function until it succeeds or the user doesn't want to try again.
        /// </summary>
        /// <param name="func">The function to execute to retrieve the data, or the error to print before asking to try again.</param>
        /// <typeparam name="T">The type of the data to retrieve.</typeparam>
        /// <returns>The data retrieved by func, if it did.</returns>
        U TryUntilValid<U>(Func<(U, string)> func)
        {
            while(true)
            {
                var (r, msg) = func();
                if(r is U result) return result;
                if(msg != null && !TryAgain(msg))
                    return default(U);
            }
        }

        /// <summary>
        /// Receives form data from the user to process, and stores it in a referenced array.
        /// </summary>
        /// <param name="prevText">A placeholder text to print before asking for data.</param>
        /// <param name="args">A reference the arguments asked to the user.</param>
        /// <returns>Whether the user didn't want to go back.</returns>
        bool GetFormInput(string prevText, ref (string, string)[] args);

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
        U GetFormInput<U>(
            string prevText,
            Func<Dictionary<string, string>, (U, string)> func,
            params string[] argNames)
        {
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

            U result = (this as IClient<char>).TryUntilValid<U>(() => {
                if(!GetFormInput(prevText, ref args)) return (default(U), null);
                return func(Utils.DictionaryFromList(args));
            });
            return result;
        }
    }
}
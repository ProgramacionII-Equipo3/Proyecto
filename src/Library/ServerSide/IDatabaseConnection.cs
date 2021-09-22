using System;
using System.Collections.Generic;

namespace Library.ServerSide
{
    /// <summary>
    /// This interface acts as an intermediary between the program and a form of permanent memory
    /// (which could be, among others, a file or a database).
    /// </summary>
    public interface IMemory
    {
        /// <summary>
        /// Retrieves the list of users from the database.
        /// </summary>
        /// <value>The list of users.</value>
        IEnumerable<User> Users { get; }

        /// <summary>
        /// Confirms whether a user can sign in with the given type, name, and password.
        /// </summary>
        /// <param name="name">The user's name.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>Whether the data belongs to a valid user</returns>
        SignInResult SignIn(string name, string password);
    }
}

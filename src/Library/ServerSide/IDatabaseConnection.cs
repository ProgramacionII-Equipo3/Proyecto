using System;
using System.Collections.Generic;

namespace Library.ServerSide
{
    /// <summary>
    /// This interface acts as an intermediary between the program and what stores the data permanently
    /// (it could be a file, a database, or any other form of permanent memory).
    /// </summary>
    public interface IDatabaseConnection
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
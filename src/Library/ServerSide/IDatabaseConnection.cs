using System;
using System.Collections.Generic;

namespace Library.ServerSide
{
    public interface IDatabaseConnection
    {
        /// <summary>
        /// Retrieves a list of users from the database.
        /// </summary>
        /// <returns>The list of users.</returns>
        IEnumerable<User> GetUsers();

        /// <summary>
        /// Confirms whether a user can sign in with the given type, name, and password.
        /// </summary>
        /// <param name="userData">The user to attempt to sign in.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>Whether the data belongs to a valid user</returns>
        SignInResult SignIn(User userData, string password);
    }
}
using System;
using Library.ServerSide;

namespace Library.ClientSide
{
    /// <summary>
    /// This interface organizes the interactions between people and the program.
    /// </summary>
    public interface IClientInterface
    {
        /// <summary>
        /// Returns a user whose data is inserted by the client.
        /// </summary>
        /// <param name="conn">The connection with which the interface validates the given user.</param>
        /// <returns>A user object representing the signed in user, or null if the operation didn't work.</returns>
        User SignIn(IDatabaseConnection conn);
    }
}

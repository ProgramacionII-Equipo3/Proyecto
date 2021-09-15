using System;
using Library.ServerSide;

namespace Library.ClientSide
{
    /// <summary>
    /// This interface organizes the interactions between people and the program
    /// </summary>
    public interface IClientInterface
    {
        User SignIn(IDatabaseConnection conn);
    }
}
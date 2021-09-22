using System;
using Library.ClientSide;
using Library.ServerSide;

namespace Library
{
    /// <summary>
    /// This class acts as a structurer, as well as the manager between the user input and the permanent memory.
    /// </summary>
    public class IOManager
    {
        /// <summary>
        /// The program's input manager
        /// </summary>
        private readonly IClient<char> client;

        /// <summary>
        /// The program's memory manager
        /// </summary>
        private readonly IMemory memory;

        public IOManager(IClient<char> client, IMemory memory)
        {
            this.client = client;
            this.memory = memory;
        }

        private (User, string) signIn(string name, string password, IMemory conn)
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

        /// <summary>
        /// Returns a user whose data is inserted by the client.
        /// </summary>
        /// <param name="conn">The connection with which the interface validates the given user.</param>
        /// <returns>A user object representing the signed in user, or null if the operation didn't work.</returns>
        public User SignIn(IMemory conn) =>
            (this as IClient<char>).GetFormInput<User>(
                "Please insert the necessary data.",
                args => signIn(args["name"], args["password"], conn),
                "name", "password"
            );

    }
}
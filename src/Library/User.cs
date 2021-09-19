using System;

namespace Library
{
    /// <summary>
    /// This class represents a registered user of the program.
    /// It doesn't contain its password, since it's only relevant to the database which the program gets the data from.
    /// </summary>
    public class User
    {
        /// <summary>
        /// The user type of this user.
        /// </summary>
        public UserType Type { get; private set; }

        /// <summary>
        /// The user's name.
        /// </summary>
        public string Name { get; }

        public User(UserType type, string name)
        {
            this.Type = type;
            this.Name = name;
        }
    }
}

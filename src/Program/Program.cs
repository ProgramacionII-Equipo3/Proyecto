using System;
using Library.ClientSide;
using Library.ServerSide;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            IClientInterface cli = new ConsoleInterface();
            IDatabaseConnection conn = new FileDatabaseConnection(@"../../Assets/Data.json");
            Library.User user = cli.SignIn(conn);

            Console.WriteLine();

            if(user == null)
            {
                Console.WriteLine("No user");
                return;
            }

            Console.WriteLine(user.Type.ToString());
            Console.WriteLine(user.Name);
        }
    }
}

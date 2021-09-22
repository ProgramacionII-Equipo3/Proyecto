using System;
using Library;
using Library.ClientSide;
using Library.ServerSide;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            IClient<char> cli = new ConsoleClient();
            IMemory conn = new FileMemory(@"../../Assets/Data.json");
            IOManager manager = new IOManager(cli, conn);
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

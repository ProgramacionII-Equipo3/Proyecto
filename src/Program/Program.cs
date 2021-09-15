using System;
using Library.ClientSide;
using Library.ServerSide;
using Library.ClientSide.ConsoleIO;
using Library.ServerSide.FileIO;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            Library.StringReader reader = new Library.StringReader(
                "Santiago De Olivera\n123\nnBruno\nABC\n"
            );
            IClientInterface clientInterface = new TextInterface(reader, new ConsoleWriter());
            IDatabaseConnection connection = new FileDatabaseConnection(@"../../Assets/Data.json");
            Console.WriteLine(clientInterface.SignIn(connection));
        }
    }
}

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
            
        }
    }
}

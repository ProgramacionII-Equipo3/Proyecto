using NUnit.Framework;
using Library.ClientSide;
using Library.ClientSide.ConsoleIO;
using Library.ServerSide;

namespace Library.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            StringReader reader = new StringReader(
                "Santiago De Olivera\n123\nnBruno\nABC\n"
            );
            IClientInterface clientInterface = new TextInterface(reader, new ConsoleWriter());
            IDatabaseConnection connection = new FileDatabaseConnection(@"../../Assets/Data.json");
            Console.WriteLine(clientInterface.SignIn(connection));
        }
    }
}
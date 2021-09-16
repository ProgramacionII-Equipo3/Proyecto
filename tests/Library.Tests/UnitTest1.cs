using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Library.ClientSide;
using Library.ServerSide;

namespace Library.Tests
{
    struct TestCaseData
    {
        public readonly string Input;
        public readonly List<(string, bool)> Expected;
        public TestCaseData(string input, params (string, bool)[] expected)
        {
            this.Input = input;
            this.Expected = expected.ToList();
        }
    }

    public class Tests
    {
        private static readonly List<TestCaseData> testCases = new List<TestCaseData>();

        [SetUp]
        public void Setup()
        {
            testCases.Add(
                new TestCaseData(
                    "Santiago De Olivera\n123\nnBruno\nABC\n",
                    ("Name: ", false),
                    ("Santiago De Olivera\n", true),
                    ("Password: ", false),
                    ("123\n", true),
                    ("There isn't a user with the specified name.\nPress \"q\" to quit...\nPress any other key to try again...\n", false),
                    ("n", true),
                    ("Name: ", false),
                    ("Bruno\n", true),
                    ("Password: ", false),
                    ("ABC\n", true)
                )
            );
        }

        [TestCase(0)]
        public void Test1(int index)
        {
            IOStringBuilder IoStr = new IOStringBuilder();
            TextReader reader = new StringReader (
                testCases[index].Input,
                IoStr
            );
            TextWriter writer = new FuncTextWriter(IoStr.AddOutputChar, Console.OutputEncoding);
            IClientInterface clientInterface = new TextInterface(reader, writer);
            IDatabaseConnection connection = new FileDatabaseConnection(@"../../../../../Assets/Data.json");
            clientInterface.SignIn(connection);
//            IoStr.Print(ConsoleColor.Red);
            
            foreach(var el in testCases[index].Expected)
            {
                Console.WriteLine(el);
            }
            Console.WriteLine("------------------");
            foreach(var el in IoStr.Text)
            {
                Console.WriteLine(el);
            }
            Assert.IsTrue(IoStr.Text.SequenceEqual(testCases[index].Expected));
        }
    }
}
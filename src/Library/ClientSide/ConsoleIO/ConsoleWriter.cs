using System;
using System.IO;
using System.Text;

namespace Library.ClientSide.ConsoleIO
{
    /// <summary>
    /// This class writes characters into the console.
    /// </summary>
    public class ConsoleWriter: TextWriter
    {
        public override Encoding Encoding {
            get => Console.OutputEncoding;
        }

        public override void Write(char value)
        {
            Console.Write(value);
        }
    }
}
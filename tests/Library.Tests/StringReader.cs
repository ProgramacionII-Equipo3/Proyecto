using System;

namespace Library.Tests
{
    public class StringReader : System.IO.TextReader
    {
        private System.IO.StringReader reader;

        public StringReader(string str)
        {
            this.reader = new System.IO.StringReader(str);
        }

        public override int Peek() => this.reader.Peek();
        public override int Read()
        {
            int r = this.reader.Read();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write((char)r);
            Console.ResetColor();
            return r;
        }
    }
}
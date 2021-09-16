using System;

namespace Library.Tests
{
    public class StringReader : System.IO.TextReader
    {
        private System.IO.StringReader reader;

        private IOStringBuilder IoStr;

        public StringReader(string str, IOStringBuilder IoStr)
        {
            this.reader = new System.IO.StringReader(str);
            this.IoStr = IoStr;
        }

        public override int Peek() => this.reader.Peek();
        public override int Read()
        {
            int r = this.reader.Read();
            IoStr?.AddInputChar((char)r);
            return r;
        }
    }
}
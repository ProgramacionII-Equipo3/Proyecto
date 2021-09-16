using System;
using System.IO;
using System.Text;

namespace Library.Tests
{
    /// <summary>
    /// This class implements TextWriter through a lambda function which determines where to write the characters.
    /// </summary>
    public class FuncTextWriter: TextWriter
    {
        /// <summary>
        /// The mentioned lambda function.
        /// </summary>
        public readonly Action<char> Action;

        private readonly Encoding _encoding;
        /// <summary>
        /// The encoding of the writer.
        /// </summary>
        /// <value></value>
        public override Encoding Encoding {
            get => Console.OutputEncoding;
        }
        public FuncTextWriter(Action<char> action, Encoding encoding)
        {
            this.Action = action;
            this._encoding = encoding;
        }

        public override void Write(char c)
        {
            (this.Action)(c);
        }
    }
}
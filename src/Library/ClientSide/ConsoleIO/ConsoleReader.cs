using System;
using System.IO;
using System.Text;

namespace Library.ClientSide.ConsoleIO
{
    /// <summary>
    /// This class reads characters from the console.
    /// </summary>
    public class ConsoleReader: TextReader
    {
        private int bufferedChar = -1;

        public override int Peek()
        {
            if(bufferedChar == -1) bufferedChar = Console.Read();
            return bufferedChar;
        }

        public override int Read()
        {
            int r = (bufferedChar != -1) ? bufferedChar : Console.Read();
            bufferedChar = -1;
            return r;
        }
    }
}
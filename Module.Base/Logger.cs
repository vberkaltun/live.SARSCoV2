using System;

namespace live.SARSCoV2.Module.Base
{
    class Logger : ILogger
    {
        #region Properties

        public static string Domain => Environment.UserName.ToString();

        private static bool IsVisibleMessage = false;
        private static object Chainlock = new object();

        #endregion

        #region Methods

        public void General(string message, bool newLine = true) => Print(message, newLine);
        public void Informational(string message, bool newLine = true) => Print(message, ConsoleColor.Blue, newLine);
        public void Initialize(string message, bool newLine = true) => Print(string.Format("Initialize: {0}", message), ConsoleColor.Yellow, newLine);
        public void Read(string message, bool newLine = true) => Print(string.Format("Read: {0}", message), ConsoleColor.DarkMagenta, newLine);
        public void Write(string message, bool newLine = true) => Print(string.Format("Write: {0}", message), ConsoleColor.Magenta, newLine);
        public void Error(string message, bool newLine = true) => Print(string.Format("Error: {0}", message), ConsoleColor.Red, newLine);
        public void Succesfull(string message, bool newLine = true) => Print(string.Format("Succesfull: {0}", message), ConsoleColor.Green, newLine);
        public void Connect(string message, bool newLine = true) => Print(string.Format("Connect: {0}", message), ConsoleColor.Green, newLine);
        public void Disconnect(string message, bool newLine = true) => Print(string.Format("Disconnect: {0}", message), ConsoleColor.DarkYellow, newLine);

        public void Print(string message, ConsoleColor color, bool newLine = true)
        {
            lock (Chainlock)
            {
                Console.ForegroundColor = color;
                Print(string.Format("{0}<{1}>: {2}", DateTime.Now.ToString("yyyy/MM/dd-h:mm:ss"), Domain, message), newLine);
                Console.ResetColor();
            }
        }
        public void Print(string message, bool newLine = true)
        {
            if (!IsVisibleMessage)
                return;

            Console.Write("{0}{1}", message, newLine ? "\n" : null);
        }

        public string ReadMessage() => Console.ReadLine();
        public char ReadChar() => Console.ReadKey().KeyChar;

        public void SetVisibleMessage(bool flag = true) => IsVisibleMessage = flag;
        public bool GetVisibleMessage() => IsVisibleMessage;

        #endregion
    }
}

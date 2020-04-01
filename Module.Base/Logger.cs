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
        public void Initialize(string message, bool newLine = true) => Print(string.Format("TASK_INT:{0}", message), ConsoleColor.Yellow, newLine);
        public void Read(string message, bool newLine = true) => Print(string.Format("HTTP_REA:{0}", message), ConsoleColor.DarkMagenta, newLine);
        public void Write(string message, bool newLine = true) => Print(string.Format("TASK_WRI:{0}", message), ConsoleColor.Magenta, newLine);
        public void Error(string message, bool newLine = true) => Print(string.Format("TASK_ERR:{0}", message), ConsoleColor.Red, newLine);
        public void Succesfull(string message, bool newLine = true) => Print(string.Format("TASK_SUC:{0}", message), ConsoleColor.Green, newLine);

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

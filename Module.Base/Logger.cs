using FluentScheduler;
using System;

namespace live.SARSCoV2.Module.Base
{
    class Logger : Registry, ILogger
    {
        #region Properties

        public static string Domain => Environment.UserName.ToString();

        private static bool IsVisibleMessage = false;
        private static object Chainlock = new object();

        public enum JobType
        {
            General, Informational, Initialize,
            Read, Write,
            Error, Succesfull,
        }

        #endregion

        #region Methods

        public void PrintMessage(string message, JobType type)
        {
            lock (Chainlock)
            {
                // print date and time
                PrintMessage(string.Format("{0}<{1}>: ", DateTime.Now.ToString("yyyy/MM/dd-h:mm:ss"), Domain), false);

                switch (type)
                {
                    default:
                    case JobType.General:
                        PrintMessage(message);
                        break;

                    case JobType.Informational:
                        PrintMessage(message, ConsoleColor.Blue);
                        break;

                    case JobType.Initialize:
                        PrintMessage(string.Format("TASK_INT:{0}", message), ConsoleColor.Yellow);
                        break;

                    case JobType.Read:
                        PrintMessage(string.Format("HTTP_REA:{0}", message), ConsoleColor.DarkMagenta);
                        break;

                    case JobType.Write:
                        PrintMessage(string.Format("TASK_WRI:{0}", message), ConsoleColor.Magenta);
                        break;

                    case JobType.Error:
                        PrintMessage(string.Format("TASK_ERR:{0}", message), ConsoleColor.Red);
                        break;

                    case JobType.Succesfull:
                        PrintMessage(string.Format("TASK_SUC:{0}", message), ConsoleColor.Green);
                        break;
                }
            }
        }
        public void PrintMessage(string message, ConsoleColor color, bool newLine = true)
        {
            Console.ForegroundColor = color;
            PrintMessage(message, newLine);
            Console.ResetColor();
        }
        public void PrintMessage(string message, bool newLine = true)
        {
            if (!IsVisibleMessage)
                return;

            if (newLine)
                Console.WriteLine("{0}", message);
            else
                Console.Write("{0}", message);
        }

        public string ReadMessage() => Console.ReadLine();
        public char ReadChar() => Console.ReadKey().KeyChar;

        public void SetVisibleMessage(bool flag = true) => IsVisibleMessage = flag;
        public bool GetVisibleMessage() => IsVisibleMessage;

        #endregion
    }
}

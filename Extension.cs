using System;
using static live.SARSCoV2.Global;

namespace live.SARSCoV2
{
    public static class Extension
    {
        #region Properties

        private static readonly object Chain = new object();

        private static bool IsVisibleMessage { get; set; }

        #endregion

        #region Console

        public static void PrintMessage(string message, JobType type)
        {
            lock (Chain)
            {
                // print date and time
                PrintMessage(string.Format("{0}<{1}>: ", DateTime.Now.ToString("yyyy/MM/dd-h:mm:ss"), Environment.UserName), false);

                switch (type)
                {
                    default:
                    case JobType.General:
                        PrintMessage(message);
                        break;

                    case JobType.Informational:
                        PrintMessage(message, ConsoleColor.Blue);
                        break;

                    case JobType.HTTPRequest:
                        PrintMessage(string.Format("HTTP_REQ:{0}", message), ConsoleColor.DarkMagenta);
                        break;

                    case JobType.Scheduled:
                        PrintMessage(string.Format("TASK_SCH:{0}", message), ConsoleColor.Yellow);
                        break;

                    case JobType.Executed:
                        PrintMessage(string.Format("TASK_EXE:{0}", message), ConsoleColor.Magenta);
                        break;

                    case JobType.Error:
                        PrintMessage(string.Format("TASK_ERR:{0}", message), ConsoleColor.Red);
                        break;

                    case JobType.Succesfull:
                        PrintMessage(string.Format("TASK_OKK:{0}", message), ConsoleColor.Green);
                        break;
                }
            }
        }
        public static void PrintMessage(string message, ConsoleColor color, bool newLine = true)
        {
            Console.ForegroundColor = color;
            PrintMessage(message, newLine);
            Console.ResetColor();
        }
        public static void PrintMessage(string message, bool newLine = true)
        {
            if (!IsVisibleMessage)
                return;

            if (newLine)
                Console.WriteLine("{0}", message);
            else
                Console.Write("{0}", message);
        }

        public static string ReadMessage() => Console.ReadLine();
        public static char ReadChar() => Console.ReadKey().KeyChar;

        public static void SetVisibleMessage(bool flag = true) => IsVisibleMessage = flag;
        public static bool GetVisibleMessage() => IsVisibleMessage;

        #endregion
    }
}

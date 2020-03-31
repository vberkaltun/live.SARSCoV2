using AutoMapper;
using ISO3166;
using System;
using System.Linq;
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
                PrintMessage(string.Format("{0}<{1}>: ", DateTime.Now.ToString("yyyy/MM/dd-h:mm:ss"), DOMAIN_USERNAME), false);

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

        #region Methods

        public static void PrintAppInfo()
        {
            PrintMessage(string.Format("{0} {1}",
                APP_NAME, APP_VERSION, DOMAIN_USERNAME), JobType.Informational);

            PrintMessage(string.Format("Exit code: {0}, Interval: {1}, Null Value Handling: {2}",
                EXIT_CODE, SCHEDULED_JOB_INTERVAL, NULL_VALUE_HANDLING), JobType.Informational);
        }

        public static Country GetCountryInfo(string country)
        {
            var result1 = Country.List.FirstOrDefault(src => src.Name == country);
            var result2 = Country.List.FirstOrDefault(src => src.TwoLetterCode == country);
            var result3 = Country.List.FirstOrDefault(src => src.ThreeLetterCode == country);
            var result4 = Country.List.FirstOrDefault(src => src.NumericCode == country);

            return result1 != null ? result1 : 
                (result2 != null ? result2 : 
                (result3 != null ? result3 : 
                (result4 != null ? result4 : null)));
        }

        #endregion
    }
}

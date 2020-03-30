using Newtonsoft.Json;
using System;
using System.Reflection;

namespace live.SARSCoV2
{
    public static class Global
    {
        public const char EXIT_CODE = 'E';
        public const int SCHEDULED_JOB_INTERVAL = 300;
        public const NullValueHandling NULL_VALUE_HANDLING = NullValueHandling.Ignore;

        public const string SQL_SERVER = "127.0.0.1";
        public const string SQL_USERNAME = "root";
        public const string SQL_PASSWORD = "8965";
        public const string SQL_DATABASE = "live.sarscov2";

        public readonly static string APP_NAME = Assembly.GetExecutingAssembly().GetName().Name.ToString();
        public readonly static string APP_VERSION = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public readonly static string DOMAIN_USERNAME = Environment.UserName.ToString();

        public enum JobType
        {
            General,
            Informational,
            HTTPRequest,
            Scheduled,
            Executed,
            Error,
            Succesfull,
        }
    }
}

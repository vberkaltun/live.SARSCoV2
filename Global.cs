using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace live.SARSCoV2
{
    public static class Global
    {
        public const char EXIT_CODE = 'E';
        public const int SCHEDULED_JOB_INTERVAL = 2;
        public const NullValueHandling NULL_VALUE_HANDLING = NullValueHandling.Ignore;

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

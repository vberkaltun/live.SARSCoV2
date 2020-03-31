using System;
using System.Collections.Generic;
using System.Reflection;
using FluentScheduler;
using live.SARSCoV2.Dataset.Http;
using live.SARSCoV2.Module.Scheduler;
using live.SARSCoV2.Module.Base;
using static live.SARSCoV2.Global;

namespace live.SARSCoV2
{
    class Program : Logger
    {
        static void Main()
        {
            new SARSCoV2();
        }
    }

    class SARSCoV2 : Logger
    {
        public SARSCoV2()
        {
            // set message visible
            SetVisibleMessage();
            // show main app informations
            PrintAppInfo();

            Dataset.Sql.ICountry aaa = new Dataset.Sql.ICountry();
            aaa.ID = 15;

            PropertyInfo[] propInfos = typeof(Dataset.Sql.ICountry).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var xxx = GetPropValue(aaa, propInfos[0].Name);

            // start the scheduler
            JobManager.Initialize(new Scheduler<General>(@"https://corona.lmao.ninja/all"));
            JobManager.Initialize(new Scheduler<List<Country>>(@"https://corona.lmao.ninja/v2/jhucsse"));
            JobManager.Initialize(new Scheduler<List<Historical>>(@"https://corona.lmao.ninja/v2/historical"));

            while (true)
            {
                if (ReadChar() != EXIT_CODE)
                    continue;

                // stop the scheduler
                JobManager.StopAndBlock();
                break;
            }
        }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        #region Methods

        public void PrintAppInfo()
        {
            PrintMessage(string.Format("{0} {1}",
                APP_NAME, APP_VERSION), JobType.Informational);

            PrintMessage(string.Format("Exit code: {0}, Interval: {1}, Null Value Handling: {2}",
                EXIT_CODE, SCHEDULED_JOB_INTERVAL, NULL_VALUE_HANDLING), JobType.Informational);
        }


        #endregion
    }
}
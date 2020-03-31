using System;
using System.Collections.Generic;
using System.Net.Http;
using FluentScheduler;
using live.SARSCoV2.Dataset.Http;
using live.SARSCoV2.Module.Base;
using live.SARSCoV2.Module.HttpRequest;
using live.SARSCoV2.Module.Scheduler;
using live.SARSCoV2.Module.SqlAdapter;
using live.SARSCoV2.Module.SqlQuery;
using static live.SARSCoV2.Global;

namespace live.SARSCoV2
{
    class Program : Logger
    {
        static void Main() => new SARSCoV2();
    }

    class SARSCoV2 : Logger
    {
        HttpClient Client = new HttpClient();
        SqlAdapter Sql = new SqlAdapter();

        HttpRequest<General> General;
        HttpRequest<List<Country>> Country;
        HttpRequest<List<Historical>> Historical;

        public SARSCoV2()
        {
            // init variable
            Client = new HttpClient();
            Sql = new SqlAdapter();

            General = new HttpRequest<General>(Client, @"https://corona.lmao.ninja/all");
            Country = new HttpRequest<List<Country>>(Client, @"https://corona.lmao.ninja/v2/jhucsse");
            Historical = new HttpRequest<List<Historical>>(Client, @"https://corona.lmao.ninja/v2/historical");

            // init console
            SetVisibleMessage();
            PrintAppInfo();

            // init scheduler
            JobManager.Initialize(new Scheduler(TaskGeneralAsync));
            JobManager.Initialize(new Scheduler(TaskCountry));
            JobManager.Initialize(new Scheduler(TaskHistorical));

            while (true)
            {
                if (ReadChar() != EXIT_CODE)
                    continue;

                // stop the scheduler
                JobManager.StopAndBlock();
                break;
            }
        }

        #region Methods

        public async void TaskGeneralAsync()
        {
            General general = await General.GetAsync();

            await Sql.ConnectAsync();
            Sql.Insert(new Query<General>(general), "general");
        }

        public async void TaskCountry()
        {
            await Country.GetAsync();
        }

        public async void TaskHistorical()
        {
            await Historical.GetAsync();
        }

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
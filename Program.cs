using System.Collections.Generic;
using System.Net.Http;
using FluentScheduler;
using live.SARSCoV2.Dataset.Http;
using live.SARSCoV2.Module.Base;
using live.SARSCoV2.Module.HttpRequest;
using live.SARSCoV2.Module.Scheduler;
using live.SARSCoV2.Module.SqlAdapter;
using live.SARSCoV2.Module.SqlQuery;
using Newtonsoft.Json;
using static live.SARSCoV2.Global;

namespace live.SARSCoV2
{
    class Program : Base
    {
        static void Main() => new SARSCoV2();
    }

    class SARSCoV2 : Base
    {
        JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings { NullValueHandling = NULL_VALUE_HANDLING };

        HttpClient Client;
        SqlAdapterOfSARSCoV2 Sql;

        HttpRequest<General> General;
        HttpRequest<List<Country>> Country;
        HttpRequest<List<Historical>> Historical;

        public SARSCoV2()
        {
            // init variable
            Client = new HttpClient();
            Sql = new SqlAdapterOfSARSCoV2();

            General = new HttpRequest<General>(Client, @"https://corona.lmao.ninja/all", JsonSerializerSettings);
            Country = new HttpRequest<List<Country>>(Client, @"https://corona.lmao.ninja/v2/jhucsse", JsonSerializerSettings);
            Historical = new HttpRequest<List<Historical>>(Client, @"https://corona.lmao.ninja/v2/historical", JsonSerializerSettings);

            // init console
            Logger.SetVisibleMessage();
            PrintAppInfo();

            // init scheduler
            JobManager.Initialize(new Scheduler(TaskGeneralAsync,10));
            JobManager.Initialize(new Scheduler(TaskCountry, 10));
            JobManager.Initialize(new Scheduler(TaskHistorical, 10));

            while (true)
            {
                if (Logger.ReadChar() != EXIT_CODE)
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
            Logger.Informational(string.Format("{0} {1}",
                APP_NAME, APP_VERSION));

            Logger.Informational(string.Format("Exit code: {0}, Interval: {1}, Null Value Handling: {2}",
                EXIT_CODE, SCHEDULED_JOB_INTERVAL, NULL_VALUE_HANDLING));
        }

        #endregion
    }

    class SqlAdapterOfSARSCoV2 : SqlAdapter
    {
        public SqlAdapterOfSARSCoV2() : base(SQL_SERVER, SQL_USERNAME, SQL_PASSWORD, SQL_DATABASE) { }

        public override void Delete<T>(Query<T> file, string tableName)
        {
            throw new System.NotImplementedException();
        }

        public override void Insert<T>(Query<T> file, string tableName)
        {
            throw new System.NotImplementedException();
        }

        public override List<T> Select<T>(Query<T> file, string tableName)
        {
            throw new System.NotImplementedException();
        }

        public override void Update<T>(Query<T> file, string tableName)
        {
            throw new System.NotImplementedException();
        }
    }
}
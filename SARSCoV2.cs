using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using FluentScheduler;
using live.SARSCoV2.Module.Base;
using live.SARSCoV2.Module.HttpRequest;
using live.SARSCoV2.Module.Scheduler;
using live.SARSCoV2.Module.SqlAdapter;
using live.SARSCoV2.Module.Property;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Http = live.SARSCoV2.Dataset.Http;
using Sql = live.SARSCoV2.Dataset.Sql;

namespace live.SARSCoV2
{
    class SARSCoV2 : BaseMember
    {
        #region Constants

        public readonly char EXIT_CODE = 'E';
        public readonly int SCHEDULED_JOB_INTERVAL = 300;
        public readonly NullValueHandling NULL_VALUE_HANDLING = NullValueHandling.Ignore;

        public readonly string SQL_SERVER = "127.0.0.1";
        public readonly string SQL_USERNAME = "root";
        public readonly string SQL_PASSWORD = "8965";
        public readonly string SQL_DATABASE = "live.sarscov2";

        public readonly string APP_NAME = Assembly.GetExecutingAssembly().GetName().Name.ToString();
        public readonly string APP_VERSION = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        #endregion

        #region Properties

        public static SemaphoreSlim Semaphore = new SemaphoreSlim(1, 1);

        private JsonSerializerSettings JsonSerializerSettings;
        private SqlAdapter SqlClient;
        private HttpClient HttpClient;

        private HttpRequest<Http.General> General;
        private HttpRequest<List<Http.Country>> Country;
        private HttpRequest<List<Http.Historical>> Historical;
        private HttpRequest<List<Http.States>> States;

        #endregion

        #region Methods

        public SARSCoV2()
        {
            Init();

            while (true)
            {
                if (Logger.ReadChar() != EXIT_CODE)
                    continue;

                // stop the scheduler
                JobManager.StopAndBlock();
                break;
            }
        }

        private void Init()
        {
            // init console
            Logger.SetVisibleMessage();
            PrintAppInfo();

            // init variable
            JsonSerializerSettings = new JsonSerializerSettings { NullValueHandling = NULL_VALUE_HANDLING };
            SqlClient = new SqlAdapter(SQL_SERVER, SQL_USERNAME, SQL_PASSWORD, SQL_DATABASE);
            HttpClient = new HttpClient();

            // init request
            General = new HttpRequest<Http.General>(HttpClient, @"https://corona.lmao.ninja/all", JsonSerializerSettings);
            Country = new HttpRequest<List<Http.Country>>(HttpClient, @"https://corona.lmao.ninja/v2/jhucsse", JsonSerializerSettings);
            Historical = new HttpRequest<List<Http.Historical>>(HttpClient, @"https://corona.lmao.ninja/v2/historical", JsonSerializerSettings);
            States = new HttpRequest<List<Http.States>>(HttpClient, @"https://corona.lmao.ninja/states", JsonSerializerSettings);

            // init scheduler
            JobManager.Initialize(new Scheduler(TaskGeneral, SCHEDULED_JOB_INTERVAL));
            JobManager.Initialize(new Scheduler(TaskCountry, SCHEDULED_JOB_INTERVAL));
            JobManager.Initialize(new Scheduler(TaskHistorical, SCHEDULED_JOB_INTERVAL));
            JobManager.Initialize(new Scheduler(TaskStates, SCHEDULED_JOB_INTERVAL));
        }

        private async void TaskGeneral()
        {
            await Semaphore.WaitAsync();
            await SqlClient.ConnectAsync();

            Http.General general = await General.GetAsync();
            var result = general.ToJson()?.ToSql();

            if (result != null)
            {
                SqlClient.Insert(result, "general", "Content");

                // show general status
                Logger.Write("[General] General info successfully processed!");
            }

            await SqlClient.DisconnectAsync();
            Semaphore.Release();
        }
        private async void TaskCountry()
        {
            await Semaphore.WaitAsync();
            await SqlClient.ConnectAsync();

            int errorCount = 0;
            List<Http.Country> country = await Country.GetAsync();
            foreach (var item in country)
            {
                var result = item.ToJson()?.ToSql();

                if (result == null)
                {
                    // show error status
                    Logger.Error(string.Format("[Historical] <{0}><{1}> info can not add to database, there is no match!", item.Domain, item));

                    errorCount++;
                    continue;
                }

                SqlClient.Insert(result, "country", "Content");
            }

            // show general status
            Logger.Write(string.Format("[Country] Total {0}/{1} info successfully processed!", country.Count - errorCount, country.Count));

            await SqlClient.DisconnectAsync();
            Semaphore.Release();
        }
        private async void TaskHistorical()
        {
            await Semaphore.WaitAsync();
            await SqlClient.ConnectAsync();

            int errorCount = 0;
            List<Http.Historical> historical = await Historical.GetAsync();
            foreach (var item in historical)
            {
                var result = item.ToJson()?.ToSql();

                if (result == null)
                {
                    // show error status
                    Logger.Error(string.Format("[Historical] <{0}><{1}> info can not add to database, there is no match!", item.Domain, item));

                    errorCount++;
                    continue;
                }

                SqlClient.Insert(result, "historical", "Content");
                SqlClient.Update(result, "historical");
            }

            // show general status
            Logger.Write(string.Format("[Historical] Total {0}/{1} info successfully processed!", historical.Count - errorCount, historical.Count));

            await SqlClient.DisconnectAsync();
            Semaphore.Release();
        }
        private async void TaskStates()
        {
            await Semaphore.WaitAsync();
            await SqlClient.ConnectAsync();

            int errorCount = 0;
            List<Http.States> states = await States.GetAsync();
            foreach (var item in states)
            {
                var result = item.ToJson()?.ToSql();

                if (result == null)
                {
                    // show error status
                    Logger.Error(string.Format("[States] <{0}><{1}> info can not add to database, there is no match!", item.State, item));

                    errorCount++;
                    continue;
                }

                SqlClient.Insert(result, "states", "Content");
            }

            // show general status
            Logger.Write(string.Format("[States] Total {0}/{1} info successfully processed!", states.Count - errorCount, states.Count));

            await SqlClient.DisconnectAsync();
            Semaphore.Release();
        }

        private void PrintAppInfo()
        {
            Logger.Informational(string.Format("{0} {1}",
                APP_NAME, APP_VERSION));

            Logger.Informational(string.Format("Exit code: {0}, Interval: {1}, Null Value Handling: {2}",
                EXIT_CODE, SCHEDULED_JOB_INTERVAL, NULL_VALUE_HANDLING));
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using FluentScheduler;
using live.SARSCoV2.Module.Base;
using live.SARSCoV2.Module.HttpRequest;
using live.SARSCoV2.Module.Scheduler;
using live.SARSCoV2.Module.SqlAdapter;
using Newtonsoft.Json;
using Http = live.SARSCoV2.Dataset.Http;

namespace live.SARSCoV2
{
    class SARSCoV2 : BaseMember
    {
        #region Constants

        public readonly char EXIT_CODE = 'E';
        public readonly int SCHEDULED_JOB_INTERVAL = 300;
        public readonly NullValueHandling NULL_VALUE_HANDLING = NullValueHandling.Ignore;

        public readonly string SQL_SERVER = "*****";
        public readonly string SQL_USERNAME = "*****";
        public readonly string SQL_PASSWORD = "*****";
        public readonly string SQL_DATABASE = "*****";

        public readonly string APP_NAME = Assembly.GetExecutingAssembly().GetName().Name.ToString();
        public readonly string APP_VERSION = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        #endregion

        #region Properties

        public static SemaphoreSlim Semaphore = new SemaphoreSlim(1, 1);

        private JsonSerializerSettings JsonSerializerSettings;
        private SqlAdapter SqlClient;
        private HttpClient HttpClient;

        private HttpRequest<List<Http.CountryV1>> CountryV1;
        private HttpRequest<List<Http.CountryV2>> CountryV2;
        private HttpRequest<Http.General> General;
        private HttpRequest<List<Http.Historical>> Historical;
        private HttpRequest<List<Http.State>> States;

        #endregion

        #region Methods

        public void Init()
        {
            // init console
            Logger.SetVisibleMessage();
            PrintAppInfo();

            // init variable
            JsonSerializerSettings = new JsonSerializerSettings { NullValueHandling = NULL_VALUE_HANDLING };
            SqlClient = new SqlAdapter(SQL_SERVER, SQL_USERNAME, SQL_PASSWORD, SQL_DATABASE);
            HttpClient = new HttpClient();

            // init request
            CountryV1 = new HttpRequest<List<Http.CountryV1>>(HttpClient, @"https://corona.lmao.ninja/v2/jhucsse", JsonSerializerSettings);
            CountryV2 = new HttpRequest<List<Http.CountryV2>>(HttpClient, @"https://corona.lmao.ninja/v2/countries", JsonSerializerSettings);
            General = new HttpRequest<Http.General>(HttpClient, @"https://corona.lmao.ninja/v2/all", JsonSerializerSettings);
            Historical = new HttpRequest<List<Http.Historical>>(HttpClient, @"https://corona.lmao.ninja/v2/historical", JsonSerializerSettings);
            States = new HttpRequest<List<Http.State>>(HttpClient, @"https://corona.lmao.ninja/v2/states", JsonSerializerSettings);

            // init scheduler
            JobManager.Initialize(new Scheduler(TaskCountryV1, SCHEDULED_JOB_INTERVAL));
            JobManager.Initialize(new Scheduler(TaskCountryV2, SCHEDULED_JOB_INTERVAL));
            JobManager.Initialize(new Scheduler(TaskGeneral, SCHEDULED_JOB_INTERVAL));
            JobManager.Initialize(new Scheduler(TaskHistorical, SCHEDULED_JOB_INTERVAL));
            JobManager.Initialize(new Scheduler(TaskState, SCHEDULED_JOB_INTERVAL));
        }

        private async void TaskGeneral()
        {
            await Semaphore.WaitAsync();
            try
            {
                await SqlClient.ConnectAsync();

                Http.General general = await General.GetAsync();
                var result = general.ToJson()?.ToSql();

                if (result != null)
                {
                    SqlClient.Insert(result, "general", "Content", result.Content);

                    // show general status
                    Logger.Write("[General] General info successfully processed!");
                }

                await SqlClient.DisconnectAsync();
            }
            catch (Exception) { }
            Semaphore.Release();
        }
        private async void TaskCountryV1()
        {
            await Semaphore.WaitAsync();
            try
            {
                await SqlClient.ConnectAsync();

                int errorCount = 0;
                List<Http.CountryV1> country = await CountryV1.GetAsync();

                // truncate the recent table rows
                SqlClient.Truncate("countryv1_recent");

                foreach (var item in country)
                {
                    var result = item.ToJson()?.ToSql();

                    if (result == null)
                    {
                        // show error status
                        Logger.Error(string.Format("[CountryV1] <{0}><{1}> info can not add to database, there is no match!", item.Domain, item));

                        errorCount++;
                        continue;
                    }

                    SqlClient.Insert(result, "countryv1_recent", "Content", result.Content);
                    SqlClient.Insert(result, "countryv1_all", "Content", result.Content);
                }

                // show general status
                Logger.Write(string.Format("[CountryV1] Total {0}/{1} info successfully processed!", country.Count - errorCount, country.Count));

                await SqlClient.DisconnectAsync();
            }
            catch (Exception) { }
            Semaphore.Release();
        }
        private async void TaskCountryV2()
        {
            await Semaphore.WaitAsync();
            try
            {
                await SqlClient.ConnectAsync();

                int errorCount = 0;
                List<Http.CountryV2> country = await CountryV2.GetAsync();

                // truncate the recent table rows
                SqlClient.Truncate("countryv2_recent");

                foreach (var item in country)
                {
                    var result = item.ToJson()?.ToSql();

                    if (result == null)
                    {
                        // show error status
                        Logger.Error(string.Format("[CountryV2] <{0}><{1}> info can not add to database, there is no match!", item.Domain, item));

                        errorCount++;
                        continue;
                    }

                    SqlClient.Insert(result, "countryv2_recent", "Content", result.Content);
                    SqlClient.Insert(result, "countryv2_all", "Content", result.Content);
                }

                // show general status
                Logger.Write(string.Format("[CountryV2] Total {0}/{1} info successfully processed!", country.Count - errorCount, country.Count));

                await SqlClient.DisconnectAsync();
            }
            catch (Exception) { }
            Semaphore.Release();
        }
        private async void TaskHistorical()
        {
            await Semaphore.WaitAsync();
            try
            {
                await SqlClient.ConnectAsync();

                int errorCount = 0;
                List<Http.Historical> historical = await Historical.GetAsync();

                // truncate the recent table rows
                SqlClient.Truncate("historical");

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

                    SqlClient.Insert(result, "historical", "Content", result.Content);
                }

                // show general status
                Logger.Write(string.Format("[Historical] Total {0}/{1} info successfully processed!", historical.Count - errorCount, historical.Count));

                await SqlClient.DisconnectAsync();
            }
            catch (Exception) { }
            Semaphore.Release();
        }
        private async void TaskState()
        {
            await Semaphore.WaitAsync();
            try
            {
                await SqlClient.ConnectAsync();

                int errorCount = 0;
                List<Http.State> state = await States.GetAsync();

                // truncate the recent table rows
                SqlClient.Truncate("state_recent");

                foreach (var item in state)
                {
                    var result = item.ToJson()?.ToSql();

                    if (result == null)
                    {
                        // show error status
                        Logger.Error(string.Format("[State] <{0}><{1}> info can not add to database, there is no match!", item.Province, item));

                        errorCount++;
                        continue;
                    }

                    SqlClient.Insert(result, "state_recent", "Content", result.Content);
                    SqlClient.Insert(result, "state_all", "Content", result.Content);
                }

                // show general status
                Logger.Write(string.Format("[State] Total {0}/{1} info successfully processed!", state.Count - errorCount, state.Count));

                await SqlClient.DisconnectAsync();
            }
            catch (Exception) { }
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
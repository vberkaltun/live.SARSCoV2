using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FluentScheduler;
using live.SARSCoV2.Module.Base;
using live.SARSCoV2.Module.HttpRequest;
using live.SARSCoV2.Module.Scheduler;
using live.SARSCoV2.Module.SqlAdapter;
using live.SARSCoV2.Module.SqlQuery;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using CountryISO = ISO3166.Country;
using Http = live.SARSCoV2.Dataset.Http;
using Json = live.SARSCoV2.Dataset.Json;
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
        private ISqlAdapter SqlClient;
        private HttpClient HttpClient;

        private HttpRequest<Http.General> General;
        private HttpRequest<List<Http.Country>> Country;
        private HttpRequest<List<Http.Historical>> Historical;

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
            // init variable
            JsonSerializerSettings = new JsonSerializerSettings { NullValueHandling = NULL_VALUE_HANDLING };
            SqlClient = new SqlAdapterOfSARSCoV2(SQL_SERVER, SQL_USERNAME, SQL_PASSWORD, SQL_DATABASE);
            HttpClient = new HttpClient();

            // init request
            General = new HttpRequest<Http.General>(HttpClient, @"https://corona.lmao.ninja/all", JsonSerializerSettings);
            Country = new HttpRequest<List<Http.Country>>(HttpClient, @"https://corona.lmao.ninja/v2/jhucsse", JsonSerializerSettings);
            Historical = new HttpRequest<List<Http.Historical>>(HttpClient, @"https://corona.lmao.ninja/v2/historical", JsonSerializerSettings);

            // init console
            Logger.SetVisibleMessage();
            PrintAppInfo();

            // init scheduler
            JobManager.Initialize(new Scheduler(TaskGeneralAsync, 10));
            JobManager.Initialize(new Scheduler(TaskCountry, 10));
            JobManager.Initialize(new Scheduler(TaskHistorical, 10));
        }

        private async void TaskGeneralAsync()
        {
            await Semaphore.WaitAsync();
            await SqlClient.ConnectAsync();

            Http.General general = await General.GetAsync();
            SqlClient.Insert(new Query<Sql.General>(general.ToJson().ToSql()), "general");

            await SqlClient.DisconnectAsync();
            Semaphore.Release();
        }
        private async void TaskCountry()
        {
            await Semaphore.WaitAsync();
            await SqlClient.ConnectAsync();

            List<Http.Country> country = await Country.GetAsync();
            foreach (var item in country)
            {
                var result = item.ToJson()?.ToSql();

                if (result == null)
                {
                    Logger.Error(string.Format("Country<{0}> info can not found!", item.Domain));
                    continue;
                }

                SqlClient.Insert(new Query<Sql.Country>(result), "country");
            }

            await SqlClient.DisconnectAsync();
            Semaphore.Release();
        }
        private async void TaskHistorical()
        {
            await Semaphore.WaitAsync();
            await SqlClient.ConnectAsync();

            List<Http.Historical> historical = await Historical.GetAsync();
            foreach (var item in historical)
            {
                var result = item.ToJson()?.ToSql();

                if (result == null)
                {
                    Logger.Error(string.Format("Country<{0}><{1}> info can not found!", item.Domain, item));
                    continue;
                }
                
                SqlClient.Insert(new Query<Sql.Historical>(result), "historical");
            }

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

    class SqlAdapterOfSARSCoV2 : SqlAdapter
    {
        #region Methods

        public SqlAdapterOfSARSCoV2(string server, string username, string password, string database)
            : base(server, username, password, database) => Expression.Empty();

        public override void Insert<T>(Query<T> file, string tableName)
        {
            var template = @"INSERT INTO {0}({1}) VALUES({2})";
            var properties = file.GetProperties();

            string target = string.Join(", ", properties.Keys.ToArray()).Trim();
            string source = "@" + string.Join(", @", properties.Keys.ToArray()).Trim();

            MySqlCommand command = new MySqlCommand(string.Format(template, tableName, target, source), Connection);
            command.Prepare();

            foreach (var item in properties)
                command.Parameters.AddWithValue(string.Format("@{0}", item.Key), item.Value);

            command.ExecuteNonQuery();
        }

        #endregion
    }
}
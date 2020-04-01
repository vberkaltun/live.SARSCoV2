using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using FluentScheduler;
using live.SARSCoV2.Dataset.Http;
using live.SARSCoV2.Module.Base;
using live.SARSCoV2.Module.HttpRequest;
using live.SARSCoV2.Module.Scheduler;
using live.SARSCoV2.Module.SqlAdapter;
using live.SARSCoV2.Module.SqlQuery;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using CountryISO = ISO3166.Country;

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

        JsonSerializerSettings JsonSerializerSettings;
        ISqlAdapter Sql;
        HttpClient Client;

        HttpRequest<General> General;
        HttpRequest<List<Country>> Country;
        HttpRequest<List<Historical>> Historical;

        #endregion

        #region Methods

        public SARSCoV2()
        {
            // init variable
            JsonSerializerSettings = new JsonSerializerSettings { NullValueHandling = NULL_VALUE_HANDLING };
            Sql = new SqlAdapterOfSARSCoV2(SQL_SERVER, SQL_USERNAME, SQL_PASSWORD, SQL_DATABASE);
            Client = new HttpClient();

            General = new HttpRequest<General>(Client, @"https://corona.lmao.ninja/all", JsonSerializerSettings);
            Country = new HttpRequest<List<Country>>(Client, @"https://corona.lmao.ninja/v2/jhucsse", JsonSerializerSettings);
            Historical = new HttpRequest<List<Historical>>(Client, @"https://corona.lmao.ninja/v2/historical", JsonSerializerSettings);

            // init console
            Logger.SetVisibleMessage();
            PrintAppInfo();
            Task.Run(async () => await Sql.ConnectAsync()).Wait();

            // init scheduler
            JobManager.Initialize(new Scheduler(TaskGeneralAsync, 10));
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

        ~SARSCoV2() => Task.Run(async () => await Sql.DisconnectAsync()).Wait();

        private async void TaskGeneralAsync()
        {
            General general = await General.GetAsync();

            Sql.Insert(new Query<Dataset.Sql.General>(srcJsonToSql), "general");
        }
        private async void TaskCountry()
        {
            List<Country> country = await Country.GetAsync();
        }
        private async void TaskHistorical()
        {
            List<Historical> historical = await Historical.GetAsync();
        }

        private void PrintAppInfo()
        {
            Logger.Informational(string.Format("{0} {1}",
                APP_NAME, APP_VERSION));

            Logger.Informational(string.Format("Exit code: {0}, Interval: {1}, Null Value Handling: {2}",
                EXIT_CODE, SCHEDULED_JOB_INTERVAL, NULL_VALUE_HANDLING));
        }
        private CountryISO GetCountryInfo(string country)
        {
            var result1 = CountryISO.List.FirstOrDefault(src => src.Name == country);
            var result2 = CountryISO.List.FirstOrDefault(src => src.TwoLetterCode == country);
            var result3 = CountryISO.List.FirstOrDefault(src => src.ThreeLetterCode == country);
            var result4 = CountryISO.List.FirstOrDefault(src => src.NumericCode == country);

            return result1 != null ? result1 :
                (result2 != null ? result2 :
                (result3 != null ? result3 :
                (result4 != null ? result4 : null)));
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
            // call first base
            base.Insert(file, tableName);

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

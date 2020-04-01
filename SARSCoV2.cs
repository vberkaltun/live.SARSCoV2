using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using AutoMapper;
using FluentScheduler;
using live.SARSCoV2.Dataset.Http;
using live.SARSCoV2.Module.Base;
using live.SARSCoV2.Module.HttpRequest;
using live.SARSCoV2.Module.Scheduler;
using live.SARSCoV2.Module.SqlAdapter;
using live.SARSCoV2.Module.SqlQuery;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using static live.SARSCoV2.Global;

namespace live.SARSCoV2
{
    class SARSCoV2 : BaseMember
    {
        #region Properties

        JsonSerializerSettings JsonSerializerSettings;
        HttpClient Client;
        SqlAdapterOfSARSCoV2 Sql;

        HttpRequest<General> General;
        HttpRequest<List<Country>> Country;
        HttpRequest<List<Historical>> Historical;

        #endregion

        #region Methods

        public SARSCoV2()
        {
            // init variable
            JsonSerializerSettings = new JsonSerializerSettings { NullValueHandling = NULL_VALUE_HANDLING };
            Client = new HttpClient();
            Sql = new SqlAdapterOfSARSCoV2();

            General = new HttpRequest<General>(Client, @"https://corona.lmao.ninja/all", JsonSerializerSettings);
            Country = new HttpRequest<List<Country>>(Client, @"https://corona.lmao.ninja/v2/jhucsse", JsonSerializerSettings);
            Historical = new HttpRequest<List<Historical>>(Client, @"https://corona.lmao.ninja/v2/historical", JsonSerializerSettings);

            // init console
            Logger.SetVisibleMessage();
            PrintAppInfo();

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

        public async void TaskGeneralAsync()
        {
            General general = await General.GetAsync();

            await Sql.ConnectAsync();

            IMapper httpToJson = JSON.General.CreateMapper();
            IMapper jsonToSql = SQL.General.CreateMapper();

            var srcHttpToJson = httpToJson.Map<Dataset.Http.General, Dataset.Json.General>(general);
            var srcJsonToSql = jsonToSql.Map<Dataset.Json.General, Dataset.Sql.General>(srcHttpToJson);

            Sql.Insert(new Query<Dataset.Sql.General>(srcJsonToSql), "general");

            await Sql.DisconnectAsync();
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
        #region Methods

        public SqlAdapterOfSARSCoV2() : base(SQL_SERVER, SQL_USERNAME, SQL_PASSWORD, SQL_DATABASE) => Expression.Empty();

        public override void Delete<T>(Query<T> file, string tableName) => Expression.Empty();
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
        public override List<T> Select<T>(Query<T> file, string tableName) => default;
        public override void Update<T>(Query<T> file, string tableName) => Expression.Empty();

        #endregion
    }

    public static class Global
    {
        #region Properties

        public const char EXIT_CODE = 'E';
        public const int SCHEDULED_JOB_INTERVAL = 300;
        public const NullValueHandling NULL_VALUE_HANDLING = NullValueHandling.Ignore;

        public const string SQL_SERVER = "127.0.0.1";
        public const string SQL_USERNAME = "root";
        public const string SQL_PASSWORD = "8965";
        public const string SQL_DATABASE = "live.sarscov2";

        public readonly static string APP_NAME = Assembly.GetExecutingAssembly().GetName().Name.ToString();
        public readonly static string APP_VERSION = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        #endregion

        #region Classes

        public class JSON
        {
            // initialize the mapper
            public static readonly MapperConfiguration Statistics = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Dataset.Http.General, Dataset.Json.Statistics>()
                .ForPath(tar => tar.Cases, src => src.MapFrom(src => src.Cases))
                .ForPath(tar => tar.Deaths, src => src.MapFrom(src => src.Deaths))
                .ForPath(tar => tar.Recovered, src => src.MapFrom(src => src.Recovered));
            });

            // initialize the mapper
            public static readonly MapperConfiguration General = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Dataset.Http.General, Dataset.Json.General>()
                .ForMember(tar => tar.Updated, src => src.MapFrom(src => src.Updated))
                .ForMember(tar => tar.Statistics, src => src.MapFrom(sub => Statistics.CreateMapper().Map<Dataset.Http.General, Dataset.Json.Statistics>(sub)));
            });

            // initialize the mapper
            public static readonly MapperConfiguration Country = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Dataset.Http.Country, Dataset.Json.Country>()
                .ForPath(tar => tar.Updated, src => src.MapFrom(src => src.Updated))

                .ForPath(tar => tar.DomainInfo.Domain, src => src.MapFrom(src => src.Domain))
                .ForPath(tar => tar.DomainInfo.Province, src => src.MapFrom(src => src.Province))
                .ForPath(tar => tar.DomainInfo.ISO2, src => src.MapFrom(src => GetCountryInfo(src.Domain).TwoLetterCode))
                .ForPath(tar => tar.DomainInfo.ISO3, src => src.MapFrom(src => GetCountryInfo(src.Domain).ThreeLetterCode))
                .ForPath(tar => tar.DomainInfo.Latitude, src => src.MapFrom(src => src.Coordinates.Latitude))
                .ForPath(tar => tar.DomainInfo.Longitude, src => src.MapFrom(src => src.Coordinates.Longitude))

                .ForPath(tar => tar.Statistics.Cases, src => src.MapFrom(src => src.Statistics.Cases))
                .ForPath(tar => tar.Statistics.Deaths, src => src.MapFrom(src => src.Statistics.Deaths))
                .ForPath(tar => tar.Statistics.Recovered, src => src.MapFrom(src => src.Statistics.Recovered));
            });

            // initialize the mapper
            public static readonly MapperConfiguration Historical = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Dataset.Http.Historical, Dataset.Json.Historical>()
                .ForPath(tar => tar.DomainInfo.Domain, src => src.MapFrom(src => src.Domain))
                .ForPath(tar => tar.DomainInfo.Province, src => src.MapFrom(src => src.Province))
                .ForPath(tar => tar.DomainInfo.ISO2, src => src.MapFrom(src => GetCountryInfo(src.Domain).TwoLetterCode))
                .ForPath(tar => tar.DomainInfo.ISO3, src => src.MapFrom(src => GetCountryInfo(src.Domain).ThreeLetterCode))

                .ForPath(tar => tar.Timeline.Cases, src => src.MapFrom(src => src.Timeline.Cases))
                .ForPath(tar => tar.Timeline.Deaths, src => src.MapFrom(src => src.Timeline.Deaths))
                .ForPath(tar => tar.Timeline.Recovered, src => src.MapFrom(src => src.Timeline.Recovered));
            });
        }

        public class SQL
        {
            // initialize the mapper
            public static readonly MapperConfiguration General = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Dataset.Json.General, Dataset.Sql.General>()
                .ForPath(tar => tar.Updated, src => src.MapFrom(src => src.Updated))
                .ForPath(tar => tar.Content, src => src.MapFrom(src => JsonConvert.SerializeObject(src)));
            });

            // initialize the mapper
            public static readonly MapperConfiguration Country = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Dataset.Json.Country, Dataset.Sql.Country>()
                .ForPath(tar => tar.Updated, src => src.MapFrom(src => src.Updated))
                .ForPath(tar => tar.CountryISO2, src => src.MapFrom(src => GetCountryInfo(src.DomainInfo.ISO2).TwoLetterCode))
                .ForPath(tar => tar.CountryISO3, src => src.MapFrom(src => GetCountryInfo(src.DomainInfo.ISO3).ThreeLetterCode))
                .ForPath(tar => tar.Province, src => src.MapFrom(src => src.DomainInfo.Province))
                .ForPath(tar => tar.Content, src => src.MapFrom(src => JsonConvert.SerializeObject(src)));
            });

            // initialize the mapper
            public static readonly MapperConfiguration Historical = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Dataset.Json.Historical, Dataset.Sql.Historical>()
                .ForPath(tar => tar.CountryISO2, src => src.MapFrom(src => GetCountryInfo(src.DomainInfo.ISO2).TwoLetterCode))
                .ForPath(tar => tar.CountryISO3, src => src.MapFrom(src => GetCountryInfo(src.DomainInfo.ISO3).ThreeLetterCode))
                .ForPath(tar => tar.Province, src => src.MapFrom(src => src.DomainInfo.Province))
                .ForPath(tar => tar.Content, src => src.MapFrom(src => JsonConvert.SerializeObject(src)));
            });
        }

        #endregion

        #region Methods

        private static ISO3166.Country GetCountryInfo(string country)
        {
            var result1 = ISO3166.Country.List.FirstOrDefault(src => src.Name == country);
            var result2 = ISO3166.Country.List.FirstOrDefault(src => src.TwoLetterCode == country);
            var result3 = ISO3166.Country.List.FirstOrDefault(src => src.ThreeLetterCode == country);
            var result4 = ISO3166.Country.List.FirstOrDefault(src => src.NumericCode == country);

            return result1 != null ? result1 :
                (result2 != null ? result2 :
                (result3 != null ? result3 :
                (result4 != null ? result4 : null)));
        }

        #endregion
    }
}

using AutoMapper;
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
            General, Informational, Initialize,
            Read, Write,
            Error, Succesfull,
        }

        public class Automapper
        {
            // initialize the mapper
            public readonly MapperConfiguration General = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Dataset.Http.General, Dataset.Sql.General>()
                .ForMember(tar => tar.Updated, src => src.MapFrom(src => src.Updated))

                .ForMember(tar => tar.Statistics.Cases, src => src.MapFrom(src => src.Cases))
                .ForMember(tar => tar.Statistics.Deaths, src => src.MapFrom(src => src.Deaths))
                .ForMember(tar => tar.Statistics.Recovered, src => src.MapFrom(src => src.Recovered));
            });


            // initialize the mapper
            public readonly MapperConfiguration CountryV1 = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Dataset.Http.CountryV1, Dataset.Sql.CountryV1>()
                .ForMember(tar => tar.Updated, src => src.MapFrom(src => src.Updated))

                .ForMember(tar => tar.DomainInfo.Domain, src => src.MapFrom(src => src.DomainInfo.Domain))
                .ForMember(tar => tar.DomainInfo.ISO2, src => src.MapFrom(src => src.DomainInfo.ISO2))
                .ForMember(tar => tar.DomainInfo.ISO3, src => src.MapFrom(src => src.DomainInfo.ISO3))

                .ForMember(tar => tar.DomainInfo.Latitude, src => src.MapFrom(src => src.DomainInfo.Latitude))
                .ForMember(tar => tar.DomainInfo.Longitude, src => src.MapFrom(src => src.DomainInfo.Longitude))

                .ForMember(tar => tar.PerOneMillion.Cases, src => src.MapFrom(src => src.CasesPerOneMillion))
                .ForMember(tar => tar.PerOneMillion.Deaths, src => src.MapFrom(src => src.DeathsPerOneMillion))

                .ForMember(tar => tar.Today.Cases, src => src.MapFrom(src => src.TodayCases))
                .ForMember(tar => tar.Today.Deaths, src => src.MapFrom(src => src.TodayDeaths));
            });

            // initialize the mapper
            public readonly MapperConfiguration CountryV2 = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Dataset.Http.CountryV2, Dataset.Sql.CountryV2>()
                .ForMember(tar => tar.Updated, src => src.MapFrom(src => src.Updated))

                .ForMember(tar => tar.DomainInfo.Domain, src => src.MapFrom(src => src.Domain))
                .ForMember(tar => tar.DomainInfo.ISO2, src => src.MapFrom(src => Extension.GetCountryInfo(src.Domain).TwoLetterCode))
                .ForMember(tar => tar.DomainInfo.ISO3, src => src.MapFrom(src => Extension.GetCountryInfo(src.Domain).ThreeLetterCode))

                .ForMember(tar => tar.DomainInfo.Latitude, src => src.MapFrom(src => src.Coordinates.Latitude))
                .ForMember(tar => tar.DomainInfo.Longitude, src => src.MapFrom(src => src.Coordinates.Longitude))

                .ForMember(tar => tar.DomainInfo.Province, src => src.MapFrom(src => src.Province))

                .ForMember(tar => tar.Statistics.Cases, src => src.MapFrom(src => src.Statistics.Cases))
                .ForMember(tar => tar.Statistics.Deaths, src => src.MapFrom(src => src.Statistics.Deaths))
                .ForMember(tar => tar.Statistics.Recovered, src => src.MapFrom(src => src.Statistics.Recovered));
            });

            // initialize the mapper
            public readonly MapperConfiguration Historical = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Dataset.Http.Historical, Dataset.Sql.Historical>()
                .ForMember(tar => tar.DomainInfo.Domain, src => src.MapFrom(src => src.Domain))
                .ForMember(tar => tar.DomainInfo.ISO2, src => src.MapFrom(src => Extension.GetCountryInfo(src.Domain).TwoLetterCode))
                .ForMember(tar => tar.DomainInfo.ISO3, src => src.MapFrom(src => Extension.GetCountryInfo(src.Domain).ThreeLetterCode))

                .ForMember(tar => tar.DomainInfo.Province, src => src.MapFrom(src => src.Province))

                .ForMember(tar => tar.Statistics.Cases, src => src.MapFrom(src => src.Timeline.Cases))
                .ForMember(tar => tar.Statistics.Deaths, src => src.MapFrom(src => src.Timeline.Deaths))
                .ForMember(tar => tar.Statistics.Recovered, src => src.MapFrom(src => src.Timeline.Recovered));
            });
        }
    }
}

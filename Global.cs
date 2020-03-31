using AutoMapper;
using ISO3166;
using Newtonsoft.Json;
using System;
using System.Linq;
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

        public class Automapper
        {
            // initialize the mapper
            public readonly MapperConfiguration General = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Dataset.Http.General, Dataset.Json.General>()
                .ForMember(tar => tar.Updated, src => src.MapFrom(src => src.Updated))

                .ForMember(tar => tar.Statistics.Cases, src => src.MapFrom(src => src.Cases))
                .ForMember(tar => tar.Statistics.Deaths, src => src.MapFrom(src => src.Deaths))
                .ForMember(tar => tar.Statistics.Recovered, src => src.MapFrom(src => src.Recovered));
            });

            // initialize the mapper
            public readonly MapperConfiguration CountryV2 = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Dataset.Http.Country, Dataset.Json.Country>()
                .ForMember(tar => tar.Updated, src => src.MapFrom(src => src.Updated))

                .ForMember(tar => tar.DomainInfo.Domain, src => src.MapFrom(src => src.Domain))
                .ForMember(tar => tar.DomainInfo.Province, src => src.MapFrom(src => src.Province))
                .ForMember(tar => tar.DomainInfo.ISO2, src => src.MapFrom(src => GetCountryInfo(src.Domain).TwoLetterCode))
                .ForMember(tar => tar.DomainInfo.ISO3, src => src.MapFrom(src => GetCountryInfo(src.Domain).ThreeLetterCode))
                .ForMember(tar => tar.DomainInfo.Latitude, src => src.MapFrom(src => src.Coordinates.Latitude))
                .ForMember(tar => tar.DomainInfo.Longitude, src => src.MapFrom(src => src.Coordinates.Longitude))

                .ForMember(tar => tar.Statistics.Cases, src => src.MapFrom(src => src.Statistics.Cases))
                .ForMember(tar => tar.Statistics.Deaths, src => src.MapFrom(src => src.Statistics.Deaths))
                .ForMember(tar => tar.Statistics.Recovered, src => src.MapFrom(src => src.Statistics.Recovered));
            });

            // initialize the mapper
            public readonly MapperConfiguration Historical = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Dataset.Http.Historical, Dataset.Json.Historical>()
                .ForMember(tar => tar.DomainInfo.Domain, src => src.MapFrom(src => src.Domain))
                .ForMember(tar => tar.DomainInfo.Province, src => src.MapFrom(src => src.Province))
                .ForMember(tar => tar.DomainInfo.ISO2, src => src.MapFrom(src => GetCountryInfo(src.Domain).TwoLetterCode))
                .ForMember(tar => tar.DomainInfo.ISO3, src => src.MapFrom(src => GetCountryInfo(src.Domain).ThreeLetterCode))

                .ForMember(tar => tar.Timeline.Cases, src => src.MapFrom(src => src.Timeline.Cases))
                .ForMember(tar => tar.Timeline.Deaths, src => src.MapFrom(src => src.Timeline.Deaths))
                .ForMember(tar => tar.Timeline.Recovered, src => src.MapFrom(src => src.Timeline.Recovered));
            });

            public static Country GetCountryInfo(string country)
            {
                var result1 = Country.List.FirstOrDefault(src => src.Name == country);
                var result2 = Country.List.FirstOrDefault(src => src.TwoLetterCode == country);
                var result3 = Country.List.FirstOrDefault(src => src.ThreeLetterCode == country);
                var result4 = Country.List.FirstOrDefault(src => src.NumericCode == country);

                return result1 != null ? result1 :
                    (result2 != null ? result2 :
                    (result3 != null ? result3 :
                    (result4 != null ? result4 : null)));
            }
        }
    }
}

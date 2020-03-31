using AutoMapper;
using live.SARSCoV2.Dataset.Http;
using live.SARSCoV2.Dataset.Sql;
using System;
using static live.SARSCoV2.Global;

namespace live.SARSCoV2
{
    public static class Extension
    {
        #region Properties

        private static readonly object Chain = new object();

        private static bool IsVisibleMessage { get; set; }

        #endregion

        #region Console

        public static void PrintMessage(string message, JobType type)
        {
            lock (Chain)
            {
                // print date and time
                PrintMessage(string.Format("{0}<{1}>: ", DateTime.Now.ToString("yyyy/MM/dd-h:mm:ss"), DOMAIN_USERNAME), false);

                switch (type)
                {
                    default:
                    case JobType.General:
                        PrintMessage(message);
                        break;

                    case JobType.Informational:
                        PrintMessage(message, ConsoleColor.Blue);
                        break;

                    case JobType.Initialize:
                        PrintMessage(string.Format("TASK_INT:{0}", message), ConsoleColor.Yellow);
                        break;

                    case JobType.Read:
                        PrintMessage(string.Format("HTTP_REA:{0}", message), ConsoleColor.DarkMagenta);
                        break;

                    case JobType.Write:
                        PrintMessage(string.Format("TASK_WRI:{0}", message), ConsoleColor.Magenta);
                        break;

                    case JobType.Error:
                        PrintMessage(string.Format("TASK_ERR:{0}", message), ConsoleColor.Red);
                        break;

                    case JobType.Succesfull:
                        PrintMessage(string.Format("TASK_SUC:{0}", message), ConsoleColor.Green);
                        break;
                }
            }
        }
        public static void PrintMessage(string message, ConsoleColor color, bool newLine = true)
        {
            Console.ForegroundColor = color;
            PrintMessage(message, newLine);
            Console.ResetColor();
        }
        public static void PrintMessage(string message, bool newLine = true)
        {
            if (!IsVisibleMessage)
                return;

            if (newLine)
                Console.WriteLine("{0}", message);
            else
                Console.Write("{0}", message);
        }

        public static string ReadMessage() => Console.ReadLine();
        public static char ReadChar() => Console.ReadKey().KeyChar;

        public static void SetVisibleMessage(bool flag = true) => IsVisibleMessage = flag;
        public static bool GetVisibleMessage() => IsVisibleMessage;

        #endregion

        #region Methods

        public static void PrintAppInfo()
        {
            PrintMessage(string.Format("{0} {1}",
                APP_NAME, APP_VERSION, DOMAIN_USERNAME), JobType.Informational);

            PrintMessage(string.Format("Exit code: {0}, Interval: {1}, Null Value Handling: {2}",
                EXIT_CODE, SCHEDULED_JOB_INTERVAL, NULL_VALUE_HANDLING), JobType.Informational);
        }

        public static void InitAutomapper()
        {
            // initialize the mapper
            var configofGeneral = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Dataset.Http.General, Dataset.Sql.General>()
                .ForMember(tar => tar.Updated,              sor => sor.MapFrom(src => src.Updated))
                
                .ForMember(tar => tar.Statistics.Cases,     sor => sor.MapFrom(src => src.Cases))
                .ForMember(tar => tar.Statistics.Deaths,    sor => sor.MapFrom(src => src.Deaths))
                .ForMember(tar => tar.Statistics.Recovered, sor => sor.MapFrom(src => src.Recovered));
            });


            // initialize the mapper
            var configofCountry1 = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Dataset.Http.CountryV1, Dataset.Sql.CountryV1>()
                .ForMember(tar => tar.Updated,              sor => sor.MapFrom(src => src.Updated))
                
                .ForMember(tar => tar.DomainInfo.Domain,    sor => sor.MapFrom(src => src.DomainInfo.Domain))
                .ForMember(tar => tar.DomainInfo.ISO2,      sor => sor.MapFrom(src => src.DomainInfo.ISO2))
                .ForMember(tar => tar.DomainInfo.ISO3,      sor => sor.MapFrom(src => src.DomainInfo.ISO3))
                .ForMember(tar => tar.DomainInfo.Latitude,  sor => sor.MapFrom(src => src.DomainInfo.Latitude))
                .ForMember(tar => tar.DomainInfo.Longitude, sor => sor.MapFrom(src => src.DomainInfo.Longitude))
                
                .ForMember(tar => tar.PerOneMillion.Cases,  sor => sor.MapFrom(src => src.CasesPerOneMillion))
                .ForMember(tar => tar.PerOneMillion.Deaths, sor => sor.MapFrom(src => src.DeathsPerOneMillion))
                
                .ForMember(tar => tar.Today.Cases,          sor => sor.MapFrom(src => src.TodayCases))
                .ForMember(tar => tar.Today.Deaths,         sor => sor.MapFrom(src => src.TodayDeaths));
            });

            // initialize the mapper
            var configofCountry2 = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Dataset.Http.CountryV2, Dataset.Sql.CountryV2>()
                .ForMember(tar => tar.Updated, sor => sor.MapFrom(src => src.Updated))

                .ForMember(tar => tar.DomainInfo.Domain,    sor => sor.MapFrom(src => src.Domain))
                .ForMember(tar => tar.DomainInfo.Province,  sor => sor.MapFrom(src => src.Province))
                //.ForMember(tar => tar.DomainInfo.ISO2, sor => sor.MapFrom(src => src.DomainInfo.ISO2))
                //.ForMember(tar => tar.DomainInfo.ISO3, sor => sor.MapFrom(src => src.DomainInfo.ISO3))
                .ForMember(tar => tar.DomainInfo.Latitude,  sor => sor.MapFrom(src => src.Coordinates.Latitude))
                .ForMember(tar => tar.DomainInfo.Longitude, sor => sor.MapFrom(src => src.Coordinates.Longitude))

                .ForMember(tar => tar.Statistics.Cases,     sor => sor.MapFrom(src => src.Statistics.Cases))
                .ForMember(tar => tar.Statistics.Deaths,    sor => sor.MapFrom(src => src.Statistics.Deaths))
                .ForMember(tar => tar.Statistics.Recovered, sor => sor.MapFrom(src => src.Statistics.Recovered));
            });

            // initialize the mapper
            var configofHistorical = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Dataset.Http.Historical, Dataset.Sql.Historical>()
                .ForMember(tar => tar.DomainInfo.Domain,    sor => sor.MapFrom(src => src.Domain))
                .ForMember(tar => tar.DomainInfo.Province,  sor => sor.MapFrom(src => src.Province))
                //.ForMember(tar => tar.DomainInfo.ISO2, sor => sor.MapFrom(src => src.DomainInfo.ISO2))
                //.ForMember(tar => tar.DomainInfo.ISO3, sor => sor.MapFrom(src => src.DomainInfo.ISO3))
                //.ForMember(tar => tar.DomainInfo.Latitude, sor => sor.MapFrom(src => src.DomainInfo.Latitude))
                //.ForMember(tar => tar.DomainInfo.Longitude, sor => sor.MapFrom(src => src.DomainInfo.Longitude))

                .ForMember(tar => tar.Statistics.Cases,     sor => sor.MapFrom(src => src.Timeline.Cases))
                .ForMember(tar => tar.Statistics.Deaths,    sor => sor.MapFrom(src => src.Timeline.Deaths))
                .ForMember(tar => tar.Statistics.Recovered, sor => sor.MapFrom(src => src.Timeline.Recovered));
            });
        }

        #endregion
    }
}

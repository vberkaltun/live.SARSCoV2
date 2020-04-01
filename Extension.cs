using System.Linq;
using Newtonsoft.Json;
using CountryISO = ISO3166.Country;
using Json = live.SARSCoV2.Dataset.Json;
using Http = live.SARSCoV2.Dataset.Http;
using Sql = live.SARSCoV2.Dataset.Sql;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace live.SARSCoV2
{
    static class Extension
    {
        #region Converter JSON

        public static Json.Country ToJson(this Http.Country var)
        {
            string ISO2 = GetCountryInfo(var.Domain).TwoLetterCode;
            string ISO3 = GetCountryInfo(var.Domain).ThreeLetterCode;

            var DomainInfo = new Json.CountryInfo
            {
                Domain = var.Domain,
                Province = var.Province,
                ISO2 = ISO2,
                ISO3 = ISO3
            };
            var  Coordinates = new Json.Coordinates
            {
                Latitude = var.Coordinates.Latitude,
                Longitude = var.Coordinates.Longitude
            };
            var Statistics = new Json.Statistics
            {
                Cases = var.Statistics.Cases,
                Deaths = var.Statistics.Deaths,
                Recovered = var.Statistics.Recovered
            };

            return new Json.Country
            {
                Updated = DateTime.Parse(var.Updated).ToUnixTime(),
                DomainInfo = DomainInfo,
                Coordinates = Coordinates,
                Statistics = Statistics
            };
        }
        public static Json.General ToJson(this Http.General var)
        {
            return new Json.General
            {
                Updated = var.Updated,
                Statistics = new Json.Statistics
                {
                    Cases = var.Cases,
                    Deaths = var.Deaths,
                    Recovered = var.Recovered
                }
            };
        }
        public static Json.Historical ToJson(this Http.Historical var)
        {
            var ISO2 = GetCountryInfo(var.Domain).TwoLetterCode;
            var ISO3 = GetCountryInfo(var.Domain).ThreeLetterCode;

            var DomainInfo = new Json.CountryInfo
            {
                Domain = var.Domain,
                Province = var.Province,
                ISO2 = ISO2,
                ISO3 = ISO3
            };
            var Timeline = new Json.Timeline
            {
                Cases = var.Timeline.Cases,
                Deaths = var.Timeline.Deaths,
                Recovered = var.Timeline.Recovered
            };

            return new Json.Historical
            {
                DomainInfo = DomainInfo,
                Timeline = Timeline
            };
        }

        #endregion

        #region Converter SQL

        public static Sql.Country ToSql(this Json.Country var)
        {
            return new Sql.Country
            {
                Updated = var.Updated,
                Domain = var.DomainInfo.Domain,
                Province = var.DomainInfo.Province,
                DomainISO2 = var.DomainInfo.ISO2,
                DomainISO3 = var.DomainInfo.ISO3,
                Content = JsonConvert.SerializeObject(var, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
            };
        }
        public static Sql.General ToSql(this Json.General var)
        {
            return new Sql.General
            {
                Updated = var.Updated,
                Content = JsonConvert.SerializeObject(var, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
            };
        }
        public static Sql.Historical ToSql(this Json.Historical var)
        {
            return new Sql.Historical
            {
                Domain = var.DomainInfo.Domain,
                DomainISO2 = var.DomainInfo.ISO2,
                DomainISO3 = var.DomainInfo.ISO3,
                Province = var.DomainInfo.Province,
                Content = JsonConvert.SerializeObject(var, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
            };
        }

        #endregion

        #region Other Methods

        public static CountryISO GetCountryInfo(string country)
        {
            // remove in brackets
            var names = GetCountryNames(country);

            foreach (var item in names)
            {
                var result1 = CountryISO.List.FirstOrDefault(src => src.Name.Contains(country));
                var result2 = CountryISO.List.FirstOrDefault(src => src.TwoLetterCode.Contains(country));
                var result3 = CountryISO.List.FirstOrDefault(src => src.ThreeLetterCode.Contains(country));
                var result4 = CountryISO.List.FirstOrDefault(src => src.NumericCode.Contains(country));

                if (result1 != null) return result1;
                if (result2 != null) return result2;
                if (result3 != null) return result3;
                if (result4 != null) return result4;
            }

            return default;
        }
        public static List<string> GetCountryNames(string country)
        {
            List<string> result = new List<string>();

            // remove in brackets
            result.Add(Regex.Replace(country, @" ?\(.*?\)", string.Empty));
            result.Add(Regex.Replace(country, @"rect64\(([a-f0-9]+)\)", string.Empty));

            return result;
        }

        public static long ToUnixTime(this DateTime date)
        {
            date = date.ToUniversalTime();
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return Convert.ToInt64((date - epoch).TotalSeconds);
        }

        #endregion
    }
}

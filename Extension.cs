using System.Linq;
using Newtonsoft.Json;
using CountryISO = ISO3166.Country;
using Json = live.SARSCoV2.Dataset.Json;
using Http = live.SARSCoV2.Dataset.Http;
using Sql = live.SARSCoV2.Dataset.Sql;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace live.SARSCoV2
{
    static class Extension
    {
        #region Converter JSON

        public static Json.Country ToJson(this Http.Country var)
        {
            var ISO = GetCountryInfo(var.Domain);

            if (ISO == null)
                return null;

            return new Json.Country
            {
                Updated = DateTime.Parse(var.Updated).ToUnixTime(),
                DomainInfo = new Json.CountryInfo
                {
                    Domain = var.Domain,
                    Province = var.Province,
                    ISO2 = ISO.TwoLetterCode,
                    ISO3 = ISO.TwoLetterCode,
                },
                Coordinates = new Json.Coordinates
                {
                    Latitude = var.Coordinates.Latitude,
                    Longitude = var.Coordinates.Longitude
                },
                Statistics = new Json.Statistics
                {
                    Cases = var.Statistics.Cases,
                    Deaths = var.Statistics.Deaths,
                    Recovered = var.Statistics.Recovered
                }
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
            var ISO = GetCountryInfo(var.Domain);

            if (ISO == null)
                return null;

            return new Json.Historical
            {
                DomainInfo = new Json.CountryInfo
                {
                    Domain = var.Domain,
                    Province = var.Province,
                    ISO2 = ISO.TwoLetterCode,
                    ISO3 = ISO.TwoLetterCode
                },
                Timeline = new Json.Timeline
                {
                    Cases = var.Timeline.Cases,
                    Deaths = var.Timeline.Deaths,
                    Recovered = var.Timeline.Recovered
                }
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
                Content = JsonConvert.SerializeObject(var)
            };
        }
        public static Sql.General ToSql(this Json.General var)
        {
            return new Sql.General
            {
                Updated = var.Updated,
                Content = JsonConvert.SerializeObject(var)
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
                Content = JsonConvert.SerializeObject(var)
            };
        }

        #endregion

        #region Other Methods

        public static CountryISO GetCountryInfo(string country)
        {
            // fix names and remove in brackets
            var names = GetCountryNames(FixCountryNames(country));

            foreach (var name in names)
            {
                foreach (var item in CountryISO.List)
                {
                    if (item.Name.Contains(name))
                        return item;
                    if (item.TwoLetterCode.Contains(name))
                        return item;
                    if (item.ThreeLetterCode.Contains(name))
                        return item;
                    if (item.NumericCode.Contains(name))
                        return item;
                }
            }

            return null;
        }
        public static string FixCountryNames(string country)
        {
            // add all name hot fix to here 
            country = country.Replace("Burma", "Myanmar");
            country = country.Replace("Côte d'Ivoire", "384");
            country = country.Replace("Cote d'Ivoire", "384");

            return country;
        }
        public static List<string> GetCountryNames(string country)
        {
            List<string> result = new List<string>();

            string output1 = Regex.Match(country, @"\(([^)]*)\)").Groups[1].Value;
            string output2 = Regex.Replace(country, @" ?\(.*?\)", string.Empty);

            // remove in brackets
            if (output1 != string.Empty) result.Add(output1);
            if (output2 != string.Empty) result.Add(output2);

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

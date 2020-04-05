using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using CountryISO = ISO3166.Country;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;
using live.SARSCoV2.Module.Property;
using live.SARSCoV2.Module.SqlAdapter;
using Json = live.SARSCoV2.Dataset.Json;
using Http = live.SARSCoV2.Dataset.Http;
using Sql = live.SARSCoV2.Dataset.Sql;
using Nager.Country;

namespace live.SARSCoV2
{
    static class Extension
    {
        #region Converter JSON

        public static Json.CountryV1 ToJson(this Http.CountryV1 var)
        {
            var ISO = GetCountryInfo(var.Domain);
            var region = ISO?.GetCountryInfo();

            if (ISO == null || region == null)
                return null;

            return new Json.CountryV1
            {
                Updated = DateTime.Parse(var.Updated).ToUnixTime().ToString(),
                DomainInfo = new Json.CountryInfo
                {
                    Region = Enum.GetName(typeof(Region), region.Region),
                    Domain = ISO.Name,
                    Province = var.Province,
                    ISO2 = ISO.TwoLetterCode,
                    ISO3 = ISO.ThreeLetterCode,
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
        public static Json.CountryV2 ToJson(this Http.CountryV2 var)
        {
            var ISO = GetCountryInfo(var.Domain);
            var region = ISO?.GetCountryInfo();

            if (ISO == null || region == null)
                return null;

            return new Json.CountryV2
            {
                Updated = var.Updated.ToString(),
                DomainInfo = new Json.CountryInfo
                {
                    Region = Enum.GetName(typeof(Region), region.Region),
                    Domain = ISO.Name,
                    ISO2 = ISO.TwoLetterCode,
                    ISO3 = ISO.ThreeLetterCode,
                },
                Coordinates = new Json.Coordinates
                {
                    Latitude = var.DomainInfo.Latitude,
                    Longitude = var.DomainInfo.Longitude
                },
                Statistics = new Json.Statistics
                {
                    Cases = var.Cases,
                    Deaths = var.Deaths,
                    Recovered = var.Recovered
                },
                Today = new Json.Statistics
                {
                    Cases = var.TodayCases,
                    Deaths = var.TodayDeaths
                },
                PerOneMillion = new Json.Statistics
                {
                    Cases = var.CasesPerOneMillion,
                    Deaths = var.DeathsPerOneMillion
                }
            };
        }
        public static Json.General ToJson(this Http.General var)
        {
            return new Json.General
            {
                Updated = var.Updated.ToString(),
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
            var region = ISO?.GetCountryInfo();

            if (ISO == null || region == null)
                return null;

            return new Json.Historical
            {
                Updated = DateTime.UtcNow.ToUnixTime().ToString(),
                DomainInfo = new Json.CountryInfo
                {
                    Region = Enum.GetName(typeof(Region), region.Region),
                    Domain = ISO.Name,
                    Province = var.Province,
                    ISO2 = ISO.TwoLetterCode,
                    ISO3 = ISO.ThreeLetterCode
                },
                Timeline = new Json.Timeline
                {
                    Cases = var.Timeline.Cases,
                    Deaths = var.Timeline.Deaths,
                    Recovered = var.Timeline.Recovered
                }
            };
        }
        public static Json.State ToJson(this Http.State var)
        {
            var ISO = GetCountryInfo("USA");

            if (ISO == null)
                return null;

            return new Json.State
            {
                Updated = DateTime.UtcNow.ToUnixTime().ToString(),
                DomainInfo = new Json.CountryInfo
                {
                    Domain = ISO.Name,
                    Province = var.Province,
                    ISO2 = ISO.TwoLetterCode,
                    ISO3 = ISO.ThreeLetterCode
                },
                Statistics = new Json.Statistics
                {
                    Cases = var.Cases,
                    Deaths = var.Deaths,
                }
            };
        }

        #endregion

        #region Converter SQL

        public static Sql.CountryV1 ToSql(this Json.CountryV1 var)
        {
            string updated = var.Updated.ToString();
            var.Updated = null;

            return new Sql.CountryV1
            {
                Updated = string.Format("{0}.{1}", var.DomainInfo.ISO3, updated),
                Region = var.DomainInfo.Region,
                Domain = var.DomainInfo.Domain,
                Province = var.DomainInfo.Province,
                DomainISO2 = var.DomainInfo.ISO2,
                DomainISO3 = var.DomainInfo.ISO3,
                Content = JsonConvert.SerializeObject(var, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
            };
        }
        public static Sql.CountryV2 ToSql(this Json.CountryV2 var)
        {
            string updated = var.Updated.ToString();
            var.Updated = null;

            return new Sql.CountryV2
            {
                Updated = string.Format("{0}.{1}", var.DomainInfo.ISO3, updated),
                Region = var.DomainInfo.Region,
                Domain = var.DomainInfo.Domain,
                DomainISO2 = var.DomainInfo.ISO2,
                DomainISO3 = var.DomainInfo.ISO3,
                Content = JsonConvert.SerializeObject(var, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
            };
        }
        public static Sql.General ToSql(this Json.General var)
        {
            string updated = var.Updated.ToString();
            var.Updated = null;

            return new Sql.General
            {
                Updated = updated,
                Content = JsonConvert.SerializeObject(var, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
            };
        }
        public static Sql.Historical ToSql(this Json.Historical var)
        {
            string updated = var.Updated.ToString();
            var.Updated = null;

            return new Sql.Historical
            {
                Updated = updated,
                Region = var.DomainInfo.Region,
                Domain = var.DomainInfo.Domain,
                DomainISO2 = var.DomainInfo.ISO2,
                DomainISO3 = var.DomainInfo.ISO3,
                Province = var.DomainInfo.Province,
                Content = JsonConvert.SerializeObject(var, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
            };
        }
        public static Sql.State ToSql(this Json.State var)
        {
            string updated = var.Updated.ToString();
            var.Updated = null;

            return new Sql.State
            {
                Updated = updated,
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
            // fix names and remove in brackets
            var names = GetCountryNames(FixCountryNames(country));

            foreach (var name in names)
            {
                foreach (var item in CountryISO.List)
                {
                    if (item.Name.Contains(name) && name.Length > 3)
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
        public static ICountryInfo GetCountryInfo(this CountryISO country)
        {
            return new CountryProvider().GetCountry(country.ThreeLetterCode);
        }

        public static string FixCountryNames(string country)
        {
            // add all name hot-fix to here 
            country = country.Replace("Burma", "MMR");
            country = country.Replace("Côte d'Ivoire", "CIV");
            country = country.Replace("Cote d'Ivoire", "CIV");
            country = country.Replace("Korea, South", "KOR");
            country = country.Replace("S. Korea", "KOR");
            country = country.Replace("Laos", "LAO");
            country = country.Replace("Taiwan*", "TWN");
            country = country.Replace("Vietnam", "VNM");
            country = country.Replace("Libyan Arab Jamahiriya", "LBY");
            country = country.Replace("UAE", "ARE");
            country = country.Replace("Swaziland", "SWZ");
            country = country.Replace("DRC", "COD");
            country = country.Replace("Caribbean Netherlands", "BES");
            country = country.Replace("British Virgin Islands", "VGB");
            country = country.Replace("St. Barth", "BLM");
            country = country.Replace("Saint Barthélemy", "BLM");

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
        public static string ToHexString(this byte[] array)
        {
            StringBuilder hex = new StringBuilder(array.Length * 2);
            foreach (byte index in array)
                hex.AppendFormat("{0:x2}", index);

            return hex.ToString();
        }

        #endregion
    }

    static class SqlCommand
    {
        #region Insert

        private static Dictionary<string, object> GetProperties(Sql.CountryV1 item)
            => new Property<Sql.CountryV1>(item).GetProperties();
        private static Dictionary<string, object> GetProperties(Sql.CountryV2 item)
            => new Property<Sql.CountryV2>(item).GetProperties();
        private static Dictionary<string, object> GetProperties(Sql.General item)
            => new Property<Sql.General>(item).GetProperties();
        private static Dictionary<string, object> GetProperties(Sql.Historical item)
            => new Property<Sql.Historical>(item).GetProperties();
        private static Dictionary<string, object> GetProperties(Sql.State item)
            => new Property<Sql.State>(item).GetProperties();

        public static void Insert(this SqlAdapter sqlClient, Sql.CountryV1 file, string tableName, string comparer)
            => Insert(sqlClient, GetProperties(file), tableName, comparer);
        public static void Insert(this SqlAdapter sqlClient, Sql.CountryV2 file, string tableName, string comparer)
            => Insert(sqlClient, GetProperties(file), tableName, comparer);
        public static void Insert(this SqlAdapter sqlClient, Sql.General file, string tableName, string comparer)
            => Insert(sqlClient, GetProperties(file), tableName, comparer);
        public static void Insert(this SqlAdapter sqlClient, Sql.Historical file, string tableName, string comparer)
            => Insert(sqlClient, GetProperties(file), tableName, comparer);
        public static void Insert(this SqlAdapter sqlClient, Sql.State file, string tableName, string comparer)
            => Insert(sqlClient, GetProperties(file), tableName, comparer);

        private static void Insert(this SqlAdapter sqlClient, Dictionary<string, object> keyValuePairs, string tableName, string whereNotExists)
        {
            var template = @"INSERT INTO " + tableName + @"({0}) select {1} " +
                @"WHERE NOT EXISTS (Select " + whereNotExists + @" From " + tableName + @" where " + whereNotExists + @" = @" + whereNotExists + @")";

            string target = string.Join(", ", keyValuePairs.Keys.ToArray()).Trim();
            string source = "@" + string.Join(", @", keyValuePairs.Keys.ToArray()).Trim();

            MySqlCommand command = new MySqlCommand(string.Format(template, target, source), sqlClient.Connection);
            command.Prepare();

            foreach (var item in keyValuePairs)
                command.Parameters.AddWithValue(string.Format("@{0}", item.Key), item.Value);

            try { command.ExecuteNonQuery(); } catch { }
        }

        #endregion

        #region Update

        public static void Update(this SqlAdapter sqlClient, Sql.Historical file, string tableName)
        {
            var template = @"UPDATE " + tableName + @" SET Content = '" + file.Content + @"' WHERE DomainISO3 = " + file.DomainISO3;

            MySqlCommand command = new MySqlCommand(template, sqlClient.Connection);
            command.Prepare();

            try { command.ExecuteNonQuery(); } catch { }
        }

        #endregion
    }
}

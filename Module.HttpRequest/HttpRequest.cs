using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using live.SARSCoV2.Module.Base;
using static live.SARSCoV2.Global;

namespace live.SARSCoV2.Module.HttpRequest
{
    class HttpRequest<T> : Logger, IHttpRequest<T>
    {
        #region Properties

        public static string ClassName => typeof(T).FullName;
        public HttpClient Client { get; private set; }

        #endregion

        #region Methods

        public HttpRequest()
        {
            // print message
            PrintMessage(ClassName, JobType.Initialize);

            Client = new HttpClient();
        }

        public async Task<T> GetAsync(string path)
        {
            // print message
            PrintMessage(ClassName, JobType.Read);

            // read data
            var response = await Client.GetAsync(path);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            // parse and return, do not 
            return JsonConvert.DeserializeObject<T>(responseBody, new JsonSerializerSettings { NullValueHandling = NULL_VALUE_HANDLING });
        }

        #endregion
    }
}
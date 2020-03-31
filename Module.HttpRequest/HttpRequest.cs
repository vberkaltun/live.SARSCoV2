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
        public string Path { get; private set; }

        #endregion

        #region Methods

        public HttpRequest(HttpClient client = null, string path = null)
        {
            // print message
            PrintMessage(ClassName, JobType.Initialize);

            Client = client;
            Path = path;
        }

        public async Task<T> GetAsync()
        {
            // print message
            PrintMessage(ClassName, JobType.Read);

            // read data
            var response = await Client.GetAsync(Path);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            // parse and return, do not 
            return JsonConvert.DeserializeObject<T>(responseBody, new JsonSerializerSettings { NullValueHandling = NULL_VALUE_HANDLING });
        }

        #endregion
    }
}
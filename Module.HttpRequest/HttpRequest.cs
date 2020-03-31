using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using static live.SARSCoV2.Global;

namespace live.SARSCoV2.Module.HttpRequest
{
    partial class HttpRequest<T> : IHttpRequest<T>
    {
        #region Properties

        public string ClassName => typeof(T).FullName;
        public HttpClient Client { get; private set; }

        #endregion

        #region Methods

        public HttpRequest() => Client = new HttpClient();

        public virtual async Task<T> GetAsync(string path)
        {
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
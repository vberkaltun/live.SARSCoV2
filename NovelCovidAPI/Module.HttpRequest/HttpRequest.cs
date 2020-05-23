using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NovelCovidAPI.Module.HttpRequest
{
    public class HttpRequest<T> : IHttpRequest<T>
    {
        #region Properties

        public HttpClient Client { get; private set; }
        public string Path { get; private set; }
        public JsonSerializerSettings SerializerSettings { get; private set; }

        #endregion

        #region Methods

        public HttpRequest(HttpClient client, string path, JsonSerializerSettings serializerSettings)
        {
            Client = client;
            Path = path;
            SerializerSettings = serializerSettings;
        }

        public async Task<T> GetAsync()
        {
            // read data
            var response = await Client.GetAsync(Path);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            // parse and return, do not 
            return JsonConvert.DeserializeObject<T>(responseBody, SerializerSettings);
        }

        #endregion
    }
}
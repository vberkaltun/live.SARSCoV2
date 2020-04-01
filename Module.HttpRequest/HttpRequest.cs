using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using live.SARSCoV2.Module.Base;

namespace live.SARSCoV2.Module.HttpRequest
{
    class HttpRequest<T> : Base.Base, IHttpRequest<T>
    {
        #region Properties

        public static string ClassName => typeof(T).FullName;

        public HttpClient Client { get; private set; }
        public string Path { get; private set; }
        public JsonSerializerSettings SerializerSettings { get; private set; }

        #endregion

        #region Methods

        public HttpRequest(HttpClient client, string path, JsonSerializerSettings serializerSettings)
        {
            // print message
            Logger.Initialize(ClassName);

            Client = client;
            Path = path;
            SerializerSettings = serializerSettings;
        }

        public async Task<T> GetAsync()
        {
            // print message
            Logger.Read(ClassName);

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
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace NovelCovidAPI.Module.HttpRequest
{
    interface IHttpRequest<T>
    {
        HttpClient Client { get; }
        string Path { get; }
        JsonSerializerSettings SerializerSettings { get; }

        Task<T> GetAsync();
    }
}
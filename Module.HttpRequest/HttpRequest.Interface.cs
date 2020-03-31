using System.Net.Http;
using System.Threading.Tasks;

namespace live.SARSCoV2.Module.HttpRequest
{
    interface IHttpRequest<T>
    {
        HttpClient Client { get; }

        Task<T> GetAsync(string path);
    }
}
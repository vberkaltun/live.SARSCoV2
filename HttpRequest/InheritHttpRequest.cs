using System.Threading.Tasks;

namespace live.SARSCoV2.HttpRequest
{
    class InheritHttpRequest<T> : HttpRequest<T>
    {
        public override async Task<T> GetAsync(string path)
        {
            // print message
            Extension.PrintMessage(ClassName, Global.JobType.HTTPRequest);

            // call base
            return await base.GetAsync(path);
        }
    }
}
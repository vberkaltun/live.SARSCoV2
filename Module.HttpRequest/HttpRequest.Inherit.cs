using System.Threading.Tasks;
using static live.SARSCoV2.Global;

namespace live.SARSCoV2.Module.HttpRequest
{
    class InheritHttpRequest<T> : HttpRequest<T>
    {
        public override async Task<T> GetAsync(string path)
        {
            // print message
            Extension.PrintMessage(ClassName, JobType.Read);

            // call base
            return await base.GetAsync(path);
        }
    }
}
using System.Collections.Generic;
using System.Reflection;

namespace live.SARSCoV2.Module.SqlQuery
{
    interface IQuery<T>
    {
        Dictionary<string, object> GetProperties();
        T GetFile();
    }
}

using System.Collections.Generic;

namespace live.SARSCoV2.Module.SqlQuery
{
    interface IQuery<T>
    {
        Dictionary<string, object> GetProperties();
        T GetFile();
    }
}

using System.Collections.Generic;

namespace live.SARSCoV2.Module.Property
{
    interface IProperty<T>
    {
        Dictionary<string, object> GetProperties();
        T GetFile();
    }
}

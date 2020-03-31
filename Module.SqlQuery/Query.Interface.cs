using System.Reflection;

namespace live.SARSCoV2.Module.SqlQuery
{
    interface IQuery<T>
    {
        T File { get; }

        object GetValue(string propertyName);
        PropertyInfo[] GetProperties();
    }
}

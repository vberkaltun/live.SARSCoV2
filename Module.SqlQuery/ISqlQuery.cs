using System.Reflection;

namespace live.SARSCoV2.Module.SqlQuery
{
    interface ISqlQuery
    {
        string ClassName { get; }

        PropertyInfo[] Properties { get; }
        string Command { get;  }
    }
}

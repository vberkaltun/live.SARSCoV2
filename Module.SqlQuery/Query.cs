using System.Reflection;

namespace live.SARSCoV2.Module.SqlQuery
{
    class Query<T> : IQuery<T>
    {
        #region Properties

        public static string ClassName => typeof(T).FullName;

        public T File { get; private set; }

        #endregion

        #region Methods

        public Query(T file) => File = file;

        public object GetValue(string propertyName) => File.GetType().GetProperty(propertyName).GetValue(File, null);
        public PropertyInfo[] GetProperties() => typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        #endregion
    }
}

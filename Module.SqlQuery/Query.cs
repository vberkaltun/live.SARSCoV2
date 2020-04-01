using System.Collections.Generic;
using System.Reflection;

namespace live.SARSCoV2.Module.SqlQuery
{
    class Query<T> : IQuery<T>
    {
        #region Properties

        public static string ClassName => typeof(T).FullName;

        private T File { get; set; }
        private Dictionary<string, object> Properties { get; set; }

        #endregion

        #region Methods

        public Query(T file)
        {
            File = file;
            Properties = new Dictionary<string, object>();
        }

        public T GetFile() => File;
        public Dictionary<string, object> GetProperties()
        {
            var source = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var item in source)
                Properties.Add(item.Name, File.GetType().GetProperty(item.Name).GetValue(File, null));

            return Properties;
        }

        #endregion
    }
}

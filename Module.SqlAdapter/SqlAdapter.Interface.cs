using System.Collections.Generic;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using live.SARSCoV2.Module.SqlQuery;

namespace live.SARSCoV2.Module.SqlAdapter
{
    interface ISqlAdapter
    {
        string Server { get; }
        string Username { get; }
        string Password { get; }
        string Database { get; }

        MySqlConnection Connection { get; }
        bool IsConnected { get; }

        Task ConnectAsync();
        Task DisconnectAsync();

        void Insert<T>(Query<T> file, string tableName);
        List<T> Select<T>(Query<T> file, string tableName);
        void Update<T>(Query<T> file, string tableName);
        void Delete<T>(Query<T> file, string tableName);

        string GetConnectionString();
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using live.SARSCoV2.Module.Property;

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

        string GetConnectionString();
    }
}

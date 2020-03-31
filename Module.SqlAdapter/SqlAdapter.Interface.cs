using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace live.SARSCoV2.Module.SqlAdapter
{
    interface ISqlAdapter
    {
        string ClassName { get; }

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

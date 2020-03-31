using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace live.SARSCoV2.Module.SqlAdapter
{
    interface ISqlAdapter
    {
        Task ConnectAsync();
        Task DisconnectAsync();

        string GetConnectionString();
    }
}

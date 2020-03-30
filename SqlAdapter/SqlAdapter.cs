using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using static live.SARSCoV2.Global;

namespace live.SARSCoV2.Database
{
    class SqlAdapter : ISqlAdapter
    {
        #region Properties

        public string Server { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Database { get; private set; }

        public MySqlConnection Connection { get; private set; }
        public bool IsConnected { get; private set; }

        #endregion

        #region Methods

        public SqlAdapter(string server = SQL_SERVER, string username = SQL_USERNAME,
            string password = SQL_PASSWORD, string database = SQL_DATABASE)
        {
            Server = server;
            Username = username;
            Password = password;
            Database = database;
        }

        public virtual async Task ConnectAsync()
        {
            Connection = new MySqlConnection(GetConnectionString());

            await Connection.OpenAsync();
            IsConnected = true;
        }
        public virtual async Task DisconnectAsync()
        {
            await Connection.CloseAsync();
            IsConnected = false;
        }

        public string GetConnectionString() => string.Format(@"server={0}; uid={1}; pwd={2}; database={3}", Server, Username, Password, Database);

        #endregion
    }
}

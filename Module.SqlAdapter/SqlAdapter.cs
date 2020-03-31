using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using live.SARSCoV2.Module.Base;
using static live.SARSCoV2.Global;

namespace live.SARSCoV2.Module.SqlAdapter
{
    class SqlAdapter : Logger, ISqlAdapter
    {
        #region Properties

        public static string ClassName => typeof(SqlAdapter).FullName;

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
            // print message
            PrintMessage(ClassName, JobType.Initialize);

            Server = server;
            Username = username;
            Password = password;
            Database = database;
        }

        public virtual async Task ConnectAsync()
        {
            // print message
            PrintMessage(ClassName, JobType.Succesfull);

            Connection = new MySqlConnection(GetConnectionString());

            await Connection.OpenAsync();
            IsConnected = true;
        }
        public virtual async Task DisconnectAsync()
        {
            // print message
            PrintMessage(ClassName, JobType.Error);

            await Connection.CloseAsync();
            IsConnected = false;
        }

        public string GetConnectionString() => string.Format(@"server={0}; uid={1}; pwd={2}; database={3}", Server, Username, Password, Database);

        #endregion
    }
}

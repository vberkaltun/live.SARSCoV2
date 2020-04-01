﻿using System.Collections.Generic;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using live.SARSCoV2.Module.Base;
using live.SARSCoV2.Module.SqlQuery;

namespace live.SARSCoV2.Module.SqlAdapter
{
    class SqlAdapter : BaseMember, ISqlAdapter
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

        public SqlAdapter(string server, string username, string password, string database)
        {
            // print message
            Logger.Initialize(ClassName);

            Server = server;
            Username = username;
            Password = password;
            Database = database;
        }

        public async Task ConnectAsync()
        {
            // print message
            Logger.Succesfull(ClassName);

            Connection = new MySqlConnection(GetConnectionString());

            await Connection.OpenAsync();
            IsConnected = true;
        }
        public async Task DisconnectAsync()
        {
            // print message
            Logger.Error(ClassName);

            await Connection.CloseAsync();
            IsConnected = false;
        }

        public virtual void Insert<T>(Query<T> file, string tableName)
        {
            // print message
            Logger.Write(ClassName);
        }
        public virtual List<T> Select<T>(Query<T> file, string tableName)
        {
            // print message
            Logger.Read(ClassName);

            return default;
        }
        public virtual void Update<T>(Query<T> file, string tableName)
        {
            // print message
            Logger.Write(ClassName);
        }
        public virtual void Delete<T>(Query<T> file, string tableName)
        {
            // print message
            Logger.Write(ClassName);
        }

        public string GetConnectionString() => string.Format(@"server={0}; uid={1}; pwd={2}; database={3}", Server, Username, Password, Database);

        #endregion
    }
}

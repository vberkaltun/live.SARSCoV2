﻿using System.Threading.Tasks;
using MySql.Data.MySqlClient;

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

﻿using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace live.SARSCoV2.Database
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

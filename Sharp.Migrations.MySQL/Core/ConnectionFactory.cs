using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;

namespace Sharp.Migrations.MySQL.Core
{
    public class ConnectionFactory
    {
        public string CnnectionString { get; }

        public ConnectionFactory(string cnnString)
        {
            if (string.IsNullOrWhiteSpace(cnnString))
            {
                throw new ArgumentException($"'{nameof(cnnString)}' cannot be null or whitespace.", nameof(cnnString));
            }
            CnnectionString = cnnString;
        }

        public IDbConnection GetConnection(string connString)
        {
            var conn = new MySqlConnection(CnnectionString);
            conn.Open();
            return conn;
        }

    }
}

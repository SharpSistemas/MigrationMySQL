using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace Sharp.Migrations.MySQL.Core
{
    public class ConnectionFactory
    {
        public string ConnectionString { get; }

        public ConnectionFactory(string cnnString)
        {
            if (string.IsNullOrWhiteSpace(cnnString))
            {
                throw new ArgumentException($"'{nameof(cnnString)}' cannot be null or whitespace.", nameof(cnnString));
            }
            ConnectionString = cnnString;
        }

        public IDbConnection GetConnection()
        {
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();
            return conn;
        }

    }
}

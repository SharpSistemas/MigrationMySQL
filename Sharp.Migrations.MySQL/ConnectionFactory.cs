using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace Sharp.MySQL
{
    /// <summary>
    /// Connection factory class
    /// </summary>
    public class ConnectionFactory
    {
        /// <summary>
        /// Connection string to reach the MySQL database
        /// </summary>
        public string ConnectionString { get; }

        /// <summary>
        /// Constructor to fill the ConnectionString
        /// </summary>
        /// <param name="cnnString">Connection string to reach the MySQL database</param>
        public ConnectionFactory(string cnnString)
        {
            if (string.IsNullOrWhiteSpace(cnnString))
            {
                throw new ArgumentException($"'{nameof(cnnString)}' cannot be null or whitespace.", nameof(cnnString));
            }
            ConnectionString = cnnString;
        }

        /// <summary>
        /// Opens a connection to the database
        /// </summary>
        /// <returns>An open connection of the database</returns>
        public IDbConnection GetConnection()
        {
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();
            return conn;
        }

    }
}

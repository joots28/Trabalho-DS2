using System;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace SistemaSeguro.Api.Data
{
    public interface IDatabaseConnectionFactory
    {
        MySqlConnection CreateConnection();
        string ConnectionString { get; }
    }

    public class DatabaseConnectionFactory : IDatabaseConnectionFactory
    {
        private readonly string _connectionString;

        public DatabaseConnectionFactory(IConfiguration configuration)
        {
            string? envConn = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            if (!string.IsNullOrWhiteSpace(envConn))
            {
                _connectionString = envConn;
                return;
            }

            string? host = Environment.GetEnvironmentVariable("DB_HOST");
            if (!string.IsNullOrWhiteSpace(host))
            {
                string db = Environment.GetEnvironmentVariable("DB_NAME") ?? "db_seguradora";
                string user = Environment.GetEnvironmentVariable("DB_USER") ?? "root";
                string pass = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "root";
                string port = Environment.GetEnvironmentVariable("DB_PORT") ?? "3306";
                _connectionString = $"Server={host};Port={port};Database={db};Uid={user};Pwd={pass};";
                return;
            }

            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? "Server=localhost;Port=3306;Database=db_seguradora;Uid=root;Pwd=root;";
        }

        public string ConnectionString => _connectionString;

        public MySqlConnection CreateConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}

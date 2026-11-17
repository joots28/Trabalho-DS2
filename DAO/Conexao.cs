using System;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace SistemaSeguro.DAO
{
    public static class Conexao
    {
        private static string ObterConnectionString()
        {
            // 1. Variável de ambiente direta
            string envConn = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            if (!string.IsNullOrWhiteSpace(envConn))
            {
                return envConn;
            }

            // 2. Variáveis de ambiente compostas
            string host = Environment.GetEnvironmentVariable("DB_HOST");
            if (!string.IsNullOrWhiteSpace(host))
            {
                string db = Environment.GetEnvironmentVariable("DB_NAME") ?? "db_seguradora";
                string user = Environment.GetEnvironmentVariable("DB_USER") ?? "root";
                string pass = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "root";
                string port = Environment.GetEnvironmentVariable("DB_PORT") ?? "3306";
                return $"Server={host};Port={port};Database={db};Uid={user};Pwd={pass};";
            }

            // 3. Arquivo App.config / Web.config
            try
            {
                var configConn = ConfigurationManager.ConnectionStrings["db_seguradora"];
                if (configConn != null && !string.IsNullOrWhiteSpace(configConn.ConnectionString))
                {
                    return configConn.ConnectionString;
                }
            }
            catch
            {
                // Fallback caso ConfigurationManager falhe
            }

            // 4. Fallback padrão local
            return "Server=localhost;Database=db_seguradora;Uid=root;Pwd=root;";
        }

        public static string StringConexao => ObterConnectionString();

        public static MySqlConnection ObterConexao()
        {
            return new MySqlConnection(StringConexao);
        }

        public static bool TestarConexao(out string mensagemErro)
        {
            mensagemErro = string.Empty;
            try
            {
                using (var conn = ObterConexao())
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                mensagemErro = ex.Message;
                return false;
            }
        }
    }
}

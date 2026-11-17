using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using MySqlConnector;
using SistemaSeguro.Api.Data;
using SistemaSeguro.Api.Models;

namespace SistemaSeguro.Api.Repositories
{
    public interface IAutomovelRepository
    {
        Task<int> InserirAsync(Automovel auto);
        Task<List<Automovel>> ListarTodosAsync(int? idCliente = null);
        Task<Automovel?> ObterPorIdAsync(int id);
        Task<bool> AtualizarAsync(Automovel auto);
        Task<bool> ExcluirAsync(int id);
        Task<bool> ExisteAsync(int id);
    }

    public class AutomovelRepository : IAutomovelRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;

        public AutomovelRepository(IDatabaseConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> InserirAsync(Automovel auto)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync();

            const string sql = @"INSERT INTO automovel (placa, modelo, marca, ano, id_cliente) 
                                 VALUES (@placa, @modelo, @marca, @ano, @idCliente);
                                 SELECT LAST_INSERT_ID();";

            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@placa", auto.Placa.ToUpper().Trim());
            cmd.Parameters.AddWithValue("@modelo", auto.Modelo.Trim());
            cmd.Parameters.AddWithValue("@marca", auto.Marca.Trim());
            cmd.Parameters.AddWithValue("@ano", auto.Ano);
            cmd.Parameters.AddWithValue("@idCliente", auto.IdCliente);

            var idResult = await cmd.ExecuteScalarAsync();
            auto.IdAutomovel = Convert.ToInt32(idResult);
            return auto.IdAutomovel;
        }

        public async Task<List<Automovel>> ListarTodosAsync(int? idCliente = null)
        {
            var lista = new List<Automovel>();
            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync();

            string sql = @"SELECT a.id_automovel, a.placa, a.modelo, a.marca, a.ano, a.id_cliente, c.nome AS nome_cliente 
                           FROM automovel a
                           INNER JOIN cliente c ON a.id_cliente = c.id_cliente";

            if (idCliente.HasValue)
            {
                sql += " WHERE a.id_cliente = @idCliente";
            }
            sql += " ORDER BY a.id_automovel ASC";

            await using var cmd = new MySqlCommand(sql, conn);
            if (idCliente.HasValue)
            {
                cmd.Parameters.AddWithValue("@idCliente", idCliente.Value);
            }

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new Automovel
                {
                    IdAutomovel = reader.GetInt32("id_automovel"),
                    Placa = reader.GetString("placa"),
                    Modelo = reader.GetString("modelo"),
                    Marca = reader.GetString("marca"),
                    Ano = reader.IsDBNull(reader.GetOrdinal("ano")) ? 0 : reader.GetInt32("ano"),
                    IdCliente = reader.GetInt32("id_cliente"),
                    NomeCliente = reader.GetString("nome_cliente")
                });
            }
            return lista;
        }

        public async Task<Automovel?> ObterPorIdAsync(int id)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync();

            const string sql = @"SELECT a.id_automovel, a.placa, a.modelo, a.marca, a.ano, a.id_cliente, c.nome AS nome_cliente 
                                 FROM automovel a
                                 INNER JOIN cliente c ON a.id_cliente = c.id_cliente
                                 WHERE a.id_automovel = @id";

            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Automovel
                {
                    IdAutomovel = reader.GetInt32("id_automovel"),
                    Placa = reader.GetString("placa"),
                    Modelo = reader.GetString("modelo"),
                    Marca = reader.GetString("marca"),
                    Ano = reader.IsDBNull(reader.GetOrdinal("ano")) ? 0 : reader.GetInt32("ano"),
                    IdCliente = reader.GetInt32("id_cliente"),
                    NomeCliente = reader.GetString("nome_cliente")
                };
            }
            return null;
        }

        public async Task<bool> AtualizarAsync(Automovel auto)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync();

            const string sql = @"UPDATE automovel 
                                 SET placa = @placa, 
                                     modelo = @modelo, 
                                     marca = @marca, 
                                     ano = @ano, 
                                     id_cliente = @idCliente 
                                 WHERE id_automovel = @id";

            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@placa", auto.Placa.ToUpper().Trim());
            cmd.Parameters.AddWithValue("@modelo", auto.Modelo.Trim());
            cmd.Parameters.AddWithValue("@marca", auto.Marca.Trim());
            cmd.Parameters.AddWithValue("@ano", auto.Ano);
            cmd.Parameters.AddWithValue("@idCliente", auto.IdCliente);
            cmd.Parameters.AddWithValue("@id", auto.IdAutomovel);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync();

            const string sql = "DELETE FROM automovel WHERE id_automovel = @id";

            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> ExisteAsync(int id)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync();

            const string sql = "SELECT COUNT(1) FROM automovel WHERE id_automovel = @id";

            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            var count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            return count > 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using MySqlConnector;
using SistemaSeguro.Api.Data;
using SistemaSeguro.Api.Models;

namespace SistemaSeguro.Api.Repositories
{
    public interface IClienteRepository
    {
        Task<int> InserirAsync(Cliente cliente);
        Task<List<Cliente>> ListarTodosAsync(string? termoBusca = null);
        Task<Cliente?> ObterPorIdAsync(int id);
        Task<bool> AtualizarAsync(Cliente cliente);
        Task<bool> ExcluirAsync(int id);
        Task<bool> ExisteAsync(int id);
    }

    public class ClienteRepository : IClienteRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;

        public ClienteRepository(IDatabaseConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> InserirAsync(Cliente cliente)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync();

            const string sql = @"INSERT INTO cliente (nome, endereco, telefone, email, data_nascimento, profissao) 
                                 VALUES (@nome, @endereco, @telefone, @email, @dataNasc, @profissao);
                                 SELECT LAST_INSERT_ID();";

            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@nome", cliente.Nome);
            cmd.Parameters.AddWithValue("@endereco", cliente.Endereco);
            cmd.Parameters.AddWithValue("@telefone", (object?)cliente.Telefone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@email", (object?)cliente.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@dataNasc", cliente.DataNascimento.HasValue ? cliente.DataNascimento.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@profissao", (object?)cliente.Profissao ?? DBNull.Value);

            var idResult = await cmd.ExecuteScalarAsync();
            cliente.IdCliente = Convert.ToInt32(idResult);
            return cliente.IdCliente;
        }

        public async Task<List<Cliente>> ListarTodosAsync(string? termoBusca = null)
        {
            var lista = new List<Cliente>();
            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync();

            string sql = @"SELECT id_cliente, nome, endereco, telefone, email, data_nascimento, profissao 
                           FROM cliente";

            if (!string.IsNullOrWhiteSpace(termoBusca))
            {
                sql += " WHERE nome LIKE @termo OR email LIKE @termo OR profissao LIKE @termo";
            }
            sql += " ORDER BY id_cliente ASC";

            await using var cmd = new MySqlCommand(sql, conn);
            if (!string.IsNullOrWhiteSpace(termoBusca))
            {
                cmd.Parameters.AddWithValue("@termo", $"%{termoBusca.Trim()}%");
            }

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new Cliente
                {
                    IdCliente = reader.GetInt32("id_cliente"),
                    Nome = reader.GetString("nome"),
                    Endereco = reader.IsDBNull(reader.GetOrdinal("endereco")) ? string.Empty : reader.GetString("endereco"),
                    Telefone = reader.IsDBNull(reader.GetOrdinal("telefone")) ? null : reader.GetString("telefone"),
                    Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString("email"),
                    DataNascimento = reader.IsDBNull(reader.GetOrdinal("data_nascimento")) ? null : reader.GetDateTime("data_nascimento"),
                    Profissao = reader.IsDBNull(reader.GetOrdinal("profissao")) ? null : reader.GetString("profissao")
                });
            }
            return lista;
        }

        public async Task<Cliente?> ObterPorIdAsync(int id)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync();

            const string sql = "SELECT id_cliente, nome, endereco, telefone, email, data_nascimento, profissao FROM cliente WHERE id_cliente = @id";

            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Cliente
                {
                    IdCliente = reader.GetInt32("id_cliente"),
                    Nome = reader.GetString("nome"),
                    Endereco = reader.IsDBNull(reader.GetOrdinal("endereco")) ? string.Empty : reader.GetString("endereco"),
                    Telefone = reader.IsDBNull(reader.GetOrdinal("telefone")) ? null : reader.GetString("telefone"),
                    Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString("email"),
                    DataNascimento = reader.IsDBNull(reader.GetOrdinal("data_nascimento")) ? null : reader.GetDateTime("data_nascimento"),
                    Profissao = reader.IsDBNull(reader.GetOrdinal("profissao")) ? null : reader.GetString("profissao")
                };
            }
            return null;
        }

        public async Task<bool> AtualizarAsync(Cliente cliente)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync();

            const string sql = @"UPDATE cliente 
                                 SET nome = @nome, 
                                     endereco = @endereco, 
                                     telefone = @telefone, 
                                     email = @email, 
                                     data_nascimento = @dataNasc, 
                                     profissao = @profissao 
                                 WHERE id_cliente = @id";

            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@nome", cliente.Nome);
            cmd.Parameters.AddWithValue("@endereco", cliente.Endereco);
            cmd.Parameters.AddWithValue("@telefone", (object?)cliente.Telefone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@email", (object?)cliente.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@dataNasc", cliente.DataNascimento.HasValue ? cliente.DataNascimento.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@profissao", (object?)cliente.Profissao ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@id", cliente.IdCliente);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync();

            const string sql = "DELETE FROM cliente WHERE id_cliente = @id";

            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> ExisteAsync(int id)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync();

            const string sql = "SELECT COUNT(1) FROM cliente WHERE id_cliente = @id";

            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            var count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            return count > 0;
        }
    }
}

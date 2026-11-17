using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using MySqlConnector;
using SistemaSeguro.Api.Data;
using SistemaSeguro.Api.Models;

namespace SistemaSeguro.Api.Repositories
{
    public interface ISeguroRepository
    {
        Task<int> InserirAsync(Seguro seguro);
        Task<List<Seguro>> ListarTodosAsync(int? idCliente = null);
        Task<Seguro?> ObterPorIdAsync(int id);
        Task<bool> AtualizarAsync(Seguro seguro);
        Task<bool> ExcluirAsync(int id);
        Task<bool> ExisteAsync(int id);
    }

    public class SeguroRepository : ISeguroRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;

        public SeguroRepository(IDatabaseConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> InserirAsync(Seguro seguro)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync();

            const string sql = @"INSERT INTO seguro (numero_apolice, valor_cobertura, id_cliente, id_automovel) 
                                 VALUES (@numApolice, @valor, @idCliente, @idAutomovel);
                                 SELECT LAST_INSERT_ID();";

            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@numApolice", seguro.NumeroApolice.Trim());
            cmd.Parameters.AddWithValue("@valor", seguro.ValorCobertura);
            cmd.Parameters.AddWithValue("@idCliente", seguro.IdCliente);
            cmd.Parameters.AddWithValue("@idAutomovel", seguro.IdAutomovel);

            var idResult = await cmd.ExecuteScalarAsync();
            seguro.IdSeguro = Convert.ToInt32(idResult);
            return seguro.IdSeguro;
        }

        public async Task<List<Seguro>> ListarTodosAsync(int? idCliente = null)
        {
            var lista = new List<Seguro>();
            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync();

            string sql = @"SELECT s.id_seguro, s.numero_apolice, s.valor_cobertura, s.id_cliente, s.id_automovel, 
                                  c.nome AS nome_cliente, a.placa AS placa_automovel
                           FROM seguro s
                           INNER JOIN cliente c ON s.id_cliente = c.id_cliente
                           INNER JOIN automovel a ON s.id_automovel = a.id_automovel";

            if (idCliente.HasValue)
            {
                sql += " WHERE s.id_cliente = @idCliente";
            }
            sql += " ORDER BY s.id_seguro ASC";

            await using var cmd = new MySqlCommand(sql, conn);
            if (idCliente.HasValue)
            {
                cmd.Parameters.AddWithValue("@idCliente", idCliente.Value);
            }

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new Seguro
                {
                    IdSeguro = reader.GetInt32("id_seguro"),
                    NumeroApolice = reader.GetString("numero_apolice"),
                    ValorCobertura = reader.IsDBNull(reader.GetOrdinal("valor_cobertura")) ? 0 : reader.GetDecimal("valor_cobertura"),
                    IdCliente = reader.GetInt32("id_cliente"),
                    NomeCliente = reader.GetString("nome_cliente"),
                    IdAutomovel = reader.GetInt32("id_automovel"),
                    PlacaAutomovel = reader.GetString("placa_automovel")
                });
            }
            return lista;
        }

        public async Task<Seguro?> ObterPorIdAsync(int id)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync();

            const string sql = @"SELECT s.id_seguro, s.numero_apolice, s.valor_cobertura, s.id_cliente, s.id_automovel, 
                                        c.nome AS nome_cliente, a.placa AS placa_automovel
                                 FROM seguro s
                                 INNER JOIN cliente c ON s.id_cliente = c.id_cliente
                                 INNER JOIN automovel a ON s.id_automovel = a.id_automovel
                                 WHERE s.id_seguro = @id";

            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Seguro
                {
                    IdSeguro = reader.GetInt32("id_seguro"),
                    NumeroApolice = reader.GetString("numero_apolice"),
                    ValorCobertura = reader.IsDBNull(reader.GetOrdinal("valor_cobertura")) ? 0 : reader.GetDecimal("valor_cobertura"),
                    IdCliente = reader.GetInt32("id_cliente"),
                    NomeCliente = reader.GetString("nome_cliente"),
                    IdAutomovel = reader.GetInt32("id_automovel"),
                    PlacaAutomovel = reader.GetString("placa_automovel")
                };
            }
            return null;
        }

        public async Task<bool> AtualizarAsync(Seguro seguro)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync();

            const string sql = @"UPDATE seguro 
                                 SET numero_apolice = @numApolice, 
                                     valor_cobertura = @valor, 
                                     id_cliente = @idCliente, 
                                     id_automovel = @idAutomovel 
                                 WHERE id_seguro = @id";

            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@numApolice", seguro.NumeroApolice.Trim());
            cmd.Parameters.AddWithValue("@valor", seguro.ValorCobertura);
            cmd.Parameters.AddWithValue("@idCliente", seguro.IdCliente);
            cmd.Parameters.AddWithValue("@idAutomovel", seguro.IdAutomovel);
            cmd.Parameters.AddWithValue("@id", seguro.IdSeguro);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync();

            const string sql = "DELETE FROM seguro WHERE id_seguro = @id";

            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> ExisteAsync(int id)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync();

            const string sql = "SELECT COUNT(1) FROM seguro WHERE id_seguro = @id";

            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            var count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            return count > 0;
        }
    }
}
